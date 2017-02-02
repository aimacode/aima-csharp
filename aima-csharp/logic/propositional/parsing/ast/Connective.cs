using System;
using aima.core.util;
using System.Collections.Generic;

namespace aima.core.logic.propositional.parsing.ast
{
    /**
    * Artificial Intelligence A Modern Approach (3rd Edition): page 244.<br>
    * <br>
    * 
    * <pre>
    * <b>Logical Connectives:</b> There are five connectives in common use:
    * 1. ~   (not).
    * 2. &   (and).
    * 3. |   (or).
    * 4. =>  (implication).
    * 5. <=> (biconditional).
    * 
    * Note: We use ASCII characters that commonly have the same meaning to those 
    * symbols used in the book.
    * 
    * OPERATOR PRECEDENCE: ~, &, |, =>, <=>
    * </pre>
    * 
    * @author Avinash Agarwal
    * 
    */
    public class Connective
    {
        public static readonly Connective NOT = new Connective("~", 10);
        public static readonly Connective AND = new Connective("&", 8);
        public static readonly Connective OR = new Connective("|", 6);
        public static readonly Connective IMPLICATION = new Connective("=>", 4);
        public static readonly Connective BICONDITIONAL = new Connective("<=>", 2);

        public static IEnumerable<Connective> Values
        {
            get
            {
                yield return NOT;
                yield return AND;
                yield return OR;
                yield return IMPLICATION;
                yield return BICONDITIONAL;
            }
        }

        /**
	    * 
	    * @return the symbol for this connective.
	    */
        public String getSymbol()
        {
            return symbol;
        }

        /**
         * 
         * @return the precedence associated with this connective.
         */
        public int getPrecedence()
        {
            return precedence;
        }
        
        public String toString()
        {
            return getSymbol();
        }

        /**
         * Determine if a given symbol is representative of a connective.
         * 
         * @param symbol
         *            a symbol to be tested whether or not is represents a
         *            connective.
         * @return true if the symbol passed in is representative of a connective.
         */
        public static bool isConnective(String symbol)
        {
            if (NOT.getSymbol().Equals(symbol))
            {
                return true;
            }
            else if (AND.getSymbol().Equals(symbol))
            {
                return true;
            }
            else if (OR.getSymbol().Equals(symbol))
            {
                return true;
            }
            else if (IMPLICATION.getSymbol().Equals(symbol))
            {
                return true;
            }
            else if (BICONDITIONAL.getSymbol().Equals(symbol))
            {
                return true;
            }
            return false;
        }

        /**
         * Get the connective associated with the given symbolic representation.
         * 
         * @param symbol
         *            a symbol for which a corresponding connective is wanted.
         * @return the connective associated with a given symbol.
         * @throws IllegalArgumentException
         *             if a connective cannot be found that matches the given
         *             symbol.
         */
        public static Connective get(String symbol)
        {
            if (NOT.getSymbol().Equals(symbol))
            {
                return NOT;
            }
            else if (AND.getSymbol().Equals(symbol))
            {
                return AND;
            }
            else if (OR.getSymbol().Equals(symbol))
            {
                return OR;
            }
            else if (IMPLICATION.getSymbol().Equals(symbol))
            {
                return IMPLICATION;
            }
            else if (BICONDITIONAL.getSymbol().Equals(symbol))
            {
                return BICONDITIONAL;
            }

            throw new ArgumentException(
                    "Not a valid symbol for a connective: " + symbol);
        }

        /**
         * Determine if the given character is at the beginning of a connective.
         * 
         * @param ch
         *            a character.
         * @return true if the given character is at the beginning of a connective's
         *         symbolic representation, false otherwise.
         */
        public static bool isConnectiveIdentifierStart(char ch)
        {
            return _connectiveLeadingChars.Contains(ch);
        }

        /**
         * Determine if the given character is part of a connective.
         * 
         * @param ch
         *            a character.
         * @return true if the given character is part of a connective's symbolic
         *         representation, false otherwise.
         */
        public static bool isConnectiveIdentifierPart(char ch)
        {
            return _connectiveChars.Contains(ch);
        }

        //
        // PRIVATE
        //
        private static readonly HashSet<char> _connectiveLeadingChars = Util.createSet('~', '&', '|', '=', '<');
        private static readonly HashSet<char> _connectiveChars        = Util.createSet('~', '&', '|', '=', '<', '>');

        private readonly int precedence;
        private readonly String symbol;

        private Connective(String symbol, int precedence)
        {
            this.precedence = precedence;
            this.symbol = symbol;
        }

    }

}
