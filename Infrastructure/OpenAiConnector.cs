using OpenAI_API;

namespace HackChatbotApi.Infrastructure;

public interface IOpenAiConnector
{
	public OpenAIAPI OpenAIAPI { get; set; }
	public void Connect();
}
public class OpenAiConnector : IOpenAiConnector
{
	public OpenAIAPI OpenAIAPI { get; set; }

	public void Connect()
	{
		OpenAIAPI = new OpenAIAPI(new APIAuthentication(Environment.GetEnvironmentVariable("OpenAIKey")));
	}
}
