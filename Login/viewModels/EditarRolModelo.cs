using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login.viewModels
{
    public class EditarRolModelo
    {
        public EditarRolModelo()
        {
            Usuarios = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage ="El nombre del Rol es Obligatorio")]
        public string Nombre { get; set; }

        public List<string> Usuarios { get; set; }


    }
}
