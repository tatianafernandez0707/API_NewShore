using Data_Access.Context;
using Data_Access.Interfaces;
using Models.Entities;
using Models.ReturnModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.JourneyFlight
{
    public class JourneyFlight : IJourneyFlight
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public JourneyFlight(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ReturnControl> SaveJourneyFlight(TbJourneyFlight tbJourneyFlight)
        {
            ReturnControl returnControl = new ReturnControl();

            try
            {
                _applicationDbContext.TbJourneyFlight.Add(tbJourneyFlight);

                await _applicationDbContext.SaveChangesAsync();

                returnControl.Flag = true;
                returnControl.Message = "information saved successfully";
                returnControl.Data = tbJourneyFlight;

                return returnControl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
