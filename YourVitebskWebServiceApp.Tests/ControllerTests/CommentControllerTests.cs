using YourVitebskWebServiceApp.Controllers;
using YourVitebskWebServiceApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using YourVitebskWebServiceApp.ViewModels;
using YourVitebskWebServiceApp.ViewModels.IndexViewModels;
using System.Linq;

namespace YourVitebskWebServiceApp.Tests.ControllerTests
{
    public class CommentControllerTests
    {
        private readonly Mock<ICommentRepository> _mockRepo;
        private readonly CommentsController _controller;

        public CommentControllerTests()
        {
            _mockRepo = new Mock<ICommentRepository>();
            _controller = new CommentsController(_mockRepo.Object);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.CommentsGet))).Returns(true);
            _mockRepo.Setup(repo => repo.CheckRolePermission(nameof(Helpers.RolePermission.CommentsDelete))).Returns(true);
            _mockRepo.Setup(repo => repo.Get()).Returns(new List<CommentViewModel>()
            {
                new CommentViewModel(),
                new CommentViewModel(),
                new CommentViewModel(),
            });

            _mockRepo.Setup(repo => repo.Get(3)).Returns(new CommentViewModel());
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
            var objects = Assert.IsType<CommentIndexViewModel>(viewResult.Model);
            Assert.Equal(3, objects.Data.Count());
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
            Assert.IsType<NotFoundResult>(result);
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
