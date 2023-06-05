using Example.Model;
using Example.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Repository
{
    public class WaiterRepository : IWaiterRepository
    {
        private readonly string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";

        public async Task<List<Waiter>> Get()
        {
            List<Waiter> waiters = new List<Waiter>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select \"FirstName\" as \"Waiter first name\", \"LastName\" as \"Waiter last name\", title as \"Restaurant name\" from waiter \r\njoin restaurant on restaurant.id = waiter.restaurantid ", connection))
                    {
                        using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Waiter waiter = new Waiter();


                                waiter.FirstName = (string)reader["Waiter first name"];
                                waiter.LastName = (string)reader["Waiter last name"];
                                waiter.RestaurantName = (string)reader["Restaurant name"];
                                

                                waiters.Add(waiter);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return waiters;
        }
        public async Task<Waiter> Get(Guid id)
        {
            Waiter waiter = new Waiter();
            try
            {
                waiter = await GetWaiterById(id);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return waiter;
        }
        public async Task<Waiter> GetWaiterById(Guid id)
        {
            Waiter waiter = new Waiter();
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("Select * From Waiter WHERE Id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        reader.Read();

                        waiter.Id = (Guid)reader["Id"];
                        waiter.FirstName = (string)reader["FirstName"];
                        waiter.LastName = (string)reader["LastName"];
                        waiter.RestaurantId = (Guid)reader["RestaurantId"];
                    }
                }
            }
            return waiter;
        }
    }
}
