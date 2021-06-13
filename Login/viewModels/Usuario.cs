using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Login.Models;

namespace Login.viewModels
{
    public class Usuario:IdentityUser
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Los Apellidos exceder los 50 caracteres")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        public List<OrdenCompra> OrdenCompras { get; set; }

    }
}
