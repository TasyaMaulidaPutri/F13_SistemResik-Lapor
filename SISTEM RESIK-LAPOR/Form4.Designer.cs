namespace SISTEM_RESIK_LAPOR
{
    partial class Form4
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnLaporan = new System.Windows.Forms.Button();
            this.btnSetoran = new System.Windows.Forms.Button();
            this.btnKelolaLaporan = new System.Windows.Forms.Button();
            this.btnVerifikasiSetoran = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.label1.Location = new System.Drawing.Point(228, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "SISTEM RESIK-LAPOR";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblUser.Location = new System.Drawing.Point(304, 91);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(140, 25);
            this.lblUser.TabIndex = 1;
            this.lblUser.Text = "Login sebagai:";
            this.lblUser.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnLaporan
            // 
            this.btnLaporan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnLaporan.Location = new System.Drawing.Point(131, 174);
            this.btnLaporan.Name = "btnLaporan";
            this.btnLaporan.Size = new System.Drawing.Size(166, 47);
            this.btnLaporan.TabIndex = 2;
            this.btnLaporan.Text = "Buat Laporan";
            this.btnLaporan.UseVisualStyleBackColor = true;
            this.btnLaporan.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSetoran
            // 
            this.btnSetoran.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSetoran.Location = new System.Drawing.Point(131, 227);
            this.btnSetoran.Name = "btnSetoran";
            this.btnSetoran.Size = new System.Drawing.Size(166, 45);
            this.btnSetoran.TabIndex = 3;
            this.btnSetoran.Text = "Buat Setoran";
            this.btnSetoran.UseVisualStyleBackColor = true;
            this.btnSetoran.Click += new System.EventHandler(this.btnSetoran_Click);
            // 
            // btnKelolaLaporan
            // 
            this.btnKelolaLaporan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnKelolaLaporan.Location = new System.Drawing.Point(489, 174);
            this.btnKelolaLaporan.Name = "btnKelolaLaporan";
            this.btnKelolaLaporan.Size = new System.Drawing.Size(177, 47);
            this.btnKelolaLaporan.TabIndex = 4;
            this.btnKelolaLaporan.Text = "Kelola Laporan";
            this.btnKelolaLaporan.UseVisualStyleBackColor = true;
            this.btnKelolaLaporan.Click += new System.EventHandler(this.btnKelolaLaporan_Click);
            // 
            // btnVerifikasiSetoran
            // 
            this.btnVerifikasiSetoran.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnVerifikasiSetoran.Location = new System.Drawing.Point(489, 227);
            this.btnVerifikasiSetoran.Name = "btnVerifikasiSetoran";
            this.btnVerifikasiSetoran.Size = new System.Drawing.Size(177, 45);
            this.btnVerifikasiSetoran.TabIndex = 5;
            this.btnVerifikasiSetoran.Text = "Verifikasi Setoran";
            this.btnVerifikasiSetoran.UseVisualStyleBackColor = true;
            this.btnVerifikasiSetoran.Click += new System.EventHandler(this.btnVerifikasiSetoran_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button5.Location = new System.Drawing.Point(131, 383);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(106, 39);
            this.button5.TabIndex = 6;
            this.button5.Text = "Logout";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.btnVerifikasiSetoran);
            this.Controls.Add(this.btnKelolaLaporan);
            this.Controls.Add(this.btnSetoran);
            this.Controls.Add(this.btnLaporan);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.label1);
            this.Name = "Form4";
            this.Text = "Form4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Button btnLaporan;
        private System.Windows.Forms.Button btnSetoran;
        private System.Windows.Forms.Button btnKelolaLaporan;
        private System.Windows.Forms.Button btnVerifikasiSetoran;
        private System.Windows.Forms.Button button5;
    }
}