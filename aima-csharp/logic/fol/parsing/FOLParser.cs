using System;
using System.Collections.Generic;
using aima.core.logic.common;
using aima.core.logic.fol.domain;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.parsing
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class FOLParser
    {
	private FOLLexer lexer;

	protected Token[] lookAheadBuffer;

	protected int LookAhead = 1;

	public FOLParser(FOLLexer lexer)
	{
	    this.lexer = lexer;
	    lookAheadBuffer = new Token[LookAhead];
	}

	public FOLParser(FOLDomain domain): this(new FOLLexer(domain))
	{
	    
	}

	public FOLDomain getFOLDomain()
	{
	    return lexer.getFOLDomain();
	}

	public Sentence parse(String s)
	{
	    setUpToParse(s);
	    return parseSentence();
	}

	public void setUpToParse(String s)
	{
	    lookAheadBuffer = new Token[1];
	    lexer.setInput(s);
	    fillLookAheadBuffer();
	}

	private Term parseTerm()
	{
	    Token t = lookAhead(1);
	    int tokenType = t.getType();
	    if (tokenType == (int)LogicTokenTypes.CONSTANT)
	    {
		return parseConstant();
	    }
	    else if (tokenType == (int)LogicTokenTypes.VARIABLE)
	    {
		return parseVariable();
	    }
	    else if (tokenType == (int)LogicTokenTypes.FUNCTION)
	    {
		return parseFunction();
	    }
	    else
	    {
		return null;
	    }
	}

	public Term parseVariable()
	{
	    Token t = lookAhead(1);
	    String value = t.getText();
	    consume();
	    return new Variable(value);
	}

	public Term parseConstant()
	{
	    Token t = lookAhead(1);
	    String value = t.getText();
	    consume();
	    return new Constant(value);
	}

	public Term parseFunction()
	{
	    Token t = lookAhead(1);
	    String functionName = t.getText();
	    List<Term> terms = processTerms();
	    return new Function(functionName, terms);
	}

	public Sentence parsePredicate()
	{
	    Token t = lookAhead(1);
	    String predicateName = t.getText();
	    List<Term> terms = processTerms();
	    return new Predicate(predicateName, terms);
	}

	private List<Term> processTerms()
	{
	    consume();
	    List<Term> terms = new List<Term>();
	    match("(");
	    Term term = parseTerm();
	    terms.Add(term);

	    while (lookAhead(1).getType() == (int)LogicTokenTypes.COMMA)
	    {
		match(",");
		term = parseTerm();
		terms.Add(term);
	    }
	    match(")");
	    return terms;
	}

	public Sentence parseTermEquality()
	{
	    Term term1 = parseTerm();
	    match("=");
	    // System.Console.WriteLine("=");
	    Term term2 = parseTerm();
	    return new TermEquality(term1, term2);
	}

	public Sentence parseNotSentence()
	{
	    match("NOT");
	    return new NotSentence(parseSentence());
	}

	// PROTECTED METHODS
	
	protected Token lookAhead(int i)
	{
	    return lookAheadBuffer[i - 1];
	}

	protected void consume()
	{
	    // System.Console.WriteLine("consuming" +lookAheadBuffer[0].getText());
	    loadNextTokenFromInput();
	    // System.Console.WriteLine("next token " +lookAheadBuffer[0].getText());
	}

	protected void loadNextTokenFromInput()
	{

	    bool eoiEncountered = false;
	    for (int i = 0; i < LookAhead - 1; i++)
	    {

		lookAheadBuffer[i] = lookAheadBuffer[i + 1];
		if (isEndOfInput(lookAheadBuffer[i]))
		{
		    eoiEncountered = true;
		    break;
		}
	    }
	    if (!eoiEncountered)
	    {
		try
		{
		    lookAheadBuffer[LookAhead - 1] = lexer.nextToken();
		}
		catch (Exception e)
		{
		    Console.WriteLine(e);
		}
	    }
	}

	protected bool isEndOfInput(Token t)
	{
	    return (t.getType() == (int)LogicTokenTypes.EOI);
	}

	protected void fillLookAheadBuffer()
	{
	    for (int i = 0; i < LookAhead; i++)
	    {
		lookAheadBuffer[i] = lexer.nextToken();
	    }
	}

	protected void match(String terminalSymbol)
	{
	    if (lookAhead(1).getText().Equals(terminalSymbol))
	    {
		consume();
	    }
	    else
	    {
		throw new ApplicationException(
			"Syntax error detected at match. Expected "
				+ terminalSymbol + " but got "
				+ lookAhead(1).getText());
	    }
	}

	// PRIVATE METHODS
	
	private Sentence parseSentence()
	{
	    Token t = lookAhead(1);
	    if (lParen(t))
	    {
		return parseParanthizedSentence();
	    }
	    else if ((lookAhead(1).getType() == (int)LogicTokenTypes.QUANTIFIER))
	    {

		return parseQuantifiedSentence();
	    }
	    else if (notToken(t))
	    {
		return parseNotSentence();
	    }
	    else if (predicate(t))
	    {
		return parsePredicate();
	    }
	    else if (term(t))
	    {
		return parseTermEquality();
	    }

	    throw new ApplicationException("parse failed with Token " + t.getText());
	}

	private Sentence parseQuantifiedSentence()
	{
	    String quantifier = lookAhead(1).getText();
	    consume();
	    List<Variable> variables = new List<Variable>();
	    Variable var = (Variable)parseVariable();
	    variables.Add(var);
	    while (lookAhead(1).getType() == (int)LogicTokenTypes.COMMA)
	    {
		consume();
		var = (Variable)parseVariable();
		variables.Add(var);
	    }
	    Sentence sentence = parseSentence();
	    return new QuantifiedSentence(quantifier, variables, sentence);
	}

	private Sentence parseParanthizedSentence()
	{
	    match("(");
	    Sentence sen = parseSentence();
	    while (binaryConnector(lookAhead(1)))
	    {
		String connector = lookAhead(1).getText();
		consume();
		Sentence other = parseSentence();
		sen = new ConnectedSentence(connector, sen, other);
	    }
	    match(")");
	    return sen; /* new ParanthizedSentence */

	}

	private bool binaryConnector(Token t)
	{
	    if ((t.getType() == (int)LogicTokenTypes.CONNECTOR)
			    && (!(t.getText().Equals("NOT"))))
	    {
		return true;
	    }
	    else
	    {
		return false;
	    }
	}

	private bool lParen(Token t)
	{
	    if (t.getType() == (int)LogicTokenTypes.LPAREN)
	    {
		return true;
	    }
	    else
	    {
		return false;
	    }
	}

	private bool term(Token t)
	{
	    if ((t.getType() == (int)LogicTokenTypes.FUNCTION)
			    || (t.getType() == (int)LogicTokenTypes.CONSTANT)
			    || (t.getType() == (int)LogicTokenTypes.VARIABLE))
	    {
		return true;
	    }
	    else
	    {
		return false;
	    }

	}

	private bool predicate(Token t)
	{
	    if ((t.getType() == (int)LogicTokenTypes.PREDICATE))
	    {
		return true;
	    }
	    else
	    {
		return false;
	    }
	}

	private bool notToken(Token t)
	{
	    if ((t.getType() == (int)LogicTokenTypes.CONNECTOR)
			    && (t.getText().Equals("NOT")))
	    {
		return true;
	    }
	    else
	    {
		return false;
	    }
	}
    }
}