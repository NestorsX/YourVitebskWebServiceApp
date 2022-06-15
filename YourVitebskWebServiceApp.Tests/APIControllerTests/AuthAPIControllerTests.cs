using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using YourVitebskWebServiceApp.APIControllers;
using YourVitebskWebServiceApp.APIModels;
using System.Linq;
using System;

namespace YourVitebskWebServiceApp.Tests.APIControllerTests
{
    public class AuthAPIControllerTests
    {
        private readonly Mock<IAuthService> _mockRepo;
        private readonly AuthController _controller;

        public AuthAPIControllerTests()
        {
            _mockRepo = new Mock<IAuthService>();
            _controller = new AuthController(_mockRepo.Object);
        }

        [Fact]
        public async void Login_ReturnsCorrectType()
        {
            var result = await _controller.Login(new UserLoginDTO());
            Assert.IsType<ActionResult<ResponseModel>>(result);
        }

        [Fact]
        public async void Register_ReturnsCorrectType()
        {
            var result = await _controller.Register(new UserRegisterDTO());
            Assert.IsType<ActionResult<ResponseModel>>(result);
        }
    }
}
