using aima.core.agent.impl;

namespace aima.core.environment.map
{
    public class MoveToAction : DynamicAction
    {
	public const String ATTRIBUTE_MOVE_TO_LOCATION = "location";

	public MoveToAction(String location) : base("moveTo")
	{
	    setAttribute(ATTRIBUTE_MOVE_TO_LOCATION, location);
	}

	public String getToLocation()
	{
	    return (String)getAttribute(ATTRIBUTE_MOVE_TO_LOCATION);
	}
    }
}
