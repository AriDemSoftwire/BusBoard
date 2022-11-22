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

namespace BusBoard.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var client = new RestClient();
            client.UseNewtonsoftJson();

            string postcode = Console.ReadLine();
            string postUrl = $"https://api.postcodes.io/postcodes/{postcode}";

            async Task<RestResponse> fetchMethod(string urlAddress)
            {
                var request = new RestRequest(urlAddress);
                var response = await client.GetAsync(request);
                return response;
            }

            var postResponse = fetchMethod(postUrl);

            var postResult = postResponse.Result;
            var postJson = JsonConvert.DeserializeObject<Postcode>(postResult.Content);

            double postLat = postJson.result.latitude;
            double postLon = postJson.result.longitude;

            Console.WriteLine(Math.Round(postLat, 2));
            Console.WriteLine(Math.Round(postLon, 2));

            var stopUrl = $"https://api.tfl.gov.uk/StopPoint/?lat={postLat}&lon={postLon}&stopTypes=NaptanPublicBusCoachTram";

            var stopResponse = fetchMethod(stopUrl);
            var stopResult = stopResponse.Result;
            var stopJson = JsonConvert.DeserializeObject<Root>(stopResult.Content);

            List<string> busStops = new List<string>();
            List<string> busStopNames = new List<string>();
            List<string> busLines = new List<string>();

            var stopLength = stopJson.stopPoints.Count;

            for (int i = 0; i < stopLength - 1; i++)
            {
                busStops.Add(stopJson.stopPoints[i].naptanId);
                busStopNames.Add(stopJson.stopPoints[i].commonName);
                busLines.Add(stopJson.stopPoints[i].lines[0].name);
            }


            for (int i = 0; i < stopLength - 1; i++)
            {
                var busUrl = $"https://api.tfl.gov.uk/Line/{busLines[i]}/Arrivals/{busStops[i]}";

                var busResponse = fetchMethod(busUrl);

                Console.WriteLine($"Name of the stop: {busStopNames[i]}");
                try
                {
                    var busResult = busResponse.Result;
                    var busJson = JsonConvert.DeserializeObject<Bus>(busResult.Content);
                    Console.WriteLine(busJson.lineName);
                }
                catch
                {
                    var busResult = busResponse.Result;
                    var busJson = JsonConvert.DeserializeObject<List<Bus>>(busResult.Content);

                    var busJsonLength = busJson.Count;
                    for (int j = 0; j < busJsonLength - 1; j++)
                    {
                        Console.WriteLine(busJson[j].lineName);
                    }
                }



            }
            Console.ReadKey();
        }




        public class Bus
        {
            [JsonProperty("$type")]
            public string type { get; set; }
            public string id { get; set; }
            public int operationType { get; set; }
            public string vehicleId { get; set; }
            public string naptanId { get; set; }
            public string stationName { get; set; }
            public string lineId { get; set; }
            public string lineName { get; set; }
            public string platformName { get; set; }
            public string direction { get; set; }
            public string bearing { get; set; }
            public string destinationNaptanId { get; set; }
            public string destinationName { get; set; }
            public DateTime timestamp { get; set; }
            public int timeToStation { get; set; }
            public string currentLocation { get; set; }
            public string towards { get; set; }
            public DateTime expectedArrival { get; set; }
            public DateTime timeToLive { get; set; }
            public string modeName { get; set; }
            public Timing timing { get; set; }
        }

        public class Timing
        {
            [JsonProperty("$type")]
            public string type { get; set; }
            public string countdownServerAdjustment { get; set; }
            public DateTime source { get; set; }
            public DateTime insert { get; set; }
            public DateTime read { get; set; }
            public DateTime sent { get; set; }
            public DateTime received { get; set; }
        }

        // Postcodes
        public class Codes
        {
            public string admin_district { get; set; }
            public string admin_county { get; set; }
            public string admin_ward { get; set; }
            public string parish { get; set; }
            public string parliamentary_constituency { get; set; }
            public string ccg { get; set; }
            public string ccg_id { get; set; }
            public string ced { get; set; }
            public string nuts { get; set; }
            public string lsoa { get; set; }
            public string msoa { get; set; }
            public string lau2 { get; set; }
        }

        public class Result
        {
            public string postcode { get; set; }
            public int quality { get; set; }
            public int eastings { get; set; }
            public int northings { get; set; }
            public string country { get; set; }
            public string nhs_ha { get; set; }
            public double longitude { get; set; }
            public double latitude { get; set; }
            public string european_electoral_region { get; set; }
            public string primary_care_trust { get; set; }
            public string region { get; set; }
            public string lsoa { get; set; }
            public string msoa { get; set; }
            public string incode { get; set; }
            public string outcode { get; set; }
            public string parliamentary_constituency { get; set; }
            public string admin_district { get; set; }
            public string parish { get; set; }
            public object admin_county { get; set; }
            public string admin_ward { get; set; }
            public object ced { get; set; }
            public string ccg { get; set; }
            public string nuts { get; set; }
            public Codes codes { get; set; }
        }

        public class Postcode
        {
            public int status { get; set; }
            public Result result { get; set; }
        }

        // Bus stop info

        public class Root
        {
            public List<double> centrePoint { get; set; }
            public List<StopPoint> stopPoints { get; set; }
            public int pageSize { get; set; }
            public int total { get; set; }
            public int page { get; set; }
        }
        public class AdditionalProperty
        {
            [JsonProperty("$type")]
            public string type { get; set; }
            public string category { get; set; }
            public string key { get; set; }
            public string sourceSystemKey { get; set; }
            public string value { get; set; }
        }

        public class Crowding
        {
            [JsonProperty("$type")]
            public string type { get; set; }
        }

        public class Line
        {
            [JsonProperty("$type")]
            public string id { get; set; }
            public string name { get; set; }
            public string uri { get; set; }
            public string type { get; set; }
            public Crowding crowding { get; set; }
            public string routeType { get; set; }
            public string status { get; set; }
        }

        public class LineGroup
        {
            [JsonProperty("$type")]
            public string type { get; set; }
            public string naptanIdReference { get; set; }
            public string stationAtcoCode { get; set; }
            public List<string> lineIdentifier { get; set; }
        }

        public class LineModeGroup
        {
            [JsonProperty("$type")]
            public string type { get; set; }
            public string modeName { get; set; }
            public List<string> lineIdentifier { get; set; }
        }

        public class StopPoint
        {
            [JsonProperty("$type")]
            public string type { get; set; }
            public string naptanId { get; set; }
            public string indicator { get; set; }
            public string stopLetter { get; set; }
            public List<string> modes { get; set; }
            public string icsCode { get; set; }
            public string stopType { get; set; }
            public string stationNaptan { get; set; }
            public List<Line> lines { get; set; }
            public List<LineGroup> lineGroup { get; set; }
            public List<LineModeGroup> lineModeGroups { get; set; }
            public bool status { get; set; }
            public string id { get; set; }
            public string commonName { get; set; }
            public double distance { get; set; }
            public string placeType { get; set; }
            public List<AdditionalProperty> additionalProperties { get; set; }
            public List<object> children { get; set; }
            public double lat { get; set; }
            public double lon { get; set; }
        }

    }
}

