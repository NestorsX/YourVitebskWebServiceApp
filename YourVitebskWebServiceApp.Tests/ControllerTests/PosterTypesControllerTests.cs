using YourVitebskWebServiceApp.Controllers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;
using System.Linq;

namespace YourVitebskWebServiceApp.Tests.ControllerTests
{
    public class PosterTypesControllerTests
    {
        private readonly Mock<IPosterTypeRepository> _mockRepo;
        private readonly PosterTypesController _controller;

        public PosterTypesControllerTests()
        {
            _mockRepo = new Mock<IPosterTypeRepository>();
            _controller = new PosterTypesController(_mockRepo.Object); 
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.PostersGet))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.PostersCreate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.PostersUpdate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.PostersDelete))).Returns(true);
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
            var result = _controller.Index(null);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ReturnsExactNumberOfObjects()
        {
            var result = _controller.Index(null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var objects = Assert.IsType<PosterTypeIndexViewModel>(viewResult.Model);
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
                Name = "TestName3"
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
