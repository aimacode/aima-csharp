using aima.core.agent;
using aima.core.agent.impl;
using aima.core.search.framework;
using aima.core.util;

namespace aima.core.search.online
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 4.21, page
     * 150.<br>
     * <br>
     * 
     * <pre>
     * function ONLINE-DFS-AGENT(s') returns an action
     *   inputs: s', a percept that identifies the current state
     *   persistent: result, a table, indexed by state and action, initially empty
     *               untried, a table that lists, for each state, the actions not yet tried
     *               unbacktracked, a table that lists, for each state, the backtracks not yet tried
     *               s, a, the previous state and action, initially null
     *    
     *   if GOAL-TEST(s') then return stop
     *   if s' is a new state (not in untried) then untried[s'] &lt;- ACTIONS(s')
     *   if s is not null then
     *       result[s, a] &lt;- s'
     *       add s to the front of the unbacktracked[s']
     *   if untried[s'] is empty then
     *       if unbacktracked[s'] is empty then return stop
     *       else a &lt;- an action b such that result[s', b] = POP(unbacktracked[s'])
     *   else a &lt;- POP(untried[s'])
     *   s &lt;- s'
     *   return a
     * </pre>
     * 
     * Figure 4.21 An online search agent that uses depth-first exploration. The
     * agent is applicable only in state spaces in which every action can be
     * "undone" by some other action.<br>
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class OnlineDFSAgent : AbstractAgent
    {
	private OnlineSearchProblem problem;
	private PerceptToStateFunction ptsFunction;
	private readonly TwoKeyHashMap<object, agent.Action, object> result = new TwoKeyHashMap<object, agent.Action, object>();
	// untried, a table that lists, for each state, the actions not yet tried
	private readonly Dictionary<object, List<agent.Action>> untried = new Dictionary<object, List<agent.Action>>();
	// unbacktracked, a table that lists,
	// for each state, the backtracks not yet tried
	private readonly Dictionary<object, List<object>> unbacktracked = new Dictionary<object, List<object>>();
	// s, a, the previous state and action, initially null
	private object s = null;
	private agent.Action a = null;

	/**
	 * Constructs an online DFS agent with the specified search problem and
	 * percept to state function.
	 * 
	 * @param problem
	 *            an online search problem for this agent to solve
	 * @param ptsFunction
	 *            a function which returns the problem state associated with a
	 *            given Percept.
	 */
	public OnlineDFSAgent(OnlineSearchProblem problem,
			PerceptToStateFunction ptsFunction)
	{
	    setProblem(problem);
	    setPerceptToStateFunction(ptsFunction);
	}

	/**
	 * Returns the search problem for this agent.
	 * 
	 * @return the search problem for this agent.
	 */
	public OnlineSearchProblem getProblem()
	{
	    return problem;
	}

	/**
	 * Sets the search problem for this agent to solve.
	 * 
	 * @param problem
	 *            the search problem for this agent to solve.
	 */
	public void setProblem(OnlineSearchProblem problem)
	{
	    this.problem = problem;
	    init();
	}

	/**
	 * Returns the percept to state function of this agent.
	 * 
	 * @return the percept to state function of this agent.
	 */
	public PerceptToStateFunction getPerceptToStateFunction()
	{
	    return ptsFunction;
	}

	/**
	 * Sets the percept to state functino of this agent.
	 * 
	 * @param ptsFunction
	 *            a function which returns the problem state associated with a
	 *            given Percept.
	 */
	public void setPerceptToStateFunction(PerceptToStateFunction ptsFunction)
	{
	    this.ptsFunction = ptsFunction;
	}

	// function ONLINE-DFS-AGENT(s') returns an action
	// inputs: s', a percept that identifies the current state
	public override core.agent.Action execute(Percept psDelta)
	{
	    object sDelta = ptsFunction.getState(psDelta);
	    // if GOAL-TEST(s') then return stop
	    if (goalTest(sDelta))
	    {
		a = NoOpAction.NO_OP;
	    }
	    else
	    {
		// if s' is a new state (not in untried) then untried[s'] <-
		// ACTIONS(s')
		if (!untried.ContainsKey(sDelta))
		{
		    untried.Add(sDelta, actions(sDelta));
		}

		// if s is not null then do
		if (null != s)
		{
		    // Note: If I've already seen the result of this
		    // [s, a] then don't put it back on the unbacktracked
		    // list otherwise you can keep oscillating
		    // between the same states endlessly.
		    if (!(sDelta.Equals(result.get(s, a))))
		    {
			// result[s, a] <- s'
			result.put(s, a, sDelta);

			// Ensure the unbacktracked always has a list for s'
			if (!unbacktracked.ContainsKey(sDelta))
			{
			    unbacktracked.Add(sDelta, new List<object>());
			}

			// add s to the front of the unbacktracked[s']
			unbacktracked[sDelta].Add(s);
		    }
		}

		// if untried[s'] is empty then
		if (untried[sDelta].Capacity == 0)
		{
		    // if unbacktracked[s'] is empty then return stop
		    if (unbacktracked[sDelta].Capacity == 0)
		    {
			a = NoOpAction.NO_OP;
		    }
		    else
		    {
			// else a <- an action b such that result[s', b] =
			// POP(unbacktracked[s'])
			object popped = unbacktracked[sDelta].Remove(0);
			foreach(Pair<object, agent.Action> sa in result.Keys)
			{
			    if (sa.getFirst().Equals(sDelta) && result[sa].Equals(popped))
			    {
				a = sa.getSecond();
				break;
			    }
			}
		    }
		}
		else //Needs debugging.
		{
		    // else a <- POP(untried[s'])
		    //a = untried[sDelta].Remove(0);
		}
	    }

	    if (a.isNoOp())
	    {
		// I'm either at the Goal or can't get to it,
		// which in either case I'm finished so just die.
		setAlive(false);
	    }

	    // s <- s'
	    s = sDelta;
	    // return a
	    return a;
	}

	// PRIVATE METHODS

	private void init()
	{
	    setAlive(true);
	    result.Clear();
	    untried.Clear();
	    unbacktracked.Clear();
	    s = null;
	    a = null;
	}

	private bool goalTest(object state)
	{
	    return getProblem().isGoalState(state);
	}

	private List<agent.Action> actions(object state)
	{
	    return new List<agent.Action>(problem.getActionsFunction()
			    .actions(state));
	}
    }
}