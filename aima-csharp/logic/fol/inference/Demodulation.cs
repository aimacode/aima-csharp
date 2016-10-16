using System;
using System.Collections.Generic;
using System.Linq;
using aima.core.logic.fol.inference.proof;
using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.inference
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 354.<br>
     * <br>
     * Demodulation: For any terms x, y, and z, where z appears somewhere in literal
     * m<sub>i</sub> and where UNIFY(x,z) = &theta;:<br>
     * 
     * <pre>
     *                 x=y,    m<sub>1</sub> OR ... OR m<sub>n</sub>[z]
     *     ------------------------------------------------------------
     *     SUB(SUBST(&theta;,x), SUBST(&theta;,y), m<sub>1</sub> OR ... OR m<sub>n</sub>)
     * </pre>
     * 
     * where SUBST is the usual substitution of a binding list, and SUB(x,y,m) means
     * to replace x with y everywhere that x occurs within m.<br>
     * <br>
     * Some additional restrictions/clarifications highlighted in:<br>
     * <a href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture15.pdf"
     * >Demodulation Restrictions</a> <br>
     * 1. Unit Equations Only.<br>
     * 2. Variables substituted in Equation Only.<br>
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class Demodulation : AbstractModulation
    {
	public Demodulation()
	{

	}

	public Clause apply(TermEquality assertion, Clause clExpression)
	{
	    Clause altClExpression = null;

	    foreach (Literal l1 in clExpression.getLiterals())
	    {
		AtomicSentence altExpression = apply(assertion, l1
			.getAtomicSentence());
		if (null != altExpression)
		{
		    // I have an alternative, create a new clause
		    // with the alternative and return
		    List<Literal> newLits = new List<Literal>();
		    foreach (Literal l2 in clExpression.getLiterals())
		    {
			if (l1.Equals(l2))
			{
			    newLits.Add(l1.newInstance(altExpression));
			}
			else
			{
			    newLits.Add(l2);
			}
		    }
		    // Only apply demodulation at most once on
		    // each call.
		    altClExpression = new Clause(newLits);
		    altClExpression.setProofStep(new ProofStepClauseDemodulation(
			    altClExpression, clExpression, assertion));
		    if (clExpression.isImmutable())
		    {
			altClExpression.setImmutable();
		    }
		    if (!clExpression.isStandardizedApartCheckRequired())
		    {
			altClExpression.setStandardizedApartCheckNotRequired();
		    }
		    break;
		}
	    }

	    return altClExpression;
	}

	public AtomicSentence apply(TermEquality assertion,
		AtomicSentence expression)
	{
	    AtomicSentence altExpression = null;

	    IdentifyCandidateMatchingTerm icm = getMatchingSubstitution(assertion
		    .getTerm1(), expression);

	    if (null != icm)
	    {
		Term replaceWith = substVisitor.subst(
			icm.getMatchingSubstitution(), assertion.getTerm2());
		// Want to ignore reflexivity axiom situation, i.e. x = x
		if (!icm.getMatchingTerm().Equals(replaceWith))
		{
		    ReplaceMatchingTerm rmt = new ReplaceMatchingTerm();

		    // Only apply demodulation at most once on each call.
		    altExpression = rmt.replace(expression, icm.getMatchingTerm(),
			    replaceWith);
		}
	    }
	    return altExpression;
	}

	// PROTECTED METHODS	
	public override bool isValidMatch(Term toMatch,
		List<Variable> toMatchVariables, Term possibleMatch,
		Dictionary<Variable, Term> substitution)
	{
	    // Demodulation only allows substitution in the equation only,
	    // if the substitution contains variables not in the toMatch
	    // side of the equation (i.e. left hand side), then
	    // it is not a legal demodulation match.
	    // Note: see:
	    // http://logic.stanford.edu/classes/cs157/2008/lectures/lecture15.pdf
	    // slide 23 for an example.
	    bool contained = !substitution.Keys.Except(toMatchVariables).Any();
	    if (contained)
	    {
		return true;
	    }
	    return false;
	}
    }
}