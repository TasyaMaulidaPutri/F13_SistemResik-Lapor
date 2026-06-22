using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SISTEM_RESIK_LAPOR
{
    public partial class Dashboard : Form
    {
        DALLaporan dbLaporan = new DALLaporan();
        DALSetoran dbSetoran = new DALSetoran();

        DataTable dtLaporan;
        DataTable dtSetoran;

        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            // DTP Tahun
            dtpTahun.Format = DateTimePickerFormat.Custom;
            dtpTahun.CustomFormat = "yyyy";
            dtpTahun.ShowUpDown = true;

            // Combo Status
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("Semua");
            cmbStatus.Items.Add("lapor");
            cmbStatus.Items.Add("proses");
            cmbStatus.Items.Add("selesai");

            cmbStatus.SelectedIndex = 0;

            LoadChartLaporan();
            LoadChartSetoran();
        }
        private void LoadChartLaporan()
        {
            chartLaporan.Series.Clear();
            chartLaporan.ChartAreas.Clear();
            chartLaporan.ChartAreas.Add(new ChartArea());

            try
            {
                int tahun = dtpTahun.Value.Year;
                dtLaporan = dbLaporan.getAllDataChart(tahun);

                Series s = new Series("Laporan");
                s.ChartType = SeriesChartType.Bar;

                foreach (DataRow row in dtLaporan.Rows)
                {
                    s.Points.AddXY(
                        row["status"].ToString(),
                        Convert.ToInt32(row["JumlahLaporan"])
                    );
                }

                chartLaporan.Series.Add(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadChartSetoran()
        {
            chartSetoran.Series.Clear();
            chartSetoran.ChartAreas.Clear();
            chartSetoran.ChartAreas.Add(new ChartArea());

            try
            {
                int tahun = dtpTahun.Value.Year;
                dtSetoran = dbSetoran.getAllDataChart(tahun);

                Series s = new Series("Total Poin");
                s.ChartType = SeriesChartType.Column;

                foreach (DataRow row in dtSetoran.Rows)
                {
                    s.Points.AddXY(
                        row["Bulan"].ToString(),
                        Convert.ToInt32(row["TotalPoin"])
                    );
                }

                chartSetoran.Series.Add(s);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadChartLaporan();
            LoadChartSetoran();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cmbStatus.SelectedIndex = 0;
            dtpTahun.Value = DateTime.Now;
            LoadChartLaporan();
            LoadChartSetoran();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2(0, "admin");
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3(0, "admin");
            frm.Show();
        }
    }
}
