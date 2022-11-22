using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPIChamba.Models;

namespace WebAPIChamba.Controllers
{
    public class HomeController : Controller
    {
        private readonly sql10579654Context Context;

        public HomeController(ILogger<HomeController> logger, sql10579654Context context)
        {
            Context = context;
        }

        [HttpPost("/api/login/type")]
        public IActionResult LoginTypePost([FromBody] Login login)
        {
            var resultado = (from TablaLogin in Context.Logins
                             where
                             TablaLogin.Correo == login.Correo && TablaLogin.Contraseña == login.Contraseña
                             select TablaLogin).FirstOrDefault();
            if (resultado != null)
            {
                if (resultado.Rol == "U")
                {
                    return Ok("U");

                }
                else
                {
                    return Ok("E");
                }
            }
            return Ok("Ok");
        }


        [HttpPost("/api/login")]
        public IActionResult LoginPost([FromBody]Login login)
        {
            var resultado = (from TablaLogin in Context.Logins
                                where
                                TablaLogin.Correo == login.Correo && TablaLogin.Contraseña == login.Contraseña
                                select TablaLogin).FirstOrDefault();
            if (resultado != null)
            {
                if (resultado.Rol == "U")
                {
                    var usuario = (from TablaUsuarios in Context.Usuarios
                                    where TablaUsuarios.CorreoUsuario == resultado.Correo
                                    select TablaUsuarios).FirstOrDefault();
                    String usuarioJson = JsonConvert.SerializeObject(usuario);
                    return Ok(usuarioJson);

                }
                else
                {
                    var empresa = (from TablaEmpresas in Context.Empresas
                                    where TablaEmpresas.CorreoEmpresa == resultado.Correo
                                    select TablaEmpresas).FirstOrDefault();
                    String empresaJson = JsonConvert.SerializeObject(empresa);
                    return Ok(empresaJson);
                }
            }
            return Ok(login);

        }

    }
}
