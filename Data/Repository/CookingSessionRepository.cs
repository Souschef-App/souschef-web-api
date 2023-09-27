using Microsoft.EntityFrameworkCore;
using souschef.server.Data.Models;

namespace souschef.server.Data.Repository.Contracts
{
    public class CookingSessionRepository : ICookingSessionRepository
    { 
        private readonly PostGresDBContext _context;

        public CookingSessionRepository(PostGresDBContext context){ _context = context; }

        public IEnumerable<CookingSession>? CookingSessions => _context.CookingSession?
                .Include(cS => cS.Recipes!).ThenInclude(x => x.Ingredients)
                .Include(cS => cS.Recipes!).ThenInclude(x => x.Kitchenware)
                .Include(cS => cS.Recipes!).ThenInclude(x => x.Tasks).ThenInclude(x => x.Ingredients)
                .Include(cS => cS.Recipes!).ThenInclude(x => x.Tasks).ThenInclude(x => x.Kitchenware)
                .Include(cS => cS.Host);


        Models.Task ICookingSessionRepository.GetTask()
        {
            throw new NotImplementedException();
        }

        public void SaveCookingSession(CookingSession cookingSession)
        {
             _context.CookingSession?.Add(cookingSession);
             _context.SaveChanges();
        }

        public IEnumerable<CookingSession>? GetCookingSessionsByUser(Guid userId)
        {
            return CookingSessions?.Where(cs => cs.Host?.Id == userId.ToString());
        }

        public CookingSession? GetCookingSessionsById(Guid sessionId)
        {
            return CookingSessions?.Where(cs => cs.Id == sessionId).First();
        }

        public IEnumerable<ApplicationUser> GetUsers(Guid sessionId)
        {
           return _context.Users.Where(u => u.CurrentSessionId == sessionId);
        }

    }
}