using aima.core.logic.fol.parsing.ast;

namespace aima.core.logic.fol
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 9.1, page
     * 328.<br>
     * <br>
     * 
     * <pre>
     * function UNIFY(x, y, theta) returns a substitution to make x and y identical
     *   inputs: x, a variable, constant, list, or compound
     *           y, a variable, constant, list, or compound
     *           theta, the substitution built up so far (optional, defaults to empty)
     *           
     *   if theta = failure then return failure
     *   else if x = y the return theta
     *   else if VARIABLE?(x) then return UNIVY-VAR(x, y, theta)
     *   else if VARIABLE?(y) then return UNIFY-VAR(y, x, theta)
     *   else if COMPOUND?(x) and COMPOUND?(y) then
     *       return UNIFY(x.ARGS, y.ARGS, UNIFY(x.OP, y.OP, theta))
     *   else if LIST?(x) and LIST?(y) then
     *       return UNIFY(x.REST, y.REST, UNIFY(x.FIRST, y.FIRST, theta))
     *   else return failure
     *   
     * ---------------------------------------------------------------------------------------------------
     * 
     * function UNIFY-VAR(var, x, theta) returns a substitution
     *            
     *   if {var/val} E theta then return UNIFY(val, x, theta)
     *   else if {x/val} E theta then return UNIFY(var, val, theta)
     *   else if OCCUR-CHECK?(var, x) then return failure
     *   else return add {var/x} to theta
     * </pre>
     * 
     * Figure 9.1 The unification algorithm. The algorithm works by comparing the
     * structures of the inputs, elements by element. The substitution theta that is
     * the argument to UNIFY is built up along the way and is used to make sure that
     * later comparisons are consistent with bindings that were established earlier.
     * In a compound expression, such as F(A, B), the OP field picks out the
     * function symbol F and the ARGS field picks out the argument list (A, B).
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * @author Mike Stampone
     * 
     */
    public class Unifier
    {
	private static SubstVisitor _substVisitor = new SubstVisitor();
        private VariableCollector _variableCollector;

        public Unifier()
	{

	}

	/**
	 * Returns a Dictionary<Variable, Term> representing the substitution (i.e. a set
	 * of variable/term pairs) or null which is used to indicate a failure to
	 * unify.
	 * 
	 * @param x
	 *            a variable, constant, list, or compound
	 * @param y
	 *            a variable, constant, list, or compound
	 * 
	 * @return a Dictionary<Variable, Term> representing the substitution (i.e. a set
	 *         of variable/term pairs) or null which is used to indicate a
	 *         failure to unify.
	 */
	public Dictionary<Variable, Term> unify(FOLNode x, FOLNode y)
	{
	    return unify(x, y, new Dictionary<Variable, Term>());
	}

	/**
	 * Returns a Dictionary<Variable, Term> representing the substitution (i.e. a set
	 * of variable/term pairs) or null which is used to indicate a failure to
	 * unify.
	 * 
	 * @param x
	 *            a variable, constant, list, or compound
	 * @param y
	 *            a variable, constant, list, or compound
	 * @param theta
	 *            the substitution built up so far
	 * 
	 * @return a Dictionary<Variable, Term> representing the substitution (i.e. a set
	 *         of variable/term pairs) or null which is used to indicate a
	 *         failure to unify.
	 */
	public Dictionary<Variable, Term> unify(FOLNode x, FOLNode y,
		Dictionary<Variable, Term> theta)
	{
	    // if theta = failure then return failure
	    if (theta == null)
	    {
		return null;
	    }
	    else if (x.Equals(y))
	    {
		// else if x = y then return theta
		return theta;
	    }
	    else if (x is Variable)
	    {
		// else if VARIABLE?(x) then return UNIVY-VAR(x, y, theta)
		return unifyVar((Variable)x, y, theta);
	    }
	    else if (y is Variable)
	    {
		// else if VARIABLE?(y) then return UNIFY-VAR(y, x, theta)
		return unifyVar((Variable)y, x, theta);
	    }
	    else if (isCompound(x) && isCompound(y))
	    {
		// else if COMPOUND?(x) and COMPOUND?(y) then
		// return UNIFY(x.ARGS, y.ARGS, UNIFY(x.OP, y.OP, theta))
		return unify(args(x), args(y), unifyOps(op(x), op(y), theta));
	    }
	    else
	    {
		// else return failure
		return null;
	    }
	}

	/**
	 * Returns a Dictionary<Variable, Term> representing the substitution (i.e. a set
	 * of variable/term pairs) or null which is used to indicate a failure to
	 * unify.
	 * 
	 * @param x
	 *            a variable, constant, list, or compound
	 * @param y
	 *            a variable, constant, list, or compound
	 * @param theta
	 *            the substitution built up so far
	 * 
	 * @return a Dictionary<Variable, Term> representing the substitution (i.e. a set
	 *         of variable/term pairs) or null which is used to indicate a
	 *         failure to unify.
	 */
	// else if LIST?(x) and LIST?(y) then
	// return UNIFY(x.REST, y.REST, UNIFY(x.FIRST, y.FIRST, theta))
	public Dictionary<Variable, Term> unify(List<FOLNode> x,
	       List<FOLNode> y, Dictionary<Variable, Term> theta)
	{
	    if (theta == null)
	    {
		return null;
	    }
	    else if (x.Count != y.Count)
	    {
		return null;
	    }
	    else if (x.Count == 0 && y.Count == 0)
	    {
		return theta;
	    }
	    else if (x.Count == 1 && y.Count == 1)
	    {
		return unify(x[0], y[0], theta);
	    }
	    else
	    {
		return unify(x.Skip(1).ToList<FOLNode>(), y.Skip(1).ToList<FOLNode>(), unify(
			x[0], y[0], theta));
	    }
	}

	// PROTECTED METHODS

	// Note: You can subclass and override this method in order
	// to re-implement the OCCUR-CHECK?() to always
	// return false if you want that to be the default
	// behavior, as is the case with Prolog.
	protected bool occurCheck(Dictionary<Variable, Term> theta, Variable var,
	       FOLNode x)
	{
	    if (x is Function)
	    {
		List<Variable> varsToCheck = _variableCollector
			.collectAllVariables((Function)x);
		if (varsToCheck.Contains(var))
		{
		    return true;
		}

		// Now need to check if cascading will cause occurs to happen
		// e.g.
		// Loves(SF1(v2),v2)
		// Loves(v3,SF0(v3))
		// or
		// P(v1,SF0(v1),SF0(v1))
		// P(v2,SF0(v2),v2 )
		// or
		// P(v1, F(v2),F(v2),F(v2),v1, F(F(v1)),F(F(F(v1))),v2)
		// P(F(v3),v4, v5, v6, F(F(v5)),v4, F(v3), F(F(v5)))
		return cascadeOccurCheck(theta, var, varsToCheck,
			new List<Variable>(varsToCheck));
	    }
	    return false;
	}

	// PRIVATE METHODS

	/**
         * <code>
         * function UNIFY-VAR(var, x, theta) returns a substitution
         *   inputs: var, a variable
         *       x, any expression
         *       theta, the substitution built up so far
         * </code>
         */
	private Dictionary<Variable, Term> unifyVar(Variable var, FOLNode x,
		Dictionary<Variable, Term> theta)
	{

	    if (!(x is Term))
	    {
		return null;
	    }
	    else if (theta.ContainsKey(var))
	    {
		// if {var/val} E theta then return UNIFY(val, x, theta)
		return unify(theta[var], x, theta);
	    }
	    else if (theta.Keys.Contains(x))
	    {
		// else if {x/val} E theta then return UNIFY(var, val, theta)
		return unify(var, (FOLNode)theta[(Variable)x], theta);
	    }
	    else if (occurCheck(theta, var, x))
	    {
		// else if OCCUR-CHECK?(var, x) then return failure
		return null;
	    }
	    else
	    {
		// else return add {var/x} to theta
		cascadeSubstitution(theta, var, (Term)x);
		return theta;
	    }
	}

	private Dictionary<Variable, Term> unifyOps(String x, String y,
		Dictionary<Variable, Term> theta)
	{
	    if (theta == null)
	    {
		return null;
	    }
	    else if (x.Equals(y))
	    {
		return theta;
	    }
	    else
	    {
		return null;
	    }
	}

	private List<FOLNode> args(FOLNode x)
	{
	    return x.getArgs();
	}

	private String op(FOLNode x)
	{
	    return x.getSymbolicName();
	}

	private bool isCompound(FOLNode x)
	{
	    return x.isCompound();
	}

	private bool cascadeOccurCheck(Dictionary<Variable, Term> theta, Variable var,
	       List<Variable> varsToCheck, List<Variable> varsCheckedAlready)
	{
	    // Want to check if any of the variable to check end up
	    // looping back around on the new variable.
	    List<Variable> nextLevelToCheck = new List<Variable>();
	    foreach (Variable v in varsToCheck)
	    {
		Term t = null;
		if (theta.ContainsKey(v))
		{
		    t = theta[v];
		}
		if (null == t)
		{
		    // Variable may not be a key so skip
		    continue;
		}
		if (t.Equals(var))
		{
		    // e.g.
		    // v1=v2
		    // v2=SFO(v1)
		    return true;
		}
		else if (t is Function)
		{
		    // Need to ensure the function this variable
		    // is to be replaced by does not contain var.
		    List<Variable> indirectvars = _variableCollector
			    .collectAllVariables(t);
		    if (indirectvars.Contains(var))
		    {
			return true;
		    }
		    else
		    {
			// Determine the next cascade/level
			// of variables to check for looping
			foreach (Variable iv in indirectvars)
			{
			    if (!varsCheckedAlready.Contains(iv))
			    {
				nextLevelToCheck.Add(iv);
			    }
			}
		    }
		}
	    }
	    if (nextLevelToCheck.Count > 0)
	    {
		varsCheckedAlready.AddRange(nextLevelToCheck);
		return cascadeOccurCheck(theta, var, nextLevelToCheck,
			varsCheckedAlready);
	    }
	    return false;
	}

	// See:
	// http://logic.stanford.edu/classes/cs157/2008/miscellaneous/faq.html#jump165
	// for need for this.
	private void cascadeSubstitution(Dictionary<Variable, Term> theta, Variable var,
		Term x)
	{
	    theta.Add(var, x);
	    List<Variable> thetaKeys = theta.Keys.ToList<Variable>();
	    foreach (Variable v in thetaKeys)
	    {
		Term t = theta[v];
		if (theta.ContainsKey(v))
		{
		    theta[v] = _substVisitor.subst(theta, t);
		}
		else
		{
		    theta.Add(v, _substVisitor.subst(theta, t));
		}
	    }
	}
    }
}
