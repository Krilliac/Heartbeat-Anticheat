// HeartbeatServer.cs
 
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharedLibrary;
 
namespace HeartbeatServerApp
{
    /// <summary>
    /// The HeartbeatServer class is responsible for managing TCP connections and 
    /// maintaining a heartbeat signal to connected clients to ensure connectivity.
    /// </summary>
    public class HeartbeatServer
    {
        private TcpListener _listener; // TCP server listening for client connections
        private bool _isRunning; // Flag to control the server's listening loop
        private readonly ILogger<HeartbeatServer> _logger; // Logger for logging information and errors
 
        /// <summary>
        /// Initializes a new instance of the HeartbeatServer class.
        /// </summary>
        /// <param name="port">The port number on which the server will listen for connections.</param>
        /// <param name="logger">The logger used for logging information and errors.</param>
        public HeartbeatServer(int port, ILogger<HeartbeatServer> logger)
        {
            _listener = new TcpListener(System.Net.IPAddress.Any, port); // Initialize the listener to accept connections on any network interface
            _logger = logger; // Set the logger
        }
 
        /// <summary>
        /// Starts the server and begins listening for incoming client connections.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task StartAsync()
        {
            try
            {
                _listener.Start(); // Start the listener
                _isRunning = true; // Set the flag to indicate the server is running
                _logger.LogInformation("Server started."); // Log server start information
 
                while (_isRunning) // Continue to listen for clients as long as the server is running
                {
                    var client = await _listener.AcceptTcpClientAsync(); // Accept a new client connection
                    _ = HandleClientCommAsync(client); // Handle communication with the connected client asynchronously
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting server."); // Log any exceptions that occur during server start
            }
        }
 
        /// <summary>
        /// Handles communication with a connected client.
        /// </summary>
        /// <param name="client">The connected TcpClient object.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task HandleClientCommAsync(TcpClient client)
        {
            using var clientStream = client.GetStream(); // Get the network stream for reading and writing
            var message = new byte[] { (byte)OpCode.Heartbeat }; // Prepare a heartbeat message
 
            while (_isRunning) // Continue to communicate with the client as long as the server is running
            {
                try
                {
                    await clientStream.WriteAsync(message, 0, message.Length); // Send the heartbeat message to the client
                    await clientStream.FlushAsync(); // Ensure the message is sent immediately
                    await Task.Delay(5000); // Wait for 5 seconds before sending the next heartbeat
 
                    var buffer = new byte[1024]; // Buffer for reading incoming messages
                    int bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length); // Read the incoming message
                    if (bytesRead > 0) // If a message was received
                    {
                        OpCode opCode = (OpCode)buffer[0]; // Get the operation code from the message
                        switch (opCode) // Handle different operation codes
                        {
                            case OpCode.CheatDetected: // If a cheat was detected
                                string cheatName = SharedDefinitions.GetString(buffer.Skip(1).ToArray()); // Get the cheat name from the message
                                _logger.LogWarning($"Cheat detected: {cheatName}"); // Log a warning with the cheat name
                                break;
                            case OpCode.MemoryRead: // If a memory read operation occurred
                                int memoryValue = BitConverter.ToInt32(buffer, 1); // Get the memory value from the message
                                _logger.LogInformation($"Memory read value: {memoryValue}"); // Log the memory value
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during client communication."); // Log any exceptions that occur during client communication
                    break; // Exit the loop if an error occurs
                }
            }
        }
    }
}
