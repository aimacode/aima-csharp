# ![](https://github.com/aimacode/aima-java/blob/gh-pages/aima3e/images/aima3e.jpg)aima-csharp
C# implementation of algorithms from [Russell](http://www.cs.berkeley.edu/~russell/) And [Norvig's](http://www.norvig.com/) [Artificial Intelligence - A Modern Approach 3rd Edition](http://aima.cs.berkeley.edu/). You can use this in conjunction with a course on AI, or for study on your own.

## Index of Implemented Algorithms

Here is a table of algorithms, the figure, name of the code in the book, and the file where they are implemented in the code. This chart was made for the third edition of the book and needs to be updated for the upcoming fourth edition. Empty implementations are a good place for contributors to look for an issue. The [aima-pseudocode](https://github.com/aimacode/aima-pseudocode) project describes all the algorithms from the book.

|Fig|Page|Name (in book)|Code|
| -------- |:--------:| :-----| :----- |
|2|34|Environment|[Environment](/aima-csharp/agent/Environment.cs)|
|2.1|35|Agent|[Agent](/aima-csharp/agent/Agent.cs)|
|2.3|36|Table-Driven-Vacuum-Agent||
|2.7|47|Table-Driven-Agent|[TableDrivenAgentProgram](/aima-csharp/agent/impl/aprog/TableDrivenAgentProgram.cs)|
|2.8|48|Reflex-Vacuum-Agent|[?ReflexVacuumAgent?](/aima-csharp/agent/impl/aprog/ModelBasedReflexAgentProgram.cs)|
|2.10|49|Simple-Reflex-Agent|[SimpleReflexAgentProgram](/aima-csharp/agent/impl/aprog/SimpleReflexAgentProgram.cs)|
|2.12|51|Model-Based-Reflex-Agent|[ModelBasedReflexAgentProgram](/aima-csharp/agent/impl/aprog/ModelBasedReflexAgentProgram.cs)|
|3|66|Problem|
|3.1|67|Simple-Problem-Solving-Agent|[SimpleProblemSolvingAgent](/aima-csharp/search/framework/SimpleProblemSolvingAgent.cs)|
|3.2|68|Romania|[SimplifiedRoadMapOfPartOfRomania](/aima-csharp/environment/map/SimplifiedRoadMapOfPartOfRomania.cs)|
|3.7|77|Tree-Search|[TreeSearch](/aima-csharp/search/framework/qsearch/TreeSearch.cs)|
|3.7|77|Graph-Search|[GraphSearch](/aima-csharp/search/framework/qsearch/GraphSearch.cs)|
|3.10|79|Node|[Node](/aima-csharp/search/framework/Node.cs)|
|3.11|82|Breadth-First-Search|
|3.14|84|Uniform-Cost-Search|
|3|85|Depth-first Search|
|3.17|88|Depth-Limited-Search|
|3.18|89|Iterative-Deepening-Search|
|3|90|Bidirectional search|
|3|92|Best-First search|
|3|92|Greedy best-First search|
|3|93|A\* Search|
|3.26|99|Recursive-Best-First-Search |
|4.2|122|Hill-Climbing|
|4.5|126|Simulated-Annealing|
|4.8|129|Genetic-Algorithm|
|4.11|136|And-Or-Graph-Search|
|4|147|Online search problem|[OnlineSearchProblem](/aima-csharp/search/online/OnlineSearchProblem.cs)|
|4.21|150|Online-DFS-Agent|[OnlineDFSAgent](/aima-csharp/search/online/OnlineDFSAgent.cs)|
|4.24|152|LRTA\*-Agent|[LRTAStarAgent](/aima-csharp/search/online/LRTAStarAgent.cs)|
|5.3|166|Minimax-Decision|
|5.7|170|Alpha-Beta-Search|
|6|202|CSP|
|6.1|204|Map CSP|
|6.3|209|AC-3|
|6.5|215|Backtracking-Search|
|6.8|221|Min-Conflicts|
|6.11|224|Tree-CSP-Solver|
|7|235|Knowledge Base|
|7.1|236|KB-Agent|[KBAgent](/aima-csharp/logic/propositional/agent/KBAgent.cs)|
|7.7|244|Propositional-Logic-Sentence|[Sentence](/aima-csharp/logic/propositional/parsing/ast/Sentence.cs)|
|7.10|248|TT-Entails|
|7|253|Convert-to-CNF|
|7.12|255|PL-Resolution|
|7.15|258|PL-FC-Entails?|
|7.17|261|DPLL-Satisfiable?|
|7.18|263|WalkSAT|
|7.20|270|Hybrid-Wumpus-Agent|
|7.22|272|SATPlan|
|9|323|Subst|
|9.1|328|Unify|[Unifier](/aima-csharp/logic/fol/Unifier.cs)|
|9.3|332|FOL-FC-Ask|[FOLFCAsk](/aima-csharp/logic/fol/inference/FOLFCAsk.cs)|
|9.3|332|FOL-BC-Ask|[FOLBCAsk](/aima-csharp/logic/fol/inference/FOLBCAsk.cs)|
|9|345|CNF|[CNFConverter](/aima-csharp/logic/fol/CNFConverter.cs)|
|9|347|Resolution|[FOLTFMResolution](/aima-csharp/logic/fol/inference/FOLTFMResolution.cs)|
|9|354|Demodulation||
|9|354|Paramodulation|[Paramodulation](/aima-csharp/logic/fol/inference/Paramodulation.cs)|
|9|345|Subsumption|[SubsumptionElimination](/aima-csharp/logic/fol/SubsumptionElimination.cs)|
|10.9|383|Graphplan|---|
|11.5|409|Hierarchical-Search|---|
|11.8|414|Angelic-Search|---|
|13.1|484|DT-Agent|---|
|13|484|Probability-Model|
|13|487|Probability-Distribution|
|13|490|Full-Joint-Distribution|
|14|510|Bayesian Network|
|14.9|525|Enumeration-Ask|
|14.11|528|Elimination-Ask|
|14.13|531|Prior-Sample|
|14.14|533|Rejection-Sampling|
|14.15|534|Likelihood-Weighting|
|14.16|537|GIBBS-Ask|
|15.4|576|Forward-Backward|
|15|578|Hidden Markov Model|
|15.6|580|Fixed-Lag-Smoothing|
|15|590|Dynamic Bayesian Network|
|15.17|598|Particle-Filtering|
|16.9|632|Information-Gathering-Agent|---|
|17|647|Markov Decision Process|
|17.4|653|Value-Iteration|
|17.7|657|Policy-Iteration|
|17.9|663|POMDP-Value-Iteration|---|
|18.5|702|Decision-Tree-Learning|
|18.8|710|Cross-Validation-Wrapper|---|
|18.11|717|Decision-List-Learning|
|18.24|734|Back-Prop-Learning|
|18.34|751|AdaBoost|
|19.2|771|Current-Best-Learning|---|
|19.3|773|Version-Space-Learning|---|
|19.8|786|Minimal-Consistent-Det|---|
|19.12|793|FOIL|---|
|21.2|834|Passive-ADP-Agent|
|21.4|837|Passive-TD-Agent|
|21.8|844|Q-Learning-Agent|
|22.1|871|HITS|
|23.5|894|CYK-Parse|
|25.9|982|Monte-Carlo-Localization|

# Index of data structures

Here is a table of the implemented data structures, the figure, name of the implementation in the repository, and the file where they are implemented.

| **Figure** | **Name (in repository)** | **File** |
|:-----------|:-------------------------|:---------|
| 3.2    | romania_map              | |
| 4.9    | vacumm_world             | |
| 4.23   | one_dim_state_space      | |
| 6.1    | australia_map            | |
| 7.13   | wumpus_world_inference   | |
| 7.16   | horn_clauses_KB          | |
| 17.1   | sequential_decision_environment | |
| 18.2   | waiting_decision_tree    | |
