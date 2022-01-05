using aima.core.util;

namespace aima.core.environment.cellworld
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 645.<br>
     * <br>
     * 
     * A representation for the environment depicted in figure 17.1.<br>
     * <br>
     * <b>Note:<b> the x and y coordinates are always positive integers starting at
     * 1.<br>
     * <b>Note:<b> If looking at a rectangle - the coordinate (x=1, y=1) will be the
     * bottom left hand corner.<br>
     * 
     * 
     * @param <C>
     *            the type of content for the Cells in the world.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class CellWorld<C>
    {
	private LinkedHashSet<Cell<C>> cells = new LinkedHashSet<Cell<C>>();
	private Dictionary<int, Dictionary<int, Cell<C>>> cellLookup = new Dictionary<int, Dictionary<int, Cell<C>>>();
	/**
	 * Construct a Cell World with size xDimension * y Dimension cells, all with
	 * their values set to a default content value.
	 * 
	 * @param xDimension
	 *            the size of the x dimension.
	 * @param yDimension
	 *            the size of the y dimension.
	 * 
	 * @param defaultCellContent
	 *            the default content to assign to each cell created.
	 */
	public CellWorld(int xDimension, int yDimension, C defaultCellContent)
	{
	    for (int x = 1; x <= xDimension; x++)
	    {
		Dictionary<int, Cell<C>> xCol = new Dictionary<int, Cell<C>>();
		for (int y = 1; y <= yDimension; y++)
		{
		    Cell<C> c = new Cell<C>(x, y, defaultCellContent);
		    cells.Add(c);
		    xCol[y] = c;
		}
		cellLookup[x] = xCol;
	    }
	}

	/**
	 * 
	 * @return all the cells in this world.
	 */
	public LinkedHashSet<Cell<C>> getCells()
	{
	    return cells;
	}

	/**
	 * Determine what cell would be moved into if the specified action is
	 * performed in the specified cell. Normally, this will be the cell adjacent
	 * in the appropriate direction. However, if there is no cell in the
	 * adjacent direction of the action then the outcome of the action is to
	 * stay in the same cell as the action was performed in.
	 * 
	 * @param s
	 *            the cell location from which the action is to be performed.
	 * @param a
	 *            the action to perform (Up, Down, Left, or Right).
	 * @return the Cell an agent would end up in if they performed the specified
	 *         action from the specified cell location.
	 */
	public Cell<C> result(Cell<C> s, CellWorldAction a)
	{
	    Cell<C> sDelta = getCellAt(a.getXResult(s.getX()), a.getYResult(s
			    .getY()));
	    if (null == sDelta)
	    {
		// Default to no effect
		// (i.e. bumps back in place as no adjoining cell).
		sDelta = s;
	    }
	    return sDelta;
	}

	/**
	 * Remove the cell at the specified location from this Cell World. This
	 * allows you to introduce barriers into different location.
	 * 
	 * @param x
	 *            the x dimension of the cell to be removed.
	 * @param y
	 *            the y dimension of the cell to be removed.
	 */
	public void removeCell(int x, int y)
	{
	    Dictionary<int, Cell<C>> xCol = cellLookup[x];
	    if (null != xCol)
	    {
		xCol.Remove(y);
		cells.Remove(xCol[y]);
	    }
	}

	/**
	 * Get the cell at the specified x and y locations.
	 * 
	 * @param x
	 *            the x dimension of the cell to be retrieved.
	 * @param y
	 *            the y dimension of the cell to be retrieved.
	 * @return the cell at the specified x,y location, null if no cell exists at
	 *         this location.
	 */
	public Cell<C> getCellAt(int x, int y)
	{
	    Cell<C> c = null;
	    Dictionary<int, Cell<C>> xCol = cellLookup[x];
	    if (null != xCol)
	    {
		c = xCol[y];
	    }
	    return c;
	}
    }
}