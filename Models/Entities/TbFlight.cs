using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public partial class TbFlight
    {
        public TbFlight()
        {
            TbJourneyFlight = new HashSet<TbJourneyFlight>();
        }

        public int IdFlight { get; set; }
        public int Price { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int IdTransport { get; set; }

        public virtual TbTransport IdTransportNavigation { get; set; }
        public virtual ICollection<TbJourneyFlight> TbJourneyFlight { get; set; }
    }
}
