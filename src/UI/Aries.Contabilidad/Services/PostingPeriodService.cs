using System.Net.Http.Json;
using Aries.Contabilidad.Models.PostingPeriods;

namespace Aries.Contabilidad.Services
{
    public class PostingPeriodService : BaseHttpService, IPostingPeriodService
    {
        public PostingPeriodService(IHttpClientFactory httpClientFactory, ILogger<PostingPeriodService> logger)
            : base(httpClientFactory, logger)
        {
        }

        public async Task<List<PostingPeriodDto>> GetPostingPeriodsAsync(string companyId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<PostingPeriodDto>>($"PostingPeriod/GetPostingPeriods/{companyId}");
                return response ?? new List<PostingPeriodDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
} 