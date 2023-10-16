namespace souschef.server.Services
{
    public interface ISubTaskGenerationService
    {
        void StartInferenceSession(string ID);
        Task<string> RequestSubTaskGeneration(string recipeStep);
    }
}
