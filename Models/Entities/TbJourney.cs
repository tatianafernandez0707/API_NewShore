using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public partial class TbJourney
    {
        public TbJourney()
        {
            TbJourneyFlight = new HashSet<TbJourneyFlight>();
        }

        public int IdJourney { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Price { get; set; }

        public virtual ICollection<TbJourneyFlight> TbJourneyFlight { get; set; }
    }
}
