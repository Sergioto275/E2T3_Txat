using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Bezeroa
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private byte[] buffer = new byte[256];
        private string erabiltzaile;

        public Form1()
        {
            InitializeComponent();
            // Asociar el evento KeyDown al campo de texto para detectar la tecla Enter
            txtMesua.KeyDown += txtMesua_KeyDown;

            // Crear la pestaña "Gaurko hitzorduak"
            TabPage tabPage = new TabPage("Gaurko hitzorduak");
            // Aquí puedes agregar los controles que desees en esta pestaña
            Label lblHitzorduak = new Label();
            lblHitzorduak.Text = "Lista de hitzorduak para hoy:";
            lblHitzorduak.Location = new System.Drawing.Point(10, 10);
            tabPage.Controls.Add(lblHitzorduak);
            tabHitzorduak.TabPages.Add(tabPage);
        }

        private void btnKonektatu_Click(object sender, EventArgs e)
        {
            try
            {
                // Conectar al servidor
                client = new TcpClient("localhost", 5000);
                stream = client.GetStream();

                // Obtener el nombre de usuario
                erabiltzaile = txtErabiltzailea.Text.Trim();
                byte[] nameBytes = Encoding.ASCII.GetBytes(erabiltzaile);
                stream.Write(nameBytes, 0, nameBytes.Length);

                // Deshabilitar el campo de nombre y habilitar el chat
                txtErabiltzailea.Enabled = false;
                btnKonex.Enabled = false;
                txtMesua.Enabled = true;
                btnBidali.Enabled = true;

                // Iniciar hilo para escuchar los mensajes
                var readThread = new System.Threading.Thread(MesuakJaso);
                readThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message);
            }
        }

        private void btnBidali_Click(object sender, EventArgs e)
        {
            try
            {
                // Enviar mensaje
                string message = txtMesua.Text.Trim();
                if (string.IsNullOrEmpty(message))
                {
                    return; // No enviar si el mensaje está vacío
                }
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                stream.Write(messageBytes, 0, messageBytes.Length);

                // Mostrar el mensaje en el ListBox con nombre de usuario y hora
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                lstTxat.Items.Add($"{erabiltzaile} [{timestamp}]: {message}");
                txtMesua.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar mensaje: " + ex.Message);
            }
        }

        // Manejo del evento KeyDown en txtMesua para enviar el mensaje cuando se presiona Enter
        private void txtMesua_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificar si se presionó Enter (código de la tecla Enter es 13)
            if (e.KeyCode == Keys.Enter)
            {
                // Prevenir el sonido de la tecla Enter
                e.SuppressKeyPress = true;

                // Llamar al método para enviar el mensaje, igual que al hacer clic en el botón
                btnBidali_Click(sender, e);
            }
        }
        private void MesuakJaso()
        {
            int bytesRead;
            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    // Usar Invoke para actualizar la UI desde un hilo distinto
                    Invoke(new Action(() =>
                    {
                        string timestamp = DateTime.Now.ToString("HH:mm:ss");
                        lstTxat.Items.Add($"{message} [{timestamp}]");
                    }));
                }
            }
            catch (IOException ex)
            {
                // Log o manejo de la excepción
                MessageBox.Show($"Error reading from stream: {ex.Message}");
            }
        }

        // Método para cerrar la conexión cuando se cierra la aplicación
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null && client.Connected)
            {
                stream.Close();
                client.Close();
            }
        }

        private void tabPageHitzorduak_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Hitzorduak pestaña click");

        }

        private void lstTxat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
