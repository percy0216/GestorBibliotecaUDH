
using Firebase.Auth;
using Firebase.Storage;

namespace GestorBiblioteca.Services
{
    public class ServicioImagen : IServicioImagen
    {
        public async Task<string> SubirImagen(Stream archivo, string nombre)
        {
            string email = "percyrojas@gmail.com"; 
            string clave = "percy123";
            string ruta = "bdd2-f39b6.firebasestorage.app";
            string api_key = "AIzaSyDzDt2eIqTCLQCqHB7oifomX81mePM7uNM";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })

                .Child("Fotos_Perfil")
                .Child(nombre)
                .PutAsync(archivo, cancellation.Token);

            var downloadURL = await task;
            return downloadURL;

        }
    }
}
