using GestorBiblioteca.Models;
using GestorBiblioteca.Models.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorBiblioteca.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly LibreriaContext _context;
        public CategoriasController(LibreriaContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Lista()
        {
            return View(await _context.Categorias.ToListAsync());
        }

        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (ModelState.IsValid)
                try
                {
                    _context.Add(categoria);
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "Categoria creada con exito";
                    return RedirectToAction("Lista");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Ha ocurrido un error");
                }
            return View(categoria);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "Categoria actualizada con exito";
                    return RedirectToAction("Lista");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(ex.Message, "Ocurrio un error al actualizar");
                }
            }
            return View(categoria);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }
            try
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = "Categoria eliminada con exito!";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "Error, no se pudeo eliminar el registro..");
            }
            return RedirectToAction(nameof(Lista));
        }
    }
}
