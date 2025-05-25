using System.Net.Http.Json;
using Aries.Contabilidad.Models.JournalEntries;

namespace Aries.Contabilidad.Services
{
    public class JournalEntryService : IJournalEntryService
    {
        private readonly HttpClient _httpClient;

        public JournalEntryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<JournalEntryDto>> GetJournalEntriesAsync(int postingPeriodId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<JournalEntryDto>>($"JournalEntry/GetJournalEntries/{postingPeriodId}");
                return response ?? new List<JournalEntryDto>();
            }
            catch (Exception)
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

        public async Task<int> CreateJournalEntryLineAsync(JournalEntryLineDto line)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("JournalEntryLine/CreateJournalEntryLine", line);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<int>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateJournalEntryLineAsync(JournalEntryLineDto line)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("JournalEntryLine/UpdateJournalEntryLine", line);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteJournalEntryLineAsync(JournalEntryLineDto line)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("JournalEntryLine/DeleteJournalEntryLine", line);
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
            catch (Exception)
            {
                throw;
            }
        }
    }
} 