using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
namespace TransactionScheduling.Project.Domain.Operations
{
    public enum RollbackOperation
    {
        Insert, Delete, Update, Nothing
    }
    // An operation which can only affect one table
    public abstract class BaseSqlOperation(SqlConnection con)
    {
        protected readonly SqlConnection _con = con;
        public abstract string TableName { get; }
        public List<int?> RowIds { get; set; } = new();
        
        private DataTable? PreviousData;
        protected RollbackOperation RollbackOperation = RollbackOperation.Nothing;

        protected void LoadCurrentData()
        {
            
            if (RowIds.Any())
            {
                using var cmd = new SqlCommand($"SELECT * FROM [{TableName}] WHERE [Id] IN ({string.Join(",",RowIds)}) ", _con);
                using var adadpter = new SqlDataAdapter(cmd);
                using var table = new DataTable();
                adadpter.Fill(table);
                PreviousData = table;
            }
        }

        public virtual object? Execute()
        {
            LoadCurrentData();

            return default;
        }


        public void Rollback()
        {
            if (RowIds.Any())
            {
                switch (RollbackOperation)
                {
                    case RollbackOperation.Insert:
                        ExecuteInsertRollback();
                        break;

                    case RollbackOperation.Update:
                        ExecuteUpdateRollback();
                        break;

                    case RollbackOperation.Delete:
                        ExecuteDeleteRollback();
                        break;
                }

            }
        }

        private void ExecuteDeleteRollback()
        {
            using var cmd = new SqlCommand($"DELETE FROM [{TableName}] WHERE [Id] IN ({string.Join(",",RowIds)})", _con);
            Console.WriteLine($"Execut op rollback {cmd.CommandText}");
            cmd.ExecuteNonQuery();

        }

        private void ExecuteInsertRollback()
        {
            using var preCmd = new SqlCommand($"SET IDENTITY_INSERT [dbo].[{TableName}] ON; ", _con);
            preCmd.ExecuteNonQuery();

            foreach (DataRow row in PreviousData.Rows)
            {
                List<string> cols = [];
                List<string> values = [];
                foreach (DataColumn col in row.Table.Columns)
                {
                    cols.Add($"[{col.ColumnName}]");
                    values.Add(FormatValue(col, row[col.ColumnName]));

                }
              
                using var cmd = new SqlCommand($"INSERT INTO [{TableName}]({string.Join(",", cols)}) VALUES ({string.Join(",", values)})", _con);
                Console.WriteLine($"Execut op rollback {cmd.CommandText}");
                cmd.ExecuteNonQuery();
              
            }
            using var postCmd = new SqlCommand($"SET IDENTITY_INSERT [dbo].[{TableName}] OFF; ", _con);
            postCmd.ExecuteNonQuery();

        }

        private void ExecuteUpdateRollback()
        {
            foreach (DataRow row in PreviousData.Rows)
            {

                List<string> assingments = [];

                foreach (DataColumn col in row.Table.Columns)
                {
                    if (!col.ColumnName.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                    {

                        assingments.Add($"[{col.ColumnName}] = {FormatValue(col, row[col.ColumnName])}");
                    }
                }
                var cmd = new SqlCommand($"UPDATE {TableName} SET {string.Join(",", assingments)} WHERE [Id] = {row["Id"]} ", _con);
                Console.WriteLine($"Execut op rollback {cmd.CommandText}");
                cmd.ExecuteNonQuery();

            }
        }

        private static string FormatValue(DataColumn col, object value)
        {
            if (value == DBNull.Value)
                return "NULL";
            if (col.DataType == typeof(string))
                return $"'{value.ToString().Replace("'","`")}'";
            if (col.DataType == typeof(DateTime))
                return $"'{(DateTime)value:yyyy-MM-dd HH:mm:ss}'";
            if (col.DataType == typeof(TimeSpan))
                return $"'{(TimeSpan)value:HH:mm:ss}'";
            return value.ToString() ?? "NULL";

        }

    }

}
