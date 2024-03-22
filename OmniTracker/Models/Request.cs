using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmniTracker.Models
{
    public class Request
    {
        public int Id { get; set; }
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "невозможное описание")]
        public string Description { get; set; }
        [Display(Name = "Дата создания")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Статус заявки")]
        [Required(ErrorMessage = "невозможный статус")]
        public string Status { get; set; }
        [Display(Name = "Срок устранения")]
        [Required(ErrorMessage = "невозможный срок устранения")]
        public string TermEimination { get; set; }

        [ForeignKey("UserId")]
        [Display(Name = "Пользователь")]
        public User User { get; set; }
    }
}
