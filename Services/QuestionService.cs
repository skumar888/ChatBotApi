using HackChatbotApi.Data;
using HackChatbotApi.Infrastructure;
using HackChatbotApi.Model;
using OpenAI_API.Chat;

namespace HackChatbotApi.Services;

public interface IQuestionService
{
    public Task<string> AnswerQuestion(QuestionRequest questionRequest);
    public Task<string> AnswerQuestionTurbo(QuestionRequest questionRequest);
}
public class QuestionService : IQuestionService
{
    private readonly IOpenAiConnector _openAiConnector;
    private readonly IDataContext _dataContext;

    public QuestionService(IOpenAiConnector openAiConnector, IDataContext dataContext)
    {
        _openAiConnector = openAiConnector;
        _dataContext = dataContext;

    }

    public async Task<string> AnswerQuestion(QuestionRequest questionRequest)
    {
        var conversation = _openAiConnector.OpenAIAPI.Chat.CreateConversation();
        conversation.Model = OpenAI_API.Models.Model.ChatGPTTurbo;
        conversation.AppendUserInput($"Please use the following database schema for any sql query requests.{await _dataContext.GetSchema()}. Every response make sure table names and column names are case sensitive and are encapsulated in double quotes.What is the postgres sql query for: {questionRequest.Question}. Return just the query.");
        var response = await conversation.GetResponseFromChatbotAsync();
        return await _dataContext.RunQuery(response);
    }

    public async Task<string> AnswerQuestionTurbo(QuestionRequest questionRequest)
    {
        var query = $"Please use the following database schema for following request.{await _dataContext.GetSchema()}.What is the postgres sql query for: {questionRequest.Question}. Return just the query.";
        var result = await _openAiConnector.OpenAIAPI.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = OpenAI_API.Models.Model.ChatGPTTurbo,
            Temperature = 0.1,
            MaxTokens = 50,
            Messages = new ChatMessage[] {
            new ChatMessage(ChatMessageRole.User,query )
        }
        });
        return result.ToString();
    }
}
