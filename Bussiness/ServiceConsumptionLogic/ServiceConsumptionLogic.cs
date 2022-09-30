using Data_Access.Interfaces;
using Microsoft.Extensions.Configuration;
using Models.ReturnModels;
using Models.ServiceConsumptionModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.ServiceConsumptionLogic
{
    public class ServiceConsumptionLogic : IServiceConsumption
    {
        private readonly IConfiguration _configuration;
        public ServiceConsumptionLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ReturnControl> PointOneServiceConsumption(int level)
        {
            ReturnControl returnControl = new ReturnControl();

            try 
            {
                HttpClient client = new HttpClient();

                var urlBase = _configuration["baseUrl"];

                client.BaseAddress = new Uri(urlBase);

                var route = urlBase + level;

                var response = await client.GetAsync(route);

                if (response.IsSuccessStatusCode)
                {
                    var jsonReading = await response.Content.ReadAsStringAsync();

                    jsonReading = jsonReading.ToUpper();

                    var result = JsonConvert.DeserializeObject<List<ServiceConsumption>>(jsonReading);

                    if (result.Count > 0)
                    {
                        returnControl.Data = result;
                        returnControl.Flag = true;
                        returnControl.Message = "json successfully consumed";
                    }
                    else
                    {
                        returnControl.Data = result;
                        returnControl.Flag = false;
                        returnControl.Message = "Error deserializing json";
                    }
                }
                else
                {
                    returnControl.Flag = false;
                    returnControl.Message = "Json consumption error";
                    returnControl.Status = 400;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return returnControl;
        }
    }
}
