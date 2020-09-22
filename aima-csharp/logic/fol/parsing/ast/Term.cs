using System.Collections.Generic;

namespace aima.core.logic.fol.parsing.ast
{
  /**
   * @author Ravi Mohan
   * @author Ciaran O'Reilly
   */
  public interface Term : FOLNode
  {
    List<Term> getArgs();

    Term copy();
  }
}