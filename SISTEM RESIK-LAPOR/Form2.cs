using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;

namespace SISTEM_RESIK_LAPOR
{
    public partial class Form2 : Form
    {
        string connString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA; Initial Catalog=DBResikLaporADO; Integrated Security=True";
        SqlConnection conn;
        private BindingSource bindingSource = new BindingSource();
        private DataTable dtLaporan = new DataTable();
        byte[] fotoBytes;

        int idUserLogin;
        string roleUser;
        public Form2(int idUser, string role)
        {
            InitializeComponent();

            dataGridView1.CellClick += dataGridView1_CellClick;
            idUserLogin = idUser;
            roleUser = role.ToLower();

            this.Load+= Form2_Load;

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

                btnRekapData.Visible = false; // Sembunyikan tombol rekap

                cmbStatus.Enabled = false;
                btnImpDb.Enabled = false;
                button5.Enabled = false;// Nonaktifkan tombol import untuk masyarakat

            }
            else if (roleUser == "admin")
            {
                btnUpdate.Visible = true;
                btnTampil.Visible = true;
                btnDelete.Visible = true;

                btnRekapData.Visible = true; // Tampilkan tombol rekap

                txtDeskripsi.ReadOnly = true;
                txtLokasi.ReadOnly = true;
                txtFoto.ReadOnly = true;

                cmbStatus.Enabled = true;
                button1.Enabled = false; // Nonaktifkan tombol pilih foto untuk admin
            }

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadData();

            LoadData();
            HitungJumlah();
        }

        private void HitungJumlah()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CountLaporan", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter outputParam = new SqlParameter("@jumlah", SqlDbType.Int);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        lblTotalLaporan.Text = "Total Laporan: " + outputParam.Value.ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("SQL Error hitung total: " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Gagal menghitung total: " + ex.Message);
            }
        }
        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetLaporan", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            dtLaporan = new DataTable();
                            da.Fill(dtLaporan);
                            bindingSource.DataSource = dtLaporan;
                            dataGridView1.DataSource = bindingSource;
                            BindControls();
                        }
                    }
                }

                HitungJumlah();

                // Enable/disable button seperti pola modul
                dataGridView1.Enabled = true;
                btnInsert.Enabled = true;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Gagal HitungJumlahdata: " + ex.Message);
            }
        }
        private void BindControls()
        {
            txtDeskripsi.DataBindings.Clear();
            txtLokasi.DataBindings.Clear();
            cmbStatus.DataBindings.Clear();

            txtDeskripsi.DataBindings.Add(
                "Text",
                bindingSource,
                "deskripsi");

            txtLokasi.DataBindings.Add(
                "Text",
                bindingSource,
                "lokasi_maps");

            cmbStatus.DataBindings.Add(
                "Text",
                bindingSource,
                "status");
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
                    using (SqlCommand cmd =
                        new SqlCommand("sp_UpdateLaporan", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_laporan", idlap);
                        cmd.Parameters.AddWithValue("@deskripsi", txtDeskripsi.Text);
                        cmd.Parameters.AddWithValue("@foto", txtFoto.Text);
                        cmd.Parameters.AddWithValue("@lokasi_maps", txtLokasi.Text);
                        cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                        conn.Open();

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil diupdate");

                LoadData();
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("General Error : " + ex.Message);
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string deskripsi = txtDeskripsi.Text.Trim();
            string lokasi = txtLokasi.Text.Trim();

            if (roleUser != "masyarakat")
            {
                MessageBox.Show("Hanya masyarakat yang boleh membuat laporan!");
                return;
            }

            if (deskripsi == "" || lokasi == "")
            {
                MessageBox.Show("Deskripsi dan lokasi wajib diisi!");
                return;
            }

            if (fotoBytes == null)
            {
                MessageBox.Show("Pilih foto terlebih dahulu");
                return;
            }

            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertLaporan", conn, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_user", idUserLogin);
                cmd.Parameters.AddWithValue("@deskripsi", deskripsi);
                cmd.Parameters.Add("@foto", SqlDbType.VarBinary).Value = fotoBytes;
                cmd.Parameters.AddWithValue("@lokasi_maps", lokasi);
                cmd.ExecuteNonQuery();

                SqlCommand cmdLog = new SqlCommand(
                    @"INSERT INTO LogAktivitas (aktivitas, waktu)
              VALUES (@aktivitas, GETDATE())",
                    conn, trans);
                cmdLog.Parameters.AddWithValue("@aktivitas", "INSERT LAPORAN : " + deskripsi);
                cmdLog.ExecuteNonQuery();

                trans.Commit();

                MessageBox.Show("Laporan berhasil ditambahkan");

                LoadData();
                ClearForm();
            }
            catch (SqlException ex)
            {
                trans.Rollback();
                SimpanLog(ex.Message);
                MessageBox.Show("SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                SimpanLog(ex.Message);
                MessageBox.Show("General Error : " + ex.Message);
            }
            finally
            {
                conn.Close();
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
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteLaporan", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_laporan", idlap);

                        conn.Open();

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Data berhasil dihapus");
                        else
                            MessageBox.Show("Data tidak ditemukan");
                    }
                }

                LoadData();
            }
            catch (SqlException ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("SQL Error : " + ex.Message);
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("General Error : " + ex.Message);
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
            

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void HitungTotal_Click_1(object sender, EventArgs e)
        {
            HitungJumlah();
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
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SearchLaporan", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@keyword", txtSearch.Text);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal melakukan pencarian: "
                    + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (bindingSource != null && bindingSource.DataSource != null)
                {
                    string filterText = txtSearch.Text.Trim().Replace("'", "''");

                    if (string.IsNullOrEmpty(filterText))
                    {
                        bindingSource.RemoveFilter();
                    }
                    else
                    {
                        bindingSource.Filter = string.Format("deskripsi LIKE '%{0}%'", filterText);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Filter: " + ex.Message);
            }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = @"
                    IF OBJECT_ID('dbo.Laporan_Backup') IS NOT NULL
                    BEGIN
                    DELETE FROM dbo.Laporan;

                    SET IDENTITY_INSERT dbo.Laporan ON;

                    INSERT INTO dbo.Laporan 
                    (id_laporan, id_user, deskripsi, foto, lokasi_maps, status)
                    SELECT 
                    id_laporan, id_user, deskripsi, foto, lokasi_maps, status 
                    FROM dbo.Laporan_Backup;

                    SET IDENTITY_INSERT dbo.Laporan OFF;
                    END
                    ELSE
                    BEGIN
                        RAISERROR('Tabel backup tidak ditemukan!',16,1)
                    END";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data berhasil direset");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset gagal: " + ex.Message);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    // Nilai deskripsi DIHARDCODE di sini, supaya tidak terganggu DataBindings txtDeskripsi
                    string deskripsiBaru = "tes injection";

                    string query =
                        "UPDATE Laporan SET deskripsi='" +
                        deskripsiBaru +
                        "' WHERE id_laporan='" +
                        txtSearch.Text + "'";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Update berhasil");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtDeskripsi_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void txtLokasi_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLokasi_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtFoto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsControl(e.KeyChar) ||
                e.KeyChar == '.' || e.KeyChar == '_' || e.KeyChar == '-')
            {
                return;
            }
            e.Handled = true;
        }

        private void SimpanLog(string pesan)
        {
            try
            {
                using (SqlConnection connLog = new SqlConnection(connString))
                {
                    string query = @"INSERT INTO LogError (waktu, pesan_error)
                             VALUES (GETDATE(), @pesan)";

                    using (SqlCommand cmd = new SqlCommand(query, connLog))
                    {
                        cmd.Parameters.AddWithValue("@pesan", pesan);
                        connLog.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { /* log gagal, abaikan supaya tidak infinite loop */ }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Cetaklaporan frm = new Cetaklaporan();
            frm.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd =
        new OpenFileDialog();

            ofd.Filter =
                "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() ==
                DialogResult.OK)
            {
                fotoBytes =
                    File.ReadAllBytes(
                    ofd.FileName);

                fotoLaporan.Image =
                    Image.FromFile(
                    ofd.FileName);

                fotoLaporan.SizeMode =
                    PictureBoxSizeMode.StretchImage;

                txtFoto.Text =
                    Path.GetFileName(
                    ofd.FileName);
            }
        }

        private void ClearForm()
        {
            txtDeskripsi.Clear();
            txtLokasi.Clear();
            txtFoto.Clear();
            fotoLaporan.Image = null;
            fotoBytes = null;
            cmbStatus.SelectedIndex = 0;
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Excel Workbook|*.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    using (var stream = System.IO.File.Open(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(new ExcelDataReader.ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataReader.ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });

                            DataTable dt = result.Tables[0];
                            dataGridView1.DataSource = dt;
                            dataGridView1.Enabled = false;

                            // Disable semua button kecuali Import DB
                            btnImpDb.Enabled = true;
                            btnInsert.Enabled = false;
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
                            btnTampil.Enabled = false;
                        }
                    }
                }
            }
        }

        private void btnImpDb_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt =
                    (DataTable)dataGridView1.DataSource;

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "Tidak ada data untuk diimport.");

                    return;
                }

                int sukses = 0;

                using (SqlConnection conn =
                    new SqlConnection(connString))
                {
                    conn.Open();

                    foreach (DataRow row in dt.Rows)
                    {
                        string deskripsi =
                            row["deskripsi"]
                            .ToString()
                            .Trim();

                        string lokasi =
                            row["lokasi_maps"]
                            .ToString()
                            .Trim();

                        // Skip baris kosong
                        if (string.IsNullOrEmpty(deskripsi)
                            || string.IsNullOrEmpty(lokasi))
                        {
                            continue;
                        }

                        using (SqlCommand cmd =
                            new SqlCommand(
                            "sp_InsertLaporan",
                            conn))
                        {
                            cmd.CommandType =
                                CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue(
                                "@id_user",
                                idUserLogin);

                            cmd.Parameters.AddWithValue(
                                "@deskripsi",
                                deskripsi);

                            // Karena dari excel tidak ada foto
                            cmd.Parameters.Add(
                                "@foto",
                                SqlDbType.VarBinary)
                                .Value = DBNull.Value;

                            cmd.Parameters.AddWithValue(
                                "@lokasi_maps",
                                lokasi);

                            cmd.ExecuteNonQuery();
                        }

                        sukses++;
                    }
                }

                MessageBox.Show(
                    "Import berhasil : "
                    + sukses
                    + " data");

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal import : "
                    + ex.Message);
            }
        }
    }
}
