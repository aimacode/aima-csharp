using System;
using System.Collections.Generic;
using aima.core.agent;

namespace aima.core.agent.impl
{
    /**
     * @author Ciaran O'Reilly
     */
    public class DynamicState : ObjectWithDynamicAttributes, State
    {
        public DynamicState()
        {

        }


        public String describeType()
        {
            return typeof(State).Name;
        }
    }
}