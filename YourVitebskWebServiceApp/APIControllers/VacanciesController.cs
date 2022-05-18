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
    public class VacanciesController : ControllerBase
    {
        private readonly IService<Vacancy> _vacanciesService;

        public VacanciesController(IService<Vacancy> vacanciesService)
        {
            _vacanciesService = vacanciesService;
        }

        // Gets all vacancies
        [HttpGet("vacancies/all")]
        public async Task<IEnumerable<Vacancy>> GetAll()
        {
            return await _vacanciesService.GetAll();
        }

        // Gets vacancy by id
        [HttpGet("vacancies/{id}")]
        public async Task<ActionResult<ResponseModel>> Get(int id)
        {
            try
            {
                Vacancy vacancy = await _vacanciesService.GetById(id);
                return Ok(ResponseModel.CreateResponseWithContent(vacancy));
            }
            catch (ArgumentException e)
            {
                return NotFound(ResponseModel.CreateResponseWithError(e.Message));
            }
        }
    }
}
