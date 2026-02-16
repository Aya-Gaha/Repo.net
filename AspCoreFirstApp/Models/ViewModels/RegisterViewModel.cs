using System.ComponentModel.DataAnnotations;

namespace AspCoreFirstApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "L'adresse email est requise")]
        [EmailAddress(ErrorMessage = "Adresse email invalide")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Ville")]
        public string? City { get; set; }
    }
}