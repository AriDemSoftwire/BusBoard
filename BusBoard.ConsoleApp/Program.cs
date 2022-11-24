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
        static public void Main(string[] args)
        {
            string userPostcode = Console.ReadLine();

            var postcodeLatitude = ApiProgram.getLatitudeOfPostcode(userPostcode).latitude;

            var postcodeLongitude = ApiProgram.getLongitudeOfPostcode(userPostcode).longitude;

            var listOfStopsAtPostcode = ApiProgram.getStopsInRadius(postcodeLatitude, postcodeLongitude);

            var listOfStopsWithBuses = ApiProgram.getUpcomingBuses(listOfStopsAtPostcode);

            for (int i = 0; i < listOfStopsWithBuses.Count; i++)
            {
                Console.WriteLine($"Name of the stop: {listOfStopsWithBuses[i].commonName}");
                if (listOfStopsWithBuses[i].upcomingBuses.Count != 0)
                {
                    foreach (var bus in listOfStopsWithBuses[i].upcomingBuses)
                    {
                        Console.WriteLine(bus.lineName);
                    }
                }
                else
                {
                    Console.WriteLine("There are no upcoming buses at this stop");
                    continue;
                }
            }

            Console.ReadKey();
        }
    }
}





