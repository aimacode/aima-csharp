using System;
using System.Collections.Generic;
using aima.core.logic.fol.inference;
using aima.core.logic.fol.kb.data;

namespace aima.core.logic.fol.inference.trace
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface FOLTFMResolutionTracer
    {
	void stepStartWhile(HashSet<Clause> clauses, int totalNoClauses,
			int totalNoNewCandidateClauses);

	void stepOuterFor(Clause i);

	void stepInnerFor(Clause i, Clause j);

	void stepResolved(Clause iFactor, Clause jFactor, HashSet<Clause> resolvents);

	void stepFinished(HashSet<Clause> clauses, InferenceResult result);
    }
}