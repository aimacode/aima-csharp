using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using aima.core.agent.impl;

namespace aima.core.agent.impl.aprog.simplerule
{
    /**
     * Implementation of a NOT condition.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class NOTCondition : Condition
    {
        private Condition con;

        public NOTCondition(Condition c)
        {
            Debug.Assert(null != con);

            con = c;
        }

        public override bool evaluate(ObjectWithDynamicAttributes p)
        {
            return (!con.evaluate(p));
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("![").Append(con).Append("]").ToString();
        }
    }
}