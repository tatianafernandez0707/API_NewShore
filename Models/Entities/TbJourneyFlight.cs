using System;
using System.Collections.Generic;

namespace Models.Entities
{
    public partial class TbJourneyFlight
    {
        public int IdJourneyFlight { get; set; }
        public int IdFlight { get; set; }
        public int IdJourney { get; set; }

        public virtual TbFlight IdFlightNavigation { get; set; }
        public virtual TbJourney IdJourneyNavigation { get; set; }
    }
}
