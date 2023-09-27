using Microsoft.EntityFrameworkCore.Infrastructure;
using souschef.server.Data.DTOs;

using static souschef.server.Data.LiveSession.LiveSessions;

namespace souschef.server.LiveSession
{
    public static class TaskAlgorithmn
    {
        public static (Guid recipeId, Data.Models.Task nextTask)? Entry(Dictionary<Guid, LiveRecipe> liveRecipes, UserDTO user)
        {
            //user's last recipe chain and most far off are less then min off
            //stay on current task chain
            //if close opt for the same task stream the user was on before
            //else push for the one behind 
            var progress = CheckRecipeProgress(liveRecipes.Values.ToList());

            var maxRecipeTimeLeft = progress.OrderBy(d => d.Value).Last();
            var recipeIDs = liveRecipes.Keys.ToArray();

            if (user.CurrentRecipe != null)
            {
                 progress.TryGetValue((Guid)user.CurrentRecipe, out float userRecipeTimeLeft);

                if (maxRecipeTimeLeft.Value - userRecipeTimeLeft <= 60)
                {
                    return TryGetTask(NewOrderSetOfRecipeIds((Guid)user.CurrentRecipe, recipeIDs), liveRecipes, user);

                    //var nextTaskInQueue = GetNextUnfinishedTask(liveRecipes[(Guid)user.CurrentRecipe!].Tasks);
                    //if (nextTaskInQueue != null && SkillRatingVSTask(nextTaskInQueue, user))
                    //    return ((Guid)user.CurrentRecipe, nextTaskInQueue);
                }
                else
                {
                    return TryGetTask(NewOrderSetOfRecipeIds(maxRecipeTimeLeft.Key, recipeIDs), liveRecipes, user);
                }
            }
            else
            {
                return TryGetTask(NewOrderSetOfRecipeIds(maxRecipeTimeLeft.Key, recipeIDs), liveRecipes, user);
                //var nextTaskInQueue = GetNextUnfinishedTask(liveRecipes[maxRecipeTimeLeft.Key].Tasks);

                //if (nextTaskInQueue != null && SkillRatingVSTask(nextTaskInQueue, user))
                //{
                //    return (maxRecipeTimeLeft.Key, nextTaskInQueue);
                //}
            }
        }

        private static (Guid recipeId, Data.Models.Task nextTask)? TryGetTask(Guid[] recipeIds, Dictionary<Guid, LiveRecipe> liveRecipes, UserDTO user)
        {
            var nextTaskInQueue = GetNextUnfinishedTask(liveRecipes[recipeIds[0]].Tasks);

            if (nextTaskInQueue != null && SkillRatingVSTask(nextTaskInQueue, user))
                return (recipeIds[0], nextTaskInQueue);

            Guid[] newRecipeIds = recipeIds.Skip(1).ToArray();

            if (newRecipeIds.Length <= 0)
                return null;

            TryGetTask(newRecipeIds, liveRecipes, user);

            return null;
        }

        static Dictionary<Guid, float> CheckRecipeProgress(List<LiveRecipe> liveRecipes)
        {
            Dictionary<Guid, float> timeLeft = new();

            for (int i = 0; i < liveRecipes.Count; i++)
            {
                var recipe = liveRecipes[i];
                var incompleTasks = recipe.Tasks.Where(t => !t.Finished);
                var totalLeft  = incompleTasks.Sum(x => x.Duration);

                timeLeft.Add(recipe.id, totalLeft);
            }

            return timeLeft;
        }

        static bool SkillRatingVSTask(Data.Models.Task task, UserDTO user)
        {
            Console.WriteLine("task.Difficulty " + task.Difficulty + " user.SkillLevel " + user.SkillLevel);
            return task.Difficulty <= 5;
        }

        static Data.Models.Task? GetNextUnfinishedTask(List<Data.Models.Task> tasks)
        {

            var incompleteTasks = tasks.Where(t => !t.Finished && !t.InProgress).OrderBy(t => t.Order).ToList();

            return CheckIncompleteTasks(incompleteTasks, tasks);
        }

        /// <summary>
        /// Look for the next incomplete task that is ok to begin on
        /// </summary>
        /// <param name="incompleteTasks"></param>
        /// <param name="tasks"></param>
        /// <returns></returns>
        static Data.Models.Task? CheckIncompleteTasks(List<Data.Models.Task> incompleteTasks, List<Data.Models.Task> tasks)
        {
            for (int i = 0; i < incompleteTasks.Count; i++)
            {
                if(ScanForOkayToBeginStatus(incompleteTasks[i], tasks))
                {
                    return incompleteTasks[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Check For no Dependencies or all Deps Fufilled 
        /// </summary>
        /// <param name="task"></param>
        /// <param name="_tasks"></param>
        /// <returns></returns>
        static bool ScanForOkayToBeginStatus(Data.Models.Task task, List<Data.Models.Task> _tasks)
        {
            if(task.Dependencies!.Length <= 0)
            {
                return true;
            }
            else
            {
                for(int i = 0; i < task.Dependencies.Length; i++)
                {
                    var task_dep = _tasks.Where(t => t.Order == task.Dependencies[i]).First();

                    if (!task_dep.Finished)
                    {
                        return false;
                    }
                }
                return true;
            }

        }

        /// <summary>
        /// Add My Id To The Start -> First One that will be tried in the Recursive Loop
        /// </summary>
        /// <param name="myID"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        static Guid[] NewOrderSetOfRecipeIds(Guid myID, Guid[] ids)
        {
            Guid[] shallowCopyIds = (Guid[])ids.Clone();
            shallowCopyIds.ToList().Remove(myID);

            var newIds = new List<Guid>
            {
                myID
            };

            newIds.AddRange(shallowCopyIds);

            return newIds.ToArray();
        }
    }
}

