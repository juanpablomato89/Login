using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.viewModels
{
    public class UsuarioClaimModelo
    {
        public UsuarioClaimModelo()
        {
            Claims = new List<UsuarioClaim>();
        }

        public string IdUsuario { get; set; }
        public List<UsuarioClaim> Claims { get; set; }
    }
}
