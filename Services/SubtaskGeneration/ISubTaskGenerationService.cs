namespace souschef.server.Services.SubtaskGeneration
{
    public interface ISubTaskGenerationService
    {
        void StartInferenceSession(string ID);
        Task<string> RequestSubTaskGeneration(string recipeStep);
        Task<string> RequestRegenerationOfSubTask(string subTask, string ID);
        Task<string> RequestRegenerationOfAllSubTask(string subTasks, string ID);
    }
}
