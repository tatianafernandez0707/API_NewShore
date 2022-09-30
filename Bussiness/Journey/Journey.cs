using Data_Access.Context;
using Data_Access.Interfaces;
using Models.Entities;
using Models.ModelsParameters;
using Models.ReturnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Journey
{
    public class Journey : IJourney
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public Journey(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<TbJourney> SaveJourney(TbJourney tbJourney)
        {
            try
            {
                _applicationDbContext.TbJourney.Add(tbJourney);

                await _applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return tbJourney;
        }

        public async Task<TbJourney> ConsultJourney(GetRouteParameters getRoute)
        {
            TbJourney tbJourney = new TbJourney();

            try
            {
                var consultRoute = _applicationDbContext.
                                        TbJourney.Where(x => x.Origin == getRoute.Origin
                                        && x.Destination == getRoute.Destination).ToList();

                if (consultRoute.Count > 0)
                {
                    for (int i = 0; i < consultRoute.Count; i++)
                    {
                        tbJourney.IdJourney = consultRoute[i].IdJourney;
                        tbJourney.Origin = consultRoute[i].Origin;
                    }
                }

                return tbJourney;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
