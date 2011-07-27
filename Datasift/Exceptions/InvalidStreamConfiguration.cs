using System;
using System.Collections.Generic;
using System.Text;

namespace Datasift.Exceptions
{
    public class InvalidStreamConfiguration : Exception
    {
        public InvalidStreamConfiguration()
            : base("The API key,username and DatasiftStream hash are required!")
        {

        }
    }
}
