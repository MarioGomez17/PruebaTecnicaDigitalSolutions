using API_Backend.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        [HttpPost("CrearUsuario")]
        public string CrearUsuario(string Nombre)
        {
            Usuario Usuario = new();
            return Usuario.CrearUsuario(Nombre);
        }

        [HttpPost("SeguirUsuario")]
        public string SeguirUsuario(string NombreSeguidor, string NombreSeguido)
        {
            Usuario Usuario = new();
            return Usuario.SeguirUsuario(NombreSeguidor, NombreSeguido);
        }

        [HttpGet("TraerSeguidos")]
        public JsonResult TraerSeguidos(string Nombre)
        {
            Usuario Usuario = new();
            Usuario = Usuario.TraerUsuario(Nombre);
            return Json(Usuario.TraerSeguidos(Usuario.Id));
        }

    }
}
