using Aries.Contabilidad.Models.JournalEntries;

namespace Aries.Contabilidad.Services
{
    public interface IJournalEntryLineService
    {
        Task<int> CreateJournalEntryLineAsync(JournalEntryLineDto journalEntryLine);
        Task UpdateJournalEntryLineAsync(JournalEntryLineDto journalEntryLine);
        Task DeleteJournalEntryLineAsync(JournalEntryLineDto journalEntryLine);
        Task<List<JournalEntryLineDto>> GetJournalEntryLinesAsync(int journalEntryId);
    }
} 