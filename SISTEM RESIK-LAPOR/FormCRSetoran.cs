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
    public partial class FormCRSetoran : Form
    {
        string connectionString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA;Initial Catalog=DBResikLaporADO;Integrated Security=True";

        private string status;
        private DateTime tahun;

        public FormCRSetoran(string status, DateTime tahun)
        {
            InitializeComponent();
            this.status = status;
            this.tahun = tahun;
            this.Load += FormCRSetoran_Load;

        }


        private void FormCRSetoran_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_ReportSetoran", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@inStatus", status);
                    cmd.Parameters.AddWithValue("@inTahun", tahun.Year);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    CrystalReport2 rpt = new CrystalReport2();
                    rpt.SetDataSource(dt);

                    crystalReportViewer1.ReportSource = rpt;
                    crystalReportViewer1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load report setoran: " + ex.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
