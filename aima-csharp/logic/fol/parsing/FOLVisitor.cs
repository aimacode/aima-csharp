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
	Object visitPredicate(Predicate p, Object arg);

	Object visitTermEquality(TermEquality equality, Object arg);

	Object visitVariable(Variable variable, Object arg);

	Object visitConstant(Constant constant, Object arg);

	Object visitFunction(Function function, Object arg);

	Object visitNotSentence(NotSentence sentence, Object arg);

	Object visitConnectedSentence(ConnectedSentence sentence, Object arg);

	Object visitQuantifiedSentence(QuantifiedSentence sentence,
			Object arg);
    }
}