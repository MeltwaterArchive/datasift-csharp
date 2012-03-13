using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Datasift;
using Datasift.Exceptions;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Datasift.Api
{
    public class DatasiftApiRequest
    {
        Config config;
        /// <summary>
        /// The CSDL string to use;
        /// </summary>
        private string csdl;
        private bool postRequest = false;
        private Dictionary<string, string> postFields;
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
            postFields = new Dictionary<string, string>();
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
            IsPostRequest = true;
            postFields.Add("csdl", csdl);
            // have a response object constructed and returned
            return new DatasiftApiResponse(request("compile", ""));
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
                return new DatasiftApiResponse("{error:\"You must provide a Hash in order to make a request to the Stream API\"}");
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
        /// <param name="postRequest">The API postRequest to make the request to e.g. api.dataisft.net/compile</param>
        /// <param name="param">A set of GET request parameters in the standard URL form, preceeded by an & e.g.
        /// &csdl=interaction.csdl "apple"
        /// </param>
        /// <returns>Returns the responses string from the HTTP request.</returns>
        protected virtual string request(string method, string param)
        {
            try
            {
                //Console.WriteLine(config.getApiUrl(postRequest) + param);
                //create our request
                WebRequest req = WebRequest.Create(config.getApiUrl(method) + param);
                req.Headers["Authorization"] = config.Authorization;
                req.Headers["User-Agent"] = config.UserAgent;
                if (IsPostRequest)
                {
                    string postData = GetPostData();
                    req.Method = "POST";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.ContentLength = byteArray.Length;
                    Stream dataStream = req.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    //reset in case the object is re-used and the next request should be a GET
                    IsPostRequest = false;
                }
                //get response stream
                Stream stream = req.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                //read the entire stream
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return "{error:\"" + ex.Message + "\"}";
            }
        }
        /// <summary>
        /// URL encode all the fields in postFields 
        /// </summary>
        /// <returns>The URL endoded version of postFields</returns>
        private string GetPostData()
        {
            StringBuilder postString = new StringBuilder();
            bool first = true;
            foreach (KeyValuePair<string, string> field in postFields)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    postString.Append("&");
                }
                postString.AppendFormat("{0}={1}", field.Key, HttpUtility.UrlEncode(field.Value));
            }
            return postString.ToString();
        }
        /// <summary>
        /// True if this request will be performed using HTTP POST
        /// </summary>
        public bool IsPostRequest { get { return postRequest; } set { postRequest = value; } }
    }
}
