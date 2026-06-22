using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SISTEM_RESIK_LAPOR
{
    internal class DALSetoran
    {
        public static string GetConnectionString()
        {
            string connectionString =
                $"Data Source={GetLoacalIPAddress()}\\TASYAMAULIDA;Initial Catalog=DBResikLaporADO;User ID=sa;Password=PasswordSA;";
            return connectionString;
        }

        SqlConnection conn = new SqlConnection(GetConnectionString());

        SqlDataAdapter da;
        DataTable dtSetoran;

        // =====================================
        // COUNT SETORAN
        // =====================================
        public int CountSetoran()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_CountSetoran", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            SqlParameter outputParam =
                new SqlParameter("@Total", SqlDbType.Int);

            outputParam.Direction =
                ParameterDirection.Output;

            cmd.Parameters.Add(outputParam);

            cmd.ExecuteNonQuery();

            return Convert.ToInt32(outputParam.Value);
        }

        // =====================================
        // GET ALL SETORAN
        // =====================================
        public DataTable GetSetoran()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_GetSetoran", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);

            dtSetoran = new DataTable();

            da.Fill(dtSetoran);

            return dtSetoran;
        }

        // =====================================
        // GET SETORAN BY ID
        // =====================================
        public DataTable GetSetoranById(int idSetoran)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_GetSetoranById", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@id_setoran",
                idSetoran);

            da = new SqlDataAdapter(cmd);

            dtSetoran = new DataTable();

            da.Fill(dtSetoran);

            return dtSetoran;
        }

        // =====================================
        // INSERT SETORAN
        // =====================================
        public void InsertSetoran(
      int idUser,
      int beratKg,
      string namaJenisSampah,
      int poinPerKg)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                SqlCommand cmd =
                    new SqlCommand("sp_InsertSetoran", conn);

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Transaction =
                    trans;

                cmd.Parameters.AddWithValue(
                    "@id_user",
                    idUser);

                cmd.Parameters.AddWithValue(
                    "@berat_kg",
                    beratKg);

                cmd.Parameters.AddWithValue(
                    "@nama_jenis_sampah",
                    namaJenisSampah);

                cmd.Parameters.AddWithValue(
                    "@poin_per_kg",
                    poinPerKg);

                cmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        // =====================================
        // UPDATE SETORAN
        // =====================================
        public void UpdateSetoran(
    int idSetoran,
    int beratKg,
    string namaJenisSampah,
    int poinPerKg,
    string statusVerifikasi)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_UpdateSetoran", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@id_setoran",
                idSetoran);

            cmd.Parameters.AddWithValue(
                "@berat_kg",
                beratKg);

            cmd.Parameters.AddWithValue(
                "@nama_jenis_sampah",
                namaJenisSampah);

            cmd.Parameters.AddWithValue(
                "@poin_per_kg",
                poinPerKg);

            cmd.Parameters.AddWithValue(
                "@status_verifikasi",
                statusVerifikasi);

            cmd.ExecuteNonQuery();
        }
        // =====================================
        // DELETE SETORAN
        // =====================================
        public void DeleteSetoran(int idSetoran)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_DeleteSetoran", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@id_setoran",
                idSetoran);

            cmd.ExecuteNonQuery();
        }

        // =====================================
        // SEARCH SETORAN
        // =====================================
        public DataTable CariSetoran(int idSetoran)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn =
                new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd =
                    new SqlCommand(
                        "sp_GetSetoranById",
                        conn);

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@id_setoran",
                    idSetoran);

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                da.Fill(dt);
            }

            return dt;
        }

        // =====================================
        // DASHBOARD
        // =====================================
        // DALSetoran.cs
        public DataTable getAllDataChart(int tahun)
        {
            DataTable dt = new DataTable();
            try
            {
               
        SqlConnection conn = new SqlConnection(GetConnectionString());
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DashboardSetoran", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pTahun", tahun);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return dt;
        }
        public DataTable getDataChartByTahun(
            DateTime tanggalSetor)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand(
                    "sp_DashboardSetoranByTahun",
                    conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@pTahun",
                tanggalSetor.Year);

            da = new SqlDataAdapter(cmd);

            dtSetoran = new DataTable();

            da.Fill(dtSetoran);

            return dtSetoran;
        }

        // =====================================
        // GET LOCAL IP
        // =====================================
        public static string GetLoacalIPAddress()
        {
            string localIP = string.Empty;

            try
            {
                var host =
                    System.Net.Dns.GetHostEntry(
                        System.Net.Dns.GetHostName());

                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily ==
                        System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error getting local IP address: "
                    + ex.Message);
            }

            return localIP;
        }
    }
}

