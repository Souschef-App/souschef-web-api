using System.Diagnostics;
using System.Net.Sockets;
using Grpc.Net.Client;
using souschef.server.Data.DTOs;

namespace souschef.server.Services.SubtaskGeneration
{
    public class SubTaskGenerationService : ISubTaskGenerationService
    {

        public SubTaskGenerationService()
        {
            Debug.WriteLine("SubTaskGenerationService");
            Debug.WriteLine("CREATING CHANNEL");
        }

        public void StartInferenceSession(string ID)
        {

        }

        public async Task<List<Data.Models.Task>> RequestSubTaskGeneration(string recipeStep)
        {
            using var channel = GrpcChannel.ForAddress("http://ai:50051");
            var client = new RecipeGeneration.RecipeGenerationClient(channel);

            Console.WriteLine("requesting breakdown");

            var reply = await client.getRecipeBreakDownAsync(new RecipeBreakdownRequest { Description = recipeStep });

            Console.WriteLine("reply " + reply.Tasks.Count);

            List<Data.Models.Task> tasks = new();

            for (int i = 0; i < reply.Tasks.Count; i++)
            {
                var task = new Data.Models.Task
                {
                    Id = new Guid(reply.Tasks[i].Uuid.ToByteArray()),
                    Title = reply.Tasks[i].Title,
                    Description = reply.Tasks[i].Description,
                    Duration = reply.Tasks[i].Duration,
                    Difficulty = reply.Tasks[i].Difficulty,
                    Dependencies = Helpers.Conversions.ConvertProtoDependencyListtoDependencyArray(reply.Tasks[i].Dependencies),
                    Ingredients = Helpers.Conversions.ConvertProtoIngredientToIngredient(reply.Tasks[i].Ingredients),
                    Kitchenware = Helpers.Conversions.ConvertProtoKitchenwareToKitchenware(reply.Tasks[i].Kitchenware)
                };

                Console.WriteLine("Id " + task.Id);
                tasks.Add(task);
            }

            Console.WriteLine("Count " + tasks.Count);

            return tasks;
        }

        public enum Units
        {
            none,
            ounces,
            pounds,
            grams,
            kilograms,
            teaspoon,
            tablespoon,
            cups,
            pints,
            quarts,
            gallons,
            mililiters,
            liters
        }

        public async Task<Data.Models.Task> RequestRegenerationOfSubTask(string prompt, TaskDTO dtoTask)
        {
            Debug.WriteLine("RequestRegenerationOfSubTask");
            if (dtoTask.Ingredients == null || dtoTask.KitchenWare == null)
                throw new Exception("Ingredients NULL");

            Guid ID = dtoTask.Id != null ? (Guid)dtoTask.Id : throw new Exception("dtoTask.Id NULL");

            using var channel = GrpcChannel.ForAddress("http://ai:50051");
            var client = new RecipeGeneration.RecipeGenerationClient(channel);

            Task task = new()
            {
                Title = dtoTask.Title,
                Description = dtoTask.Description,
                Difficulty = dtoTask.Difficulty,
                Duration = (int)dtoTask.Duration,
            };

            foreach (var ingredient in dtoTask.Ingredients)
            {
                Fraction q = new()
                {
                    Whole = ingredient!.Quantity!.Whole,
                    Numerator = ingredient!.Quantity!.Numerator,
                    Denominator = ingredient!.Quantity!.Denominator,
                };

                Ingredient ing = new()
                {
                    Name = ingredient.Name,
                    Quantity = q,
                    Unit = ingredient.Unit.ToString()
                };

                task.Ingredients.Add(ing);
            }

            foreach (var kitchenware in dtoTask.KitchenWare)
            {
                Kitchenware ware = new()
                {
                    Name = kitchenware.Name,
                    Quantity = kitchenware.Quantity,
                };

                task.Kitchenware.Add(ware);
            }

            Console.WriteLine("task " + task);

            var reply = await client.retryTaskAsync(new RetryTaskRequest { Prompt = prompt, Task = task });

            var returnTask = new Data.Models.Task
            {
                Id = ID,
                Title = reply.Task.Title,
                Description = reply.Task.Description,
                Duration = reply.Task.Duration,
                Difficulty = reply.Task.Difficulty,
                Dependencies = Helpers.Conversions.ConvertProtoDependencyListtoDependencyArray(reply.Task.Dependencies),
                Ingredients = Helpers.Conversions.ConvertProtoIngredientToIngredient(reply.Task.Ingredients),
                Kitchenware = Helpers.Conversions.ConvertProtoKitchenwareToKitchenware(reply.Task.Kitchenware)
            };

            return returnTask;

        }

        public async Task<string> RequestRegenerationOfAllSubTask(string subTasks, string ID)
        {
            return "";
        }
    }
}
