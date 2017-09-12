using System;
using System.Collections.Generic;
using System.Text;
using aima.core.logic.fol.parsing;

namespace aima.core.logic.fol.parsing.ast
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class ConnectedSentence : Sentence
    {
	private String connector;
	private Sentence first, second;
	private List<Sentence> args = new List<Sentence>();
	private String stringRep = null;
	private int hashCode = 0;

	public ConnectedSentence(String connector, Sentence first, Sentence second)
	{
	    this.connector = connector;
	    this.first = first;
	    this.second = second;
	    args.Add(first);
	    args.Add(second);
	}

	public String getConnector()
	{
	    return connector;
	}

	public Sentence getFirst()
	{
	    return first;
	}

	public Sentence getSecond()
	{
	    return second;
	}

	// START-Sentence

	public String getSymbolicName()
	{
	    return getConnector();
	}

	public bool isCompound()
	{
	    return true;
	}

	public List<FOLNode> getArgs()
	{
	    Sentence[] copy = new Sentence[args.Count];
	    args.CopyTo(copy);
	    return new List<FOLNode>(copy);
	}

	public Object accept(FOLVisitor v, Object arg)
	{
	    return v.visitConnectedSentence(this, arg);
	}

        public FOLNode copy()
        {
            return null;
        }

        public Sentence copySentence()
	{
	    return new ConnectedSentence(connector, first.copySentence(), second.copySentence());
	}

	// END-Sentence

	public override bool Equals(Object o)
	{

	    if (this == o)
	    {
		return true;
	    }
	    if ((o == null) || !(o is ConnectedSentence))
	    {
		return false;
	    }
	    ConnectedSentence cs = (ConnectedSentence)o;
	    return cs.getConnector().Equals(getConnector())
		    && cs.getFirst().Equals(getFirst())
		    && cs.getSecond().Equals(getSecond());
	}

	public override int GetHashCode()
	{
	    if (0 == hashCode)
	    {
		hashCode = 17;
		hashCode = 37 * hashCode + getConnector().GetHashCode();
		hashCode = 37 * hashCode + getFirst().GetHashCode();
		hashCode = 37 * hashCode + getSecond().GetHashCode();
	    }
	    return hashCode;
	}

	public override String ToString()
	{
	    if (null == stringRep)
	    {
		StringBuilder sb = new StringBuilder();
		sb.Append("(");
		sb.Append(first.ToString());
		sb.Append(" ");
		sb.Append(connector);
		sb.Append(" ");
		sb.Append(second.ToString());
		sb.Append(")");
		stringRep = sb.ToString();
	    }
	    return stringRep;
	}
    }
}