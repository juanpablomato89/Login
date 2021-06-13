using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Authorization;
using Login.Response;
using Microsoft.AspNetCore.Identity;
using Login.viewModels;

namespace Login.Controllers
{
    
    public class ProductoController : Controller
    {
        private readonly LoginContext _context;
        private readonly RoleManager<IdentityRole> gestionRoles;
        private readonly UserManager<Usuario> gestionUsuarios;

        public ProductoController(LoginContext context, RoleManager<IdentityRole> gestionRoles, UserManager<Usuario> gestionUsuarios)
        {
            _context = context;
            this.gestionRoles = gestionRoles;
            this.gestionUsuarios = gestionUsuarios;
        }

        // GET: Producto
        
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Producto.ToListAsync());
        }

        // GET: Producto/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.ID == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,Nombre,Descripcion,Cantidad,Slug,Precio")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Producto/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Descripcion,Cantidad,Slug,Precio")] Producto producto)
        {
            if (id != producto.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // GET: Producto/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .FirstOrDefaultAsync(m => m.ID == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.ID == id);
        }

        [Authorize]
        public async Task<IActionResult> Comprar(int id)
        {
            Respuesta respuesta = new Respuesta();
            Producto producto = _context.Producto.FirstOrDefault(x => x.ID.Equals(id));
            if (producto==null)
            {
                return NotFound();
            }
            Usuario usuario = await gestionUsuarios.GetUserAsync(User);

            OrdenCompra orden = new OrdenCompra();
            orden.Estado = Estado.Created;
            orden.Fecha = DateTime.Now;
            orden.Producto = producto;
            orden.Usuario = usuario;
            RedirectToAction("Create", "OrdenCompras");
            orden.ProductoID = producto.ID;
            orden.UsuarioID = usuario.Id;

            return View(orden);
        }

        [HttpPost, ActionName("Comprar")]
        [Authorize]
        public async Task<IActionResult> Comprar([Bind("ID,ProductoID,UsuarioID,Fecha,Estado,Cantidad")] OrdenCompra ordenCompra)
        {
            if (ModelState.IsValid)
            {
                Respuesta respuesta = new Respuesta();
                Producto producto = await _context.Producto.FindAsync(ordenCompra.ProductoID);
                if (ordenCompra.Cantidad > producto.Cantidad)
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = "La cantidad seleccionada no puede ser mayor a la existente o ya no existen productos disponibles";
                    return BadRequest(respuesta);
                }
                int cantidad = producto.Cantidad - ordenCompra.Cantidad;
                producto.Cantidad = cantidad;
                await Edit(producto.ID, producto);
                _context.OrdenCompra.Add(ordenCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Producto");
            }
            return View(ordenCompra);
        }
    }
}
