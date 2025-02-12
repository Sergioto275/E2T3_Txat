using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bezeroa
{
    public partial class Form1 : Form
    {
        private TcpClient client = null!;
        private NetworkStream stream = null!;
        private byte[] buffer = new byte[256];
        private string erabiltzaile = null!;
        private Label lblEstatus;
        private Label lblHitzorduakLabel;

        // HttpClient objektu bakarra
        private static readonly HttpClient httpClient = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            ConfigureHitzorduakTab(); // Hitzorduak fitxaren konfigurazioa
        }

        private void ConfigureHitzorduakTab()
        {
            // "Hitzorduak" fitxaren konfigurazioa
            var tabPage = new TabPage("Hitzorduak");
            lblHitzorduak = new Label
            {
                Text = "Gaurko hitzorduak:", // Etiketa hasieratu
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(500, 200)
            };
            tabPage.Controls.Add(lblHitzorduak);

            // Beste fitxak ezabatu eta "Hitzorduak" bakarra gehitu
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
                lblEstatus.Text = "Konektatzen...";
                lblEstatus.ForeColor = System.Drawing.Color.DarkOrange;
                Application.DoEvents();

                // TCP konektatzea
                client = new TcpClient("localhost", 5000);
                stream = client.GetStream();

                // Erabiltzaile izena bidaltzea
                erabiltzaile = txtErabiltzailea.Text.Trim();
                byte[] nameBytes = Encoding.ASCII.GetBytes(erabiltzaile);
                stream.Write(nameBytes, 0, nameBytes.Length);

                // Txat kontrolak aktibatzea
                ToggleChatControls(true);

                lblEstatus.Text = "Konektatuta !";
                lblEstatus.ForeColor = System.Drawing.Color.DarkGreen; // Cambiar el color del texto a verde
                StartReadingMessages();
            }
            catch (Exception ex)
            {
                if (lblEstatus != null)
                {
                    lblEstatus.Text = "Errorea konektatzean";
                }
                MessageBox.Show("Errorea: " + ex.Message);
            }
        }

        private void ToggleChatControls(bool enable)
        {
            // Txat kontrolak aktibatu edo desaktibatu
            txtErabiltzailea.Enabled = !enable;
            btnKonex.Enabled = !enable;
            txtMesua.Enabled = enable;
            btnBidali.Enabled = enable;
        }
        private void StartReadingMessages()
        {
            // Mezuek irakurtzeko thread-a abiaraztea
            var readThread = new System.Threading.Thread(MesuakJaso);
            readThread.Start();
        }

        private void btnBidali_Click(object sender, EventArgs e)
        {
            try
            {
                string message = txtMesua.Text.Trim();
                if (string.IsNullOrEmpty(message)) return; // Mezu hutsik ez bidali

                // Mezuaren byteak bidaltzea
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                stream.Write(messageBytes, 0, messageBytes.Length);

                // Txat-ean mezuaren erakustea
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                lstTxat.Items.Add($"{erabiltzaile} [{timestamp}]: {message}");
                txtMesua.Clear(); // Testua ezabatzea
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar mensaje: " + ex.Message); // Bidalketa errorea
            }
        }
        // Enter tekla erabilita mezuak bidaltzea
        private void txtMesua_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Enter tekla pasatzen da
                btnBidali_Click(sender ?? this, e); // Mezuak bidaltzen dira
            }
        }

        // Mezuek irakurtzea
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
                MessageBox.Show($"Stream-a irakurtzeko errorea: {ex.Message}"); // Irakurketako errorearen mezua
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null && client.Connected)
            {
                stream.Close();
                client.Close();
            }
        }

        // API-ra dei bat egiteko, gaurko hitzorduak lortzeko
        private async void tabPageHitzorduak_Click_1(object sender, EventArgs e)
        {
            try
            {
                lblHitzorduak.Text = "Kargatzen hitzorduak...";

                // Hitzorduak lortzea API-tik
                HttpResponseMessage response = await httpClient.GetAsync("http://localhost:8080/api/hitzorduak/hoy");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    lblHitzorduak.Text = "Gaurko hitzorduak:\n" + responseData;
                }
                else
                {
                    lblHitzorduak.Text = $"Hitzorduen lortutako errorea. Egoera kodea: {response.StatusCode}";
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Erantzunaren errorea: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                lblHitzorduak.Text = "Errorea eskaera egitean.";
                MessageBox.Show($"Errorea: {ex.Message}");
            }
        }
    }
}