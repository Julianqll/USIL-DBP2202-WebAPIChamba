using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPIChamba.Models;

namespace WebAPIChamba.Controllers
{
    [Route("Api/[Controller]")]
    public class UsuarioController : Controller
    {
        private readonly sql10579654Context Context;

        public UsuarioController(sql10579654Context context)
        {
            Context = context;
        }

        [HttpGet]
        public IActionResult PerfilUsuario()
        {
            String? sesion = HttpContext.Session.GetString("usuario");
            var sesionJson = JsonConvert.DeserializeObject<Usuario>(sesion);
            return Ok(sesionJson);
        }


        [HttpPost]
        public IActionResult RegistroUsuarioPost(Usuario usuario)
        {
            Context.Usuarios.Add(usuario);
            Context.SaveChanges();
            Login login = new Login();
            login.Correo = usuario.CorreoUsuario;
            login.Contraseña = Request.Form["contrasenaField"];
            login.Rol = "U";
            Context.Logins.Add(login);
            Context.SaveChanges();
            return Redirect("/login");
        }

        [HttpGet, Route("/buscarPuestos")]
        public IActionResult BuscarPuestos()
        {
            var puestos = Context.Puestos;
            ViewBag.Puestos = puestos;
            return View();
        }

        [HttpGet, Route("/postular/{puesto}")]
        public IActionResult Postular(int puesto)
        {
            ViewData["id"] = puesto;
            return View();
        }

        [HttpPost, Route("/postular/{puesto}")]
        public IActionResult PostularPost(Postulacion postulacion, int puesto)
        {
            System.Console.WriteLine("si funcon");

            if (ModelState.IsValid)
            {
                System.Console.WriteLine("si moddelo");

                String? sesion = HttpContext.Session.GetString("usuario");
                var sesionJson = JsonConvert.DeserializeObject<Usuario>(sesion);
                postulacion.Postulante = sesionJson.IdUsuario;
                postulacion.Puesto = puesto;
                postulacion.EstadoPostulacion = "Enviada";
                postulacion.Observacion = "Pendiente";
                Context.Postulacions.Add(postulacion);
                Context.SaveChanges();
                return Redirect("/misPostulaciones");

            }
            return View("Postular");
        }

        [HttpGet, Route("/misPostulaciones")]
        public IActionResult MisPostulaciones()
        {
            String? sesion = HttpContext.Session.GetString("usuario");
            var sesionJson = JsonConvert.DeserializeObject<Usuario>(sesion);
            var postulaciones = (from TablaPostulaciones in Context.Postulacions
                                 join TablaPuestos in Context.Puestos
                                 on TablaPostulaciones.Puesto equals TablaPuestos.IdPuesto
                                 where
                                 TablaPostulaciones.Postulante == sesionJson.IdUsuario
                                 select new
                                 {
                                     Nombre = TablaPuestos.NombrePuesto,
                                     Descripcion = TablaPuestos.DescripcionPuesto,
                                     Id = TablaPostulaciones.IdPostulacion,
                                     Estado = TablaPostulaciones.EstadoPostulacion,
                                     Observacion = TablaPostulaciones.Observacion,
                                 }).AsEnumerable();
            ViewBag.Postulaciones = postulaciones;

            return View();
        }
        [HttpPost, Route("/editarPerfilUsuario")]
        public IActionResult EditarPerfil()
        {
            String? sesion = HttpContext.Session.GetString("usuario");
            var sesionJson = JsonConvert.DeserializeObject<Usuario>(sesion);
            var resultado = (from TablaUsuarios in Context.Usuarios
                             where TablaUsuarios.IdUsuario == sesionJson.IdUsuario
                             select TablaUsuarios).Single();
            resultado.ApodoUsuario = Request.Form["apodoEmpresa"];
            resultado.NombresUsuario = Request.Form["nombreEmpresa"];
            resultado.ApellidosUsuario = Request.Form["apellidos"];
            resultado.CorreoUsuario = Request.Form["correoEmpresa"];
            resultado.BiografiaUsuario = Request.Form["bioEmpresa"];
            resultado.FotoPerfilUsuario = Request.Form["fotoEmpresa"];
            Context.SaveChanges();
            var usuario = (from TablaUsuarios in Context.Usuarios
                           where TablaUsuarios.CorreoUsuario == sesionJson.CorreoUsuario
                           select TablaUsuarios).FirstOrDefault();
            HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(usuario));
            return Redirect("/perfilUsuario");
        }
    }
}
