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
    public class ProofStepClauseFactor : AbstractProofStep
    {
	private List<ProofStep> predecessors = new List<ProofStep>();
	private Clause factor = null;
	private Clause factorOf = null;
	private Literal lx = null;
	private Literal ly = null;
	private Dictionary<Variable, Term> subst = new Dictionary<Variable, Term>();
	private Dictionary<Variable, Term> renameSubst = new Dictionary<Variable, Term>();

	public ProofStepClauseFactor(Clause factor, Clause factorOf, Literal lx,
			Literal ly, Dictionary<Variable, Term> subst,
			Dictionary<Variable, Term> renameSubst)
	{
	    this.factor = factor;
	    this.factorOf = factorOf;
	    this.lx = lx;
	    this.ly = ly;
	    
	    foreach (Variable key in subst.Keys)
	    {
		this.subst.Add(key, subst[key]);
	    }

	    foreach (Variable key in renameSubst.Keys)
	    {
		this.renameSubst.Add(key, renameSubst[key]);
	    }

	    this.predecessors.Add(factorOf.getProofStep());
	}

	// START-ProofStep

	public override List<ProofStep> getPredecessorSteps()
	{
	    return new ReadOnlyCollection<ProofStep>(predecessors).ToList<ProofStep>();
	}

	public override String getProof()
	{
	    return factor.ToString();
	}

	public override String getJustification()
	{
	    return "Factor of " + factorOf.getProofStep().getStepNumber() + "  ["
			    + lx + ", " + ly + "], subst=" + subst + ", renaming="
			    + renameSubst;
	}

	// END-ProofStep
    }
}