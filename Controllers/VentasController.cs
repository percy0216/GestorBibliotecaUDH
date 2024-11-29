﻿using GestorBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorBiblioteca.Controllers
{
    public class VentasController : Controller
    {
        private readonly LibreriaContext _context;

        public VentasController(LibreriaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Lista()
        {
            decimal sumaTotal = _context.Ventas.Sum(v => v.Total);
            ViewBag.sumaTotal = sumaTotal;
            decimal sumaDiaria = _context.Ventas
                .Where(v => v.Fecha.Date == DateTime.Today)
                .Sum(v => v.Total);
            ViewBag.SumaDiaria = sumaDiaria;
            return View(await _context.Ventas
                .Include(v => v.Libro).ThenInclude(l => l.Autor)
                .Include(v => v.Libro).ThenInclude(l => l.Categoria)
                .Include(v => v.Usuario)
                .ToListAsync());
        }
    }
}
