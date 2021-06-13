using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Authorization;

namespace Login.Controllers
{
    [Authorize]
    public class OrdenComprasController : Controller
    {
        private readonly LoginContext _context;
        
        public OrdenComprasController(LoginContext context)
        {
            _context = context;
        }

        // GET: OrdenCompras
        public async Task<IActionResult> Index()
        {
            var loginContext = _context.OrdenCompra.Include(o => o.Producto).Include(o => o.Usuario);
            return View(await loginContext.ToListAsync());
        }

        // GET: OrdenCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenCompra = await _context.OrdenCompra
                .Include(o => o.Producto)
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ordenCompra == null)
            {
                return NotFound();
            }

            return View(ordenCompra);
        }

        // GET: OrdenCompras/Create
        [Authorize(Policy ="")]
        public IActionResult Create()
        {
            ViewData["ProductoID"] = new SelectList(_context.Producto, "ID", "Descripcion");
            ViewData["UsuarioID"] = new SelectList(_context.Users, "Id", "Nombre");
            return View();
        }

        // POST: OrdenCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProductoID,UsuarioID,Fecha,Estado,Cantidad")] OrdenCompra ordenCompra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordenCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoID"] = new SelectList(_context.Producto, "ID", "Descripcion", ordenCompra.ProductoID);
            ViewData["UsuarioID"] = new SelectList(_context.Users, "Id", "Nombre", ordenCompra.UsuarioID);
            return View(ordenCompra);
        }

        // GET: OrdenCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenCompra = await _context.OrdenCompra.FindAsync(id);
            if (ordenCompra == null)
            {
                return NotFound();
            }
            ViewData["ProductoID"] = new SelectList(_context.Producto, "ID", "Descripcion", ordenCompra.ProductoID);
            ViewData["UsuarioID"] = new SelectList(_context.Users, "Id", "Nombre", ordenCompra.UsuarioID);
            return View(ordenCompra);
        }

        // POST: OrdenCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProductoID,UsuarioID,Fecha,Estado,Cantidad")] OrdenCompra ordenCompra)
        {
            if (id != ordenCompra.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                 var ordenCompras = await _context.OrdenCompra
                .Include(o => o.Producto)
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);
                    if (ordenCompras != null)
                    {
                        if (ordenCompra.Estado.Equals(Estado.Canceled))
                        {
                            ordenCompras.Producto.Cantidad += ordenCompra.Cantidad;
                            ordenCompras.Cantidad = 0;
                            ordenCompras.Estado = Estado.Canceled;
                        }
                        else if (ordenCompra.Estado.Equals(Estado.Confirmed))
                        {
                            ordenCompras.Estado = Estado.Confirmed;
                        }                       
                        
                        _context.Update(ordenCompras);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenCompraExists(ordenCompra.ID))
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
            ViewData["ProductoID"] = new SelectList(_context.Producto, "ID", "Descripcion", ordenCompra.ProductoID);
            ViewData["UsuarioID"] = new SelectList(_context.Users, "Id", "Nombre", ordenCompra.UsuarioID);
            return View(ordenCompra);
        }

        // GET: OrdenCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordenCompra = await _context.OrdenCompra
                .Include(o => o.Producto)
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ordenCompra == null)
            {
                return NotFound();
            }

            return View(ordenCompra);
        }

        // POST: OrdenCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordenCompra = await _context.OrdenCompra.FindAsync(id);
            _context.OrdenCompra.Remove(ordenCompra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdenCompraExists(int id)
        {
            return _context.OrdenCompra.Any(e => e.ID == id);
        }
    }
}
