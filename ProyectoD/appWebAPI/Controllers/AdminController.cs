using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProyectoD;

namespace appWebAPI.Controllers
{
    public class AdminController : ApiController
    {
        ModelDBContainer db = new ModelDBContainer();

        /////////////////// TODO CONTROLADOR ADMIN CLIENTE ///////////////////////////////

        //CONSULTAS CLIENTE//
        // Muestra una lista de todos los clientes que tenemos en la base de datos
        [Route("api/cliente")]
        [HttpGet]
        public List<Cliente> GetAllClientess()
        {
            return db.Set<Cliente>().ToList();
        }

        // GET: api/cliente/{id} ruta para poner id cliente y mostrar ese id
        [Route("api/cliente/{id}")]
        [HttpGet]
        public IHttpActionResult GetClienteId(int id)
        {
            try
            {
                Cliente clienteId = db.Set<Cliente>().FirstOrDefault(p => p.Id == id);

                return Ok(clienteId);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }
        }

        // api/cliente/dni/{dni} ruta para poner dni cliente y mostrar ese dni
        [Route("api/cliente/dni/{dni}")]
        [HttpGet]
        public IHttpActionResult GetClienteDni(string dni)
        {
            try
            {
                // Muesta el cliente que hemos igualado por dni, y mostramos los datos que necesitamos (Solo los de Cliente)
                var cliente = db.Set<Cliente>().Where(x => x.dni == dni).ToList().Select(cl => new Cliente
                {
                    Id = cl.Id,
                    nombre = cl.nombre,
                    email = cl.email,
                    contrasena = cl.contrasena,
                    cuentabancaria = cl.cuentabancaria,
                    dni = cl.dni,
                    fechanacimiento = cl.fechanacimiento,
                    foto = cl.foto,
                    telefono = cl.telefono
                }).ToList();
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }
        }

        //Modificar un cliente buscando por Id
        [Route("api/cliente/modificar/")]
        [HttpPost]
        public IHttpActionResult GetModClienteId(Cliente cl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos invalidos");
            }
            //Se modifica todos los campos de cliente, los que cambien los guardara y los que no se mantendran
            Cliente modCliente = db.Set<Cliente>().FirstOrDefault(c => c.Id == cl.Id);

            modCliente.Id = cl.Id;
            modCliente.nombre = cl.nombre;
            modCliente.email = cl.email;
            modCliente.contrasena = cl.contrasena;
            modCliente.cuentabancaria = cl.cuentabancaria;
            modCliente.dni = cl.dni;
            modCliente.fechanacimiento = cl.fechanacimiento;
            modCliente.foto = cl.foto;
            modCliente.telefono = cl.telefono;
            try
            {
                db.SaveChanges();
                return Ok(modCliente);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }

        }

        // DELETE: api/clientes/eliminar/ {id} y nos eliminara el cliente que hemos selecioando por id
        [Route("api/cliente/eliminar/{id}")]
        [HttpGet]
        public IHttpActionResult DeleteClienteId(int id)
        {
            Cliente clienteDelete = db.Set<Cliente>().FirstOrDefault(p => p.Id == id);
            db.Set<Cliente>().Remove(clienteDelete);
            db.SaveChanges();
            return Ok(db.Set<Cliente>().ToList());
        }
        // DELETE: api/clientes/eliminar/dni/ {dni} y nos eliminara el cliente que hemos selecioando por dni
        [Route("api/cliente/eliminar/dni/{dni}")]
        [HttpGet]
        public IHttpActionResult DeleteClienteDni(string dni)
        {
            Cliente clienteDelete = db.Set<Cliente>().FirstOrDefault(p => p.dni == dni);
            db.Set<Cliente>().Remove(clienteDelete);
            db.SaveChanges();
            return Ok(db.Set<Cliente>().ToList());
        }







        ///////////////////TODO CONTROLADOR ADMIN CONDUCTOR ///////////////////////////////

        //CONSULTAS CONDUCTOR//
        // Mostrar conductores en una lista
        [Route("api/conductor")]
        [HttpGet]
        public List<Conductor> GetAllConductores()
        {
            return db.Set<Conductor>().ToList();
        }
        // GET: api/conductor/{id} ruta para poner id conductor y mostrar ese id
        [Route("api/conductor/{id}")]
        [HttpGet]
        public IHttpActionResult GetConductorId(int id)
        {
            try
            {
                Conductor conductorId = db.Set<Conductor>().FirstOrDefault(p => p.Id == id);
                return Ok(conductorId);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }
        }

        // api/conductor/dni/{dni} ruta para poner dni conductor y mostrar los datos del conductor (solo el conductor)
        [Route("api/conductor/dni/{dni}")]
        [HttpGet]
        public IHttpActionResult GetConductorDni(string dni)
        {
            try
            {
                var conductorDni = db.Set<Conductor>().Where(x => x.dni == dni).ToList().Select(co => new Conductor
                {
                    Id = co.Id,
                    nombre = co.nombre,
                    email = co.email,
                    contrasena = co.contrasena,
                    cuentabancaria = co.cuentabancaria,
                    dni = co.dni,
                    fechanacimiento = co.fechanacimiento,
                    foto = co.foto,
                    telefono = co.telefono,
                    licencia = co.licencia,
                    puntuacion = co.puntuacion,
                    estado = co.estado
                }).ToList();
                return Ok(conductorDni);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }

        }

        //Se modifica el conductor
        [Route("api/conductor/modificar/")]
        [HttpPost]
        public IHttpActionResult GetModConductorId(Conductor co)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos invalidos");
            }
            //Se modifica todos los campos de conductor, los que cambien los guardara y los que no se mantendran
            Conductor modConductor = db.Set<Conductor>().FirstOrDefault(c => c.Id == co.Id);

            modConductor.Id = co.Id;
            modConductor.nombre = co.nombre;
            modConductor.email = co.email;
            modConductor.contrasena = co.contrasena;
            modConductor.cuentabancaria = co.cuentabancaria;
            modConductor.dni = co.dni;
            modConductor.fechanacimiento = co.fechanacimiento;
            modConductor.foto = co.foto;
            modConductor.telefono = co.telefono;
            modConductor.estado = co.estado;
            modConductor.puntuacion = co.puntuacion;
            modConductor.IdCoche = co.IdCoche;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }
            return Ok(modConductor);
        }

        // DELETE: api/clientes/eliminar/ {id} y nos eliminara el cliente que hemos selecioando por id
        [Route("api/conductor/eliminar/{id}")]
        [HttpGet]
        public IHttpActionResult DeleteConductorId(int id)
        {
            Conductor conductorDelete = db.Set<Conductor>().FirstOrDefault(p => p.Id == id);
            db.Set<Conductor>().Remove(conductorDelete);
            db.SaveChanges();
            return Ok(db.Set<Cliente>().ToList());
        }
        // DELETE: api/conductor/eliminar/ {dni} y nos eliminara el conductor que hemos selecioando por dni
        [Route("api/conductor/eliminar/dni/{dni}")]
        [HttpGet]
        public IHttpActionResult DeleteConductorDni(string dni)
        {
            Conductor conductorDelete = db.Set<Conductor>().FirstOrDefault(p => p.dni == dni);
            db.Set<Conductor>().Remove(conductorDelete);
            db.SaveChanges();
            return Ok(db.Set<Conductor>().ToList());
        }




        /////////////////// TODO CONTROLADOR ADMIN COCHE ///////////////////////////////

        //CONSULTAS COCHE//
        // Mostrar coches en una lista
        [Route("api/coche")]
        [HttpGet]
        public List<Coche> GetAllCoches()
        {
            return db.Set<Coche>().ToList();
        }

        // Api para registrar el coche, recibe los datos para poder guardarlo en la base de datos
        [Route("api/registrarConductor/coche")]
        [HttpPost]
        public IHttpActionResult RegistrarCoche([FromBody] Coche datos)
        {
            try
            {
                Coche coche = db.Set<Coche>().Add(datos);
                db.SaveChanges();

                if (coche == null)
                {
                    string respuesta = "El coche es nulo";
                    return Ok(respuesta);
                }

                return Ok(coche.Id);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }
        }

        // GET: api/coche/{id} ruta para poner id conductor y mostrar ese id
        [Route("api/coche/{id}")]
        [HttpGet]
        public IHttpActionResult GetCocheId(int id)
        {
            try
            {
                Coche cocheId = db.Set<Coche>().FirstOrDefault(p => p.Id == id);
                return Ok(cocheId);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }

        }

        // api/coche/matricula/{matricula} ruta para poner matricula del coche y mostrar el coche
        [Route("api/coche/matricula/{matricula}")]
        [HttpGet]
        public IHttpActionResult GetCocheDni(string matricula)
        {
            try
            {
                var cocheMatri = db.Set<Coche>().Where(x => x.matricula == matricula).ToList().Select(vh => new Coche
                {
                    Id = vh.Id,
                    matricula = vh.matricula,
                    modelo = vh.modelo,
                    color = vh.color,
                    distintivoambiental = vh.distintivoambiental,
                    plaza = vh.plaza,
                    fumar = vh.fumar,
                    mascota = vh.mascota
                }).ToList();
                return Ok(cocheMatri);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }

        }

        //Se modifica el coche
        [Route("api/coche/modificar/")]
        [HttpPost]
        public IHttpActionResult GetModCocheId(Coche co)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos invalidos");
            }
            //Se modifica todos los campos del coche, los que cambien los guardara y los que no se mantendran
            Coche modCoche = db.Set<Coche>().FirstOrDefault(c => c.Id == co.Id);

            modCoche.Id = co.Id;
            modCoche.matricula = co.matricula;
            modCoche.modelo = co.modelo;
            modCoche.color = co.color;
            modCoche.plaza = co.plaza;
            modCoche.distintivoambiental = co.distintivoambiental;
            modCoche.mascota = co.mascota;
            modCoche.fumar = co.fumar;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }
            return Ok(modCoche);
        }

        // DELETE: api/coche/eliminar/ {matricula} y nos eliminara el coche que hemos selecioando por matricula
        [Route("api/coche/eliminar/{matricula}")]
        [HttpGet]
        public IHttpActionResult DeleteCoche(string matricula)
        {
            Coche cocheDelete = db.Set<Coche>().FirstOrDefault(p => p.matricula == matricula);
            db.Set<Coche>().Remove(cocheDelete);
            db.SaveChanges();
            return Ok(db.Set<Coche>().ToList());
        }




        /////////////////// TODO CONTROLADOR ADMIN VIAJE ///////////////////////////////

        [Route("api/viaje")]
        [HttpGet]
        public List<Viaje> GetAllViajes()
        {
            return db.Set<Viaje>().ToList();
        }

        // GET: api/conductor/{id} ruta para poner id viaje y mostrar los datos del viaje (Solo del viaje)
        [Route("api/viaje/{id}")]
        [HttpGet]
        public IHttpActionResult GetViajeId(int id)
        {
            try
            {
                var viajeId = db.Set<Viaje>().Where(x => x.IdViaje == id).ToList().Select(vi => new Viaje
                {
                    IdViaje = vi.IdViaje,
                    origen = vi.origen,
                    destino = vi.destino,
                    distancia = vi.distancia,
                    horainicio = vi.horainicio,
                    estadoViaje = vi.estadoViaje,
                    horafinal = vi.horafinal,
                    tarifa = vi.tarifa,
                    puntuacion = vi.puntuacion,
                    comentarioCliente = vi.comentarioCliente
                }).ToList();
                return Ok(viajeId);
            }
            catch (Exception ex)
            {
                string respuesta = ex.Message;
                return Ok(respuesta);
            }

        }

        //Se modifica el viaje
        [Route("api/viaje/modificar/")]
        [HttpPost]
        public IHttpActionResult GetModViajeId(Viaje vj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Datos invalidos");
            }
            //Se modifica todos los campos del viaje, los que cambien los guardara y los que no se mantendran
            Viaje modViaje = db.Set<Viaje>().FirstOrDefault(c => c.IdViaje == vj.IdViaje);

            modViaje.IdViaje = vj.IdViaje;
            modViaje.origen = vj.origen;
            modViaje.destino = vj.destino;
            modViaje.distancia = vj.distancia;
            modViaje.horainicio = vj.horainicio;
            modViaje.horafinal = vj.horafinal;
            modViaje.tarifa = vj.tarifa;
            modViaje.puntuacion = vj.puntuacion;
            modViaje.IdCliente = vj.IdCliente;
            modViaje.IdConductor = vj.IdConductor;
            modViaje.estadoViaje = vj.estadoViaje;
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

        // DELETE: api/viaje/eliminar/ {idViaje} y nos eliminara el viaje que hemos selecioando por idViaje
        [Route("api/viaje/eliminar/{idViaje}")]
        [HttpGet]
        public IHttpActionResult DeleteViaje(int idviaje)
        {
            Viaje viaje = db.Set<Viaje>().FirstOrDefault(p => p.IdViaje == idviaje);
            db.Set<Viaje>().Remove(viaje);
            db.SaveChanges();
            return Ok(db.Set<Viaje>().ToList());
        }






        /////////////////////////  TODO ADMIN FLOTA PRUEBAS ///////////////////////////

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


        [Route("api/viaje/pruebaviaje/{id}")]
        [HttpGet]
        public IHttpActionResult GetViajePruebaId(int id)
        {
            //    try
            //    {  // Se intenta coger los resultados que este Reservado por si tiene mas de 1 viaje
            //        Viaje viajeEstado = db.Set<Viaje>().FirstOrDefault(v=>v.IdViaje == id && v.estadoViaje == "Reservado");
            //var viajeEstado = (from per in db.Set<Viaje>()
            //                   where per.IdViaje == id
            //                   select per);
            //foreach (var a in viajeEstado)
            //{
            //    return Ok(a.estadoViaje + a.horainicio + a.horafinal + a.origen + a.destino + a.distancia + a.tarifa + a.IdViaje + a.IdConductor);
            //}

            //Mostramos los datos del viaje al conductor para ver si acepta el viaje
            //return Ok(viajeEstado.Select(x=>x.estadoViaje) + " " + viajeEstado.Select(x => x.horainicio)+ " " + viajeEstado.Select(x => x.horafinal) + " " + viajeEstado.Select(x => x.origen) + " " + viajeEstado.Select(x => x.destino) + " " + viajeEstado.Select(x => x.distancia) + " " + viajeEstado.Select(x => x.tarifa) + " " + viajeEstado.Select(x => x.IdViaje) + " " + viajeEstado.Select(x => x.IdConductor));

            //return Ok(viajeEstado.IdViaje + " " + viajeEstado.origen + " " + viajeEstado.destino + " " + viajeEstado.distancia + " " + viajeEstado.horainicio + " " + viajeEstado.horafinal== viajeEstado.horainicio + " " + viajeEstado.tarifa + " " + viajeEstado.IdCliente + " " + viajeEstado.IdConductor + " " + viajeEstado.estadoViaje);
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
            var idviaje = db.Set<Viaje>().FirstOrDefault(idv => idv.estadoViaje == "solicitado" && idv.IdConductor == id);


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
    }
}