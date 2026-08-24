using Microsoft.Data.SqlClient;
using System.Data;

namespace JapaneseLearning.User.Infrastructure.Database;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}

public sealed class SqlConnectionFactory(string connectionString)
    : ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(connectionString);
    }
}