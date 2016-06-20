using System;

namespace aima.core.util
{
    /**
     * Mock implementation of the Randomizer interface so that the set of Random
     * numbers returned are in fact predefined.
     * 
     * @author Ravi Mohan
     * 
     */
    public class MockRandomizer : Randomizer
    {
	private double[] values;
	private int index;

	/**
	 * 
	 * @param values
	 *            the set of predetermined random values to loop over.
	 */
	public MockRandomizer(double[] values)
	{
	    this.values = new double[values.Length];
	    System.Array.Copy(values, 0, this.values, 0, values.Length);
	    this.index = 0;
	}

	// START-Randomizer
	public double nextDouble()
	{
	    if(index == values.Length)
	    {
		index = 0;
	    }
	    return values[index++];
	}

	//END-Randomizer
    }
}