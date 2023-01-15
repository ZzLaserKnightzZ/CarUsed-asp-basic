namespace CarUsed.Models.OutPutModels
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set;}
    }
}
