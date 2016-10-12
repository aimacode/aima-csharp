using System;
using System.Collections.Generic;
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
    public class ProofStepClauseBinaryResolvent : AbstractProofStep
    {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Clause resolvent = null;
	private Literal posLiteral = null;
	private Literal negLiteral = null;
	private Clause parent1, parent2 = null;
	private Dictionary<Variable, Term> subst = new Dictionary<Variable, Term>();
	private Dictionary<Variable, Term> renameSubst = new Dictionary<Variable, Term>();

	public ProofStepClauseBinaryResolvent(Clause resolvent, Literal pl,
			Literal nl, Clause parent1, Clause parent2,
			Dictionary<Variable, Term> subst, Dictionary<Variable, Term> renameSubst)
	{
	    this.resolvent = resolvent;
	    this.posLiteral = pl;
	    this.negLiteral = nl;
	    this.parent1 = parent1;
	    this.parent2 = parent2;

	    foreach (Variable key in subst.Keys)
	    {
		this.subst.Add(key, subst[key]);
	    }

	    foreach (Variable key in renameSubst.Keys)
	    {
		this.renameSubst.Add(key, renameSubst[key]);
	    }

	    this.predecessors.Add(parent1.getProofStep());
	    this.predecessors.Add(parent2.getProofStep());
	}

	// START-ProofStep

	public override List<ProofStep> getPredecessorSteps()
	{
	    return new ReadOnlyCollection<ProofStep>(predecessors).ToList<ProofStep>();
	}

	public override String getProof()
	{
	    return resolvent.ToString();
	}

	public override String getJustification()
	{
	    int lowStep = parent1.getProofStep().getStepNumber();
	    int highStep = parent2.getProofStep().getStepNumber();

	    if (lowStep > highStep)
	    {
		lowStep = highStep;
		highStep = parent1.getProofStep().getStepNumber();
	    }

	    return "Resolution: " + lowStep + ", " + highStep + "  [" + posLiteral
			    + ", " + negLiteral + "], subst=" + subst + ", renaming="
			    + renameSubst;
	}

	// END-ProofStep
    }
}