using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Zerbitzaria
{
    private static TcpListener listener;
    private static TcpClient client;
    private static NetworkStream stream;
    private static byte[] buffer = new byte[1024];

    public static async Task Main()
    {
        try
        {
            listener = new TcpListener(IPAddress.Parse("192.168.213.1"), 5000); // Tu IP y puerto
            listener.Start();

            Console.WriteLine("Zerbitzaria abian da.");

            // Esperar a que un cliente se conecte
            client = await listener.AcceptTcpClientAsync();
            stream = client.GetStream();

            Console.WriteLine("Bezeroa konektatuta.");

            // Recibir y enviar mensajes de forma continua
            while (true)
            {
                // Recibir mensaje del cliente
                string message = await ReceiveMessage();
                if (message.ToLower() == "irten") break;

                // Mostrar mensaje recibido
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[Servidor] {message}");
                Console.ResetColor();

                // Responder al cliente
                await SendMessage("Mezu ongi jaso da.");
            }

            // Cerrar la conexión
            client.Close();
            listener.Stop();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Función para recibir mensajes
    private static async Task<string> ReceiveMessage()
    {
        int bytesRead = await Task.Run(() => stream.Read(buffer, 0, buffer.Length));
        return Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
    }

    // Función para enviar mensajes
    private static async Task SendMessage(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        await Task.Run(() => stream.Write(data, 0, data.Length));
    }
}
