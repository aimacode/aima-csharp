using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using aima.core.agent;
using aima.core.agent.impl;

namespace aima.core.agent.impl.aprog.simplerule
{
    /**
     * A simple implementation of a "condition-action rule".
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
     public class Rule
    {
        private Condition con;

        private Action action;


        /**
         * Constructs a condition-action rule.
         * 
         * @param con
         *            a condition
         * @param action
         *            an action
         */
        public Rule(Condition c, Action act)
        {
            Debug.Assert(null != con);
            Debug.Assert(null != action);

            con = c;
            action = act;
        }

        public bool evaluate(ObjectWithDynamicAttributes p)
        {
            return (con.evaluate(p));
        }

        /**
	 * Returns the action of this condition-action rule.
	 * 
	 * @return the action of this condition-action rule.
	 */
        public Action getAction()
        {
            return action;
        }

        public override bool Equals(System.Object o)
        {
            if (o == null || !(o is Rule))
            {
                return base.Equals(o);
            }
            return (ToString().Equals(((Rule)o).ToString()));
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override System.String ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("if ").Append(con).Append(" then ").Append(action)
                    .Append(".").ToString();
        }
    }
}