using System.Collections.Generic;
using System;

namespace aima.core.util
{
    /**
     * Implementation of the Randomizer Interface using Java's java.util.Random
     * class.
     * 
     * @author Ravi Mohan
     * 
     */
    public class CSharpRandomizer : Randomizer
    {
	private static Random _r = new Random();
	private Random r = null;

	public CSharpRandomizer(): this(_r)
	{

	}

	public CSharpRandomizer(Random r)
	{
	    this.r = r;
	}

	// START-Randomizer
	public double nextDouble()
	{
	    return r.NextDouble();
	}

	// END-Randomizer
    }
}