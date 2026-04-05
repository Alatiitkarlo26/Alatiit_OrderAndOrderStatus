using MySql.Data.MySqlClient;
using GadgetStoreModels;
using System;

namespace GadgetStoreDataService
{
    public class GadgetStoreDbData
    {
        // I used XAMPP for MySQL connection database.

        private string _connectionString = "server=localhost;database=gadget_store_db;user=root;password=;";

        public bool SaveTransactionToDb(Transaction transaction)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO transactions (transaction_id, product_name, total_price, transaction_date) " +
                                   "VALUES (@id, @name, @price, @date)";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", transaction.TransactionId.ToString());
                        cmd.Parameters.AddWithValue("@name", transaction.ProductName);
                        cmd.Parameters.AddWithValue("@price", transaction.TotalPrice);
                        cmd.Parameters.AddWithValue("@date", transaction.TransactionDate);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                 
                    Console.WriteLine($"[DB ERROR] Could not sync to XAMPP: {ex.Message}");
                    return false;
                }
            }
        }
    
        


        public void DeleteTransactionFromDb(Guid id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM transactions WHERE transaction_id = @id";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id.ToString());
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTransactionInDb(Transaction t)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE transactions SET product_name = @name, total_price = @price WHERE transaction_id = @id";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@name", t.ProductName);
                    cmd.Parameters.AddWithValue("@price", t.TotalPrice);
                    cmd.Parameters.AddWithValue("@id", t.TransactionId.ToString());
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}