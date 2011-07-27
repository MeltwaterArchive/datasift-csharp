using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datasift.Exceptions
{
    class DataSiftIncompleteInteraction : Exception
    {
        public DataSiftIncompleteInteraction() : base("An incomplete Interaction was encountered") { }
    }
}
