using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Zerbitzaria
{
    class Program
    {
        private static List<TcpClient> bezeroak = new List<TcpClient>();
        private static List<string> bzrIzena = new List<string>();
        private static TcpListener zerbitzari = new TcpListener(IPAddress.Any, 5000);
        private const int MaxBezeroak = 15;

        static void Main(string[] args)
        {
            zerbitzari.Start();
            Console.WriteLine("Zerbitzaria abian dago. Konexioak itxaroten...");
            while (true)
            {
                try
                {
                    if (bezeroak.Count >= MaxBezeroak)
                    {
                        Console.WriteLine("Errorea: Gehienezko bezero kopurua gaindituta. Zerbitzaria itxiko da.");
                        zerbitzari.Stop();
                        Environment.Exit(1);
                    }
                    TcpClient client = zerbitzari.AcceptTcpClient();
                    Thread bThread = new Thread(() => Bezeroak(client));
                    bThread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errorea: {ex.Message}");
                }
            }
        }

        static void Bezeroak(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[256];
                int bytesRead;

                // Erabiltzaile izena irakurri
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                string erabiltzailea = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();

                // Erabiltzaile izena egiaztatu
                if (bzrIzena.Contains(erabiltzailea))
                {
                    string errorMessage = "Errorea: Erabiltzaile izena jada erregistratuta dago.";
                    byte[] errorBytes = Encoding.ASCII.GetBytes(errorMessage);
                    stream.Write(errorBytes, 0, errorBytes.Length);
                    client.Close();
                    return;
                }

                bzrIzena.Add(erabiltzailea);
                bezeroak.Add(client);
                BroadcastMessage($"{erabiltzailea} txatean sartu da", client);

                Console.WriteLine($"{erabiltzailea} konektatu da.");

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    BroadcastMessage($"{erabiltzailea}: {message}", client);
                }

                // Bezeroa deskonektatu
                bezeroak.Remove(client);
                bzrIzena.Remove(erabiltzailea);
                BroadcastMessage($"{erabiltzailea} deskonektatu da", client);
                Console.WriteLine($"{erabiltzailea} joan da.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errorea bezeroaren konektatzean: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        static void BroadcastMessage(string message, TcpClient senderClient)
        {
            try
            {
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                foreach (var client in bezeroak)
                {
                    if (client != senderClient)
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(messageBytes, 0, messageBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errorea mezua bidaltzean: {ex.Message}");
            }
        }
    }
}
