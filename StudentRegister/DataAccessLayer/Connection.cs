using System.Data;
using Microsoft.Data.SqlClient;

namespace DataAccessLayer
{
    public class Connection
    {
        private static readonly string connectionString = @"Data Source=localhost;Initial Catalog=SchoolReg;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
