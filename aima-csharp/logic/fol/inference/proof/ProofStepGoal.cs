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
    public class ProofStepGoal : AbstractProofStep
    {
	private static readonly List<ProofStep> _noPredecessors = new List<ProofStep>();

	private Object proof = "";

	public ProofStepGoal(Object proof)
	{
	    this.proof = proof;
	}

	// START-ProofStep

	public override List<ProofStep> getPredecessorSteps()
	{
	    return new ReadOnlyCollection<ProofStep>(_noPredecessors).ToList<ProofStep>();
	}
	
	public override String getProof()
	{
	    return proof.ToString();
	}
	
	public override String getJustification()
	{
	    return "Goal";
	}

	// END-ProofStep
    }
}