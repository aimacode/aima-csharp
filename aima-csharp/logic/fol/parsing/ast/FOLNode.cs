using aima.core.logic.common;

namespace aima.core.logic.fol.parsing.ast
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public interface FOLNode : ParseTreeNode
    {
	String getSymbolicName();

	bool isCompound();

	List<FOLNode> getArgs();

	Object accept(FOLVisitor v, Object arg);

	FOLNode copy();
    }
}