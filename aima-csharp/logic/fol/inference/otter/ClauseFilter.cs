using aima.core.logic.fol.kb.data;

namespace aima.core.logic.fol.inference.otter
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface ClauseFilter
    {
	HashSet<Clause> filter(HashSet<Clause> clauses);
    }
}