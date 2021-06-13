using Login.Models;
using Login.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Login.Controllers
{
    [AllowAnonymous]
    public class CuentasController : Controller
    {
        private readonly UserManager<Usuario> gestionUsuarios;

        private readonly SignInManager<Usuario> gestionLogin;      


        public CuentasController(UserManager<Usuario> gestionUsuarios, SignInManager<Usuario> gestionLogin)
        {
            this.gestionUsuarios = gestionUsuarios;
            this.gestionLogin = gestionLogin;
            
        }

        [HttpGet]
        [Route("Cuentas/Registro")]        
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("Cuentas/Registro")]
        public async Task<IActionResult> Registro(RegistroModelo model)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nombre = model.Nombre,
                    Apellidos = model.Apellidos,
                                       
                };

                var resultado = await gestionUsuarios.CreateAsync(usuario, model.Password);
                

                if (resultado.Succeeded)
                {
                    if (gestionLogin.IsSignedIn(User) && User.IsInRole("Adminstrador"))
                    {
                        return RedirectToAction("ListaUsuarios", "Rol");
                    }

                    await gestionLogin.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("Index", "home");

                }

                foreach (var item in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("Cuentas/CerrarSesion")]
        public async Task<IActionResult> CerrarSesion()
        {
            await gestionLogin.SignOutAsync();
            return RedirectToAction("Index", "home");
        }

        [HttpGet]
        [Route("Cuentas/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Cuentas/Login")]
        public async Task<IActionResult> Login(LoginModelo login)
        {
            if (ModelState.IsValid)
            {
                var resultado = await gestionLogin.PasswordSignInAsync(login.Email, login.Password, login.Recuerdame, false);
               
                if (resultado.Succeeded)
                {
                     Usuario  usuario =await gestionUsuarios.FindByEmailAsync(login.Email);
                     await gestionLogin.SignInAsync(usuario, isPersistent: false);

                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Inicio de Sesion no valido");
            }

            return View(login);
        }

        [AcceptVerbs("Get", "Post")]
        [Route("Cuentas/ComprobarEmail")]
        public async Task<ActionResult> ComprobarEmail(string email)
        {
            var user = await gestionUsuarios.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"El Email {email} no esta disponible"); 
            }
        }

        [HttpGet]
        [Route("Cuentas/AccesoDenegado")]
        public ActionResult AccesoDenegado()
        {
            return View();
        }
    }
}
