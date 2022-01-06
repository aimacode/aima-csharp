using System.Text;
using aima.core.logic.fol.inference;
using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing;
using aima.core.logic.fol.parsing.ast;
using aima.core.logic.fol.domain;
using aima.core.logic.fol.inference.proof;

namespace aima.core.logic.fol.kb
{
    /**
     * A First Order Logic (FOL) Knowledge Base.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class FOLKnowledgeBase
    {
	private FOLParser parser;
	private InferenceProcedure inferenceProcedure;
	private Unifier unifier;
	private SubstVisitor substVisitor;
	private VariableCollector variableCollector;
	private StandardizeApart _standardizeApart;
	private CNFConverter cnfConverter;
	
	// Persistent data structures	
	// Keeps track of the Sentences in their original form as added to the
	// Knowledge base.
	private List<Sentence> originalSentences = new List<Sentence>();
	// The KB in clause form
	private List<Clause> clauses = new List<Clause>();
	// Keep track of all of the definite clauses in the database
	// along with those that represent implications.
	private List<Clause> allDefiniteClauses = new List<Clause>();
	private List<Clause> implicationDefiniteClauses = new List<Clause>();
	// All the facts in the KB indexed by Atomic Sentence name (Note: pg. 279)
	private Dictionary<String, List<Literal>> indexFacts = new Dictionary<String, List<Literal>>();
	// Keep track of indexical keys for uniquely standardizing apart sentences
	private StandardizeApartIndexical variableIndexical = StandardizeApartIndexicalFactory
		.newStandardizeApartIndexical('v');
	private StandardizeApartIndexical queryIndexical = StandardizeApartIndexicalFactory
		.newStandardizeApartIndexical('q');
   
	
	// PUBLIC METHODS
	
	public FOLKnowledgeBase(FOLDomain domain) : this(domain, new FOLOTTERLikeTheoremProver())
	{
	    // Default to Full Resolution if not set.

	}

	public FOLKnowledgeBase(FOLDomain domain,
		InferenceProcedure inferenceProcedure) : this(domain, inferenceProcedure, new Unifier())
	{

	}

	public FOLKnowledgeBase(FOLDomain domain,
		InferenceProcedure inferenceProcedure, Unifier unifier)
	{
	    this.parser = new FOLParser(new FOLDomain(domain));
	    this.inferenceProcedure = inferenceProcedure;
	    this.unifier = unifier;
	    //
	    this.substVisitor = new SubstVisitor();
	    this.variableCollector = new VariableCollector();
	    this._standardizeApart = new StandardizeApart(variableCollector,
		    substVisitor);
	    this.cnfConverter = new CNFConverter(parser);
	}

	public void clear()
	{
	    this.originalSentences.Clear();
	    this.clauses.Clear();
	    this.allDefiniteClauses.Clear();
	    this.implicationDefiniteClauses.Clear();
	    this.indexFacts.Clear();
	}

	public InferenceProcedure getInferenceProcedure()
	{
	    return inferenceProcedure;
	}

	public void setInferenceProcedure(InferenceProcedure inferenceProcedure)
	{
	    if (null != inferenceProcedure)
	    {
		this.inferenceProcedure = inferenceProcedure;
	    }
	}

	public Sentence tell(String aSentence)
	{
	    Sentence s = parser.parse(aSentence);
	    tell(s);
	    return s;
	}

	public void tell(List<Sentence> sentences)
	{
	    foreach (Sentence s in sentences)
	    {
		tell(s);
	    }
	}

	public void tell(Sentence aSentence)
	{
	    store(aSentence);
	}

	/**
	* 
	* @param aQuerySentence
	* @return an InferenceResult.
	*/
	public InferenceResult ask(String aQuerySentence)
	{
	    return ask(parser.parse(aQuerySentence));
	}

	public InferenceResult ask(Sentence aQuery)
	{
	    // Want to standardize apart the query to ensure
	    // it does not clash with any of the sentences
	    // in the database
	    StandardizeApartResult saResult = _standardizeApart.standardizeApart(
		    aQuery, queryIndexical);

	    // Need to map the result variables (as they are standardized apart)
	    // to the original queries variables so that the caller can easily
	    // understand and use the returned set of substitutions
	    InferenceResult infResult = getInferenceProcedure().ask(this,
		    saResult.getStandardized());
	    List<Proof> proofs = infResult.getProofs();
	    foreach (Proof p in proofs)
	    {
		Dictionary<Variable, Term> im = p.getAnswerBindings();
		Dictionary<Variable, Term> em = new Dictionary<Variable, Term>();
		foreach (Variable rev in saResult.getReverseSubstitution().Keys)
		{
		    em.Add((Variable)saResult.getReverseSubstitution()[rev],
			    im[rev]);
		}
		p.replaceAnswerBindings(em);
	    }
	    return infResult;
	}

	public int getNumberFacts()
	{
	    return allDefiniteClauses.Count - implicationDefiniteClauses.Count;
	}

	public int getNumberRules()
	{
	    return clauses.Count - getNumberFacts();
	}

	public List<Sentence> getOriginalSentences()
	{
	    return new System.Collections.ObjectModel.ReadOnlyCollection<Sentence>(originalSentences).ToList<Sentence>();
	}

	public List<Clause> getAllDefiniteClauses()
	{
	    return new System.Collections.ObjectModel.ReadOnlyCollection<Clause>(allDefiniteClauses).ToList<Clause>();
	}

	public List<Clause> getAllDefiniteClauseImplications()
	{
	    return new System.Collections.ObjectModel.ReadOnlyCollection<Clause>(implicationDefiniteClauses).ToList<Clause>();
	}

	public List<Clause> getAllClauses()
	{
	    return new System.Collections.ObjectModel.ReadOnlyCollection<Clause>(clauses).ToList<Clause>();
	}

	// Note: pg 278, FETCH(q) concept.
	public /* lock */ List<Dictionary<Variable, Term>> fetch(Literal l)
	{
	    // Get all of the substitutions in the KB that p unifies with
	    List<Dictionary<Variable, Term>> allUnifiers = new List<Dictionary<Variable, Term>>();

	    List<Literal> matchingFacts = fetchMatchingFacts(l);
	    if (null != matchingFacts)
	    {
		foreach (Literal fact in matchingFacts)
		{
		    Dictionary<Variable, Term> substitution = unifier.unify(l
			    .getAtomicSentence(), fact.getAtomicSentence());
		    if (null != substitution)
		    {
			allUnifiers.Add(substitution);
		    }
		}
	    }
	    return allUnifiers;
	}

	// Note: To support FOL-FC-Ask
	public List<Dictionary<Variable, Term>> fetch(List<Literal> literals)
	{
	    List<Dictionary<Variable, Term>> possibleSubstitutions = new List<Dictionary<Variable, Term>>();

	    if (literals.Count > 0)
	    {
		Literal first = literals[0];
		List<Literal> rest = literals.Skip(1).ToList<Literal>();

		recursiveFetch(new Dictionary<Variable, Term>(), first, rest,
			possibleSubstitutions);
	    }
	    return possibleSubstitutions;
	}

	public Dictionary<Variable, Term> unify(FOLNode x, FOLNode y)
	{
	    return unifier.unify(x, y);
	}

	public Sentence subst(Dictionary<Variable, Term> theta, Sentence aSentence)
	{
	    return substVisitor.subst(theta, aSentence);
	}

	public Literal subst(Dictionary<Variable, Term> theta, Literal l)
	{
	    return substVisitor.subst(theta, l);
	}

	public Term subst(Dictionary<Variable, Term> theta, Term aTerm)
	{
	    return substVisitor.subst(theta, aTerm);
	}

	// Note: see page 277.
	public Sentence standardizeApart(Sentence aSentence)
	{
	    return _standardizeApart.standardizeApart(aSentence, variableIndexical)
		    .getStandardized();
	}

	public Clause standardizeApart(Clause aClause)
	{
	    return _standardizeApart.standardizeApart(aClause, variableIndexical);
	}

	public Chain standardizeApart(Chain aChain)
	{
	    return _standardizeApart.standardizeApart(aChain, variableIndexical);
	}

	public List<Variable> collectAllVariables(Sentence aSentence)
	{
	    return variableCollector.collectAllVariables(aSentence);
	}

	public CNF convertToCNF(Sentence aSentence)
	{
	    return cnfConverter.convertToCNF(aSentence);
	}

	public List<Clause> convertToClauses(Sentence aSentence)
	{
	    CNF cnf = cnfConverter.convertToCNF(aSentence);

	    return new List<Clause>(cnf.getConjunctionOfClauses());
	}

	public Literal createAnswerLiteral(Sentence forQuery)
	{
	    String alName = parser.getFOLDomain().addAnswerLiteral();
	    List<Term> terms = new List<Term>();

	    List<Variable> vars = variableCollector.collectAllVariables(forQuery);
	    foreach (Variable v in vars)
	    {
		// Ensure copies of the variables are used.
		terms.Add((Term)v.copy());
	    }
	    return new Literal(new Predicate(alName, terms));
	}

	// Note: see pg. 281
	public bool isRenaming(Literal l)
	{
	    List<Literal> possibleMatches = fetchMatchingFacts(l);
	    if (null != possibleMatches)
	    {
		return isRenaming(l, possibleMatches);
	    }
	    return false;
	}

	// Note: see pg. 281
	public bool isRenaming(Literal l, List<Literal> possibleMatches)
	{

	    foreach (Literal q in possibleMatches)
	    {
		if (l.isPositiveLiteral() != q.isPositiveLiteral())
		{
		    continue;
		}
		Dictionary<Variable, Term> subst = unifier.unify(l.getAtomicSentence(), q
			.getAtomicSentence());
		if (null != subst)
		{
		    int cntVarTerms = 0;
		    foreach (Term t in subst.Values)
		    {
			if (t is Variable)
			{
			    cntVarTerms++;
			}
		    }
		    // If all the substitutions, even if none, map to Variables
		    // then this is a renaming
		    if (subst.Count == cntVarTerms)
		    {
			return true;
		    }
		}
	    }
	    return false;
	}

	public override String ToString()
	{
	    StringBuilder sb = new StringBuilder();
	    foreach (Sentence s in originalSentences)
	    {
		sb.Append(s.ToString());
		sb.Append("\n");
	    }
	    return sb.ToString();
	}

	
	// PROTECTED METHODS
	
	protected FOLParser getParser()
	{
	    return parser;
	}

	
	// PRIVATE METHODS
	
	// Note: pg 278, STORE(s) concept.
	private /*lock*/ void store(Sentence aSentence)
	{
	    originalSentences.Add(aSentence);

	    // Convert the sentence to CNF
	    CNF cnfOfOrig = cnfConverter.convertToCNF(aSentence);
	    List<Clause> conjunctionOfClauses = cnfOfOrig.getConjunctionOfClauses();
	    foreach (Clause c in conjunctionOfClauses)
	    {
		c.setProofStep(new ProofStepClauseClausifySentence(c, aSentence));
		if (c.isEmpty())
		{
		    // This should not happen, if so the user
		    // is trying to add an unsatisfiable sentence
		    // to the KB.
		    throw new ArgumentException(
			    "Attempted to add unsatisfiable sentence to KB, orig=["
				    + aSentence + "] CNF=" + cnfOfOrig);
		}

		// Ensure all clauses added to the KB are Standardized Apart.
		Clause c2 = _standardizeApart.standardizeApart(c, variableIndexical);

		// Will make all clauses immutable
		// so that they cannot be modified externally.
		c2.setImmutable();
		if (!clauses.Contains(c2))
		{
		    clauses.Add(c2);
		    // If added keep track of special types of
		    // clauses, as useful for query purposes
		    if (c2.isDefiniteClause())
		    {
			allDefiniteClauses.Add(c2);
		    }
		    if (c2.isImplicationDefiniteClause())
		    {
			implicationDefiniteClauses.Add(c2);
		    }
		    if (c2.isUnitClause())
		    {
			indexFact(c2.getLiterals().First<Literal>());
		    }
		}
	    }
	}

	// Only if it is a unit clause does it get indexed as a fact
	// see pg. 279 for general idea.
	private void indexFact(Literal fact)
	{
	    String factKey = getFactKey(fact);
	    if (!indexFacts.ContainsKey(factKey))
	    {
		indexFacts.Add(factKey, new List<Literal>());
	    }
	    indexFacts[factKey].Add(fact);
	}

	private void recursiveFetch(Dictionary<Variable, Term> theta, Literal l,
		List<Literal> remainingLiterals,
		List<Dictionary<Variable, Term>> possibleSubstitutions)
	{

	    // Find all substitutions for current predicate based on the
	    // substitutions of prior predicates in the list (i.e. SUBST with
	    // theta).
	    List<Dictionary<Variable, Term>> pSubsts = fetch(subst(theta, l));

	    // No substitutions, therefore cannot continue
	    if (null == pSubsts)
	    {
		return;
	    }

	    foreach (Dictionary<Variable, Term> psubst in pSubsts)
	    {
		// Ensure all prior substitution information is maintained
		// along the chain of predicates (i.e. for shared variables
		// across the predicates).
		foreach (Variable key in theta.Keys)
		{
		    psubst.Add(key, theta[key]);
		}
		if (remainingLiterals.Count == 0)
		{
		    // This means I am at the end of the chain of predicates
		    // and have found a valid substitution.
		    possibleSubstitutions.Add(psubst);
		}
		else
		{
		    // Need to move to the next link in the chain of substitutions
		    Literal first = remainingLiterals[0];
		    List<Literal> rest = remainingLiterals.Skip(1).ToList<Literal>();

		    recursiveFetch(psubst, first, rest, possibleSubstitutions);
		}
	    }
	}

	private List<Literal> fetchMatchingFacts(Literal l)
	{
	    if (!indexFacts.ContainsKey(getFactKey(l)))
	    {
		return null;
	    }
	    return indexFacts[getFactKey(l)];
	}

	private String getFactKey(Literal l)
	{
	    StringBuilder key = new StringBuilder();
	    if (l.isPositiveLiteral())
	    {
		key.Append("+");
	    }
	    else
	    {
		key.Append("-");
	    }
	    key.Append(l.getAtomicSentence().getSymbolicName());
	    return key.ToString();
	}
    }
}