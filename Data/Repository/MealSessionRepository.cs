// Repositories/SessionRepository.cs

using System;
using System.Collections.Generic;
using System.Linq;

public class SessionRepository
{
    private readonly List<MealSession> _sessions = new List<MealSession>();
    private int _nextId = 1;

    public MealSession Create(MealSession session)
    {
        session.Id = _nextId++;
        _sessions.Add(session);
        return session;
    }

    public MealSession? Get(int id)
    {
        return _sessions.FirstOrDefault(s => s.Id == id);
    }

    public IEnumerable<MealSession> GetAll()
    {
        return _sessions;
    }

    public void Update(MealSession session)
    {
        var existingSession = Get(session.Id);
        if (existingSession != null)
        {
            existingSession.DateTime = session.DateTime;
        }
    }

    public void Delete(int id)
    {
        var session = Get(id);
        if (session != null)
        {
            _sessions.Remove(session);
        }
    }
}
