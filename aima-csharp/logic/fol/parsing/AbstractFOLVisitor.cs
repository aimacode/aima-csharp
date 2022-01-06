using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.parsing
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class AbstractFOLVisitor : FOLVisitor
    {
	public AbstractFOLVisitor()
	{
	}

	protected Sentence recreate(Object ast)
	{
	    return ((Sentence)ast).copySentence();
	}

	public virtual Object visitVariable(Variable variable, Object arg)
	{
	    return variable.copy();
	}

	public virtual Object visitQuantifiedSentence(QuantifiedSentence sentence,
	       Object arg)
	{
	    List<Variable> variables = new List<Variable>();
	    foreach (Variable var in sentence.getVariables())
	    {
		variables.Add((Variable)var.accept(this, arg));
	    }

	    return new QuantifiedSentence(sentence.getQuantifier(), variables,
		    (Sentence)sentence.getQuantified().accept(this, arg));
	}

	public Object visitPredicate(Predicate predicate, Object arg)
	{
	    List<Term> terms = predicate.getTerms();
	    List<Term> newTerms = new List<Term>();
	    for (int i = 0; i < terms.Count; i++)
	    {
		Term t = terms[i];
		Term subsTerm = (Term)t.accept(this, arg);
		newTerms.Add(subsTerm);
	    }
	    return new Predicate(predicate.getPredicateName(), newTerms);
	}

	public Object visitTermEquality(TermEquality equality, Object arg)
	{
	    Term newTerm1 = (Term)equality.getTerm1().accept(this, arg);
	    Term newTerm2 = (Term)equality.getTerm2().accept(this, arg);
	    return new TermEquality(newTerm1, newTerm2);
	}

	public Object visitConstant(Constant constant, Object arg)
	{
	    return constant;
	}

	public Object visitFunction(Function function, Object arg)
	{
	    List<Term> terms = function.getTerms();
	    List<Term> newTerms = new List<Term>();
	    for (int i = 0; i < terms.Count; i++)
	    {
		Term t = terms[i];
		Term subsTerm = (Term)t.accept(this, arg);
		newTerms.Add(subsTerm);
	    }
	    return new Function(function.getFunctionName(), newTerms);
	}

	public Object visitNotSentence(NotSentence sentence, Object arg)
	{
	    return new NotSentence((Sentence)sentence.getNegated().accept(this,
		    arg));
	}

	public Object visitConnectedSentence(ConnectedSentence sentence, Object arg)
	{
	    Sentence substFirst = (Sentence)sentence.getFirst().accept(this, arg);
	    Sentence substSecond = (Sentence)sentence.getSecond()
		    .accept(this, arg);
	    return new ConnectedSentence(sentence.getConnector(), substFirst,
		    substSecond);
	}
    }
}