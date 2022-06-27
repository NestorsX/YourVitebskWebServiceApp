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
    public class CafesControllerTests
    {
        private readonly Mock<ICafeRepository> _mockRepo;
        private readonly Mock<ICafeTypeRepository> _cafeTypeMockRepo;
        private readonly CafesController _controller;

        public CafesControllerTests()
        {
            _mockRepo = new Mock<ICafeRepository>();
            _cafeTypeMockRepo = new Mock<ICafeTypeRepository>();
            _controller = new CafesController(_mockRepo.Object, _cafeTypeMockRepo.Object);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.CafesGet))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.CafesCreate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.CafesUpdate))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.CafesDelete))).Returns(true);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<CafeViewModel>()
            {
                new CafeViewModel 
                { 
                    CafeId = 1,
                    CafeTypeId = 1,
                    CafeType = "CafeType1",
                    Title = "Title1",
                    Description = "Description1",
                    WorkingTime = "WorkingTime1",
                    Address = "Address1",
                    ExternalLink = "Link1"
                },
                new CafeViewModel
                {
                    CafeId = 2,
                    CafeTypeId = 2,
                    CafeType = "CafeType2",
                    Title = "Title2",
                    Description = "Description2",
                    WorkingTime = "WorkingTime2",
                    Address = "Address2",
                    ExternalLink = "Link2"
                }
            });

            _mockRepo.Setup(repo => repo.Get(2)).Returns(new Cafe
            {
                CafeId = 2,
                CafeTypeId = 2,
                Title = "Title2",
                Description = "Description2",
                WorkingTime = "WorkingTime2",
                Address = "Address2",
                ExternalLink = "Link2"
            });

            _cafeTypeMockRepo.Setup(repo => repo.Get()).Returns(new List<CafeType>()
            {
                new CafeType { CafeTypeId = 1, Name = "CafeType1" },
                new CafeType { CafeTypeId = 2, Name = "CafeType2" },
                new CafeType { CafeTypeId = 3, Name = "CafeType3" }
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
            var objects = Assert.IsType<CafeIndexViewModel>(viewResult.Model);
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
            var obj = new Cafe { };
            var result = _controller.Create(obj, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testObj = Assert.IsType<Cafe>(viewResult.Model);
            Assert.Equal(obj.Title, testObj.Title);
        }

        [Fact]
        public void Create_RedirectsToIndex()
        {
            var obj = new Cafe { };
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
            var newObj = new Cafe
            {
                CafeId = 2,
            };

            var result = _controller.Edit(newObj, null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testObj = Assert.IsType<Cafe>(viewResult.Model);
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
            var newObj = new Cafe
            {
                CafeId = 2,
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
