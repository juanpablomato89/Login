using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Login.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Login.viewModels;

namespace Login.Data
{
    public class LoginContext : IdentityDbContext<Usuario>
    {
        public LoginContext (DbContextOptions<LoginContext> options)
            : base(options)
        {
        }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<OrdenCompra> OrdenCompra { get; set; }
    }
}
