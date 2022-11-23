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
using BusBoard.Api;

namespace BusBoard.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string postcode = Console.ReadLine();

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

            for (int i = 0; i < busStopNames.Count - 1; i++)
            {
                Console.WriteLine($"Name of the stop: {busStopNames[i]}");
                foreach (var bus in buses[i])
                {
                    Console.WriteLine(bus);
                }
            }

            Console.ReadKey();
        }
    }
}





