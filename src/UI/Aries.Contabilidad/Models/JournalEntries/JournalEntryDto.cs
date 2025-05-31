using System;
using System.Collections.Generic;

namespace Aries.Contabilidad.Models.JournalEntries
{
    public class JournalEntryDto: BaseModel
    {
        public int Number { get; set; }
        public int PostingPeriodId { get; set; }
        public string PostingPeriodName { get; set; }
        public DateTime UpdatedAt { get; set; }
        public JournalEntryStatus Status { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal TotalCredits { get; set; }
        public bool IsBalanced => TotalDebits == TotalCredits;
        public List<JournalEntryLineDto> Lines { get; set; } = new List<JournalEntryLineDto>();
        public bool ShowDetails { get; set; }
        public bool IsPosted { get; set; }
    }
} 