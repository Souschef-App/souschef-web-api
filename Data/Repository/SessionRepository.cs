using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;

    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Session> GetSessionAsync(int sessionId)
    {
        return await _context.Sessions.FindAsync(sessionId);
    }

    public async Task<IEnumerable<Session>> GetAllSessionsAsync()
    {
        return await _context.Sessions.ToListAsync();
    }

    public async Task AddSessionAsync(Session session)
    {
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSessionAsync(int sessionId, Session updatedSession)
    {
        var existingSession = await _context.Sessions.FindAsync(sessionId);
        if (existingSession != null)
        {
            // Update properties of the existing session based on updatedSession
            existingSession.Name = updatedSession.Name;
            existingSession.StartTime = updatedSession.StartTime;
            // Update other properties as needed
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteSessionAsync(int sessionId)
    {
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session != null)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }
    }

    // Implement other methods as needed
}
