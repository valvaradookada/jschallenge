using Challenge.Api.Models;
using Challenge.Api.Services;
using Microsoft.AspNetCore.Mvc;
namespace Challenge.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController: ControllerBase
{
    private readonly IChatService _chatService;
    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("ReceiveMessage")]
    public async Task<ActionResult> ReceiveMessage([FromBody] ChatMessageRequest chatMessage)
    {
        try
        {
            await _chatService.ReceiveMessage(chatMessage.User, chatMessage.Message);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return UnprocessableEntity();
        }
    }
    
    [HttpGet("PreviousMessages")]
    public async Task<ActionResult> PreviousMessages()
    {
        try
        {
            var result = await _chatService.GetPreviousMessages();
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return UnprocessableEntity();
        }
    }
}