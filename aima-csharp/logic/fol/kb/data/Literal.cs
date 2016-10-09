using System;
using System.Collections;
using System.Text;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.kb.data
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 244.<br>
     * <br>
     * A literal is either an atomic sentence (a positive literal) or a negated
     * atomic sentence (a negative literal).
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class Literal
    {
	private AtomicSentence atom = null;
	private bool negativeLiteral = false;
	private String strRep = null;
	private int hashCode = 0;

	public Literal(AtomicSentence atom)
	{
	    this.atom = atom;
	}

	public Literal(AtomicSentence atom, bool negated)
	{
	    this.atom = atom;
	    this.negativeLiteral = negated;
	}

	public virtual Literal newInstance(AtomicSentence atom)
	{
	    return new Literal(atom, negativeLiteral);
	}

	public bool isPositiveLiteral()
	{
	    return !negativeLiteral;
	}

	public bool isNegativeLiteral()
	{
	    return negativeLiteral;
	}

	public AtomicSentence getAtomicSentence()
	{
	    return atom;
	}

	public override String ToString()
	{
	    if (null == strRep)
	    {
		StringBuilder sb = new StringBuilder();
		if (isNegativeLiteral())
		{
		    sb.Append("~");
		}
		sb.Append(getAtomicSentence().ToString());
		strRep = sb.ToString();
	    }

	    return strRep;
	}

	public override bool Equals(Object o)
	{

	    if (this == o)
	    {
		return true;
	    }
	    if (o.GetType() != this.GetType())
	    {
		// This prevents ReducedLiterals
		// being treated as equivalent to
		// normal Literals.
		return false;
	    }
	    if (!(o is Literal))
	    {
		return false;
	    }
	    Literal l = (Literal)o;
	    return l.isPositiveLiteral() == isPositiveLiteral()
		    && l.getAtomicSentence().getSymbolicName().Equals(
			    atom.getSymbolicName())
		    && l.getAtomicSentence().getArgs().Equals(atom.getArgs());
	}

	public override int GetHashCode()
	{
	    if (0 == hashCode)
	    {
		hashCode = 17;
		hashCode = 37 * hashCode + this.GetType().Name.GetHashCode()
			+ (isPositiveLiteral() ? "+".GetHashCode() : "-".GetHashCode())
			+ atom.getSymbolicName().GetHashCode();
		foreach (Term t in atom.getArgs())
		{
		    hashCode = 37 * hashCode + t.GetHashCode();
		}
	    }
	    return hashCode;
	}
    }
}