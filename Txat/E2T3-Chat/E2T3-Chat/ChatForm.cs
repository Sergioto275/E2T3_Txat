using System;
using System.Windows.Forms;

namespace E2T3_Chat
{
    public partial class ChatForm : Form
    {
        private string erabiltzailea;

        public ChatForm(string erabiltzailea)
        {
            InitializeComponent();
            this.erabiltzailea = erabiltzailea;
            lblOngiEtorri.Text = $"Ongi etorri: @{erabiltzailea}!";
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            MesuaBidali();
        }

        // Enter pulsatzen duguenean mezua bidaltzeko
        private void txtMesua_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 
                MesuaBidali();
            }
        }

        private void MesuaBidali()
        {
            if (!string.IsNullOrWhiteSpace(txtMesua.Text))
            {
                string ordua = DateTime.Now.ToString("HH:mm:ss");
                lstTxat.Items.Add($"{erabiltzailea} [{ordua}]: {txtMesua.Text}");
                txtMesua.Clear();
            }
        }
    }
}
