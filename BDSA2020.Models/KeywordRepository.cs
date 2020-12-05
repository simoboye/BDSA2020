using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;
using Microsoft.EntityFrameworkCore;

namespace BDSA2020.Models
{
    public class KeywordRepository : IKeywordRepository
    {
        private readonly IContext context;

        public KeywordRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<string>> GetKeywordsAsync()
        {
            return await context.Keywords.Select(k => k.Name).ToListAsync();
        }
    }
}
