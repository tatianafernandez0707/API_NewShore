using Models.ModelsParameters;
using Models.ReturnModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Interfaces
{
    public interface IGetRoute
    {
        Task<ReplySucess> CheckRoute(GetRouteParameters getRouteParameters);
        Task<ReplySucess> CheckRoutePlanning(int idJourney);
    }
}
