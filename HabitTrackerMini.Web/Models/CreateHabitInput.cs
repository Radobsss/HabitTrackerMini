using System.ComponentModel.DataAnnotations;

namespace HabitTrackerMini.Web.Models
{
    public class CreateHabitInput
    {
        [Display(Name = "Име")]
        [Required(ErrorMessage = "Въведи име на навика.")]
        [StringLength(100, ErrorMessage = "Името може да съдържа до 100 знака.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Въведи категория.")]
        [StringLength(60, ErrorMessage = "Категорията може да съдържа до 60 знака.")]
        public string Category { get; set; } = string.Empty;

        [Display(Name = "Дни седмично")]
        [Range(1, 7, ErrorMessage = "Седмичната цел трябва да е между 1 и 7 дни.")]
        public int WeeklyGoal { get; set; } = 3;

        [Display(Name = "Описание (по желание)")]
        [StringLength(500, ErrorMessage = "Описанието може да съдържа до 500 знака.")]
        public string? Description { get; set; }
    }
}
