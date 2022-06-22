using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using YourVitebskWebServiceApp.APIModels;
using System;

namespace YourVitebskWebServiceApp.APIControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // Gets all users except the caller
        [HttpGet("all/{id}")]
        public async Task<IEnumerable<UsersListItem>> GetAllUsers(int id)
        {
            return await _usersService.GetAllUsers(id);
        }

        // Gets comments count by user id
        [HttpGet("commentscount/{id}")]
        public async Task<ActionResult<ResponseModel>> GetCommentsCount(int id)
        {
            try
            {
                var result = await _usersService.GetCommentsCount(id);
                return Ok(ResponseModel.CreateResponseWithContent(result));
            }
            catch (ArgumentException e)
            {
                return NotFound(ResponseModel.CreateResponseWithError(e.Message));
            }

        }
    }
}
