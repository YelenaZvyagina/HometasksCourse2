using static System.Boolean;

namespace MyFtp;

/// <summary>
/// class for processing client. 
/// </summary>
public class Client
{
    private readonly IPAddress _ip;
    private readonly int _port;
    private readonly TcpClient _client = new();
    
    public Client(IPAddress ip, int port)
    {
        _ip = ip;
        _port = port;
    }

    /// <summary>
    /// Returns list of files and directories on server by given path and specifies is it a directory
    /// </summary>
    public async Task<List<(string name, bool isDir)>> List(string pathToDirectory, CancellationToken token)
    {
        await _client.ConnectAsync(_ip.ToString(), _port, token);
        await using var stream = _client.GetStream();
        await using var writer = new StreamWriter(stream) {AutoFlush = true};
        using var reader = new StreamReader(stream);
        await writer.WriteLineAsync($"1 {pathToDirectory} \n");
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

        var result = new List<(string name, bool isDir)>();
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
                result.Add((directoryName, isDir));
            }
            else
            {
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// Downloads file from server
    /// </summary>
    public async Task<(long, string)> Get(string pathToFileOnServer, CancellationToken token)
    {
        await _client.ConnectAsync(_ip.ToString(), _port, token);
        await using var stream = _client.GetStream();
        await using var writer = new StreamWriter(stream) {AutoFlush = true};
        using var reader = new StreamReader(stream);
        await writer.WriteLineAsync($"2 {pathToFileOnServer} \n");
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

        var pathToSave = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\..\..\DownloadedFiles\{Path.GetFileName(pathToFileOnServer)}");
        await using var fileStream = File.Create(pathToSave);
        await stream.CopyToAsync(fileStream, token);
        return (size, pathToSave);
    }
}