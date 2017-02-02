using aima.core.logic.propositional.parsing.ast;
using System;

namespace aima.core.logic.propositional.parsing.ast
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 244.<br>
     * <br>
     * <b>Complex Sentence:</b> are constructed from simpler sentences, using
     * parentheses (and square brackets) and logical connectives.
     *
     * @author Ciaran O'Reilly
     * @author Ravi Mohan 
     */
    public class ComplexSentence : Sentence
    {


        private Connective connective;
        private Sentence[] simplerSentences;
        // Lazy initialize these values.
        private int cachedHashCode = -1;
        private String cachedConcreteSyntax = null;
    
        /**
	    * Constructor.
	    * 
	    * @param connective
	    *            the complex sentence's connective.
	    * @param sentences
	    *            the simpler sentences making up the complex sentence.
	    */
        public ComplexSentence(Connective connective, params Sentence[] sentences)
        {
            // Assertion checks
            assertLegalArguments(connective, sentences);
    
            this.connective = connective;
            simplerSentences = new Sentence[sentences.Length];
            for (int i = 0; i < sentences.Length; i++)
            {
                simplerSentences[i] = sentences[i];
            }
        }
    
        /**
	    * Convenience constructor for binary sentences.
	    * 
	    * @param sentenceL
	    * 			the left hand sentence.
	    * @param binaryConnective
	    * 			the binary connective.0
12	    * @param sentenceR
	    *  		the right hand sentence.
	    */
        public ComplexSentence(Sentence sentenceL, Connective binaryConnective, Sentence sentenceR) : this(binaryConnective, sentenceL, sentenceR)
        {
            
        }
    
        public override Connective getConnective()
        {
            return connective;
        }
    
        public override int getNumberSimplerSentences()
        {
            return simplerSentences.Length;
        }
    
        public override Sentence getSimplerSentence(int offset)
        {
            return simplerSentences[offset];
        }
    
        public override bool Equals(Object o)
        {
            if (this == o)
            {
                return true;
            }
            if ((o == null) || (this.GetType() != o.GetType()))
            {
                return false;
            }
    
            bool result = false;
            ComplexSentence other = (ComplexSentence)o;
            if (other.GetHashCode() == this.GetHashCode())
            {
                if (other.getConnective().Equals(this.getConnective())
                        && other.getNumberSimplerSentences() == this
                                .getNumberSimplerSentences())
                {
                    // connective and # of simpler sentences match
                    // assume match and then test each simpler sentence
                    result = true;
                    for (int i = 0; i < this.getNumberSimplerSentences(); i++)
                    {
                        if (!other.getSimplerSentence(i).Equals(
                                this.getSimplerSentence(i)))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
    
            return result;
        }
    
        public override int GetHashCode()
        {
            if (cachedHashCode == -1)
            {
                cachedHashCode = 17 * getConnective().GetHashCode();
                foreach(Sentence s in simplerSentences)
                {
                    cachedHashCode = (cachedHashCode * 37) + s.GetHashCode();
                }
            }
    
            return cachedHashCode;
        }
    
        public override String ToString()
        {
            if (cachedConcreteSyntax == null)
            {
                if (isUnarySentence())
                {
                    cachedConcreteSyntax = getConnective()
                            + bracketSentenceIfNecessary(getConnective(), getSimplerSentence(0));
                }
                else if (isBinarySentence())
                {
                    cachedConcreteSyntax = bracketSentenceIfNecessary(getConnective(), getSimplerSentence(0))
                            + " "
                            + getConnective()
                            + " "
                            + bracketSentenceIfNecessary(getConnective(), getSimplerSentence(1));
                }
            }
            return cachedConcreteSyntax;
        }
    
        //
        // PRIVATE
        //
        private void assertLegalArguments(Connective connective, params Sentence[] sentences)
        {
            if (connective == null)
            {
                throw new ArgumentException("Connective must be specified.");
            }
            if (sentences == null)
            {
                throw new ArgumentException("> 0 simpler sentences must be specified.");
            }
            if (connective == Connective.NOT)
            {
                if (sentences.Length != 1)
                {
                    throw new ArgumentException("A not (~) complex sentence only take 1 simpler sentence not " + sentences.Length);
                }
            }
            else
            {
                if (sentences.Length != 2)
                {
                    throw new ArgumentException("Connective is binary (" + connective + ") but only " + sentences.Length + " simpler sentences provided");
                }
            }
        }
    }
}
        