using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datasift.Exceptions
{
    class InvalidStreamHashException : Exception
    {
        public InvalidStreamHashException() : base("The hash component is required to make a stream request.") { }
    }
}
