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
            roleUser = role;
        }
        private void FormMenu_Load(object sender, EventArgs e)
        {
            lblUser.Text = "Login sebagai: " + roleUser;

            if (roleUser == "Masyarakat")
            {
                btnKelolaLaporan.Visible = false;
                btnVerifikasiSetoran.Visible = false;

                btnLaporan.Visible = true;
                btnSetoran.Visible = true;
            }
            else if (roleUser == "Admin")
            {
                btnLaporan.Visible = false;
                btnSetoran.Visible = false;

                btnKelolaLaporan.Visible = true;
                btnVerifikasiSetoran.Visible = true;
            }
        }

       
    }
}
