using Microsoft.Data.SqlClient;

namespace KetNoiQUANLY
{
    public static class DB
    {
        public static string ConnectionString =
            "Data Source=1112024BANG\\SQLEXPRESS;Initial Catalog=QuanLyBanHang;" +
            "Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}