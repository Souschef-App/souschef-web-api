using Npgsql;

namespace souschef.server.Helpers
{
    public static class ConnectionHelper
    {
        public static string GetConnectionString()
        {
            // var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return BuildConnectionString();
        }

        //build the connection string from the environment
        private static string BuildConnectionString()
        {
            // var databaseUri = new Uri(databaseUrl);
            // var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = Environment.GetEnvironmentVariable("DATABASE_URL"),
                Port = int.Parse(Environment.GetEnvironmentVariable("PGPORT")!),
                Username = Environment.GetEnvironmentVariable("PGUSER"),
                Password = Environment.GetEnvironmentVariable("PGPASSWORD"),
                Database = Environment.GetEnvironmentVariable("PGDATABASE"),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }
    }
}
