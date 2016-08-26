using System;
using System.Collections.Generic;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.parsing
{
    /**
     * @author Ravi Mohan
     * 
     */
    public interface FOLVisitor
    {
	public Object visitPredicate(Predicate p, Object arg);

	public Object visitTermEquality(TermEquality equality, Object arg);

	public Object visitVariable(Variable variable, Object arg);

	public Object visitConstant(Constant constant, Object arg);

	public Object visitFunction(Function function, Object arg);

	public Object visitNotSentence(NotSentence sentence, Object arg);

	public Object visitConnectedSentence(ConnectedSentence sentence, Object arg);

	public Object visitQuantifiedSentence(QuantifiedSentence sentence,
			Object arg);
    }
}