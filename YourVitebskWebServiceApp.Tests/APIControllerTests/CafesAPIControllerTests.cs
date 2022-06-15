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
    public class CafesAPIControllerTests
    {
        private readonly Mock<IService<Cafe>> _mockRepo;
        private readonly CafesController _controller;

        public CafesAPIControllerTests()
        {
            _mockRepo = new Mock<IService<Cafe>>();
            _controller = new CafesController(_mockRepo.Object);
            IEnumerable<Cafe> cafes = new List<Cafe>()
            {
                new Cafe()
            };

            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(cafes);
            _mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync(cafes.First());
            _mockRepo.Setup(repo => repo.GetById(-1)).ThrowsAsync(new ArgumentException());
        }

        [Fact]
        public async void GetAll_ReturnsCorrectType()
        {
            var result = await _controller.GetAll(0,10);
            Assert.IsAssignableFrom<IEnumerable<Cafe>>(result);
        }

        [Fact]
        public async void GetAll_ReturnsExactNumberOfObjects()
        {
            var result = await _controller.GetAll(0, 10);
            var objects = Assert.IsAssignableFrom<IEnumerable<Cafe>>(result);
            Assert.Single(objects);
        }

        [Fact]
        public async void Get_ReturnsCorrectType()
        {
            var result = await _controller.Get(1);
            Assert.IsType<ActionResult<ResponseModel>>(result);
        }

        [Fact]
        public async void Get_ReturnsOkResult()
        {
            var result = await _controller.Get(1);
            Assert.IsType<ActionResult<ResponseModel>>(result);
            Assert.IsAssignableFrom<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void Get_ReturnsNotFoundResult()
        {
            var result = await _controller.Get(-1);
            Assert.IsType<ActionResult<ResponseModel>>(result);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result.Result);
        }
    }
}
