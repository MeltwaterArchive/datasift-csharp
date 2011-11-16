using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Datasift;
using Datasift.Exceptions;
using System.Web;

namespace Datasift.Api
{
    public class DatasiftApiRequest
    {
        Config config;
        /// <summary>
        /// The CSDL string to use;
        /// </summary>
        private string csdl;
        /// <summary>
        /// A representation of a CSDL statement
        /// </summary>
        /// <param name="csdl"> The CSDL string you wish to opporate on.</param>
        /// <param name="config">A config instance of configType, API</param>
        public DatasiftApiRequest(Config config)
        {
            if (config == null)
            {
                throw new InvalidStreamConfiguration();
            }
            this.config = config;
        }
        /// <summary>
        /// Get or Set a CSDL string for this request
        /// </summary>
        public string CSDL
        {
            get { return csdl; }
            set { csdl = value; }
        }
        /// <summary>
        /// Make a request to the Datasift API to compile the csdl available
        /// </summary>
        /// <returns>An ApiResponse object which is a convience wrapper for API responses. OR null if no CSDL is specified</returns>
        public DatasiftApiResponse Compile()
        {
            if (csdl == null)
            {
                return null;
            }
            // have a response object constructed and returned
            return new DatasiftApiResponse(request("compile", "&csdl=" + csdl));
        }
        public DatasiftApiResponse Compile(string csdl)
        {
            this.csdl = csdl;
            return Compile();
        }

        /// <summary>
        /// Make a stream request to the Datasift API.
        /// </summary>
        /// <param name="hash">The hash for the stream you are making the request from</param>
        /// <param name="interaction_id">The last identifier of the interaction you want to retrieve from</param>
        /// <param name="count"> The max number of interactions you want to retrieve. Or -1 to not set a limit </param>
        /// <returns>An API response which has the Stream property populated</returns>
        public DatasiftApiResponse Stream(string hash, string interaction_id, int count)
        {
            if (hash == null)
            {
                throw new InvalidStreamHashException();
            }
            StringBuilder query = new StringBuilder();
            query.Append("&hash=" + hash);
            if (interaction_id != null)
            {
                query.Append("&interaction_id=" + interaction_id);
            }
            if (count != -1)
            {
                query.Append("&count=" + count);
            }

            // have a response object constructed and returned
            return new DatasiftApiResponse(request("stream", query.ToString()));
        }

        public DatasiftApiResponse Stream(string hash)
        {
            return Stream(hash, null, -1);
        }
        /// <summary>
        /// Make a request to the API to get the cost of a particular stream
        /// </summary>
        /// <param name="hash">The hash for the stream you want the costs for</param>
        /// <returns>An ApiResponse object which has its Costs and and Total properties populated</returns>
        public DatasiftApiResponse Dpu(string hash)
        {
            // have a response object constructed and returned
            return new DatasiftApiResponse(request("dpu", "&hash=" + hash));
        }

        /// <summary>
        /// modifier protected only for the sake of overriding to test without nmock
        /// Makes an HTTP GET request
        /// </summary>
        /// <param name="method">The API method to make the request to e.g. api.dataisft.net/compile</param>
        /// <param name="param">A set of GET request parameters in the standard URL form, preceeded by an & e.g.
        /// &csdl=interaction.csdl "apple"
        /// </param>
        /// <returns>Returns the responses string from the HTTP request.</returns>
        protected virtual string request(string method, string param)
        {
            try
            {
                //Console.WriteLine(config.getApiUrl(method) + param);
                //create our request
                WebRequest req = WebRequest.Create(config.getApiUrl(method) + param);
                //get response stream
                Stream stream = req.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                //read the entire stream
                return reader.ReadToEnd();
            }
            catch (Exception e) {
                return "{error:\""+e.Message+"\"}";
            }
        }

        /// <summary>
        /// Gets usage information and allowances
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public DatasiftApiResponse Usage(Dictionary<string, string> param)
        {
            if (param == null)
            {
                return new DatasiftApiResponse(request("usage", ""));
            }
            else
            {
                StringBuilder strParams = new StringBuilder();
                foreach (KeyValuePair<string, string> kvp in param)
                {
                    if (kvp.Value != null)
                    {
                        strParams.Append("&").Append(kvp.Key).Append("=").Append(Uri.EscapeUriString(kvp.Value));
                    }
                }
                return new DatasiftApiResponse(request("usage", strParams.ToString()));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Your usage info</returns>
        public DatasiftApiResponse Usage()
        {
            return Usage(null);
        }
    }
}
