using System.Text.Json.Serialization;

namespace Aries.Contabilidad.Models.Utils
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency
    {
        [JsonPropertyName("colones")]
        colones = 1,
        [JsonPropertyName("dolres")]
        dolares = 2
    }
} 