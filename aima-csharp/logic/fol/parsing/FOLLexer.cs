using System.Text;
using aima.core.logic.common;
using aima.core.logic.fol.domain;

namespace aima.core.logic.fol.parsing
{
    /**
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * 
     */
    public class FOLLexer : Lexer
    {
	private FOLDomain domain;
	private HashSet<String> connectors, quantifiers;

	public FOLLexer(FOLDomain domain)
	{
	    this.domain = domain;

	    connectors = new HashSet<String>();
	    connectors.Add(Connectors.NOT);
	    connectors.Add(Connectors.AND);
	    connectors.Add(Connectors.OR);
	    connectors.Add(Connectors.IMPLIES);
	    connectors.Add(Connectors.BICOND);

	    quantifiers = new HashSet<String>();
	    quantifiers.Add(Quantifiers.FORALL);
	    quantifiers.Add(Quantifiers.EXISTS);
	}

	public FOLDomain getFOLDomain()
	{
	    return domain;
	}

	public override Token nextToken()
	{
	    int startPosition = getCurrentPositionInInput();
	    if (lookAhead(1) == '(')
	    {
		consume();
		return new Token((int)LogicTokenTypes.LPAREN, "(", startPosition);

	    }
	    else if (lookAhead(1) == ')')
	    {
		consume();
		return new Token((int)LogicTokenTypes.RPAREN, ")", startPosition);

	    }
	    else if (lookAhead(1) == ',')
	    {
		consume();
		return new Token((int)LogicTokenTypes.COMMA, ",", startPosition);

	    }
	    else if (identifierDetected())
	    {
		// System.Console.WriteLine("identifier detected");
		return identifier();
	    }
	    else if (char.IsWhiteSpace(lookAhead(1)))
	    {
		consume();
		return nextToken();
	    }
	    else if (lookAhead(1) == 65535)
	    {
		return new Token((int)LogicTokenTypes.EOI, "EOI", startPosition);
	    }
	    else
	    {
		throw new LexerException("Lexing error on character " + lookAhead(1) + " at position " + getCurrentPositionInInput(), getCurrentPositionInInput());
	    }
	}

	private bool isCSharpIdentifierStart(char c)
	{
	    return char.IsLetter(c) || c == '_' || c == '$' || char.IsNumber(c);
	}

	private Token identifier()
	{
	    int startPosition = getCurrentPositionInInput();
	    StringBuilder sbuf = new StringBuilder();
	    while ((isCSharpIdentifierStart(lookAhead(1)))
		    || partOfConnector())
	    {
		sbuf.Append(lookAhead(1));
		consume();
	    }
	    String readString = sbuf.ToString();
	    // System.Console.WriteLine(readString);
	    if (connectors.Contains(readString))
	    {
		return new Token((int)LogicTokenTypes.CONNECTOR, readString, startPosition);
	    }
	    else if (quantifiers.Contains(readString))
	    {
		return new Token((int)LogicTokenTypes.QUANTIFIER, readString, startPosition);
	    }
	    else if (domain.getPredicates().Contains(readString))
	    {
		return new Token((int)LogicTokenTypes.PREDICATE, readString, startPosition);
	    }
	    else if (domain.getFunctions().Contains(readString))
	    {
		return new Token((int)LogicTokenTypes.FUNCTION, readString, startPosition);
	    }
	    else if (domain.getConstants().Contains(readString))
	    {
		return new Token((int)LogicTokenTypes.CONSTANT, readString, startPosition);
	    }
	    else if (isVariable(readString))
	    {
		return new Token((int)LogicTokenTypes.VARIABLE, readString, startPosition);
	    }
	    else if (readString.Equals("="))
	    {
		return new Token((int)LogicTokenTypes.EQUALS, readString, startPosition);
	    }
	    else
	    {
		throw new LexerException("Lexing error on character " + lookAhead(1) + " at position " + getCurrentPositionInInput(), getCurrentPositionInInput());
	    }
	}

	private bool isVariable(String s)
	{
	    return (char.IsLower(s[0]));
	}

	private bool identifierDetected()
	{
	    return (isCSharpIdentifierStart(((char)lookAheadBuffer[0]))
		    || partOfConnector());
	}

	private bool partOfConnector()
	{
	    return (lookAhead(1) == '=') || (lookAhead(1) == '<')
			    || (lookAhead(1) == '>');
	}
    }
}
