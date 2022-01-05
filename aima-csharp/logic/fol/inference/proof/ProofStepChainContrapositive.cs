using System.Collections.ObjectModel;
using aima.core.logic.fol.kb.data;

namespace aima.core.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepChainContrapositive : AbstractProofStep
    {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Chain contrapositive = null;
	private Chain contrapositiveOf = null;

	public ProofStepChainContrapositive(Chain contrapositive,
		Chain contrapositiveOf)
	{
	    this.contrapositive = contrapositive;
	    this.contrapositiveOf = contrapositiveOf;
	    this.predecessors.Add(contrapositiveOf.getProofStep());
	}
	
	// START-ProofStep

	public override List<ProofStep> getPredecessorSteps()
	{
	    return new ReadOnlyCollection<ProofStep>(predecessors).ToList<ProofStep>();
	}

	public override String getProof()
	{
	    return contrapositive.ToString();
	}

	public override String getJustification()
	{
	    return "Contrapositive: "
		    + contrapositiveOf.getProofStep().getStepNumber();
	}

	// END-ProofStep
    }
}