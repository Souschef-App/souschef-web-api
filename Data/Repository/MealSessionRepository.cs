/*// Repositories/SessionRepository.cs

using Microsoft.EntityFrameworkCore;
using souschef.server.Data;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class MealSessionRepository
{
    private readonly PostGresDBContext _context;
    public IEnumerable<MealSession>? MealSessions => _context.MealSessions.Include(c => c.Users).Include(c => c.Plan).ThenInclude(x => x.Recipes).ThenInclude(y => y.Recipe);

    public MealSessionRepository(PostGresDBContext context)
    {
        _context = context;
    }

    public MealSession Create(MealSessionCreateDTO session)
    {
        MealSession newSession = new MealSession();
        newSession.DateTime = session.DateTime;
        newSession.Plan = _context.MealPlans.FirstOrDefault(c => c.Id == session.PlanId);
        newSession.ServerIp = session.ServerIp;
        newSession.SessionCode = session.SessionCode;
        var res = _context.MealSessions.Add(newSession);
        _context.SaveChanges();
        return res.Entity;
    }

    public MealSession? Get(Guid id)
    {
        return MealSessions.FirstOrDefault(c => c.Id == id);
    }
    
    public MealSession? GetByCode(string code)
    {
        return MealSessions.FirstOrDefault(c => c.SessionCode.Equals(code));
    }

    public IEnumerable<MealSession> GetAll()
    {
        return MealSessions;
    }

    public void Update(MealSession session)
    {
        var existingSession = Get(session.Id);
        if (existingSession != null)
        {
            existingSession.DateTime = session.DateTime;
            _context.MealSessions.Update(existingSession);
            _context.SaveChanges();
        }
    }

    public void Delete(Guid id)
    {
        var session = Get(id);
        if (session != null)
        {
            _context.MealSessions.Remove(session);
            _context.SaveChanges();
        }
    }

    public void CreateSessionUser(Guid id, Guid userId)
    {
        Console.WriteLine(id + " " + userId);
        MealSessionUser user = new MealSessionUser();
        user.Session = MealSessions.FirstOrDefault(c => c.Id == id);
        user.User = _context.ApplicationUsers.FirstOrDefault(c => c.Id.Equals(userId.ToString()));
        _context.MealSessionUsers.Add(user);
        _context.SaveChanges();
    }

    public void DeleteSessionUser(Guid id, Guid userId)
    {
        MealSessionUser user = _context.MealSessions.Include(c => c.Users).ThenInclude(x => x.User).FirstOrDefault(c => c.Id == id).Users.FirstOrDefault(c => c.User.Id.Equals(userId.ToString()));
        _context.MealSessionUsers.Remove(user);
        _context.SaveChanges();
    }
}
*/