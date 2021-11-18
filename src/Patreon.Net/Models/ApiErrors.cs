using System;
using Newtonsoft.Json;

namespace Patreon.Net.Models
{
    internal sealed class ApiErrors
    {
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }
    }

    internal sealed class Error
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("code_name")]
        public string CodeName { get; set; }
        [JsonProperty("detail")]
        public string Detail { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
