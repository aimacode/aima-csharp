using aima.core.agent;
using aima.core.agent.impl;
using aima.core.search.framework.problem;

namespace aima.core.search.framework
{
    /**
     * Provides several useful static methods for implementing search.
     * 
     * @author Ravi Mohan
     * @author Ruediger Lunde
     * 
     */
    public class SearchUtils
    {
        /**
	 * Returns the list of actions corresponding to the complete path to the
	 * given node or NoOp if path length is one.
	 */    
        public static List<agent.Action> getSequenceOfActions(Node node)
        {
            List<Node> nodes = node.GetPathFromRoot();
            List<agent.Action> actions = new List<agent.Action>();

            if(nodes.Count == 1)
            {
                // I'm at the root node, this indicates I started at the
                // Goal node, therefore just return a NoOp
                actions.Add(NoOpAction.NO_OP);
            }
            else
            {
                // ignore the root node this has no action
                // hence index starts from 1 not zero
                for (int i = 1; i < nodes.Count; i++)
                {
                    Node node_temp = nodes[i];
                    actions.Add(node_temp.Action);
                }
            }
            return actions;
        }

        /** Returns an empty action list. */
        public static List<agent.Action> failure()
        {
            return new List<agent.Action>();
        }

        /** Checks whether a list of actions is empty. */
        public static bool isFailure(List<agent.Action> actions)
        {
            if(actions.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }      

        }
        

        /**
	 * Calls the goal test of the problem and - if the goal test is effectively
	 * a {@link SolutionChecker} - additionally checks, whether the solution is
	 * acceptable. Solution checkers can be used to analyze several or all
	 * solutions with only one search run.
	 */
        public static bool isGoalState(Problem p, Node n)
        {
            bool isGoal = false;
            GoalTest gt = p.getGoalTest();
            if (gt.isGoalState(n.State))
            {
                if (gt is SolutionChecker)
                {
                    isGoal = ((SolutionChecker)gt).isAcceptableSolution(
                            getSequenceOfActions(n), n.State);
                }
                else
                {
                    isGoal = true;
                }
            }
            return isGoal;
        }
    }
}