using System.Net.Http.Json;
using Aries.Contabilidad.Models.JournalEntries;
using Microsoft.Extensions.Logging;

namespace Aries.Contabilidad.Services
{
    public class JournalEntryService : BaseHttpService, IJournalEntryService
    {
        public JournalEntryService(IHttpClientFactory httpClientFactory, ILogger<JournalEntryService> logger)
            : base(httpClientFactory, logger)
        {
        }

        public async Task<List<JournalEntryDto>> GetJournalEntriesAsync(int postingPeriodId, JournalEntryFilter journalEntryFilter)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<JournalEntryDto>>($"JournalEntry/GetJournalEntries/{postingPeriodId}");
                return response ?? new List<JournalEntryDto>();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> GetConsecutiveNumberAsync(int postingPeriodId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<int>($"JournalEntry/GetConsecutiveNumber/{postingPeriodId}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> CreateJournalEntryAsync(JournalEntryDto journalEntry)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("JournalEntry/CreateJournalEntry", journalEntry);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<int>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateJournalEntryAsync(JournalEntryDto journalEntry)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("JournalEntry/UpdateJournalEntry", journalEntry);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteJournalEntryAsync(JournalEntryDto journalEntry)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("JournalEntry/DeleteJournalEntry", journalEntry);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
} 