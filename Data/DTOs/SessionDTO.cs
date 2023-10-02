// DTOs/SessionDto.cs
public class SessionDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    // Add other session properties you want to expose to the client
}
// Create a DTO for updating a session
public class UpdateSessionDto
{
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    // Add properties for updating other session properties as needed
}