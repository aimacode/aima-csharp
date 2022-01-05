using aima.core.logic.fol.kb;
using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol.inference
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface InferenceProcedure
    {
	/**
	 * 
	 * @param kb
	 *            the knowledge base against which the query is to be made.
	 * @param query
	 *            the query to be answered.
	 * @return an InferenceResult.
	 */
	InferenceResult ask(FOLKnowledgeBase kb, Sentence query);
    }
}