using System;
using System.Collections.Generic;

namespace aima.core.util
{
    /**
     * Basic supports for Intervals.
     * 
     * @see <a href="http://en.wikipedia.org/wiki/Interval_%28mathematics%29"
     *      >Interval</a>
     * 
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public class Interval<C>
    {
	private IComparable<C> lower = null;
	private bool lowerInclusive = true;
	private IComparable<C> upper = null;
	private bool upperInclusive = true;

	public Interval()
	{

	}

	/**
	 * Constructs a closed interval from the two specified end points.
	 * 
	 * @param lower
	 *            the lower end point of the interval
	 * @param upper
	 *            the upper end point of the interval
	 */
	public Interval(IComparable<C> lower, IComparable<C> upper)
	{
	    setLower(lower);
	    setUpper(upper);
	}

	/**
	 * Constructs an interval from the two specified end points.
	 * 
	 * @param lower
	 *            the lower end point of the interval
	 * @param lowerInclusive
	 *            wether or not the lower end of the interval is inclusive of
	 *            its value.
	 * @param upper
	 *            the upper end point of the interval
	 * @param upperInclusive
	 *            whether or not the upper end of the interval is inclusive of
	 *            its value.
	 */
	public Interval(IComparable<C> lower, bool lowerInclusive,
			IComparable<C> upper, bool upperInclusive)
	{
	    setLower(lower);
	    setLowerInclusive(lowerInclusive);
	    setUpper(upper);
	    setUpperInclusive(upperInclusive);
	}

	/**
	 * Returns <code>true</code> if the specified object is between the end
	 * points of this interval.
	 * 
	 * @return <code>true</code> if the specified value is between the end
	 *         points of this interval.
	 */
	public bool isInInterval(C o)
	{
	    if(null == lower || null == upper)
	    {
		return false;
	    }

	    bool In = true;

	    if (isLowerInclusive())
	    {
		In = lower.CompareTo(o) <= 0;
	    }
	    else
	    {
		In = lower.CompareTo(o) < 0;
	    }

	    if (In)
	    {
		if (isUpperInclusive())
		{
		    In = upper.CompareTo(o) >= 0;
		}
		else
		{
		    In = upper.CompareTo(o) > 0;
		}
	    }
	    return In;
	}

	/**
	 * Returns <code>true</code> if this interval is lower inclusive.
	 * 
	 * @return <code>true</code> if this interval is lower inclusive.
	 */
	public bool isLowerInclusive()
	{
	    return lowerInclusive;
	}

	/**
	 * Returns <code>true</code> if this interval is not lower inclusive.
	 * 
	 * @return <code>true</code> if this interval is not lower inclusive.
	 */
	public bool isLowerExclusive()
	{
	    return !lowerInclusive;
	}

	/**
	 * Sets the interval to lower inclusive or lower exclusive.
	 * 
	 * @param inclusive
	 *            <code>true</code> represents lower inclusive and
	 *            <code>false</code> represents lower exclusive.
	 */
	public void setLowerInclusive(bool inclusive)
	{
	    this.lowerInclusive = inclusive;
	}

	/**
	 * Sets the interval to lower exclusive or lower inclusive.
	 * 
	 * @param exclusive
	 *            <code>true</code> represents lower exclusive and
	 *            <code>false</code> represents lower inclusive.
	 */
	public void setLowerExclusive(bool exclusive)
	{
	    this.lowerInclusive = !exclusive;
	}

	/**
	 * Returns the lower end point of the interval.
	 * 
	 * @return the lower end point of the interval.
	 */
	public IComparable<C> getLower()
	{
	    return lower;
	}

	/**
	 * Sets the lower end point of the interval.
	 * 
	 * @param lower
	 *            the lower end point of the interval
	 */
	public void setLower(IComparable<C> lower)
	{
	    this.lower = lower;
	}

	/**
	 * Returns <code>true</code> if this interval is upper inclusive.
	 * 
	 * @return <code>true</code> if this interval is upper inclusive.
	 */
	public bool isUpperInclusive()
	{
	    return upperInclusive;
	}

	/**
	 * Returns <code>true</code> if this interval is upper exclusive.
	 * 
	 * @return <code>true</code> if this interval is upper exclusive.
	 */
	public bool isUpperExclusive()
	{
	    return !upperInclusive;
	}

	/**
	 * Sets the interval to upper inclusive or upper exclusive.
	 * 
	 * @param inclusive
	 *            <code>true</code> represents upper inclusive and
	 *            <code>false</code> represents upper exclusive.
	 */
	public void setUpperInclusive(bool inclusive)
	{
	    this.upperInclusive = inclusive;
	}

	/**
	 * Sets the interval to upper exclusive or upper inclusive.
	 * 
	 * @param exclusive
	 *            <code>true</code> represents upper exclusive and
	 *            <code>false</code> represents upper inclusive.
	 */
	public void setUpperExclusive(bool exclusive)
	{
	    this.upperInclusive = !exclusive;
	}

	/**
	 * Returns the upper end point of the interval.
	 * 
	 * @return the upper end point of the interval.
	 */
	public IComparable<C> getUpper()
	{
	    return upper;
	}

	/**
	 * Sets the upper end point of the interval.
	 * 
	 * @param upper
	 *            the upper end point of the interval.
	 */
	public void setUpper(IComparable<C> upper)
	{
	    this.upper = upper;
	}

	public System.String toString()
	{
	    return (lowerInclusive ? "[" : "(") + lower + ", " + upper
			    + (upperInclusive ? "]" : ")");
	}
    }
}