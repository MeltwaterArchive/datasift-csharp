using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datasift.Api;
using Datasift;
using Newtonsoft.Json.Linq;
namespace DatasiftTest.Mocks
{
    /// <summary>
    /// Use in place of DatasiftApiRequest. Not using mock since only one method needs changing/simulating
    /// </summary>
    public class DatasiftApiRequestMock : DatasiftApiRequest
    {

        public DatasiftApiRequestMock(Config config) : base(config) { }

        /// <summary>
        /// override to provide expected date
        /// </summary>
        /// <param name="method"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override string request(string method, string param)
        {
            switch (method)
            {
                case "compile": return "{\"hash\":\"hashcode\",\"created_at\":\"now\",\"cost\":\"1\"}";
                case "stream": return "{\"stream\":{\"status\":\"connected\"}}";
            }
            Console.WriteLine("overriden request called");
            return null;
        }
    }
}
