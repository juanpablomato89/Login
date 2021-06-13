using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login.viewModels
{
    public class EditarUsuarioModelo
    {
        public EditarUsuarioModelo()
        {
            Notificaciones = new List<string>();
            Roles = new List<string>();

        }


        public string Id { get; set; }

        [Required]
        public string Usuario { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellidos { get; set; }
        public List<string> Notificaciones { get; set; }

        public IList<string> Roles { get; set; }
    }
}
