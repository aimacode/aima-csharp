using System;
using System.Collections.Generic;

namespace aima.core.util
{
    /**
     * @author Ravi Mohan
     * @author Mike Stampone
     * 
     */
    public class Pair<X, Y>
    {
        private X a;

        private Y b;

        /**
    	 * Constructs a Pair from two given elements
    	 * 
    	 * @param a
    	 *            the first element
    	 * @param b
    	 *            the second element
    	 */
        public Pair(X a, Y b)
        {
            this.a = a;
            this.b = b;
        }

        /**
	 * Returns the first element of the pair
    	 * 
    	 * @return the first element of the pair
    	 */
        public X getFirst()
        {
            return a;
        }

        /**
         * Returns the second element of the pair
	 * 
	 * @return the second element of the pair
	 */
        public Y getSecond()
        {
            return b;
        }


        public override bool Equals(Object o)
        {
            if (o is Pair<X, Y>)
            {
                Pair<X, Y> p = (Pair<X, Y>)o;
                return a.Equals(p.a) && b.Equals(p.b);
            }
            return false;
        }

        public int hashCode()
        {
            return a.GetHashCode() + 31 * b.GetHashCode();
        }

        public override String ToString()
        {
            return "< " + getFirst().ToString() + " , " + getSecond().ToString()
                    + " > ";
        }
    }
}