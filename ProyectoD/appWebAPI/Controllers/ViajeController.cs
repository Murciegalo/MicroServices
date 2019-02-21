using ProyectoD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace appWebAPI.Controllers
{
    public class ViajeController : ApiController
    {
        ModelDBContainer db = new ModelDBContainer();

        //Prueba para el estado del conductor
        [Route("api/viaje/prueba/{id}")]
        [HttpGet]
        public IHttpActionResult GetConductorEstadoId(int id)
        {
            try
            {
                Conductor conductorEstado = db.Set<Conductor>().FirstOrDefault(p => p.Id == id);
                if (conductorEstado.estado == "Ocupado")
                {
                    return Ok(conductorEstado.nombre + "  " + conductorEstado.estado);
                }
                else return Ok(conductorEstado.nombre + "  " + conductorEstado.estado);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }

        }

        //Actualiza Puntuación del conductor una vez terminado los viajes se invoca a la Api para que actualice la puntuación del conductor en específico.
        [Route("api/viaje/Actualiza/Puntuacion/{idCond}")]
        [HttpGet]
        public IHttpActionResult PromediaPuntuacion(string idCond)
        {
            int idC = Int32.Parse(idCond);
            var Promedio = db.Set<Viaje>().Where(r => r.IdConductor == idC).Average(r => r.puntuacion);
            Conductor objetoConductor = (from s in db.Set<Conductor>() where (s.Id == idC) select s).FirstOrDefault();
            objetoConductor.puntuacion = Convert.ToInt32(Promedio);
            db.SaveChanges();
            string retorno = objetoConductor.puntuacion.ToString();
            return Ok(retorno);

        }

        // GET: api/registrarViaje registra los viajes añadiendo los datos que se han pasado y se devuelve una respuesta con los datos del viaje
        [Route("api/registrarViaje")]
        [HttpPost]
        public IHttpActionResult RegistrarCliente(Viaje datos)
        {
            try
            {
                db.Set<Viaje>().Add(datos);
                db.SaveChanges();
                Viaje nuevoViaje = db.Set<Viaje>().FirstOrDefault(p => p.IdViaje == datos.IdViaje); //TODO : HE HECHO CAMBIOS EN ESTE CÓDIGO
                return Ok(nuevoViaje.IdViaje);//TODO : HE HECHO CAMBIOS EN ESTE CÓDIGO
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }
        }

        //TODO: ELIMINAR ESTA PORQUERIA AL FINALIZAR.

        //// GET: api/registrarViaje registra los viajes añadiendo los datos que se han pasado y se devuelve una respuesta con los datos del viaje
        //[Route("api/registrarViaje")]
        //[HttpPost]
        //public IHttpActionResult RegistrarCliente(Viaje datos)
        //{
        //    try
        //    {
        //        db.Set<Viaje>().Add(datos);
        //        db.SaveChanges();
        //        Viaje viajeId = db.Set<Viaje>().FirstOrDefault(p => p.IdViaje == datos.IdViaje);
        //        return Ok(viajeId);
        //    }
        //    catch (Exception ex)
        //    {
        //        string respuesta = ex.Message;
        //        return Ok(respuesta);
        //    }
        //}

        [Route("api/viaje/pruebaviaje/{id}")]
        [HttpGet]
        public IHttpActionResult GetViajePruebaId(int id)
        {
            try
            { // Si el estado que muestra del viaje esta reservado pues le mostrara al conductor el viaje, sino no

                var viajeId = db.Set<Viaje>().Where(x => x.IdConductor == id && x.estadoViaje == "solicitado").ToList().Select(vi => new Viaje
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
                }).FirstOrDefault();

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

        [Route("api/viaje/iniciado/{id}")]
        [HttpGet]
        public IHttpActionResult GetViajeIniciado(int id)
        {
            try
            { // Si el estado que muestra del viaje esta reservado pues le mostrara al conductor el viaje, sino no

                var viajeId = db.Set<Viaje>().Where(x => x.IdViaje == id && x.estadoViaje == "aceptado").ToList().Select(vi => new Viaje
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
                }).FirstOrDefault();

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

        [Route("api/viaje/finalizado/{id}")]
        [HttpGet]
        public IHttpActionResult GetViajeFinalizado(int id)
        {
            try
            { // Si el estado que muestra del viaje esta reservado pues le mostrara al conductor el viaje, sino no

                var viajeId = db.Set<Viaje>().Where(x => x.IdViaje == id && x.estadoViaje == "iniciado").ToList().Select(vi => new Viaje
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
                }).FirstOrDefault();

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


                     
        

        // Muestra una lista de todos los viajes q se han registrado en la base de datos
        [Route("api/viaje/lista")]
        [HttpGet]
        public List<Viaje> Get(Viaje datos)
        {
            List<Viaje> lista = new List<Viaje>();
            lista.Add(datos);
            return lista;
        }

        // Modifica los datos del viaje que hemos cambiado y buscamos el viaje por id
        [Route("api/viajes/modificar/")]
        [HttpPost]
        public IHttpActionResult GetModificarId(Viaje vj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos invalidos");
            }

            Viaje modViaje = db.Set<Viaje>().FirstOrDefault(c => c.IdViaje == vj.IdViaje);
            modViaje.IdViaje = vj.IdViaje;
            modViaje.origen = vj.origen;
            modViaje.destino = vj.destino;
            modViaje.distancia = vj.distancia;
            modViaje.horainicio = vj.horainicio;
            modViaje.horafinal = vj.horafinal;
            modViaje.tarifa = vj.tarifa;
            modViaje.puntuacion = vj.puntuacion;
            modViaje.IdCliente = modViaje.IdCliente;
            modViaje.IdConductor = vj.IdConductor;
            modViaje.estadoViaje = vj.estadoViaje;
            modViaje.comentarioCliente = vj.comentarioCliente;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }
            return Ok(modViaje);
        }

    }
}