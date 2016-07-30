using System.Collections.Generic;
using aima.core.agent;

namespace aima.core.environment.wumpusworld
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 237.<br>
     * <br>
     * The agent has five sensors, each of which gives a single bit of information:
     * <ul>
     * <li>In the square containing the wumpus and in the directly (not diagonally)
     * adjacent squares, the agent will perceive a Stench.</li>
     * <li>In the squares directly adjacent to a pit, the agent will perceive a
     * Breeze.</li>
     * <li>In the square where the gold is, the agent will perceive a Glitter.</li>
     * <li>When an agent walks into a wall, it will perceive a Bump.</li>
     * <li>When the wumpus is killed, it emits a woeful Scream that can be perceived
     * anywhere in the cave.</li>
     * </ul>
     * 
     * @author Federico Baron
     * @author Alessandro Daniele
     * @author Ciaran O'Reilly
     */
    public class AgentPercept : Percept
    {
	private bool stench;
	private bool breeze;
	private bool glitter;
	private bool bump;
	private bool scream;

	/**
	 * Default Constructor. All sensor inputs are considered false.
	 */
	public AgentPercept()
	{
	    setStench(false);
	    setBreeze(false);
	    setGlitter(false);
	    setBump(false);
	    setScream(false);
	}

	/**
	 * Constructor with all 5 sensor inputs explicitly set.
	 * 
	 * @param stench
	 * @param breeze
	 * @param glitter
	 * @param bump
	 * @param scream
	 */
	public AgentPercept(bool stench, bool breeze, bool glitter,
			bool bump, bool scream)
	{
	    setStench(stench);
	    setBreeze(breeze);
	    setGlitter(glitter);
	    setBump(bump);
	    setScream(scream);
	}

	public bool isStench()
	{
	    return stench;
	}

	public void setStench(bool stench)
	{
	    this.stench = stench;
	}

	public bool isBreeze()
	{
	    return breeze;
	}

	public void setBreeze(bool breeze)
	{
	    this.breeze = breeze;
	}

	public bool isGlitter()
	{
	    return glitter;
	}

	public void setGlitter(bool glitter)
	{
	    this.glitter = glitter;
	}

	public bool isBump()
	{
	    return bump;
	}

	public void setBump(bool bump)
	{
	    this.bump = bump;
	}

	public bool isScream()
	{
	    return scream;
	}

	public void setScream(bool scream)
	{
	    this.scream = scream;
	}

	public string toString()
	{
	    return "[" + ((stench) ? "Stench" : "None") + ", "
			      + ((breeze) ? "Breeze" : "None") + ", "
			      + ((glitter) ? "Glitter" : "None") + ", "
			      + ((bump) ? "Bump" : "None") + ", "
			      + ((scream) ? "Scream" : "None") + "]";
	}
    }
}
