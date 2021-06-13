using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login.viewModels
{
    public class RegistroModelo
    {
        [Required(ErrorMessage = "El Nombre del Usuario es un campo Obligatorio")]
        [Display(Name = "Nombre del Usurio"), MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los Apellidos del Usuario es un campo Obligatorio")]
        [Display(Name = "Apellidos del Usurio"), MinLength(3, ErrorMessage = "El o los Apellidos deben tener al menos 3 caracteres")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage ="El Email es Obligatorio")]
        [EmailAddress]
        [Display(Name ="Correo Electronico")]
        [Remote(action: "ComprobarEmail", controller:"Cuentas")]
        public string Email { get; set; }

        [Required(ErrorMessage ="La password es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Es obligatorio confirmar la password")]
        [DataType(DataType.Password)]
        [Display(Name ="Repetir Password")]
        [Compare("Password",ErrorMessage ="La password y la password de confirmacion no coinciden")]
        public string PasswordValidar { get; set; }
    }
}
