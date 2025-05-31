using System.Net.Http.Json;
using Aries.Contabilidad.Models.JournalEntries;

namespace Aries.Contabilidad.Services
{
    public class JournalEntryLineService : BaseHttpService, IJournalEntryLineService
    {
        public JournalEntryLineService(IHttpClientFactory httpClientFactory, ILogger<JournalEntryService> logger)
            : base(httpClientFactory, logger)
        {
            
            
        }

        public async Task<int> CreateJournalEntryLineAsync(JournalEntryLineDto journalEntryLine)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("JournalEntryLine/CreateJournalEntryLine", journalEntryLine);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<int>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateJournalEntryLineAsync(JournalEntryLineDto journalEntryLine)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("JournalEntryLine/UpdateJournalEntryLine", journalEntryLine);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteJournalEntryLineAsync(JournalEntryLineDto journalEntryLine)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("JournalEntryLine/DeleteJournalEntryLine", journalEntryLine);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<JournalEntryLineDto>> GetJournalEntryLinesAsync(int journalEntryId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<JournalEntryLineDto>>($"JournalEntryLine/FindJournalEntryLine/{journalEntryId}");
                return response ?? new List<JournalEntryLineDto>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
} 