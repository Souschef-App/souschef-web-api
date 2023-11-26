using souschef.server.Data.DTOs;
using souschef.server.Data.Models;

namespace souschef.server.Helpers
{
    public static class Conversions
    {
        public static UserDTO ToUserDTO(ApplicationUser user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                SkillLevel = user.SkillLevel,
                /*               CurrentSessionId = user.CurrentSessionId.ToString(),
                               CurrentRecipe = null,*/
            };
        }

        public static Data.Models.Task? ToTask(TaskDTO _step)
        {
            if (_step.Ingredients != null && _step.KitchenWare != null)
            {
                return new Data.Models.Task
                {
                    Id = Guid.NewGuid(),
                    Title = _step.Title,
                    Description = _step.Description,
                    Ingredients = _step.Ingredients.ToList(),
                    Kitchenware = _step.KitchenWare.ToList(),
                    Difficulty = _step.Difficulty,
                    Duration = _step.Duration,
                    Dependencies = null, //_step.Dependencies

                };

            }

            return null;
        }

        public static long GetUnixTimeStamp(DateTime _dateTime)
        {
            return ((DateTimeOffset)_dateTime).ToUnixTimeSeconds();
        }

        #region GRPC Conversions

        public static List<Ingredient> ConvertProtoIngredientToIngredient(Google.Protobuf.Collections.RepeatedField<Services.SubtaskGeneration.Ingredient> protoIngredients)
        {
            List<Ingredient> ingredients = new();

            foreach (var protoIngredient in protoIngredients)
            {
                Services.SubtaskGeneration.SubTaskGenerationService.Units unit;

                if (!Enum.TryParse(protoIngredient.Unit, out unit))
                {
                    unit = Services.SubtaskGeneration.SubTaskGenerationService.Units.none;
                }

                var ingredient = new Ingredient
                {
                    Id = Guid.NewGuid(),
                    Name = protoIngredient.Name,
                    Quantity = protoIngredient.Quantity,
                    Unit = (int)unit
                };

                ingredients.Add(ingredient);
            }

            return ingredients;
        }

        public static List<Kitchenware> ConvertProtoKitchenwareToKitchenware(Google.Protobuf.Collections.RepeatedField<Services.SubtaskGeneration.Kitchenware> protoKitchenware)
        {
            List<Kitchenware> kitchenware = new();

            foreach (var protoKitchenItem in protoKitchenware)
            {
                var kitchenItem = new Kitchenware
                {
                    Name = protoKitchenItem.Name,
                    Quantity = protoKitchenItem.Quantity,
                };

                kitchenware.Add(kitchenItem);
            }

            return kitchenware;
        }

        public static Dependency[] ConvertProtoDependencyListtoDependencyArray(Google.Protobuf.Collections.RepeatedField<Services.SubtaskGeneration.Dependency> dependencies)
        {
            List<Dependency> deps = new();
            foreach (var dep in dependencies)
            {
                Dependency newDep = new()
                {
                    Title = dep.Name,
                    ID = new Guid(dep.UUID.ToByteArray()),
                };

                deps.Add(newDep);
            }

            return deps.ToArray();
        }

        #endregion

    }
}
