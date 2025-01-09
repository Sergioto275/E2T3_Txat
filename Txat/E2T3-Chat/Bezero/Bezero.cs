using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Bezero
{
    private static TcpClient client;
    private static NetworkStream stream;
    private static byte[] buffer = new byte[1024];

    public static async Task Main()
    {
        try
        {
            client = new TcpClient("192.168.213.1", 5000); // IP del servidor (tu IP)
            stream = client.GetStream();

            Console.WriteLine("Zerbitzari konektatzen.");

            // Crear un task para recibir mensajes
            Task.Run(() => ReceiveMessages());

            while (true)
            {
                // Mostrar el mensaje para escribir un nuevo mensaje
                Console.Write("\nIdatzi mezua: ");
                string message = Console.ReadLine();
                if (message.ToLower() == "irten") break;

                // Enviar el mensaje al servidor
                await SendMessage(message);

                // Mostrar el mensaje con la fecha y hora antes de enviarlo
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"[{timestamp}] Tú: {message}");
                Console.ResetColor();
            }

            // Cerrar la conexión
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
        }
    }

    // Recibir mensajes del servidor
    private static async Task ReceiveMessages()
    {
        int bytesRead;
        while (true)
        {
            try
            {
                bytesRead = await Task.Run(() => stream.Read(buffer, 0, buffer.Length));
                if (bytesRead == 0) break;

                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                if (!string.IsNullOrEmpty(message))  // Verificar que no esté vacío
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Servidor: {message}");
                    Console.ResetColor();
                }
            }
            catch (Exception)
            {
                break;
            }
        }
    }

    // Enviar mensaje al servidor
    private static async Task SendMessage(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        await Task.Run(() => stream.Write(data, 0, data.Length));
    }
}
