using MySql.Data.MySqlClient;
using GadgetStoreModels;
using System;

namespace GadgetStoreDataService
{
    public class GadgetStoreDbData
    {
        // I used Xampp for MySQL connection database.

        /* 
           SQL SCRIPT FOR XAMPP: 
                  CREATE TABLE transactions (
                   transaction_id VARCHAR(50) PRIMARY KEY,
                   product_name VARCHAR(100) NOT NULL,
                   total_price DECIMAL(10, 2) NOT NULL,
                   quantity INT NOT NULL,
                   transaction_date DATETIME NOT NULL ); 
        */


        // Tells C# where the MySQL server is located.
        private string _connectionString = "server=localhost;database=gadget_store_db;user=root;password=;";

        public bool SaveTransactionToDb(Transaction transaction)
        { 
            
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                 
                    string query = "INSERT INTO transactions (transaction_id, product_name, total_price, quantity, transaction_date) " +
                                   "VALUES (@id, @name, @price, @qty, @date)";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", transaction.TransactionId.ToString());
                        cmd.Parameters.AddWithValue("@name", transaction.ProductName);
                        cmd.Parameters.AddWithValue("@price", transaction.TotalPrice);
                        cmd.Parameters.AddWithValue("@qty", transaction.Quantity); 
                        cmd.Parameters.AddWithValue("@date", transaction.TransactionDate);
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DB ERROR] Save failed: {ex.Message}");
                    return false;
                }
            }
        }

        public void DeleteTransactionFromDb(Guid id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM transactions WHERE transaction_id = @id";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DB ERROR] Delete failed: {ex.Message}");
                }
            }
        }

        public void UpdateTransactionInDb(Transaction t)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                 
                    string query = "UPDATE transactions SET product_name = @name, total_price = @price, quantity = @qty WHERE transaction_id = @id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", t.ProductName);
                        cmd.Parameters.AddWithValue("@price", t.TotalPrice);
                        cmd.Parameters.AddWithValue("@qty", t.Quantity);
                        cmd.Parameters.AddWithValue("@id", t.TransactionId.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DB ERROR] Update failed: {ex.Message}");
                }
            }
        }
    }
}