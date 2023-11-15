using System.Diagnostics;
using System.Reflection.Metadata;
using Grpc.Net.Client;
using Microsoft.Extensions.ObjectPool;

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
                    Duration = 0,
                    Difficulty = reply.Tasks[i].Difficulty,
                    Dependencies = GetGUIDArrayFromByteString(reply.Tasks[i].Dependencies),
                    Ingredients = convertProtoIngredientToIngredient(reply.Tasks[i].Ingredients),
                    Kitchenware = convertProtoKitchenwareToKitchenware(reply.Tasks[i].Kitchenware)
                };

                Console.WriteLine("Id " + task.Id);
                tasks.Add(task);
            }

            Console.WriteLine("Count " + tasks.Count);

            return tasks;
        }

        static Guid[] GetGUIDArrayFromByteString(Google.Protobuf.Collections.RepeatedField<Google.Protobuf.ByteString> bytesStrings)
        {
            List<Guid> uuids = new();
            foreach (var bytesString in bytesStrings)
            {
                uuids.Add(new Guid(bytesString.ToByteArray()));
            }

            return uuids.ToArray();
        }

        List<Data.Models.Ingredient> convertProtoIngredientToIngredient(Google.Protobuf.Collections.RepeatedField<Ingredient> protoIngredients)
        {
            List<Data.Models.Ingredient> ingredients = new();

            foreach (var protoIngredient in protoIngredients)
            {
                var ing = new Data.Models.Ingredient
                {
                    Name = protoIngredient.Name,
                    Quantity = protoIngredient.Quantity,
                    Unit = 0
                };

                ingredients.Add(ing);
            }

            return ingredients;
        }

        List<Data.Models.Kitchenware> convertProtoKitchenwareToKitchenware(Google.Protobuf.Collections.RepeatedField<Kitchenware> protoKitchenware)
        {
            List<Data.Models.Kitchenware> kitchenware = new();

            foreach (var protoKitchenItem in protoKitchenware)
            {
                var kitchenItem = new Data.Models.Kitchenware
                {
                    Name = protoKitchenItem.Name,
                    Quantity = protoKitchenItem.Quantity,
                };

                kitchenware.Add(kitchenItem);
            }

            return kitchenware;
        }

        public async Task<string> RequestRegenerationOfSubTask(string subTask, string ID)
        {
            return "";
        }

        public async Task<string> RequestRegenerationOfAllSubTask(string subTasks, string ID)
        {
            return "";
        }
    }
}
