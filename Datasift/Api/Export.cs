using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Datasift.Api
{
    public class Export
    {
        private JObject json;
        public Export(string json)
        {
            this.json = JObject.Parse(json);
        }
        /// <summary>
        /// Returns the ID for this recording
        /// </summary>
        public string Id { get { return json["id"] == null ? null : json["id"].ToString(); } set { } }
        /// <summary>
        /// Returns the ID for this recording
        /// </summary>
        public string RecordingId { get { return json["recording_id"] == null ? null : json["recording_id"].ToString(); } set { } }
        /// <summary>
        /// When the recording was/should start(ed)
        /// </summary>
        public string StartTime { get { return json["start"] == null ? null : json["start"].ToString(); } set { } }
        /// <summary>
        /// When the recording should finish or finished
        /// </summary>
        public string FinishTime { get { return json["end"] == null ? null : json["end"].ToString(); } set { } }
        /// <summary>
        /// The name of this export
        /// </summary>
        public string Name { get { return json["name"] == null ? null : json["name"].ToString(); } set { } }
        /// <summary>
        /// The status of this export
        /// </summary>
        public string Status { get { return json["status"] == null ? null : json["status"].ToString(); } set { } }
    }
}
