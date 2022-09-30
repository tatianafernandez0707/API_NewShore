using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Models.ServiceConsumptionModels
{

    public class ServiceConsumption
    {
        public string departureStation { get; set; }
        public string arrivalStation { get; set; }
        public string flightCarrier { get; set; }
        public string flightNumber { get; set; }
        public int price { get; set; }
    }
}
