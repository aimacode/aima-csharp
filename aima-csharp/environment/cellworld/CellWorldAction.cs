using System.Collections.Generic;
using aima.core.agent;
using aima.core.util;

namespace aima.core.environment.cellworld
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 645.<br>
     * <br>
     * 
     * The actions in every state are Up, Down, Left, and Right.<br>
     * <br>
     * <b>Note:<b> Moving 'North' causes y to increase by 1, 'Down' y to decrease by
     * 1, 'Left' x to decrease by 1, and 'Right' x to increase by 1 within a Cell
     * World.
     * 
     * @author Ciaran O'Reilly
     * 
     */
     public class CellWorldAction : Action
    {
	public CellWorldAction()
	{

	}

	public CellWorldAction(ActionEnum action)
	{
	    this._action = action;
	}


	public enum ActionEnum
	{
	    Up
	    ,
	    Down
	    ,
	    Left
	    ,
	    Right
	    ,
	    None
	}

	private ActionEnum _action;

	private static readonly LinkedHashSet<ActionEnum> _actions = new LinkedHashSet<ActionEnum>();


	static CellWorldAction()
	{
	    _actions.Add(ActionEnum.Up);
	    _actions.Add(ActionEnum.Down);
	    _actions.Add(ActionEnum.Left);
	    _actions.Add(ActionEnum.Right);
	    _actions.Add(ActionEnum.None);
	}

	/**
	 * 
	 * @return a set of the actual actions.
	 */
	public static LinkedHashSet<ActionEnum> actions()
	{
	    return _actions;
	}

	// START-Action

	public bool isNoOp()
	{
	    if (this._action == ActionEnum.None)
	    {
		return true;
	    }
	    return false;
	}

	// END-Action
	
	/**
	 * 
	 * @param curX
	 *            the current x position.
	 * @return the result on the x position of applying this action.
	 */
	public int getXResult(int curX)
	{
	    int newX = curX;

	    switch (this._action)
	    {
		case ActionEnum.Left:
		    newX--;
		    break;
		case ActionEnum.Right:
		    newX++;
		    break;
	    }
	    return newX;
	}

	/**
	 * 
	 * @param curY
	 *            the current y position.
	 * @return the result on the y position of applying this action.
	 */
	public int getYResult(int curY)
	{
	    int newY = curY;

	    switch (this._action)
	    {
		case ActionEnum.Up:
		    newY++;
		    break;
		case ActionEnum.Down:
		    newY--;
		    break;
	    }
	    return newY;
	}

	/**
	 * 
	 * @return the first right angled action related to this action.
	 */
	public ActionEnum getFirstRightAngledAction()
	{
	    ActionEnum a = ActionEnum.None;

	    switch (this._action)
	    {
		case ActionEnum.Up:
		case ActionEnum.Down:
		    a = ActionEnum.Left;
		    break;
		case ActionEnum.Left:
		case ActionEnum.Right:
		    a = ActionEnum.Down;
		    break;
		case ActionEnum.None:
		    a = ActionEnum.None;
		    break;
	    }
	    return a;
	}

	/**
	 * 
	 * @return the second right angled action related to this action.
	 */
	public ActionEnum getSecondRightAngledAction()
	{
	    ActionEnum a = ActionEnum.None;

	    switch (this._action)
	    {
		case ActionEnum.Up:
		case ActionEnum.Down:
		    a = ActionEnum.Right;
		    break;
		case ActionEnum.Left:
		case ActionEnum.Right:
		    a = ActionEnum.Up;
		    break;
		case ActionEnum.None:
		    a = ActionEnum.None;
		    break;
	    }
	    return a;
	}
    }
}