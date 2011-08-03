using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Datasift.DatasiftStream;
using Newtonsoft.Json.Linq;

namespace Datasift.DatasiftStream
{
    public class Interaction
    {
        private JObject json;
        private string statusMsg = null;
        private string statusLevel = null;
        private bool isError = false;
        private bool isWarning = false;
        private bool isTick = false;
        /// <summary>
        /// An API interaction object
        /// </summary>
        /// <param name="jsonStr"> The json string object from the API stream</param>
        public Interaction(string jsonStr)
        {
            try
            {
                json = JObject.Parse(jsonStr);
                JToken status = json["status"];
                if (status != null)
                {
                    statusLevel = status.ToString();
                    statusMsg = json["message"].ToString();
                    switch (status.ToString())
                    {
                        case "connected": isTick = true; break;
                        case "error": isError = true; break;
                        case "warning": isWarning = true; break;
                        default: break;
                    }
                }
            }
            catch (Exception e)
            {
                //if an exception ocured then make this an error
                isError = true;
                statusMsg = "Unable to parse json, maybe an incomplete interaction - "+e.Message;
            }
        }
        /// <summary>
        /// The reason for the status this interaction represents
        /// </summary>
        /// <returns></returns>
        public string StatusMessage() { return statusMsg; }
        public bool IsError() { return isError; }
        public bool IsWarning() { return isWarning; }
        public bool IsTick() { return isTick; }
        /// <summary>
        /// Check to see if this interaction is one of the set of status the API occasionally sends out
        /// </summary>
        /// <returns>true if it is a status message false otherwise</returns>
        public bool HasStatus()
        {
            if (IsError() || IsWarning() || IsTick())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Returns a part of an interaction based on the argument passed in
        /// the format of <paramref name="data"/> should be interaction.source or twitter.xxx etc
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A string for the matched data or null if it is not present</returns>
        public string Get(string data)
        {
            if (HasStatus() == false)//if we have a status then return null
            {
                string[] parts = data.Split(new string[] { "." }, StringSplitOptions.None);
                JToken ret = this.json;
                foreach (string part in parts)
                {
                    if (ret != null)
                    {
                        ret = ret[part];
                    }
                }
                return (ret == null) ? null : ret.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Convert this Interaction to a string
        /// </summary>
        /// <returns>The string representation of this Interaction object</returns>
        public override string ToString()
        {
            return json!= null ? json.ToString() : null;
        }
    }
}
