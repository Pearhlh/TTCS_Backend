using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Model.Request;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers.Connection;
[Route("api/[controller]")]
[ApiController]
public class GeminiTestController : ControllerBase
{
    private readonly IGeminiClient _geminiClient;

    public GeminiTestController(IGeminiClient geminiClient)
    {
        _geminiClient = geminiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Example(string inputText)
    {

        var messages = new List<Content>
            {
                new Content
                {
                    Parts = new List<Part>
                    {
                        new Part
                        {
                            Text = inputText
                        },
                        new Part
                        {
                            Text = "Viết dạng tiếng việt, format sao cho có thể chèn vào thẻ div"
                        }
                    }
                },
                // Add more Content objects as needed
            };

        var response = await _geminiClient.TextPrompt(messages);
        // Process the response as needed
        return Ok(response);
    }

    [HttpGet("model")]
    public async Task<IActionResult> Example()
    {
        var response = await _geminiClient.GetModels();
        // Process the response as needed
        return Ok(response);
    }
}


