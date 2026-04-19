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
    public partial class Form2 : Form
    {
        string connString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA; Initial Catalog=DBResikLaporADO; Integrated Security=True";
        SqlConnection conn;

        int idUserLogin;
        string roleUser;
        public Form2(int idUser, string role)
        {
            InitializeComponent();
            idUserLogin = idUser;
            roleUser = role.ToLower();

            MessageBox.Show("Role: " + roleUser);

            if (roleUser.ToLower() == "masyarakat")
            {
                btnUpdate.Visible = false;
                cmbStatus.Enabled = false;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString);

           
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("lapor");
            cmbStatus.Items.Add("proses");
            cmbStatus.Items.Add("bersih");
            cmbStatus.SelectedIndex = 0;


            if (roleUser == "masyarakat")
            {
                btnUpdate.Visible = false;
                btnTampil.Visible = false;
                cmbStatus.Enabled = false;
            }

            LoadData();
        }

        void LoadData(string keyword = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query;

                    if (roleUser == "masyarakat")
                    {
                        query = "SELECT * FROM Laporan WHERE id_user=@id";
                    }
                    else
                    {
                        query = "SELECT * FROM Laporan";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (roleUser == "masyarakat")
                            cmd.Parameters.AddWithValue("@id", idUserLogin);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
    }
}
