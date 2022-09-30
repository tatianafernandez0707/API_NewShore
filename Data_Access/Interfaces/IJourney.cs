using Models.Entities;
using Models.ModelsParameters;
using Models.ReturnModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Interfaces
{
    public interface IJourney
    {
        Task<TbJourney> SaveJourney(TbJourney tbJourney);
        Task<TbJourney> ConsultJourney(GetRouteParameters getRoute);
    }
}
