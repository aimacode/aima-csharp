using System;
using System.Collections.Generic;
using aima.core.agent;

namespace aima.core.agent.impl
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
     public class DynamicEnvironmentState : ObjectWithDynamicAttributes, EnvironmentState
    {
        public DynamicEnvironmentState()
        {

        }

        public String describeType()
        {
            return typeof(EnvironmentState).Name;
        }
    }
}