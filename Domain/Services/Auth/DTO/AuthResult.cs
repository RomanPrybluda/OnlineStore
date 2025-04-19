namespace Domain
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public string? Token { get; set; }

    }
}
