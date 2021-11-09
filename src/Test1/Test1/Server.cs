using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Test1
{
    public class Server
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly TcpListener _listener;
        private static TcpClient _client;

        public Server(int port)
        { 
            _listener = new TcpListener(IPAddress.Any, port);
        }
        
        public async Task RunServer()
        {
            _listener.Start();
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("Server is here");
                while (true)
                {
                    _client = await _listener.AcceptTcpClientAsync();
                    var stream = _client.GetStream();
                    await ReadMsg(stream);
                    await WriteMsg(stream);
                }
            }
            _listener.Stop();
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
                Console.WriteLine("Data to send is");
                var dataToSend = Console.ReadLine();
                await writer.WriteAsync(dataToSend + "\n");
                if (dataToSend == "exit")
                {
                    _cancellationTokenSource.Cancel();
                    _client.Close();
                    Environment.Exit(0);
                }
            }
            
        }
        
    }
}