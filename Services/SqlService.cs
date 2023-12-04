using almondCove.Interefaces.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;

namespace almondCove.Services
{
    public class SqlService : ISqlService
  {
    private readonly IConfigManager _configuration;
    private readonly SqlConnection _connection;
    private readonly ILogger<SqlService> _logger;

    public SqlService(IConfigManager configManager, ILogger<SqlService> logger)
    {
      _configuration = configManager;
      _connection = new SqlConnection(_configuration.GetConnString());
      _logger = logger;
    }

    public SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null, SqlConnection customConnection = null, SqlTransaction transaction = null)
    {
      SqlDataReader reader = null;

      using (SqlConnection connection = customConnection ?? new SqlConnection(_configuration.GetConnString()))
      {
        if (connection.State != ConnectionState.Open)
        {
          connection.Open();
        }

        using SqlCommand command = new(query, _connection, transaction);
        if (parameters != null)
        {
          command.Parameters.AddRange(parameters);
        }

        try
        {
          transaction ??= connection.BeginTransaction();

          command.Transaction = transaction;

          reader = command.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (Exception ex)
        {
          // If an error occurs, close the reader and rollback the transaction
          reader?.Close();
          transaction?.Rollback();
          Console.WriteLine(ex.Message);
        }
      }

      return reader;
    }

    public void Dispose()
    {
      _connection.Dispose();
    }

    public int ExecuteNonQuery(string query, SqlParameter[] parameters = null, SqlConnection customConnection = null, SqlTransaction transaction = null)
    {
      int rowsAffected = 0;
      _logger.LogError(query?.ToString());

      using (SqlConnection connection = customConnection ?? new SqlConnection(_configuration.GetConnString()))
      {
        if (connection.State != ConnectionState.Open)
        {
          connection.Open();
        }

        using SqlCommand command = new(query, connection, transaction);
        if (parameters != null)
        {
          command.Parameters.AddRange(parameters);
        }

        try
        {
          transaction ??= connection.BeginTransaction();
          command.Transaction = transaction;
          rowsAffected = command.ExecuteNonQuery();
          transaction.Commit();
        }
        catch (Exception ex)
        {
          transaction?.Rollback();
          _logger.LogError(ex.Message?.ToString());
        }
      }

      return rowsAffected;
    }

    public object ExecuteScalar(string query, SqlParameter[] parameters = null)
    {
      object result = null;

      using (SqlConnection connection = new(_configuration.GetConnString()))
      {
        if (connection.State != ConnectionState.Open)
        {
          connection.Open();
        }

        using SqlCommand command = new(query, connection);
        if (parameters != null)
        {
          command.Parameters.AddRange(parameters);
        }

        try
        {
          result = command.ExecuteScalar();
        }
        catch (Exception ex)
        {
          // Handle exceptions as needed
          Console.WriteLine(ex.Message);
        }
      }

      return result;
    }

  }

}
