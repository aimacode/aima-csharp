namespace aima.core.search.framework.qsearch
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.7, page 77.
     * <br>
     * 
     * <pre>
     * function TREE-SEARCH(problem) returns a solution, or failure
     *   initialize the frontier using the initial state of the problem
     *   loop do
     *     if the frontier is empty then return failure
     *     choose a leaf node and remove it from the frontier
     *     if the node contains a goal state then return the corresponding solution
     *     expand the chosen node, adding the resulting nodes to the frontier
     * </pre>
     * 
     * Figure 3.7 An informal description of the general tree-search algorithm.
     * 
     * <br>
     * This implementation is based on the template method
     * {@link #search(Problem, Queue)} from superclass {@link QueueSearch} and
     * provides implementations for the needed primitive operations.
     * 
     * @author Ravi Mohan
     * @author Ruediger Lunde
     * 
     */
    public class TreeSearch : QueueSearch
    {
	public TreeSearch(): this(new NodeExpander())
	{

	}

	public TreeSearch(NodeExpander nodeExpander): base(nodeExpander)
	{
	    
	}

	/**
	 * Inserts the node at the tail of the frontier.
	 */	
	protected override void addToFrontier(Node node)
	{
	    frontier.Enqueue(node);
	    updateMetrics(frontier.Count);
	}

	/**
	 * Removes and returns the node at the head of the frontier.
	 * 
	 * @return the node at the head of the frontier.
	 */
	protected override Node removeFromFrontier()
	{
	    Node result = frontier.Dequeue();
	    updateMetrics(frontier.Count);
	    return result;
	}

	/**
	 * Checks whether the frontier contains not yet expanded nodes.
	 */
	protected override bool isFrontierEmpty()
	{
	    if(frontier.Count == 0)
	    {
		return true;
	    }
	    else
		return false;
	}
    }
}