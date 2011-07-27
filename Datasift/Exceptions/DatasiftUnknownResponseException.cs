using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datasift.Exceptions
{
    class DatasiftUnknownResponseException : Exception
    {
        public DatasiftUnknownResponseException(string code)
            : base("An unexpected status code was returned by the DatasiftStream API, The status code : " + code)
        {
        }
    }
}
