using System;
using System.Collections.Generic;

namespace aima.core.util
{
    /**
     * A utility class for keeping counts of objects. Will return 0 for any object
     * for which it has not recorded a count against.
     * 
     * @author Ravi Mohan
     * @author Mike Stampone
     */
    public class FrequencyCounter<T>
    {
        private Dictionary<T, int> counter;
        private int total;

        /**
	     * Default Constructor.
	     */
        public FrequencyCounter()
        {
            counter = new Dictionary<T, int>();
            total = 0;
        }

        /**
	 * Returns the count to which the specified key is mapped in this frequency
	 * counter, or 0 if the map contains no mapping for this key.
	 * 
	 * @param key
	 *            the key whose associated count is to be returned.
	 * 
	 * @return the count to which this map maps the specified key, or 0 if the
	 *         map contains no mapping for this key.
	 */
        public int getCount(T key)
        {
            int value = counter[key];
            if (value == null)
            {
                return 0;
            }
            return value;
        }

        /**
	 * Increments the count to which the specified key is mapped in this
	 * frequency counter, or puts 1 if the map contains no mapping for this key.
	 * 
	 * @param key
	 *            the key whose associated count is to be returned.
	 */
        public void incrementFor(T key)
        {
            int value = counter[key];
            if(value == null)
            {
                counter.Add(key, 1);
            }
            else
            {
                counter.Add(key, value + 1);
            }
            // Keep track of the total
            total++;
        }

        /**
	 * Returns the count to which the specified key is mapped in this frequency
	 * counter, divided by the total of all counts.
	 * 
	 * @param key
	 *            the key whose associated count is to be divided.
    	 * 
    	 * @return the count to which this map maps the specified key, divided by
    	 *         the total count.
    	 */
        public Double probabilityOf(T key)
        {
            int value = getCount(key);
            if (value == 0)
            {
                return 0.0;
            }
            else
            {
                Double total = 0.0;
                foreach (T k in counter.Keys)
                {
                    total += getCount(k);
                }
                return value / total;
            }
        }

        /**
	 * 
	 * @return a set of objects for which frequency counts have been recorded.
    	 */
        public HashSet<T> getStates()
        {
            return new HashSet<T>(counter.Keys);
        }

        /**
	 * Remove all the currently recorded frequency counts.
	 */
        public void clear()
        {
            counter.Clear();
            total = 0;
        }

        public String toString()
        {
            return counter.ToString();
        }
    }
}