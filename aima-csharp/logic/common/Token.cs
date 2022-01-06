namespace aima.core.logic.common
{
    /**
     * A token generated by a lexer from a sequence of characters.
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class Token
    {
	private int type;
	private String text;
	private int startCharPositionInInput;

	/**
	 * Constructs a token from the specified token-name and attribute-value
	 * 
	 * @param type
	 *            the token-name
	 * @param text
	 *            the attribute-value
	 * @param startCharPositionInInput
	 *            the position (starting from 0) at which this token 
	 *            starts in  the input.
	 */
	public Token(int type, String text, int startCharPositionInInput)
	{
	    this.type = type;
	    this.text = text;
	    this.startCharPositionInInput = startCharPositionInInput;
	}

	/**
	 * Returns the attribute-value of this token.
	 * 
	 * @return the attribute-value of this token.
	 */
	public String getText()
	{
	    return text;
	}

	/**
	 * Returns the token-name of this token.
	 * 
	 * @return the token-name of this token.
	 */
	public int getType()
	{
	    return type;
	}

	/**
	 * @return the position (starting from 0) at which this token starts in the
	 *         input.
	 */
	public int getStartCharPositionInInput()
	{
	    return startCharPositionInInput;
	}

	public  override bool Equals(Object o)
	{

	    if (this == o)
	    {
		return true;
	    }
	    if ((o == null) || !(o is Token))
	    {
		return false;
	    }

	    Token other = (Token)o;
	    return ((other.type == type) && (other.text.Equals(text)) && (other.startCharPositionInInput == startCharPositionInInput));
	}

	public override int GetHashCode()
	{
	    int result = 17;
	    result = 37 * result + type;
	    result = 37 * result + text.GetHashCode();
	    result = 37 * result + startCharPositionInInput;
	    return result;
	}

	public override String ToString()
	{
	    return "[ " + type + " " + text + " " + startCharPositionInInput + " ]";
	}
    }
}