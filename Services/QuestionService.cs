using HackChatbotApi.Infrastructure;
using HackChatbotApi.Model;

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
        var conversation = _openAiConnector.OpenAIAPI.Chat.CreateConversation();
        conversation.AppendUserInput(questionRequest.Question);
        return await conversation.GetResponseFromChatbotAsync();
    }
}
