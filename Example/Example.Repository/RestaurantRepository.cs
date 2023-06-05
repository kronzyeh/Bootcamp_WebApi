using Example.Common;
using Example.Model;
using Example.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Example.Repository
{
    public class RestaurantRepository:IRestaurantRepository
    {
        private readonly string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";
        public async Task<List<Restaurant>> Get()
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Restaurant", connection))
                    {
                        using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Restaurant restaurant = new Restaurant();

                                restaurant.Id = (Guid)reader["Id"];
                                restaurant.Title = (string)reader["Title"];
                                restaurant.Seats = (int)reader["Seats"];
                                restaurant.Address = (string)reader["Address"];
                                restaurant.OwnerName = (string)reader["OwnerName"];

                                restaurants.Add(restaurant);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString()); 
            }
            return restaurants;
        }
        public async Task<PageDetails> Get(Paging paging, Sorting sorting, Filter filter)
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            int totalCount = 0;
            PageDetails pageDetails = new PageDetails();
            pageDetails.ItemsPerPage = paging.PageSize;
            pageDetails.PageNumber = paging.PageNumber;
            
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    StringBuilder query = new StringBuilder("SELECT * FROM Restaurant ");
                    StringBuilder countQuery = new StringBuilder("SELECT COUNT(*) FROM Restaurant ");
                    if (filter != null){
                        query.Append("WHERE seats >= @minimumSeats ");
                    }
                    
                    if (sorting != null)
                    {
                        query.Append("ORDER BY "); 
                        query.Append(sorting.OrderBy); 

                        if (sorting.SortOrder[0] == 'D')
                            query.Append(" DESC "); 
                        else
                            query.Append(" ASC "); 
                    }
                    if (paging != null)
                    {
                        query.Append("OFFSET @offset "); 
                        query.Append("LIMIT @limit "); 

                        
                        using (NpgsqlCommand cmd = new NpgsqlCommand(query.ToString(), connection))
                        {
                            cmd.Parameters.AddWithValue("@offset", (paging.PageNumber - 1) * paging.PageSize);
                            cmd.Parameters.AddWithValue("@limit", paging.PageSize);
                            cmd.Parameters.AddWithValue("@minimumSeats", filter.MinimumSeats);

                            using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                            {
                                while (reader.Read())
                                {
                                    Restaurant restaurant = new Restaurant();
                                    restaurant.Id = (Guid)reader["Id"];
                                    restaurant.Title = (string)reader["Title"];
                                    restaurant.Seats = (int)reader["Seats"];
                                    restaurant.Address = (string)reader["Address"];
                                    restaurant.OwnerName = (string)reader["OwnerName"];
                                    restaurants.Add(restaurant);
                                    pageDetails.restaurants = restaurants;
                                }
                                reader.Close();
                            }
                        }
                    }
                    using (NpgsqlCommand countCmd = new NpgsqlCommand(countQuery.ToString(), connection))
                    {
                        object totalCounts = await countCmd.ExecuteScalarAsync();
                        pageDetails.TotalCount = Convert.ToInt32(totalCounts);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            
            return (pageDetails);
        }



        public async Task<Restaurant> Get(Guid id)
        {
            Restaurant restaurant = new Restaurant();
            try
            {
                restaurant = await GetRestaurantById(id);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return restaurant;
        }
        public async Task<int> Post([FromBody] Restaurant restaurant)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO Restaurant (Id, Title, Seats, Address, OwnerName) VALUES (@Id, @Title, @Seats, @Address, @OwnerName)", connection))
                    {
                        command.Parameters.AddWithValue("@Id", restaurant.Id = Guid.NewGuid());
                        command.Parameters.AddWithValue("@Title", restaurant.Title);
                        command.Parameters.AddWithValue("@Seats", restaurant.Seats);
                        command.Parameters.AddWithValue("@Address", restaurant.Address);
                        command.Parameters.AddWithValue("@OwnerName", restaurant.OwnerName);


                        rowsAffected = await command.ExecuteNonQueryAsync();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }

            return rowsAffected;
        }
        public async Task<int> Put(Guid id, [FromBody] Restaurant restaurant)
        {
            int rowsAffected = 0;
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand())
                {

                    StringBuilder updateQuery = new StringBuilder("UPDATE Restaurant SET ");

                    if (!string.IsNullOrEmpty(restaurant.Title))
                    {
                        updateQuery.Append("Title = @Title, ");
                        command.Parameters.AddWithValue("@Title", restaurant.Title);

                    }

                    if (restaurant.Seats != null)
                    {
                        updateQuery.Append("Seats = @Seats, ");
                        command.Parameters.AddWithValue("@Seats", restaurant.Seats);
                    }

                    if (!string.IsNullOrEmpty(restaurant.Address))
                    {
                        updateQuery.Append("Address = @Address, ");
                        command.Parameters.AddWithValue("@Address", restaurant.Address);
                    }

                    if (!string.IsNullOrEmpty(restaurant.OwnerName))
                    {
                        updateQuery.Append("OwnerName = @OwnerName, ");
                        command.Parameters.AddWithValue("@OwnerName", restaurant.OwnerName);
                    }


                    if (updateQuery.Length > 0)
                    {
                        updateQuery.Length -= 2;
                    }

                    updateQuery.Append(" WHERE id = @Id");

                    string query = updateQuery.ToString();
                    command.CommandText = query;
                    command.Connection = connection;
                    command.Parameters.AddWithValue("Id", @id);
                    rowsAffected = await command.ExecuteNonQueryAsync();
                    
                }
            }

            return rowsAffected;
        }
        public async Task<int> Delete(Guid id)
        {
            int rowsAffected = 0;
            try
            {
                Restaurant restaurant = await GetRestaurantById(id);
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("Delete From Restaurant WHERE Id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        rowsAffected = await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }
            return rowsAffected;
        }
        public async Task<Restaurant> GetRestaurantById(Guid id)
        {
            Restaurant restaurant = new Restaurant();
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("Select * From Restaurant WHERE Id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        reader.Read();

                        restaurant.Id = (Guid)reader["Id"];
                        restaurant.Title = (string)reader["Title"];
                        restaurant.Seats = (int)reader["Seats"];
                        restaurant.Address = (string)reader["Address"];
                        restaurant.OwnerName = (string)reader["OwnerName"];
                    }
                }
            }
            return restaurant;
        }
    }
}
