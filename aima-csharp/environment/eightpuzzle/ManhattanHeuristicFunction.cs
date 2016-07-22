using System;
using System.Collections.Generic;
using aima.core.search.framework;
using aima.core.util;

namespace aima.core.environment.eightpuzzle
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class ManhattanHeuristicFunction : HeuristicFunction
    {
	public double h(Object state)
	{
	    EightPuzzleBoard board = (EightPuzzleBoard)state;
	    int retVal = 0;
	    for (int i = 1; i < 9; i++)
	    {
		XYLocation loc = board.getLocationOf(i);
		retVal += evaluateManhattanDistanceOf(i, loc);
	    }
	    return retVal;
	}

	public int evaluateManhattanDistanceOf(int i, XYLocation loc)
	{
	    int retVal = -1;
	    int xpos = loc.getXCoOrdinate();
	    int ypos = loc.getYCoOrdinate();
	    switch (i)
	    {

		case 1:
		    retVal = Math.Abs(xpos - 0) + Math.Abs(ypos - 1);
		    break;
		case 2:
		    retVal = Math.Abs(xpos - 0) + Math.Abs(ypos - 2);
		    break;
		case 3:
		    retVal = Math.Abs(xpos - 1) + Math.Abs(ypos - 0);
		    break;
		case 4:
		    retVal = Math.Abs(xpos - 1) + Math.Abs(ypos - 1);
		    break;
		case 5:
		    retVal = Math.Abs(xpos - 1) + Math.Abs(ypos - 2);
		    break;
		case 6:
		    retVal = Math.Abs(xpos - 2) + Math.Abs(ypos - 0);
		    break;
		case 7:
		    retVal = Math.Abs(xpos - 2) + Math.Abs(ypos - 1);
		    break;
		case 8:
		    retVal = Math.Abs(xpos - 2) + Math.Abs(ypos - 2);
		    break;

	    }
	    return retVal;
	}
    }
}