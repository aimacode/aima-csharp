using System.Collections.Generic;
using aima.core.agent;
using aima.core.search.framework.problem;
using aima.core.util;

namespace aima.core.environment.eightpuzzle
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class EightPuzzleFunctionFactory
    {
	private static ActionsFunction _actionsFunction = null;
	private static ResultFunction _resultFunction = null;

	public static ActionsFunction getActionsFunction()
	{
	    if (null == _actionsFunction)
	    {
		_actionsFunction = new EPActionsFunction();
	    }
	    return _actionsFunction;
	}

	public static ResultFunction getResultFunction()
	{
	    if (null == _resultFunction)
	    {
		_resultFunction = new EPResultFunction();
	    }
	    return _resultFunction;
	}

	private class EPActionsFunction : ActionsFunction
	{
	    public HashSet<Action> actions(System.Object state)
	    {
		EightPuzzleBoard board = (EightPuzzleBoard)state;

		HashSet<Action> actions = new HashSet<Action>();

		if (board.canMoveGap(EightPuzzleBoard.UP))
		{
		    actions.Add(EightPuzzleBoard.UP);
		}
		if (board.canMoveGap(EightPuzzleBoard.DOWN))
		{
		    actions.Add(EightPuzzleBoard.DOWN);
		}
		if (board.canMoveGap(EightPuzzleBoard.LEFT))
		{
		    actions.Add(EightPuzzleBoard.LEFT);
		}
		if (board.canMoveGap(EightPuzzleBoard.RIGHT))
		{
		    actions.Add(EightPuzzleBoard.RIGHT);
		}

		return actions;
	    }
	}

	private class EPResultFunction : ResultFunction
	{
	    public System.Object result(System.Object s, Action a)
	    {
		EightPuzzleBoard board = (EightPuzzleBoard) s;

		if (EightPuzzleBoard.UP.Equals(a)
			&& board.canMoveGap(EightPuzzleBoard.UP))
		{
		    EightPuzzleBoard newBoard = new EightPuzzleBoard(board);
		    newBoard.moveGapUp();
		    return newBoard;
		}
		else if (EightPuzzleBoard.DOWN.Equals(a)
		      && board.canMoveGap(EightPuzzleBoard.DOWN))
		{
		    EightPuzzleBoard newBoard = new EightPuzzleBoard(board);
		    newBoard.moveGapDown();
		    return newBoard;
		}
		else if (EightPuzzleBoard.LEFT.Equals(a)
		      && board.canMoveGap(EightPuzzleBoard.LEFT))
		{
		    EightPuzzleBoard newBoard = new EightPuzzleBoard(board);
		    newBoard.moveGapLeft();
		    return newBoard;
		}
		else if (EightPuzzleBoard.RIGHT.Equals(a)
		      && board.canMoveGap(EightPuzzleBoard.RIGHT))
		{
		    EightPuzzleBoard newBoard = new EightPuzzleBoard(board);
		    newBoard.moveGapRight();
		    return newBoard;
		}

		// The Action is not understood or is a NoOp
		// the result will be the current state.
		return s;
	    }
	}
    }
}