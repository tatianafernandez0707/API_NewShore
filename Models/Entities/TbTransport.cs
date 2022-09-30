using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public partial class TbTransport
    {
        public TbTransport()
        {
            TbFlight = new HashSet<TbFlight>();
        }

        public int IdTransport { get; set; }
        public string FlightCarrier { get; set; }
        public string FlightNumber { get; set; }

        public virtual ICollection<TbFlight> TbFlight { get; set; }
    }
}
