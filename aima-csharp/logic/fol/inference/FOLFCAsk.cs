using aima.core.logic.fol.inference.proof;
using aima.core.logic.fol.kb;
using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.inference
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 9.3, page 332.
     * 
     * <pre>
     * function FOL-FC-ASK(KB, alpha) returns a substitution or false
     *   inputs: KB, the knowledge base, a set of first order definite clauses
     *           alpha, the query, an atomic sentence
     *   local variables: new, the new sentences inferred on each iteration
     *   
     *   repeat until new is empty
     *      new <- {}
     *      for each rule in KB do
     *          (p1 ^ ... ^ pn => q) <- STANDARDIZE-VARAIBLES(rule)
     *          for each theta such that SUBST(theta, p1 ^ ... ^ pn) = SUBST(theta, p'1 ^ ... ^ p'n)
     *                         for some p'1,...,p'n in KB
     *              q' <- SUBST(theta, q)
     *              if q' does not unify with some sentence already in KB or new then
     *                   add q' to new
     *                   theta <- UNIFY(q', alpha)
     *                   if theta is not fail then return theta
     *      add new to KB
     *   return false
     * </pre>
     * 
     * Figure 9.3 A conceptually straightforward, but very inefficient forward-chaining algo-
     * rithm. On each iteration, it adds to KB all the atomic sentences that can be inferred in one
     * step from the implication sentences and the atomic sentences already in KB. The function
     * STANDARDIZE-VARIABLES replaces all variables in its arguments with new ones that have
     * not been used before.
     */

    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class FOLFCAsk : InferenceProcedure
    {
	public FOLFCAsk()
	{

	}
	
	// START-InferenceProcedure

	/**
         * <code>
         * function FOL-FC-ASK(KB, alpha) returns a substitution or false
         *   inputs: KB, the knowledge base, a set of first order definite clauses
         *           alpha, the query, an atomic sentence
         * </code>
         */
	public InferenceResult ask(FOLKnowledgeBase KB, Sentence query)
	{
	    // Assertions on the type of queries this Inference procedure
	    // supports
	    if (!(query is AtomicSentence))
	    {
		throw new ArgumentException(
			"Only Atomic Queries are supported.");
	    }

	    FCAskAnswerHandler ansHandler = new FCAskAnswerHandler();

	    Literal alpha = new Literal((AtomicSentence)query);

	    // local variables: new, the new sentences inferred on each iteration
	    List<Literal> newSentences = new List<Literal>();

	    // Ensure query is not already a know fact before
	    // attempting forward chaining.
	    List<Dictionary<Variable, Term>> answers = KB.fetch(alpha);
	    if (answers.Count > 0)
	    {
		ansHandler.addProofStep(new ProofStepFoChAlreadyAFact(alpha));
		ansHandler.setAnswers(answers);
		return ansHandler;
	    }

	    // repeat until new is empty
	    do
	    {
		// new <- {}
		newSentences.Clear();
		// for each rule in KB do
		// (p1 ^ ... ^ pn => q) <-STANDARDIZE-VARIABLES(rule)
		foreach (Clause impl in KB.getAllDefiniteClauseImplications())
		{
		    Clause impl2 = KB.standardizeApart(impl);
		    // for each theta such that SUBST(theta, p1 ^ ... ^ pn) =
		    // SUBST(theta, p'1 ^ ... ^ p'n)
		    // --- for some p'1,...,p'n in KB
		    foreach (Dictionary<Variable, Term> theta in KB.fetch(invert(new List<Literal>(impl2
			    .getNegativeLiterals()))))
		    {
			// q' <- SUBST(theta, q)
			Literal qPrime = KB.subst(theta, impl.getPositiveLiterals()
				[0]);
			// if q' does not unify with some sentence already in KB or
			// new then do
			if (!KB.isRenaming(qPrime)
				&& !KB.isRenaming(qPrime, newSentences))
			{
			    // add q' to new
			    newSentences.Add(qPrime);
			    ansHandler.addProofStep(impl, qPrime, theta);
			    // theta <- UNIFY(q', alpha)
			    Dictionary<Variable, Term> theta2 = KB.unify(qPrime.getAtomicSentence(), alpha
				    .getAtomicSentence());
			    // if theta is not fail then return theta
			    if (null != theta2)
			    {
				foreach (Literal l in newSentences)
				{
				    Sentence s = null;
				    if (l.isPositiveLiteral())
				    {
					s = l.getAtomicSentence();
				    }
				    else
				    {
					s = new NotSentence(l.getAtomicSentence());
				    }
				    KB.tell(s);
				}
				ansHandler.setAnswers(KB.fetch(alpha));
				return ansHandler;
			    }
			}
		    }
		}
		// add new to KB
		foreach (Literal l in newSentences)
		{
		    Sentence s = null;
		    if (l.isPositiveLiteral())
		    {
			s = l.getAtomicSentence();
		    }
		    else
		    {
			s = new NotSentence(l.getAtomicSentence());
		    }
		    KB.tell(s);
		}
	    } while (newSentences.Count > 0);
	    // return false
	    return ansHandler;
	}
	// END-InferenceProcedure	
	
	// PRIVATE METHODS	
	private List<Literal> invert(List<Literal> lits)
	{
	    List<Literal> invLits = new List<Literal>();
	    foreach (Literal l in lits)
	    {
		invLits.Add(new Literal(l.getAtomicSentence(), (l
			.isPositiveLiteral() ? true : false)));
	    }
	    return invLits;
	}

	class FCAskAnswerHandler : InferenceResult
	{

	    private ProofStep stepFinal = null;
	    private List<Proof> proofs = new List<Proof>();

	    public FCAskAnswerHandler()
	    {

	    }
	    	    
	    // START-InferenceResult
	    public bool isPossiblyFalse()
	    {
		return proofs.Count == 0;
	    }

	    public bool isTrue()
	    {
		return proofs.Count > 0;
	    }

	    public bool isUnknownDueToTimeout()
	    {
		return false;
	    }

	    public bool isPartialResultDueToTimeout()
	    {
		return false;
	    }

	    public List<Proof> getProofs()
	    {
		return proofs;
	    }
	    // END-InferenceResult
	    
	    public void addProofStep(Clause implication, Literal fact,
		    Dictionary<Variable, Term> bindings)
	    {
		stepFinal = new ProofStepFoChAssertFact(implication, fact,
			bindings, stepFinal);
	    }

	    public void addProofStep(ProofStep step)
	    {
		stepFinal = step;
	    }

	    public void setAnswers(List<Dictionary<Variable, Term>> answers)
	    {
		foreach (Dictionary<Variable, Term> ans in answers)
		{
		    proofs.Add(new ProofFinal(stepFinal, ans));
		}
	    }
	}
    }
}