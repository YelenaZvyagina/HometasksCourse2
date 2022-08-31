using MyFtp;

if (args.Length != 2)
{
    Console.WriteLine("Arguments expected: ip, port");
}
        
if (!IPAddress.TryParse(args[2], out var ip))
{
    Console.WriteLine($"Correct ip address expected, {args[0]} got");
}
        
if (!int.TryParse(args[3], out var port))
{
    Console.WriteLine($"Correct port number expected, {args[1]} got");
}
        
var server = new Server(ip, port);
Console.WriteLine("Press any key to stop server");
Console.ReadLine();
server.StopServer();
await server.RunServer();