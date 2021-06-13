using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.viewModels;
using Login.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Login.Controllers
{
    
    public class RolController : Controller
    {
        private readonly RoleManager<IdentityRole> gestionRoles;
        private readonly UserManager<Usuario> gestionUsuarios;       

        public RolController(RoleManager<IdentityRole> gestionRoles, UserManager<Usuario> gestionUsuarios)
        {
            this.gestionRoles = gestionRoles;
            this.gestionUsuarios = gestionUsuarios;
        }

        [HttpGet]
        [Route("Rol/CrearRolPolice")]
        public IActionResult CrearRol()
        {
            return View();
        }

        [HttpPost]
        [Route("Rol/CrearRolPolice")]
        public async Task<IActionResult> CrearRol(RolModelo role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identity = new IdentityRole
                {
                    Name = role.Rol
                };

                var resultado = await gestionRoles.CreateAsync(identity);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("ListarRoles", "Rol");

                }

                foreach (var item in resultado.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(role);
        }

        [HttpGet]
        [Route("Rol/ListarRoles")]
        public ActionResult ListarRoles()
        {
            var resultado=  gestionRoles.Roles;
            return View(resultado);
        }

        [HttpGet]
        [Route("Rol/EditarRol")]
        public async Task<IActionResult> EditarRol(string id)
        {
            var rol = await gestionRoles.FindByIdAsync(id);

            if (rol==null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id+ {id} no fue encontrado";
                return View("Error");
            }

            var model = new EditarRolModelo
            {
                Id = rol.Id,
                Nombre = rol.Name
            };

            foreach (var item in gestionUsuarios.Users)
            {
                if (await gestionUsuarios.IsInRoleAsync(item,rol.Name))
                {
                    model.Usuarios.Add(item.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("Rol/EditarRol")]
        public async Task<IActionResult> EditarRol(EditarRolModelo role)
        {
            var ro= await gestionRoles.FindByIdAsync(role.Id);

            if (ro == null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id+ {role.Id} no fue encontrado";
                return View("Error");
            }
            else
            {
                role.Nombre = ro.Name ;

                var resultado = await gestionRoles.UpdateAsync(ro);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("ListarRoles");
                }

                foreach (var item in resultado.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }    
            return View(role);
        }

        [HttpGet]
        [Route("Rol/EditarUsuarioRol")]
        public async Task<IActionResult> EditarUsuarioRol(string id)
        {
            ViewBag.roleId = id;
            var rol = await gestionRoles.FindByIdAsync(id);

            if (rol == null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id+ {id} no fue encontrado";
                return View("Error");
            }

            var model = new List<UsuarioRolModelo>();

            foreach (var item in gestionUsuarios.Users)
            {
                var usuariorolmodelo = new UsuarioRolModelo
                {
                    UsuarioId = item.Id,
                    UsuarioNombre = item.Nombre,
                    RolId = rol.Id
                  
                };

                if (await gestionUsuarios.IsInRoleAsync(item, rol.Name))
                {
                    usuariorolmodelo.EstaSeleccioando = true;
                }
                else
                    usuariorolmodelo.EstaSeleccioando = false;

                model.Add(usuariorolmodelo);
            }
            return View(model);
        }

        [HttpPost]
        [Route("Rol/EditarUsuarioRol")]
        public async Task<IActionResult> EditarUsuarioRol(List<UsuarioRolModelo> model,string roleId)
        {
            
            var rol = await gestionRoles.FindByIdAsync(roleId);

            if (rol == null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id= {roleId} no fue encontrado";
                return View("Error");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await gestionUsuarios.FindByIdAsync(model[i].UsuarioId);

                IdentityResult result = null;


                if (model[i].EstaSeleccioando && !(await gestionUsuarios.IsInRoleAsync(user, rol.Name)))
                {
                    result = await gestionUsuarios.AddToRoleAsync(user, rol.Name);
                }
                else if (!model[i].EstaSeleccioando && await gestionUsuarios.IsInRoleAsync(user, rol.Name))
                {
                    result = await gestionUsuarios.RemoveFromRoleAsync(user, rol.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditarRol", new { Id = roleId });
                    }
                }
            }
            return RedirectToAction("EditarRol", new { Id = roleId });


        }

        [HttpGet]
        [Route("Rol/ListaUsuarios")]
        public ActionResult ListaUsuarios()
        {
            var resultado = gestionUsuarios.Users;
            return View(resultado);
        }

        [HttpGet]
        [Route("Rol/EditarUsuario")]
        public async Task<IActionResult> EditarUsuario(string id)
        {
            var usuario = await gestionUsuarios.FindByIdAsync(id);

            if (usuario == null)
            {
                ViewBag.ErrorMessage = $"Usuario con el Id+ {id} no fue encontrado";
                return View("Error");
            }

            var usuarioClaim = await gestionUsuarios.GetClaimsAsync(usuario);

            var usuarioRoles = await gestionUsuarios.GetRolesAsync(usuario);



            var model = new EditarUsuarioModelo
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Apellidos = usuario.Apellidos,
                Usuario = usuario.UserName,
                Notificaciones = usuarioClaim.Select(c => c.Value).ToList(),
                Roles = usuarioRoles
                
            };           
            return View(model);
        }

        [HttpPost]
        [Route("Rol/EditarUsuario")]
        public async Task<IActionResult> EditarUsuario(EditarUsuarioModelo model)
        {
            var usuario = await gestionUsuarios.FindByIdAsync(model.Id);

            if (usuario == null)
            {
                ViewBag.ErrorMessage = $"Usuario con el Id= {model.Id} no fue encontrado";
                return View("Error");
            }
            else
            {
                usuario.Email = model.Email;
                usuario.UserName = model.Usuario;
                usuario.Nombre = model.Nombre;
                usuario.Apellidos = model.Apellidos;
                

                var resultado = await gestionUsuarios.UpdateAsync(usuario);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("ListaUsuarios");
                }

                foreach (var item in resultado.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("Rol/BorrarUsuario")]
        public async Task<IActionResult> BorrarUsuario(string Id)
        {
            var usuario = await gestionUsuarios.FindByIdAsync(Id);

            if (usuario == null)
            {
                ViewBag.ErrorMessage = $"Usuario con el Id= {Id} no fue encontrado";
                return View("Error");
            }
            else
            {
                var resultado = await gestionUsuarios.DeleteAsync(usuario);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("ListaUsuarios");
                }

                foreach (var item in resultado.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("ListaUsuarios");
        }

        [HttpPost]
        [Route("Rol/BorrarRol")]
        public async Task<IActionResult> BorrarRol(string Id)
        {
            var rol = await gestionRoles.FindByIdAsync(Id);

            if (rol == null)
            {
                ViewBag.ErrorMessage = $"El rol con el Id= {Id} no fue encontrado";
                return View("Error");
            }
            else
            {
                var resultado = await gestionRoles.DeleteAsync(rol);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("ListarRoles");
                }

                foreach (var item in resultado.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("ListarRoles");
        }

        [HttpGet]
        [Route("Rol/GestionarRolesUsuario")]
        public async Task<IActionResult> GestionarRolesUsuario(string IdUsuario)
        {
            ViewBag.IdUsuario = IdUsuario;
            var user = await gestionUsuarios.FindByIdAsync(IdUsuario);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id {IdUsuario} no fue encontrado";
                return View("Error");
            }

            var model = new List<RolUsuarioModelo>();

            foreach (var item in gestionRoles.Roles)
            {
                var rolusuario = new RolUsuarioModelo
                {
                    RolId = item.Id,
                    RolNombre=item.Name,
                };

                if (await gestionUsuarios.IsInRoleAsync(user, item.Name))
                {
                    rolusuario.EstaSeleccioando = true;
                }
                else
                    rolusuario.EstaSeleccioando = false;

                model.Add(rolusuario);
            }
            return View(model);
        }

        [HttpPost]
        [Route("Rol/GestionarRolesUsuario")]
        public async Task<IActionResult> GestionarRolesUsuario(List<RolUsuarioModelo> model, string IdUsuario)
        {

            var user = await gestionUsuarios.FindByIdAsync(IdUsuario);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id= {IdUsuario} no fue encontrado";
                return View("Error");
            }
            var roles = await gestionUsuarios.GetRolesAsync(user);

            var result = await gestionUsuarios.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "No podemos borrar usuarios con roles");
                return View(model);
            }

            result = await gestionUsuarios.AddToRolesAsync(user, model.Where(x => x.EstaSeleccioando).Select(y => y.RolNombre));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "No podemos añadir los roles al usuario seleccionado");
                return View(model);
            }

            
            return RedirectToAction("EditarUsuario", new { Id = IdUsuario });


        }

        [HttpGet]
        [Route("Rol/GestionarUsuarioClaims")]
        public async Task<IActionResult> GestionarUsuarioClaims(string IdUsuario)
        {
            
            var usuario = await gestionUsuarios.FindByIdAsync(IdUsuario);

            if (usuario == null)
            {
                ViewBag.ErrorMessage = $"El usuario con el Id= {IdUsuario} no fue encontrado";
                return View("Error");
            }

            var existingUserClaim = await gestionUsuarios.GetClaimsAsync(usuario);

            var modelo = new UsuarioClaimModelo
            {
                IdUsuario = IdUsuario
            };

            foreach (Claim claim in AlmacenClaim.Claims)
            {
                UsuarioClaim usuarioClaim = new UsuarioClaim
                {
                    TipoClaim = claim.Type
                };

                if (existingUserClaim.Any(c => c.Type == claim.Type))
                {
                    usuarioClaim.EstaSeleccionado = true;
                }
                else
                    usuarioClaim.EstaSeleccionado = false;
                modelo.Claims.Add(usuarioClaim);
            }
            return View(modelo);
        }

        [HttpPost]
        [Route("Rol/GestionarUsuarioClaims")]
        public async Task<IActionResult> GestionarUsuarioClaims(UsuarioClaimModelo model)
        {

            var user = await gestionUsuarios.FindByIdAsync(model.IdUsuario);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Rol con el Id= {model.IdUsuario} no fue encontrado";
                return View("Error");
            }
            var claims = await gestionUsuarios.GetClaimsAsync(user);

            var result = await gestionUsuarios.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "No podemos borrar los claims de este usuarios");
                return View(model);
            }

            result = await gestionUsuarios.AddClaimsAsync(user, model.Claims.Where(c => c.EstaSeleccionado).Select(c => new Claim(c.TipoClaim,c.TipoClaim)));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "No podemos añadir los roles al usuario seleccionado");
                return View(model);
            }
            return RedirectToAction("EditarUsuario", new { Id = model.IdUsuario });
        }







    }
}
