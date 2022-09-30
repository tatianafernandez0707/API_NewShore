using Data_Access.Context;
using Data_Access.Interfaces;
using Models.Entities;
using Models.ReturnModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Transport
{
    public class Transport : ITransport
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public Transport(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<TbTransport> SaveTransport(TbTransport tbTransport)
        {

            try
            {
                _applicationDbContext.TbTransport.Add(tbTransport);

                await _applicationDbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return tbTransport;
        }
    }
}
