using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class StandardizeApartResult
    {
	private Sentence originalSentence = null;
	private Sentence standardized = null;
	private Dictionary<Variable, Term> forwardSubstitution = null;
	private Dictionary<Variable, Term> reverseSubstitution = null;

	public StandardizeApartResult(Sentence originalSentence,
		Sentence standardized, Dictionary<Variable, Term> forwardSubstitution,
		Dictionary<Variable, Term> reverseSubstitution)
	{
	    this.originalSentence = originalSentence;
	    this.standardized = standardized;
	    this.forwardSubstitution = forwardSubstitution;
	    this.reverseSubstitution = reverseSubstitution;
	}

	public Sentence getOriginalSentence()
	{
	    return originalSentence;
	}

	public Sentence getStandardized()
	{
	    return standardized;
	}

	public Dictionary<Variable, Term> getForwardSubstitution()
	{
	    return forwardSubstitution;
	}

	public Dictionary<Variable, Term> getReverseSubstitution()
	{
	    return reverseSubstitution;
	}
    }
}