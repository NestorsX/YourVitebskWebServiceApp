using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.APIModels;

namespace YourVitebskWebServiceApp.APIControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NewsController : ControllerBase
    {
        private readonly IService<News> _newsService;

        public NewsController(IService<News> newsService)
        {
            _newsService = newsService;
        }

        // Gets all news
        [HttpGet("news/all")]
        public async Task<IEnumerable<News>> GetAll()
        {
            return await _newsService.GetAll();
        }

        // Gets news by id
        [HttpGet("news/{id}")]
        public async Task<ActionResult<ResponseModel>> Get(int id)
        {
            try
            {
                News news = await _newsService.GetById(id);
                return Ok(ResponseModel.CreateResponseWithContent(news));
            }
            catch (ArgumentException e)
            {
                return NotFound(ResponseModel.CreateResponseWithError(e.Message));
            }
        }
    }
}
