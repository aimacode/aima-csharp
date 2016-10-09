using System;
using System.Collections.Generic;
using aima.core.logic.fol.parsing;
using aima.core.logic.fol.parsing.ast;
using aima.core.logic.fol.kb.data;

namespace aima.core.logic.fol
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class VariableCollector : FOLVisitor
    {
	public VariableCollector()
	{

	}


	// Note: The set guarantees the order in which they were
	// found.
	public List<Variable> collectAllVariables(Sentence sentence)
	{
	    List<Variable> variables = new List<Variable>();

	    sentence.accept(this, variables);

	    return variables;
	}

	public List<Variable> collectAllVariables(Term aTerm)
	{
	    List<Variable> variables = new List<Variable>();

	    aTerm.accept(this, variables);

	    return variables;
	}

	public List<Variable> collectAllVariables(Clause aClause)
	{
	    List<Variable> variables = new List<Variable>();

	    foreach (Literal l in aClause.getLiterals())
	    {
		l.getAtomicSentence().accept(this, variables);
	    }

	    return variables;
	}

	public List<Variable> collectAllVariables(Chain aChain)
	{
	    List<Variable> variables = new List<Variable>();

	    foreach (Literal l in aChain.getLiterals())
	    {
		l.getAtomicSentence().accept(this, variables);
	    }

	    return variables;
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