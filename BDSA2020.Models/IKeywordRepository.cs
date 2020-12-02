using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDSA2020.Models
{
    public interface IKeywordRepository
    {
        Task<ICollection<string>> GetKeywordsAsync();
    }
}