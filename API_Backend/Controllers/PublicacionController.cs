using API_Backend.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicacionController : Controller
    {
        [HttpGet("TraerPublicaciones")]
        public string TraerPublicaciones(int IdUsuario)
        {
            Publicacion Publicacion = new();
            return Publicacion.TraerPublicaciones(IdUsuario);
        }

        [HttpPost("HacerPublicacion")]
        public string HacerPublicacion(string Nombre, string Mensaje)
        {
            Usuario Usuario = new();
            Usuario = Usuario.TraerUsuario(Nombre);
            Publicacion Publicacion = new();
            return Publicacion.HacerPublicacion(Usuario, Mensaje);
        }

    }
}
