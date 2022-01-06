using aima.core.logic.fol.parsing;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class PredicateCollector : FOLVisitor
    {
	public PredicateCollector()
	{

	}

	public List<Predicate> getPredicates(Sentence s)
	{
	    return (List<Predicate>)s.accept(this, new List<Predicate>());
	}

	public Object visitPredicate(Predicate p, Object arg)
	{
	    List<Predicate> predicates = (List<Predicate>)arg;
	    predicates.Add(p);
	    return predicates;
	}

	public Object visitTermEquality(TermEquality equality, Object arg)
	{
	    return arg;
	}

	public Object visitVariable(Variable variable, Object arg)
	{
	    return arg;
	}

	public Object visitConstant(Constant constant, Object arg)
	{
	    return arg;
	}

	public Object visitFunction(Function function, Object arg)
	{
	    return arg;
	}

	public Object visitNotSentence(NotSentence sentence, Object arg)
	{
	    sentence.getNegated().accept(this, arg);
	    return arg;
	}

	public Object visitConnectedSentence(ConnectedSentence sentence, Object arg)
	{
	    sentence.getFirst().accept(this, arg);
	    sentence.getSecond().accept(this, arg);
	    return arg;
	}

	public Object visitQuantifiedSentence(QuantifiedSentence sentence,
		Object arg)
	{
	    sentence.getQuantified().accept(this, arg);
	    return arg;
	}
    }
}
