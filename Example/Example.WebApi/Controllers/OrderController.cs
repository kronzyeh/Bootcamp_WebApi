using Example.WebApi.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    public class OrderController : ApiController
    {
        private readonly string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            List<OrderWaiter> orders = new List<OrderWaiter>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select \"FirstName\" as \"Waiter first name\", \"LastName\" as \"Waiter last name\", \"Price\"\r\nfrom \"Order\" \r\nleft join waiter on \"Order\".\"WaiterId\" = waiter.\"Id\" ", connection))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderWaiter order = new OrderWaiter();
                                order.WaiterFirstName = (string)reader["Waiter first name"];
                                order.WaiterLastName = (string)reader["Waiter last name"];
                                order.OrderPrice = Convert.ToDouble(reader["Price"]);
                                orders.Add(order);
                            }

                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, orders);
                   
                }
            }
            catch (Exception ex)
            {

                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] Order order)
        {
            try
            {


                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("INSERT INTO \"Order\" (\"Id\", \"Items\", \"Price\", \"WaiterId\", \"RestaurantId\") VALUES (@param1, @param2, @param3, @param4, @param5)", connection))
                    {
                        command.Parameters.AddWithValue("@param1", order.Id = Guid.NewGuid());
                        command.Parameters.AddWithValue("@param2", order.Items);
                        command.Parameters.AddWithValue("@param3", order.Price);
                        command.Parameters.AddWithValue("@param4", order.WaiterId);
                        command.Parameters.AddWithValue("@param5", order.RestaurantId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message.ToString());
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}