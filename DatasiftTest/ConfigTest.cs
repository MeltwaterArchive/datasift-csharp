using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Datasift;
using Datasift.Exceptions;

namespace DatasiftTest
{
    [TestClass]
    public class ConfigTest
    {
        [ExpectedException(typeof(InvalidStreamConfiguration))]
        [TestMethod]
        public void TestConstructor1()
        {
            Config c;

        }
    }
}
