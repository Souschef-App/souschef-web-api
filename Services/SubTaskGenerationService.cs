using OpenAI_API;


namespace souschef.server.Services
{
    public class SubTaskGenerationService : ISubTaskGenerationService
    {

        OpenAIAPI api;
        readonly Dictionary<string, OpenAI_API.Chat.Conversation> chats;

        public SubTaskGenerationService()
        {
            api = new OpenAIAPI();
            chats = new Dictionary<string, OpenAI_API.Chat.Conversation>();
        }

        public void StartInferenceSession(string ID)
        {
            var chat = api.Chat.CreateConversation();

            chats[ID] = chat;
        }

        public async Task<string> RequestSubTaskGeneration(string ID, string recipeStep)
        {
            var myChat = chats[ID];
            myChat.AppendSystemMessage("Break this recipe down into subtasks");
            myChat.AppendUserInput(recipeStep);

            return await myChat.GetResponseFromChatbotAsync();
        }
    }
}
