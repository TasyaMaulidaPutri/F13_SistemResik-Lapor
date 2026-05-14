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
    public partial class Form2 : Form
    {
        string connString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA; Initial Catalog=DBResikLaporADO; Integrated Security=True";
        SqlConnection conn;
        private BindingSource bindingSource = new BindingSource();
        private DataTable dtLaporan = new DataTable();

        int idUserLogin;
        string roleUser;
        public Form2(int idUser, string role)
        {
            InitializeComponent();

            dataGridView1.CellClick += dataGridView1_CellClick;
            idUserLogin = idUser;
            roleUser = role.ToLower();

            this.Load += Form2_Load;

            MessageBox.Show("Role: " + roleUser);

            if (roleUser.ToLower() == "masyarakat")
            {
                btnUpdate.Visible = false;
                btnTampil.Visible = false;
                cmbStatus.Enabled = false;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString);

            btnInsert.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnTampil.Visible = false;

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("lapor");
            cmbStatus.Items.Add("proses");
            cmbStatus.Items.Add("bersih");
            cmbStatus.SelectedIndex = 0;

            if (roleUser == "masyarakat")
            {
                btnInsert.Visible = true;
                btnDelete.Visible = true;

                cmbStatus.Enabled = false;
            }
            else if (roleUser == "admin")
            {
                btnUpdate.Visible = true;
                btnTampil.Visible = true;
                btnDelete.Visible = true;

                txtDeskripsi.ReadOnly = true;
                txtLokasi.ReadOnly = true;
                txtFoto.ReadOnly = true;

                cmbStatus.Enabled = true;
            }

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadData();

            LoadData();
            lblTotalLaporan.Text = "Total Laporan: " + HitungJumlahLaporan();
        }

        int HitungJumlahLaporan()
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
                        query = "SELECT COUNT(*) FROM Laporan WHERE id_user=@id";
                    }
                    else
                    {
                        query = "SELECT COUNT(*) FROM Laporan";
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

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT * FROM vwLaporanPublic";

                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        dtLaporan = new DataTable();
                        da.Fill(dtLaporan);
                        bindingSource.DataSource = dtLaporan;
                        dataGridView1.DataSource = bindingSource;
                        BindControls();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        private void BindControls()
        {
            txtDeskripsi.DataBindings.Clear();
            txtFoto.DataBindings.Clear();
            txtLokasi.DataBindings.Clear();
            cmbStatus.DataBindings.Clear();

            txtDeskripsi.DataBindings.Add("Text", bindingSource, "deskripsi");
            txtFoto.DataBindings.Add("Text", bindingSource, "foto");
            txtLokasi.DataBindings.Add("Text", bindingSource, "lokasi_maps");
            cmbStatus.DataBindings.Add("Text", bindingSource, "status");
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (roleUser != "admin")
                {
                    MessageBox.Show("Hanya admin yang bisa update!");
                    return;
                }

                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Pilih data dulu!");
                    return;
                }

                int idlap = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_laporan"].Value);

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = @"UPDATE Laporan 
                                SET deskripsi=@desk, 
                                foto=@foto, 
                                lokasi_maps=@lokasi, 
                                status=@status 
                                WHERE id_laporan=@idlap";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@desk", txtDeskripsi.Text);
                        cmd.Parameters.AddWithValue("@foto", txtFoto.Text);
                        cmd.Parameters.AddWithValue("@lokasi", txtLokasi.Text);
                        cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                        cmd.Parameters.AddWithValue("@idlap", idlap);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil diupdate");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string deskripsi = txtDeskripsi.Text.Trim();
                string lokasi = txtLokasi.Text.Trim();
                string foto = txtFoto.Text.Trim();

                if (roleUser != "masyarakat")
                {
                    MessageBox.Show("Hanya masyarakat yang boleh membuat laporan!");
                    return;
                }

                if (deskripsi == "" || lokasi == "")
                {
                    MessageBox.Show("Deskripsi dan Lokasi wajib diisi!");
                    return;
                }

                if (!IsValidText(deskripsi) || !IsValidText(lokasi))
                {
                    MessageBox.Show("Deskripsi dan lokasi tidak boleh mengandung simbol!");
                    return;
                }

                if (foto != "" && !IsValidText(foto))
                {
                    MessageBox.Show("Nama file foto tidak boleh mengandung simbol!");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Laporan 
                (id_user, deskripsi, foto, lokasi_maps, status)
                VALUES (@id, @desk, @foto, @lokasi, 'lapor')";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idUserLogin);
                        cmd.Parameters.AddWithValue("@desk", deskripsi);
                        cmd.Parameters.AddWithValue("@foto", foto);
                        cmd.Parameters.AddWithValue("@lokasi", lokasi);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Laporan berhasil ditambahkan");
                LoadData();

                txtDeskripsi.Clear();
                txtFoto.Clear();
                txtLokasi.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Pilih data dulu!");
                    return;
                }

                int idlap = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_laporan"].Value);

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query;

                    if (roleUser == "admin")
                    {
                        query = "DELETE FROM Laporan WHERE id_laporan=@id";
                    }
                    else
                    {
                        query = "DELETE FROM Laporan WHERE id_laporan=@id AND id_user=@user";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idlap);

                        if (roleUser == "masyarakat")
                        {
                            cmd.Parameters.AddWithValue("@user", idUserLogin);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil dihapus");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtDeskripsi.Text = row.Cells[2].Value?.ToString() ?? "";
                txtFoto.Text = row.Cells[3].Value?.ToString() ?? "";
                txtLokasi.Text = row.Cells[4].Value?.ToString() ?? "";
                cmbStatus.Text = row.Cells[5].Value?.ToString() ?? "";
            }
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
            this.laporanTableAdapter.Fill(this.dBResikLaporADODataSet2.Laporan);
            
            this.laporanTableAdapter.Fill(this.dBResikLaporADODataSet2.Laporan);

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int jumlah = HitungJumlahLaporan();
            lblTotalLaporan.Text = "Total Laporan: " + jumlah;
        }

        private bool IsValidText(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z0-9\s]+$");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dataGridView1.DataSource as DataTable;

                if (dt != null)
                {
                    string filterText = txtSearch.Text.Trim();

                    if (string.IsNullOrEmpty(filterText))
                    {
                        dt.DefaultView.RowFilter = string.Empty;
                    }
                    else
                    {
                        dt.DefaultView.RowFilter = string.Format(
                            "deskripsi LIKE '%{0}%' OR " +
                            "lokasi_maps LIKE '%{0}%' OR " +
                            "Convert(id_laporan, 'System.String') LIKE '%{0}%'",
                            filterText);
                    }

                    if (dt.DefaultView.Count == 0)
                    {
                        MessageBox.Show("Data tidak ditemukan untuk kata kunci: " + filterText);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal melakukan pencarian: " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (dt != null)
            {
                dt.DefaultView.RowFilter = string.Format("deskripsi LIKE '%{0}%'", txtSearch.Text);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }
    }
}
