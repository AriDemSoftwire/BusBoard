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
            string postcode = selection.Postcode;

            var latRes = ApiProgram.getLatitude(postcode);
            var lonRes = ApiProgram.getLongitude(postcode);

            double latitude = latRes.Result;
            double longitude = lonRes.Result;

            List<string> busStops = new List<string>();
            List<string> busStopNames = new List<string>();
            List<string> busLines = new List<string>();

            var stopInfoRes = ApiProgram.getStops(latitude, longitude);
            var stopInfo = stopInfoRes.Result;
            var stopLength = stopInfo.stopPoints.Count;

            for (int i = 0; i < stopLength - 1; i++)
            {
                busStops.Add(stopInfo.stopPoints[i].naptanId);
                busStopNames.Add(stopInfo.stopPoints[i].commonName);
                busLines.Add(stopInfo.stopPoints[i].lines[0].name);
            }

            var busRes = ApiProgram.getBuses(stopLength, busLines, busStops);

            List<List<string>> buses = busRes.Result;

            var info = new BusInfo(selection.Postcode, buses);
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