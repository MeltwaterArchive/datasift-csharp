using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Datasift.Api
{
    /// <summary>
    /// Recordings related data
    /// </summary>
    public class Recording
    {
        private JObject json;
        public Recording(string json)
        {
            this.json = JObject.Parse(json);
        }
        /// <summary>
        /// Returns the ID for this recording
        /// </summary>
        public string Id { get { return json["id"] == null ? null : json["id"].ToString(); } set { } }
        /// <summary>
        /// When the recording was/should start(ed)
        /// </summary>
        public string StartTime { get { return json["start_time"] == null ? null : json["start_time"].ToString(); } set { } }
        /// <summary>
        /// When the recording should finish or finished
        /// </summary>
        public string FinishTime { get { return json["finish_time"] == null ? null : json["finish_time"].ToString(); } set { } }
        /// <summary>
        /// The name of this recording
        /// </summary>
        public string Name { get { return json["name"] == null ? null : json["name"].ToString(); } set { } }
        /// <summary>
        /// The stream hash for this recording
        /// </summary>
        public string Hash { get { return json["hash"] == null ? null : json["hash"].ToString(); } set { } }
    }
}
