using Action = aima.core.agent.Action;

namespace aima.core.search.framework
{
    /// <summary>
    ///     Represents a node in a search tree which corresponds to a state in a state space.
    /// </summary>
    /// <remarks>
    ///     Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.10, page 79.
    ///     - Nodes are the data structures from which the search tree is constructed.
    ///     - Each node has a parent, a state, and various bookkeeping fields.
    ///     - Arrows point from child to parent.
    ///     <para />
    ///     @author Ravi Mohan
    ///     @author Ciaran O'Reilly
    ///     @author Mike Stampone
    /// </remarks>
    public class Node
    {
        /// <summary>
        ///     The action that was applied to the parent to generate this node.
        /// </summary>
        public Action Action { get; }

        /// <summary>
        ///     The node in the search tree that generated this node.
        /// </summary>
        public Node Parent { get; }

        /// <summary>
        ///     The cumulative cost, traditionally denoted by g(n), of the path from the initial state to this node (as indicated by the parent pointers).
        /// </summary>
        public double PathCost { get; }

        /// <summary>
        ///     The state in the state space to which this node corresponds.
        /// </summary>
        public object State { get; }

        /// <summary>
        ///     Creates a new instance of <see cref="Node" /> with the specified state.
        /// </summary>
        public Node(object state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            State = state;
            PathCost = 0.0;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="Node" /> with the specified state, parent, agent.Action, and step cost.
        ///     <para>
        ///         The step cost is the cost from the parent node to this node.
        ///     </para>
        /// </summary>
        public Node(object state, Node parent, agent.Action action, double stepCost) : this(state)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Parent = parent;
            Action = action;
            PathCost = parent.PathCost + stepCost;
        }

        /// <summary>
        ///     Returns the path from the root node to this node.
        /// </summary>
        public List<Node> GetPathFromRoot()
        {
            var path = new List<Node>();
            var current = this;

            while (true)
            {
                path.Insert(0, current);

                if (current.IsRoot())
                {
                    break;
                }

                current = current.Parent;
            }

            return path;
        }

        /// <summary>
        ///     Returns <see langword="true" /> if this node has no parent node; otherwise, <see langword="false" />.
        /// </summary>
        public bool IsRoot()
        {
            return Parent == null;
        }

        public override string ToString()
        {
            return string.Join(" <-- ", GetPathFromRoot().Select(x => $"[action={Action}, state={State}, pathCost={PathCost}]"));
        }
    }
}