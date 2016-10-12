using System;
using System.Collections.Generic;
using aima.core.logic.fol.kb.data;

namespace aima.core.logic.fol.inference.otter
{
    /**
     * Heuristic for selecting lightest clause from SOS. To avoid recalculating the
     * lightest clause on every call, the interface supports defining the initial
     * sos and updates to that set so that it can maintain its own internal data
     * structures to allow for incremental re-calculation of the lightest clause.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public interface LightestClauseHeuristic
    {
	/**
	 * Get the lightest clause from the SOS
	 * 
	 * @return the lightest clause.
	 */
	Clause getLightestClause();

	/**
	 * SOS life-cycle methods allowing implementations of this interface to
	 * incrementally update the calculation of the lightest clause as opposed to
	 * having to recalculate each time.
	 * 
	 * @param clauses
	 */
	void initialSOS(List<Clause> clauses);

	void addedClauseToSOS(Clause clause);

	void removedClauseFromSOS(Clause clause);
    }
}