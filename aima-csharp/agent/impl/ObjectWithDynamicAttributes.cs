using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace aima.core.agent.impl
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
     public abstract class ObjectWithDynamicAttributes
    {
        private Dictionary<Object, Object> attributes = new Dictionary<Object, Object>();

        //PUBLIC METHODS

        /**
         * By default, returns the simple name of the underlying class as given in
         * the source code.
         * 
         * @return the simple name of the underlying class
         */
        public String describeType()
        {
            return this.GetType().Name;
        }

        /**
	 * Returns a string representation of the object's current attributes
	 * 
	 * @return a string representation of the object's current attributes
	 */
        public String describeAttributes()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            bool first = true;
            foreach (Object key in attributes.Keys)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }

                sb.Append(key);
                sb.Append("==");
                sb.Append(attributes[key]);
            }
            sb.Append("]");

            return sb.ToString();
        }

        /**
	 * Returns an unmodifiable view of the object's key set
	 * 
	 * @return an unmodifiable view of the object's key set
	 */
        public HashSet<Object> getKeySet()
        {
            return new HashSet<Object>(attributes.Keys);
        }

        /**
	 * Associates the specified value with the specified attribute key. If the
	 * ObjectWithDynamicAttributes previously contained a mapping for the
	 * attribute key, the old value is replaced.
	 * 
	 * @param key
	 *            the attribute key
	 * @param value
	 *            the attribute value
	 */
        public void setAttribute(Object key, Object value)
        {
            attributes[key] = value;
        }

        /**
	 * Returns the value of the specified attribute key, or null if the
	 * attribute was not found.
	 * 
	 * @param key
	 *            the attribute key
	 * 
	 * @return the value of the specified attribute name, or null if not found.
	 */
        public Object getAttribute(Object key)
        {
            return attributes[key];
        }

        /**
	 * Removes the attribute with the specified key from this
	 * ObjectWithDynamicAttributes.
	 * 
	 * @param key
	 *            the attribute key
	 */
        public void removeAttribute(Object key)
        {
            attributes.Remove(key);
        }


        /**
         * Creates and returns a copy of this ObjectWithDynamicAttributes
         */
        public ObjectWithDynamicAttributes copy()
        {
            ObjectWithDynamicAttributes copy = null;

            try
            {
                copy = (ObjectWithDynamicAttributes)this.GetType().GetConstructor(System.Type.EmptyTypes).Invoke(null);
                foreach (object val in attributes)
                {
                    copy.attributes.Add(val, attributes[val]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return copy;
        }

        public override bool Equals(Object o)
        {
            if (o == null || this.GetType() != o.GetType())
            {
                return base.Equals(o);
            }
            return attributes.Equals(((ObjectWithDynamicAttributes)o).attributes);
        }

        public override int GetHashCode()
        {
            return attributes.GetHashCode();
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(describeType());
            sb.Append(describeAttributes());

            return sb.ToString();
        }
    }
}