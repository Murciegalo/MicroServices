using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProyectoD; // Api controlador
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace appWebAPI.Controllers
{
    public class LoginController : ApiController
    {
        //contrasenadb es para guardar la contraseña de la DB del usuario y poder comparar con la del TextBox
        string contrasenadb = "";
        //respuesta es para almacenar los comentarios que luego aparecerán según cada casa en el try. Aparece "true" por defecto para que en el login.xaml.cs se pueda coger ese caso como correcto y pasar a la ventana de Viaje.
        string respuesta = "true";

        ModelDBContainer db = new ModelDBContainer();

        // GET: api/Login
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // LOGIN PARA EL CLIENTE
        [HttpPost]
        [Route("api/clientes/login/entrar")]
        public IHttpActionResult GetClientemail([FromBody] Object loginJson)
        {

            //Con LinQ
            if (loginJson == null)
            {
                return BadRequest("Me la estás liando");
            }

            JObject jObject = JObject.FromObject(loginJson);

            string miemail = (string)jObject.SelectToken("Mail");

            Cliente clientemail = (from c in db.Set<Cliente>()
                                   where c.email == miemail
                                   select c).FirstOrDefault();

            //Lo mismo de antes pero hecho con lambda: Cliente clientemail = db.Set<Cliente>().FirstOrDefault(p => p.email == miemail);

            string token = "";
            try
            {

                if (clientemail == null)
                {
                    respuesta = "No quieras correr sin saber andar: regístrate o escribe bien el mail.";
                    return Ok(respuesta);
                }
                else
                {
                    contrasenadb = clientemail.contrasena.ToString();
                    if (contrasenadb == jObject.SelectToken("Contrasena").ToString())
                    {
                        token = TokenGenerator.GenerateTokenJwt(miemail);
                        return Ok(token);
                    }
                    else
                    {
                        respuesta = "Fiera, pon bien la contraseña";
                        return Ok(respuesta);
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest("Me la estás liando, poniendo algo mal en el Mail/Contraseña");
            }
        }

        // LOGIN PARA EL CONDUCTOR
        [HttpPost]
        [Route("api/conductor/login/entrar")]
        public IHttpActionResult GetConductormail([FromBody] Object loginJson)
        {

            //Con LinQ
            if (loginJson == null)
            {
                return BadRequest("Me la estás liando");
            }

            JObject jObject = JObject.FromObject(loginJson);

            string miemail = (string)jObject.SelectToken("Mail");

            Conductor conductoremail = (from c in db.Set<Conductor>()
                                        where c.email == miemail
                                        select c).FirstOrDefault();

            //Lo mismo de antes pero hecho con lambda: Cliente clientemail = db.Set<Cliente>().FirstOrDefault(p => p.email == miemail);

            string token = "";
            try
            {

                if (conductoremail == null)
                {
                    respuesta = "No quieras correr sin saber andar: regístrate o escribe bien el mail.";
                    return Ok(respuesta);
                }
                else
                {
                    contrasenadb = conductoremail.contrasena.ToString();
                    if (contrasenadb == jObject.SelectToken("Contrasena").ToString())
                    {
                        token = TokenGenerator.GenerateTokenJwt(miemail);
                        return Ok(token);
                    }
                    else
                    {
                        respuesta = "Fiera, pon bien la contraseña";
                        return Ok(respuesta);
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest("Me la estás liando, poniendo algo mal en el Mail/Contraseña");
            }
        }

        // LOGIN PARA EL ADMIN
        [HttpPost]
        [Route("api/admin/login/entrar")]
        public IHttpActionResult GetAdminrmail([FromBody] Object loginJson)
        {

            //Con LinQ
            if (loginJson == null)
            {
                return BadRequest("Me la estás liando");
            }

            JObject jObject = JObject.FromObject(loginJson);

            string miemail = (string)jObject.SelectToken("Mail");

            Admin adminemail = (from c in db.Set<Admin>()
                                where c.email == miemail
                                select c).FirstOrDefault();

            //Lo mismo de antes pero hecho con lambda: Cliente clientemail = db.Set<Cliente>().FirstOrDefault(p => p.email == miemail);

            string token = "";
            try
            {

                if (adminemail == null)
                {
                    respuesta = "No quieras correr sin saber andar: regístrate o escribe bien el mail.";
                    return Ok(respuesta);
                }
                else
                {
                    contrasenadb = adminemail.contrasena.ToString();
                    if (contrasenadb == jObject.SelectToken("Contrasena").ToString())
                    {
                        token = TokenGenerator.GenerateTokenJwt(miemail);
                        return Ok(token);
                    }
                    else
                    {
                        respuesta = "Fiera, pon bien la contraseña";
                        return Ok(respuesta);
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest("Me la estás liando, poniendo algo mal en el Mail/Contraseña");
            }
        }

        //DEVUELVE EL ID DEL CLIENTE
        //[Authorize]
        [HttpPost]
        [Route("api/clientes/login/id")]
        public IHttpActionResult GetClienteid([FromBody] Object loginJson)

        {

            //Con LinQ
            if (loginJson == null)
            {
                return BadRequest("Me la estás liando");
            }

            JObject jObject = JObject.FromObject(loginJson);

            string miemail = (string)jObject.SelectToken("Mail");

            Cliente clientemail = (from c in db.Set<Cliente>()
                                   where c.email == miemail
                                   select c).FirstOrDefault();


            return Ok(clientemail.Id);
        }

        //DEVUELVE EL ID DEL CONDUCTOR
        //[Authorize]
        [HttpPost]
        [Route("api/conductor/login/id")]
        public IHttpActionResult GetConductorid([FromBody] Object loginJson)

        {

            //Con LinQ
            if (loginJson == null)
            {
                return BadRequest("Me la estás liando");
            }

            JObject jObject = JObject.FromObject(loginJson);

            string miemail = (string)jObject.SelectToken("Mail");

            Conductor clientemail = (from c in db.Set<Conductor>()
                                     where c.email == miemail
                                     select c).FirstOrDefault();


            return Ok(clientemail.Id);
        }


        public IHttpActionResult PostPruebaAu()
        {
            return Ok("Se ha autorizado bien");

        }
        // POST: api/Login
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Login/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Login/5
        public void Delete(int id)
        {
        }
    }
}