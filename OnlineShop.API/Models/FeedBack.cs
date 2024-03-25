using System.ComponentModel.DataAnnotations;

namespace OnlineShop.API.Models
{
    public class FeedBack
    {
        public int Id { get; set; }
        public int AuthorID { get; set; }
        public string ProductID { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
    }
}
