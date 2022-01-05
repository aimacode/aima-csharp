using System.Text;
using System.Collections.ObjectModel;

namespace aima.core.logic.fol.parsing.ast
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class QuantifiedSentence : Sentence
    {
	private String quantifier;
	private List<Variable> variables = new List<Variable>();
	private Sentence quantified;
	private List<FOLNode> args = new List<FOLNode>();
	private String stringRep = null;
	private int hashCode = 0;

	public QuantifiedSentence(String quantifier, List<Variable> variables,
	       Sentence quantified)
	{
	    this.quantifier = quantifier;
	    this.variables.AddRange(variables);
	    this.quantified = quantified;
	    this.args.AddRange(variables);
	    this.args.Add(quantified);
	}

	public String getQuantifier()
	{
	    return quantifier;
	}

	public List<Variable> getVariables()
	{
	    return new ReadOnlyCollection<Variable>(variables).ToList<Variable>();
	}

	public Sentence getQuantified()
	{
	    return quantified;
	}

	// START-Sentence

	public String getSymbolicName()
	{
	    return getQuantifier();
	}

	public bool isCompound()
	{
	    return true;
	}

	public List<FOLNode> getArgs()
	{
	    return new ReadOnlyCollection<FOLNode>(args).ToList<FOLNode>();
	}

	public Object accept(FOLVisitor v, Object arg)
	{
	    return v.visitQuantifiedSentence(this, arg);
	}

	public FOLNode copy()
	{
	    List<Variable> copyVars = new List<Variable>();
	    foreach (Variable v in variables)
	    {
		copyVars.Add((Variable)v.copy());
	    }
	    return new QuantifiedSentence(quantifier, copyVars, quantified.copySentence());
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
	    if ((o == null) || !(o is QuantifiedSentence))
	    {
		return false;
	    }
	    QuantifiedSentence cs = (QuantifiedSentence)o;
	    return cs.quantifier.Equals(quantifier)
			    && cs.variables.Equals(variables)
			    && cs.quantified.Equals(quantified);
	}

	public override int GetHashCode()
	{
	    if (0 == hashCode)
	    {
		hashCode = 17;
		hashCode = 37 * hashCode + quantifier.GetHashCode();
		foreach (Variable v in variables)
		{
		    hashCode = 37 * hashCode + v.GetHashCode();
		}
		hashCode = hashCode * 37 + quantified.GetHashCode();
	    }
	    return hashCode;
	}

	public override String ToString()
	{
	    if (null == stringRep)
	    {
		StringBuilder sb = new StringBuilder();
		sb.Append(quantifier);
		sb.Append(" ");
		foreach (Variable v in variables)
		{
		    sb.Append(v.ToString());
		    sb.Append(" ");
		}
		sb.Append(quantified.ToString());
		stringRep = sb.ToString();
	    }
	    return stringRep;
	}
    }
}