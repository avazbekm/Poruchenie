using Poruchenie.Domain.Etities;
using Microsoft.EntityFrameworkCore;

namespace Poruchenie.Data;

public class AppDbContext : DbContext
{
    DbSet<Yurdik> Yurdiks { get; set; }
    DbSet<Jismoniy> Jismoniys { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
}
