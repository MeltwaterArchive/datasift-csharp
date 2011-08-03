using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Brandy
{
    public class DataTable<K, V>
    {
        private Dictionary<K, V> rows;
        private Queue<K> q;
        public DataTable()
        {
            rows = new Dictionary<K, V>();
            q = new Queue<K>();
        }
        /// <summary>
        /// Add a new row to this table
        /// </summary>
        /// <param name="key">The X value for this point</param>
        /// <param name="value">The Y value for this point</param>
        public void AddRow(K key, V value)
        {
            //anymore than the max timePeriod should be removed
            while (rows.Count >= Visualization.minutes)
            {
                rows.Remove(q.Dequeue());
            }
            rows.Add(key, value);
            q.Enqueue(key);
        }
        public Dictionary<K, V> Next()
        {
            return rows;
        }

        public int Size { get { return rows.Count; } set { } }
    }
}
