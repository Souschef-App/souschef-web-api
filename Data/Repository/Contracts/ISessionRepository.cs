public interface ISessionRepository
{
    Task<Session> GetSessionAsync(int sessionId);
    Task<IEnumerable<Session>> GetAllSessionsAsync();
    Task AddSessionAsync(Session session);
    Task UpdateSessionAsync(int sessionId, Session updatedSession);
    Task DeleteSessionAsync(int sessionId);
}