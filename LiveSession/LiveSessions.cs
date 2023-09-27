
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using souschef.server.Helpers;
using souschef.server.LiveSession;

namespace souschef.server.Data.LiveSession
{
    public class LiveSessions
    {
        private static readonly LiveSessions m_instance = new();
        private readonly Dictionary<Guid, LiveCookingSession> m_currentCookingSessions;

        LiveSessions()
        {
            m_currentCookingSessions = new Dictionary<Guid, LiveCookingSession>();
        }

        public static LiveSessions GetLiveSessions()
        {
            return m_instance;
        }

        public LiveCookingSession? GetSessionById(Guid _sessionId)
        {
            return m_currentCookingSessions.ContainsKey(_sessionId)
                ? m_currentCookingSessions[_sessionId]
                : null;
        }

        public bool RemoveSessionById(Guid _sessionId) => m_currentCookingSessions.Remove(_sessionId);

        public LiveCookingSession? StartCookingSession(CookingSession cookingSession)
        {

            if (cookingSession.Host == null || cookingSession.Recipes == null)
                return null;

            var recipes = new Dictionary<Guid, LiveRecipe>();
            var members = new List<UserDTO>();

            foreach (var r in cookingSession.Recipes)
            {
                Console.WriteLine("cookingSession.Recipes " + r.Tasks.Count);
                recipes.Add(r.Id, Conversions.ToLiveRecipe(r));
            }

            foreach (var mem in cookingSession.Guests)
            {
                members.Add(Conversions.ToUserDTO(mem));
            }

            var session = new LiveCookingSession()
            {
                Id = cookingSession.Id,
                Host = Conversions.ToUserDTO(cookingSession.Host),
                Members = members,
                Recipes = recipes
            };

            if (m_currentCookingSessions.TryAdd(cookingSession.Id, session))
                return session;
            else
                return null;
        }

        public class LiveCookingSession
        {
            public Guid? Id { get; set; }
            public UserDTO? Host { get; set; }

            public List<UserDTO> Members = new();

            public Dictionary<Guid, LiveRecipe> Recipes = new();

            public UserDTO GetUser(string userId)
            {
                return Members.Where(x => x.Id == userId).First();
            }

            public Models.Task? GetNextTask(string userId)
            {
                var user = Members.Where(x => x.Id == userId).First();
                var res = TaskAlgorithmn.Entry(Recipes, user);

                if(res != null)
                {
                    user.CurrentRecipe = res.Value.recipeId;
                    Recipes[res.Value.recipeId].GetTask(res.Value.nextTask.Id).InProgress = true;

                    return res.Value.nextTask;
                }
                else
                {
                    return null;
                }
            }  
        }

        public class LiveRecipe
        {
            public Guid id;
            public List<Models.Task> Tasks = new();

            public Models.Task GetTask(Guid id)
            {
                return Tasks.Where(x => x.Id == id).First();
            }
        }

    }
}
