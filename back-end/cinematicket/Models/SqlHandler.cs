using System;
using System.IO;
using cinematicket.Models.Interfaces;
using MySql.Data.MySqlClient;

namespace cinematicket.Models
{
    public class SqlHandler : ISqlHandler
    {
        private readonly MySqlConnection _connection;

        // public SqlHandler(MySqlConnection connection)
        // {
        //     _connection = connection;
        //     InitializeDatabase("cinema");
        // }
        
        public SqlHandler()
        {
            const string connectionString = "server=localhost;user=root;";
            _connection = new MySqlConnection(connectionString);
            InitializeDatabase("cinema");
        }

        public void ExecuteNonQuery(string query)
        {
            try
            {
                _connection.Open();
                var cmd = new MySqlCommand(query, _connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("failed in ExecuteNonQuery method.");
            }

            finally
            {
                _connection.Close();
            }
        }

        public MySqlDataReader ExecuteReader(string query)
        {
            try
            {
                _connection.Open();
                var command = new MySqlCommand(query, _connection);
                var result = command.ExecuteReader();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("failed in ExecuteReader");
            }
            finally
            {
                _connection.Close();
            }
        }

        public bool RecordExists(string query)
        {
            try
            {
                _connection.Open();
                var command = new MySqlCommand(query, _connection);
                return Convert.ToInt64(command.ExecuteScalar()) == 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("failed in ExecuteScalar");
            }
            finally
            {
                _connection.Close();
            } 
        }
        private void InitializeDatabase(string databaseName)
        {
            CreateDatabase(databaseName);
            _connection.ConnectionString += $";database={databaseName};";
            CreateTables();
        }

        private void CreateDatabase(string databaseName)
        {
            var query = $"create database if not exists {databaseName}";
            ExecuteNonQuery(query);
        }

        private void CreateTables()
        {
            var query = File.ReadAllText(@"Database\tables.txt");
            ExecuteNonQuery(query);
        }
    }
}