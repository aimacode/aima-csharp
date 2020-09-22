using System.Collections.Generic;

namespace aima.core.logic.fol.parsing.ast
{
  /**
   * @author Ciaran O'Reilly
   * 
   */
  public interface AtomicSentence : Sentence
  {
    List<Term> getArgs();

    AtomicSentence copy();
  }
}