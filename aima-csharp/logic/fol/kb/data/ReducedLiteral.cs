using System.Text;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.kb.data
{
    /**
     * @see <a
     *      href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture13.pdf"
     *      >Reduced Literal</a>
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class ReducedLiteral : Literal
    {
	private String strRep = null;

	public ReducedLiteral(AtomicSentence atom) : base(atom)
	{

	}

	public ReducedLiteral(AtomicSentence atom, bool negated) : base(atom, negated)
	{

	}

	public override Literal newInstance(AtomicSentence atom)
	{
	    return new ReducedLiteral(atom, isNegativeLiteral());
	}

	public override String ToString()
	{
	    if (null == strRep)
	    {
		StringBuilder sb = new StringBuilder();
		sb.Append("[");
		if (isNegativeLiteral())
		{
		    sb.Append("~");
		}
		sb.Append(getAtomicSentence().ToString());
		sb.Append("]");
		strRep = sb.ToString();
	    }

	    return strRep;
	}
    }
}