using aima.core.logic.fol.parsing;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.inference
{
    /**
     * Abstract base class for Demodulation and Paramodulation algorithms.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public abstract class AbstractModulation
    {
	// PROTECTED ATTRIBUTES
	protected VariableCollector variableCollector = new VariableCollector();
	protected Unifier unifier = new Unifier();
	protected SubstVisitor substVisitor = new SubstVisitor();

	// PROTECTED METODS	
	public abstract bool isValidMatch(Term toMatch,
		List<Variable> toMatchVariables, Term possibleMatch,
		Dictionary<Variable, Term> substitution);

	protected IdentifyCandidateMatchingTerm getMatchingSubstitution(
		Term toMatch, AtomicSentence expression)
	{

	    IdentifyCandidateMatchingTerm icm = new IdentifyCandidateMatchingTerm(
		    toMatch, expression, this);

	    if (icm.isMatch())
	    {
		return icm;
	    }
	    // indicates no match
	    return null;
	}

	protected class IdentifyCandidateMatchingTerm : FOLVisitor
	{
	    private Term toMatch = null;
	    private List<Variable> toMatchVariables = null;
	    private Term matchingTerm = null;
	    private Dictionary<Variable, Term> substitution = null;
	    private AbstractModulation abstractModulation;

	    public IdentifyCandidateMatchingTerm(Term toMatch,
		    AtomicSentence expression, AbstractModulation abstractModulation)
	    {
		this.abstractModulation = abstractModulation;
		this.toMatch = toMatch;
		this.toMatchVariables = abstractModulation.variableCollector
			.collectAllVariables(toMatch);

		expression.accept(this, null);
	    }

	    public bool isMatch()
	    {
		return null != matchingTerm;
	    }

	    public Term getMatchingTerm()
	    {
		return matchingTerm;
	    }

	    public Dictionary<Variable, Term> getMatchingSubstitution()
	    {
		return substitution;
	    }
	    
	    // START-FOLVisitor
	    public Object visitPredicate(Predicate p, Object arg)
	    {
		foreach (Term t in p.getArgs())
		{
		    // Finish processing if have found a match
		    if (null != matchingTerm)
		    {
			break;
		    }
		    t.accept(this, null);
		}
		return p;
	    }

	    public Object visitTermEquality(TermEquality equality, Object arg)
	    {
		foreach (Term t in equality.getArgs())
		{
		    // Finish processing if have found a match
		    if (null != matchingTerm)
		    {
			break;
		    }
		    t.accept(this, null);
		}
		return equality;
	    }

	    public Object visitVariable(Variable variable, Object arg)
	    {

		if (null != (substitution = abstractModulation.unifier.unify(toMatch, variable)))
		{
		    if (abstractModulation.isValidMatch(toMatch, toMatchVariables, variable,
			    substitution))
		    {
			matchingTerm = variable;
		    }
		}
		return variable;
	    }

	    public Object visitConstant(Constant constant, Object arg)
	    {
		if (null != (substitution = abstractModulation.unifier.unify(toMatch, constant)))
		{
		    if (abstractModulation.isValidMatch(toMatch, toMatchVariables, constant,
			    substitution))
		    {
			matchingTerm = constant;
		    }
		}
		return constant;
	    }

	    public Object visitFunction(Function function, Object arg)
	    {
		if (null != (substitution = abstractModulation.unifier.unify(toMatch, function)))
		{
		    if (abstractModulation.isValidMatch(toMatch, toMatchVariables, function,
			    substitution))
		    {
			matchingTerm = function;
		    }
		}

		if (null == matchingTerm)
		{
		    // Try the Function's arguments
		    foreach (Term t in function.getArgs())
		    {
			// Finish processing if have found a match
			if (null != matchingTerm)
			{
			    break;
			}
			t.accept(this, null);
		    }
		}
		return function;
	    }

	    public Object visitNotSentence(NotSentence sentence, Object arg)
	    {
		throw new NotImplementedException(
			"visitNotSentence() should not be called.");
	    }

	    public Object visitConnectedSentence(ConnectedSentence sentence,
		    Object arg)
	    {
		throw new NotImplementedException(
			"visitConnectedSentence() should not be called.");
	    }

	    public Object visitQuantifiedSentence(QuantifiedSentence sentence,
		    Object arg)
	    {
		throw new NotImplementedException(
			"visitQuantifiedSentence() should not be called.");
	    }
	    // END-FOLVisitor
	}

	protected class ReplaceMatchingTerm : FOLVisitor
	{
	    private Term toReplace = null;
	    private Term replaceWith = null;
	    private bool replaced = false;

	    public ReplaceMatchingTerm()
	    {
	    }

	    public AtomicSentence replace(AtomicSentence expression,
		    Term toReplace, Term replaceWith)
	    {
		this.toReplace = toReplace;
		this.replaceWith = replaceWith;

		return (AtomicSentence)expression.accept(this, null);
	    }

	    //
	    // START-FOLVisitor
	    public Object visitPredicate(Predicate p, Object arg)
	    {
		List<Term> newTerms = new List<Term>();
		foreach (Term t in p.getTerms())
		{
		    Term subsTerm = (Term)t.accept(this, arg);
		    newTerms.Add(subsTerm);
		}
		return new Predicate(p.getPredicateName(), newTerms);
	    }

	    public Object visitTermEquality(TermEquality equality, Object arg)
	    {
		Term newTerm1 = (Term)equality.getTerm1().accept(this, arg);
		Term newTerm2 = (Term)equality.getTerm2().accept(this, arg);
		return new TermEquality(newTerm1, newTerm2);
	    }

	    public Object visitVariable(Variable variable, Object arg)
	    {
		if (!replaced)
		{
		    if (toReplace.Equals(variable))
		    {
			replaced = true;
			return replaceWith;
		    }
		}
		return variable;
	    }

	    public Object visitConstant(Constant constant, Object arg)
	    {
		if (!replaced)
		{
		    if (toReplace.Equals(constant))
		    {
			replaced = true;
			return replaceWith;
		    }
		}
		return constant;
	    }

	    public Object visitFunction(Function function, Object arg)
	    {
		if (!replaced)
		{
		    if (toReplace.Equals(function))
		    {
			replaced = true;
			return replaceWith;
		    }
		}

		List<Term> newTerms = new List<Term>();
		foreach (Term t in function.getTerms())
		{
		    Term subsTerm = (Term)t.accept(this, arg);
		    newTerms.Add(subsTerm);
		}
		return new Function(function.getFunctionName(), newTerms);
	    }

	    public Object visitNotSentence(NotSentence sentence, Object arg)
	    {
		throw new NotImplementedException(
			"visitNotSentence() should not be called.");
	    }

	    public Object visitConnectedSentence(ConnectedSentence sentence,
		    Object arg)
	    {
		throw new NotImplementedException(
			"visitConnectedSentence() should not be called.");
	    }

	    public Object visitQuantifiedSentence(QuantifiedSentence sentence,
		    Object arg)
	    {
		throw new NotImplementedException(
			"visitQuantifiedSentence() should not be called.");
	    }
	    // END-FOLVisitor    
	}
    }
}