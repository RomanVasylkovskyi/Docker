namespace ApiStore.Models.Account
{
    public class LoginViewModel
    {
        /// <summary>
        /// Пошта користувача
        /// </summary>
        /// <example>admin@gmail.com</example>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Пароль користувача
        /// </summary>
        /// <example>admin1</example>
        public string Password { get; set; } = string.Empty;
    }
}
