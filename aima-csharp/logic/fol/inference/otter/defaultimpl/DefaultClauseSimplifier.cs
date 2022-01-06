using aima.core.logic.fol.kb.data;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.inference.otter.defaultimpl
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class DefaultClauseSimplifier : ClauseSimplifier
    {
	private Demodulation demodulation = new Demodulation();
	private List<TermEquality> rewrites = new List<TermEquality>();

	public DefaultClauseSimplifier()
	{

	}

	public DefaultClauseSimplifier(List<TermEquality> rewrites)
	{
	    this.rewrites.AddRange(rewrites);
	}

	// START-ClauseSimplifier

	public Clause simplify(Clause c)
	{
	    Clause simplified = c;

	    // Apply each of the rewrite rules to
	    // the clause
	    foreach (TermEquality te in rewrites)
	    {
		Clause dc = simplified;
		// Keep applying the rewrite as many times as it
		// can be applied before moving on to the next one.
		while (null != (dc = demodulation.apply(te, dc)))
		{
		    simplified = dc;
		}
	    }

	    return simplified;
	}

	// END-ClauseSimplifier
    }
}