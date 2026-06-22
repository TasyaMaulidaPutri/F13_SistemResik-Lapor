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
    public partial class Cetaklaporan : Form
    {
        static string connectionString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA;Initial Catalog=DBResikLaporADO;User ID=sa;Password=PasswordSA";

        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtLaporan;

        public Cetaklaporan()
        {
            InitializeComponent();
            this.Load += Cetaklaporan_Load;

        }

        private void Cetaklaporan_Load(object sender, EventArgs e)
        {

            dtpTahun.Format = DateTimePickerFormat.Custom;
            dtpTahun.CustomFormat = "yyyy";
            dtpTahun.ShowUpDown = true;
            dtpTahun.MinDate = new DateTime(2000, 1, 1);
            dtpTahun.MaxDate = DateTime.Now;

            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("lapor");
            cmbStatus.Items.Add("proses");
            cmbStatus.Items.Add("bersih");
            cmbStatus.SelectedIndex = 0;

            btnCetak.Enabled = false;

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormCrystalReport frm = new FormCrystalReport(cmbStatus.Text, dtpTahun.Value);
            frm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("sp_ReportLaporan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inStatus", cmbStatus.Text);
                cmd.Parameters.AddWithValue("@inTahun", dtpTahun.Value.Year.ToString());

                da = new SqlDataAdapter(cmd);

                dtLaporan = new DataTable();
                da.Fill(dtLaporan);

                dataGridView1.DataSource = dtLaporan;

                if (dtLaporan.Rows.Count > 0)
                {
                    btnCetak.Enabled = true;
                }
                else
                {
                    btnCetak.Enabled = false;
                    MessageBox.Show("Data tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

       
    }
}
