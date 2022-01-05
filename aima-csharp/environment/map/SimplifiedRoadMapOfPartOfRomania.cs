namespace aima.core.environment.map
{
    /**
     * Represents a simplified road map of Romania. The initialization method is
     * declared static. So it can also be used to initialize other specialized
     * subclasses of {@link ExtendableMap} with road map data from Romania. Location
     * names, road distances and directions have been extracted from Artificial
     * Intelligence A Modern Approach (2nd Edition), Figure 3.2, page 63. The
     * straight-line distances to Bucharest have been taken from Artificial
     * Intelligence A Modern Approach (2nd Edition), Figure 4.1, page 95.
     * 
     * @author Ruediger Lunde
     */
    public class SimplifiedRoadMapOfPartOfRomania : ExtendableMap
    {
	public SimplifiedRoadMapOfPartOfRomania()
	{
	    initMap(this);
	}

	// The different locations in the simplified map of part of Romania
	public const String ORADEA = "Oradea";
	public const String ZERIND = "Zerind";
	public const String ARAD = "Arad";
	public const String TIMISOARA = "Timisoara";
	public const String LUGOJ = "Lugoj";
	public const String MEHADIA = "Mehadia";
	public const String DOBRETA = "Dobreta";
	public const String SIBIU = "Sibiu";
	public const String RIMNICU_VILCEA = "RimnicuVilcea";
	public const String CRAIOVA = "Craiova";
	public const String FAGARAS = "Fagaras";
	public const String PITESTI = "Pitesti";
	public const String GIURGIU = "Giurgiu";
	public const String BUCHAREST = "Bucharest";
	public const String NEAMT = "Neamt";
	public const String URZICENI = "Urziceni";
	public const String IASI = "Iasi";
	public const String VASLUI = "Vaslui";
	public const String HIRSOVA = "Hirsova";
	public const String EFORIE = "Eforie";

	/**
         * Initializes a map with a simplified road map of Romania.
         */
	public static void initMap(ExtendableMap map)
	{
	    // mapOfRomania
	    map.clear();
	    map.addBidirectionalLink(ORADEA, ZERIND, 71.0);
	    map.addBidirectionalLink(ORADEA, SIBIU, 151.0);
	    map.addBidirectionalLink(ZERIND, ARAD, 75.0);
	    map.addBidirectionalLink(ARAD, TIMISOARA, 118.0);
	    map.addBidirectionalLink(ARAD, SIBIU, 140.0);
	    map.addBidirectionalLink(TIMISOARA, LUGOJ, 111.0);
	    map.addBidirectionalLink(LUGOJ, MEHADIA, 70.0);
	    map.addBidirectionalLink(MEHADIA, DOBRETA, 75.0);
	    map.addBidirectionalLink(DOBRETA, CRAIOVA, 120.0);
	    map.addBidirectionalLink(SIBIU, FAGARAS, 99.0);
	    map.addBidirectionalLink(SIBIU, RIMNICU_VILCEA, 80.0);
	    map.addBidirectionalLink(RIMNICU_VILCEA, PITESTI, 97.0);
	    map.addBidirectionalLink(RIMNICU_VILCEA, CRAIOVA, 146.0);
	    map.addBidirectionalLink(CRAIOVA, PITESTI, 138.0);
	    map.addBidirectionalLink(FAGARAS, BUCHAREST, 211.0);
	    map.addBidirectionalLink(PITESTI, BUCHAREST, 101.0);
	    map.addBidirectionalLink(GIURGIU, BUCHAREST, 90.0);
	    map.addBidirectionalLink(BUCHAREST, URZICENI, 85.0);
	    map.addBidirectionalLink(NEAMT, IASI, 87.0);
	    map.addBidirectionalLink(URZICENI, VASLUI, 142.0);
	    map.addBidirectionalLink(URZICENI, HIRSOVA, 98.0);
	    map.addBidirectionalLink(IASI, VASLUI, 92.0);
	    // addBidirectionalLink(VASLUI - already all linked
	    map.addBidirectionalLink(HIRSOVA, EFORIE, 86.0);
	    // addBidirectionalLink(EFORIE - already all linked

	    // distances and directions
	    // reference location: Bucharest
	    map.setDistAndDirToRefLocation(ARAD, 366, 117);
	    map.setDistAndDirToRefLocation(BUCHAREST, 0, 360);
	    map.setDistAndDirToRefLocation(CRAIOVA, 160, 74);
	    map.setDistAndDirToRefLocation(DOBRETA, 242, 82);
	    map.setDistAndDirToRefLocation(EFORIE, 161, 282);
	    map.setDistAndDirToRefLocation(FAGARAS, 176, 142);
	    map.setDistAndDirToRefLocation(GIURGIU, 77, 25);
	    map.setDistAndDirToRefLocation(HIRSOVA, 151, 260);
	    map.setDistAndDirToRefLocation(IASI, 226, 202);
	    map.setDistAndDirToRefLocation(LUGOJ, 244, 102);
	    map.setDistAndDirToRefLocation(MEHADIA, 241, 92);
	    map.setDistAndDirToRefLocation(NEAMT, 234, 181);
	    map.setDistAndDirToRefLocation(ORADEA, 380, 131);
	    map.setDistAndDirToRefLocation(PITESTI, 100, 116);
	    map.setDistAndDirToRefLocation(RIMNICU_VILCEA, 193, 115);
	    map.setDistAndDirToRefLocation(SIBIU, 253, 123);
	    map.setDistAndDirToRefLocation(TIMISOARA, 329, 105);
	    map.setDistAndDirToRefLocation(URZICENI, 80, 247);
	    map.setDistAndDirToRefLocation(VASLUI, 199, 222);
	    map.setDistAndDirToRefLocation(ZERIND, 374, 125);
	}
    }
}