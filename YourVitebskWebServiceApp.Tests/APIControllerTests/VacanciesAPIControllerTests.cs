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
    public class VacanciesAPIControllerTests
    {
        private readonly Mock<IService<Vacancy>> _mockRepo;
        private readonly VacanciesController _controller;

        public VacanciesAPIControllerTests()
        {
            _mockRepo = new Mock<IService<Vacancy>>();
            _controller = new VacanciesController(_mockRepo.Object);
            IEnumerable<Vacancy> vacancies = new List<Vacancy>()
            {
                new Vacancy()
            };

            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(vacancies);
            _mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync(vacancies.First());
            _mockRepo.Setup(repo => repo.GetById(-1)).ThrowsAsync(new ArgumentException());
        }

        [Fact]
        public async void GetAll_ReturnsCorrectType()
        {
            var result = await _controller.GetAll(0, 10);
            Assert.IsAssignableFrom<IEnumerable<Vacancy>>(result);
        }

        [Fact]
        public async void GetAll_ReturnsExactNumberOfObjects()
        {
            var result = await _controller.GetAll(0, 10);
            var objects = Assert.IsAssignableFrom<IEnumerable<Vacancy>>(result);
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
