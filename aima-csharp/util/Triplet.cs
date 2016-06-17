using System;
using System.Collections.Generic;

namespace aima.core.util
{
    /**
     * @author Ravi Mohan
     * @author Mike Stampone
     * 
     */
     public class Triplet<X, Y, Z>
    {
        private X x;

	private Y y;

        private Z z;

	/**
	 * Constructs a triplet with three specified elements.
	 * 
	 * @param x
	 *            the first element of the triplet.
	 * @param y
	 *            the second element of the triplet.
	 * @param z
	 *            the third element of the triplet.
	 */
	public Triplet(X x, Y y, Z z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /**
	 * Returns the second element of the triplet.
	 * 
	 * @return the second element of the triplet.
	 */
        public Y getSecond()
        {
            return y;
        }

        /**
	 * Returns the third element of the triplet.
	 * 
	 * @return the third element of the triplet.
	 */
        public Z getThird()
        {
            return z;
        }

        public override bool Equals(Object o)
        {
            if (o is Triplet<X, Y, Z>)
            {
                Triplet<X, Y, Z> other = (Triplet<X, Y, Z>)o;
                return (x.Equals(other.x)) && (y.Equals(other.y))
                        && (y.Equals(other.y));
            }
            return false;
        }

        public int hashCode()
        {
            return x.GetHashCode() + 31 * y.GetHashCode() + 31 * z.GetHashCode();
        }

        public String toString()
        {
            return "< " + x.ToString() + " , " + y.ToString() + " , "
                    + z.ToString() + " >";
        }
    }
}