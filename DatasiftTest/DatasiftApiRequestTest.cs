using Datasift.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Datasift;
using Datasift.Exceptions;
using DatasiftTest.Mocks;
using Datasift.DatasiftStream;
namespace DatasiftTest
{


    /// <summary>
    ///This is a test class for DatasiftApiRequestTest and is intended
    ///to contain all DatasiftApiRequestTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DatasiftApiRequestTest
    {


        private Config config;
        private DatasiftApiRequestMock target;
        public DatasiftApiRequestTest()
        {
            config = new Config(Config.ConfigType.API, "courtney", "mykey");
            target = new DatasiftApiRequestMock(config);
        }

        [ExpectedException(typeof(InvalidStreamConfiguration))]
        [TestMethod()]
        public void DatasiftApiRequestConstructorTest()
        {
            DatasiftApiRequestMock target = new DatasiftApiRequestMock(null);
        }
        [TestMethod()]
        public void CompileTest()
        {
            DatasiftApiResponse res = target.Compile("interaction.content=\"apple\"");
            //while no requests are sent to the server the response object should have its hash populated with the string "hashcode"
            Assert.AreEqual("hashcode", res.Hash);
            Assert.AreEqual("now", res.CreatedAt);
            Assert.AreEqual("1", res.RequestCost);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidStreamHashException))]
        public void StreamTest1()
        {
            target.Stream(null);
        }

        [TestMethod()]
        public void StreamTest()
        {
            target = new DatasiftApiRequestMock(config);
            DatasiftApiResponse actual = target.Stream("somehash");
            foreach (Interaction i in actual.Stream)
            {
               //TODO test interaction content?
            }
        }
    }
}
