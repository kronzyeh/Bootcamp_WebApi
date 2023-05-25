using Example.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Example.WebApi.Controllers
{
    public class CarController : Controller
    {
        //Get list of cars
        //Tested in postman, when you give it a list of cars it shows it in a table defined in index.cshtml
        // GET /Car/Index
        public ActionResult Index(List<Car> cars)
        {

            return View(cars);
        }

        //Get specific car
        //Tested in postman, you give it a car object in JSON format and it gives back the car color and license plate (it can be something else this is just an example)
        // GET /Car/GetCar
        public string GetCar(Car car)
        {
            return car.CarColor + car.LicensePlate;
        } 

        //Add a new car to the list
        //Tested in postman, in body you give the list and one specific car and then it shows on the index table with new list with new car that we added
        // POST /Car/AddCar
        public ActionResult AddCar(List<Car> cars, Car car)
        {
            cars.Add(car);

            return View("Index", cars);
            
        }
        //Edit Parking spot of the specific car
        //Tested in postman, as a parameters you give it one car, and as a second parameter it is only a parking spot as a string and then it shows that car with the changed parking spot

        //PUT /Car/UpdateParkingSpot
        public ActionResult UpdateParkingSpot(Car car, string parkingSpot)
        {
            car.ParkingSpot = parkingSpot;
            List<Car> cars = new List<Car>();
            cars.Add(car);
            return View("Index", cars);
        }
        //delete car, tested on postman and when i give the list with a car that has the same parameter like the car that is the other parameter then it deletes it from the list
        //and on index the list is shown without that car

        // DELETE /Car/DeleteCar
        public ActionResult DeleteCar(List<Car> cars, Car car)
        {
            for (int i = cars.Count - 1; i >= 0; i--)
            {
                if (cars[i].LicensePlate == car.LicensePlate)
                {
                    cars.RemoveAt(i);
                }
            }

            return View("Index", cars);
        }
    }
}
