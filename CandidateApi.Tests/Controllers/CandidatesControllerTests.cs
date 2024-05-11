using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using CandidateAPI.Controllers;
using CandidateAPI.Models;
using CandidateAPI.Repositories;

public class CandidatesControllerTests
{
    private readonly Mock<ICandidateRepository> _mockRepo;
    private readonly Mock<IMemoryCache> _mockCache;
    private MemoryCache _memoryCache;

    public CandidatesControllerTests()
    {
        _mockRepo = new Mock<ICandidateRepository>();
        _mockCache = new Mock<IMemoryCache>();

        var opts = new MemoryCacheOptions();
        _memoryCache = new MemoryCache(opts);

        _mockCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(_memoryCache.CreateEntry("dummy"));
    }

    [Fact]
    public async Task GetCandidates_ReturnsCandidatesFromCache()
    {
        var cacheKey = "candidateListCacheKey";
        var cachedData = new List<Candidate>
        {
            new Candidate { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new Candidate { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
        };
        _memoryCache.Set(cacheKey, cachedData);

        var controller = new CandidatesController(_mockRepo.Object, _memoryCache);

        var result = await controller.GetCandidates();
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Candidate>>>(result);
        var returnValue = Assert.IsType<List<Candidate>>(actionResult.Value);

        Assert.Equal(2, returnValue.Count);
        Assert.Equal("John", returnValue[0].FirstName);
    }

    [Fact]
    public async Task GetCandidate_ReturnsCandidateFromRepositoryWhenNotInCache()
    {
        var candidate = new Candidate { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        _mockRepo.Setup(repo => repo.GetCandidateByIdAsync(1)).ReturnsAsync(candidate);
        var controller = new CandidatesController(_mockRepo.Object, _memoryCache);

        var result = await controller.GetCandidate(1);
        var actionResult = Assert.IsType<ActionResult<Candidate>>(result);
        var returnValue = Assert.IsType<Candidate>(actionResult.Value);

        Assert.Equal("John", returnValue.FirstName);
    }

    [Fact]
    public async Task PostCandidate_InvalidatesCache()
    {
        var newCandidate = new Candidate { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com" };
        _mockRepo.Setup(repo => repo.AddCandidateAsync(It.IsAny<Candidate>())).ReturnsAsync(new Candidate { Id = 3, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com" });
        var controller = new CandidatesController(_mockRepo.Object, _memoryCache);

        var result = await controller.PostCandidate(newCandidate);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Candidate>(createdAtActionResult.Value);

        Assert.Equal("Alice", returnValue.FirstName);
        Assert.False(_memoryCache.TryGetValue("candidateListCacheKey", out _));
    }
}
