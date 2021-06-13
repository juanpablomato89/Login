using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.viewModels
{
    public class UsuarioRolModelo
    {
        public string UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }

        public bool EstaSeleccioando { get; set; }

        public string RolId { get;set; }
    }
}
