using System;
using System.Threading.Tasks;

namespace Test1
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            switch (args.Length)
            {
                case > 2:
                    throw new ArgumentException("Expected 1 or 2 arguments, ip is required for client port is required");
                case 2:
                {
                    var ip = args[0];
                    int.TryParse(args[1], out int port);
                    Client client = new Client(ip, port);
                    await client.RunClient();
                    break;
                }
                case 1:
                {
                    int.TryParse(args[0], out int port);
                    Server server = new Server(port);
                    await server.RunServer();
                    break;
                }
            }
        }
    }
}