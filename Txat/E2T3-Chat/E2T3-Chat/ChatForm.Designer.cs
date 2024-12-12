namespace E2T3_Chat
{
    partial class ChatForm
    {
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label lblOngiEtorri;
        private System.Windows.Forms.TextBox txtMesua;
        private System.Windows.Forms.ListBox lstTxat;
        private System.Windows.Forms.Button bidaliBtn;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem kontsultakMenu;
        private System.Windows.Forms.ToolStripMenuItem stockIkusi;
        private System.Windows.Forms.ToolStripMenuItem zitakIkusi;
        private void InitializeComponent()
        {
            lblOngiEtorri = new Label();
            txtMesua = new TextBox();
            lstTxat = new ListBox();
            bidaliBtn = new Button();
            menuStrip = new MenuStrip();
            kontsultakMenu = new ToolStripMenuItem();
            stockIkusi = new ToolStripMenuItem();
            zitakIkusi = new ToolStripMenuItem();

            SuspendLayout();

            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { kontsultakMenu });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(400, 28);
            menuStrip.TabIndex = 4;
            menuStrip.Text = "menuStrip";

            // 
            // konstuakMenu
            // 
            kontsultakMenu.DropDownItems.AddRange(new ToolStripItem[] { stockIkusi, zitakIkusi });
            kontsultakMenu.Name = "kontsultakMenu";
            kontsultakMenu.Size = new Size(67, 24);
            kontsultakMenu.Text = "Kontsultak";

            // 
            // stockIkusi
            // 
            stockIkusi.Name = "stockIkusi";
            stockIkusi.Size = new Size(180, 26);
            stockIkusi.Text = "Stock";
            stockIkusi.Click += stockIkusi_Click;

            // 
            // Zitak
            // 
            zitakIkusi.Name = "zitakIkusi";
            zitakIkusi.Size = new Size(180, 26);
            zitakIkusi.Text = "Gaurko zitak";
            zitakIkusi.Click += zitakIkusi_Click;

            // 
            // lblOngiEtorri
            // 
            lblOngiEtorri.AutoSize = true;
            lblOngiEtorri.Location = new Point(10, 40);
            lblOngiEtorri.Name = "lblOngiEtorri";
            lblOngiEtorri.Size = new Size(89, 20);
            lblOngiEtorri.TabIndex = 0;
            lblOngiEtorri.Text = "Ongi etorri !";

            // 
            // txtMesua
            // 
            txtMesua.Location = new Point(10, 247);
            txtMesua.Name = "txtMesua";
            txtMesua.Size = new Size(280, 27);
            txtMesua.TabIndex = 2;
            txtMesua.KeyDown += txtMesua_KeyDown;

            // 
            // lstTxat
            // 
            lstTxat.FormattingEnabled = true;
            lstTxat.Location = new Point(10, 70);
            lstTxat.Name = "lstTxat";
            lstTxat.Size = new Size(374, 164);
            lstTxat.TabIndex = 1;

            // 
            // bidaliBtn
            // 
            bidaliBtn.Location = new Point(296, 246);
            bidaliBtn.Name = "bidaliBtn";
            bidaliBtn.Size = new Size(88, 28);
            bidaliBtn.TabIndex = 3;
            bidaliBtn.Text = "Bidali";
            bidaliBtn.UseVisualStyleBackColor = true;
            bidaliBtn.Click += btnSend_Click;

            // 
            // Txat
            // 
            ClientSize = new Size(400, 300);
            Controls.Add(menuStrip);
            Controls.Add(lblOngiEtorri);
            Controls.Add(lstTxat);
            Controls.Add(txtMesua);
            Controls.Add(bidaliBtn);
            MainMenuStrip = menuStrip;
            Name = "Txat";
            Text = "Txat";
            ResumeLayout(false);
            PerformLayout();
        }

        // STOCKA IKUSIKO DUGU EBENTUA
        private void stockIkusi_Click(object sender, EventArgs e)
        {
            MessageBox.Show("XXXXX.", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ZITAK IKUSIKO DUGU EBENTUA
        private void zitakIkusi_Click(object sender, EventArgs e)
        {
            List<string> gaurkoZitak = hartuGaurtkoZitak();

            Form zitakLehioa = new Form();
            zitakLehioa.Text = "Gaurko zitak";

            //Zitak ikusiko dugu
            ListBox lbZitak = new ListBox();
            lbZitak.Dock = DockStyle.Fill; 
            lbZitak.DataSource = gaurkoZitak;

            zitakLehioa.Controls.Add(lbZitak);
            zitakLehioa.Show();
        }

        //Lista batekin zitak ikusiko ditugu (gero JSON edo API bitzartez)
        private List<string> hartuGaurtkoZitak()
        {
            return new List<string>
          {
                 "[Izena 1]: 10:00 AM",
                 "Izena 2: 12:30 PM",
                 "Izena 3: 3:00 PM",
                 "Izena 4: 5:30 PM",
         };
        }

    }
}
