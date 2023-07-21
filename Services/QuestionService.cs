using HackChatbotApi.Infrastructure;
using HackChatbotApi.Model;
using OpenAI_API;

namespace HackChatbotApi.Services;

public interface IQuestionService
{
    public Task<string> AnswerQuestion(QuestionRequest questionRequest);
}
public class QuestionService : IQuestionService
{
    IOpenAiConnector _openAiConnector;
    public QuestionService(IOpenAiConnector openAiConnector)
    {
        _openAiConnector = openAiConnector;
    }

    public async Task<string> AnswerQuestion(QuestionRequest questionRequest)
    {
        _openAiConnector.OpenAIAPI = new OpenAIAPI(new APIAuthentication(Environment.GetEnvironmentVariable("OpenAIKey")));
        var conversation = _openAiConnector.OpenAIAPI.Chat.CreateConversation();
        conversation.AppendUserInput(questionRequest.Question);
        return await conversation.GetResponseFromChatbotAsync();
    }
}
