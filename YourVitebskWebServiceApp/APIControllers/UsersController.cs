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

        // Gets user by email and password
        [HttpGet("auth/{email}/{password}")]
        public async Task<ActionResult<User>> AuthUser(string email, string password)
        {
            try
            {
                User user = await _usersService.GetByData(email, password);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Inserts user into db
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                User newUser = await _usersService.Create(user);
                return Ok(newUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Updates user
        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] User user)
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
