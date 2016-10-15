using System;
using System.Collections.Generic;
using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class StandardizeApartInPlace
    {
	private static CollectAllVariables _collectAllVariables = new CollectAllVariables();

	public static int standardizeApart(Chain c, int saIdx)
	{
	    List<Variable> variables = new List<Variable>();
	    foreach (Literal l in c.getLiterals())
	    {
		collectAllVariables(l.getAtomicSentence(), variables);
	    }
	    return standardizeApart(variables, c, saIdx);
	}

	public static int standardizeApart(Clause c, int saIdx)
	{
	    List<Variable> variables = new List<Variable>();
	    foreach (Literal l in c.getLiterals())
	    {
		collectAllVariables(l.getAtomicSentence(), variables);
	    }

	    return standardizeApart(variables, c, saIdx);
	}

	// PRIVATE METHODS

	private static int standardizeApart(List<Variable> variables, Object expr,
		int saIdx)
	{
	    Dictionary<String, int> indexicals = new Dictionary<String, int>();
	    foreach (Variable v in variables)
	    {
		if (!indexicals.ContainsKey(v.getIndexedValue()))
		{
		    indexicals.Add(v.getIndexedValue(), saIdx++);
		}
	    }
	    foreach (Variable v in variables)
	    {
		int i = indexicals[v.getIndexedValue()];
		if (null == i)
		{
		    throw new ApplicationException("ERROR: duplicate var=" + v
			    + ", expr=" + expr);
		}
		else
		{
		    v.setIndexical(i);
		}
	    }
	    return saIdx;
	}

	private static void collectAllVariables(Sentence s, List<Variable> vars)
	{
	    s.accept(_collectAllVariables, vars);
	}
    }

    class CollectAllVariables : FOLVisitor
    {
	public CollectAllVariables()
	{

	}

	public Object visitVariable(Variable var, Object arg)
	{
	    List<Variable> variables = (List<Variable>)arg;
	    variables.Add(var);
	    return var;
	}

	public Object visitQuantifiedSentence(QuantifiedSentence sentence,
		Object arg)
	{
	    // Ensure I collect quantified variables too
	    List<Variable> variables = (List<Variable>)arg;
	    variables.AddRange(sentence.getVariables());

	    sentence.getQuantified().accept(this, arg);

	    return sentence;
	}

	public Object visitPredicate(Predicate predicate, Object arg)
	{
	    foreach (Term t in predicate.getTerms())
	    {
		t.accept(this, arg);
	    }
	    return predicate;
	}

	public Object visitTermEquality(TermEquality equality, Object arg)
	{
	    equality.getTerm1().accept(this, arg);
	    equality.getTerm2().accept(this, arg);
	    return equality;
	}

	public Object visitConstant(Constant constant, Object arg)
	{
	    return constant;
	}

	public Object visitFunction(Function function, Object arg)
	{
	    foreach (Term t in function.getTerms())
	    {
		t.accept(this, arg);
	    }
	    return function;
	}

	public Object visitNotSentence(NotSentence sentence, Object arg)
	{
	    sentence.getNegated().accept(this, arg);
	    return sentence;
	}

	public Object visitConnectedSentence(ConnectedSentence sentence, Object arg)
	{
	    sentence.getFirst().accept(this, arg);
	    sentence.getSecond().accept(this, arg);
	    return sentence;
	}
    }
}