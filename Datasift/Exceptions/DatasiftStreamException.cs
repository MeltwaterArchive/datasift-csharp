using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datasift.Exceptions
{
    class DatasiftStreamException : Exception
    {
        public DatasiftStreamException()
            : base("An unkown error has occured while reading this DatasiftStream")
        {
        }
        public DatasiftStreamException(string msg) : base(msg) { }
    }
}
