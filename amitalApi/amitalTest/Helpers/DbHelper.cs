using amitalTest.Extensions;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace amitalTest.Helpers
{
    public class DbHelper
    {
        #region convenience functions
        public static void CheckAndOpenConnection(IDbConnection dbConnection)
        {
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
        }
        public static void CheckAndCloseConnection(IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
            }
        }
        // ExecuteReader is a method used to execute a SQL query and retrieve the results as a SqlDataReader object. This object can be used to iterate through the rows of the results and access the data. It is primarily used for reading data and is optimized for forward-only, read-only access to data.
        public static async Task<List<T>> ExecuteReaderAsync<T>(SqlConnection connection, string query, SqlParameter[] parameters = null)
        where T : new()
        {
            CheckAndOpenConnection(connection);
            var command = CreateCommand(connection, query, parameters);
            var reader = await command.ExecuteReaderAsync();
            var list = reader.MapToList<T>();
            CheckAndCloseConnection(connection);

            return list;
        }

        // It is important to note that ExecuteScalar can only return one value
        public static async Task<object> ExecuteScalarAsync(SqlConnection connection, string query, SqlParameter[] parameters = null)
        {
            CheckAndOpenConnection(connection);
            var command = CreateCommand(connection, query, parameters);
            var result = await command.ExecuteScalarAsync();

            CheckAndCloseConnection(connection);

            return result;
        }
        public static object ExecuteScalar(SqlConnection connection, string query, SqlParameter[] parameters = null)
        {
            CheckAndOpenConnection(connection);
            var command = CreateCommand(connection, query, parameters);
            var result = command.ExecuteScalar();

            CheckAndCloseConnection(connection);

            return result;
        }

        // ExecuteNonQuery is typically used when performing database operations that do not return a result set, such as inserting, updating, or deleting data. It can also be used for executing stored procedures or executing SQL commands that do not return any results, such as creating or dropping tables.
        public static async Task<int> ExecuteNonQueryAsync(SqlConnection connection, string query, SqlParameter[] parameters = null)
        {
            CheckAndOpenConnection(connection);
            var command = CreateCommand(connection, query, parameters);
            var result = await command.ExecuteNonQueryAsync();
            CheckAndCloseConnection(connection);

            return result;
        }

        private static SqlCommand CreateCommand(SqlConnection connection, string query, SqlParameter[] parameters = null)
        {
            SqlCommand command = new(query, connection);

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
        #endregion
    }
}
