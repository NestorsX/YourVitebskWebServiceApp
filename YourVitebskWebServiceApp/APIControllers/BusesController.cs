using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using YourVitebskWebServiceApp.APIModels;

namespace YourVitebskWebServiceApp.APIControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusesController : ControllerBase
    {
        private readonly IBusService _busService;

        public BusesController(IBusService busService)
        {
            _busService = busService;
        }

        // Gets all bus numbers
        [HttpGet("buses")]
        public async Task<IEnumerable<Models.Bus>> GetAll()
        {
            return await _busService.GetAllBuses();
        }

        // Gets bus routes by bus id
        [HttpGet("routes/{busId}")]
        public async Task<ActionResult<ResponseModel>> GetRoutes(int busId)
        {
            try
            {
                IEnumerable<BusRoute> busRoutes = await _busService.GetBusRoutes(busId);
                return Ok(ResponseModel.CreateResponseWithContent(busRoutes));
            }
            catch (ArgumentException e)
            {
                return NotFound(ResponseModel.CreateResponseWithError(e.Message));
            }
        }

        // Gets shedule for specific bus stop
        [HttpPost("shedule")]
        public async Task<ActionResult<ResponseModel>> GetShedule(BusDTO busDTO)
        {
            try
            {
                BusShedule busShedule = await _busService.GetBusShedule(busDTO);
                return Ok(ResponseModel.CreateResponseWithContent(busShedule));
            }
            catch (ArgumentException e)
            {
                return NotFound(ResponseModel.CreateResponseWithError(e.Message));
            }
        }
    }
}
