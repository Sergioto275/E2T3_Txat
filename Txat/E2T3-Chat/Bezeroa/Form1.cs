using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Bezeroa
{
    public partial class Form1 : Form
    {
        private TcpClient client = null!;
        private NetworkStream stream = null!;
        private byte[] buffer = new byte[256];
        private string erabiltzaile = null!;
        private Label lblEstatus = new Label();
        private Label lblHitzorduakLabel = new Label();

        private static List<string> erabiltzaileAktiboak = new List<string>();
        private static readonly HttpClient httpClient = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            Hitzorduak(); // Hitzorduak fitxaren konfigurazioa
        }

        private void Hitzorduak()
        {
            var tabPage = new TabPage("Hitzorduak");
            lblHitzorduak = new Label
            {
                Text = "Gaurko hitzorduak:",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(500, 200)
            };
            tabPage.Controls.Add(lblHitzorduak);
            tabHitzorduak.TabPages.Clear();
            tabHitzorduak.TabPages.Add(tabPage);
        }

        private void btnKonektatu_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblEstatus == null)
                {
                    lblEstatus = new Label
                    {
                        Location = new System.Drawing.Point(10, 50),
                        Size = new System.Drawing.Size(200, 20)
                    };
                    this.Controls.Add(lblEstatus);
                }

                do
                {
                    erabiltzaile = Microsoft.VisualBasic.Interaction.InputBox("Sartu erabiltzaile izena:", "Erabiltzailea");
                    if (string.IsNullOrWhiteSpace(erabiltzaile))
                    {
                        MessageBox.Show("Erabiltzaile izena ezin da hutsik egon.");
                        continue;
                    }
                } while (erabiltzaileAktiboak.Contains(erabiltzaile));

                lblEstatus.Text = "Konektatzen...";
                lblEstatus.ForeColor = System.Drawing.Color.DarkOrange;
                Application.DoEvents();

                client = new TcpClient("localhost", 5000);
                stream = client.GetStream();
                erabiltzaileAktiboak.Add(erabiltzaile);

                byte[] nameBytes = Encoding.ASCII.GetBytes(erabiltzaile);
                stream.Write(nameBytes, 0, nameBytes.Length);

                ToggleChatControls(true);

                lblEstatus.Text = "Konektatuta !";
                lblEstatus.ForeColor = System.Drawing.Color.DarkGreen;
                StartReadingMessages();
            }
            catch (Exception ex)
            {
                lblEstatus.Text = "Errorea konektatzean";
                MessageBox.Show("Errorea: " + ex.Message);
            }
        }

        private void ToggleChatControls(bool enable)
        {
            txtErabiltzailea.Enabled = !enable;
            btnKonex.Enabled = !enable;
            txtMesua.Enabled = enable;
            btnBidali.Enabled = enable;
        }

        private void StartReadingMessages()
        {
            var readThread = new System.Threading.Thread(MesuakJaso);
            readThread.Start();
        }

        private void btnBidali_Click(object sender, EventArgs e)
        {
            try
            {
                string message = txtMesua.Text.Trim();
                if (string.IsNullOrEmpty(message)) return;

                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                stream.Write(messageBytes, 0, messageBytes.Length);

                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                lstTxat.Items.Add($"{erabiltzaile} [{timestamp}]: {message}");
                txtMesua.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar mensaje: " + ex.Message);
            }
        }

        private void txtMesua_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnBidali_Click(sender ?? this, e);
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
                    Invoke(new Action(() =>
                    {
                        string timestamp = DateTime.Now.ToString("HH:mm:ss");
                        lstTxat.Items.Add($"{message} [{timestamp}]");
                    }));
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Stream-a irakurtzeko errorea: {ex.Message}");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null && client.Connected)
            {
                erabiltzaileAktiboak.Remove(erabiltzaile);
                stream.Close();
                client.Close();
            }
        }
    }
}
