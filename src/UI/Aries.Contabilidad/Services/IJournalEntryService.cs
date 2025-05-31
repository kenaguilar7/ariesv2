using Aries.Contabilidad.Models.JournalEntries;

namespace Aries.Contabilidad.Services
{
    public interface IJournalEntryService
    {
        Task<List<JournalEntryDto>> GetJournalEntriesAsync(int postingPeriodId, JournalEntryFilter journalEntryFilter);
        Task<int> GetConsecutiveNumberAsync(int postingPeriodId);
        Task<int> CreateJournalEntryAsync(JournalEntryDto journalEntry);
        Task UpdateJournalEntryAsync(JournalEntryDto journalEntry);
        Task DeleteJournalEntryAsync(JournalEntryDto journalEntry);
    }
} 