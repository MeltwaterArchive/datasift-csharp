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
            Config c = new Config(null, null, null, null);
        }
        [TestMethod]
        public void TestConstructor2() {
            Config c = new Config(Config.ConfigType.API, "username", "apikey");
            string url=c.getApiUrl("test");
            Assert.AreEqual("http://api.datasift.com/",url.Substring(0,24));

            c = new Config(Config.ConfigType.STREAM, "username", "apikey");
             url=c.getStreamUrl();
            Assert.AreEqual("http://stream.datasift.com/",url.Substring(0,27));
        }

        [TestMethod]
        public void TestBufferSize()
        {
            //no host or hash should be fine
            Config c = new Config(null, "username", "key", null);
            Assert.AreEqual(32768, c.BufferSize);//default 32kb buffer size
            c.BufferSize = 12;
            Assert.AreEqual(12, c.BufferSize);
        }
    }
}
