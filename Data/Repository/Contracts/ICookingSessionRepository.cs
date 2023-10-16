
using souschef.server.Data.Models;

namespace souschef.server.Data.Repository.Contracts
{
    public interface ICookingSessionRepository
    {
        public string? GetIP(int sessionCode);
    }
}