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
            HitungJumlah();
        }

        private void HitungJumlah()
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CountLaporan", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter outputParam = new SqlParameter("@jumlah", SqlDbType.Int);

                        outputParam.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(outputParam);

                        conn.Open();

                        cmd.ExecuteNonQuery();

                        lblTotalLaporan.Text =
                            "Total Laporan: "
                            + outputParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menghitung total: "
                    + ex.Message);
            }
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
            
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            
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
                    conn.Open();

                    string query = "UPDATE Laporan SET deskripsi='HACKED' WHERE deskripsi='" + txtDeskripsi.Text + "'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int result = cmd.ExecuteNonQuery();
                        MessageBox.Show(result + " baris data berhasil dimanipulasi lewat SQL Injection!");
                    }
                }
                LoadData(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Injeksi: " + ex.Message);
            }
        }

        private void txtDeskripsi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar))
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
    }
}
