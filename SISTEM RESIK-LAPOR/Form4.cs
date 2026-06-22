using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SISTEM_RESIK_LAPOR
{
    public partial class Form4 : Form
    {
        string connString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA; Initial Catalog=DBResikLaporADO; Integrated Security=True";
        SqlConnection conn;
        int idUserLogin;
        string roleUser;
        public Form4(int idUser, string role)
        {
            InitializeComponent();
            idUserLogin = idUser;
            roleUser = role.ToLower();
            this.Load += Form4_Load;
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            lblUser.Text = "Login sebagai: " + roleUser;

            btnLaporan.Visible = false;
            btnSetoran.Visible = false;
            btnKelolaLaporan.Visible = false;
            btnVerifikasiSetoran.Visible = false;
            btnDashboard.Visible = false; // Dashboard disembunyikan dulu

            if (roleUser == "masyarakat")
            {
                btnLaporan.Visible = true;
                btnSetoran.Visible = true;
            }
            else if (roleUser == "admin")
            {
                btnKelolaLaporan.Visible = true;
                btnVerifikasiSetoran.Visible = true;
                btnDashboard.Visible = true;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (roleUser != "masyarakat")
            {
                MessageBox.Show("Akses ditolak!");
                return;
            }

            Form2 f2 = new Form2(idUserLogin, roleUser);
            f2.Show();
        }

        private void btnSetoran_Click(object sender, EventArgs e)
        {
            if (roleUser != "masyarakat")
    {
        MessageBox.Show("Akses ditolak!");
        return;
    }

    Form3 f3 = new Form3(idUserLogin, roleUser);
    f3.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }

        private void btnKelolaLaporan_Click(object sender, EventArgs e)
        {
            if (roleUser != "admin")
            {
                MessageBox.Show("Akses ditolak!");
                return;
            }

            Form2 f2 = new Form2(idUserLogin, roleUser);
            f2.Show();
        }

        private void btnVerifikasiSetoran_Click(object sender, EventArgs e)
        {
            if (roleUser != "admin")
            {
                MessageBox.Show("Akses ditolak!");
                return;
            }

            Form3 f3 = new Form3(idUserLogin, roleUser);
            f3.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (roleUser != "admin")
            {
                MessageBox.Show("Akses ditolak!");
                return;
            }

            Dashboard frmDashboard = new Dashboard();
            frmDashboard.Show();

            this.Hide();
        }
    }
}
