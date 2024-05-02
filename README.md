# Heartbeat-Anticheat

!!!UNTESTED AND LARGELY AI GENERATED WITH NO MSVS PROJECT!!!

The client is meant to be built as a DLL, embedded and encrypted into an application of choice, but you probably could internally implement or run as a standalone console as well.

Addresses and cheat names are hard coded, may move over to NoSQL/MySQL at a future point in time.

If time permits in the future, this project will be fleshed out a little more, along with a proper project setup.

However, whoever is reading this, wishes to implement, test and commit to the project, feel free to do so.

Documentation for Heartbeat Server and Client

Introduction
The Heartbeat Server and Client are components of a client-server application that communicates over a TCP connection. The server sends heartbeat messages to clients, detects cheat programs, and reads memory values. The client listens for server messages and performs memory reads.

Components
1. SharedLibrary (Shared Library)
Contains shared resources, enums, and utility methods used by both the server and client.

Enums:
OpCode: Defines operation codes for different message types (e.g., Heartbeat, CheatDetected, MemoryRead).

Classes:
CheatProgram: Represents a cheat program name. (ie, CheatEngine.exe)
MemoryAddress: Represents a memory address to check. (ie, 0x004000)

Methods:
GetBytes(string str): Converts a string to a byte array.
GetString(byte[] bytes): Converts a byte array to a string.

2. HeartbeatServer (Server)
Listens for incoming client connections and handles communication.

Features:
Heartbeat messages every 5 seconds.
Cheat detection (logs detected cheat program names).
Memory read requests (logs read memory values).

Resource Cleanup:
Properly disposes of resources (e.g., network streams).

Logging:
Logs server events and exceptions.

Documentation:
Detailed setup instructions.
Explanation of cheat detection and memory read handling.

3. HeartbeatClient (Client)
Connects to the server and listens for messages.

Features:
Heartbeat response to the server.
Memory read requests (reads memory values from the process).

Resource Cleanup:
Properly disposes of resources (e.g., network streams).

Logging:
Logs client events and exceptions.
