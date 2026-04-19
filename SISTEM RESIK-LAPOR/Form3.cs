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
    public partial class Form3 : Form
    {
        string connString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA; Initial Catalog=DBResikLaporADO; Integrated Security=True";
        SqlConnection conn;
        int idUserLogin;
        string roleUser;
        int selectedIdSetoran;
        public Form3(int idUser, string role)
        {
            InitializeComponent();
            idUserLogin = idUser;
            roleUser = role?.ToLower() ?? "";

            MessageBox.Show("Role: " + roleUser);

            if (roleUser == "masyarakat")
            {
                btnUpdate.Visible = false;
                cmbStatus.Enabled = false;
                txtPoint.Enabled = false;
            }
        }
        
    }
    
}
