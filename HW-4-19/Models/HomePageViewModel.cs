using WebAuthentication.Data;

namespace HW_4_19.Models
{
    public class HomePageViewModel
    {
        public List<Ad> Ads = new List<Ad>();
       public int UserId { get; set; }
    }
}
