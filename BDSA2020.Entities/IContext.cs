using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BDSA2020.Entities
{
    public interface IContext
    {
        DbSet<Student> Students { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<PlacementDescription> PlacementDescriptions { get; set; }
        DbSet<Saved> Saved { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}