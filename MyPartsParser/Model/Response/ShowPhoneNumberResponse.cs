using System.Text.Json.Serialization;

namespace MyPartsParser.Model.Response
{
    public class ShowPhoneNumberResponse
    {
        [property: JsonPropertyName("data")]
        public string? PhoneNumber { get; set; }
    }
}
