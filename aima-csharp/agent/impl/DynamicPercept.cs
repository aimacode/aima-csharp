using System;
using System.Collections.Generic;
using aima.core.agent;
using System.Diagnostics;

namespace aima.core.agent.impl
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class DynamicPercept : ObjectWithDynamicAttributes, Percept
    {
        public DynamicPercept()
        {

        }

        public String describeType()
        {
            return typeof(Percept).Name;
        }

        /**
	 * Constructs a DynamicPercept with one attribute
	 * 
	 * @param key1
	 *            the attribute key
	 * @param value1
	 *            the attribute value
	 */
        public DynamicPercept(Object key1, Object value1)
        {
            setAttribute(key1, value1);
        }


        /**
         * Constructs a DynamicPercept with two attributes
         * 
         * @param key1
         *            the first attribute key
         * @param value1
         *            the first attribute value
         * @param key2
         *            the second attribute key
         * @param value2
         *            the second attribute value
         */
        public DynamicPercept(Object key1, Object value1, Object key2, Object value2)
        {
            setAttribute(key1, value1);
            setAttribute(key2, value2);
        }

        /**
	 * Constructs a DynamicPercept with an array of attributes
	 * 
	 * @param keys
	 *            the array of attribute keys
	 * @param values
	 *            the array of attribute values
	 */
        public DynamicPercept(Object[] keys, Object[] values)
        {
            Debug.Assert(keys.Length == values.Length);

            for(int i = 0; i < keys.Length; i++)
            {
                setAttribute(keys[i], values[i]);
            }
        }
    }
}