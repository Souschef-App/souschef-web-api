using System.Diagnostics;
using Grpc.Net.Client;
using OpenAI_API;



namespace souschef.server.Services.SubtaskGeneration
{
    public class SubTaskGenerationService : ISubTaskGenerationService
    {
        OpenAIAPI api;

        readonly Dictionary<string, OpenAI_API.Chat.Conversation> chats;

        public SubTaskGenerationService()
        {
            Debug.WriteLine("SubTaskGenerationService");

            api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_KEY"));
            chats = new Dictionary<string, OpenAI_API.Chat.Conversation>();

            Debug.WriteLine("CREATING CHANNEL");
        }

        public void StartInferenceSession(string ID)
        {
            var chat = api.Chat.CreateConversation();

            chats[ID] = chat;

        }

        public async Task<string> RequestSubTaskGeneration(string recipeStep)
        {
            using var channel = GrpcChannel.ForAddress("http://ai:50051");
            var client = new RecipeGeneration.RecipeGenerationClient(channel);

            Debug.WriteLine("REQUESTIN REPLY");
            var reply = await client.getRecipeBreakDownAsync(new RecipeBreakdownRequest { Description = "cook the chicken" });
            Console.WriteLine("Greeting: " + reply.Message);
            return reply.Message;
        }

        public async Task<string> RequestRegenerationOfSubTask(string subTask, string ID)
        {
            var myChat = chats[ID];
            myChat.AppendSystemMessage("your role is a bot that processes a recipe into a set of smaller individualized tasks");
            myChat.AppendSystemMessage("you'll return a list of tasks items that each have the following header: Title, Description, Ingredients, Kitchenware, Duration, Difficulty, Dependencies");
            myChat.AppendSystemMessage("Title will be the name of task, Description will be the instructions, Ingredients will be a list if the ingredients just for that step, Kitchenware will be a list of all the utensils and other tools, Duration will be an estimate in minutes for the task, Difficulty will be an estimate from 0-3 on how hard the task is, Dependencies is a list of any other tasks that need to be completed before this task indicated as an index number");
            myChat.AppendUserInput("The result returned should be in json format");
            myChat.AppendSystemMessage("this is a step that was genenerated before");
            myChat.AppendSystemMessage("can you retry generating this step in a different way?");
            myChat.AppendUserInput(subTask);
            myChat.AppendSystemMessage("can you return this list as JSON and only return the JSON in your response (NO OTHER TEXT), the list should be wrapped in a object with key recipe to access the list");

            return await myChat.GetResponseFromChatbotAsync();
        }

        public async Task<string> RequestRegenerationOfAllSubTask(string subTasks, string ID)
        {
            var myChat = chats[ID];
            myChat.AppendSystemMessage("your role is a bot that processes a recipe into a set of smaller individualized tasks");
            myChat.AppendSystemMessage("you'll return a list of tasks items that each have the following header: Title, Description, Ingredients, Kitchenware, Duration, Difficulty, Dependencies");
            myChat.AppendSystemMessage("Title will be the name of task, Description will be the instructions, Ingredients will be a list if the ingredients just for that step, Kitchenware will be a list of all the utensils and other tools, Duration will be an estimate in minutes for the task, Difficulty will be an estimate from 0-3 on how hard the task is, Dependencies is a list of any other tasks that need to be completed before this task indicated as an index number");
            myChat.AppendUserInput("The result returned should be in json format");
            myChat.AppendSystemMessage("this is a step that was genenerated before");
            myChat.AppendSystemMessage("can you retry generating this step in a different way?");
            myChat.AppendUserInput(subTasks);
            myChat.AppendSystemMessage("can you return this list as JSON and only return the JSON in your response (NO OTHER TEXT), the list should be wrapped in a object with key recipe to access the list");

            return await myChat.GetResponseFromChatbotAsync();
        }
    }
}
