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
        public void agentActed(Agent agent, agent.Action action, EnvironmentState resultingState)
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