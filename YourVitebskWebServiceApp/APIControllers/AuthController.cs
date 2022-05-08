using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.Models;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using System;

namespace YourVitebskWebServiceApp.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserDTO user)
        {
            if (string.IsNullOrEmpty(user.Email) 
                || string.IsNullOrEmpty(user.Password) 
                || string.IsNullOrEmpty(user.FirstName) 
                || string.IsNullOrEmpty(user.LastName))
            {
                return BadRequest("Заполните все обязательные поля");
            }

            try
            {
                string token = await _authService.Register(user);
                return Ok(token);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("Введите email");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Введите пароль");
            }

            try
            {
                string token = await _authService.Login(user);
                return Ok(token);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
