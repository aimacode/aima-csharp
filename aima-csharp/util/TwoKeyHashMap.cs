using System.Collections.Generic;

namespace aima.core.util
{
    /**
     * Provides a hash map which is indexed by two keys. In fact this is just a hash
     * map which is indexed by a pair containing the two keys. The provided two-key
     * access methods try to increase code readability.
     * 
     * @param <K1>
     *            First key
     * @param <K2>
     *            Second key
     * @param <V>
     *            Result value
     * 
     * @author Ruediger Lunde
     * @author Mike Stampone
     */
    public class TwoKeyHashMap<K1, K2, V> : Dictionary<Pair<K1, K2>, V>
    {
	private static readonly long serialVersionUID = -2232849394229644162L;

	/**
	 * Associates the specified value with the specified key pair in this map.
	 * If the map previously contained a mapping for this key pair, the old
	 * value is replaced.
	 * 
	 * @param key1
	 *            the first key of the key pair, with which the specified value
	 *            is to be associated.
	 * @param key2
	 *            the second key of the key pair, with which the specified value
	 *            is to be associated.
	 * @param value
	 *            the value to be associated with the key pair.
	 * 
	 */
	public void put(K1 key1, K2 key2, V value)
	{
	    base.Add(new Pair<K1, K2>(key1, key2), value);
	}

	/**
	 * Returns the value to which the specified key pair is mapped in this two
	 * key hash map, or <code>null</code> if the map contains no mapping for
	 * this key pair. A return value of null does not <em>necessarily</em>
	 * indicate that the map contains no mapping for the key; it is also
	 * possible that the map explicitly maps the key to <code>null</code>. The
	 * <code>containsKey</code> method may be used to distinguish these two
	 * cases.
	 * 
	 * @param key1
	 *            the first key of the key pair, whose associated value is to be
	 *            returned.
	 * @param key2
	 *            the second key of the key pair, whose associated value is to
	 *            be returned.
	 * 
	 * @return the value to which this map maps the specified key pair, or
	 *         <code>null</code> if the map contains no mapping for this key
	 *         pair.
	 */
	public V get(K1 key1, K2 key2)
	{
	    V value;
	    if(base.TryGetValue(new Pair<K1, K2>(key1, key2), out value))
	    {
		return value;
	    }

	    return value;
	}

	/**
	 * Returns <code>true</code> if this map contains a mapping for the
	 * specified key pair.
	 * 
	 * @param key1
	 *            the first key of the key pair whose presence in this map is to
	 *            be tested.
	 * @param key2
	 *            the second key of the key pair whose presence in this map is
	 *            to be tested.
	 * 
	 * @return <code>true</code> if this map contains a mapping for the
	 *         specified key.
	 */
	public bool containsKey(K1 key1, K2 key2)
	{
	    return base.ContainsKey(new Pair<K1, K2>(key1, key2));
	}

	/**
	 * Removes the mapping for this key pair from this map if present.
	 * 
	 * @param key1
	 *            the first key of the key pair, whose mapping is to be removed
	 *            from the map.
	 * @param key2
	 *            the second key of the key pair, whose mapping is to be removed
	 *            from the map.
	 * 
	 * @return the previous value associated with the specified key pair, or
	 *         <code>null</code> if there was no mapping for the key pair. A
	 *         <code>null</code> return can also indicate that the map
	 *         previously associated <code>null</code> with the specified key
	 *         pair.
	 */
	public V removeKey(K1 key1, K2 key2, V value)
	{
	    base.Remove(new Pair<K1, K2>(key1, key2));
	    return value;
	}

	// public static void main(String[] args) {
	// TwoKeyHashMap<String, String, String> hash = new TwoKeyHashMap<String,
	// String, String>();
	// hash.put("A", "A", "C");
	// hash.put("A", "A", "D");
	// hash.put("B", "A", "E");
	// System.Console.WriteLine(hash.get("A", "A"));
	// System.Console.WriteLine(hash.get("A", "B"));
	// }	
    }
}