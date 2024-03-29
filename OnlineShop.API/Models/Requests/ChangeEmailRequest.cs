namespace OnlineShop.API.Models.Requests
{
    public class ChangeEmailRequest
    {
        public int Id { get; set; }
        public string NewEmail { get; set; }
        public string Password { get; set; }
    }
}
