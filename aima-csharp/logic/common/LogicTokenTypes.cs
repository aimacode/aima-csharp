using System;
using System.Collections.Generic;

namespace aima.core.logic.common
{
    /**
     * @author Ravi Mohan
     * 
     */
    public enum LogicTokenTypes : int
    {
	SYMBOL = 1,

	LPAREN = 2,

	RPAREN = 3,

	COMMA = 4,

	CONNECTOR = 5,

	QUANTIFIER = 6,

	PREDICATE = 7,

	FUNCTION = 8,

	VARIABLE = 9,

	CONSTANT = 10,

	TRUE = 11,

	FALSE = 12,

	EQUALS = 13,

	WHITESPACE = 1000,

	EOI = 9999 // End of Input.
    }
}