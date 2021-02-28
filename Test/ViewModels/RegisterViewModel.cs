using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Test.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nom d'utilisateur")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Nom")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Date de naissance")]
        public DateTime DateOfBirth { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Bio")]
        public string Notes { get; set; }

        [Required]
        [Display(Name = "Mot de passe")]
        [MinLength(6, ErrorMessage = "Le mot de passe ne doit pas être moins de 6 caractères")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [Display(Name = "Confirmation mot de passe")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}