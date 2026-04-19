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
    public partial class Form1 : System.Windows.Forms.Form
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
        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString);

            try
            {
                conn.Open();
                lblStatus.Text = "Status : Koneksi Berhasil";
                conn.Close();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status : Koneksi Gagal";
                MessageBox.Show(ex.Message);
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
            try
            {
                conn.Open();

                string query = "SELECT id_user, role FROM Users WHERE email=@email AND password=@password";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);

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
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
