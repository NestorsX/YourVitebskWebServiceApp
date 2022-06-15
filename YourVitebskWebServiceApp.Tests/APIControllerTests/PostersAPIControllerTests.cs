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
    public class PostersAPIControllerTests
    {
        private readonly Mock<IService<Poster>> _mockRepo;
        private readonly PostersController _controller;

        public PostersAPIControllerTests()
        {
            _mockRepo = new Mock<IService<Poster>>();
            _controller = new PostersController(_mockRepo.Object);
            IEnumerable<Poster> posters = new List<Poster>()
            {
                new Poster()
            };

            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(posters);
            _mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync(posters.First());
            _mockRepo.Setup(repo => repo.GetById(-1)).ThrowsAsync(new ArgumentException());
        }

        [Fact]
        public async void GetAll_ReturnsCorrectType()
        {
            var result = await _controller.GetAll(0, 10);
            Assert.IsAssignableFrom<IEnumerable<Poster>>(result);
        }

        [Fact]
        public async void GetAll_ReturnsExactNumberOfObjects()
        {
            var result = await _controller.GetAll(0, 10);
            var objects = Assert.IsAssignableFrom<IEnumerable<Poster>>(result);
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
