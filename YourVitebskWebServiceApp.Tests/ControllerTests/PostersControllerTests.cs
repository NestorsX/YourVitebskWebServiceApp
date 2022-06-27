using YourVitebskWebServiceApp.Controllers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using Xunit;
using YourVitebskWebServiceApp.ViewModels;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;
using System.Linq;

namespace YourVitebskWebServiceApp.Tests.ControllerTests
{
    public class PostersControllerTests
    {
        private readonly Mock<IPosterRepository> _mockRepo;
        private readonly Mock<IPosterTypeRepository> _posterTypeMockRepo;
        private readonly PostersController _controller;

        public PostersControllerTests()
        {
            _mockRepo = new Mock<IPosterRepository>();
            _posterTypeMockRepo = new Mock<IPosterTypeRepository>();
            _controller = new PostersController(_mockRepo.Object, _posterTypeMockRepo.Object);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.PostersGet))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.PostersCreate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.PostersUpdate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.PostersDelete))).Returns(true);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<PosterViewModel>()
            {
                new PosterViewModel
                {
                    PosterId = 1,
                    PosterTypeId = 1,
                    PosterType = "PosterType1",
                    Title = "Title1",
                    Description = "Description1",
                    Address = "Address1",
                    DateTime = "DateTime1",
                    ExternalLink = null
                },
                new PosterViewModel
                {
                    PosterId = 2,
                    PosterTypeId = 2,
                    PosterType = "PosterType2",
                    Title = "Title2",
                    Description = "Description2",
                    Address = "Address2",
                    DateTime = "DateTime2",
                    ExternalLink = null
                }
            });

            _mockRepo.Setup(repo => repo.Get(2)).Returns(new Poster
            {
                PosterId = 2,
                PosterTypeId = 2,
                Title = "Title2",
                Description = "Description2",
                Address = "Address2",
                DateTime = "DateTime2",
                ExternalLink = null
            });

            _posterTypeMockRepo.Setup(repo => repo.Get()).Returns(new List<PosterType>()
            {
                new PosterType { PosterTypeId = 1, Name = "PosterType1" },
                new PosterType { PosterTypeId = 2, Name = "PosterType2" },
                new PosterType { PosterTypeId = 3, Name = "PosterType3" }
            });
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var result = _controller.Index(null, null);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ReturnsExactNumberOfObjects()
        {
            var result = _controller.Index(null, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var objects = Assert.IsType<PosterIndexViewModel>(viewResult.Model);
            Assert.Equal(2, objects.Data.Count());
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
