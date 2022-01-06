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
    public class MapAgent : SimpleProblemSolvingAgent
    {
	protected Map map = null;
	protected DynamicState state = new DynamicState();

	private EnvironmentViewNotifier notifier = null;
	private Search _search = null;
	private String[] goals = null;
	private int goalTestPos = 0;

	public MapAgent(Map map, EnvironmentViewNotifier notifier, Search search)
	{
	    this.map = map;
	    this.notifier = notifier;
	    this._search = search;
	}

	public MapAgent(Map map, EnvironmentViewNotifier notifier, Search search,
			int maxGoalsToFormulate): base(maxGoalsToFormulate)
	{	    
	    this.map = map;
	    this.notifier = notifier;
	    this._search = search;
	}

	public MapAgent(Map map, EnvironmentViewNotifier notifier, Search search,
			String[] goals): base(goals.Length)
	{	    
	    this.map = map;
	    this.notifier = notifier;
	    this._search = search;
	    this.goals = new String[goals.Length];
	    Array.Copy(goals, 0, this.goals, 0, goals.Length);
	}

	// PROTECTED METHODS
	
	protected override State updateState(Percept p)
	{
	    DynamicPercept dp = (DynamicPercept)p;

	    state.setAttribute(DynAttributeNames.AGENT_LOCATION,
			    dp.getAttribute(DynAttributeNames.PERCEPT_IN));
	    return state;
	}

	protected override Object formulateGoal()
	{
	    Object goal = null;
	    if (null == goals)
	    {
		goal = map.randomlyGenerateDestination();
	    }
	    else
	    {
		goal = goals[goalTestPos];
		goalTestPos++;
	    }
	    notifier.notifyViews("CurrentLocation=In("
			    + state.getAttribute(DynAttributeNames.AGENT_LOCATION)
			    + "), Goal=In(" + goal + ")");
	    return goal;
	}

	protected override Problem formulateProblem(Object goal)
	{
	    return new BidirectionalMapProblem(map,
			    (String)state.getAttribute(DynAttributeNames.AGENT_LOCATION),
			    (String)goal);
	}
	
	protected override List<agent.Action> search(Problem problem)
	{
	    List<agent.Action> actions = new List<agent.Action>();
	    try
	    {
		List<agent.Action> sactions = _search.search(problem);
		foreach (agent.Action action in sactions)
		{
		    actions.Add(action);
		}
	    }
	    catch (Exception ex)
	    {
		System.Diagnostics.Debug.WriteLine(ex.ToString());
	    }
	    return actions;
	}

	protected override void notifyViewOfMetrics()
	{
	    HashSet<String> keys = _search.getMetrics().keySet();
	    foreach (String key in keys)
	    {
		notifier.notifyViews("METRIC[" + key + "]="
				+ _search.getMetrics().get(key));
	    }
	}

    }
}