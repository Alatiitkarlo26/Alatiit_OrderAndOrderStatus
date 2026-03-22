using MySql.Data.MySqlClient;
using GadgetStoreModels;
using System;

namespace GadgetStoreDataService
{
    public class GadgetStoreDbData
    {
        // I used XXAMPP for MySQL connection database.

        private string _connectionString = "server=localhost;database=gadget_store_db;user=root;password=;";

        public void SaveTransactionToDb(Transaction transaction)
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                }
            }
        }
    }
}