namespace GestorBiblioteca.Services
{
    public interface IServicioImagen
    {
        Task<string> SubirImagen(Stream archivo, string nombre);
    }
}
