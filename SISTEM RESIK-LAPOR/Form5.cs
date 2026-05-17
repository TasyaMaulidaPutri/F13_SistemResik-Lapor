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
    public partial class Form5 : Form
    {
        string connString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA; Initial Catalog=DBResikLaporADO; Integrated Security=True";

        public Form5()
        {
            InitializeComponent();
        }

        private bool IsValidNama(string nama)
        {
            return Regex.IsMatch(nama, @"^[a-zA-Z\s]+$");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }

        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @".+");
        }

        private bool IsValidAlamat(string alamat)
        {
            return Regex.IsMatch(alamat, @"^[a-zA-Z0-9\s.,-]+$");
        }

        private void btnRegistrasi_Click(object sender, EventArgs e)
        {
            string nama = txtNama.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string alamat = txtAlamat.Text.Trim();

            if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(alamat))
            {
                MessageBox.Show("Semua data wajib diisi!");
                return; 
            }

            if (!IsValidNama(nama))
            {
                MessageBox.Show("Nama hanya boleh huruf dan spasi!");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Format email tidak valid atau mengandung karakter terlarang!");
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password minimal 6 karakter!");
                return;
            }

            if (!IsValidAlamat(alamat))
            {
                MessageBox.Show("Alamat tidak boleh mengandung simbol aneh! Hanya boleh huruf, angka, spasi, titik (.), koma (,), atau strip (-).");
                return; 
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string cek = "SELECT COUNT(*) FROM Users WHERE email=@email";
                    SqlCommand cmdCek = new SqlCommand(cek, conn);
                    cmdCek.Parameters.AddWithValue("@email", email);

                    int count = (int)cmdCek.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Email sudah terdaftar!");
                        return;
                    }

                    string query = @"INSERT INTO Users (nama, email, password, alamat, role) 
                             VALUES (@nama, @email, @password, @alamat, @role)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@alamat", alamat);
                    cmd.Parameters.AddWithValue("@role", "Masyarakat");

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registrasi berhasil!");

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

        private void textAlamat_TextChanged(object sender, EventArgs e)
        {

        }

        private void textAlamat_KeyPress(object sender, KeyPressEventArgs e)
        {
    
            if (char.IsLetterOrDigit(e.KeyChar))
            {
                return;
            }

            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (char.IsWhiteSpace(e.KeyChar))
            {
                return;
            }

            if (e.KeyChar == '.' || e.KeyChar == ',' || e.KeyChar == '-')
            {
                return; 
            }

            e.Handled = true;
        }
    }
}
