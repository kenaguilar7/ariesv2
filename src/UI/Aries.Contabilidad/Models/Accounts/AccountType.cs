using System.Text.Json.Serialization;

namespace Aries.Contabilidad.Models.Accounts
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AccountType
    {
        [JsonPropertyName("Cuenta_Titulo")]
        Cuenta_Titulo = 1,
        [JsonPropertyName("Cuenta_De_Mayor")]
        Cuenta_De_Mayor = 2,
        [JsonPropertyName("Cuenta_Auxiliar")]
        Cuenta_Auxiliar = 3,
    }
} 