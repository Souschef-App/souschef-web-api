using Microsoft.EntityFrameworkCore;
using souschef.server.Data.Models;

namespace souschef.server.Data.Repository.Contracts
{
    public class CookingSessionRepository : ICookingSessionRepository
    {
        private readonly PostGresDBContext _context;

        public CookingSessionRepository(PostGresDBContext context) { _context = context; }

        public string? GetIP(int sessionCode)
        {
            var cookingSession = _context.CookingSessions?.FirstOrDefault(session => session.Code == sessionCode);
            return cookingSession?.IP;
        }
    }
}