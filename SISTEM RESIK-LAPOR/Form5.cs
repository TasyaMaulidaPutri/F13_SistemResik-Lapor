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
    public partial class Form5 : Form
    {
        string connString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA; Initial Catalog=DBResikLaporADO; Integrated Security=True";

        public Form5()
        {
            InitializeComponent();
        }

        private void btnRegistrasi_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNama.Text == "" || txtEmail.Text == "" || txtPassword.Text == "")
                {
                    MessageBox.Show("Semua data wajib diisi!");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    
                    string cek = "SELECT COUNT(*) FROM Users WHERE email=@email";
                    SqlCommand cmdCek = new SqlCommand(cek, conn);
                    cmdCek.Parameters.AddWithValue("@email", txtEmail.Text);

                    int count = (int)cmdCek.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Email sudah terdaftar!");
                        return;
                    }


                    string query = @"INSERT INTO Users (nama, email, password, alamat, role) VALUES (@nama, @email, @password, @alamat, @role)";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@nama", txtNama.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text);
                    cmd.Parameters.AddWithValue("@role", "Masyarakat");

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registrasi berhasil sebagai Masyarakat!");

                   
                    Form1 login = new Form1();
                    login.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }
    }
}
