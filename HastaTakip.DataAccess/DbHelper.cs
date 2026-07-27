using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;


namespace HastaTakip.DataAccess
{
    public class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
