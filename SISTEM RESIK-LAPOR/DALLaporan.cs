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
    internal class DALLaporan
    {
        public static string GetConnectionString()
        {
            string connectionString =
                $"Data Source={GetLoacalIPAddress()}\\TASYAMAULIDA;Initial Catalog=DBResikLaporADO;User ID=sa;Password=PasswordSA;";
            return connectionString;
        }

        SqlConnection conn = new SqlConnection(GetConnectionString());

        SqlDataAdapter da;
        DataTable dtLaporan;

        // =====================================
        // COUNT LAPORAN
        // =====================================
        public int CountLaporan()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_CountLaporan", conn);

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
        // GET ALL LAPORAN
        // =====================================
        public DataTable GetLaporan()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_GetLaporan", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            da = new SqlDataAdapter(cmd);

            dtLaporan = new DataTable();

            da.Fill(dtLaporan);

            return dtLaporan;
        }

        // =====================================
        // GET LAPORAN BY ID
        // =====================================
        public DataTable GetLaporanById(int idLaporan)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_GetLaporanById", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@id_laporan",
                idLaporan);

            da = new SqlDataAdapter(cmd);

            dtLaporan = new DataTable();

            da.Fill(dtLaporan);

            return dtLaporan;
        }

        // =====================================
        // INSERT LAPORAN
        // =====================================
        public void InsertLaporan(
            int idUser,
            string deskripsi,
            byte[] foto,
            string lokasiMaps)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlTransaction trans =
                conn.BeginTransaction();

            try
            {
                SqlCommand cmd =
                    new SqlCommand("sp_InsertLaporan", conn);

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Transaction =
                    trans;

                cmd.Parameters.AddWithValue(
                    "@pIdUser",
                    idUser);

                cmd.Parameters.AddWithValue(
                    "@pDeskripsi",
                    deskripsi);

                cmd.Parameters.AddWithValue(
                    "@pFoto",
                    foto);

                cmd.Parameters.AddWithValue(
                    "@pLokasiMaps",
                    lokasiMaps);

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
        // UPDATE LAPORAN
        // =====================================
        public void UpdateLaporan(
            int idLaporan,
            string deskripsi,
            byte[] foto,
            string lokasiMaps,
            string status)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_UpdateLaporan", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@pIdLaporan",
                idLaporan);

            cmd.Parameters.AddWithValue(
                "@pDeskripsi",
                deskripsi);

            cmd.Parameters.AddWithValue(
                "@pFoto",
                foto);

            cmd.Parameters.AddWithValue(
                "@pLokasiMaps",
                lokasiMaps);

            cmd.Parameters.AddWithValue(
                "@pStatus",
                status);

            cmd.ExecuteNonQuery();
        }

        // =====================================
        // DELETE LAPORAN
        // =====================================
        public void DeleteLaporan(int idLaporan)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand("sp_DeleteLaporan", conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@pIdLaporan",
                idLaporan);

            cmd.ExecuteNonQuery();
        }

        // =====================================
        // SEARCH LAPORAN
        // =====================================
        public DataTable CariLaporan(int idLaporan)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn =
                new SqlConnection(GetConnectionString()))
            {
                SqlCommand cmd =
                    new SqlCommand(
                        "sp_GetLaporanById",
                        conn);

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@id_laporan",
                    idLaporan);

                SqlDataAdapter da =
                    new SqlDataAdapter(cmd);

                da.Fill(dt);
            }

            return dt;
        }

        // =====================================
        // DASHBOARD
        // =====================================
        public DataTable getAllDataChart(int tahun)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection conn = new SqlConnection(GetConnectionString());
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GrafikLaporanStatus", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pTahun", tahun);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return dt;
        }

        public DataTable getDataChartByTahun(DateTime tanggalLapor)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd =
                new SqlCommand(
                    "sp_DashBoardByTahun",
                    conn);

            cmd.CommandType =
                CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue(
                "@pTahun",
                tanggalLapor.Year.ToString());

            da = new SqlDataAdapter(cmd);

            dtLaporan = new DataTable();

            da.Fill(dtLaporan);

            return dtLaporan;
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
