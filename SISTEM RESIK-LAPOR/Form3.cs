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
        void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = (roleUser == "masyarakat")
                        ? "SELECT * FROM Setoran WHERE id_user=@id"
                        : "SELECT * FROM Setoran";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    if (roleUser == "masyarakat")
                        cmd.Parameters.AddWithValue("@id", idUserLogin);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString);

            LoadComboStatus();
            LoadData();

            if (roleUser == "masyarakat")
            {
                btnUpdate.Enabled = false;
                cmbStatus.Enabled = false;
                txtPoint.Enabled = false;
            }
        }
        void LoadComboStatus()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("pending");
            cmbStatus.Items.Add("verifikasi");
            cmbStatus.Items.Add("ditolak");

            cmbStatus.SelectedIndex = 0;
        }
        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (conn == null)
                {
                    conn = new SqlConnection(connString);
                }
                conn.Open();

                string query = @"INSERT INTO Setoran
                        (id_user, berat_kg, nama_jenis_sampah, poin_per_kg, total_poin_setoran, status_verifikasi)
                        VALUES
                        (@id, @berat, @jenis, 0, 0, 'pending')";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", idUserLogin);
                cmd.Parameters.AddWithValue("@berat", Convert.ToDouble(txtBerat.Text));
                cmd.Parameters.AddWithValue("@jenis", txtJenis.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Setoran berhasil dikirim");

                conn.Close();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
    
}
