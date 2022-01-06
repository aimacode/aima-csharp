using System.Text;
using aima.core.logic.fol.inference.otter;
using aima.core.logic.fol.inference.otter.defaultimpl;
using aima.core.logic.fol.inference.proof;
using aima.core.logic.fol.kb;
using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.inference
{
    /**
 * Artificial Intelligence A Modern Approach (2nd Edition): Figure 9.14, page
 * 307.<br>
 * <br>
 * 
 * <pre>
 * procedure OTTER(sos, usable)
 *   inputs: sos, a set of support-clauses defining the problem (a global variable)
 *   usable, background knowledge potentially relevant to the problem
 *   
 *   repeat
 *      clause <- the lightest member of sos
 *      move clause from sos to usable
 *      PROCESS(INFER(clause, usable), sos)
 *   until sos = [] or a refutation has been found
 * 
 * --------------------------------------------------------------------------------
 * 
 * function INFER(clause, usable) returns clauses
 *   
 *   resolve clause with each member of usable
 *   return the resulting clauses after applying filter
 *   
 * --------------------------------------------------------------------------------
 * 
 * procedure PROCESS(clauses, sos)
 * 
 *   for each clause in clauses do
 *       clause <- SIMPLIFY(clause)
 *       merge identical literals
 *       discard clause if it is a tautology
 *       sos <- [clause | sos]
 *       if clause has no literals then a refutation has been found
 *       if clause has one literal then look for unit refutation
 * </pre>
 * 
 * Figure 9.14 Sketch of the OTTER theorem prover. Heuristic control is applied
 * in the selection of the "lightest" clause and in the FILTER function that
 * eliminates uninteresting clauses from consideration.<br>
 * <br>
 * <b>Note:</b> The original implementation of OTTER has been retired but its
 * successor, <b>Prover9</b>, can be found at:<br>
 * <a href="http://www.prover9.org/">http://www.prover9.org/</a><br>
 * or<br>
 * <a href="http://www.cs.unm.edu/~mccune/mace4/">http://www.cs.unm.edu/~mccune/
 * mace4/</a><br>
 * Should you wish to play with a mature implementation of a theorem prover :-)<br>
 * <br>
 * For lots of interesting problems to play with, see <b>The TPTP Problem
 * Library for Automated Theorem Proving</b>:<br>
 * <a href="http://www.cs.miami.edu/~tptp/">http://www.cs.miami.edu/~tptp/</a><br>
 * 
 * @author Ciaran O'Reilly
 * 
 */
    public class FOLOTTERLikeTheoremProver : InferenceProcedure
    {
	// Ten seconds is default maximum query time permitted
	private long maxQueryTime = 10 * 1000;
	private bool useParamodulation = true;
	private LightestClauseHeuristic lightestClauseHeuristic = new DefaultLightestClauseHeuristic();
	private ClauseFilter clauseFilter = new DefaultClauseFilter();
	private ClauseSimplifier clauseSimplifier = new DefaultClauseSimplifier();
	private Paramodulation paramodulation = new Paramodulation();

	public FOLOTTERLikeTheoremProver()
	{

	}

	public FOLOTTERLikeTheoremProver(long maxQueryTime)
	{
	    setMaxQueryTime(maxQueryTime);
	}

	public FOLOTTERLikeTheoremProver(bool useParamodulation)
	{
	    setUseParamodulation(useParamodulation);
	}

	public FOLOTTERLikeTheoremProver(long maxQueryTime,
		bool useParamodulation)
	{
	    setMaxQueryTime(maxQueryTime);
	    setUseParamodulation(useParamodulation);
	}

	public long getMaxQueryTime()
	{
	    return maxQueryTime;
	}

	public void setMaxQueryTime(long maxQueryTime)
	{
	    this.maxQueryTime = maxQueryTime;
	}

	public bool isUseParamodulation()
	{
	    return useParamodulation;
	}

	public void setUseParamodulation(bool useParamodulation)
	{
	    this.useParamodulation = useParamodulation;
	}

	public LightestClauseHeuristic getLightestClauseHeuristic()
	{
	    return lightestClauseHeuristic;
	}

	public void setLightestClauseHeuristic(
		LightestClauseHeuristic lightestClauseHeuristic)
	{
	    this.lightestClauseHeuristic = lightestClauseHeuristic;
	}

	public ClauseFilter getClauseFilter()
	{
	    return clauseFilter;
	}

	public void setClauseFilter(ClauseFilter clauseFilter)
	{
	    this.clauseFilter = clauseFilter;
	}

	public ClauseSimplifier getClauseSimplifier()
	{
	    return clauseSimplifier;
	}

	public void setClauseSimplifier(ClauseSimplifier clauseSimplifier)
	{
	    this.clauseSimplifier = clauseSimplifier;
	}

	// START-InferenceProcedure
	public InferenceResult ask(FOLKnowledgeBase KB, Sentence alpha)
	{
	    List<Clause> sos = new List<Clause>();
	    List<Clause> usable = new List<Clause>();

	    // Usable set will be the set of clauses in the KB,
	    // are assuming this is satisfiable as using the
	    // Set of Support strategy.
	    foreach (Clause c in KB.getAllClauses())
	    {
		Clause c2 = KB.standardizeApart(c);
		c2.setStandardizedApartCheckNotRequired();
		usable.AddRange(c2.getFactors());
	    }

	    // Ensure reflexivity axiom is added to usable if using paramodulation.
	    if (isUseParamodulation())
	    {
		// Reflexivity Axiom: x = x
		TermEquality reflexivityAxiom = new TermEquality(new Variable("x"),
				new Variable("x"));
		Clause reflexivityClause = new Clause();
		reflexivityClause.addLiteral(new Literal(reflexivityAxiom));
		reflexivityClause = KB.standardizeApart(reflexivityClause);
		reflexivityClause.setStandardizedApartCheckNotRequired();
		usable.Add(reflexivityClause);
	    }

	    Sentence notAlpha = new NotSentence(alpha);
	    // Want to use an answer literal to pull
	    // query variables where necessary
	    Literal answerLiteral = KB.createAnswerLiteral(notAlpha);
	    List<Variable> answerLiteralVariables = KB
			    .collectAllVariables(answerLiteral.getAtomicSentence());
	    Clause answerClause = new Clause();

	    if (answerLiteralVariables.Count > 0)
	    {
		Sentence notAlphaWithAnswer = new ConnectedSentence(Connectors.OR,
				notAlpha, answerLiteral.getAtomicSentence());
		foreach (Clause c in KB.convertToClauses(notAlphaWithAnswer))
		{
		    Clause c2 = KB.standardizeApart(c);
		    c2.setProofStep(new ProofStepGoal(c2));
		    c2.setStandardizedApartCheckNotRequired();
		    sos.AddRange(c2.getFactors());
		}

		answerClause.addLiteral(answerLiteral);
	    }
	    else
	    {
		foreach (Clause c in KB.convertToClauses(notAlpha))
		{
		    Clause c2 = KB.standardizeApart(c);
		    c2.setProofStep(new ProofStepGoal(c2));
		    c2.setStandardizedApartCheckNotRequired();
		    sos.AddRange(c2.getFactors());
		}
	    }

	    // Ensure all subsumed clauses are removed
	    foreach (Clause c in SubsumptionElimination.findSubsumedClauses(usable))
	    {
		usable.Remove(c);
	    }
	    foreach (Clause c in SubsumptionElimination.findSubsumedClauses(sos))
	    {
		sos.Remove(c);
	    }

	    OTTERAnswerHandler ansHandler = new OTTERAnswerHandler(answerLiteral,
			    answerLiteralVariables, answerClause, maxQueryTime);

	    IndexedClauses idxdClauses = new IndexedClauses(
			    getLightestClauseHeuristic(), sos, usable);

	    return otter(ansHandler, idxdClauses, sos, usable);
	}

	// END-InferenceProcedure

	/**
         * <pre>
         * procedure OTTER(sos, usable) 
         *   inputs: sos, a set of support-clauses defining the problem (a global variable) 
         *   usable, background knowledge potentially relevant to the problem
         * </pre>
         */
	private InferenceResult otter(OTTERAnswerHandler ansHandler,
		IndexedClauses idxdClauses, List<Clause> sos, List<Clause> usable)
	{

	    getLightestClauseHeuristic().initialSOS(sos);

	    // * repeat
	    do
	    {
		// * clause <- the lightest member of sos
		Clause clause = getLightestClauseHeuristic().getLightestClause();
		if (null != clause)
		{
		    // * move clause from sos to usable
		    sos.Remove(clause);
		    getLightestClauseHeuristic().removedClauseFromSOS(clause);
		    usable.Add(clause);
		    // * PROCESS(INFER(clause, usable), sos)
		    process(ansHandler, idxdClauses, infer(clause, usable), sos,
			    usable);
		}
		// * until sos = [] or a refutation has been found
	    } while (sos.Count != 0 && !ansHandler.isComplete());

	    return ansHandler;
	}

	/**
         * <pre>
         * function INFER(clause, usable) returns clauses
         */
	private List<Clause> infer(Clause clause, List<Clause> usable)
	{
	    List<Clause> resultingClauses = new List<Clause>();
	    // * resolve clause with each member of usable
	    foreach (Clause c in usable)
	    {
		List<Clause> resolvents = clause.binaryResolvents(c);
		foreach (Clause rc in resolvents)
		{
		    resultingClauses.Add(rc);
		}

		// if using paramodulation to handle equality
		if (isUseParamodulation())
		{
		    List<Clause> paras = paramodulation.apply(clause, c, true);
		    foreach (Clause p in paras)
		    {
			resultingClauses.Add(p);
		    }
		}
	    }

	    // * return the resulting clauses after applying filter
	    return getClauseFilter().filter(new HashSet<Clause>(resultingClauses)).ToList();
	}

	// procedure PROCESS(clauses, sos)
	private void process(OTTERAnswerHandler ansHandler,
		IndexedClauses idxdClauses, List<Clause> clauses, List<Clause> sos,
		List<Clause> usable)
	{

	    // * for each clause in clauses do
	    foreach (Clause clause in clauses)
	    {
		// * clause <- SIMPLIFY(clause)
		Clause clause2 = getClauseSimplifier().simplify(clause);

		// * merge identical literals
		// Note: Not required as handled by Clause Implementation
		// which keeps literals within a Set, so no duplicates
		// will exist.

		// * discard clause if it is a tautology
		if (clause2.isTautology())
		{
		    continue;
		}

		// * if clause has no literals then a refutation has been found
		// or if it just contains the answer literal.
		if (!ansHandler.isAnswer(clause2))
		{
		    // * sos <- [clause | sos]
		    // This check ensure duplicate clauses are not
		    // introduced which will cause the
		    // LightestClauseHeuristic to loop continuously
		    // on the same pair of objects.
		    if (!sos.Contains(clause2) && !usable.Contains(clause2))
		    {
			foreach (Clause ac in clause2.getFactors())
			{
			    if (!sos.Contains(ac) && !usable.Contains(ac))
			    {
				idxdClauses.addClause(ac, sos, usable);

				// * if clause has one literal then look for unit
				// refutation
				lookForUnitRefutation(ansHandler, idxdClauses, ac,
					sos, usable);
			    }
			}
		    }
		}

		if (ansHandler.isComplete())
		{
		    break;
		}
	    }
	}

	private void lookForUnitRefutation(OTTERAnswerHandler ansHandler,
		IndexedClauses idxdClauses, Clause clause, List<Clause> sos,
		List<Clause> usable)
	{

	    List<Clause> toCheck = new List<Clause>();

	    if (ansHandler.isCheckForUnitRefutation(clause))
	    {
		foreach (Clause s in sos)
		{
		    if (s.isUnitClause())
		    {
			toCheck.Add(s);
		    }
		}
		foreach (Clause u in usable)
		{
		    if (u.isUnitClause())
		    {
			toCheck.Add(u);
		    }
		}
	    }

	    if (toCheck.Count > 0)
	    {
		toCheck = infer(clause, toCheck);
		foreach (Clause t in toCheck)
		{
		    // * clause <- SIMPLIFY(clause)
		    Clause t2 = getClauseSimplifier().simplify(t);

		    // * discard clause if it is a tautology
		    if (t2.isTautology())
		    {
			continue;
		    }

		    // * if clause has no literals then a refutation has been found
		    // or if it just contains the answer literal.
		    if (!ansHandler.isAnswer(t2))
		    {
			// * sos <- [clause | sos]
			// This check ensure duplicate clauses are not
			// introduced which will cause the
			// LightestClauseHeuristic to loop continuously
			// on the same pair of objects.
			if (!sos.Contains(t2) && !usable.Contains(t2))
			{
			    idxdClauses.addClause(t2, sos, usable);
			}
		    }

		    if (ansHandler.isComplete())
		    {
			break;
		    }
		}
	    }
	}

	// This is a simple indexing on the clauses to support
	// more efficient forward and backward subsumption testing.
	class IndexedClauses
	{
	    private LightestClauseHeuristic lightestClauseHeuristic = null;
	    // Group the clauses by their # of literals.
	    private Dictionary<int, List<Clause>> clausesGroupedBySize = new Dictionary<int, List<Clause>>();
	    // Keep track of the min and max # of literals.
	    private int minNoLiterals = int.MaxValue;
	    private int maxNoLiterals = 0;

	    public IndexedClauses(LightestClauseHeuristic lightestClauseHeuristic,
		    List<Clause> sos, List<Clause> usable)
	    {
		this.lightestClauseHeuristic = lightestClauseHeuristic;
		foreach (Clause c in sos)
		{
		    indexClause(c);
		}
		foreach (Clause c in usable)
		{
		    indexClause(c);
		}
	    }

	    public void addClause(Clause c, List<Clause> sos, List<Clause> usable)
	    {
		// Perform forward subsumption elimination
		bool addToSOS = true;
		for (int i = minNoLiterals; i < c.getNumberLiterals(); i++)
		{
		    List<Clause> fs = clausesGroupedBySize[i];
		    if (null != fs)
		    {
			foreach (Clause s in fs)
			{
			    if (s.subsumes(c))
			    {
				addToSOS = false;
				break;
			    }
			}
		    }
		    if (!addToSOS)
		    {
			break;
		    }
		}

		if (addToSOS)
		{
		    sos.Add(c);
		    lightestClauseHeuristic.addedClauseToSOS(c);
		    indexClause(c);
		    // Have added clause, therefore
		    // perform backward subsumption elimination
		    List<Clause> subsumed = new List<Clause>();
		    for (int i = c.getNumberLiterals() + 1; i <= maxNoLiterals; i++)
		    {
			subsumed.Clear();
			List<Clause> bs = clausesGroupedBySize[i];
			if (null != bs)
			{
			    foreach (Clause s in bs)
			    {
				if (c.subsumes(s))
				{
				    subsumed.Add(s);
				    if (sos.Contains(s))
				    {
					sos.Remove(s);
					lightestClauseHeuristic
						.removedClauseFromSOS(s);
				    }
				    usable.Remove(s);
				}
			    }
			    foreach (Clause c2 in subsumed)
			    {
				bs.Remove(c2);
			    }
			}
		    }
		}
	    }

	    // PRIVATE METHODS

	    private void indexClause(Clause c)
	    {
		int size = c.getNumberLiterals();
		if (size < minNoLiterals)
		{
		    minNoLiterals = size;
		}
		if (size > maxNoLiterals)
		{
		    maxNoLiterals = size;
		}
		List<Clause> cforsize = null;
		if (!clausesGroupedBySize.ContainsKey(size))
		{
		    cforsize = new List<Clause>();
		    clausesGroupedBySize.Add(size, cforsize);
		}
		else
		{
		    cforsize = clausesGroupedBySize[size];
		}
		cforsize.Add(c);
	    }
	}

	class OTTERAnswerHandler : InferenceResult
	{
	    private Literal answerLiteral = null;
	    private List<Variable> answerLiteralVariables = null;
	    private Clause answerClause = null;
	    private long finishTime = 0L;
	    private bool complete = false;
	    private List<Proof> proofs = new List<Proof>();
	    private bool timedOut = false;

	    public OTTERAnswerHandler(Literal answerLiteral,
		    List<Variable> answerLiteralVariables, Clause answerClause,
		    long maxQueryTime)
	    {
		this.answerLiteral = answerLiteral;
		this.answerLiteralVariables = answerLiteralVariables;
		this.answerClause = answerClause;
		//
		this.finishTime = DateTime.UtcNow.Ticks + maxQueryTime;
	    }

	    // START-InferenceResult
	    public bool isPossiblyFalse()
	    {
		return !timedOut && proofs.Count == 0;
	    }

	    public bool isTrue()
	    {
		return proofs.Count > 0;
	    }

	    public bool isUnknownDueToTimeout()
	    {
		return timedOut && proofs.Count == 0;
	    }

	    public bool isPartialResultDueToTimeout()
	    {
		return timedOut && proofs.Count > 0;
	    }

	    public List<Proof> getProofs()
	    {
		return proofs;
	    }

	    // END-InferenceResult

	    public bool isComplete()
	    {
		return complete;
	    }

	    public bool isLookingForAnswerLiteral()
	    {
		return !answerClause.isEmpty();
	    }

	    public bool isCheckForUnitRefutation(Clause clause)
	    {

		if (isLookingForAnswerLiteral())
		{
		    if (2 == clause.getNumberLiterals())
		    {
			foreach (Literal t in clause.getLiterals())
			{
			    if (t.getAtomicSentence().getSymbolicName().Equals(
				    answerLiteral.getAtomicSentence()
					    .getSymbolicName()))
			    {
				return true;
			    }
			}
		    }
		}
		else
		{
		    return clause.isUnitClause();
		}

		return false;
	    }

	    public bool isAnswer(Clause aClause)
	    {
		bool isAns = false;

		if (answerClause.isEmpty())
		{
		    if (aClause.isEmpty())
		    {
			proofs.Add(new ProofFinal(aClause.getProofStep(),
				new Dictionary<Variable, Term>()));
			complete = true;
			isAns = true;
		    }
		}
		else
		{
		    if (aClause.isEmpty())
		    {
			// This should not happen
			// as added an answer literal to sos, which
			// implies the database (i.e. premises) are
			// unsatisfiable to begin with.
			throw new ApplicationException(
				"Generated an empty clause while looking for an answer, implies original KB or usable is unsatisfiable");
		    }

		    if (aClause.isUnitClause()
			    && aClause.isDefiniteClause()
			    && aClause.getPositiveLiterals()[0]
				    .getAtomicSentence().getSymbolicName().Equals(
					    answerLiteral.getAtomicSentence()
						    .getSymbolicName()))
		    {
			Dictionary<Variable, Term> answerBindings = new Dictionary<Variable, Term>();
			List<Term> answerTerms = aClause.getPositiveLiterals()[
				0].getAtomicSentence().getArgs();
			int idx = 0;
			foreach (Variable v in answerLiteralVariables)
			{
			    answerBindings.Add(v, (Term)answerTerms[idx]);
			    idx++;
			}
			bool addNewAnswer = true;
			foreach (Proof p in proofs)
			{
			    if (p.getAnswerBindings().Equals(answerBindings))
			    {
				addNewAnswer = false;
				break;
			    }
			}
			if (addNewAnswer)
			{
			    proofs.Add(new ProofFinal(aClause.getProofStep(),
				    answerBindings));
			}
			isAns = true;
		    }
		}

		if (DateTime.UtcNow.Ticks > finishTime)
		{
		    complete = true;
		    // Indicate that I have run out of query time
		    timedOut = true;
		}

		return isAns;
	    }

	    public override String ToString()
	    {
		StringBuilder sb = new StringBuilder();
		sb.Append("isComplete=" + complete);
		sb.Append("\n");
		sb.Append("result=" + proofs);
		return sb.ToString();
	    }
	}
    }
}