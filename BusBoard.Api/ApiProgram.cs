using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using System.Security.Cryptography.X509Certificates;
using static BusBoard.Api.ApiProgram;
using System.IO.Ports;
using BusBoard.Api;

namespace BusBoard.Api
{

    public static class ApiProgram
    {
        public static Result getLatitudeOfPostcode(string post)
        {
            RestClient client = new RestClient();
            string postUrl = $"https://api.postcodes.io/postcodes/{post}";
            var request = new RestRequest(postUrl);
            var response = client.Execute<Postcode>(request).Data.result;
            return response;
        }

        public static Result getLongitudeOfPostcode(string post)
        {
            RestClient client = new RestClient();
            string postUrl = $"https://api.postcodes.io/postcodes/{post}";
            var request = new RestRequest(postUrl);
            var response = client.Execute<Postcode>(request).Data.result;
            return response;
        }

        public static List<StopPoints> getStopsInRadius(double lat, double lon)
        {
            RestClient client = new RestClient();
            var stopUrl = $"https://api.tfl.gov.uk/StopPoint/?lat={lat}&lon={lon}&stopTypes=NaptanPublicBusCoachTram";
            var request = new RestRequest(stopUrl);
            var response = client.Execute<Stop>(request).Data.stopPoints;
            return response;
        }
        public static List<StopPoints> getUpcomingBuses(List<StopPoints> stops)
        {
            RestClient client = new RestClient();
            List<StopPoints> stopInfo = stops;

            for (int i = 0; i < stopInfo.Count; i++)
            {
                string listOfLines = "";
                foreach (Line line in stopInfo[i].lines)
                {
                    listOfLines = listOfLines + line.name + ',';
                }

                var busUrl = $"https://api.tfl.gov.uk/Line/{listOfLines}/Arrivals/{stops[i].naptanId}";

                var request = new RestRequest(busUrl);
                var response = client.Execute<List<Bus>>(request).Data;
                stopInfo[i].upcomingBuses = response;

            }

            return stopInfo;
        }

    }

    public class Postcode
    {
        public Result result { get; set; }
    }

    public class Stop
    {
        public List<StopPoints> stopPoints { get; set; }
    }

    public class Line
    {
        public string name { get; set; }

    }

    public class Bus
    {
        public string lineName { get; set; }
    }

}
public class Result
{
    public double latitude { get; set; }
    public double longitude { get; set; }
}

public class StopPoints
{
    public string naptanId { get; set; }
    public List<Line> lines { get; set; }
    public string commonName { get; set; }
    public List<Bus> upcomingBuses { get; set; }

}
