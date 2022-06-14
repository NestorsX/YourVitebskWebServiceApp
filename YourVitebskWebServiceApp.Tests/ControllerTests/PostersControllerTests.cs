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
    public class PostersControllerTests
    {
        private readonly Mock<IImageRepository<Poster>> _mockRepo;
        private readonly PostersController _controller;

        public PostersControllerTests()
        {
            _mockRepo = new Mock<IImageRepository<Poster>>();
            var optionsBuilder = new DbContextOptionsBuilder<YourVitebskDBContext>();
            var options = optionsBuilder.UseSqlServer(DBSettings.DBConnection).Options;
            _controller = new PostersController(new YourVitebskDBContext(options), _mockRepo.Object);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<Poster>()
            {
                new Poster
                {
                    PosterId = 1,
                    PosterTypeId = 1,
                    Title = "TestTitle1",
                    Description = "TestDescription1",
                    Address = "TestAddress1",
                    DateTime = "TestDateTime1",
                    ExternalLink = null
                },
                new Poster
                {
                    PosterId = 2,
                    PosterTypeId = 2,
                    Title = "TestTitle2",
                    Description = "TestDescription2",
                    Address = "TestAddress2",
                    DateTime = "TestDateTime2",
                    ExternalLink = null
                }
            });

            _mockRepo.Setup(repo => repo.Get(2)).Returns(new Poster
            {
                PosterId = 2,
                PosterTypeId = 2,
                Title = "TestTitle2",
                Description = "TestDescription2",
                Address = "TestAddress2",
                DateTime = "TestDateTime2",
                ExternalLink = null
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
            var objects = Assert.IsType<List<Poster>>(viewResult.Model);
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
            _controller.ModelState.AddModelError("Title", "Заведение с таким именем уже используется");
            var obj = new Poster
            {
                PosterId = 0,
                PosterTypeId = 1,
                Title = "TestTitle1",
                Description = "TestDescription1",
                Address = "TestAddress1",
                DateTime = "TestDateTime1",
                ExternalLink = null
            };

            var result = _controller.Create(obj, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testObj = Assert.IsType<Poster>(viewResult.Model);
            Assert.Equal(obj.Title, testObj.Title);
        }

        [Fact]
        public void Create_RedirectsToIndex()
        {
            var obj = new Poster
            {
                PosterId = 0,
                PosterTypeId = 1,
                Title = "TestTitle3",
                Description = "TestDescription3",
                Address = "TestAddress3",
                DateTime = "TestDateTime3",
                ExternalLink = null
            };

            var result = _controller.Create(obj, null);
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
            _controller.ModelState.AddModelError("Title", "Заведение с таким именем уже используется");
            var newObj = new Poster
            {
                PosterId = 2,
                PosterTypeId = 1,
                Title = "TestTitle1",
                Description = "TestDescription2",
                Address = "TestAddress2",
                DateTime = "TestDateTime2",
                ExternalLink = null
            };

            var result = _controller.Edit(newObj, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testObj = Assert.IsType<Poster>(viewResult.Model);
            Assert.Equal(newObj.Title, testObj.Title);
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
            var newObj = new Poster
            {
                PosterId = 2,
                PosterTypeId = 1,
                Title = "TestTitle3",
                Description = "TestDescription2",
                Address = "TestAddress2",
                DateTime = "TestDateTime2",
                ExternalLink = null
            };

            var result = _controller.Edit(newObj, null);
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
        public void Delete_RedirectsToIndex()
        {
            var result = _controller.Delete(2);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
