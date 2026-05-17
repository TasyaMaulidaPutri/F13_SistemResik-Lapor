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

           
        }

        private void textAlamat_TextChanged(object sender, EventArgs e)
        {

        }

        private void textAlamat_KeyPress(object sender, KeyPressEventArgs e)
        {
  
           
        }
    }
}
