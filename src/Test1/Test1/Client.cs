using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Test1
{
    public class Client
    {
        private readonly TcpClient client;

        public Client(string ip, int port)
        {
            client = new TcpClient(ip, port);
        }

        public async Task RunClient()
        {
            var stream = client.GetStream(); 
            Console.WriteLine("Client is here");

            await ReadMsg(stream);
            await WriteMsg(stream);
        }

        private async Task ReadMsg(Stream stream)
        {
            var reader = new StreamReader(stream);
            var data = await reader.ReadLineAsync();
            Console.WriteLine($"Received data {data}");
        }

        private async Task WriteMsg(Stream stream)
        {
            var writer = new StreamWriter(stream);
            await writer.FlushAsync();
            while (true)
            {
                var dataToSend = Console.ReadLine();
                await writer.WriteAsync(dataToSend + "\n");
                if (dataToSend == "exit")
                {
                    client.Close();
                    Environment.Exit(0);
                }
            }
        }
        
    }
}