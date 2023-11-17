
using souschef.server.Data.Models;

namespace souschef.server.Data.Repository.Contracts
{
    public interface ILiveSessionRepository
    {
        public LiveSession? CreateSession(string ipAddress);
        public bool DeleteSessionByCode(int sessionCode);
        public LiveSession? GetSessionByCode(int sessionCode);
    }
}