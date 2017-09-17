using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using aima.core.logic.fol;
using aima.core.logic.fol.parsing;
using aima.core.logic.fol.inference.proof;
using System.Collections.ObjectModel;

namespace aima.core.logic.fol.parsing.ast
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class NotSentence : Sentence
    {
	private Sentence negated;
	private List<Sentence> args = new List<Sentence>();
	private String stringRep = null;
	private int hashCode = 0;

	public NotSentence(Sentence negated)
	{
	    this.negated = negated;
	    args.Add(negated);
	}

	public Sentence getNegated()
	{
	    return negated;
	}

	// START-Sentence

	public String getSymbolicName()
	{
	    return Connectors.NOT;
	}

	public bool isCompound()
	{
	    return true;
	}

	public List<FOLNode> getArgs()
	{
	    return new ReadOnlyCollection<Sentence>(args).ToList<FOLNode>();
	}

	public Object accept(FOLVisitor v, Object arg)
	{
	    return v.visitNotSentence(this, arg);
	}

	public FOLNode copy()
	{
	    return new NotSentence((Sentence)negated.copy());
	}

        public Sentence copySentence()
        {
            return null;
        }

        // END-Sentence

	public override bool Equals(Object o)
	{

	    if (this == o)
	    {
		return true;
	    }
	    if ((o == null) || !(o is NotSentence))
	    {
		return false;
	    }
	    NotSentence ns = (NotSentence)o;
	    return (ns.negated.Equals(negated));
	}

	public override int GetHashCode()
	{
	    if (0 == hashCode)
	    {
		hashCode = 17;
		hashCode = 37 * hashCode + negated.GetHashCode();
	    }
	    return hashCode;
	}

	public override String ToString()
	{
	    if (null == stringRep)
	    {
		StringBuilder sb = new StringBuilder();
		sb.Append("NOT(");
		sb.Append(negated.ToString());
		sb.Append(")");
		stringRep = sb.ToString();
	    }
	    return stringRep;
	}
    }
}
