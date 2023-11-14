
using souschef.server.Data.Models;

namespace souschef.server.Data.Repository.Contracts
{
    public interface ILiveSessionRepository
    {
        public LiveSession? GetSessionByCode(int sessionCode);
    }
}