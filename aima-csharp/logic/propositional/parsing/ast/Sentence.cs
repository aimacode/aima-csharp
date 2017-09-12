using System;
using System.Collections.Generic;
using System.Linq;
using aima.core.logic.common;
using aima.core.logic.propositional.parsing;
using aima.core.util;

namespace aima.core.logic.propositional.parsing.ast
{
    /**
    * Artificial Intelligence A Modern Approach (3rd Edition): page 244.<br>
    * <br>
    * The base of the knowledge representation language for propositional logic.
    * Note: this class hierarchy defines the abstract syntax representation used
    * for representing propositional logic.
    * 
    * @author Ciaran O'Reilly
    * @author Ravi Mohan
    * @author Avinash Agarwal
    * 
    */

    public abstract class Sentence : ParseTreeNode
    {
        /**
	    * 
	    * @return the logical connective associated with this sentence if it has
	    *         one (i.e. is a ComplexSentence), null otherwise.
	    */
        public virtual Connective getConnective()
        {
            return null;
        }

        /**
	     * 
	     * @return the number of simpler sentences contained in this sentence. Will
	     *         only be > 0 if a Complex Sentence.
	     */
        public virtual int getNumberSimplerSentences()
        {
            return 0;
        }

        /**
         * Get the simplified sentence, at the specified offset (starts at 0),
         * contained by this Sentence if it is a Complex Sentence, null otherwise.
         * 
         * @param offset
         *            the offset of the contained simplified sentence to retrieve.
         * @return the simplified sentence, at the specified offset, contained by
         *         this sentence (if a complex sentence), null otherwise.
         */
        public virtual Sentence getSimplerSentence(int offset)
        {
            return null;
        }

        /**
         * 
         * @return true if a complex sentence with a Not connective, false
         *         otherwise.
         */
        public bool isNotSentence()
        {
            return hasConnective(Connective.NOT);
        }

        /**
         * 
         * @return true if a complex sentence with an And connective, false
         *         otherwise.
         */
        public bool isAndSentence()
        {
            return hasConnective(Connective.AND);
        }

        /**
         * 
         * @return true if a complex sentence with an Or connective, false
         *         otherwise.
         */
        public bool isOrSentence()
        {
            return hasConnective(Connective.OR);
        }

        /**
         * 
         * @return true if a complex sentence with an Implication connective, false
         *         otherwise.
         */
        public bool isImplicationSentence()
        {
            return hasConnective(Connective.IMPLICATION);
        }

        /**
         * 
         * @return true if a complex sentence with a Biconditional connective, false
         *         otherwise.
         */
        public bool isBiconditionalSentence()
        {
            return hasConnective(Connective.BICONDITIONAL);
        }

        /**
         * 
         * @return true if a proposition symbol, false otherwise.
         */
        public bool isPropositionSymbol()
        {
            return getConnective() == null;
        }

        /**
         * 
         * @return true if a complex sentence containing a single simpler sentence,
         *         false otherwise.
         */
        public bool isUnarySentence()
        {
            return hasConnective(Connective.NOT);
        }

        /**
         * 
         * @return true if a complex sentence containing two simpler sentences,
         *         false otherwise.
         */
        public bool isBinarySentence()
        {
            return getConnective() != null && !hasConnective(Connective.NOT);
        }

        /**
         * Allow a PLVisitor to walk over the abstract syntax tree represented by this
         * Sentence.
         * 
         * @param plv
         *            a Propositional Logic visitor.
         * @param arg
         *            an optional argument for use by the visior.
         * @return a result specific to the visitors behavior.
         */
        public R accept<A, R>(PLVisitor<A, R> plv, A arg)
        {
            R result = default(R);
            if (isPropositionSymbol())
            {
                result = plv.visitPropositionSymbol((PropositionSymbol)this, arg);
            }
            else if (isUnarySentence())
            {
                result = plv.visitUnarySentence((ComplexSentence)this, arg);
            }
            else if (isBinarySentence())
            {
                result = plv.visitBinarySentence((ComplexSentence)this, arg);
            }

            return result;
        }

        /**
         * Utility routine that will create a string representation of a given
         * Sentence and place it inside brackets if it is a complex sentence that
         * has lower precedence than this complex sentence.<br>
         * <br>
         * Note: this is a form of pretty printing, whereby we only add brackets in
         * the concrete syntax representation as needed to ensure it can be parsed
         * back again into an equivalent abstract syntax representation used here.
         * 
         * @param parentConnective
         *            the connective of the parent sentence.
         * @param childSentence
         *            a simpler child sentence.
         * @return a String representation of the Sentence, bracketed if the parent
         *         based on its connective has higher precedence.
         */
        public String bracketSentenceIfNecessary(Connective parentConnective,
                Sentence childSentence)
        {
            String result = null;
            if (childSentence is ComplexSentence) {
                ComplexSentence cs = (ComplexSentence)childSentence;
                if (cs.getConnective().getPrecedence() < parentConnective
                        .getPrecedence())
                {
                    result = "(" + childSentence + ")";
                }
            }

            if (result == null)
            {
                result = childSentence.ToString();
            }

            return result;
        }

        /**
         * Create a disjunction of disjuncts.
         * @param disjuncts
         * 			the disjuncts from which to create the disjunction.
         * @return a disjunction of the given disjuncts.
         */
        public static Sentence newDisjunction(params Sentence[] disjuncts)
        {
            return newDisjunction(disjuncts.ToList());
        }

        /**
         * Create a disjunction of disjuncts.
         * @param disjuncts
         * 			the disjuncts from which to create the disjunction.
         * @return a disjunction of the given disjuncts.
         */
        public static Sentence newDisjunction(List<Sentence> disjuncts)
        {
            if (disjuncts.Count == 0)
            {
                return PropositionSymbol.FALSE;
            }
            else if (disjuncts.Count == 1)
            {
                return disjuncts[0];
            }
            return new ComplexSentence(Util.first(disjuncts), Connective.OR, newDisjunction(Util.rest(disjuncts)));
        }

        /**
         * Create a conjunction of conjuncts.
         * @param conjuncts
         * 			the conjuncts from which to create the conjunction.
         * @return a conjunction of the given conjuncts.
         */
        public static Sentence newConjunction(params Sentence[] conjuncts)
        {
            return newConjunction(conjuncts.ToList());
        }

        /**
         * Create a conjunction of conjuncts.
         * @param conjuncts
         * 			the conjuncts from which to create the conjunction.
         * @return a conjunction of the given conjuncts.
         */
        public static Sentence newConjunction(List<Sentence> conjuncts)
        {
            if (conjuncts.Count == 0)
            {
                return PropositionSymbol.TRUE;
            }
            else if (conjuncts.Count == 1)
            {
                return conjuncts[0];
            }
            return new ComplexSentence(Util.first(conjuncts), Connective.AND, newConjunction(Util.rest(conjuncts)));
        }
        
        // PROTECTED

        protected bool hasConnective(Connective connective)
        {
            // Note: can use '==' as Connective is an enum.
            return getConnective() == connective;
        }
    }
}
