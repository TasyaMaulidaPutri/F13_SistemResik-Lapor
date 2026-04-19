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
    public partial class Form2 : Form
    {
        string connString = "Data Source=LAPTOP-7BCU6RBN\\TASYAMAULIDA; Initial Catalog=DBResikLaporADO; Integrated Security=True";
        SqlConnection conn;

        int idUserLogin;
        string roleUser;
        public Form2(int idUser, string role)
        {
            InitializeComponent();
            idUserLogin = idUser;
            roleUser = role.ToLower();

            MessageBox.Show("Role: " + roleUser);

            if (roleUser.ToLower() == "masyarakat")
            {
                btnUpdate.Visible = false;
                cmbStatus.Enabled = false;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connString);

           
            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("lapor");
            cmbStatus.Items.Add("proses");
            cmbStatus.Items.Add("bersih");
            cmbStatus.SelectedIndex = 0;


            if (roleUser == "masyarakat")
            {
                btnUpdate.Visible = false;
                btnTampil.Visible = false;
                cmbStatus.Enabled = false;
            }

            LoadData();
        }

        void LoadData(string keyword = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query;

                    if (roleUser == "masyarakat")
                    {
                        query = "SELECT * FROM Laporan WHERE id_user=@id";
                    }
                    else
                    {
                        query = "SELECT * FROM Laporan";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (roleUser == "masyarakat")
                            cmd.Parameters.AddWithValue("@id", idUserLogin);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                if (txtDeskripsi.Text == "" || txtLokasi.Text == "")
                {
                    MessageBox.Show("Deskripsi dan Lokasi wajib diisi!");
                    return;
                }
                if (roleUser != "masyarakat")
                {
                    MessageBox.Show("Hanya masyarakat yang boleh membuat laporan!");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Laporan 
                            (id_user, deskripsi, foto, lokasi_maps, status)
                            VALUES 
                            (@id, @desk, @foto, @lokasi, @status)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idUserLogin); 
                        cmd.Parameters.AddWithValue("@desk", txtDeskripsi.Text);
                        cmd.Parameters.AddWithValue("@foto", txtFoto.Text);
                        cmd.Parameters.AddWithValue("@lokasi", txtLokasi.Text);

                        string status = (roleUser == "admin") ? cmbStatus.Text : "lapor";
                        cmd.Parameters.AddWithValue("@status", status);

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

       
    }
}
