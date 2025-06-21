using System.Net.Http.Json;
using Aries.Contabilidad.Models.JournalEntries;

namespace Aries.Contabilidad.Services
{
    public class JournalEntryLineService : BaseHttpService, IJournalEntryLineService
    {
        private readonly ILocalStorageService _localStorageService;

        public JournalEntryLineService(IHttpClientFactory httpClientFactory, ILogger<JournalEntryService> logger, ILocalStorageService localStorageService)
            : base(httpClientFactory, logger)
        {
            _localStorageService = localStorageService;
        }

        public async Task<int> CreateJournalEntryLineAsync(JournalEntryLineDto journalEntryLine)
        {
            try
            {
                var user = await _localStorageService.GetCurrentUserSesion();
                journalEntryLine.CreatedBy = user.Id;
                journalEntryLine.UpdatedBy = user.Id;

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
                var user = await _localStorageService.GetCurrentUserSesion();
                journalEntryLine.UpdatedBy = user.Id;

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
                var user = await _localStorageService.GetCurrentUserSesion();
                journalEntryLine.UpdatedBy = user.Id;

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