using System.Text.Json.Serialization;

namespace StudentProject.Services.Api.Response
{
    public class MessageError
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}