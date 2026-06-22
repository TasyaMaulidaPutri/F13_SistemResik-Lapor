using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SISTEM_RESIK_LAPOR
{
    public partial class Form1 : Form
    {
        string connString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA; Initial Catalog=DBResikLaporADO; Integrated Security=True";
        SqlConnection conn;
        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectDatabase()
        {

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                MessageBox.Show("Koneksi berhasil");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^[a-zA-Z0-9]+$");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    lblStatus.Text = "Status : Koneksi Berhasil";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Status : Koneksi Gagal";
                    MessageBox.Show(ex.Message);
                }
            }

            linkRegister.Text = "Belum punya akun? Registrasi";
            linkRegister.Visible = true;
            linkRegister.BringToFront();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Email dan password harus diisi!");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Format email tidak valid!");
                return;
            }

            if (!IsValidPassword(password))
            {
                MessageBox.Show("Password tidak boleh mengandung simbol!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = "SELECT id_user, role FROM Users WHERE email=@email AND password=@password";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int idUser = Convert.ToInt32(reader["id_user"]);
                        string role = reader["role"].ToString();

                        MessageBox.Show("Login sebagai " + role);

                        Form4 menu = new Form4(idUser, role);
                        menu.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Email atau password salah!");
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form5 register = new Form5();
            register.Show();
            this.Hide();
        }

    }
    
}
