using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using YourVitebskWebServiceApp.APIModels;

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

        // Gets all users
        [HttpGet("all")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _usersService.GetAllUsers();
        }

        // Gets user by id
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel>> GetUser(int id)
        {
            User user = await _usersService.GetById(id);
            if (user == null)
            {
                return NotFound(ResponseModel.CreateResponseWithError("Пользователь не найден!"));
            }

            return Ok(ResponseModel.CreateResponseWithContent(user));
        }

        // Updates user
        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user)
        {
            try
            {
                await _usersService.Update(user);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(ResponseModel.CreateResponseWithError(e.Message));
            }
        }
    }
}
