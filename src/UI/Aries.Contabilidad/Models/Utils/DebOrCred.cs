using System.Text.Json.Serialization;

namespace Aries.Contabilidad.Models.Utils
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DebOrCred
    {
        [JsonPropertyName("Debito")]
        Debito = 1,
        [JsonPropertyName("Credito")]
        Credito = 2
    }
} 