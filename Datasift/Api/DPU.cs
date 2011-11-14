using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace Datasift.Api
{

    public class DPU
    {
        private Newtonsoft.Json.Linq.JObject json;
        private Dictionary<string, DPUItem> dpus;
        public DPU(Newtonsoft.Json.Linq.JObject json)
        {
            this.json = json;
            dpus = new Dictionary<string, DPUItem>();
            //get a KV set of DPU break down
            Dictionary<string, DPUItem> values = JsonConvert.DeserializeObject<Dictionary<string, DPUItem>>(json["detail"].ToString());
            foreach (KeyValuePair<string, DPUItem> entry in values)
            {
                DPUItem val = entry.Value;
                //get all the targets within this DPU item
                Dictionary<string, DPUItem> targets = JsonConvert.DeserializeObject<Dictionary<string, DPUItem>>(json["detail"][entry.Key]["targets"].ToString());
                foreach (KeyValuePair<string, DPUItem> e in targets)
                {
                    val.addTarget(e.Key, e.Value);
                }
                dpus.Add(entry.Key, val);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>A Dictionary whose keys maps to a DPUItem which contains a further break down of DPUs</returns>
        public Dictionary<string, DPUItem> GetDPU
        {
            get { return dpus; }
            set { }
        }
        /// <summary>
        /// Get the total, i.e the total DPU for all the operators and targets used
        /// </summary>
        public double Total
        {
            get { return Convert.ToDouble(json["dpu"].ToString()); }
            set { }
        }
        public override string ToString()
        {
            return json.ToString();
        }
    }
}
