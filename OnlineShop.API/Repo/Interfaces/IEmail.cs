namespace OnlineShop.API.Repo.Interfaces
{
    public interface IEmail
    {
        void SendMail(string body, string reciever, string subject);
    }
}
