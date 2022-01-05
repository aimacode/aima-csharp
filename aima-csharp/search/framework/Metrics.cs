namespace aima.core.search.framework
{
    /**
     * Stores key-value pairs for efficiency analysis.
     * 
     * @author Ravi Mohan
     * @author Ruediger Lunde
     */
    public class Metrics
    {
        private Dictionary<String, String> hash;

        public Metrics()
        {
            this.hash = new Dictionary<String, String>();
        }

        public void set(String name, int i)
        {
            hash[name] = i.ToString();
        }

        public void set(String name, double d)
        {
            hash[name] = d.ToString();
        }

        public void incrementInt(String name)
        {
            set(name, getInt(name) + 1);
        }

        public void set(String name, long l)
        {
            hash[name] = l.ToString();
        }

        public int getInt(String name)
        {
            return int.Parse(hash[name]);
        }

        public double getDouble(String name)
        {
            return double.Parse(hash[name]);
        }

        public long getLong(String name)
        {
            return long.Parse(hash[name]);
        }

        public String get(String name)
        {
            return hash[name];
        }

        public HashSet<String> keySet()
        {
            return new HashSet<string>(hash.Keys);
        }

        public String toString()
        {
            SortedDictionary<String, String> map = new SortedDictionary<String, String>(hash);
            return map.ToString();
        }
    }
}