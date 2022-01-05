namespace aima.core.logic.fol.domain
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class FOLDomain
    {
	private HashSet<String> constants, functions, predicates;
	private int skolemConstantIndexical = 0;
	private int skolemFunctionIndexical = 0;
	private int answerLiteralIndexical = 0;
	private List<FOLDomainListener> listeners = new List<FOLDomainListener>();

	public FOLDomain()
	{
	    this.constants = new HashSet<String>();
	    this.functions = new HashSet<String>();
	    this.predicates = new HashSet<String>();
	}

	public FOLDomain(FOLDomain toCopy): this(toCopy.getConstants(), toCopy.getFunctions(), toCopy
			    .getPredicates())
	{
	    
	}

	public FOLDomain(HashSet<String> constants, HashSet<String> functions,
			HashSet<String> predicates)
	{
	    this.constants = new HashSet<String>(constants);
	    this.functions = new HashSet<String>(functions);
	    this.predicates = new HashSet<String>(predicates);
	}

	public HashSet<String> getConstants()
	{
	    return constants;
	}

	public HashSet<String> getFunctions()
	{
	    return functions;
	}

	public HashSet<String> getPredicates()
	{
	    return predicates;
	}

	public void addConstant(String constant)
	{
	    constants.Add(constant);
	}

	public String addSkolemConstant()
	{

	    String sc = null;
	    do
	    {
		sc = "SC" + (skolemConstantIndexical++);
	    } while (constants.Contains(sc) || functions.Contains(sc)
			    || predicates.Contains(sc));

	    addConstant(sc);
	    notifyFOLDomainListeners(new FOLDomainSkolemConstantAddedEvent(this, sc));

	    return sc;
	}

	public void addFunction(String function)
	{
	    functions.Add(function);
	}

	public String addSkolemFunction()
	{
	    String sf = null;
	    do
	    {
		sf = "SF" + (skolemFunctionIndexical++);
	    } while (constants.Contains(sf) || functions.Contains(sf)
			    || predicates.Contains(sf));

	    addFunction(sf);
	    notifyFOLDomainListeners(new FOLDomainSkolemFunctionAddedEvent(this, sf));

	    return sf;
	}

	public void addPredicate(String predicate)
	{
	    predicates.Add(predicate);
	}

	public String addAnswerLiteral()
	{
	    String al = null;
	    do
	    {
		al = "Answer" + (answerLiteralIndexical++);
	    } while (constants.Contains(al) || functions.Contains(al)
			    || predicates.Contains(al));

	    addPredicate(al);
	    notifyFOLDomainListeners(new FOLDomainAnswerLiteralAddedEvent(this, al));

	    return al;
	}

	public void addFOLDomainListener(FOLDomainListener listener)
	{
	    lock (listeners) 
	    {
		if (!listeners.Contains(listener))
		{
		    listeners.Add(listener);
		}
	    }
	}

	public void removeFOLDomainListener(FOLDomainListener listener)
	{
	    lock (listeners) 
	    {
		listeners.Remove(listener);
	    }
	}

	// PRIVATE METHODS
	
	private void notifyFOLDomainListeners(FOLDomainEvent evt)
	{
	    lock (listeners)
	    {
		foreach (FOLDomainListener l in listeners)
		{
		    evt.notifyListener(l);
		}
	    }
	}
    }
}