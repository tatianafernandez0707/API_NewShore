using Data_Access.Context;
using Data_Access.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Entities;
using Models.ModelsParameters;
using Models.ReturnModels;
using Models.ServiceConsumptionModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Bussiness.GetRoute
{
    public class GetRoute : IGetRoute
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IServiceConsumption _serviceConsumption;
        private readonly IFlight _flight;
        private readonly IJourney _journey;
        private readonly ITransport _transport;
        private readonly IJourneyFlight _journeyFlight;
        public GetRoute(ApplicationDbContext applicationDbContext,
            IServiceConsumption serviceConsumption, 
            IFlight flight,
            IJourney journey,
            ITransport transport,
            IJourneyFlight journeyFlight)
        {
            _applicationDbContext = applicationDbContext;
            _serviceConsumption = serviceConsumption;
            _flight = flight;
            _journey = journey;
            _transport = transport;
            _journeyFlight = journeyFlight;
        }

        ReplySucess returnControl = new ReplySucess();

        public async Task<ReplySucess> CheckRoute(GetRouteParameters getRouteParameters)
        {
            ReplySucess returnControl = new ReplySucess();

            try
            {
                //Level 2

                var consumeService = await _serviceConsumption.PointOneServiceConsumption(2);

                if (consumeService.Flag == true)
                {
                    var consultRoute = _journey.ConsultJourney(getRouteParameters);

                    if (consultRoute.Result.IdJourney != 0)
                    {
                        var callMethodJson = CheckRoutePlanning(consultRoute.Result.IdJourney);

                        returnControl.Ok = callMethodJson.Result.Ok;
                        returnControl.Message = callMethodJson.Result.Message;
                        returnControl.Status = callMethodJson.Result.Status;
                        returnControl.json = callMethodJson.Result.json;

                        return returnControl;
                    }
                    else
                    {
                        List<ServiceConsumption> listData = new List<ServiceConsumption>();

                        listData = (List<ServiceConsumption>)consumeService.Data;

                        if (listData.Count > 0)
                        {
                            DataTable dataTable = new DataTable
                            {
                                TableName = "TB_GetRoute"
                            };

                            dataTable.Columns.Add("departureStation", typeof(string));
                            dataTable.Columns.Add("arrivalStation", typeof(string));
                            dataTable.Columns.Add("flightCarrier", typeof(string));
                            dataTable.Columns.Add("flightNumber", typeof(string));
                            dataTable.Columns.Add("price", typeof(int));

                            foreach (var dataJson in listData)
                            {
                                DataRow _instance = dataTable.NewRow();
                                _instance["departureStation"] = dataJson.departureStation;
                                _instance["arrivalStation"] = dataJson.arrivalStation;
                                _instance["flightCarrier"] = dataJson.flightCarrier;
                                _instance["flightNumber"] = dataJson.flightNumber;
                                _instance["price"] = dataJson.price;
                                dataTable.Rows.Add(_instance);
                            }

                            if (dataTable.Rows.Count > 0)
                            {
                                var dataOrigin = dataTable.AsEnumerable().Where(p => p.Field<string>(0) == getRouteParameters.Origin).ToList();

                                if (dataOrigin.Count > 0)
                                {
                                    TbJourney tbJourney = new TbJourney();

                                    for (int i = 0; i < dataOrigin.Count; i++)
                                    {
                                        string origin = (string)dataOrigin[i].ItemArray[0];
                                        string destiny = (string)dataOrigin[i].ItemArray[1];
                                        string flightCarrier = (string)dataOrigin[i].ItemArray[2];
                                        string flightNumber = (string)dataOrigin[i].ItemArray[3];
                                        int price = (int)dataOrigin[i].ItemArray[4];

                                        var dataRoute = dataTable.AsEnumerable().Where(p => p.Field<string>(0) == destiny && p.Field<string>(1) == getRouteParameters.Destination).ToList();

                                        if (dataRoute.Count > 0)
                                        {
                                            tbJourney.Origin = getRouteParameters.Origin;
                                            tbJourney.Destination = getRouteParameters.Destination;

                                            var saveJourney = _journey.SaveJourney(tbJourney);

                                            TbTransport tbTransport1 = new TbTransport
                                            {
                                                FlightCarrier = flightCarrier,
                                                FlightNumber = flightNumber
                                            };

                                            var callMethondSaveTransport1 = _transport.SaveTransport(tbTransport1);

                                            TbFlight tbFlight1 = new TbFlight
                                            {
                                                IdTransport = callMethondSaveTransport1.Result.IdTransport,
                                                Origin = origin,
                                                Destination = destiny,
                                                Price = price
                                            };

                                            await _flight.SaveFlight(tbFlight1);

                                            TbJourneyFlight tbJourneyFlight1 = new TbJourneyFlight
                                            {
                                                IdFlight = tbFlight1.IdFlight,
                                                IdJourney = tbJourney.IdJourney
                                            };

                                            await _journeyFlight.SaveJourneyFlight(tbJourneyFlight1);

                                            if (saveJourney.Result.IdJourney != 0)
                                            {
                                                bool Flag = false;
                                                for (int d = 0; d < dataRoute.Count; d++)
                                                {
                                                    string originRoute = (string)dataRoute[d].ItemArray[0];
                                                    string destinyRoute = (string)dataRoute[d].ItemArray[1];
                                                    string flightCarrierRoute = (string)dataRoute[d].ItemArray[2];
                                                    string flightNumberRoute = (string)dataRoute[d].ItemArray[3];
                                                    int priceRoute = (int)dataRoute[d].ItemArray[4];

                                                    TbTransport tbTransport = new TbTransport
                                                    {
                                                        FlightCarrier = flightCarrierRoute,
                                                        FlightNumber = flightNumberRoute
                                                    };

                                                    var callMethondSaveTransport = _transport.SaveTransport(tbTransport);

                                                    if (callMethondSaveTransport.Result.IdTransport != 0)
                                                    {
                                                        TbFlight tbFlight = new TbFlight
                                                        {
                                                            IdTransport = callMethondSaveTransport.Result.IdTransport,
                                                            Origin = originRoute,
                                                            Destination = destinyRoute,
                                                            Price = priceRoute
                                                        };

                                                        await _flight.SaveFlight(tbFlight);

                                                        Flag = true;

                                                        TbJourneyFlight tbJourneyFlight = new TbJourneyFlight
                                                        {
                                                            IdFlight = tbFlight.IdFlight,
                                                            IdJourney = tbJourney.IdJourney
                                                        };

                                                        await _journeyFlight.SaveJourneyFlight(tbJourneyFlight);
                                                    }

                                                    if (d == dataRoute.Count -1)
                                                    {
                                                        if (Flag == true)
                                                        {
                                                            var callMethodJson = CheckRoutePlanning(tbJourney.IdJourney);

                                                            returnControl.Ok = callMethodJson.Result.Ok;
                                                            returnControl.Message = callMethodJson.Result.Message;
                                                            returnControl.Status = callMethodJson.Result.Status;
                                                            returnControl.json = callMethodJson.Result.json;

                                                            return returnControl;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                returnControl.Ok = false;
                                                returnControl.Message = "Error saving trip";
                                                returnControl.Status = 400;
                                            }
                                        }
                                        else
                                        {
                                            returnControl.Ok = false;
                                            returnControl.Message = "no information was found regarding the destination";
                                            returnControl.Status = 404;
                                        }
                                    }
                                }
                                else
                                {
                                    returnControl.Ok = false;
                                    returnControl.Message = "origin not found in json object";
                                    returnControl.Status = 404;
                                }
                            }
                            else
                            {
                                returnControl.Ok = false;
                                returnControl.Message = "The data object list is empty";
                                returnControl.Status = 400;
                            }
                        }
                    }
                }
                else
                {
                    returnControl.Ok = false;
                    returnControl.Message = "The data object list is empty";
                    returnControl.Status = 400;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return returnControl;
        }

        #region consult information of the previously loaded route
        public async Task<ReplySucess> CheckRoutePlanning(int idJourney)
        {
            try
            {
                SqlConnection connection = (SqlConnection)_applicationDbContext.Database.GetDbConnection();

                await connection.OpenAsync();

                string consult = "SELECT '' AS Journey, journey.Origin, journey.Destination, journey.Price, " +
                                " flight.Origin, flight.Destination, flight.Price, " +
                                " transport.FlightCarrier, transport.FlightNumber " +
                                " FROM TB_JourneyFlight journeyFlight " +
                                " JOIN TB_Flight flight " +
                                " ON journeyFlight.IdFlight = flight.IdFlight " +
                                " JOIN TB_Journey journey " +
                                " ON journeyFlight.IdJourney = journey.IdJourney " +
                                " JOIN TB_Transport transport " +
                                " ON flight.IdTransport = transport.IdTransport " +
                                " WHERE journey.IdJourney = " + idJourney +
                                " FOR JSON AUTO ";

                SqlCommand invokeQuery = new SqlCommand(consult, connection);

                SqlDataReader runQuery = await invokeQuery.ExecuteReaderAsync();

                string json = "";
                while (await runQuery.ReadAsync())
                {
                    json = runQuery.GetString(0);
                }

                if (json != "")
                {
                    returnControl.Ok = true;
                    returnControl.Message = "Json armed successfully";
                    returnControl.Status = 200;
                    returnControl.json = json;
                }
                else
                {
                    returnControl.Ok = false;
                    returnControl.Status = 404;
                    returnControl.Message = "no information found / error in json return";
                    returnControl.json = "[]";

                }

                await runQuery.CloseAsync();

                return returnControl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        #endregion
    }
}
