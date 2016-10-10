using System;
using System.Collections.Generic;
using aima.core.logic.fol.kb.data;

namespace aima.core.logic.fol.inference.otter
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface ClauseSimplifier
    {
	Clause simplify(Clause c);
    }
}
