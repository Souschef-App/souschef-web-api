// DTOs/SessionDto.cs
public class SessionDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
}
public class UpdateSessionDto
{
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
}