using Example.WebApi.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Example.WebApi.Controllers
{
    public class CarController : ApiController
    {
        public static List<Car> cars = new List<Car>();
        private readonly string _connectionString;
        public CarController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;
        }
        // GET /Car/Index
        public HttpResponseMessage Get()
        {
            if (cars.Count == 0) return Request.CreateResponse(HttpStatusCode.NotFound);
            return Request.CreateResponse(HttpStatusCode.OK, cars);
        }

        
        // GET /Car/Get
        public HttpResponseMessage Get(string licensePlate)
        {
            if(cars.Count==0) return Request.CreateResponse(HttpStatusCode.BadRequest);
            foreach (Car car in cars)
            {
                if (car.LicensePlate == licensePlate) return Request.CreateResponse(HttpStatusCode.OK, licensePlate);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        } 


        // POST /Car/AddCar
        public HttpResponseMessage Post(Car car)
        {
            string _connectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=tomo;";



            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO cars (licenseplate, parkingspot, carcolor) VALUES (@param1, @param2, @param3)", connection))
                    {
                        command.Parameters.AddWithValue("@param1", car.LicensePlate);
                        command.Parameters.AddWithValue("@param2", car.ParkingSpot);
                        command.Parameters.AddWithValue("@param3", car.CarColor);
  

                        command.ExecuteNonQuery();
                    }
                }
       
            return Request.CreateResponse(HttpStatusCode.OK, cars);
            
        }


        //PUT /Car/Put
        [HttpPut]
        public HttpResponseMessage Put(string licensePlate, string parkingSpot)
        {
            if (cars.Count == 0) return Request.CreateResponse(HttpStatusCode.NotFound);
            Car carToEdit = cars.FirstOrDefault(p => p.LicensePlate==licensePlate);
            if(carToEdit == null)
            {
                carToEdit.ParkingSpot = parkingSpot;
            }
            return Request.CreateResponse(HttpStatusCode.OK, cars);
        }

        // DELETE /Car/DeleteCar
        
        public HttpResponseMessage Delete(string licensePlate)
        {
            if (cars.Count == 0) return Request.CreateResponse(HttpStatusCode.NotFound);
            Car carToDelete = cars.FirstOrDefault(p => p.LicensePlate==licensePlate);
            if (carToDelete != null)
            {
                cars.Remove(carToDelete);
                return Request.CreateResponse(HttpStatusCode.OK, cars);
            }

            return Request.CreateResponse(HttpStatusCode.OK, cars);
        }
    }
}
