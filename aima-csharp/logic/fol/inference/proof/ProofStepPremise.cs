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
    public class ProofStepPremise : AbstractProofStep
    {
	private static readonly List<ProofStep> _noPredecessors = new List<ProofStep>();

	private Object proof = "";

	public ProofStepPremise(Object proof)
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
	    return "Premise";
	}

	// END-ProofStep
    }
}