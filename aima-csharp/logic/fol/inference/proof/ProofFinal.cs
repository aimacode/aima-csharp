using System;
using System.Collections.Generic;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofFinal : Proof
    {
	private Dictionary<Variable, Term> answerBindings = new Dictionary<Variable, Term>();
	private ProofStep finalStep = null;
	private List<ProofStep> proofSteps = null;

	public ProofFinal(ProofStep finalStep, Dictionary<Variable, Term> answerBindings)
	{
	    this.finalStep = finalStep;
	    this.answerBindings = answerBindings;
	}

	// START-Proof

	public List<ProofStep> getSteps()
	{
	    // Only calculate if the proof steps are actually requested.
	    if (null == proofSteps)
	    {
		calculateProofSteps();
	    }
	    return proofSteps;
	}

	public Dictionary<Variable, Term> getAnswerBindings()
	{
	    return answerBindings;
	}

	public void replaceAnswerBindings(Dictionary<Variable, Term> updatedBindings)
	{
	    answerBindings.Clear();
	    answerBindings = updatedBindings;
	}

	// END-Proof

	public override String ToString()
	{
	    return answerBindings.ToString();
	}

	
	// PRIVATE METHODS
	
	private void calculateProofSteps()
	{
	    proofSteps = new List<ProofStep>();
	    addToProofSteps(finalStep);

	    // Move all premises to the front of the
	    // list of steps
	    int to = 0;
	    for (int i = 0; i < proofSteps.Count; i++)
	    {
		if (proofSteps[i] is ProofStepPremise)
		{
		    ProofStep m = proofSteps[i];
		    proofSteps.RemoveAt(i);
		    proofSteps.Insert(to, m);
		    to++;
		}
	    }

	    // Move the Goals after the premises
	    for (int i = 0; i < proofSteps.Count; i++)
	    {
		if (proofSteps[i] is ProofStepGoal)
		{
		    ProofStep m = proofSteps[i];
		    proofSteps.RemoveAt(i);
		    proofSteps.Insert(to, m);
		    to++;
		}
	    }

	    // Assign the step #s now that all the proof
	    // steps have been unwound
	    for (int i = 0; i < proofSteps.Count; i++)
	    {
		proofSteps[i].setStepNumber(i + 1);
	    }
	}

	private void addToProofSteps(ProofStep step)
	{
	    if (!proofSteps.Contains(step))
	    {
		proofSteps.Insert(0, step);
	    }
	    else
	    {
		proofSteps.Remove(step);
		proofSteps.Insert(0, step);
	    }
	    List<ProofStep> predecessors = step.getPredecessorSteps();
	    for (int i = predecessors.Count - 1; i >= 0; i--)
	    {
		addToProofSteps(predecessors[i]);
	    }
	}
    }
}