using System;
using System.Collections.Generic;
using System.Text;

namespace aima.core.logic.fol
{
    /**
     * This class ensures unique standardize apart indexicals are created.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class StandardizeApartIndexicalFactory
    {
	private static Dictionary<char, int> _assignedIndexicals = new Dictionary<char, int>();

	// For use in test cases, where predictable behavior is expected.
	public static void flush()
	{
	    lock (_assignedIndexicals)
	    {
		_assignedIndexicals.Clear();
	    }
	}

	public static StandardizeApartIndexical newStandardizeApartIndexical(
		Char preferredPrefix)
	{
	    char ch = preferredPrefix;
	    if (!(Char.IsLetter(ch) && Char.IsLower(ch)))
	    {
		throw new ArgumentException("Preferred prefix :"
				+ preferredPrefix + " must be a valid a lower case letter.");
	    }

	    StringBuilder sb = new StringBuilder();
	    lock (_assignedIndexicals)
	    {
		int currentPrefixCnt = -1;
		if (!_assignedIndexicals.ContainsKey(preferredPrefix))
		{
		    currentPrefixCnt = 0;
		    _assignedIndexicals.Add(preferredPrefix, currentPrefixCnt);
		}
		else
		{
		    currentPrefixCnt += 1;
		    _assignedIndexicals[preferredPrefix] = currentPrefixCnt;
		}

		sb.Append(preferredPrefix);
		for (int i = 0; i < currentPrefixCnt; i++)
		{
		    sb.Append(preferredPrefix);
		}
	    }

	    return new StandardizeApartIndexicalImpl(sb.ToString());
	}
    }

    class StandardizeApartIndexicalImpl : StandardizeApartIndexical
    {
	private String prefix = null;
	private int index = 0;

	public StandardizeApartIndexicalImpl(String prefix)
	{
	    this.prefix = prefix;
	}

	// START-StandardizeApartIndexical
	public String getPrefix()
	{
	    return prefix;
	}

	public int getNextIndex()
	{
	    return index++;
	}
	// END-StandardizeApartIndexical	
    }
}