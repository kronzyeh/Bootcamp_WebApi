using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.WebPages;
using Example.WebApi.Models;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Example.WebApi.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";
        public RestaurantController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
        }
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Restaurant", connection))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
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
                    return Request.CreateResponse(HttpStatusCode.OK, restaurants);
                }
            }
            catch (Exception ex)
            {

                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(Guid id)
        {
            try
            {
                Restaurant restaurant = GetRestaurantById(id);

                return Request.CreateResponse(HttpStatusCode.OK, restaurant);
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] Restaurant restaurant)
        {

            try
            {


                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("INSERT INTO Restaurant (Id, Title, Seats, Address, OwnerName) VALUES (@param1, @param2, @param3, @param4, @param5)", connection))
                    {
                        command.Parameters.AddWithValue("@param1", restaurant.Id = Guid.NewGuid());
                        command.Parameters.AddWithValue("@param2", restaurant.Title);
                        command.Parameters.AddWithValue("@param3", restaurant.Seats);
                        command.Parameters.AddWithValue("@param4", restaurant.Address);
                        if (!string.IsNullOrEmpty(restaurant.OwnerName))
                        {
                            command.Parameters.AddWithValue("@OwnerName", restaurant.OwnerName);
                        }


                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.OK, restaurant);

        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(Guid id, [FromBody] Restaurant restaurant)
        {
           
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
                    command.ExecuteNonQuery();
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, restaurant);
        }



        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                Restaurant restaurant = GetRestaurantById(id);
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("Delete From Restaurant WHERE Id = @id", connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch( Exception ex )
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        private Restaurant GetRestaurantById(Guid id)
        {
            Restaurant restaurant = new Restaurant();
            using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("Select * From Restaurant WHERE Id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
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