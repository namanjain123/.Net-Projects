using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Services.IService;

namespace Services.Service
{
    public class SQLiteService:ISQLiteService
    {
        private readonly string _connectionString;

        public SQLiteService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void UpdateData(object newData)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT OR REPLACE INTO @YourTable (@YourColumn) VALUES (@NewData)";
                    command.Parameters.AddWithValue("@NewData", newData);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
