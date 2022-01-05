using aima.core.agent;
using aima.core.search.framework.problem;

namespace aima.core.environment.map
{
    /**
     * Implementation of StepCostFunction interface that uses the distance between
     * locations to calculate the cost in addition to a constant cost, so that it
     * may be used in conjunction with a Uniform-cost search.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class MapStepCostFunction : StepCostFunction
    {
	private Map map = null;
		
	// Used by Uniform-cost search to ensure every step is greater than or equal
	// to some small positive constant
	private static double constantCost = 1.0;

	public MapStepCostFunction(Map map)
	{
	    this.map = map;
	}

	//
	// START-StepCostFunction
	public double c(object fromCurrentState, agent.Action action, object toNextState)
	{

	    string fromLoc = fromCurrentState.ToString();
	    string toLoc = toNextState.ToString();

	    double distance = map.getDistance(fromLoc, toLoc);

	    if (distance == null || distance <= 0)
	    {
		return constantCost;
	    }

	    return distance;
	}

	// END-StepCostFunction
    }
}