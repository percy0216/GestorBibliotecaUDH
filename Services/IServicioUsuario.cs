﻿using GestorBiblioteca.Models.Entidades;

namespace GestorBiblioteca.Services
{
    public interface IServicioUsuario
    {
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> SaveUsuario(Usuario usuario);
        Task<Usuario> GetUsuario(string nombreUsuario);

    }
}
