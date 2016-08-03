/*
 * Test: RemoteClientTest
 * Class: RemoteClient
 * Author: Pradeep Singh
 * Date: 2 July 2016
 * Change Log:
 *      2 July 2016: Created the test.
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BringDownClient.Logic;
using System.IO;
using System.Text;

namespace BringDownClient.Test
{
    [TestClass]
    public class RemoteClientTest
    {
        [TestMethod]
        public void ConstructionTestPositive()
        {
            MockRemoteClient client = new MockRemoteClient("http://www.contoso1234.com:8080/letters/readme.html");
            Assert.IsNotNull(client);
            MemoryStream stream = new MemoryStream();
            byte[] buffer = System.Text.ASCIIEncoding.ASCII.GetBytes("This is a mock response");
            stream.Write(buffer, 0, buffer.Length);
            client.SetupTestData(stream);
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void ConstructionTestInvalidAddressNegative()
        {
            MockRemoteClient client = new MockRemoteClient("htt1://www.contoso1234.com:8080/letters/readme.html");
            Assert.IsNotNull(client);
            MemoryStream stream = new MemoryStream();
            byte[] buffer = System.Text.ASCIIEncoding.ASCII.GetBytes("This is a mock response");
            stream.Write(buffer, 0, buffer.Length);
            client.SetupTestData(stream);
        }

        [TestMethod]
        public void ReadTestPositive()
        {
            MockRemoteClient client = new MockRemoteClient("http://www.contoso1234.com:8080/letters/readme.html");
            Assert.IsNotNull(client);
            MemoryStream stream = new MemoryStream();
            string mockString="This is a mock response";
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes(mockString);
            stream.Write(buffer, 0, buffer.Length);
            stream.Seek(0, SeekOrigin.Begin);
            client.SetupTestData(stream);
            MemoryStream readData = new MemoryStream();
            int countRead;
            byte[] readBytes = new byte[4];
            while ((countRead = client.Read(readBytes, 0, 4)) > 0)
            {
                readData.Write(readBytes, 0, countRead);
            }
            readData.Seek(0,SeekOrigin.Begin);
            StreamReader reader = new StreamReader(readData, ASCIIEncoding.ASCII);
            Assert.AreEqual(mockString, reader.ReadToEnd());
        }
    }

    public class MockRemoteClient : RemoteClient
    {
        public MockRemoteClient(string address)
            : this(new RemoteAddress(address))
        {

        }

        public MockRemoteClient(RemoteAddress address)
            : base(address)
        {

        }

        public void SetupTestData(Stream dataStream)
        {
            _dataStream = dataStream;
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
            protected set
            {
                base.CanRead = _dataStream.CanRead;
            }
        }
    }
}
