using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Restaurant.DataAccess
{
    public class SqlHelper
    {
        private readonly static string _connectionString;
        static SqlHelper()
        {
            IConfiguration configuration = new ConfigurationBuilder()
           .SetBasePath(AppContext.BaseDirectory) // or Directory.GetCurrentDirectory()
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection connection string not found");
        }

        private static SqlConnection GetConnection() => new SqlConnection(_connectionString);

        /// <summary>
        /// Creates a configured SqlCommand with stored procedure setup
        /// </summary>
        private static SqlCommand CreateCommand(string storedProcedure, SqlConnection connection, SqlParameter[] parameters = null)
        {
            var command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 60
            };

            if (parameters?.Length > 0)
                command.Parameters.AddRange(parameters);

            return command;
        }

        #region Synchronous Methods

        /// <summary>
        /// Executes a stored procedure and returns a DataTable with all results
        /// </summary>
        public static DataTable ExecuteDataTable(string storedProcedure, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            try
            {
                var dataTable = new DataTable();

                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                connection.Open();

                using var reader = command.ExecuteReader();
                dataTable.Load(reader);

                return dataTable;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error executing stored procedure '{storedProcedure}'.", ex);
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns a single value (first column of first row)
        /// </summary>
        public static T ExecuteScalar<T>(string storedProcedure, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            try
            {
                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                connection.Open();

                var result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return default(T);

                return (T)Convert.ChangeType(result, typeof(T));

            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error executing stored procedure '{storedProcedure}'.", ex);
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns affected rows count
        /// </summary>
        public static int ExecuteNonQuery(string storedProcedure, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            try
            {
                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                connection.Open();

                return command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error executing stored procedure '{storedProcedure}'.", ex);
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the value of the first output parameter
        /// </summary>
        public static T ExecuteWithOutputParameter<T>(string storedProcedure, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            if (parameters == null || parameters?.Length == 0)
                throw new ArgumentException("At least one output parameter is required.", nameof(parameters));

            try
            {
                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                connection.Open();
                command.ExecuteNonQuery();

                // Find the first output parameter
                var outputParam = Array.Find(parameters, p =>
                    p.Direction == ParameterDirection.Output ||
                    p.Direction == ParameterDirection.InputOutput);

                if (outputParam?.Value == null || outputParam.Value == DBNull.Value)
                    return default(T);

                return (T)Convert.ChangeType(outputParam.Value, typeof(T));
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error executing stored procedure '{storedProcedure}'.", ex);
            }
        }

        /// <summary>
        /// Executes a stored procedure with a reader callback for memory-efficient processing
        /// </summary>
        public static void ExecuteReader(string storedProcedure, Action<SqlDataReader> readerCallback, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            if (readerCallback == null)
                throw new ArgumentNullException(nameof(readerCallback));

            try
            {
                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                connection.Open();

                using var reader = command.ExecuteReader();
                readerCallback(reader);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error executing stored procedure '{storedProcedure}'.", ex);
            }
        }

        #endregion

        #region Async Methods

        /// <summary>
        /// Asynchronously executes a stored procedure and returns a DataTable
        /// </summary>
        public static async Task<DataTable> ExecuteDataTableAsync(string storedProcedure, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            try
            {
                var dataTable = new DataTable();

                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();
                dataTable.Load(reader);

                return dataTable;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Database error executing {storedProcedure}", ex);
            }
        }

        /// <summary>
        /// Asynchronously executes a stored procedure and returns a single value
        /// </summary>
        public static async Task<T> ExecuteScalarAsync<T>(string storedProcedure, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            try
            {
                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                    return default(T);

                return (T)Convert.ChangeType(result, typeof(T));
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Database error executing {storedProcedure}", ex);
            }
        }

        /// <summary>
        /// Asynchronously executes a stored procedure and returns affected rows count
        /// </summary>
        public static async Task<int> ExecuteNonQueryAsync(string storedProcedure, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            try
            {
                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                await connection.OpenAsync();

                return await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                throw;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error executing stored procedure '{storedProcedure}'.", ex);
            }
        }



        public static async Task ExecuteReaderAsync(string storedProcedure, Action<SqlDataReader> readerCallback, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            if (readerCallback == null)
                throw new ArgumentNullException(nameof(readerCallback));

            try
            {
                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();
                readerCallback(reader);
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error executing stored procedure '{storedProcedure}'.", ex);
            }
        }


        public static async Task<T> ExecuteWithOutputParameterAsync<T>(string storedProcedure, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            if (parameters == null || parameters?.Length == 0)
                throw new ArgumentException("At least one output parameter is required.", nameof(parameters));

            try
            {
                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                // Find the first output parameter
                var outputParam = Array.Find(parameters, p =>
                    p.Direction == ParameterDirection.Output ||
                    p.Direction == ParameterDirection.InputOutput);

                if (outputParam?.Value == null || outputParam.Value == DBNull.Value)
                    return default(T);

                return (T)Convert.ChangeType(outputParam.Value, typeof(T));
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error executing stored procedure '{storedProcedure}'.", ex);
            }
        }

        public static async Task<Dictionary<string,object>> ExecuteWithOutputParametersAsync(string storedProcedure, params SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(storedProcedure))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedure));

            if (parameters == null || parameters?.Length == 0)
                throw new ArgumentException("At least one output parameter is required.", nameof(parameters));

                var outputValues = new Dictionary<string, object>();
            try
            {
                using var connection = GetConnection();
                using var command = CreateCommand(storedProcedure, connection, parameters);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();


               foreach( SqlParameter p in command.Parameters)
               {
                   if(p.Direction == ParameterDirection.Output)
                   {
                       string paramName = p.ParameterName.StartsWith("@")
                           ? p.ParameterName.Substring(1) : p.ParameterName;

                       outputValues[paramName] =  
                       p.Value == DBNull.Value ? null : p.Value;
                   }
               }

               return outputValues;
            }
            catch (SqlException ex)
            {
                throw new DataAccessException($"Error executing stored procedure '{storedProcedure}'.", ex);
            }
        }

        #endregion

        #region Transaction Support

        /// <summary>
        /// Executes multiple stored procedures within a single transaction
        /// </summary>
        public static void ExecuteTransaction(Action<SqlConnection, SqlTransaction> transactionAction)
        {
            if (transactionAction == null)
                throw new ArgumentNullException(nameof(transactionAction));

            using var connection = GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                transactionAction(connection, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Asynchronously executes multiple stored procedures within a single transaction
        /// </summary>
        public static async Task ExecuteTransactionAsync(Func<SqlConnection, SqlTransaction, Task> transactionAction)
        {
            if (transactionAction == null)
                throw new ArgumentNullException(nameof(transactionAction));

            using var connection = GetConnection();
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                await transactionAction(connection, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a SQL parameter with the specified name and value
        /// </summary>
        public static SqlParameter CreateParameter(string name, object value, SqlDbType sqlDbType = SqlDbType.NVarChar)
        {
            return new SqlParameter(name, sqlDbType) { Value = value ?? DBNull.Value };
        }

        /// <summary>
        /// Creates an output SQL parameter
        /// </summary>
        public static SqlParameter CreateOutputParameter(string name, SqlDbType sqlDbType, int size = 0)
        {
            var parameter = new SqlParameter(name, sqlDbType)
            {
                Direction = ParameterDirection.Output
            };

            if (size > 0)
                parameter.Size = size;

            return parameter;
        }

        /// <summary>
        /// Tests the database connection
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                return connection.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    /// <summary>
    /// Custom exception for data access errors
    /// </summary>
         public class DataAccessException : Exception
         {
             public DataAccessException(string message) : base(message) { }
             public DataAccessException(string message, Exception innerException) : base(message, innerException) { }
         }
    }

}

