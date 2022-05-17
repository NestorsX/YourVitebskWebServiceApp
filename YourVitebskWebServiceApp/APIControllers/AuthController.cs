using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using System;
using YourVitebskWebServiceApp.APIModels;

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

        // Register user
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserRegisterDTO user)
        {
            if (string.IsNullOrEmpty(user.Email) 
                || string.IsNullOrEmpty(user.Password) 
                || string.IsNullOrEmpty(user.FirstName) 
                || string.IsNullOrEmpty(user.LastName))
            {
                return BadRequest(ResponseModel.CreateResponseWithError("Заполните все обязательные поля"));
            }

            try
            {
                string token = await _authService.Register(user);
                return Ok(ResponseModel.CreateResponseWithContent(token));
            }
            catch (ArgumentException e)
            {
                return BadRequest(ResponseModel.CreateResponseWithError(e.Message));
            }
        }

        // Authenticate user
        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel>> Login(UserLoginDTO user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(ResponseModel.CreateResponseWithError("Введите email"));
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(ResponseModel.CreateResponseWithError("Введите пароль"));
            }

            try
            {
                string token = await _authService.Login(user);
                return Ok(ResponseModel.CreateResponseWithContent(token));
            }
            catch (ArgumentException e)
            {
                return BadRequest(ResponseModel.CreateResponseWithError(e.Message));
            }
        }
    }
}
