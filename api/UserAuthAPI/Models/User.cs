namespace UserAuthAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public required string Username { get; set; }

        public required string UserPassword { get; set; }
    }
}
