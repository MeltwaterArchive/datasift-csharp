using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datasift.Api
{
    public class DPUItem
    {
        private Dictionary<string, DPUItem> targets;
        private int count;
        private double dpu;
        /// <summary>
        /// Creates a new DPU Item capable of holding any targets related to this DPU Item's total
        /// </summary>
        /// <param name="count"></param>
        /// <param name="dpu"></param>
        public DPUItem(int count, double dpu)
        {
            this.dpu = dpu;
            this.count = count;
            targets = new Dictionary<string, DPUItem>();
        }
        /// <summary>
        /// Gets the DPU for this Item
        /// </summary>
        public double Dpu
        {
            set { }
            get { return dpu; }
        }
        /// <summary>
        ///     Get the count
        /// </summary>
        public int Count
        {
            set { }
            get { return count; }
        }
        /// <summary>
        /// Returns true if this DPUItem has any targets,false otherwise
        /// </summary>
        public bool HasTargets
        {
            set { }
            get { return targets.Count > 0 ? true : false; }
        }
        /// <summary>
        /// Adds a new target to this DPU  item
        /// </summary>
        /// <param name="key">The key for this DPU Item</param>
        /// <param name="value">the value it maps to</param>
        public void addTarget(string key, DPUItem value)
        {
            targets.Add(key, value);
        }
        /// <summary>
        /// Gets all the targets within this DPU  item
        /// </summary>
        /// <returns>A list of KV pairs having target => DPUItem</returns>
        public Dictionary<string, DPUItem> Targets
        {
            get { return targets; }
            set { }
        }
    }
}
