using YourVitebskWebServiceApp.Controllers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace YourVitebskWebServiceApp.Tests.ControllerTests
{
    public class PosterTypesControllerTests
    {
        private readonly Mock<IRepository<PosterType>> _mockRepo;
        private readonly PosterTypesController _controller;

        public PosterTypesControllerTests()
        {
            _mockRepo = new Mock<IRepository<PosterType>>();
            var optionsBuilder = new DbContextOptionsBuilder<YourVitebskDBContext>();
            var options = optionsBuilder.UseSqlServer(DBSettings.DBConnection).Options;
            _controller = new PosterTypesController(new YourVitebskDBContext(options), _mockRepo.Object);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<PosterType>()
            {
                new PosterType
                {
                    PosterTypeId = 1,
                    Name = "TestName1"
                },
                new PosterType
                {
                    PosterTypeId = 2,
                    Name = "TestName2"
                }
            });

            _mockRepo.Setup(repo => repo.Get(2)).Returns(new PosterType
            {
                PosterTypeId = 2,
                Name = "TestName2"
            });
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var result = _controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ReturnsExactNumberOfObjects()
        {
            var result = _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var objects = Assert.IsType<List<PosterType>>(viewResult.Model);
            Assert.Equal(2, objects.Count);
        }

        [Fact]
        public void Create_ReturnsView()
        {
            var result = _controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("Name", "Вид заведения уже существует");
            var obj = new PosterType
            {
                PosterTypeId = null,
                Name = "TestName1"
            };

            var result = _controller.Create(obj);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testObj = Assert.IsType<PosterType>(viewResult.Model);
            Assert.Equal(obj.Name, testObj.Name);
        }

        [Fact]
        public void Create_RedirectsToIndex()
        {
            var obj = new PosterType
            {
                PosterTypeId = null,
                Name = "TestName2"
            };

            var result = _controller.Create(obj);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_ReturnsView()
        {
            var result = _controller.Edit(2);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("Name", "Вид заведения уже существует");
            var newObj = new PosterType
            {
                PosterTypeId = 2,
                Name = "TestName1"
            };

            var result = _controller.Edit(newObj);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testEmployee = Assert.IsType<PosterType>(viewResult.Model);
            Assert.Equal(newObj.Name, testEmployee.Name);
        }

        [Fact]
        public void Edit_InvalidId_ReturnsNotFoundResult()
        {
            var result = _controller.Edit(-1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_RedirectsToIndex()
        {
            var newObj = new PosterType
            {
                PosterTypeId = 2,
                Name = "TestName3"
            };

            var result = _controller.Edit(newObj);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_ReturnsView()
        {
            var result = _controller.ConfirmDelete(2);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsNotFoundResult()
        {
            var result = _controller.ConfirmDelete(-1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ActionExecuted_RedirectsToIndexAction()
        {
            var result = _controller.Delete(2);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
