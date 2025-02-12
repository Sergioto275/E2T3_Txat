namespace Bezeroa
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtErabiltzailea;
        private System.Windows.Forms.Button btnKonex;
        private System.Windows.Forms.TextBox txtMesua;
        private System.Windows.Forms.Button btnBidali;
        private System.Windows.Forms.ListBox lstTxat;
        private System.Windows.Forms.TabControl tabHitzorduak;
        private System.Windows.Forms.TabPage tabPageHitzorduak;
        private System.Windows.Forms.Label lblHitzorduak; // First declaration (this should remain)
        private System.Windows.Forms.Label lblStatus;

        private void InitializeComponent()
        {
            txtErabiltzailea = new TextBox();
            btnKonex = new Button();
            txtMesua = new TextBox();
            btnBidali = new Button();
            lstTxat = new ListBox();
            tabHitzorduak = new TabControl();
            tabPageHitzorduak = new TabPage();
            lblHitzorduak = new Label();
            lblStatus = new Label();
            tabHitzorduak.SuspendLayout();
            tabPageHitzorduak.SuspendLayout();
            SuspendLayout();
            // 
            // txtErabiltzailea
            // 
            txtErabiltzailea.Location = new Point(12, 10);
            txtErabiltzailea.Name = "txtErabiltzailea";
            txtErabiltzailea.Size = new Size(260, 23);
            txtErabiltzailea.TabIndex = 0;
            txtErabiltzailea.TextChanged += txtErabiltzailea_TextChanged;
            // 
            // btnKonex
            // 
            btnKonex.Location = new Point(278, 9);
            btnKonex.Name = "btnKonex";
            btnKonex.Size = new Size(75, 23);
            btnKonex.TabIndex = 1;
            btnKonex.Text = "Konektatu";
            btnKonex.UseVisualStyleBackColor = true;
            btnKonex.Click += btnKonektatu_Click;
            // 
            // txtMesua
            // 
            txtMesua.Enabled = false;
            txtMesua.Location = new Point(12, 222);
            txtMesua.Name = "txtMesua";
            txtMesua.Size = new Size(260, 23);
            txtMesua.TabIndex = 2;
            txtMesua.KeyDown += txtMesua_KeyDown;
            // 
            // btnBidali
            // 
            btnBidali.Enabled = false;
            btnBidali.Location = new Point(278, 220);
            btnBidali.Name = "btnBidali";
            btnBidali.Size = new Size(75, 24);
            btnBidali.TabIndex = 3;
            btnBidali.Text = "Bidali";
            btnBidali.UseVisualStyleBackColor = true;
            btnBidali.Click += btnBidali_Click;
            // 
            // lstTxat
            // 
            lstTxat.FormattingEnabled = true;
            lstTxat.ItemHeight = 15;
            lstTxat.Location = new Point(12, 75);
            lstTxat.Name = "lstTxat";
            lstTxat.Size = new Size(341, 139);
            lstTxat.TabIndex = 4;
            lstTxat.SelectedIndexChanged += lstTxat_SelectedIndexChanged;
            // 
            // tabHitzorduak
            // 
            tabHitzorduak.Controls.Add(tabPageHitzorduak);
            tabHitzorduak.Location = new Point(12, 261);
            tabHitzorduak.Name = "tabHitzorduak";
            tabHitzorduak.SelectedIndex = 0;
            tabHitzorduak.Size = new Size(341, 111);
            tabHitzorduak.TabIndex = 5;
            // 
            // tabPageHitzorduak
            // 
            tabPageHitzorduak.Controls.Add(lblHitzorduak);
            tabPageHitzorduak.Location = new Point(4, 24);
            tabPageHitzorduak.Name = "tabPageHitzorduak";
            tabPageHitzorduak.Size = new Size(333, 83);
            tabPageHitzorduak.TabIndex = 0;
            tabPageHitzorduak.Click += tabPageHitzorduak_Click_1;
            // 
            // lblHitzorduak
            // 
            lblHitzorduak.AutoSize = true;
            lblHitzorduak.Location = new Point(3, 3);
            lblHitzorduak.Name = "lblHitzorduak";
            lblHitzorduak.Size = new Size(0, 15);
            lblHitzorduak.TabIndex = 0;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 38);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 6;
            lblStatus.Click += lblStatus_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(370, 384);
            Controls.Add(lblStatus);
            Controls.Add(tabHitzorduak);
            Controls.Add(lstTxat);
            Controls.Add(btnBidali);
            Controls.Add(txtMesua);
            Controls.Add(btnKonex);
            Controls.Add(txtErabiltzailea);
            Name = "Form1";
            Text = "Ileapandegia Txat";
            tabHitzorduak.ResumeLayout(false);
            tabPageHitzorduak.ResumeLayout(false);
            tabPageHitzorduak.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void txtErabiltzailea_TextChanged(object sender, EventArgs e)
        {
            // Add your implementation here
            // Example: Update a label with the current text
            lblStatus.Text = txtErabiltzailea.Text;
        }

        private void lstTxat_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {
        }





    }
}
