using System.Net;
using NUnit.Framework;
using Test1;

namespace Tests
{
    public class Tests
    {
        private Server server;
        private Client client;
        private readonly string ip = "127.0.0.1";
        private const int port = 8888;
        
        [SetUp]
        public void Setup()
        {
            server = new Server(port);
            client = new Client(ip, port);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}