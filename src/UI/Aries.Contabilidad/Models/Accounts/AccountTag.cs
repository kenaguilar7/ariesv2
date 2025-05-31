using System.Text.Json.Serialization;

namespace Aries.Contabilidad.Models.Accounts
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AccountTag
    {
        [JsonPropertyName("Activo")]
        Activo = 1,
        [JsonPropertyName("Pasivo")]
        Pasivo = 2,
        [JsonPropertyName("Patrimonio")]
        Patrimonio = 3,
        [JsonPropertyName("Ingreso")]
        Ingreso = 4,
        [JsonPropertyName("CostoVenta")]
        CostoVenta = 5,
        [JsonPropertyName("Egreso")]
        Egreso = 6
    }
} 