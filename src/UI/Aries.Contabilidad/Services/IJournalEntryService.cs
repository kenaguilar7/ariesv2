using Aries.Contabilidad.Models.JournalEntries;

namespace Aries.Contabilidad.Services
{
    public interface IJournalEntryService
    {
        Task<List<JournalEntryDto>> GetJournalEntriesAsync(int postingPeriodId);
        Task<int> GetConsecutiveNumberAsync(int postingPeriodId);
        Task<int> CreateJournalEntryAsync(JournalEntryDto journalEntry);
        Task UpdateJournalEntryAsync(JournalEntryDto journalEntry);
        Task DeleteJournalEntryAsync(JournalEntryDto journalEntry);
        Task<int> CreateJournalEntryLineAsync(JournalEntryLineDto line);
        Task UpdateJournalEntryLineAsync(JournalEntryLineDto line);
        Task DeleteJournalEntryLineAsync(JournalEntryLineDto line);
        Task<List<JournalEntryLineDto>> GetJournalEntryLinesAsync(int journalEntryId);
    }
} 