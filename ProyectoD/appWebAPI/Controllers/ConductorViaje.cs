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
    public class ConductorViaje : ApiController
    {
        ModelDBContainer db = new ModelDBContainer();

        [Route("api/viaje/pruebaviaje/{id}")]
        [HttpGet]
        public IHttpActionResult GetViajePruebaId(int id)
        {
            try
            { // Si el estado que muestra del viaje esta reservado pues le mostrara al conductor el viaje, sino no

                var viajeId = db.Set<Viaje>().Where(x => x.IdConductor == id).ToList().Select(vi => new Viaje
                {
                    IdViaje = vi.IdViaje,
                    origen = vi.origen,
                    destino = vi.destino,
                    distancia = vi.distancia,
                    horainicio = vi.horainicio,
                    estadoViaje = vi.estadoViaje,
                    horafinal = vi.horafinal,
                    tarifa = vi.tarifa,
                    IdCliente = vi.IdCliente,
                    IdConductor = vi.IdConductor,
                    puntuacion = vi.puntuacion,
                    comentarioCliente = vi.comentarioCliente
                }).ToList();

                //return Ok( viajeEstado.IdViaje + viajeEstado.origen + viajeEstado.destino + viajeEstado.distancia + viajeEstado.horainicio + viajeEstado.horafinal + viajeEstado.tarifa + viajeEstado.puntuacion + viajeEstado.IdCliente + viajeEstado.IdConductor + viajeEstado.estadoViaje + viajeEstado.comentarioCliente);
                return Ok(viajeId);
                //return Ok("Estado: "+viajeEstado.estadoViaje + "  " +"Id Viaje: "+ viajeEstado.IdViaje + "  " + "Origen: " + viajeEstado.origen + " " + "Destino: " + viajeEstado.destino + "  " + "Distancia: " + viajeEstado.distancia + " " + "Hora Inicio: " + viajeEstado.horainicio + "Tarifa: " + viajeEstado.tarifa+" €" + " " + "Id conductor: " + viajeEstado.IdConductor);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok();
            }

        }

        // probando que devuelva una lista con solo el estado reservado
        [Route("api/viaje/pruebalista/")]
        [HttpGet]
        public IHttpActionResult GetAllViajesReservados(int id)
        {
            var lista = (from x in db.ViajeSet
                         where x.estadoViaje == "Solicitado" && x.IdConductor == id
                         select x).ToList();

            return Ok(lista);
        }

        //Devuelve el id del viaje solicitado
        [Route("api/viaje/viajesolicitado/{id}")]
        [HttpGet]
        public IHttpActionResult GetAllViajesSolicitado(int id)
        {
            var idviaje = db.Set<Viaje>().FirstOrDefault(idv => idv.estadoViaje == "Solicitado" && idv.IdConductor == id);


            return Ok(idviaje.IdViaje);
        }
        //Se modifica el viaje metiendo comentario y puntuacion del cliente
        [Route("api/viaje/modificarcomentario/")]
        [HttpPost]
        public IHttpActionResult ComentarioViaje(Viaje vj)
        {

            //Se modifica todos los campos del viaje, los que cambien los guardara y los que no se mantendran
            Viaje modViaje = db.Set<Viaje>().FirstOrDefault(c => c.IdViaje == vj.IdViaje);

            modViaje.puntuacion = vj.puntuacion;
            modViaje.comentarioCliente = vj.comentarioCliente;

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("No existe ese id");
            }
            return Ok(modViaje);
        }

        // GET: api/ConductorViaje
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ConductorViaje/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ConductorViaje
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ConductorViaje/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ConductorViaje/5
        public void Delete(int id)
        {
        }
    }
}
