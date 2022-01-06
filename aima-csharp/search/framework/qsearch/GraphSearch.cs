using aima.core.agent;
using aima.core.search.framework.problem;

namespace aima.core.search.framework.qsearch
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.7, page 77.
     * <br>
     * 
     * <pre>
     * function GRAPH-SEARCH(problem) returns a solution, or failure
     *   initialize the frontier using the initial state of problem
     *   initialize the explored set to be empty
     *   loop do
     *     if the frontier is empty then return failure
     *     choose a leaf node and remove it from the frontier
     *     if the node contains a goal state then return the corresponding solution
     *     add the node to the explored set
     *     expand the chosen node, adding the resulting nodes to the frontier
     *       only if not in the frontier or explored set
     * </pre>
     * 
     * Figure 3.7 An informal description of the general graph-search algorithm.
     * <br>
     * This implementation is based on the template method
     * {@link #search(Problem, Queue)} from superclass {@link QueueSearch} and
     * provides implementations for the needed primitive operations. In contrast to
     * the code above, here, nodes resulting from node expansion are added to the
     * frontier even if nodes with equal states already exist there. This makes it
     * possible to use the implementation also in combination with priority queue
     * frontiers.
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     */
    public class GraphSearch : QueueSearch
    {
	private HashSet<object> explored = new HashSet<object>();

	public GraphSearch() : this(new NodeExpander())
	{

	}

	public GraphSearch(NodeExpander nodeExpander): base(nodeExpander)
	{
	    
	}

	/**
	 * Clears the set of explored states and calls the search implementation of
	 * <code>QueSearch</code>
	 */	
	public override List<agent.Action> search(Problem problem, Queue<Node> frontier)
	{
	    // initialize the explored set to be empty
	    explored.Clear();
	    // expandedNodes = new List<Node>();
	    return base.search(problem, frontier);
	}

	/**
	 * Inserts the node at the tail of the frontier if the corresponding state
	 * was not yet explored.
	 */
	protected override void addToFrontier(Node node)
	{
	    if (!explored.Contains(node.State))
	    {
		frontier.Enqueue(node);
		updateMetrics(frontier.Count);
	    }
	}

	/**
	 * Removes the node at the head of the frontier, adds the corresponding
	 * state to the explored set, and returns the node. As the template method
	 * (the caller) calls {@link #isFrontierEmpty() before, the resulting node
	 * state will always be unexplored yet.
	 * 
	 * @return the node at the head of the frontier.
	 */
	protected override Node removeFromFrontier()
	{
	    Node result = frontier.Dequeue();
	    // add the node to the explored set
	    explored.Add(result.State);
	    updateMetrics(frontier.Count);
	    return result;
	}

	/**
	 * Pops nodes of already explored states from the top end of the frontier
	 * and checks whether there are still some nodes left.
	 */
	protected override bool isFrontierEmpty()
	{
	    while (!(frontier.Count==0) && explored.Contains(frontier.Peek().State))
		{
		    frontier.Dequeue();
		}
	    updateMetrics(frontier.Count);
	    if (frontier.Count == 0)
	    {
		return true;
	    }
	    else
		return false;
	}	
    }
}