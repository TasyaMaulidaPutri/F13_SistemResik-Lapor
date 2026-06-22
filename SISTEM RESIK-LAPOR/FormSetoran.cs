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
    public partial class FormSetoran : Form
    {
        static string connectionString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA;Initial Catalog=DBResikLaporADO;User ID=sa;Password=PasswordSA";

        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtSetoran;
        public FormSetoran()
        {
            InitializeComponent();
            this.Load += FormSetoran_Load;
        }

        private void FormSetoran_Load(object sender, EventArgs e)
        {
            dtpTahun.Format = DateTimePickerFormat.Custom;
            dtpTahun.CustomFormat = "yyyy";
            dtpTahun.ShowUpDown = true;
            dtpTahun.MinDate = new DateTime(2000, 1, 1);
            dtpTahun.MaxDate = DateTime.Now;

            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("pending");
            cmbStatus.Items.Add("verifikasi");
            cmbStatus.Items.Add("ditolak");
            cmbStatus.SelectedIndex = 0;

            btnCetak.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("sp_ReportSetoran", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inStatus", cmbStatus.Text);
                cmd.Parameters.AddWithValue("@inTahun", dtpTahun.Value.Year.ToString());

                da = new SqlDataAdapter(cmd);

                dtSetoran = new DataTable();
                da.Fill(dtSetoran);

                dataGridView1.DataSource = dtSetoran;

                if (dtSetoran.Rows.Count > 0)
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

        private void button2_Click(object sender, EventArgs e)
        {
            FormCRSetoran frm = new FormCRSetoran(cmbStatus.Text, dtpTahun.Value);
            frm.Show();
            this.Hide();
        }
    }
}
