using Models.Entities;
using Models.ReturnModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Interfaces
{
    public interface IJourneyFlight
    {
        Task<ReturnControl> SaveJourneyFlight(TbJourneyFlight tbJourneyFlight);
    }
}
