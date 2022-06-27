using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using YourVitebskWebServiceApp.APIControllers;
using YourVitebskWebServiceApp.APIModels;

namespace YourVitebskWebServiceApp.Tests.APIControllerTests
{
    public class CommentsAPIControllerTests
    {
        private readonly Mock<ICommentService> _mockRepo;
        private readonly CommentsController _controller;

        public CommentsAPIControllerTests()
        {
            _mockRepo = new Mock<ICommentService>();
            _controller = new CommentsController(_mockRepo.Object);
            IEnumerable<Comment> comments = new List<Comment>()
            {
                new Comment()
            };

            _mockRepo.Setup(repo => repo.GetAll(1, 1)).ReturnsAsync(comments);
        }

        [Fact]
        public async void GetAll_ReturnsCorrectType()
        {
            var result = await _controller.GetAll(1, 1);
            Assert.IsAssignableFrom<IEnumerable<Comment>>(result);
        }

        [Fact]
        public async void GetAll_ReturnsExactNumberOfObjects()
        {
            var result = await _controller.GetAll(1, 1);
            var objects = Assert.IsAssignableFrom<IEnumerable<Comment>>(result);
            Assert.Single(objects);
        }

        [Fact]
        public async void Add_ReturnsCorrectType()
        {
            var result = await _controller.AddComment(new CommentDTO());
            Assert.IsType<ActionResult<ResponseModel>>(result);
        }
    }
}
