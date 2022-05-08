using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using YourVitebskWebServiceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace YourVitebskWebServiceApp.APIControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CafesController : ControllerBase
    {
        private readonly IService<CafeViewModel> _cafesService;

        public CafesController(IService<CafeViewModel> cafesService)
        {
            _cafesService = cafesService;
        }

        // Gets all cafes
        [HttpGet("cafes/all")]
        public async Task<ActionResult<IEnumerable<CafeViewModel>>> GetAll()
        {
            IEnumerable<CafeViewModel> cafes = await _cafesService.GetAll();
            if (cafes == null)
            {
                return NotFound();
            }

            return Ok(cafes);
        }

        // Gets cafe by id
        [HttpGet("cafes/{id}")]
        public async Task<ActionResult<CafeViewModel>> Get(int id)
        {
            CafeViewModel cafe = await _cafesService.GetById(id);
            if (cafe == null)
            {
                return NotFound();
            }

            return Ok(cafe);
        }
    }
}
