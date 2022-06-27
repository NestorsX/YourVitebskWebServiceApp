using YourVitebskWebServiceApp.Controllers;
using YourVitebskWebServiceApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using YourVitebskWebServiceApp.ViewModels;
using System.Linq;
using YourVitebskWebServiceApp.Helpers.SortStates;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;

namespace YourVitebskWebServiceApp.Tests.ControllerTests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IRoleRepository> _roleMockRepo;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _roleMockRepo = new Mock<IRoleRepository>();
            _controller = new UsersController(_mockRepo.Object, _roleMockRepo.Object);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.UsersGet))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.UsersCreate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.UsersUpdate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.UsersDelete))).Returns(true);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<UserViewModel>()
            {
                new UserViewModel
                {
                    UserId = 1,
                    Email = "1@mail.com",
                    Password = "Password1",
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    PhoneNumber = "+375(29)333-57-34",
                    RoleId = 1,
                    IsVisible = true
                },
                new UserViewModel
                {
                    UserId = 2,
                    Email = "2@mail.com",
                    Password = "Password2",
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    PhoneNumber = "+375(29)333-47-34",
                    RoleId = 2,
                    IsVisible = false
                }
            });

            _mockRepo.Setup(repo => repo.Get(2)).Returns(new UserViewModel
            {
                UserId = 2,
                Email = "2@mail.com",
                Password = "Password2",
                FirstName = "FirstName2",
                LastName = "LastName2",
                PhoneNumber = "+375(29)333-47-34",
                RoleId = 2,
                IsVisible = false
            });
        }

        [Fact]
        public void Index_ReturnsView()
        {
            var result = _controller.Index(null, null, UserSortStates.UserIdAsc, 1);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ReturnsExactNumberOfObjects()
        {
            var result = _controller.Index(null, null, UserSortStates.UserIdAsc, 1);
            var viewResult = Assert.IsType<ViewResult>(result);
            var objects = Assert.IsType<UserIndexViewModel>(viewResult.Model);
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
            _controller.ModelState.AddModelError("Email", "Email уже используется");
            var obj = new UserViewModel
            {
                UserId = 0,
                Email = "2@mail.com",
                Password = "Password3",
                FirstName = "FirstName3",
                LastName = "LastName3",
                PhoneNumber = "+375(29)333-55-30",
                RoleId = 2,
                IsVisible = true
            };

            var result = _controller.Create(obj, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testObj = Assert.IsType<UserViewModel>(viewResult.Model);
            Assert.Equal(obj.Email, testObj.Email);
        }

        [Fact]
        public void Create_RedirectsToIndex()
        {
            var obj = new UserViewModel
            {
                UserId = 0,
                Email = "3@mail.com",
                Password = "Password3",
                FirstName = "FirstName3",
                LastName = "LastName3",
                PhoneNumber = "+375(29)333-55-30",
                RoleId = 2,
                IsVisible = true
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
            _controller.ModelState.AddModelError("Email", "Email уже используется");
            var newObj = new UserViewModel
            {
                UserId = 2,
                Email = "1@mail.com",
                Password = "Password3",
                FirstName = "FirstName3",
                LastName = "LastName3",
                PhoneNumber = "+375(29)333-55-30",
                RoleId = 2,
                IsVisible = true
            };

            var result = _controller.Edit(newObj, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testEmployee = Assert.IsType<UserViewModel>(viewResult.Model);
            Assert.Equal(newObj.Email, testEmployee.Email);
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
            var newObj = new UserViewModel
            {
                UserId = 2,
                Email = "3@mail.com",
                Password = "Password3",
                FirstName = "FirstName3",
                LastName = "LastName3",
                PhoneNumber = "+375(29)333-55-67",
                RoleId = 2,
                IsVisible = true
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
        public void Delete_ActionExecuted_RedirectsToIndexAction()
        {
            var result = _controller.Delete(2);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
