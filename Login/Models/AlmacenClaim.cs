using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Login.Models
{
    public class AlmacenClaim
    {
        public static List<Claim> Claims = new List<Claim>()
        {
            new Claim("Crear Rol","Crear Rol"),
            new Claim("Editar Rol","Editar Rol"),
            new Claim("Borrar Rol","Borrar Rol")
        }; 
    }
}
