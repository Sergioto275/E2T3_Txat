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

        static void Main(string[] args)
        {
            zerbitzari.Start();
            Console.WriteLine("Zerbitzaria abian dago. Konexioak itxaroten...");

            while (true)
            {
                TcpClient client = zerbitzari.AcceptTcpClient();
                bezeroak.Add(client);

                Thread bThread = new Thread(() => Bezeroak(client));
                bThread.Start();
            }
        }

        static void Bezeroak(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];
            int bytesRead;

            bytesRead = stream.Read(buffer, 0, buffer.Length);
            string erabiltzailea = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
            bzrIzena.Add(erabiltzailea);
            BroadcastMessage($"{erabiltzailea} txatean sartu da", client);

            Console.WriteLine($"{erabiltzailea} konektatu da.");

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                // Mezua beste bezero guztiei bidali
                BroadcastMessage($"{erabiltzailea}: {message}", client);
            }

            // Deskonexioan dagoen erabiltzailea ezabatu
            bezeroak.Remove(client);
            bzrIzena.Remove(erabiltzailea);
            BroadcastMessage($"{erabiltzailea} deskonektatu da", client);
            Console.WriteLine($"{erabiltzailea} joan da.");
        }

        static void BroadcastMessage(string message, TcpClient senderClient)
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
    }
}
