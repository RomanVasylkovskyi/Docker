using System.ComponentModel.DataAnnotations;

namespace WebHalk.Models.Account
{
    public class RegisterViewModel
    {
        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Вкажіть ім'я")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Прізвище")]
        [Required(ErrorMessage = "Вкажіть ім'я")]
        public string LastName { get; set; } = null!;
        [Display(Name = "Електронна пошта")]
        [Required(ErrorMessage = "Вкажіть назву пошти")]
        [EmailAddress(ErrorMessage = "Пошту вказано не коректно")]
        public string Email { get; set; } = null!;
        [Display(Name = "Пароль")]
        [Required(ErrorMessage ="Вкажіть пароль")]
        [MinLength(6, ErrorMessage = "Довжина паролю має бути мін 6 символів")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Display(Name = "Підтвердежння пароля")]
        [Required(ErrorMessage ="Вкажіть пароль повторно")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
