using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CandidateAPI.Data;
using CandidateAPI.Models;
using CandidateAPI.Repositories;

public class CandidateRepositoryTests
{
    private ApplicationDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CandidateDbInMemory")
            .Options;
        var databaseContext = new ApplicationDbContext(options);
        databaseContext.Database.EnsureCreated();
        if (!databaseContext.Candidates.Any())
        {
            databaseContext.Candidates.AddRange(
                new Candidate { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new Candidate { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
            );
            databaseContext.SaveChanges();
        }
        return databaseContext;
    }

    [Fact]
    public async Task GetAllCandidatesAsync_ReturnsAllCandidates()
    {
        // Arrange
        using var context = GetDatabaseContext();
        var repository = new CandidateRepository(context);

        // Act
        var candidates = await repository.GetAllCandidatesAsync();

        // Assert
        Assert.NotNull(candidates);
        Assert.Equal(2, candidates.Count());
    }

    [Fact]
    public async Task GetCandidateByIdAsync_ReturnsCorrectCandidate()
    {
        // Arrange
        using var context = GetDatabaseContext();
        var repository = new CandidateRepository(context);

        // Act
        var candidate = await repository.GetCandidateByIdAsync(1);

        // Assert
        Assert.NotNull(candidate);
        Assert.Equal("John", candidate.FirstName);
    }

    [Fact]
    public async Task AddCandidateAsync_AddsCandidateSuccessfully()
    {
        // Arrange
        using var context = GetDatabaseContext();
        var repository = new CandidateRepository(context);
        var newCandidate = new Candidate { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com" };

        // Act
        var addedCandidate = await repository.AddCandidateAsync(newCandidate);

        // Assert
        Assert.NotNull(addedCandidate);
        Assert.Equal(3, addedCandidate.Id);  // Since in-memory DB is auto-incrementing the Id.
    }

    [Fact]
    public async Task UpdateCandidateAsync_UpdatesCandidateSuccessfully()
    {
        // Arrange
        using var context = GetDatabaseContext();
        var repository = new CandidateRepository(context);
        var candidateToUpdate = await context.Candidates.FindAsync(1);

        // Act
        candidateToUpdate.LastName = "Smith";
        await repository.UpdateCandidateAsync(candidateToUpdate);

        // Assert
        var updatedCandidate = await repository.GetCandidateByIdAsync(1);
        Assert.Equal("Smith", updatedCandidate.LastName);
    }

    [Fact]
    public async Task DeleteCandidateAsync_DeletesCandidateSuccessfully()
    {
        // Arrange
        using var context = GetDatabaseContext();
        var repository = new CandidateRepository(context);

        // Act
        await repository.DeleteCandidateAsync(2);

        // Assert
        var candidate = await repository.GetCandidateByIdAsync(2);
        Assert.Null(candidate);
    }
}
