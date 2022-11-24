using System.Web.Mvc;
using BusBoard.Web.Models;
using BusBoard.Web.ViewModels;
using BusBoard.Api;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace BusBoard.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BusInfo(PostcodeSelection selection)
        {
            string userPostcode = selection.Postcode;

            var postcodeLatitude = ApiProgram.getLatitudeOfPostcode(userPostcode).latitude;

            var postcodeLongitude = ApiProgram.getLongitudeOfPostcode(userPostcode).longitude;

            var listOfStopsAtPostcode = ApiProgram.getStopsInRadius(postcodeLatitude, postcodeLongitude);

            var listOfStopsWithBuses = ApiProgram.getUpcomingBuses(listOfStopsAtPostcode);

            var info = new BusInfo(selection.Postcode, listOfStopsWithBuses);

            return View(info);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Information about this site";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact us!";

            return View();
        }
    }
}