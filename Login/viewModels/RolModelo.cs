using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login.viewModels
{
    public class RolModelo
    {
        [Required(ErrorMessage ="Este campo es obligatorio")]
        [Display(Name = "Rol")]
        public string Rol { get; set; }
    }
}
