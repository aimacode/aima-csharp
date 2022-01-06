using aima.core.search.framework.problem;

namespace aima.core.environment.map
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class BidirectionalMapProblem : Problem, BidirectionalProblem
    {
	Map map;

	Problem reverseProblem;

	public BidirectionalMapProblem(Map map, String initialState, String goalState): this(map, initialState, goalState, new DefaultGoalTest(goalState))
	{
	    
	}

	public BidirectionalMapProblem(Map map, String initialState, String goalState, GoalTest goalTest) :  base(initialState, MapFunctionFactory.getActionsFunction(map), MapFunctionFactory.getResultFunction(),
			    goalTest, new MapStepCostFunction(map))
	{ 
	    this.map = map;

	    reverseProblem = new Problem(goalState, MapFunctionFactory.getReverseActionsFunction(map),
			    MapFunctionFactory.getResultFunction(), new DefaultGoalTest(initialState),
			    new MapStepCostFunction(map));
	}

	// START Interface BidrectionalProblem
	public Problem getOriginalProblem()
	{
	    return this;
	}

	public Problem getReverseProblem()
	{
	    return reverseProblem;
	}
	// END Interface BirectionalProblem
    }
}