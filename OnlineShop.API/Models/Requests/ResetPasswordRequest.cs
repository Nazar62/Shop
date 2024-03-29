namespace OnlineShop.API.Models.Requests
{
    public class ResetPasswordRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Guid Token {  get; set; }
    }
}
