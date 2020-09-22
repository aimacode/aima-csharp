using System;
using System.Collections.Generic;
using aima.core.logic.fol.inference.proof;
using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class StandardizeApart
    {
	private VariableCollector variableCollector = null;
	private SubstVisitor substVisitor = null;

	public StandardizeApart()
	{
	    variableCollector = new VariableCollector();
	    substVisitor = new SubstVisitor();
	}

	public StandardizeApart(VariableCollector variableCollector,
			SubstVisitor substVisitor)
	{
	    this.variableCollector = variableCollector;
	    this.substVisitor = substVisitor;
	}

	// Note: see page 327.
	public StandardizeApartResult standardizeApart(Sentence sentence,
			StandardizeApartIndexical standardizeApartIndexical)
	{
	    List<Variable> toRename = variableCollector.collectAllVariables(sentence);
	    Dictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();
	    Dictionary<Variable, Term> reverseSubstitution = new Dictionary<Variable, Term>();

	    foreach (Variable var in toRename)
	    {
		Variable v = null;
		do
		{
		    v = new Variable(standardizeApartIndexical.getPrefix()
				    + standardizeApartIndexical.getNextIndex());
		    // Ensure the new variable name is not already
		    // accidentally used in the sentence
		} while (toRename.Contains(v));

		renameSubstitution.Add(var, v);
		reverseSubstitution.Add(v, var);
	    }
	    Sentence standardized = substVisitor.subst(renameSubstitution,
			    sentence);

	    return new StandardizeApartResult(sentence, standardized,
			    renameSubstitution, reverseSubstitution);
	}

		public Clause standardizeApart(Clause clause,
				StandardizeApartIndexical standardizeApartIndexical)
		{

			List<Variable> toRename = variableCollector.collectAllVariables(clause);
			Dictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();

			foreach (Variable var in toRename)
			{
				Variable v;
				do
				{
					v = new Variable(standardizeApartIndexical.getPrefix()
							+ standardizeApartIndexical.getNextIndex());
					// Ensure the new variable name is not already
					// accidentally used in the sentence
				} while (toRename.Contains(v));
				renameSubstitution[var] = v;
			}

			if (renameSubstitution.Count > 0)
			{
				List<Literal> literals = new List<Literal>();

				foreach (Literal l in clause.getLiterals())
				{
					literals.Add(substVisitor.subst(renameSubstitution, l));
				}
				Clause renamed = new Clause(literals);
				renamed.setProofStep(new ProofStepRenaming(renamed, clause
						.getProofStep()));
				return renamed;
			}
			return clause;
		}

	public Chain standardizeApart(Chain chain,
			StandardizeApartIndexical standardizeApartIndexical)
	{

	    List<Variable> toRename = variableCollector.collectAllVariables(chain);
	    Dictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();

	    foreach (Variable var in toRename)
	    {
		Variable v = null;
		do
		{
		    v = new Variable(standardizeApartIndexical.getPrefix()
				    + standardizeApartIndexical.getNextIndex());
		    // Ensure the new variable name is not already
		    // accidentally used in the sentence
		} while (toRename.Contains(v));

		renameSubstitution.Add(var, v);
	    }

	    if (renameSubstitution.Count > 0)
	    {
		List<Literal> lits = new List<Literal>();

		foreach (Literal l in chain.getLiterals())
		{
		    AtomicSentence atom = (AtomicSentence)substVisitor.subst(
				    renameSubstitution, l.getAtomicSentence());
		    lits.Add(l.newInstance(atom));
		}

		Chain renamed = new Chain(lits);

		renamed.setProofStep(new ProofStepRenaming(renamed, chain
				.getProofStep()));

		return renamed;
	    }
	    return chain;
	}

	public Dictionary<Variable, Term> standardizeApart(List<Literal> l1Literals,
			List<Literal> l2Literals,
			StandardizeApartIndexical standardizeApartIndexical)
	{
	    List<Variable> toRename = new List<Variable>();

	    foreach (Literal pl in l1Literals)
	    {
		toRename.AddRange(variableCollector.collectAllVariables(pl
				.getAtomicSentence()));
	    }
	    foreach (Literal nl in l2Literals)
	    {
		toRename.AddRange(variableCollector.collectAllVariables(nl
				.getAtomicSentence()));
	    }

	    Dictionary<Variable, Term> renameSubstitution = new Dictionary<Variable, Term>();

	    foreach (Variable var in toRename)
	    {
		Variable v = null;
		do
		{
		    v = new Variable(standardizeApartIndexical.getPrefix()
				    + standardizeApartIndexical.getNextIndex());
		    // Ensure the new variable name is not already
		    // accidentally used in the sentence
		} while (toRename.Contains(v));

		renameSubstitution.Add(var, v);
	    }

	    List<Literal> posLits = new List<Literal>();
	    List<Literal> negLits = new List<Literal>();

	    foreach (Literal pl in l1Literals)
	    {
		posLits.Add(substVisitor.subst(renameSubstitution, pl));
	    }
	    foreach (Literal nl in l2Literals)
	    {
		negLits.Add(substVisitor.subst(renameSubstitution, nl));
	    }

	    l1Literals.Clear();
	    l1Literals.AddRange(posLits);
	    l2Literals.Clear();
	    l2Literals.AddRange(negLits);

	    return renameSubstitution;
	}
    }
}