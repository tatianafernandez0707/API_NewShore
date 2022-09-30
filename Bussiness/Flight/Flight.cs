using Data_Access.Context;
using Data_Access.Interfaces;
using Models.Entities;
using Models.ReturnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Flight
{
    public class Flight : IFlight
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public Flight(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ReturnControl> SaveFlight(TbFlight tbFlight)
        {
            ReturnControl returnControl = new ReturnControl();

            try
            {
                _applicationDbContext.TbFlight.Add(tbFlight);

                var save = await _applicationDbContext.SaveChangesAsync();

                returnControl.Flag = true;
                returnControl.Message = "information saved successfully";
                returnControl.Data = tbFlight;

                return returnControl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
