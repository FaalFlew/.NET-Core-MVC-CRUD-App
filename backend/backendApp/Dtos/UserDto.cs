namespace backendApp.Dtos
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? DateJoined { get; set; }
    }
}