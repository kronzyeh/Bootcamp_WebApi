using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.WebPages;
using Example.WebApi.Models;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Example.WebApi.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly string _connectionString;
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
                string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";
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
            }
            return Request.CreateResponse(HttpStatusCode.OK, restaurants);
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
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] Restaurant restaurant)
        {

            try
            {
                string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";


                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("INSERT INTO Restaurant (Id, Title, Seats, Address, OwnerName) VALUES (@param1, @param2, @param3, @param4, @param5)", connection))
                    {
                        command.Parameters.AddWithValue("@param1", restaurant.Id = Guid.NewGuid());
                        command.Parameters.AddWithValue("@param2", restaurant.Title);
                        command.Parameters.AddWithValue("@param3", restaurant.Seats);
                        command.Parameters.AddWithValue("@param4", restaurant.Address);
                        command.Parameters.AddWithValue("@param5", restaurant.OwnerName);


                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
            }

            return Request.CreateResponse(HttpStatusCode.OK, restaurant);

        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(Guid id, [FromBody] Restaurant restaurant)
        {

            try
            {
                Restaurant restaurantBase = GetRestaurantById(id);
                string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("UPDATE Restaurant SET Title = @Title, Seats = @Seats, Address = @Address, OwnerName = @OwnerName WHERE id = @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        if (restaurant.Title.IsEmpty())
                        {
                            restaurant.Title = restaurantBase.Title;
                        }
                        else
                        {
                            restaurant.Title = restaurant.Title;
                        }
                        command.Parameters.AddWithValue("@Title", restaurant.Title);
                        if (restaurant.Seats == null)
                        {
                            restaurant.Seats = restaurantBase.Seats;
                        }
                        else
                        {
                            restaurant.Seats = restaurant.Seats;
                        }
                        command.Parameters.AddWithValue("@Seats", restaurant.Seats);
                        if (restaurant.Address.IsEmpty())
                        {
                            restaurant.Address = restaurantBase.Address;
                        }
                        else
                        {
                            restaurant.Address = restaurant.Address;
                        }
                        command.Parameters.AddWithValue("@Address", restaurant.Address);
                        if (restaurant.OwnerName.IsEmpty())
                        {
                            restaurant.OwnerName = restaurantBase.OwnerName;
                        }
                        else
                        {
                            restaurant.OwnerName = restaurant.OwnerName;
                        }
                        command.Parameters.AddWithValue("@OwnerName", restaurant.OwnerName);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex )
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest, restaurant);
            }
            return Request.CreateResponse(HttpStatusCode.OK, restaurant);
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(Guid id)
        {
            string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";
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
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        private Restaurant GetRestaurantById(Guid id)
        {
            string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";
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