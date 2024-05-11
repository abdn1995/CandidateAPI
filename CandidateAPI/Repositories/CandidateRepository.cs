using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CandidateAPI.Data;
using CandidateAPI.Models;

public class CandidateRepository : ICandidateRepository
{
    private readonly ApplicationDbContext _context;

    public CandidateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Candidate>> GetAllCandidatesAsync()
    {
        return await _context.Candidates.ToListAsync();
    }

    public async Task<Candidate> GetCandidateByIdAsync(int id)
    {
        return await _context.Candidates.FindAsync(id);
    }

    public async Task<Candidate> AddCandidateAsync(Candidate candidate)
    {
        _context.Candidates.Add(candidate);
        await _context.SaveChangesAsync();
        return candidate;
    }

    public async Task UpdateCandidateAsync(Candidate candidate)
    {
        _context.Entry(candidate).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCandidateAsync(int id)
    {
        var candidate = await _context.Candidates.FindAsync(id);
        if (candidate != null)
        {
            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
        }
    }
}
