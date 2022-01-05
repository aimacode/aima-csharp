using System.Collections.ObjectModel;
using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepClauseDemodulation : AbstractProofStep
    {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Clause demodulated = null;
	private Clause origClause = null;
	private TermEquality assertion = null;

	public ProofStepClauseDemodulation(Clause demodulated, Clause origClause,
			TermEquality assertion)
	{
	    this.demodulated = demodulated;
	    this.origClause = origClause;
	    this.assertion = assertion;
	    this.predecessors.Add(origClause.getProofStep());
	}

	// START-ProofStep
	
	public override List<ProofStep> getPredecessorSteps()
	{
	    return new ReadOnlyCollection<ProofStep>(predecessors).ToList<ProofStep>();
	}

	public override String getProof()
	{
	    return demodulated.ToString();
	}
	
	public override String getJustification()
	{
	    return "Demodulation: " + origClause.getProofStep().getStepNumber()
			    + ", [" + assertion + "]";
	}

	// END-ProofStep
    }
}