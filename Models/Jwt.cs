namespace Project_for_Aceleration_Csharp_Tryitter.Models
{
    [Serializable]
    public class JwtAuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
