using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using System;

namespace YourVitebskWebServiceApp.APIControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // Gets all users
        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _usersService.GetAllUsers();
        }

        // Gets user by id
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User user = await _usersService.GetById(id);
            if (user == null)
            {
                return NotFound("Пользователь не найден!");
            }

            return Ok(user);
        }

        // Updates user
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                await _usersService.Update(user);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
