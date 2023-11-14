using souschef.server.Data.Models;

namespace souschef.server.Data.Repository.Contracts
{
    public class LiveSessionRepository : ILiveSessionRepository
    {
        private readonly PostGresDBContext _context;

        public LiveSessionRepository(PostGresDBContext context) { _context = context; }

        public LiveSession? GetSessionByCode(int sessionCode)
        {
            return _context.LiveSessions?.FirstOrDefault(session => session.Code == sessionCode);
        }
    }
}