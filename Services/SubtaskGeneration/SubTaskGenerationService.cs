using System.Diagnostics;
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

        //static Data.Models.Dependency[] ConvertProtoDependencyListtoDependencyArray(Google.Protobuf.Collections.RepeatedField<Dependency> dependencies)
        //{
        //    List<Data.Models.Dependency> deps = new();
        //    foreach (var dep in dependencies)
        //    {
        //        Data.Models.Dependency newDep = new Data.Models.Dependency()
        //        {
        //            Title = dep.Name,
        //            ID = new Guid(dep.UUID.ToByteArray()),
        //        };

        //        deps.Add(newDep);
        //    }

        //    return deps.ToArray();
        //}

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

        //static List<Data.Models.Ingredient> ConvertProtoIngredientToIngredient(Google.Protobuf.Collections.RepeatedField<Ingredient> protoIngredients)
        //{
        //    List<Data.Models.Ingredient> ingredients = new();

        //    foreach (var protoIngredient in protoIngredients)
        //    {
        //        Units unit;

        //        if (!Enum.TryParse(protoIngredient.Unit, out unit))
        //        {
        //            unit = Units.none;
        //        }

        //        var ingredient = new Data.Models.Ingredient
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = protoIngredient.Name,
        //            Quantity = protoIngredient.Quantity,
        //            Unit = (int)unit
        //        };

        //        ingredients.Add(ingredient);
        //    }

        //    return ingredients;
        //}

        //static List<Data.Models.Kitchenware> ConvertProtoKitchenwareToKitchenware(Google.Protobuf.Collections.RepeatedField<Kitchenware> protoKitchenware)
        //{
        //    List<Data.Models.Kitchenware> kitchenware = new();

        //    foreach (var protoKitchenItem in protoKitchenware)
        //    {
        //        var kitchenItem = new Data.Models.Kitchenware
        //        {
        //            Name = protoKitchenItem.Name,
        //            Quantity = protoKitchenItem.Quantity,
        //        };

        //        kitchenware.Add(kitchenItem);
        //    }

        //    return kitchenware;
        //}

        public async Task<Data.Models.Task> RequestRegenerationOfSubTask(string prompt, TaskDTO dtoTask)
        {
            if (dtoTask.Ingredients == null || dtoTask.KitchenWare == null)
                throw new Exception("INgredients NULL");

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

                Ingredient ing = new()
                {
                    Name = ingredient.Name,
                    Quantity = ingredient.Quantity,
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
                Id = new Guid(reply.Task.Uuid.ToByteArray()),
                Title = reply.Task.Title,
                Description = reply.Task.Description,
                Duration = 0,
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
