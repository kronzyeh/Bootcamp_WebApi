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
    public class WaiterController : ApiController
    {
        private readonly string _connectionString = "Server=localhost;Port=5432;Database=restaurant_database;User Id=postgres;Password=tomo;";
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] Waiter waiter)
        {
            try
            {


                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("INSERT INTO Waiter (\"Id\", \"FirstName\", \"LastName\", RestaurantId) VALUES (@param1, @param2, @param3, @param4)", connection))
                    {
                        command.Parameters.AddWithValue("@param1", waiter.Id = Guid.NewGuid());
                        command.Parameters.AddWithValue("@param2", waiter.FirstName);
                        command.Parameters.AddWithValue("@param3", waiter.LastName);
                        command.Parameters.AddWithValue("@param4", waiter.RestaurantId);

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