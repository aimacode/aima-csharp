using System;
using System.Collections.Generic;
using aima.core.logic.fol.inference.otter;
using aima.core.logic.fol.kb.data;

namespace aima.core.logic.fol.inference.otter.defaultimpl
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class DefaultClauseFilter : ClauseFilter
    {
	public DefaultClauseFilter()
	{

	}

	// START-ClauseFilter

	public HashSet<Clause> filter(HashSet<Clause> clauses)
	{
	    return clauses;
	}

	// END-ClauseFilter
    }
}