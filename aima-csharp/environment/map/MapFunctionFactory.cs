using aima.core.agent;
using aima.core.agent.impl;
using aima.core.search.framework;
using aima.core.search.framework.problem;

namespace aima.core.environment.map
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class MapFunctionFactory
    {
	private static ResultFunction resultFunction;
	private static PerceptToStateFunction perceptToStateFunction;

	public static ActionsFunction getActionsFunction(Map map)
	{
	    return new MapActionsFunction(map, false);
	}

	public static ActionsFunction getReverseActionsFunction(Map map)
	{
	    return new MapActionsFunction(map, true);
	}

	public static ResultFunction getResultFunction()
	{
	    if (null == resultFunction)
	    {
		resultFunction = new MapResultFunction();
	    }
	    return resultFunction;
	}

	private class MapActionsFunction : ActionsFunction
	{
	    private Map map = null;
	    private bool reverseMode;

	    public MapActionsFunction(Map map, bool reverseMode)
	    {
		this.map = map;
		this.reverseMode = reverseMode;
	    }

	    public HashSet<agent.Action> actions(System.Object state)
	    {
		HashSet<agent.Action> actions = new HashSet<agent.Action>();
		System.String location = state.ToString();

		List<System.String> linkedLocations = reverseMode ? map.getPossiblePrevLocations(location)
					: map.getPossibleNextLocations(location);
		foreach (System.String linkLoc in linkedLocations)
		{
		    actions.Add(new MoveToAction(linkLoc));
		}
		return actions;
	    }
	}

	public static PerceptToStateFunction getPerceptToStateFunction()
	{
	    if (null == perceptToStateFunction)
	    {
		perceptToStateFunction = new MapPerceptToStateFunction();
	    }
	    return perceptToStateFunction;
	}

	private class MapResultFunction : ResultFunction
	{
	    public MapResultFunction()
	    {
	    }

	    public System.Object result(System.Object s, agent.Action a)
	    {

		if (a is MoveToAction)
		{
		    MoveToAction mta = (MoveToAction)a;

		    return mta.getToLocation();
		}

		// The Action is not understood or is a NoOp
		// the result will be the current state.
		return s;
	    }
	}

	private class MapPerceptToStateFunction :
		PerceptToStateFunction
	{
	    public System.Object getState(Percept p)
	    {
		return ((DynamicPercept)p)
			.getAttribute(DynAttributeNames.PERCEPT_IN);
	    }
	}
    }
}