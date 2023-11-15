using souschef.server.Data.Models;

namespace souschef.server.Data.Repository.Contracts
{
    public class LiveSessionRepository : ILiveSessionRepository
    {
        private readonly PostGresDBContext _context;

        public LiveSessionRepository(PostGresDBContext context) { _context = context; }

        public LiveSession? CreateSession(string ipAddress)
        {
            // TODO: Make generate uniqe session code (5-digit)

            var session = new LiveSession { Code = 12345, IP = ipAddress };

            _context.LiveSessions?.Add(session);
            _context.SaveChanges();

            return session;
        }

        public bool DeleteSessionByCode(int sessionCode)
        {
            var session = _context.LiveSessions?.First(session => session.Code == sessionCode);

            if (session != null)
            {
                _context.LiveSessions?.Remove(session);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public LiveSession? GetSessionByCode(int sessionCode)
        {
            return _context.LiveSessions?.FirstOrDefault(session => session.Code == sessionCode);
        }
    }
}