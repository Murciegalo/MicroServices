using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProyectoD; // Api controlador
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace appWebAPI.Controllers
{
    public class ClienteController : ApiController
    {
        //De ProyectoD/ModelDB/ModelDB.context.cs obetenemos la clase que hereda de context
        ModelDBContainer db = new ModelDBContainer();

        // Api para registrar el cliente, recibe los datos para poder guardarlo en la base de datos
        [Route("api/registrarCliente")]
        [HttpPost]
        public IHttpActionResult RegistrarCliente([FromBody] Cliente datos)
        {
            try
            {
                Cliente cliente = db.Set<Cliente>().Add(datos);
                db.SaveChanges();

                if (cliente == null)
                {
                    string respuesta ="El cliente es nulo";
                    return Ok(respuesta);
                }

                return Ok("true");
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }
        }
       

    }
}
