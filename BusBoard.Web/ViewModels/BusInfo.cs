using System.Collections.Generic;

namespace BusBoard.Web.ViewModels
{
    public class BusInfo
    {
        public BusInfo(string postCode, List<List<string>> buses)
        {
            PostCode = postCode;
            Buses = buses;
        }

        public string PostCode { get; set; }
        public List<List<string>> Buses { get; set; }
    }
}