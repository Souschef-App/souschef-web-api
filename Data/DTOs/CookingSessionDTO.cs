namespace souschef.server.Data.DTOs
{
    public class CookingSessionDTO
    {
        public string?   HostId       { get; set; }
        public string?   Name         { get; set; }
        public long      Date         { get; set; }
        public string[]? RecipeIds    { get; set; }
        public int?      OccasionType { get; set; }
    }
}
