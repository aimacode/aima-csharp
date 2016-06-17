using System;
using System.Collections.Generic;
using System.Linq;

namespace aima.core.util
{
    /**
     * Note: This code is based on - <a href=
     * "http://download.oracle.com/javase/tutorial/collections/interfaces/set.html"
     * >Java Tutorial: The Set Interface</a> <br>
     * 
     * Using LinkedHashSet, even though slightly slower than HashSet, in order to
     * ensure order is always respected (i.e. if called with TreeSet or
     * LinkedHashSet implementations).
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class SetOps
    {
        /**
    	 * 
    	 * @param <T>
    	 * @param s1
    	 * @param s2
    	 * @return the union of s1 and s2. (The union of two sets is the set
    	 *         containing all of the elements contained in either set.)
    	 */
        public static List<T> union<T>(List<T> s1, List<T> s2)
        {
            if(s1 == s2)
            {
                return s1;
            }
            LinkedHashSet<T> union = new LinkedHashSet<T>(s1);
            union.UnionWith(s2);
            return union.ToList<T>();
        }

        /**
         * 
         * @param <T>
    	 * @param s1
    	 * @param s2
    	 * @return the intersection of s1 and s2. (The intersection of two sets is
    	 *         the set containing only the elements common to both sets.)
    	 */
        public static List<T> intersection<T>(List<T> s1, List<T> s2)
        {
            if (s1 == s2)
            {
                return s1;
            }
            LinkedHashSet<T> intersection = new LinkedHashSet<T>(s1);
            intersection.IntersectWith(s2);
            return intersection.ToList<T>();
        }

        /**
     	 * 
    	 * @param <T>
    	 * @param s1
    	 * @param s2
    	 * @return the (asymmetric) set difference of s1 and s2. (For example, the
    	 *         set difference of s1 minus s2 is the set containing all of the
    	 *         elements found in s1 but not in s2.)
    	 */
        public static List<T> difference<T>(List<T> s1, List<T> s2)
        {
            LinkedHashSet<T> difference = new LinkedHashSet<T>(s1);
            difference.ExceptWith(s2);
            return difference.ToList<T>();
        }
    }
}