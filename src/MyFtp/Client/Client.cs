using static System.Boolean;

namespace MyFtp;

using System.Net;
using System.Net.Sockets;

public class Client
{
    private readonly IPAddress _ip;
    private readonly int _port;
    private readonly TcpClient _client;
        
    public Client(IPAddress ip, int port)
    {
        _ip = ip;
        _port = port;
        _client = new TcpClient();
    }

    public async Task<List<(string name, bool isDir)>> List(string path, CancellationToken token)
    {
        await _client.ConnectAsync(_ip.ToString(), _port, token);
        await using var stream = _client.GetStream();
        await using var writer = new StreamWriter(stream) {AutoFlush = true};
        using var reader = new StreamReader(stream);
        await writer.WriteLineAsync($"1 {path} \n");
        var data = await reader.ReadLineAsync();
        var splitted = data.Split(' ');
        if (!int.TryParse(splitted[0], out var size))
        {
            throw new ArgumentException("Server's response was incorrect, amount of directory's content expected");
        }
        if (data == "-1")
        {
            throw new DirectoryNotFoundException();
        }
        var resultList = new List<(string name, bool isDir)>();
        for (var i = 1; i < size * 2; i += 2)
        {
            if (!token.IsCancellationRequested)
            {
                var directoryName = splitted[i];
                if (!TryParse(splitted[i + 1], out var isDir))
                {
                    throw new ArgumentException
                        ("Wrong format of received data. Boolean value is it a directory expected ");
                }
                resultList.Add((directoryName, isDir));
            };
        }
        return resultList;
    }

    public async Task<long> Get(string pathToFile, CancellationToken token)
    {
        await _client.ConnectAsync(_ip.ToString(), _port, token);
        await using var stream = _client.GetStream();
        await using var writer = new StreamWriter(stream) {AutoFlush = true};
        using var reader = new StreamReader(stream);
        await writer.WriteLineAsync($"2 {pathToFile} \n");
        var data = await reader.ReadLineAsync();
        var splitted = data.Split(' ');
        if (data == "-1")
        {
            throw new FileNotFoundException();
        }
        if (!long.TryParse(splitted[0], out var size))
        {
            throw new ArgumentException("Server's response was incorrect, size of file expected");
        }
        await using var fileStream = File.Create(pathToFile);
        await stream.CopyToAsync(stream, token);
        return size;
    }
}