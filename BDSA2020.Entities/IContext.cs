using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BDSA2020.Entities
{
    public interface IContext
    {
        DbSet<Student> Students { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}