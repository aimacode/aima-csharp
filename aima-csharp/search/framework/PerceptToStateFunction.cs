using System;
using System.Collections.Generic;
using aima.core.agent;

namespace aima.core.search.framework
{
    /**
     * This interface is to define how to Map a Percept to a State representation
     * for a problem solver within a specific environment. This arises in the
     * description of the Online Search algorithms from Chapter 4.
     * 
     * @author Ciaran O'Reilly
     * 
     */
     public interface PerceptToStateFunction
    {
        /**
	 * Get the problem state associated with a Percept.
	 * 
	 * @param p
	 *            the percept to be transformed to a problem state.
	 * @return a problem state derived from the Percept p.
	 */
        Object getState(Percept p);
    }
}