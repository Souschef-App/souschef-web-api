using OpenAI_API;


namespace souschef.server.Services
{
    public class SubTaskGenerationService : ISubTaskGenerationService
    {

        OpenAIAPI api;
        readonly Dictionary<string, OpenAI_API.Chat.Conversation> chats;

        public SubTaskGenerationService()
        {
            api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_KEY"));
            chats = new Dictionary<string, OpenAI_API.Chat.Conversation>();
        }

        public void StartInferenceSession(string ID)
        {
            var chat = api.Chat.CreateConversation();

            chats[ID] = chat;
        }

        public async Task<string> RequestSubTaskGeneration(string recipeStep)
        {
            var myChat = api.Chat.CreateConversation();
            myChat.AppendSystemMessage("your role is a bot that processes a recipe into a set of smaller individualized tasks");
            myChat.AppendSystemMessage("you'll return a list of tasks items that each have the following header: Title, Description, Ingredients, Kitchenware, Duration, Difficulty, Dependencies");
            myChat.AppendSystemMessage("Title will be the name of task, Description will be the instructions, Ingredients will be a list if the ingredients just for that step, Kitchenware will be a list of all the utensils and other tools, Duration will be an estimate in minutes for the task, Difficulty will be an estimate from 0-3 on how hard the task is, Dependencies is a list of any other tasks that need to be completed before this task indicated as an index number");
            myChat.AppendUserInput("The result returned should be in json format");
            myChat.AppendSystemMessage("can you break down the following recipe into the list as described");
            myChat.AppendSystemMessage("each of the provided steps should be broken down even further into smaller steps");
            myChat.AppendUserInput(recipeStep);
            myChat.AppendSystemMessage("can you return this list as JSON and only return the JSON in your response (NO OTHER TEXT), the list should be wrapped in a object with key recipe to access the list");

            return await myChat.GetResponseFromChatbotAsync();
        }
    }
}
