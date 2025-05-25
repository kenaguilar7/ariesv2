using Aries.Contabilidad.Models.PostingPeriods;

namespace Aries.Contabilidad.Services
{
    public interface IPostingPeriodService
    {
        Task<List<PostingPeriod>> GetPostingPeriodsAsync();
    }
} 