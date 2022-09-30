using Data_Access.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ModelsParameters;
using Models.ReturnModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_NewShore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetRouteController : ControllerBase
    {
        private readonly IGetRoute _getRoute;

        ReplySucess oReply = new ReplySucess();

        public GetRouteController(IGetRoute getRoute)
        {
            _getRoute = getRoute;
        }

        #region method that returns the json according to the condition
        // POST: api/[controller]/
        /// <summary>
        /// get route information
        /// </summary>
        /// <remarks>
        /// Method that returns the route according to parameters
        /// </remarks>
        /// <response code="400">Bad Request. entered a validation that gave incorrect</response>
        /// <response code="200">OK. json successfully consumed</response>
        [HttpPost]
        public async Task<ActionResult<string>> ConsumptionSearchMethod([FromBody] GetRouteParameters getRouteParameters)
        {
            //Llamar servicio
            var callCreateMethod = await _getRoute.CheckRoute(getRouteParameters);

            oReply.Data = callCreateMethod.Data;
            oReply.Ok = callCreateMethod.Ok;
            oReply.Message = callCreateMethod.Message;
            oReply.json = callCreateMethod.json;
            oReply.Status = callCreateMethod.Status;

            if (oReply.Status == 200)
            {
                return Ok(oReply.json);
            }
            else
            {
                if (oReply.Status == 404)
                {
                    return NotFound(oReply);
                }
                else
                {
                    return BadRequest(oReply);
                }
            }
        }
        #endregion
    }
}
