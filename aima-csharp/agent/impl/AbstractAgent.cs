using System;
using aima.core.agent;

namespace aima.core.agent.impl
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
     public abstract class AbstractAgent : Agent
    {
        protected AgentProgram program;
        private bool alive = true;

        public AbstractAgent()
        {

        }

        /**
	     * Constructs an Agent with the specified AgentProgram.
	     * 
	     * @param aProgram
	     *            the Agent's program, which maps any given percept sequences to
	     *            an action.
	     */
        public AbstractAgent(AgentProgram aProgram)
        {
            program = aProgram;
        }

        //START-Agent
        public virtual Action execute(Percept p)
        {
            if(null != program)
            {
                return program.execute(p);
            }
            return NoOpAction.NO_OP;
        }

        public bool isAlive()
        {
            return alive;
        }

        public void setAlive(bool alive)
        {
            this.alive = alive;
        }

        //END-Agent
    }
}