using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CandidateAPI.Data;
using CandidateAPI.Models;
using CandidateAPI.Repositories;

public class CandidateRepositoryTests
{
    private async Task<ApplicationDbContext> GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CandidateDbInMemoryTest")
            .Options;

        var databaseContext = new ApplicationDbContext(options);
        databaseContext.Database.EnsureCreated();
        if (!databaseContext.Candidates.Any())
        {
            databaseContext.Candidates.AddRange(
                new Candidate
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "123-456-7890",
                    TimeInterval = "9-11",
                    LinkedInProfileUrl = "https://www.linkedin.com/in/johndoe",
                    GitHubProfileUrl = "https://github.com/johndoe",
                    Comment = "A skilled software developer."
                },
                new Candidate
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "jane.doe@example.com",
                    PhoneNumber = "098-765-4321",
                    TimeInterval = "10-12",
                    LinkedInProfileUrl = "https://www.linkedin.com/in/janedoe",
                    GitHubProfileUrl = "https://github.com/janedoe",
                    Comment = "An experienced software engineer."
                }
            );
            await databaseContext.SaveChangesAsync();
        }
        return databaseContext;
    }


    [Fact]
    public async Task GetAllCandidatesAsync_ReturnsAllCandidates()
    {
        using (var context = await GetDatabaseContext())
        {
            var repository = new CandidateRepository(context);
            var candidates = await repository.GetAllCandidatesAsync();
            Assert.Equal(2, candidates.Count());
        }
    }

    [Fact]
    public async Task GetCandidateByIdAsync_ReturnsCorrectCandidate()
    {
        using (var context = await GetDatabaseContext())
        {
            var repository = new CandidateRepository(context);
            var candidate = await repository.GetCandidateByIdAsync(1);
            Assert.NotNull(candidate);
            Assert.Equal("John", candidate.FirstName);
        }
    }

    [Fact]
    public async Task AddCandidateAsync_AddsCandidateSuccessfully()
    {
        using (var context = await GetDatabaseContext())
        {
            var repository = new CandidateRepository(context);

            // Initially, we should consider the database might be seeding 2 candidates.
            var initialCount = await context.Candidates.CountAsync();

            var candidate = new Candidate
            {
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                PhoneNumber = "555-123-4567",
                TimeInterval = "13-15",
                LinkedInProfileUrl = "https://www.linkedin.com/in/alicejohnson",
                GitHubProfileUrl = "https://github.com/alicejohnson",
                Comment = "A proactive product manager."
            };

            await repository.AddCandidateAsync(candidate);

            // Assert the new count is the initial count + 1
            Assert.Equal(initialCount + 1, await context.Candidates.CountAsync());
        }
    }
}
