namespace MyFtp;

/// <summary>
/// Class for processing Server
/// </summary>
public class Server
{
    private readonly TcpListener _server;
    private readonly List<Task> _requests = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public Server(IPAddress ip, int port)
    {
        _server = new TcpListener(ip, port);
    }
        
    public async Task RunServer()
    {
        _server.Start();
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            var tcpClient = await _server.AcceptTcpClientAsync(_cancellationTokenSource.Token);
            var request = Task.Run(() => ClientProcess(tcpClient));
            _requests.Add(request);
        }
        Task.WaitAll(_requests.ToArray());
        _server.Stop();
    }
        
    public void StopServer()
    {
        _cancellationTokenSource.Cancel();
    }

    /// <summary>
    /// Method for processing requests from clients
    /// </summary>
    private static async Task ClientProcess(TcpClient client)
    {
        using (client)
        {
            await using var stream = client.GetStream();
            await using var writer = new StreamWriter(stream) {AutoFlush = true};
            using var reader = new StreamReader(stream);
            var args = (await reader.ReadLineAsync())!.Split(' ');

            switch (args[0])
            {
                case "1":
                    await List(args[1], writer);
                    return;
                case "2":
                    await Get(args[1], writer);
                    return;
                default:
                    return;
            }   
        }
    }

    /// <summary>
    /// Returns directories and files list on the server to client
    /// </summary>
    private static async Task List(string path, StreamWriter writer)
    {
        var directory = new DirectoryInfo(path);
        if (!directory.Exists)
        {
            await writer.WriteLineAsync("-1");
        }
        var dirInfo = directory.GetFileSystemInfos();
        await writer.WriteAsync($"{dirInfo.Length}");
        foreach (var dirElement in dirInfo)
        {
            var isDir = dirElement is DirectoryInfo;
            await writer.WriteAsync($" {dirElement.Name} {isDir}");
        }
    }

    /// <summary>
    /// Downloads the file from server
    /// </summary>
    private static async Task Get(string pathToFile, StreamWriter writer)
    {
        var file = new FileInfo(pathToFile);
        if (!file.Exists)
        {
            await writer.WriteLineAsync("-1");
        }
        await writer.WriteLineAsync(file.Length.ToString());
        await using var fileStream = file.OpenRead();
        await fileStream.CopyToAsync(writer.BaseStream);
    }
}