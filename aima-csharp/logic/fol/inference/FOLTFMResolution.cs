using System;
using System.Collections.Generic;
using System.Text;
using aima.core.logic.fol;
using aima.core.logic.fol.inference.proof;
using aima.core.logic.fol.inference.trace;
using aima.core.logic.fol.kb;
using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.inference
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 347.<br>
     * <br>
     * The algorithmic approach is identical to the propositional case, described in
     * Figure 7.12.<br>
     * <br>
     * However, this implementation will use the T)wo F)inger M)ethod for looking
     * for resolvents between clauses, which is very inefficient.<br>
     * <br>
     * see:<br>
     * <a
     * href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture04.pdf">
     * http://logic.stanford.edu/classes/cs157/2008/lectures/lecture04.pdf</a>,
     * slide 21 for the propositional case. In addition, an Answer literal will be
     * used so that queries with Variables may be answered (see pg. 350 of AIMA3e).
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class FOLTFMResolution : InferenceProcedure
    {
	private long maxQueryTime = 10 * 1000;

	private FOLTFMResolutionTracer tracer = null;

	public FOLTFMResolution()
	{

	}

	public FOLTFMResolution(long maxQueryTime)
	{
	    setMaxQueryTime(maxQueryTime);
	}

	public FOLTFMResolution(FOLTFMResolutionTracer tracer)
	{
	    setTracer(tracer);
	}

	public long getMaxQueryTime()
	{
	    return maxQueryTime;
	}

	public void setMaxQueryTime(long maxQueryTime)
	{
	    this.maxQueryTime = maxQueryTime;
	}

	public FOLTFMResolutionTracer getTracer()
	{
	    return tracer;
	}

	public void setTracer(FOLTFMResolutionTracer tracer)
	{
	    this.tracer = tracer;
	}
	
	// START-InferenceProcedure
	public InferenceResult ask(FOLKnowledgeBase KB, Sentence alpha)
	{

	    // clauses <- the set of clauses in CNF representation of KB ^ ~alpha
	    List<Clause> clauses = new List<Clause>();
	    foreach (Clause c in KB.getAllClauses())
	    {
		Clause c2 = KB.standardizeApart(c);
		c2.setStandardizedApartCheckNotRequired();
		clauses.AddRange(c2.getFactors());
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
		    clauses.AddRange(c2.getFactors());
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
		    clauses.AddRange(c2.getFactors());
		}
	    }

	    TFMAnswerHandler ansHandler = new TFMAnswerHandler(answerLiteral,
		    answerLiteralVariables, answerClause, maxQueryTime);

	    // new <- {}
	    List<Clause> newClauses = new List<Clause>();
	    List<Clause> toAdd = new List<Clause>();
	    // loop do
	    int noOfPrevClauses = clauses.Count;
	    do
	    {
		if (null != tracer)
		{
		    tracer.stepStartWhile(clauses, clauses.Count, newClauses
			    .Count);
		}

		newClauses.Clear();

		// for each Ci, Cj in clauses do
		Clause[] clausesA = new Clause[clauses.Count];
		clausesA = clauses.ToArray();
		// Basically, using the simple T)wo F)inger M)ethod here.
		for (int i = 0; i < clausesA.Length; i++)
		{
		    Clause cI = clausesA[i];
		    if (null != tracer)
		    {
			tracer.stepOuterFor(cI);
		    }
		    for (int j = i; j < clausesA.Length; j++)
		    {
			Clause cJ = clausesA[j];

			if (null != tracer)
			{
			    tracer.stepInnerFor(cI, cJ);
			}

			// resolvent <- FOL-RESOLVE(Ci, Cj)
			List<Clause> resolvents = cI.binaryResolvents(cJ);

			if (resolvents.Count > 0)
			{
			    toAdd.Clear();
			    // new <- new <UNION> resolvent
			    foreach (Clause rc in resolvents)
			    {
				toAdd.AddRange(rc.getFactors());
			    }

			    if (null != tracer)
			    {
				tracer.stepResolved(cI, cJ, toAdd);
			    }

			    ansHandler.checkForPossibleAnswers(toAdd);

			    if (ansHandler.isComplete())
			    {
				break;
			    }

			    newClauses.AddRange(toAdd);
			}

			if (ansHandler.isComplete())
			{
			    break;
			}
		    }
		    if (ansHandler.isComplete())
		    {
			break;
		    }
		}

		noOfPrevClauses = clauses.Count;

		// clauses <- clauses <UNION> new
		clauses.AddRange(newClauses);

		if (ansHandler.isComplete())
		{
		    break;
		}

		// if new is a <SUBSET> of clauses then finished
		// searching for an answer
		// (i.e. when they were added the # clauses
		// did not increase).
	    } while (noOfPrevClauses < clauses.Count);

	    if (null != tracer)
	    {
		tracer.stepFinished(clauses, ansHandler);
	    }

	    return ansHandler;
	}

	// END-InferenceProcedure
	// 

	//
	// PRIVATE METHODS
	//
	class TFMAnswerHandler : InferenceResult
	{
	    private Literal answerLiteral = null;
	    private List<Variable> answerLiteralVariables = null;
	    private Clause answerClause = null;
	    private long finishTime = 0L;
	    private bool complete = false;
	    private List<Proof> proofs = new List<Proof>();
	    private bool timedOut = false;

	    public TFMAnswerHandler(Literal answerLiteral,
		    List<Variable> answerLiteralVariables, Clause answerClause,
		    long maxQueryTime)
	    {
		this.answerLiteral = answerLiteral;
		this.answerLiteralVariables = answerLiteralVariables;
		this.answerClause = answerClause;
		//
		this.finishTime = DateTime.UtcNow.Ticks + maxQueryTime;
	    }

	    //
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

	    public void checkForPossibleAnswers(List<Clause> resolvents)
	    {
		// If no bindings being looked for, then
		// is just a true false query.
		foreach (Clause aClause in resolvents)
		{
		    if (answerClause.isEmpty())
		    {
			if (aClause.isEmpty())
			{
			    proofs.Add(new ProofFinal(aClause.getProofStep(),
				    new Dictionary<Variable, Term>()));
			    complete = true;
			}
		    }
		    else
		    {
			if (aClause.isEmpty())
			{
			    // This should not happen
			    // as added an answer literal, which
			    // implies the database (i.e. premises) are
			    // unsatisfiable to begin with.
			    throw new ApplicationException(
				    "Generated an empty clause while looking for an answer, implies original KB is unsatisfiable");
			}

			if (aClause.isUnitClause()
				&& aClause.isDefiniteClause()
				&& aClause.getPositiveLiterals()[0]
					.getAtomicSentence().getSymbolicName()
					.Equals(
						answerLiteral.getAtomicSentence()
							.getSymbolicName()))
			{
			    Dictionary<Variable, Term> answerBindings = new Dictionary<Variable, Term>();
			    List<FOLNode> answerTerms = aClause.getPositiveLiterals()
				    [0].getAtomicSentence().getArgs();
			    int idx = 0;
			    foreach (Variable v in answerLiteralVariables)
			    {
				answerBindings.Add(v, (Term)answerTerms[idx]);
				idx++;
			    }
			    bool addNewAnswer = true;
			    foreach (Proof.Proof p in proofs)
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
			}
		    }

		    if (DateTime.UtcNow.Ticks > finishTime)
		    {
			complete = true;
			// Indicate that I have run out of query time
			timedOut = true;
		    }
		}
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
