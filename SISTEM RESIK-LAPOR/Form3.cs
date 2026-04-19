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
            lblTotalSetoran.Text = "Total Laporan: " + HitungTotalSetoran();

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

        int HitungTotalSetoran()
        {
            int total = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query;

                    if (roleUser == "masyarakat")
                    {
                        query = "SELECT COUNT(*) FROM Setoran WHERE id_user=@id";
                    }
                    else
                    {
                        query = "SELECT COUNT(*) FROM Setoran";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (roleUser == "masyarakat")
                            cmd.Parameters.AddWithValue("@id", idUserLogin);

                        total = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return total;
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (roleUser != "admin")
            {
                MessageBox.Show("Hanya admin yang boleh update!");
                return;
            }

            if (selectedIdSetoran == 0)
            {
                MessageBox.Show("Pilih data dulu!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    int poinPerKg = Convert.ToInt32(txtPoint.Text);
                    double berat = Convert.ToDouble(txtBerat.Text);
                    int total = (int)(poinPerKg * berat);

                    string query = @"UPDATE Setoran
                            SET poin_per_kg=@ppk,
                                total_poin_setoran=@total,
                                status_verifikasi=@status
                            WHERE id_setoran=@id";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@ppk", poinPerKg);
                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@id", selectedIdSetoran);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Berhasil diverifikasi!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Pilih data dulu!");
                return;
            }

            int idSetoran = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_setoran"].Value);

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query;
                    SqlCommand cmd;

                    if (roleUser.Trim().ToLower() == "admin")
                    {
                        query = "DELETE FROM Setoran WHERE id_setoran=@id";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", idSetoran);
                    }
                    else
                    {
                        query = "DELETE FROM Setoran WHERE id_setoran=@id AND id_user=@user";
                        cmd = new SqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@id", idSetoran);
                        cmd.Parameters.AddWithValue("@user", idUserLogin);
                    }

                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                        MessageBox.Show("Data tidak ditemukan atau bukan milik user ini!");
                    else
                        MessageBox.Show("Berhasil dihapus");
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTampilkan_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                selectedIdSetoran = Convert.ToInt32(row.Cells["id_setoran"].Value);

                txtBerat.Text = row.Cells["berat_kg"].Value.ToString();
                txtJenis.Text = row.Cells["nama_jenis_sampah"].Value.ToString();
                txtPoint.Text = row.Cells["poin_per_kg"].Value.ToString();
                cmbStatus.Text = row.Cells["status_verifikasi"].Value.ToString();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int total = HitungTotalSetoran();
            lblTotalSetoran.Text = "Total Setoran: " + total;
        }
    }
    
}
