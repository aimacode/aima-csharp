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
    public class Function : Term
    {
	private String functionName;
	private List<Term> terms = new List<Term>();
	private String stringRep = null;
	private int hashCode = 0;

	public Function(String functionName, List<Term> terms)
	{
	    this.functionName = functionName;
	    this.terms.AddRange(terms);
	}

	public String getFunctionName()
	{
	    return functionName;
	}

	public List<Term> getTerms()
	{
	    Term[] copy = new Term[terms.Count];
	    terms.CopyTo(copy);
	    return new List<Term>(copy);
	}

	// START-Term

	public String getSymbolicName()
	{
	    return getFunctionName();
	}

	public bool isCompound()
	{
	    return true;
	}

        List<FOLNode> FOLNode.getArgs()
        {
            return null;
        }

        public List<Term> getArgs()
	{
	    return getTerms();
	}

	public Object accept(FOLVisitor v, Object arg)
	{
	    return v.visitFunction(this, arg);
	}

        FOLNode FOLNode.copy()
        {
            return copy();
        }

        public Term copy()
	{
	    List<Term> copyTerms = new List<Term>();
	    foreach (Term t in terms)
	    {
		copyTerms.Add(t.copy());
	    }
	    return new Function(functionName, copyTerms);
	}

	// END-Term

	public override bool Equals(Object o)
	{

	    if (this == o)
	    {
		return true;
	    }
	    if (!(o is Function))
	    {
		return false;
	    }

	    Function f = (Function)o;

	    return f.getFunctionName().Equals(getFunctionName())
		    && f.getTerms().Equals(getTerms());
	}

	public override int GetHashCode()
	{
	    if (0 == hashCode)
	    {
		hashCode = 17;
		hashCode = 37 * hashCode + functionName.GetHashCode();
		foreach (Term t in terms)
		{
		    hashCode = 37 * hashCode + t.GetHashCode();
		}
	    }
	    return hashCode;
	}

	public override String ToString()
	{
	    if (null == stringRep)
	    {
		StringBuilder sb = new StringBuilder();
		sb.Append(functionName);
		sb.Append("(");

		bool first = true;
		foreach (Term t in terms)
		{
		    if (first)
		    {
			first = false;
		    }
		    else
		    {
			sb.Append(",");
		    }
		    sb.Append(t.ToString());
		}

		sb.Append(")");

		stringRep = sb.ToString();
	    }
	    return stringRep;
	}
    }
}