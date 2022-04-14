using System.Net;

namespace MyFtp;

public class Program
{
    public static async Task Main()
    {
        var server = new Server(IPAddress.Parse("127.0.0.1"), 1337);
        await server.RunServer();
        Console.WriteLine("Server started");
        
    }
}