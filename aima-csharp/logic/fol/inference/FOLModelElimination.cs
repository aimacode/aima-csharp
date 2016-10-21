using System;
using System.Collections.Generic;
using System.Text;
using aima.core.logic.fol.kb;
using aima.core.logic.fol.parsing;
using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing.ast;
using aima.core.logic.fol.inference.proof;
using aima.core.logic.fol.inference.trace;

namespace aima.core.logic.fol.inference
{
    /**
     * Based on lecture notes from:<br>
     * <a
     * href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture13.pdf">
     * http://logic.stanford.edu/classes/cs157/2008/lectures/lecture13.pdf</a>
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class FOLModelElimination : InferenceProcedure
    {
	// Ten seconds is default maximum query time permitted
	private long maxQueryTime = 10 * 1000;	
	private FOLModelEliminationTracer tracer = null;	
	private Unifier unifier = new Unifier();
	private SubstVisitor substVisitor = new SubstVisitor();

	public FOLModelElimination()
	{

	}

	public FOLModelElimination(long maxQueryTime)
	{
	    setMaxQueryTime(maxQueryTime);
	}

	public FOLModelElimination(FOLModelEliminationTracer tracer)
	{
	    this.tracer = tracer;
	}

	public FOLModelElimination(FOLModelEliminationTracer tracer,
		long maxQueryTime)
	{
	    this.tracer = tracer;
	    setMaxQueryTime(maxQueryTime);
	}

	public long getMaxQueryTime()
	{
	    return maxQueryTime;
	}

	public void setMaxQueryTime(long maxQueryTime)
	{
	    this.maxQueryTime = maxQueryTime;
	}

	// START-InferenceProcedure

	public InferenceResult ask(FOLKnowledgeBase kb, Sentence aQuery)
	{
	    // Get the background knowledge - are assuming this is satisfiable
	    // as using Set of Support strategy.
	    List<Clause> bgClauses = new List<Clause>(kb.getAllClauses());
	    List<Clause> removeList = SubsumptionElimination.findSubsumedClauses(bgClauses);
	    foreach (Clause c in removeList)
	    {
		bgClauses.Remove(c);
	    }
	    List<Chain> background = createChainsFromClauses(bgClauses);

	    // Collect the information necessary for constructing
	    // an answer (supports use of answer literals).
	    AnswerHandler ansHandler = new AnswerHandler(kb, aQuery, maxQueryTime, this);

	    IndexedFarParents ifps = new IndexedFarParents(ansHandler
		    .getSetOfSupport(), background);

	    // Iterative deepening to be used
	    for (int maxDepth = 1; maxDepth < int.MaxValue; maxDepth++)
	    {
		// Track the depth actually reached
		ansHandler.resetMaxDepthReached();

		if (null != tracer)
		{
		    tracer.reset();
		}

		foreach (Chain nearParent in ansHandler.getSetOfSupport())
		{
		    recursiveDLS(maxDepth, 0, nearParent, ifps, ansHandler);
		    if (ansHandler.isComplete())
		    {
			return ansHandler;
		    }
		}
		// This means the search tree
		// has bottomed out (i.e. finite).
		// Return what I know based on exploring everything.
		if (ansHandler.getMaxDepthReached() < maxDepth)
		{
		    return ansHandler;
		}
	    }

	    return ansHandler;
	}

	// END-InferenceProcedure
	
	// PRIVATE METHODS
	
	public List<Chain> createChainsFromClauses(List<Clause> clauses)
	{
	    List<Chain> chains = new List<Chain>();

	    foreach (Clause c in clauses)
	    {
		Chain chn = new Chain(c.getLiterals());
		chn.setProofStep(new ProofStepChainFromClause(chn, c));
		chains.Add(chn);
		chains.AddRange(chn.getContrapositives());
	    }

	    return chains;
	}

	// Recursive Depth Limited Search
	private void recursiveDLS(int maxDepth, int currentDepth, Chain nearParent,
		IndexedFarParents indexedFarParents, AnswerHandler ansHandler)
	{

	    // Keep track of the maximum depth reached.
	    ansHandler.updateMaxDepthReached(currentDepth);

	    if (currentDepth == maxDepth)
	    {
		return;
	    }

	    int noCandidateFarParents = indexedFarParents
		    .getNumberCandidateFarParents(nearParent);
	    if (null != tracer)
	    {
		tracer.increment(currentDepth, noCandidateFarParents);
	    }
	    indexedFarParents.standardizeApart(nearParent);
	    for (int farParentIdx = 0; farParentIdx < noCandidateFarParents; farParentIdx++)
	    {
		// If have a complete answer, don't keep
		// checking candidate far parents
		if (ansHandler.isComplete())
		{
		    break;
		}

		// Reduction
		Chain nextNearParent = indexedFarParents.attemptReduction(
			nearParent, farParentIdx);

		if (null == nextNearParent)
		{
		    // Unable to remove the head via reduction
		    continue;
		}

		// Handle Canceling and Dropping
		bool cancelled = false;
		bool dropped = false;
		do
		{
		    cancelled = false;
		    Chain nextParent = null;
		    while (nextNearParent != (nextParent = tryCancellation(nextNearParent)))
		    {
			nextNearParent = nextParent;
			cancelled = true;
		    }

		    dropped = false;
		    while (nextNearParent != (nextParent = tryDropping(nextNearParent)))
		    {
			nextNearParent = nextParent;
			dropped = true;
		    }
		} while (dropped || cancelled);

		// Check if have answer before
		// going to the next level
		if (!ansHandler.isAnswer(nextNearParent))
		{
		    // Keep track of the current # of
		    // far parents that are possible for the next near parent.
		    int noNextFarParents = indexedFarParents
			    .getNumberFarParents(nextNearParent);
		    // Add to indexed far parents
		    nextNearParent = indexedFarParents.addToIndex(nextNearParent);

		    // Check the next level
		    recursiveDLS(maxDepth, currentDepth + 1, nextNearParent,
			    indexedFarParents, ansHandler);

		    // Reset the number of far parents possible
		    // when recursing back up.
		    indexedFarParents.resetNumberFarParentsTo(nextNearParent,
			    noNextFarParents);
		}
	    }
	}

	// Returns c if no cancellation occurred
	private Chain tryCancellation(Chain c)
	{
	    Literal head = c.getHead();
	    if (null != head && !(head is ReducedLiteral))
	    {
		foreach (Literal l in c.getTail())
		{
		    if (l is ReducedLiteral)
		    {
			// if they can be resolved
			if (head.isNegativeLiteral() != l.isNegativeLiteral())
			{
			    Dictionary<Variable, Term> subst = unifier.unify(head
				    .getAtomicSentence(), l.getAtomicSentence());
			    if (null != subst)
			    {
				// I have a cancellation
				// Need to apply subst to all of the
				// literals in the cancellation
				List<Literal> cancLits = new List<Literal>();
				foreach (Literal lfc in c.getTail())
				{
				    AtomicSentence a = (AtomicSentence)substVisitor
					    .subst(subst, lfc.getAtomicSentence());
				    cancLits.Add(lfc.newInstance(a));
				}
				Chain cancellation = new Chain(cancLits);
				cancellation
					.setProofStep(new ProofStepChainCancellation(
						cancellation, c, subst));
				return cancellation;
			    }
			}
		    }
		}
	    }
	    return c;
	}

	// Returns c if no dropping occurred
	private Chain tryDropping(Chain c)
	{
	    Literal head = c.getHead();
	    if (null != head && (head is ReducedLiteral))
	    {
		Chain dropped = new Chain(c.getTail());
		dropped.setProofStep(new ProofStepChainDropped(dropped, c));
		return dropped;
	    }

	    return c;
	}

	class AnswerHandler : InferenceResult
	{
	    private Chain answerChain = new Chain();
	    private List<Variable> answerLiteralVariables;
	    private List<Chain> sos = null;
	    private bool complete = false;
	    private long finishTime = 0L;
	    private int maxDepthReached = 0;
	    private List<Proof> proofs = new List<Proof>();
	    private bool timedOut = false;

	    public AnswerHandler(FOLKnowledgeBase kb, Sentence aQuery,
		    long maxQueryTime, FOLModelElimination folModelElimination)
	    {

		finishTime = System.DateTime.UtcNow.Ticks + maxQueryTime;

		Sentence refutationQuery = new NotSentence(aQuery);

		// Want to use an answer literal to pull
		// query variables where necessary
		Literal answerLiteral = kb.createAnswerLiteral(refutationQuery);
		answerLiteralVariables = kb.collectAllVariables(answerLiteral
			.getAtomicSentence());

		// Create the Set of Support based on the Query.
		if (answerLiteralVariables.Count > 0)
		{
		    Sentence refutationQueryWithAnswer = new ConnectedSentence(
			    Connectors.OR, refutationQuery, (Sentence)answerLiteral
				    .getAtomicSentence().copy());

		    sos = folModelElimination.createChainsFromClauses(kb
			    .convertToClauses(refutationQueryWithAnswer));

		    answerChain.addLiteral(answerLiteral);
		}
		else
		{
		    sos = folModelElimination.createChainsFromClauses(kb
			    .convertToClauses(refutationQuery));
		}

		foreach (Chain s in sos)
		{
		    s.setProofStep(new ProofStepGoal(s));
		}
	    }

	    // START-InferenceResult
	    public bool isPossiblyFalse()
	    {
		return !timedOut && proofs.Count == 0;
	    }

	    public bool isTrue()
	    {
		return proofs.Count > 0;
	    }

	    public bool isUnknownDueToTimeout()
	    {
		return timedOut && proofs.Count == 0;
	    }

	    public bool isPartialResultDueToTimeout()
	    {
		return timedOut && proofs.Count > 0;
	    }

	    public List<Proof> getProofs()
	    {
		return proofs;
	    }

	    // END-InferenceResult
	    
	    public List<Chain> getSetOfSupport()
	    {
		return sos;
	    }

	    public bool isComplete()
	    {
		return complete;
	    }

	    public void resetMaxDepthReached()
	    {
		maxDepthReached = 0;
	    }

	    public int getMaxDepthReached()
	    {
		return maxDepthReached;
	    }

	    public void updateMaxDepthReached(int depth)
	    {
		if (depth > maxDepthReached)
		{
		    maxDepthReached = depth;
		}
	    }

	    public bool isAnswer(Chain nearParent)
	    {
		bool isAns = false;
		if (answerChain.isEmpty())
		{
		    if (nearParent.isEmpty())
		    {
			proofs.Add(new ProofFinal(nearParent.getProofStep(),
				new Dictionary<Variable, Term>()));
			complete = true;
			isAns = true;
		    }
		}
		else
		{
		    if (nearParent.isEmpty())
		    {
			// This should not happen
			// as added an answer literal to sos, which
			// implies the database (i.e. premises) are
			// unsatisfiable to begin with.
			throw new ApplicationException(
				"Generated an empty chain while looking for an answer, implies original KB is unsatisfiable");
		    }
		    if (1 == nearParent.getNumberLiterals()
			    && nearParent.getHead().getAtomicSentence()
				    .getSymbolicName().Equals(
					    answerChain.getHead()
						    .getAtomicSentence()
						    .getSymbolicName()))
		    {
			Dictionary<Variable, Term> answerBindings = new Dictionary<Variable, Term>();
			List<FOLNode> answerTerms = nearParent.getHead()
				.getAtomicSentence().getArgs();
			int idx = 0;
			foreach (Variable v in answerLiteralVariables)
			{
			    answerBindings.Add(v, (Term)answerTerms[idx]);
			    idx++;
			}
			bool addNewAnswer = true;
			foreach (Proof p in proofs)
			{
			    if (p.getAnswerBindings().Equals(answerBindings))
			    {
				addNewAnswer = false;
				break;
			    }
			}
			if (addNewAnswer)
			{
			    proofs.Add(new ProofFinal(nearParent.getProofStep(),
				    answerBindings));
			}
			isAns = true;
		    }
		}

		if (DateTime.Now.Ticks > finishTime)
		{
		    complete = true;
		    // Indicate that I have run out of query time
		    timedOut = true;
		}

		return isAns;
	    }

	    public override String ToString()
	    {
		StringBuilder sb = new StringBuilder();
		sb.Append("isComplete=" + complete);
		sb.Append("\n");
		sb.Append("result=" + proofs);
		return sb.ToString();
	    }
	}
    }

    class IndexedFarParents
    {
	private int saIdx = 0;
	private Unifier unifier = new Unifier();
	private SubstVisitor substVisitor = new SubstVisitor();
	private Dictionary<String, List<Chain>> posHeads = new Dictionary<String, List<Chain>>();
	private Dictionary<String, List<Chain>> negHeads = new Dictionary<String, List<Chain>>();

	public IndexedFarParents(List<Chain> sos, List<Chain> background)
	{
	    constructInternalDataStructures(sos, background);
	}

	public int getNumberFarParents(Chain farParent)
	{
	    Literal head = farParent.getHead();

	    Dictionary<String, List<Chain>> heads = null;
	    if (head.isPositiveLiteral())
	    {
		heads = posHeads;
	    }
	    else
	    {
		heads = negHeads;
	    }
	    String headKey = head.getAtomicSentence().getSymbolicName();

	    List<Chain> farParents = heads[headKey];
	    if (null != farParents)
	    {
		return farParents.Count;
	    }
	    return 0;
	}

	public void resetNumberFarParentsTo(Chain farParent, int toSize)
	{
	    Literal head = farParent.getHead();
	    Dictionary<String, List<Chain>> heads = null;
	    if (head.isPositiveLiteral())
	    {
		heads = posHeads;
	    }
	    else
	    {
		heads = negHeads;
	    }
	    String key = head.getAtomicSentence().getSymbolicName();
	    List<Chain> farParents = heads[key];
	    while (farParents.Count > toSize)
	    {
		farParents.RemoveAt(farParents.Count - 1);
	    }
	}

	public int getNumberCandidateFarParents(Chain nearParent)
	{
	    Literal nearestHead = nearParent.getHead();

	    Dictionary<String, List<Chain>> candidateHeads = null;
	    if (nearestHead.isPositiveLiteral())
	    {
		candidateHeads = negHeads;
	    }
	    else
	    {
		candidateHeads = posHeads;
	    }

	    String nearestKey = nearestHead.getAtomicSentence().getSymbolicName();

	    List<Chain> farParents = candidateHeads[nearestKey];
	    if (null != farParents)
	    {
		return farParents.Count;
	    }
	    return 0;
	}

	public Chain attemptReduction(Chain nearParent, int farParentIndex)
	{
	    Chain nnpc = null;

	    Literal nearLiteral = nearParent.getHead();

	    Dictionary<String, List<Chain>> candidateHeads = null;
	    if (nearLiteral.isPositiveLiteral())
	    {
		candidateHeads = negHeads;
	    }
	    else
	    {
		candidateHeads = posHeads;
	    }

	    AtomicSentence nearAtom = nearLiteral.getAtomicSentence();
	    String nearestKey = nearAtom.getSymbolicName();
	    List<Chain> farParents = candidateHeads[nearestKey];
	    if (null != farParents)
	    {
		Chain farParent = farParents[farParentIndex];
		standardizeApart(farParent);
		Literal farLiteral = farParent.getHead();
		AtomicSentence farAtom = farLiteral.getAtomicSentence();
		Dictionary<Variable, Term> subst = unifier.unify(nearAtom, farAtom);

		// If I was able to unify with one
		// of the far heads
		if (null != subst)
		{
		    // Want to always apply reduction uniformly
		    Chain topChain = farParent;
		    Literal botLit = nearLiteral;
		    Chain botChain = nearParent;

		    // Need to apply subst to all of the
		    // literals in the reduction
		    List<Literal> reduction = new List<Literal>();
		    foreach (Literal l in topChain.getTail())
		    {
			AtomicSentence atom = (AtomicSentence)substVisitor.subst(
				subst, l.getAtomicSentence());
			reduction.Add(l.newInstance(atom));
		    }
		    reduction.Add(new ReducedLiteral((AtomicSentence)substVisitor
			    .subst(subst, botLit.getAtomicSentence()), botLit
			    .isNegativeLiteral()));
		    foreach (Literal l in botChain.getTail())
		    {
			AtomicSentence atom = (AtomicSentence)substVisitor.subst(
				subst, l.getAtomicSentence());
			reduction.Add(l.newInstance(atom));
		    }

		    nnpc = new Chain(reduction);
		    nnpc.setProofStep(new ProofStepChainReduction(nnpc, nearParent,
			    farParent, subst));
		}
	    }
	    return nnpc;
	}

	public Chain addToIndex(Chain c)
	{
	    Chain added = null;
	    Literal head = c.getHead();
	    if (null != head)
	    {
		Dictionary<String, List<Chain>> toAddTo = null;
		if (head.isPositiveLiteral())
		{
		    toAddTo = posHeads;
		}
		else
		{
		    toAddTo = negHeads;
		}

		String key = head.getAtomicSentence().getSymbolicName();
		List<Chain> farParents = toAddTo[key];
		if (null == farParents)
		{
		    farParents = new List<Chain>();
		    toAddTo.Add(key, farParents);
		}

		added = c;
		farParents.Add(added);
	    }
	    return added;
	}

	public void standardizeApart(Chain c)
	{
	    saIdx = StandardizeApartInPlace.standardizeApart(c, saIdx);
	}

	public override String ToString()
	{
	    StringBuilder sb = new StringBuilder();

	    sb.Append("#");
	    sb.Append(posHeads.Count);
	    foreach (String key in posHeads.Keys)
	    {
		sb.Append(",");
		sb.Append(posHeads[key].Count);
	    }
	    sb.Append(" posHeads=");
	    sb.Append(posHeads.ToString());
	    sb.Append("\n");
	    sb.Append("#");
	    sb.Append(negHeads.Count);
	    foreach (String key in negHeads.Keys)
	    {
		sb.Append(",");
		sb.Append(negHeads[key].Count);
	    }
	    sb.Append(" negHeads=");
	    sb.Append(negHeads.ToString());

	    return sb.ToString();
	}

	// PRIVATE METHODS

	private void constructInternalDataStructures(List<Chain> sos,
		List<Chain> background)
	{
	    List<Chain> toIndex = new List<Chain>();
	    toIndex.AddRange(sos);
	    toIndex.AddRange(background);

	    foreach (Chain c in toIndex)
	    {
		addToIndex(c);
	    }
	}
    }
}