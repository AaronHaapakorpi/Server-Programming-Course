
using System.Collections.Generic;

namespace BikeApiTest{

public class Station
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string BikesAvailable { get; set; }
        public string SpacesAvailable { get; set; }
        public string AllowDropoff { get; set; }
        public string AsFloatingBike { get; set; }
        public string IsCarStation { get; set; }
        public string State { get; set; }
        public List<string> Networks { get; set; }
        public string RealTimeData { get; set; }
    
    }

    public class BikeRentalStationList
    {
        public List<Station> stations {get;set;}

    }


}