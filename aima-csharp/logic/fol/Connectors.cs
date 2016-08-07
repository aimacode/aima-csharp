using System;

namespace aima.core.logic.fol
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class Connectors
    {
	public static readonly String AND = "AND";

	public static readonly String OR = "OR";

	public static readonly String NOT = "NOT";

	public static readonly String IMPLIES = "=>";

	public static readonly String BICOND = "<=>";

	public static bool isAND(String connector)
	{
	    return AND.Equals(connector);
	}

	public static bool isOR(String connector)
	{
	    return OR.Equals(connector);
	}

	public static bool isNOT(String connector)
	{
	    return NOT.Equals(connector);
	}

	public static bool isIMPLIES(String connector)
	{
	    return IMPLIES.Equals(connector);
	}

	public static bool isBICOND(String connector)
	{
	    return BICOND.Equals(connector);
	}
    }
}