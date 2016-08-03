/*
 * Test: RemoteAddressTest
 * Class: RemoteAddress
 * Author: Pradeep Singh
 * Date: 2 July 2016
 * Change Log:
 *      2 July 2016: Created the test.
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BringDownClient.Logic;

namespace BringDownClient.Test
{
    [TestClass]
    public class RemoteAddressTest
    {
        private const string InvalidPortUrl = "";
        
        [TestMethod]
        public void ConstructionTestPositive()
        {
            RemoteAddress address = new RemoteAddress("http://www.contoso1234.com:8080/letters/readme.html");
            Assert.IsNotNull(address);
            Assert.IsTrue(address.IsValid);
            Assert.IsTrue(address.Port == 8080);
            Assert.IsTrue(address.HostName.Equals("www.contoso1234.com"));
            Assert.IsTrue(address.Protocol == "http");
        }

        [TestMethod]
        public void ConstructionTestPositiveHttps()
        {
            RemoteAddress address = new RemoteAddress("https://www.contoso1234.com:8080/letters/readme.html");
            Assert.IsNotNull(address);
            Assert.IsTrue(address.IsValid);
            Assert.IsTrue(address.Port == 8080);
            Assert.IsTrue(address.Protocol == "https");
        }

        [TestMethod]
        public void ConstructionTestPositiveWithoutPort()
        {
            RemoteAddress address = new RemoteAddress("https://www.contoso1234.com/letters/readme.html");
            Assert.IsNotNull(address);
            Assert.IsTrue(address.IsValid);
            Assert.IsTrue(address.Port == -1);
            Assert.IsTrue(address.Protocol == "https");
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void ConstructionTestNegative()
        {
            RemoteAddress address = new RemoteAddress("htt1://www.contoso1234.com:8080/letters/readme.html");
            
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void ConstructionTestPortNegative()
        {
            RemoteAddress address = new RemoteAddress("https://www.contoso1234.com:test/letters/readme.html");
            Assert.AreEqual(address.Port, -1);
        }
    }
}
