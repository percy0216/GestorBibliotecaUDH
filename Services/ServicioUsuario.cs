﻿using GestorBiblioteca.Models;
using GestorBiblioteca.Models.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestorBiblioteca.Services
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly LibreriaContext _context;

        public ServicioUsuario(LibreriaContext context)
        {
            _context = context;
        }

        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usuario = await _context.Usuarios.Where(u => u.Correo == correo && u.Clave == clave)
               .FirstOrDefaultAsync();
            return usuario;
        }
        public async Task<Usuario> GetUsuario(string nombreUsuario)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }
        public async Task<Usuario> SaveUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
