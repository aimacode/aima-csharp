using System;
using System.Collections.Generic;

namespace aima.core.search.framework.problem
{
    /**
     * Checks whether a given state equals an explicitly specified goal state.
     * 
     * @author Ruediger Lunde
     */
    public class DefaultGoalTest : GoalTest
    {

    private Object goalState;

    public DefaultGoalTest(Object goalState)
    {
        this.goalState = goalState;
    }

    public bool isGoalState(Object state)
    {
        return goalState.Equals(state);
    }
}
}