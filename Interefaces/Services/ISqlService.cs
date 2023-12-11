using Microsoft.Data.SqlClient;
using System.Data;

namespace almondcove.Interefaces.Services
{
    public interface ISqlService
    {
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null, SqlConnection customConnection = null, SqlTransaction transaction = null);
        public SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null, SqlConnection customConnection = null, SqlTransaction transaction = null);
        public object ExecuteScalar(string query, SqlParameter[] parameters = null);
        public void Dispose();
    }
}
