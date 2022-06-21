using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourVitebskWebServiceApp.APIServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using YourVitebskWebServiceApp.APIModels;

namespace YourVitebskWebServiceApp.APIControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // Gets all comments by service item
        [HttpGet("all")]
        public async Task<IEnumerable<Comment>> GetAll(int serviceId, int itemId)
        {
            return await _commentService.GetAll(serviceId, itemId);
        }

        // Gets news by id
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> AddComment(CommentDTO newComment)
        {
            try
            {
                await _commentService.AddComment(newComment);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(ResponseModel.CreateResponseWithError(e.Message));
            }
        }
    }
}
