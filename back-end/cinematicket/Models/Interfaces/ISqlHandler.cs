using MySql.Data.MySqlClient;

namespace cinematicket.Models.Interfaces
{
    public interface ISqlHandler
    {
        void ExecuteNonQuery(string query);
        MySqlDataReader ExecuteReader(string query);
        bool RecordExists(string query);
    }
}