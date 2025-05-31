using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Aries.Contabilidad.Models.Accounts;

namespace Aries.Contabilidad.Services
{
    public abstract class BaseHttpService
    {
        protected readonly HttpClient _httpClient;
        protected readonly JsonSerializerOptions _jsonOptions;
        protected readonly ILogger? _logger;

        protected BaseHttpService(
            IHttpClientFactory httpClientFactory,
            ILogger? logger = null)
        {
            _httpClient = httpClientFactory.CreateClient("AriesAPI");
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals | JsonNumberHandling.AllowReadingFromString,
                Converters = { 
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true)
                },
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }
    }
} 