using aima.core.search.framework.problem;

namespace aima.core.environment.eightpuzzle
{
    /**
     * @author Ruediger Lunde
     * 
     */
    public class BidirectionalEightPuzzleProblem : Problem, BidirectionalProblem
    {
	Problem reverseProblem;

	public BidirectionalEightPuzzleProblem(EightPuzzleBoard initialState): base(initialState, EightPuzzleFunctionFactory.getActionsFunction(),
				EightPuzzleFunctionFactory.getResultFunction(),
				new DefaultGoalTest(new EightPuzzleBoard(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 })))
	{
	    reverseProblem = new Problem(new EightPuzzleBoard(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }),
				EightPuzzleFunctionFactory.getActionsFunction(), EightPuzzleFunctionFactory.getResultFunction(),
				new DefaultGoalTest(initialState));
	}

	public Problem getOriginalProblem()
	{
	    return this;
	}

	public Problem getReverseProblem()
	{
	    return reverseProblem;
	}
    }
}