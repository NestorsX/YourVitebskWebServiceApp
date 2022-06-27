using YourVitebskWebServiceApp.Controllers;
using YourVitebskWebServiceApp.Interfaces;
using YourVitebskWebServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using YourVitebskWebServiceApp.ViewModels;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;
using System.Linq;

namespace YourVitebskWebServiceApp.Tests.ControllerTests
{
    public class RolesControllerTests
    {
        private readonly Mock<IRoleRepository> _mockRepo;
        private readonly RolesController _controller;

        public RolesControllerTests()
        {
            _mockRepo = new Mock<IRoleRepository>();
            _controller = new RolesController(_mockRepo.Object);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.RolesGet))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.RolesCreate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.RolesUpdate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.RolesDelete))).Returns(true);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<RoleViewModel>()
            {
                new RoleViewModel
                {
                    RoleId = 1,
                    Name = "TestName1",
                },
                new RoleViewModel
                {
                    RoleId = 2,
                    Name = "TestName2",
                },
                new RoleViewModel
                {
                    RoleId = 3,
                    Name = "TestName3",
                }
            });

            _mockRepo.Setup(repo => repo.GetForEdit(3)).Returns(new RoleDTOViewModel
            {
                RoleId = 3,
                Name = "TestName3",
            });

            _mockRepo.Setup(repo => repo.GetForView(3)).Returns(new RoleViewModel
            {
                RoleId = 3,
                Name = "TestName3",
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
            var objects = Assert.IsType<RoleIndexViewModel>(viewResult.Model);
            Assert.Equal(3, objects.Data.Count());
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
            _controller.ModelState.AddModelError("Name", "Роль уже существует");
            var obj = new RoleDTOViewModel
            {
                RoleId = null,
                Name = "TestName1",
            };

            var result = _controller.Create(obj);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testObj = Assert.IsType<RoleDTOViewModel>(viewResult.Model);
            Assert.Equal(obj.Name, testObj.Name);
        }

        [Fact]
        public void Create_RedirectsToIndex()
        {
            var obj = new RoleDTOViewModel
            {
                RoleId = null,
                Name = "TestName",
            };

            var result = _controller.Create(obj);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Edit_ReturnsView()
        {
            var result = _controller.Edit(3);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("Name", "Роль уже существует");
            var newObj = new RoleDTOViewModel
            {
                RoleId = 3,
                Name = "TestName4",
            };

            var result = _controller.Edit(newObj);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testEmployee = Assert.IsType<RoleDTOViewModel>(viewResult.Model);
            Assert.Equal(newObj.Name, testEmployee.Name);
        }

        [Fact]
        public void Edit_InvalidId_ReturnsNotFoundResult()
        {
            var result = _controller.Edit(-1);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Edit_RedirectsToIndex()
        {
            var newObj = new RoleDTOViewModel
            {
                RoleId = 3,
                Name = "TestName4",
            };

            var result = _controller.Edit(newObj);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void Delete_ReturnsView()
        {
            var result = _controller.ConfirmDelete(3);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsNotFoundResult()
        {
            var result = _controller.ConfirmDelete(-1);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Delete_ActionExecuted_RedirectsToIndexAction()
        {
            var result = _controller.Delete(3);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
