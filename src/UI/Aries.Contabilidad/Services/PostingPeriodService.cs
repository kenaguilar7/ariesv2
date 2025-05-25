using System.Net.Http.Json;
using Aries.Contabilidad.Models.PostingPeriods;

namespace Aries.Contabilidad.Services
{
    public class PostingPeriodService : IPostingPeriodService
    {
        private readonly HttpClient _httpClient;

        public PostingPeriodService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PostingPeriod>> GetPostingPeriodsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<PostingPeriod>>("PostingPeriod/GetPostingPeriods");
                return response ?? new List<PostingPeriod>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
} 