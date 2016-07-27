using System;
using System.Collections.Generic;
using aima.core.search.framework;

namespace aima.core.environment.wumpusworld
{
    /**
     * Heuristic for calculating the Manhattan distance between two rooms within a Wumpus World cave.
     * 
     * @author Federico Baron
     * @author Alessandro Daniele
     * @author Ciaran O'Reilly
     */
    public class ManhattanHeuristicFunction : HeuristicFunction
    {
	List<Room> goals = new List<Room>();

	public ManhattanHeuristicFunction(HashSet<Room> goals)
	{
	    this.goals.AddRange(goals);
	}

	public double h(Object state)
	{
	    AgentPosition pos = (AgentPosition)state;
	    int nearestGoalDist = int.MaxValue;
	    foreach (Room g in goals)
	    {
		int tmp = evaluateManhattanDistanceOf(pos.getX(), pos.getY(), g.getX(), g.getY());

		if (tmp < nearestGoalDist)
		{
		    nearestGoalDist = tmp;
		}
	    }
	    return nearestGoalDist;
	}

	// PRIVATE

	private int evaluateManhattanDistanceOf(int x1, int y1, int x2, int y2)
	{
	    return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
	}
    }
}