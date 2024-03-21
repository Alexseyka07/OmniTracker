using System.ComponentModel.DataAnnotations;

namespace OmniTracker.Models
{

    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "невозможный email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "невозможный пароль")]
        public string Password { get; set; }
        [Display(Name = "Статус")]
        public string Role { get; set; }

        public List<Request> Requests { get; set; }

    }
}
