using aima.core.agent.impl;

namespace aima.core.search.framework
{
    /**
     * A NoOp action that indicates a CutOff has occurred in a search. Used
     * primarily by DepthLimited and IterativeDeepening search routines.
     * 
     * @author Ciaran O'Reilly
     */
    public class CutOffIndicatorAction : DynamicAction
    {
        public static readonly CutOffIndicatorAction CUT_OFF = new CutOffIndicatorAction();

        // START-Action
        public bool isNoOp()
        {
            return true;
        }

        // END-Action
        private CutOffIndicatorAction(): base("CutOff")
        {

        }
    }
}