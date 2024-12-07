using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using GestorBiblioteca.Models;
using GestorBiblioteca.Models.Entidades;
using GestorBiblioteca.Services;
using System.Security.Claims;
using NuGet.Common;

namespace GestorBiblioteca.Controllers
{
    public class LoginController : Controller
    {
        private readonly IServicioUsuario _servicioUsuario;
        private readonly IServicioImagen _servicioImagen;
        private readonly LibreriaContext _context;

        public LoginController(IServicioUsuario servicioUsuario, IServicioImagen servicioImagen, LibreriaContext context)
        {
            _context = context;
            _servicioUsuario = servicioUsuario;
            _servicioImagen = servicioImagen;
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario, IFormFile Imagen)
        {
            Stream image = Imagen.OpenReadStream();
            string urlImagen = await _servicioImagen.SubirImagen(image, Imagen.FileName);

            usuario.Clave = Utilidades.EncriptarClave(usuario.Clave);
            usuario.URLFotoPerfil = urlImagen;

            Usuario usuarioCreado = await _servicioUsuario.SaveUsuario(usuario);

            if (usuarioCreado.Id > 0)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {
            
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6LfEE4wqAAAAAHAihN0YL-hqxE_UGZBcFW2BW0V4"; 

            
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}",
                    null);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

                if (result.success != "true")
                {
                    ViewData["Mensaje"] = "Verificación CAPTCHA fallida. Inténtalo de nuevo.";
                    return View(); // Devuelve la vista con el mensaje de error
                }
            }

            // Continuar con la autenticación del usuario si el CAPTCHA es válido
            Usuario usuarioEncontrado = await _servicioUsuario.GetUsuario(correo, Utilidades.EncriptarClave(clave));

            if (usuarioEncontrado == null)
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View(); // Devuelve la vista con mensaje de error
            }

            // Crear las Claims para el usuario autenticado
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuarioEncontrado.NombreUsuario),
                new Claim("FotoPerfil", usuarioEncontrado.URLFotoPerfil),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            // Iniciar sesión utilizando las Claims y las propiedades de autenticación
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            // Redirigir al usuario a la página de inicio
            return RedirectToAction("Index", "Home");
        }



    }
}