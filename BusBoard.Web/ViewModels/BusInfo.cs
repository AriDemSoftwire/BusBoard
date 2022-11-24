using System.Collections.Generic;

namespace BusBoard.Web.ViewModels
{
    public class BusInfo
    {
        public BusInfo(string postCode, List<StopPoints> listOfStopsWithBuses)
        {
            PostCode = postCode;
            ListOfStopsWithBuses = listOfStopsWithBuses;
        }

        public string PostCode { get; set; }
        public List<StopPoints> ListOfStopsWithBuses { get; set; }
    }
}