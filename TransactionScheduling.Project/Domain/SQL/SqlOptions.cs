namespace TransactionScheduling.Project.Domain.SQL
{
    public enum SqlDatabase
    {
        DB1,
        DB2
    }
    public class SqlOptions
    {
        private readonly Dictionary<SqlDatabase, string> _connectionStrings = [];
        public string this[SqlDatabase db]
        {
            get => _connectionStrings[db];
            set => _connectionStrings[db] = value;
        }
        
    }
    public static class SqlExtension
    {
        public static void AddSql(this IServiceCollection services, IConfiguration config)
        {
            var sql = new SqlOptions();
            sql[SqlDatabase.DB1] = config.GetConnectionString(nameof(SqlDatabase.DB1))!;
            sql[SqlDatabase.DB2] = config.GetConnectionString(nameof(SqlDatabase.DB2))!;
            services.AddSingleton(sql);
        }
    }

}
