using System.Net;

namespace MyFtp;

public class Program
{
    public static async Task Main()
    {
        var client = new Client(IPAddress.Parse("127.0.0.1"), 1337);
        var path = "/home/yelena/study/Hometasks/HometasksCourse2/src/MyFtp/MyFtpTests/TestDirectory";
        var response = await client.List(path, CancellationToken.None);
        Console.WriteLine("Client has started");
        foreach (var (name, isDir) in response)
        {
            Console.WriteLine( $"{name}, {isDir}");
        }
        Console.WriteLine("Client все");
    }
}