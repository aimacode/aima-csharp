using aima.core.util;

namespace aima.core.environment.map
{
    /**
     * @author Ruediger Lunde
     */
    public class StraightLineDistanceHeuristicFunction : AdaptableHeuristicFunction
    {
	public StraightLineDistanceHeuristicFunction(Object goal, Map map)
	{
	    this.goal = goal;
	    this.map = map;
	}

	public double h(Object state)
	{
	    Double result = 0.0;
	    Point2D pt1 = map.getPosition((String)state);
	    Point2D pt2 = map.getPosition((String)goal);
	    if (pt1 != null && pt2 != null)
	    {
		result = pt1.distance(pt2);
	    }
	    return result;
	}
    }
}