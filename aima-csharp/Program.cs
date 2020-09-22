using aima.core.logic.fol;
using aima.core.logic.fol.domain;
using aima.core.logic.fol.inference;
using aima.core.logic.fol.inference.proof;
using aima.core.logic.fol.kb;
using aima.core.logic.fol.parsing;
using aima.core.logic.fol.parsing.ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aima_csharp
{
    public class Program    {
      
      static String DictToString(Dictionary<Variable, Term> d) {
        StringBuilder sb = new StringBuilder();
        foreach (var kv in d)
        {
          sb.AppendFormat("({0}->{1})", kv.Key, kv.Value);          
        }
        sb.Append("\n");
        return sb.ToString();
      }
      private static void unifierDemo()
      {
        FOLParser parser = new FOLParser(DomainFactory.knowsDomain());
        Unifier unifier = new Unifier();
        var theta = new Dictionary<Variable, Term>();

        Sentence query = parser.parse("Knows(John,x)");
        Sentence johnKnowsJane = parser.parse("Knows(y,Mother(y))");

        System.Console.WriteLine("------------");
        System.Console.WriteLine("Unifier Demo");
        System.Console.WriteLine("------------");
        var subst = unifier.unify(query, johnKnowsJane, theta);
        System.Console.WriteLine("Unify '" + query + "' with '" + johnKnowsJane
            + "' to get the substitution " + DictToString(subst) + ".");
        System.Console.WriteLine("");
      }

      private static void fOL_fcAskDemo()
      {
        System.Console.WriteLine("---------------------------");
        System.Console.WriteLine("Forward Chain, Kings Demo 1");
        System.Console.WriteLine("---------------------------");
        kingsDemo1(new FOLFCAsk());
        System.Console.WriteLine("---------------------------");
        System.Console.WriteLine("Forward Chain, Kings Demo 2");
        System.Console.WriteLine("---------------------------");
        kingsDemo2(new FOLFCAsk());
        System.Console.WriteLine("---------------------------");
        System.Console.WriteLine("Forward Chain, Weapons Demo");
        System.Console.WriteLine("---------------------------");
        weaponsDemo(new FOLFCAsk());
      }

      private static void fOL_bcAskDemo()
      {
        System.Console.WriteLine("----------------------------");
        System.Console.WriteLine("Backward Chain, Kings Demo 1");
        System.Console.WriteLine("----------------------------");
        kingsDemo1(new FOLBCAsk());
        System.Console.WriteLine("----------------------------");
        System.Console.WriteLine("Backward Chain, Kings Demo 2");
        System.Console.WriteLine("----------------------------");
        kingsDemo2(new FOLBCAsk());
        System.Console.WriteLine("----------------------------");
        System.Console.WriteLine("Backward Chain, Weapons Demo");
        System.Console.WriteLine("----------------------------");
        weaponsDemo(new FOLBCAsk());
      }

      

      private static void kingsDemo1(InferenceProcedure ip)
      {
        StandardizeApartIndexicalFactory.flush();

        FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
            .createKingsKnowledgeBase(ip);

        String kbStr = kb.ToString();

        List<Term> terms = new List<Term>();
        terms.Add(new Constant("John"));
        Predicate query = new Predicate("Evil", terms);

        InferenceResult answer = kb.ask(query);

        System.Console.WriteLine("Kings Knowledge Base:");
        System.Console.WriteLine(kbStr);
        System.Console.WriteLine("Query: " + query);
        foreach (Proof p in answer.getProofs())
        {
          System.Console.Write(ProofPrinter.printProof(p));
          System.Console.WriteLine("");
        }
      }

      private static void KingsDemo2(InferenceProcedure ip)
      {
        StandardizeApartIndexicalFactory.flush();

        FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
            .createKingsKnowledgeBase(ip);

        String kbStr = kb.ToString();

        List<Term> terms = new List<Term>();
        terms.Add(new Variable("x"));
        Predicate query = new Predicate("King", terms);

        InferenceResult answer = kb.ask(query);

        System.Console.WriteLine("Kings Knowledge Base:");
        System.Console.WriteLine(kbStr);
        System.Console.WriteLine("Query: " + query);
        foreach (Proof p in answer.getProofs())
        {
          System.Console.Write(ProofPrinter.printProof(p));
        }
      }

      private static void weaponsDemo(InferenceProcedure ip)
      {
        StandardizeApartIndexicalFactory.flush();

        FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
            .createWeaponsKnowledgeBase(ip);

        String kbStr = kb.ToString();

        List<Term> terms = new List<Term>();
        terms.Add(new Variable("x"));
        Predicate query = new Predicate("Criminal", terms);

        InferenceResult answer = kb.ask(query);

        System.Console.WriteLine("Weapons Knowledge Base:");
        System.Console.WriteLine(kbStr);
        System.Console.WriteLine("Query: " + query);
        foreach (Proof p in answer.getProofs())
        {
          System.Console.Write(ProofPrinter.printProof(p));
          System.Console.WriteLine("");
        }
      }

      private static void lovesAnimalDemo(InferenceProcedure ip)
      {
        StandardizeApartIndexicalFactory.flush();

        FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
            .createLovesAnimalKnowledgeBase(ip);

        String kbStr = kb.ToString();

        List<Term> terms = new List<Term>();
        terms.Add(new Constant("Curiosity"));
        terms.Add(new Constant("Tuna"));
        Predicate query = new Predicate("Kills", terms);

        InferenceResult answer = kb.ask(query);

        System.Console.WriteLine("Loves Animal Knowledge Base:");
        System.Console.WriteLine(kbStr);
        System.Console.WriteLine("Query: " + query);
        foreach (Proof p in answer.getProofs())
        {
          System.Console.Write(ProofPrinter.printProof(p));
          System.Console.WriteLine("");
        }
      }

      private static void abcEqualityAxiomDemo(InferenceProcedure ip)
      {
        StandardizeApartIndexicalFactory.flush();

        FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
            .createABCEqualityKnowledgeBase(ip, true);

        String kbStr = kb.ToString();

        TermEquality query = new TermEquality(new Constant("A"), new Constant(
            "C"));

        InferenceResult answer = kb.ask(query);

        System.Console.WriteLine("ABC Equality Axiom Knowledge Base:");
        System.Console.WriteLine(kbStr);
        System.Console.WriteLine("Query: " + query);
        foreach (Proof p in answer.getProofs())
        {
          System.Console.Write(ProofPrinter.printProof(p));
          System.Console.WriteLine("");
        }
      }

      private static void abcEqualityNoAxiomDemo(InferenceProcedure ip)
      {
        StandardizeApartIndexicalFactory.flush();

        FOLKnowledgeBase kb = FOLKnowledgeBaseFactory
            .createABCEqualityKnowledgeBase(ip, false);

        String kbStr = kb.ToString();

        TermEquality query = new TermEquality(new Constant("A"), new Constant(
            "C"));

        InferenceResult answer = kb.ask(query);

        System.Console.WriteLine("ABC Equality No Axiom Knowledge Base:");
        System.Console.WriteLine(kbStr);
        System.Console.WriteLine("Query: " + query);
        foreach (Proof p in answer.getProofs())
        {
          System.Console.Write(ProofPrinter.printProof(p));
          System.Console.WriteLine("");
        }
      }
    
    static void kingsDemo2(InferenceProcedure ip)
        {
            StandardizeApartIndexicalFactory.flush();

            FOLKnowledgeBase kb = FOLKnowledgeBaseFactory.createKingsKnowledgeBase(ip);

            String kbStr = kb.ToString();

            List<Term> terms = new List<Term>();
            terms.Add(new Variable("x"));
            Predicate query = new Predicate("King", terms);

            InferenceResult answer = kb.ask(query);

            System.Console.WriteLine("Kings Knowledge Base:");
            System.Console.WriteLine(kbStr);
            System.Console.WriteLine("Query: " + query);
            foreach (Proof p in answer.getProofs())
            {
                System.Console.WriteLine(ProofPrinter.printProof(p));
            }
        }

        static void Main(string[] args)
        {            
            unifierDemo();
            fOL_bcAskDemo();
    }

  }
}

