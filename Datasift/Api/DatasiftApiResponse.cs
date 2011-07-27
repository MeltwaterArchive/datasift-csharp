using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Datasift.DatasiftStream;

namespace Datasift.Api
{
    public class DatasiftApiResponse
    {
        private JObject json;
        /// <summary>
        /// Creates an object that can be used to get info out of an API response.
        /// </summary>
        /// <param name="responseData"></param>
        public DatasiftApiResponse(string responseData)
        {
            json = JObject.Parse(responseData);
        }
        /// <summary>
        /// Returns an immutable string representing the hash returned in this response
        /// OR null if the response does not have this val
        /// </summary>
        public string Hash { get { return json["hash"] == null ? null : json["hash"].ToString(); } set { } }

        /// <summary>
        /// Returns an immutable string representing the created_at value returned in this response
        /// OR null if the response does not have this val
        /// </summary>
        public string CreatedAt { get { return json["created_at"] == null ? null : json["created_at"].ToString(); } set { } }

        /// <summary>
        /// Returns an immutable string representing the cost returned in this response, i.e. the cost of making the request
        /// OR null if the response does not have this val. Not to be confused with StreamCosts which returns the total costs
        /// for an entire stream, not just the request which created this response!
        /// </summary>
        public string RequestCost { get { return json["cost"] == null ? null : json["cost"].ToString(); } set { } }

        /// <summary>
        /// Returns an immutable string representing the costs value returned in this response
        /// OR null if the response does not have this val
        /// NOTE:This is only valid if the request method that created the response was cost, i.e. api.datasift.net/cost
        /// </summary>
        public string StreamCosts { get { return json["costs"] == null ? null : json["costs"].ToString(); } set { } }

        /// <summary>
        /// Returns an immutable string representing the total cost returned in this response
        /// OR null if the response does not have this val
        /// NOTE:This is only valid if the request method that created the response was cost, i.e. api.datasift.net/cost
        /// </summary>
        public string Total { get { return json["total"] == null ? null : json["total"].ToString(); } set { } }

        /// <summary>
        /// Returns an immutable list of Interactions returned in this response
        /// OR null if the response does not have this val
        /// </summary>
        public List<Interaction> Stream { get { return ApiResponseStream(); } set { } }

        /// <summary>
        /// Gets the set of interaction objects returned in this stream
        /// </summary>
        /// <returns>A list of Interaction objects, one for each returned in the stream</returns>
        private List<Interaction> ApiResponseStream()
        {
            List<Interaction> interactions = new List<Interaction>();
            JToken stream = json["stream"];
            if (json["stream"] == null)
            {
                return null;
            }
            else
            {
                foreach (JToken item in json["stream"])
                {
                    interactions.Add(new Interaction(item.ToString()));
                }
                return interactions;
            }
        }


    }
}
