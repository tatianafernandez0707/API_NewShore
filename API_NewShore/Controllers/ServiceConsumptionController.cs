using Data_Access.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.ReturnModels;
using System.Threading.Tasks;

namespace API_NewShore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceConsumptionController : ControllerBase
    {
        private readonly IServiceConsumption _serviceConsumption;

        ReplySucess oReply = new ReplySucess();

        public ServiceConsumptionController(IServiceConsumption serviceConsumption)
        {
            _serviceConsumption = serviceConsumption;
        }

        #region Consumo del servicio del punto numero 2
        // GET: api/[controller]/ConsumeService/2
        /// <summary>
        /// Consumption of the service, the information that was consulted is returned
        /// </summary>
        /// <remarks>
        /// Method that returns the variables configured in the error control class
        /// </remarks>
        /// <response code="400">errors while evaluating logic</response>
        /// <response code="200">return of the correct information, the service is consumed and the information is presented</response>
        [HttpGet]
        [Route("ConsumeService/{level}")]
        public async Task<IActionResult> GetConsumeService([FromRoute] int  level)
        {
            //service call where the logic is executed
            var callMethond = await _serviceConsumption.PointOneServiceConsumption(level);

            oReply.Data = callMethond.Data;
            oReply.Ok = callMethond.Flag;
            oReply.Message = callMethond.Message;

            if (callMethond.Status == 200)
            {
                return Ok(oReply);

            }
            else
            {
                return BadRequest(oReply);
            }
        }
        #endregion
    }
}
