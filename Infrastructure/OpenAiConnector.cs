using OpenAI_API;

namespace HackChatbotApi.Infrastructure;

public interface IOpenAiConnector
{
    public OpenAIAPI OpenAIAPI { get; set; }
}
public class OpenAiConnector : IOpenAiConnector
{
    public OpenAIAPI OpenAIAPI { get; set; }
    private readonly IConfiguration _config;

    public OpenAiConnector(IConfiguration config)
    {
        _config = config;
        OpenAIAPI = new OpenAIAPI(new APIAuthentication(_config["OpenAIKey"]));
    }
}
