using System.Text;

namespace aima.core.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofPrinter
    {
	/**
	 * Utility method for outputting proofs in a formatted textual
	 * representation.
	 * 
	 * @param proof
	 * @return a String representation of the Proof.
	 */
	public static String printProof(Proof proof)
	{
	    StringBuilder sb = new StringBuilder();

	    sb.Append("Proof, Answer Bindings: ");
	    sb.Append(proof.getAnswerBindings());
	    sb.Append("\n");

	    List<ProofStep> steps = proof.getSteps();

	    int maxStepWidth = "Step".Length;
	    int maxProofWidth = "Proof".Length;
	    int maxJustificationWidth = "Justification".Length;

	    // Calculate the maximum width for each column in the proof
	    foreach (ProofStep step in steps)
	    {
		String sn = "" + step.getStepNumber();
		if (sn.Length > maxStepWidth)
		{
		    maxStepWidth = sn.Length;
		}
		if (step.getProof().Length > maxProofWidth)
		{
		    maxProofWidth = step.getProof().Length;
		}
		if (step.getJustification().Length > maxJustificationWidth)
		{
		    maxJustificationWidth = step.getJustification().Length;
		}
	    }

	    // Give a little extra padding
	    maxStepWidth += 1;
	    maxProofWidth += 1;
	    maxJustificationWidth += 1;

	    String f = "|%-" + maxStepWidth + "s| %-" + maxProofWidth + "s|%-"
		    + maxJustificationWidth + "s|\n";

	    int barWidth = 5 + maxStepWidth + maxProofWidth + maxJustificationWidth;
	    StringBuilder bar = new StringBuilder();
	    for (int i = 0; i < barWidth; i++)
	    {
		bar.Append("-");
	    }
	    bar.Append("\n");

	    sb.Append(bar);
	    sb.Append(String.Format(f, "Step", "Proof", "Justification"));
	    sb.Append(bar);
	    foreach (ProofStep step in steps)
	    {
		sb.Append(String.Format(f, "" + step.getStepNumber(), step
			.getProof(), step.getJustification()));
	    }
	    sb.Append(bar);
	    return sb.ToString();
	}
    }
}