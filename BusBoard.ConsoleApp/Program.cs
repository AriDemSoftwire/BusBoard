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

            double latitude = await ApiProgram.getLatitude(postcode);
            double longitude = await ApiProgram.getLongitude(postcode);

            List<string> busStops = new List<string>();
            List<string> busStopNames = new List<string>();
            List<string> busLines = new List<string>();

            var stopInfo = await ApiProgram.getStops(latitude, longitude);
            var stopLength = stopInfo.stopPoints.Count;

            for (int i = 0; i < stopLength - 1; i++)
            {
                busStops.Add(stopInfo.stopPoints[i].naptanId);
                busStopNames.Add(stopInfo.stopPoints[i].commonName);
                busLines.Add(stopInfo.stopPoints[i].lines[0].name);
            }

            List<List<string>> buses = await ApiProgram.getBuses(stopLength, busLines, busStops);


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





