using Microsoft.EntityFrameworkCore;
using CandidateAPI.Models;
using CandidateAPI.Models;

namespace CandidateAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }
    }
}
