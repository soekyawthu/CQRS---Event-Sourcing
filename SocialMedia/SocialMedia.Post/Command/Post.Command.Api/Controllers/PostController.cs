using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands;
using Post.Command.Api.DTOs;
using Post.Common.DTOs;
using SocialMedia.Core.Infrastructure;

namespace Post.Command.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly ICommandDispatcher _dispatcher;

    public PostController(ILogger<PostController> logger, ICommandDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> NewPost(NewPostCommand command)
    {
        var id = Guid.NewGuid();
        try
        {
            command.Id = id;
            await _dispatcher.SendAsync(command);
            return Created(nameof(NewPost), new NewPostResponse
            {
                Id = id,
                Message = "Successfully created new post"
            });
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(e, "Client made a bad request");
            return BadRequest(new NewPostResponse
            {
                Id = id,
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            const string errorMessage = "Error while processing request to create a new post";
            _logger.LogError(e, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
            {
                Message = errorMessage
            });
        }
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditPost(Guid id, EditPostCommand command)
    {
        command.Id = id;
        try
        {
            await _dispatcher.SendAsync(command);
            return Created(nameof(NewPost), new BaseResponse
            {
                Message = "Successfully updated post"
            });
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(e, "Client made a bad request");
            return BadRequest(new BaseResponse
            {
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            const string errorMessage = "Error while processing request to update post";
            _logger.LogError(e, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
            {
                Message = errorMessage
            });
        }
    }
}