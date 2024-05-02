namespace SharedLibrary
{
    /// <summary>
    /// Operation codes used for identifying different types of network messages.
    /// </summary>
    public enum OpCode
    {
        /// <summary>
        /// Heartbeat message indicating an active connection.
        /// </summary>
        Heartbeat = 0x01,
 
        /// <summary>
        /// Notification that a cheat has been detected.
        /// </summary>
        CheatDetected = 0x04,
 
        /// <summary>
        /// Request to read a specific memory address.
        /// </summary>
        MemoryRead = 0x05
 
        // Add other operation codes as needed
    }
 
    /// <summary>
    /// Contains shared definitions and utility methods used across the application.
    /// </summary>
    public static class SharedDefinitions
    {
        /// <summary>
        /// List of known cheat programs to monitor.
        /// </summary>
        public static readonly string[] CheatPrograms = { "cheatengine", "hacktool", "trainer", "speedhack" };
 
        /// <summary>
        /// Memory addresses that are critical for cheat detection.
        /// </summary>
        public static readonly IntPtr[] MemoryAddressesToCheck = { /* Add memory addresses here */ };
 
        /// <summary>
        /// Converts a string to its ASCII byte array representation.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The ASCII byte array.</returns>
        public static byte[] GetBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
 
        /// <summary>
        /// Converts an ASCII byte array to its string representation.
        /// </summary>
        /// <param name="bytes">The byte array to convert.</param>
        /// <returns>The string representation.</returns>
        public static string GetString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
