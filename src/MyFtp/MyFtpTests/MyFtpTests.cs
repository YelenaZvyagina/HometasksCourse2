using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MyFtp;
using NUnit.Framework;

namespace MyFtpTests;

[TestFixture]
public class Tests
{
    private Server _server;
    private Client _client;

    [SetUp]
    public void Setup()
    {
        _server = new Server(IPAddress.Parse("127.0.0.1"), 1337);
        _client = new Client(IPAddress.Parse("127.0.0.1"), 1337);
        _server.RunServer();
    }

    [TearDown]
    public void TearDown()
    {
        _server.StopServer();
    }

    [Test]
    public async Task ListTest()
    {
        var expected = new List<(string name, bool isDir)>{ ("Directory1", true), ("test2.txt", false), ("test3.txt", false), ("test1.txt", false) };
        var actual  = await _client.List("/home/yelena/study/Hometasks/HometasksCourse2/src/MyFtp/MyFtpTests/TestDirectory", CancellationToken.None);
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    public void DirectoryDoesntExistTest()
    {
        Assert.ThrowsAsync<DirectoryNotFoundException>(async Task () =>
        { 
            await _client.List("notADirectoryAtAll", CancellationToken.None);
        });
    }

    [Test]
    public void FileDoesntExistTest()
    {
        Assert.ThrowsAsync<FileNotFoundException>(async Task() =>
        {
            await _client.Get("notFileAtAll", CancellationToken.None);
        });
    }
}