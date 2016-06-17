using System;
using System.Collections.Generic;
using aima.core.agent;

namespace aima.core.agent.impl
{
    /**
     * @author Ciaran O'Reilly
     */
    public class DynamicAction : ObjectWithDynamicAttributes,
            Action
    {
        public const String ATTRIBUTE_NAME = "name";

        public DynamicAction(String name)
        {
            this.setAttribute(ATTRIBUTE_NAME, name);
        }

        /**
	 * Returns the value of the name attribute.
	 * 
	 * @return the value of the name attribute.
	 */
        public String getName()
        {
            return (System.String)getAttribute(ATTRIBUTE_NAME);
        }

        // START-Action
        public bool isNoOp()
        {
            return false;
        }

        // END-Action
        public String describeType()
        {
            return this.GetType().Name;
        }
    }
}