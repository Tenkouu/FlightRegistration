// In SimpleTcpClient/Program.cs
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string serverIp = "127.0.0.1"; // localhost
        int serverPort = 8899;       // Port your AgentSocketServer is listening on

        Console.WriteLine($"Attempting to connect to server at {serverIp}:{serverPort}...");

        try
        {
            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync(serverIp, serverPort);
                Console.WriteLine("Connected to server!");

                NetworkStream stream = client.GetStream();
                Console.WriteLine("Listening for messages from the server...");
                Console.WriteLine("---");

                // Optional: Send an identification message or a test message
                // string testMessage = "Hello from SimpleTcpClient!\n";
                // byte[] messageBytes = Encoding.UTF8.GetBytes(testMessage);
                // await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                // Console.WriteLine($"Sent: {testMessage.Trim()}");
                // Console.WriteLine("---");


                byte[] buffer = new byte[4096]; // Buffer to store incoming data
                int bytesRead;

                // Loop to continuously read data from the server
                while (client.Connected && (bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"RECEIVED: {receivedMessage.TrimEnd()}"); // TrimEnd to remove trailing newlines for cleaner console output
                    Console.ResetColor();
                    Console.WriteLine(); // Add a newline for the next message
                    Console.WriteLine("---");
                }

                if (!client.Connected)
                {
                    Console.WriteLine("Server disconnected.");
                }
            }
        }
        catch (SocketException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"SocketException: {ex.Message}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("Client closed. Press any key to exit.");
        Console.ReadKey();
    }
}