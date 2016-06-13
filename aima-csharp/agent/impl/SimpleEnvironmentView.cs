using System;
using System.Collections.Generic;
using aima.core.agent;

namespace aima.core.agent.impl
{
    /**
     * Simple environment view which uses the standard output stream to inform about
     * relevant events.
     * 
     * @author Ruediger Lunde
     */
    public class SimpleEnvironmentView : EnvironmentView
    {
        public void agentActed(Agent agent, Action action, EnvironmentState resultingState)
        {
            System.Console.WriteLine("Agent acted: " + action.ToString());
        }

        public void agentAdded(Agent agent, EnvironmentState resultingState)
        {
            System.Console.WriteLine("Agent added.");
        }

        public void notify(string msg)
        {
            System.Console.WriteLine("Message: " + msg);
        }
    }
}