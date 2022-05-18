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
    public class PostersController : ControllerBase
    {
        private readonly IService<Poster> _postersService;

        public PostersController(IService<Poster> postersService)
        {
            _postersService = postersService;
        }

        // Gets all posters
        [HttpGet("posters/all")]
        public async Task<IEnumerable<Poster>> GetAll()
        {
            return await _postersService.GetAll();
        }

        // Gets poster by id
        [HttpGet("posters/{id}")]
        public async Task<ActionResult<ResponseModel>> Get(int id)
        {
            try
            {
                Poster poster = await _postersService.GetById(id);
                return Ok(ResponseModel.CreateResponseWithContent(poster));
            }
            catch (ArgumentException e)
            {
                return NotFound(ResponseModel.CreateResponseWithError(e.Message));
            }
        }
    }
}
