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
using System.Text.RegularExpressions;

namespace SISTEM_RESIK_LAPOR
{
    public partial class Form3 : Form
    {
        string connString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA; Initial Catalog=DBResikLaporADO; Integrated Security=True";
        SqlConnection conn;

        private BindingSource bindingSource = new BindingSource();
        private DataTable dtSetoran = new DataTable();

        int idUserLogin;
        string roleUser;
        int selectedIdSetoran;
        public Form3(int idUser, string role)
        {
            InitializeComponent();
            idUserLogin = idUser;
            roleUser = role.ToLower();

            this.Load += Form3_Load;

            MessageBox.Show("Role: " + roleUser);

            if (roleUser.ToLower() == "masyarakat")
            {
                btnUpdate.Visible = false;
                btnTampilkan.Visible = false;
                cmbStatus.Enabled = false;
            }
        }
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd;

                    if (roleUser == "masyarakat")
                    {
                        string query = "SELECT * FROM Setoran WHERE id_user=@id";

                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", idUserLogin);
                    }
                    else
                    {
                        cmd = new SqlCommand("sp_GetSetoran", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        dtSetoran = new DataTable();

                        da.Fill(dtSetoran);

                        bindingSource.DataSource = dtSetoran;

                        dataGridView1.DataSource = bindingSource;

                        BindControls();
                    }
                }

                HitungJumlah();
                HitungPoinMasyarakat();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal load data: "
                    + ex.Message);
            }
        }
        private void BindControls()
        {
            txtBerat.DataBindings.Clear();
            txtJenis.DataBindings.Clear();
            txtPoint.DataBindings.Clear();
            cmbStatus.DataBindings.Clear();

            txtBerat.DataBindings.Add("Text", bindingSource, "berat_kg");
            txtJenis.DataBindings.Add("Text", bindingSource, "nama_jenis_sampah");
            txtPoint.DataBindings.Add("Text", bindingSource, "poin_per_kg");
            cmbStatus.DataBindings.Add("Text", bindingSource,"status_verifikasi");
        }
        private void HitungJumlah()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CountSetoran", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter outputParam = new SqlParameter("@jumlah", SqlDbType.Int);

                        outputParam.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(outputParam);

                        conn.Open();

                        cmd.ExecuteNonQuery();

                        lblTotalSetoran.Text = "Total Setoran: " + outputParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menghitung total: " + ex.Message);
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString);

            LoadComboStatus();

            btnInsert.Visible = false;
            btnUpdate.Visible = false;

            if (roleUser == "masyarakat")
            {
                btnInsert.Visible = true;
                btnUpdate.Enabled = false;
                cmbStatus.Enabled = false;
                txtPoint.Enabled = false;
            }
            else if (roleUser == "admin")
            {
                btnInsert.Visible = false;
                btnUpdate.Visible = true;
                txtJenis.ReadOnly = true;
                txtBerat.ReadOnly = true;
                txtPoint.ReadOnly = false;
                cmbStatus.Enabled = true;
            }

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

           
            dataGridView1.ScrollBars = ScrollBars.Both;

            LoadData();
            HitungJumlah();
            HitungPoinMasyarakat();
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
                string jenis = txtJenis.Text.Trim();

                string beratText = txtBerat.Text.Trim();

                if (string.IsNullOrEmpty(jenis))
                {
                    MessageBox.Show("Pemberitahuan: Kolom Jenis Sampah belum diisi!",
                        "Data Belum Lengkap", 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtJenis.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(beratText))
                {
                    MessageBox.Show(
                        "Pemberitahuan: Kolom Berat belum diisi!",
                        "Data Belum Lengkap",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtBerat.Focus();

                    return;
                }

                if (!double.TryParse(
                    beratText,
                    out double berat))
                {
                    MessageBox.Show(
                        "Berat harus berupa angka!",
                        "Validasi Gagal",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                if (berat <= 0)
                {
                    MessageBox.Show(
                        "Berat harus lebih dari 0!",
                        "Validasi Gagal",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                if (!IsValidText(jenis))
                {
                    MessageBox.Show(
                        "Jenis hanya boleh huruf!",
                        "Validasi Gagal",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertSetoran", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_user", idUserLogin);
                        cmd.Parameters.AddWithValue("@berat_kg", Convert.ToInt32(berat));
                        cmd.Parameters.AddWithValue("@nama_jenis_sampah", jenis);
                        cmd.Parameters.AddWithValue("@poin_per_kg", 0);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Setoran berhasil dikirim",
                    "Sukses",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadData();

                txtJenis.Clear();
                txtBerat.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Kesalahan Sistem",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateSetoran", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_setoran", selectedIdSetoran);
                        cmd.Parameters.AddWithValue("@berat_kg", Convert.ToInt32 (txtBerat.Text));
                        cmd.Parameters.AddWithValue("@nama_jenis_sampah", txtJenis.Text);
                        cmd.Parameters.AddWithValue("@poin_per_kg", Convert.ToInt32 (txtPoint.Text));
                        cmd.Parameters.AddWithValue("@status_verifikasi", cmbStatus.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
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
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show(
                        "Pilih data dulu!",
                        "Peringatan",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                int idSetoran =
                    Convert.ToInt32(
                        dataGridView1.CurrentRow
                        .Cells["id_setoran"].Value);

                DialogResult result =
                    MessageBox.Show(
                        "Yakin ingin menghapus data?",
                        "Konfirmasi",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteSetoran", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_setoran", idSetoran);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Data berhasil dihapus!",
                    "Sukses",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadData();
                HitungJumlah();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error saat menghapus: "
                    + ex.Message,
                    "Kesalahan Sistem",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

                if (row.Cells["id_setoran"].Value != null && row.Cells["id_setoran"].Value != DBNull.Value)
                {
                    selectedIdSetoran = Convert.ToInt32(row.Cells["id_setoran"].Value);
                }
                else
                {
                    selectedIdSetoran = 0;
                }

                txtBerat.Text = row.Cells["berat_kg"].Value?.ToString() ?? "0";
                txtJenis.Text = row.Cells["nama_jenis_sampah"].Value?.ToString() ?? "";
                txtPoint.Text = row.Cells["poin_per_kg"].Value?.ToString() ?? "0";
                cmbStatus.Text = row.Cells["status_verifikasi"].Value?.ToString() ?? "pending";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int total = HitungTotalSetoran();
            lblTotalSetoran.Text = "Total Setoran: " + total;
        }

        private bool IsValidText(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z\s]+$");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load_1(object sender, EventArgs e)
        {
            this.setoranTableAdapter.Fill(this.dBResikLaporADODataSet3.Setoran);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (dt != null)
            {
                dt.DefaultView.RowFilter = string.Format("nama_jenis_sampah LIKE '%{0}%'", txtSearch.Text);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SearchSetoran", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@keyword", txtSearch.Text);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        dataGridView1.DataSource = dt;

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show(
                                "Data tidak ditemukan!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Terjadi kesalahan saat mencari: "
                    + ex.Message);
            }
        }

        private void txtBerat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                return; 
            }

            e.Handled = true;
        }

        private void txtPoint_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                return; 
            }

            e.Handled = true;
        }

        private void HitungPoinMasyarakat()
        {
            if (roleUser != "masyarakat")
            {
                lblTotalPoinKeseluruhan.Text = "Total Poin: (Hanya untuk Masyarakat)";
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT ISNULL(SUM(total_poin_setoran), 0) FROM Setoran WHERE id_user = @id_user AND status_verifikasi = 'verifikasi'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_user", idUserLogin);

                        conn.Open();

                        int totalPoin = Convert.ToInt32(cmd.ExecuteScalar());

 
                        lblTotalPoinKeseluruhan.Text = "Total Poin Keseluruhan: " + totalPoin + " Poin";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat total poin keseluruhan: " + ex.Message, "Kesalahan Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}


