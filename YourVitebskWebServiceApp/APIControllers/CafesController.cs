using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using YourVitebskWebServiceApp.APIModels;
using System.Linq;

namespace YourVitebskWebServiceApp.APIControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CafesController : ControllerBase
    {
        private readonly IService<Cafe> _cafesService;

        public CafesController(IService<Cafe> cafesService)
        {
            _cafesService = cafesService;
        }

        // Gets all cafes
        [HttpGet("all")]
        public async Task<IEnumerable<Cafe>> GetAll(int offset, int count)
        {
            return (await _cafesService.GetAll()).Skip(offset).Take(count);
        }

        // Gets cafe by id
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel>> Get(int id)
        {
            try
            {
                Cafe cafe = await _cafesService.GetById(id);
                return Ok(ResponseModel.CreateResponseWithContent(cafe));
            }
            catch (ArgumentException e)
            {
                return NotFound(ResponseModel.CreateResponseWithError(e.Message));
            }
        }
    }
}
