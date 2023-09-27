
using souschef.server.Data.Models;

namespace souschef.server.Data.Repository.Contracts
{
    public interface ICookingSessionRepository
    { 
        public Models.Task GetTask();
        public void SaveCookingSession(CookingSession cookingSession);
        public IEnumerable<ApplicationUser> GetUsers(Guid sessionId);
        public IEnumerable<CookingSession>? GetCookingSessionsByUser(Guid userId);
        public CookingSession? GetCookingSessionsById(Guid sessionId);
    }
}