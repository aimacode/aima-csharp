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