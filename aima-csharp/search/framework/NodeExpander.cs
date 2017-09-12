using System.Collections.Generic;
using aima.core.agent;
using aima.core.search.framework.problem;

namespace aima.core.search.framework
{
    /**
     * Instances of this class are responsible for node creation and expansion. They
     * compute path costs, support progress tracing, and count the number of
     * {@link #expand(Node, Problem)} calls.
     * 
     * @author Ruediger Lunde
     *
     */
    public class NodeExpander
    {
        // expanding nodes

        public Node createRootNode(System.Object state)
        {
            return new Node(state);
        }

        /**
	 * Computes the path cost for getting from the root node state via the
	 * parent node state to the specified state, creates a new node for the
	 * specified state, adds it as child of the provided parent, and returns it.
	 */
         public Node createNode(System.Object state, Node parent, Action action, double stepCost)
        {
            return new Node(state, parent, action, parent.PathCost + stepCost);
        }

        /**
	 * Returns the children obtained from expanding the specified node in the
    	 * specified problem.
    	 * 
    	 * @param node
    	 *            the node to expand
    	 * @param problem
    	 *            the problem the specified node is within.
    	 * 
    	 * @return the children obtained from expanding the specified node in the
    	 *         specified problem.
    	 */
        public List<Node> expand(Node node, Problem problem)
        {
            List<Node> successors = new List<Node>();

            ActionsFunction actionsFunction = problem.getActionsFunction();
            ResultFunction resultFunction = problem.getResultFunction();
            StepCostFunction stepCostFunction = problem.getStepCostFunction();

            foreach (Action action in actionsFunction.actions(node.State))
            {
                System.Object successorState = resultFunction.result(node.State, action);

                double stepCost = stepCostFunction.c(node.State, action, successorState);
                successors.Add(createNode(successorState, node, action, stepCost));
            }

            foreach (NodeListener listener in nodeListeners)
            {
                listener.onNodeExpanded(node);
            }
            counter++;
            return successors;
        }

        // progress tracing and statistical data

        /** Interface for progress Tracers */
        public interface NodeListener
        {
            void onNodeExpanded(Node node);
        }

        /**
	 * All node listeners added to this list get informed whenever a node is
	 * expanded.
	 */
        private List<NodeListener> nodeListeners = new List<NodeListener>();

        /** Counts the number of {@link #expand(Node, Problem)} calls. */
        private int counter;

        /**
    	 * Adds a listener to the list of node listeners. It is informed whenever a
	 * node is expanded during search.
	 */
        public void addNodeListener(NodeListener listener)
        {
            nodeListeners.Add(listener);
        }

        /**
    	 * Resets the counter for {@link #expand(Node, Problem)} calls.
    	 */
        public void resetCounter()
        {
            counter = 0;
        }

        /**
	 * Returns the number of {@link #expand(Node, Problem)} calls since the last
	 * counter reset.
	 */
        public int getNumOfExpandCalls()
        {
            return counter;
        }
    }
}