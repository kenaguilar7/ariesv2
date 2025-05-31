namespace Aries.Contabilidad.Models.JournalEntries
{
    public class JournalEntryFilter
    {
        public string SearchTerm { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartNumber { get; set; }
        public string EndNumber { get; set; }
    }
}
