using System.ComponentModel.DataAnnotations;

namespace CarUsed.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IList<Car> Cars { get; set; }
    }
}
