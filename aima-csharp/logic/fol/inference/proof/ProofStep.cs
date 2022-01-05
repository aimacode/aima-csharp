namespace aima.core.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface ProofStep
    {
	int getStepNumber();

	void setStepNumber(int step);

	List<ProofStep> getPredecessorSteps();

	String getProof();

	String getJustification();
    }
}