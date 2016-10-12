using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepBwChGoal : AbstractProofStep
    {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Clause toProve = null;
	private Literal currentGoal = null;
	private Dictionary<Variable, Term> bindings = new Dictionary<Variable, Term>();

	public ProofStepBwChGoal(Clause toProve, Literal currentGoal,
		Dictionary<Variable, Term> bindings)
	{
	    this.toProve = toProve;
	    this.currentGoal = currentGoal;
	    foreach (Variable key in bindings.Keys)
	    {
		this.bindings.Add(key, bindings[key]);
	    }
	}

	public Dictionary<Variable, Term> getBindings()
	{
	    return bindings;
	}

	public void setPredecessor(ProofStep predecessor)
	{
	    predecessors.Clear();
	    predecessors.Add(predecessor);
	}

	// START-ProofStep

	public override List<ProofStep> getPredecessorSteps()
	{
	    return new ReadOnlyCollection<ProofStep>(predecessors).ToList<ProofStep>();
	}

	public override String getProof()
	{
	    StringBuilder sb = new StringBuilder();
	    List<Literal> nLits = toProve.getNegativeLiterals();
	    for (int i = 0; i < toProve.getNumberNegativeLiterals(); i++)
	    {
		sb.Append(nLits[i].getAtomicSentence());
		if (i != (toProve.getNumberNegativeLiterals() - 1))
		{
		    sb.Append(" AND ");
		}
	    }
	    if (toProve.getNumberNegativeLiterals() > 0)
	    {
		sb.Append(" => ");
	    }
	    sb.Append(toProve.getPositiveLiterals()[0]);
	    return sb.ToString();
	}

	public override String getJustification()
	{
	    return "Current Goal " + currentGoal.getAtomicSentence().ToString()
		    + ", " + bindings;
	}

	// END-ProofStep
    }
}