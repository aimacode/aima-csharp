using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace aima.core.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepRenaming : AbstractProofStep
    {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Object proof = "";

	public ProofStepRenaming(Object proof, ProofStep predecessor)
	{
	    this.proof = proof;
	    this.predecessors.Add(predecessor);
	}

	// START-ProofStep

	public override List<ProofStep> getPredecessorSteps()
	{
	    return new ReadOnlyCollection<ProofStep>(predecessors).ToList<ProofStep>();
	}

	public override String getProof()
	{
	    return proof.ToString();
	}

	public override String getJustification()
	{
	    return "Renaming of " + predecessors[0].getStepNumber();
	}

	// END-ProofStep
    }
}