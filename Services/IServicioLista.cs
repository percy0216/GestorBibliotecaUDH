using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestorBiblioteca.Services
{
    public interface IServicioLista
    {
        Task<IEnumerable<SelectListItem>> GetListaAutores();
        Task<IEnumerable<SelectListItem>> GetListaCategorias();
    }
}


