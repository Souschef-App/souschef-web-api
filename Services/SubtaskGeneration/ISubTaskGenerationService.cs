

using souschef.server.Data.DTOs;

namespace souschef.server.Services.SubtaskGeneration
{
    public interface ISubTaskGenerationService
    {
        void StartInferenceSession(string ID);
        Task<List<Data.Models.Task>> RequestSubTaskGeneration(string recipeStep);
        Task<Data.Models.Task> RequestRegenerationOfSubTask(string prompt, TaskDTO dtoTask);
        Task<string> RequestRegenerationOfAllSubTask(string subTasks, string ID);
    }
}
