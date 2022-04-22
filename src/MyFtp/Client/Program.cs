namespace MyFtp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Arguments expected: request type (1 for list, 2 for get), path, ip, port");
        }
        
        if (!int.TryParse(args[0], out var requestType))
        {
            Console.WriteLine($"Incorrect arguments. Request type 1 for List or 2 for Get expected, {args[0]} got");
        }
        
        if (!IPAddress.TryParse(args[2], out var ip))
        {
            Console.WriteLine($"Correct ip address expected, {args[2]} got");
        }
        
        if (!int.TryParse(args[3], out var port))
        {
            Console.WriteLine($"Correct port number expected, {args[3]} got");
        }
        
        var path = args[1];
        var cts = new CancellationTokenSource();
        var client = new Client(ip, port);

        
        switch (requestType)
        {
            case 1:
                try
                {
                    var response = await client.List(path, cts.Token);
                    foreach (var (name, isDir) in response)
                    {
                        Console.WriteLine($"{name}, {isDir}");
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("There is no directory by the path you've specified");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Operation was canceled");
                }
                return;
            
            case 2:
                try
                {
                    var (size, savedPath) = await client.Get(path, cts.Token);
                    Console.WriteLine($"File was downloaded to {savedPath}. File size {size}");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("There is no file by the path you've specified");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Operation was canceled");
                }
                return;
            
            default:
                Console.WriteLine("Request Type can be only 1 for list or 2 for get");
                return;
        }
    }
}