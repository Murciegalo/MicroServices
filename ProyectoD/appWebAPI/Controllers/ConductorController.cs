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
    public class ConductorController : ApiController
    {
        //1
        //De ProyectoD/ModelDB/ModelDB.context.cs obetenemos la clase que hereda de context
        ModelDBContainer db = new ModelDBContainer();           //Api controlador


        //2
        //Previous VERIFICATION >>> Save to DB
        [Route("api/registrarConductor")] 
        [HttpPost]
        public IHttpActionResult RegistrarConductor([FromBody] Conductor datos)
        {
            try
            {
                Conductor conductor = db.Set<Conductor>().Add(datos);
                db.SaveChanges();
                if (conductor == null)
                {
                    throw new Exception("No se puede ingresar datos nulos o mal completados");
                }

                return Ok("true");
            }
            catch (Exception)
            {
                throw new Exception("Datos de registro incorrectos");
            }
        }

        // GET: api/Conductor   >>> for ALL CONDUCTORES
        [Route("api/conductores")]
        public List<Conductor> GetAllConductores()
        {
            return db.Set<Conductor>().ToList();        //return Conductor from DB as List
        }

        // ID of CONDUCTOR       >>  Get me 1 conductor
        [Route("api/conductores/{id}")]
        public Conductor GetConductorId(int id)
        {
            Conductor conductorId = db.Set<Conductor>().FirstOrDefault(p => p.Id == id);

            if (conductorId == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return conductorId;
        }

        //----------------------

        // PUT: api/Conductor/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Conductor/5
        public void Delete(int id)
        {
        }
    }
}
