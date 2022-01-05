using aima.core.agent;

namespace aima.core.search.framework.problem
{
    /**
     * Returns one for every action.
     * 
     * @author Ravi Mohan
     */
    public class DefaultStepCostFunction : StepCostFunction
    {


        public double c(System.Object stateFrom, agent.Action action, System.Object stateTo)
        {
            return 1;
        }
    }
}