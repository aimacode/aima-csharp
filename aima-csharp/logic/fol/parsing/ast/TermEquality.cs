using System.Text;
using System.Collections.ObjectModel;

namespace aima.core.logic.fol.parsing.ast
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class TermEquality : AtomicSentence
    {
	private Term term1, term2;
	private List<Term> terms = new List<Term>();
	private String stringRep = null;
	private int hashCode = 0;

	public static String getEqualitySynbol()
	{
	    return "=";
	}

	public TermEquality(Term term1, Term term2)
	{
	    this.term1 = term1;
	    this.term2 = term2;
	    terms.Add(term1);
	    terms.Add(term2);
	}

	public Term getTerm1()
	{
	    return term1;
	}

	public Term getTerm2()
	{
	    return term2;
	}
		
	// START-AtomicSentence

	public String getSymbolicName()
	{
	    return getEqualitySynbol();
	}

	public bool isCompound()
	{
	    return true;
	}

        List<Term> AtomicSentence.getArgs()
        {
            return null;
        }

        AtomicSentence AtomicSentence.copy()
        {
            return null;
        }

        public List<FOLNode> getArgs()
	{
	    return new ReadOnlyCollection<Term>(terms).ToList<FOLNode>();
	}

	public Object accept(FOLVisitor v, Object arg)
	{
	    return v.visitTermEquality(this, arg);
	}

	public FOLNode copy()
	{
	    return new TermEquality((Term)term1.copy(), (Term)term2.copy());
	}

        public Sentence copySentence()
        {
            return null;
        }

        // END-AtomicSentence
	
	public override bool Equals(Object o)
	{

	    if (this == o)
	    {
		return true;
	    }
	    if ((o == null) || !(o is TermEquality))
	    {
		return false;
	    }
	    TermEquality te = (TermEquality)o;

	    return te.getTerm1().Equals(term1) && te.getTerm2().Equals(term2);
	}

	public override int GetHashCode()
	{

	    if (0 == hashCode)
	    {
		hashCode = 17;
		hashCode = 37 * hashCode + getTerm1().GetHashCode();
		hashCode = 37 * hashCode + getTerm2().GetHashCode();
	    }
	    return hashCode;
	}

	public override String ToString()
	{
	    if (null == stringRep)
	    {
		StringBuilder sb = new StringBuilder();
		sb.Append(term1.ToString());
		sb.Append(" = ");
		sb.Append(term2.ToString());
		stringRep = sb.ToString();
	    }
	    return stringRep;
	}
    }
}