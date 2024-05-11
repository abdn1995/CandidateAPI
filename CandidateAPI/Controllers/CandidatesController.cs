using Microsoft.AspNetCore.Mvc;
using CandidateAPI.Models;
using CandidateAPI.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandidateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateRepository _repository;
        private readonly IMemoryCache _memoryCache;
        private readonly string _candidateListCacheKey = "candidateListCacheKey";
        private readonly string _candidateDetailsCacheKey = "candidateDetailsCacheKey_";

        public CandidatesController(ICandidateRepository repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }

        // GET: api/Candidates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidates()
        {
            if (!_memoryCache.TryGetValue(_candidateListCacheKey, out List<Candidate> candidateList))
            {
                candidateList = (List<Candidate>)await _repository.GetAllCandidatesAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set(_candidateListCacheKey, candidateList, cacheEntryOptions);
            }
            return candidateList;
        }

        // GET: api/Candidates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Candidate>> GetCandidate(int id)
        {
            string cacheKey = _candidateDetailsCacheKey + id;
            if (!_memoryCache.TryGetValue(cacheKey, out Candidate candidate))
            {
                candidate = await _repository.GetCandidateByIdAsync(id);
                if (candidate == null)
                {
                    return NotFound();
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _memoryCache.Set(cacheKey, candidate, cacheEntryOptions);
            }
            return candidate;
        }

        // POST: api/Candidates
        [HttpPost]
        public async Task<ActionResult<Candidate>> PostCandidate(Candidate candidate)
        {
            await _repository.AddCandidateAsync(candidate);
            _memoryCache.Remove(_candidateListCacheKey);
            return CreatedAtAction("GetCandidate", new { id = candidate.Id }, candidate);
        }

        // PUT: api/Candidates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCandidate(int id, Candidate candidate)
        {
            if (id != candidate.Id)
            {
                return BadRequest();
            }
            await _repository.UpdateCandidateAsync(candidate);
            _memoryCache.Remove(_candidateListCacheKey);
            _memoryCache.Remove(_candidateDetailsCacheKey + id);
            return NoContent();
        }

        // DELETE: api/Candidates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            await _repository.DeleteCandidateAsync(id);
            _memoryCache.Remove(_candidateListCacheKey);
            _memoryCache.Remove(_candidateDetailsCacheKey + id);
            return NoContent();
        }
    }
}
