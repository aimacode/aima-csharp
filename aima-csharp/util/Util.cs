using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aima.core.util
{

    /**
     * @author Ravi Mohan
     * 
     */
    public class Util
    {
        public const String NO = "No";

        public const String YES = "Yes";
        
        private static Random r = new Random();

        /**
	 * Get the first element from a list.
	 * 
    	 * @param l
    	 *            the list the first element is to be extracted from.
    	 * @return the first element of the passed in list.
    	 */
        public static T first<T>(List<T> l)
        {
            return l[0];
        }

        /**
    	 * Get a sublist of all of the elements in the list except for first.
    	 * 
    	 * @param l
    	 *            the list the rest of the elements are to be extracted from.
    	 * @return a list of all of the elements in the passed in list except for
    	 *         the first element.
    	 */
        public static List<T> rest<T>(List<T> l)
        {
            List<T> newList = l.GetRange(1, l.Count - 1);
            return newList;
        }

        /**
	    * Create a set for the provided values.
	    * @param values
	    *        the sets initial values.
	    * @return a Set of the provided values.
	    */
    
        public static HashSet<T> createSet<T>(params T[] values)
        {
            HashSet<T> set = new HashSet<T>();

            foreach(T t in values)
            {
                set.Add(t);
            }

            return set;
        }


        public static bool randombool()
        {
            int trueOrFalse = r.Next(2);
            return (!(trueOrFalse == 0));
        }

        /**
    	 * Randomly select an element from a list.
    	 * 
    	 * @param <T>
    	 *            the type of element to be returned from the list l.
    	 * @param l
    	 *            a list of type T from which an element is to be selected
    	 *            randomly.
    	 * @return a randomly selected element from l.
	     */
        public static T selectRandomlyFromList<T>(List<T> l)
        {
            return l[r.Next(l.Count)];
        }


        public static double[] normalize(double[] probDist)
        {
            int len = probDist.Length;
            double total = 0.0;
            foreach (double d in probDist)
            {
                total = total + d;
            }

            double[] normalized = new double[len];
            if (total != 0)
            {
                for (int i = 0; i < len; i++)
                {
                    normalized[i] = probDist[i] / total;
                }
            }
            return normalized;
        }

        public static List<Double> normalize(List<Double> values)
        {
            double[] valuesAsArray = new double[values.Count];
            for (int i = 0; i < valuesAsArray.Length; i++)
            {
                valuesAsArray[i] = values[i];
            }
            double[] normalized = normalize(valuesAsArray);
            List<Double> results = new List<Double>();
            for (int i = 0; i < normalized.Length; i++)
            {
                results.Add(normalized[i]);
            }
            return results;
        }

        public static int min(int i, int j)
        {
            return (i > j ? j : i);
        }

        public static int max(int i, int j)
        {
            return (i < j ? j : i);
        }

        public static int max(int i, int j, int k)
        {
            return max(max(i, j), k);
        }

        public static int min(int i, int j, int k)
        {
            return min(min(i, j), k);
        }

        public static T mode<T>(List<T> l)
        {
            Dictionary<T, int> hash = new Dictionary<T, int>();
            foreach (T obj in l)
            {
                if (hash.ContainsKey(obj))
                {
                    hash[obj] = hash[obj] + 1;
                }
                else
                {
                    hash.Add(obj, 1);
                }
            }

            T maxkey = hash.Keys.FirstOrDefault<T>();
            foreach (T key in hash.Keys)
            {
                if (hash[key] > hash[maxkey])
                {
                    maxkey = key;
                }
            }
            return maxkey;
        }

        public static String[] yesno()
        {
            return new String[] { YES, NO };
        }

        public static double log2(double d)
        {
            return System.Math.Log(d) / System.Math.Log(2);
        }

        public static double information(double[] probabilities)
        {
            double total = 0.0;
            foreach (double d in probabilities)
            {
                total += (-1.0 * log2(d) * d);
            }
            return total;
        }

        public static List<T> removeFrom<T>(List<T> list, T member)
        {
            List<T> newList = new List<T>(list);
            newList.Remove(member);
            return newList;
        }

        public static double sumOfSquares<T>(List<T> list)
        {
            double accum = 0;
            foreach (T item in list)
            {
                accum = accum + (Convert.ToDouble(item) * Convert.ToDouble(item));
            }
            return accum;
        }

        public static String ntimes(String s, int n)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                buf.Append(s);
            }
            return buf.ToString();
        }

        public static void checkForNanOrInfinity(double d)
        {
            if (Double.IsNaN(d))
            {
                throw new ArgumentException("Not a Number");
            }
            if (Double.IsInfinity(d))
            {
                throw new ArgumentException("Infinite Number");
            }
        }

        public static int randomNumberBetween(int i, int j)
        {
            /* i,j bothinclusive */
            return r.Next(j - i + 1) + i;
        }

        public static double calculateMean(List<Double> lst)
        {
            Double sum = 0.0;
            foreach (Double d in lst)
            {
                sum = sum + d;
            }
            return sum / lst.Count;
        }

        public static double calculateStDev(List<Double> values, double mean)
        {

            int listSize = values.Count;

            Double sumOfDiffSquared = 0.0;
            foreach (Double value in values)
            {
                double diffFromMean = value - mean;
                sumOfDiffSquared += ((diffFromMean * diffFromMean) / (listSize - 1));
                // division moved here to avoid sum becoming too big if this
                // doesn't work use incremental formulation

            }
            double variance = sumOfDiffSquared;
            // (listSize - 1);
            // assumes at least 2 members in list.
            return System.Math.Sqrt(variance);
        }

        public static List<Double> normalizeFromMeanAndStdev(List<Double> values,
                double mean, double stdev)
        {
            List<Double> normalized = new List<Double>();
            foreach (Double d in values)
            {
                normalized.Add((d - mean) / stdev);
            }
            return normalized;
        }

        public static double generateRandomDoubleBetween(double lowerLimit,
                double upperLimit)
        {

            return lowerLimit + ((upperLimit - lowerLimit) * r.NextDouble());
        }
    }
}
