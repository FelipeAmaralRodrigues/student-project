using System.Text.Json.Serialization;

namespace StudentProject.Services.Api.Response
{
    public class Result
    {
        [JsonPropertyName("succeeded")]
        public bool Succeeded { get; set; }

        [JsonPropertyName("errors")]
        public MessageError[] Errors { get; set; }

        [JsonPropertyName("data")]
        public dynamic Data { get; set; }
    }
}