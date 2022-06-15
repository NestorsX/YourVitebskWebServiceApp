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
    public class NewsAPIControllerTests
    {
        private readonly Mock<IService<News>> _mockRepo;
        private readonly NewsController _controller;

        public NewsAPIControllerTests()
        {
            _mockRepo = new Mock<IService<News>>();
            _controller = new NewsController(_mockRepo.Object);
            IEnumerable<News> news = new List<News>()
            {
                new News()
            };

            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(news);
            _mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync(news.First());
            _mockRepo.Setup(repo => repo.GetById(-1)).ThrowsAsync(new ArgumentException());
        }

        [Fact]
        public async void GetAll_ReturnsCorrectType()
        {
            var result = await _controller.GetAll(0, 10);
            Assert.IsAssignableFrom<IEnumerable<News>>(result);
        }

        [Fact]
        public async void GetAll_ReturnsExactNumberOfObjects()
        {
            var result = await _controller.GetAll(0, 10);
            var objects = Assert.IsAssignableFrom<IEnumerable<News>>(result);
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
