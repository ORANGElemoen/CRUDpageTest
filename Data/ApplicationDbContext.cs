using Microsoft.EntityFrameworkCore;
using CRUDpageTest.Models;

namespace CRUDpageTest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; } = null!;
    }
}
