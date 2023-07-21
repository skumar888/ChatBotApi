using HackChatbotApi.Model;
using HackChatbotApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackChatbotApi.Controllers;

[Route("api/[controller]")]
public class QuestionSubmitController : ControllerBase
{
    IQuestionService _questionService;
    public QuestionSubmitController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitQuestion([FromBody] QuestionRequest Question)
    {
        return Ok(await _questionService.AnswerQuestion(Question));
    }
}

