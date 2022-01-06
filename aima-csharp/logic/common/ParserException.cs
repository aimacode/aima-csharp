namespace aima.core.logic.common
{
    /**
     * A runtime exception to be used to describe Parser exceptions. In particular
     * it provides information to help in identifying which tokens proved
     * problematic in the parse.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class ParserException : SystemException
    {
	private static readonly long serialVersionUID = 1L;

	private List<Token> problematicTokens = new List<Token>();

	public ParserException(String message, params Token[] problematicTokens): base(message)
	{	    
	    if (problematicTokens != null)
	    {
		foreach (Token pt in problematicTokens)
		{
		    this.problematicTokens.Add(pt);
		}
	    }
	}

	public ParserException(String message, Exception cause, params Token[] problematicTokens): base(message, cause)
	{	    
	    if (problematicTokens != null)
	    {
		foreach (Token pt in problematicTokens)
		{
		    this.problematicTokens.Add(pt);
		}
	    }
	}

	/**
	 * 
	 * @return a list of 0 or more tokens from the input stream that are
	 *         believed to have contributed to the parse exception.
	 */
	public List<Token> getProblematicTokens()
	{
	    return problematicTokens;
	}
    }
}