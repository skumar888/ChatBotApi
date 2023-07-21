using Newtonsoft.Json;

namespace HackChatbotApi.Model;

public class QuestionRequest
{
    [JsonRequired]
    [JsonProperty("question")]
    public String Question { get; set; }
}
