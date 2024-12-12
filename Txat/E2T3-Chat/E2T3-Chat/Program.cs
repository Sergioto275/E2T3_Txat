using System;
using System.Windows.Forms;

namespace E2T3_Chat
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            string erabiltzailea = $"{Guid.NewGuid().ToString("N").Substring(0, 8)}";
            Application.Run(new ChatForm(erabiltzailea));
        }
    }
}
