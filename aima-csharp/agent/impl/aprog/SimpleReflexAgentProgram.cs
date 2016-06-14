using System;
using System.Collections.Generic;
using aima.core.agent;
using aima.core.agent.impl;
using aima.core.agent.impl.aprog.simplerule;

namespace aima.core.agent.impl.aprog
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.10, page
     * 49.<br>
     * <br>
     * 
     * <pre>
     * function SIMPLE-RELEX-AGENT(percept) returns an action
     *   persistent: rules, a set of condition-action rules
     *        
     * state  <- INTERPRET-INPUT(percept);
     * rule   <- RULE-MATCH(state, rules);
     * action <- rule.ACTION;
     * return action
     * </pre>            
     * Figure 2.10 A simple reflex agent. It acts according to a rule whose
     * condition matches the current state, as defined by the percept.
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     * 
     */
     public class SimpleReflexAgentProgram : AgentProgram
    {
        // persistent: rules, a set of condition-action rules
        private HashSet<Rule> rules;

        /**
	     * Constructs a SimpleReflexAgentProgram with a set of condition-action
	     * rules.
	     * 
	     * @param ruleSet
	     *            a set of condition-action rules
	     */
         public SimpleReflexAgentProgram(HashSet<Rule> ruleSet)
        {
            rules = ruleSet;
        }

        // START-AgentProgram

        // function SIMPLE-RELEX-AGENT(percept) returns an action
        public Action execute(Percept percept)
        {
            // state <- INTERPRET-INPUT(percept);
            ObjectWithDynamicAttributes state = interpretInput(percept);
            // rule <- RULE-MATCH(state, rules);
            Rule rule = ruleMatch(state, rules);
            // action <- rule.ACTION;
            // return action
            return ruleAction(rule);
        }

        // END-AgentProgram

        // PROTECTED METHODS

        protected ObjectWithDynamicAttributes interpretInput(Percept p)
        {
            return (DynamicPercept)p;
        }

        protected Rule ruleMatch(ObjectWithDynamicAttributes state,
                HashSet<Rule> rulesSet)
        {
            foreach (Rule r in rulesSet)
            {
                if (r.evaluate(state))
                {
                    return r;
                }
            }
            return null;
        }

        protected Action ruleAction(Rule r)
        {
            return null == r ? NoOpAction.NO_OP : r.getAction();
        }
    }
}