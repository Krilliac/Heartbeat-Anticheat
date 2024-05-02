using SharedLibrary;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
 
public class HeartbeatClient
{
    private TcpClient _client;
    private string _serverIP;
    private int _serverPort;
    private ILogger<HeartbeatClient> _logger;
 
    public HeartbeatClient(string ip, int port, ILogger<HeartbeatClient> logger)
    {
        _serverIP = ip;
        _serverPort = port;
        _client = new TcpClient();
        _logger = logger;
    }
 
    public async Task StartAsync()
    {
        try
        {
            await _client.ConnectAsync(_serverIP, _serverPort);
            _logger.LogInformation("Connected to server.");
            await ListenForServerMessagesAsync(_client.GetStream());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting client.");
        }
    }
 
    private async Task ListenForServerMessagesAsync(NetworkStream serverStream)
    {
        var message = new byte[4096];
 
        while (true)
        {
            try
            {
                var bytesRead = await serverStream.ReadAsync(message, 0, message.Length);
                if (bytesRead == 0)
                    break;
 
                OpCode opCode = (OpCode)message[0];
                switch (opCode)
                {
                    case OpCode.Heartbeat:
                        Console.WriteLine("Received heartbeat from server.");
                        break;
                    case OpCode.MemoryRead:
                        // Server is requesting a memory read
                        await PerformMemoryReadAsync(serverStream);
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log and handle exceptions
                break;
            }
        }
    }
 
    private async Task PerformMemoryReadAsync(NetworkStream serverStream)
    {
        foreach (var address in SharedResources.MemoryAddressesToCheck)
        {
            int value = await ProcessMemoryReader.ReadIntFromProcessAsync(Process.GetCurrentProcess(), address);
            byte[] message = new byte[] { (byte)OpCode.MemoryRead }
                .Concat(BitConverter.GetBytes(value)).ToArray();
            await serverStream.WriteAsync(message, 0, message.Length);
            await serverStream.FlushAsync();
        }
    }
}
