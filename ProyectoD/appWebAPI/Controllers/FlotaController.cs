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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using appWebAPI.Servicios;


namespace appWebAPI.Controllers
{
   
    public class FlotaController : ApiController
    {


        ModelDBContainer dbFlota = new ModelDBContainer(); //obtener objeto Modelo de nuestra base de dato

        [Route("api/Tarifa/Distancia")]
        [HttpGet]
        public double GetTarifaDistancia()//TARIFA ACTUAL PARA COBRAR SEGÚN LA DISTANCIA TOTAL DEL VIAJE
        {
            double tarifaDistancia = 125; ///Presio en Centimos
            return tarifaDistancia;
        }

        [Route("api/Tarifa/Tiempo")]
        [HttpGet]
        public double GetTarifaTiempo()//TARIFA ACTUAL PARA COBRAR SEGÚN EL TIEMPO TOTAL DEL VIAJE
        {
            double tarifaTiempo = 5; //presio en centimos
            return tarifaTiempo;
        }

        [Route("api/Flota/EstadoViaje/Cancelar/{idViaje}")]  ///PERMITE SABER AL CLIENTE SI SU VIAJE ESTA INICIADO FINALIZADO ETC.
        [HttpGet]
        public string CancelarViaje(string idViaje)
        {
            try
            {
                int idV = Int32.Parse(idViaje);
                Viaje viajesolicitado = dbFlota.Set<Viaje>().FirstOrDefault(p => p.IdViaje == idV);
                viajesolicitado.estadoViaje = "cancelado";
                dbFlota.SaveChanges();
                string estadoViaje = viajesolicitado.estadoViaje;
                return estadoViaje;
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return respuesta;
            }
        }


        [Route("api/Flota/EstadoViaje/{idViaje}")]  ///PERMITE SABER AL CLIENTE SI SU VIAJE ESTA INICIADO FINALIZADO ETC.
        [HttpGet]
        public string EstadoViaje(string idViaje)
        {
            try
            {
                int idV = Int32.Parse(idViaje);
                Viaje viajesolicitado = dbFlota.Set<Viaje>().FirstOrDefault(p => p.IdViaje == idV);
                string estadoViaje = viajesolicitado.estadoViaje;
                return estadoViaje;
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return respuesta;
            }
        }

        
        // GET: api/registrarViaje registra los viajes añadiendo los datos que se han pasado y se devuelve una respuesta con los datos del viaje
        [Route("api/Flota/registrarViaje")]
        [HttpPost]
        public IHttpActionResult RegistrarCliente(Viaje datos)
        {
            try
            {
                dbFlota.Set<Viaje>().Add(datos);
                dbFlota.SaveChanges();
                Viaje nuevoViaje = dbFlota.Set<Viaje>().FirstOrDefault(p => p.IdViaje == datos.IdViaje); //TODO : HE HECHO CAMBIOS EN ESTE CÓDIGO
                return Ok(nuevoViaje.IdViaje);//TODO : HE HECHO CAMBIOS EN ESTE CÓDIGO
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }
        }

        [Route("api/InfoConductores/{id}")]
        [HttpGet]
        public IHttpActionResult PostInfoConductores(string id)
        {
            bool isConductor = Int32.TryParse(id, out int idcond); 
            
            Conductor objetoConductor = (from s in dbFlota.Set<Conductor>() where (s.Id == idcond) select s).FirstOrDefault();
            InfoConductores infoConductores = new InfoConductores()
            {
                NombreConductor = objetoConductor.nombre.ToString(),
                DniConductor = objetoConductor.dni.ToString(),
                MatriculaCoche = objetoConductor.Coche.matricula.ToString(),
                ModeloCoche = objetoConductor.Coche.modelo.ToString(),
                ColorCoche = objetoConductor.Coche.color.ToString(),
                DistintivoAmbiental = objetoConductor.Coche.distintivoambiental.ToString(),
                PuntuacionConductor = objetoConductor.puntuacion.ToString()
            };
                                  
            return Ok(infoConductores);
        }



        [Route("api/Flota")]
        [HttpPost]
        public IHttpActionResult PostCochesFlota([FromBody] Object encuesta)
        {
            if (encuesta == null)
            {
                return BadRequest("No se puede encontrar un coche con los datos especificados");
            }
            JObject jObject = JObject.FromObject(encuesta);
            string origen = (string)jObject.SelectToken("Origen");
            string destino = (string)jObject.SelectToken("Destino");
            bool reservaAhora = (bool)jObject.SelectToken("ReservaAhora");
            string fumadores = (string)jObject.SelectToken("Fumadores");
            string mascotas = (string)jObject.SelectToken("Mascotas");
            string plazas = (string)jObject.SelectToken("Plazas");
            string coordenadasOrigen = (string)jObject.SelectToken("CoordenadasOrigen");


            List<Flota> flotaEntera = (from s in dbFlota.Set<Flota>() select s).ToList();
            List<Flota> sercanos = new List<Flota>();

            foreach (Flota ft in flotaEntera)
            {
                string coordenadasCoches = ft.longitud + "," + ft.Latitud;
                int distancias = ServiciosHere.DistanciaHere(origen,coordenadasCoches);
                if(1000 > distancias)
                {
                    sercanos.Add(ft);
                    //System.Threading.Thread.Sleep(10000);
                }

               
            }

            List<Conductor> ConductoresAptos = (from c in dbFlota.Set<Conductor>()
                                                where ((c.estado == "Disponible")
                                                      & (c.Coche.fumar == fumadores)
                                                      & (c.Coche.mascota == mascotas)
                                                      & (c.Coche.plaza == plazas))
                                                select c).ToList();

            //Recorro todos los conductores con Coches aptos para el servcio y determino cual está mas serca según el listado de flota con coches sercanos PERO SOLO SU ID.
            List<int> definitivos = new List<int>();
            foreach (Conductor c in ConductoresAptos)
            {
                foreach (Flota f in sercanos)
                {
                    if (c.IdCoche == f.idCoche)
                    {
                        definitivos.Add(c.Id);  ////Agregame los conductores a la lista definitiva que están mas serca según el cálculo de la flota.
                    }
                }

            }

            return Ok(definitivos);

        }

    }
}
