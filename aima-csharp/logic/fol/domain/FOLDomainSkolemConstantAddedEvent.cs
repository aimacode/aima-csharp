using System;

namespace aima.core.logic.fol.domain
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class FOLDomainSkolemConstantAddedEvent : FOLDomainEvent
    {
	private static readonly long serialVersionUID = 1L;

	private String skolemConstantName;

	public FOLDomainSkolemConstantAddedEvent(Object source,
			String skolemConstantName): base(source)
	{	    
	    this.skolemConstantName = skolemConstantName;
	}

	public String getSkolemConstantName()
	{
	    return skolemConstantName;
	}
	
	public override void notifyListener(FOLDomainListener listener)
	{
	    listener.skolemConstantAdded(this);
	}
    }
}