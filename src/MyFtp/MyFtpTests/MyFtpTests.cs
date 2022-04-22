using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    private const string TestDirectoryPath = "../../../TestDirectory";

    [SetUp]
    public void Setup()
    {
        _server = new Server(IPAddress.Parse("127.0.0.1"), 1337);
        _client = new Client(IPAddress.Parse("127.0.0.1"), 1337);
        _server.RunServer();
    }
    
    [OneTimeTearDown]
    public void TearDown()
    {
        _server.StopServer();
    }
    
    [Test]
    public async Task ListTest()
    {
        var expected = new List<(string name, bool isDir)>{("test3.txt", false), ("test2.txt", false), ("test1.txt", false), ("Directory1", true)};
        var actual  = await _client.List(TestDirectoryPath, new CancellationToken());
        Assert.IsFalse(expected.Except(actual).Any());
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

    [Test]
    public void CancelTest()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();
        Assert.ThrowsAsync<TaskCanceledException>(async Task() =>
        {
            await _client.Get(TestDirectoryPath, cts.Token);
        });
    }

    [Test]
    public async Task GetTest()
    {
        var expected = 77;
        var actual = await _client.Get(Path.Combine(TestDirectoryPath, "test1.txt"), CancellationToken.None);
        Assert.AreEqual(expected, actual.Item1);
    }
}