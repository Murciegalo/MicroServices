using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ClienteAPI.Servicios
{
    public class Servicios
    {
        // 1.  VALIDAR CLIENTES
        public static bool validador(string nome, string dni, string contra, string fecha, string telef, string mail)
        {
            //Cliente
            Regex regNombre = new Regex("[a-z,A-Z]{3,20}");
            Regex regDni = new Regex("^[A-Z]{0,1}[0-9]{6,9}[A-Z]{1}$");
            Regex regContrasena = new Regex("[a-z,A-Z,0-9]{5,30}");
            Regex regFecha = new Regex("^[0-3]{1}[0-9]{1}[-/.]{1}[0-1]{1}[0-9]{1}[-/.]{1}[0-9]{4}"); // Expresion fecha mal revisar
            Regex regTelefono = new Regex("[0-9]{7,12}"); // Expresion telefono mal revisar
            Regex regEmail = new Regex("^[a-z,A-Z,0-9]{1,20}[@]{1}[a-z,A-Z,0-9]{1,10}[.]{1}[a-z,A-Z,0-9]{1,10}");

            //DateTime.TryParse("fecha", out DateTime laBuena);
            if (regNombre.IsMatch(nome) && regDni.IsMatch(dni) && regContrasena.IsMatch(contra) && regFecha.IsMatch(fecha) && regTelefono.IsMatch(telef) && regEmail.IsMatch(mail))
            {
                return true;
            }
            return false;
        }

        // 2. VALIDAR CONDUCTORES

        public static bool valConductor(string nome, string dni, string contra, string fecha, string telef, string mail, string licencia, string banco)
        {
            //Cliente
            Regex regNombre = new Regex("[a-z,A-Z]{3,20}");
            Regex regDni = new Regex("^[A-Z]{0,1}[0-9]{6,9}[A-Z]{1}$");
            Regex regContrasena = new Regex("[a-z,A-Z,0-9]{5,30}");
            Regex regFecha = new Regex("^[0-3]{1}[0-9]{1}[-/.]{1}[0-1]{1}[0-9]{1}[-/.]{1}[0-9]{4}"); // Expresion fecha mal revisar
            Regex regTelefono = new Regex("[0-9]{7,12}"); // Expresion telefono mal revisar
            Regex regEmail = new Regex("^[a-z,A-Z,0-9]{1,20}[@]{1}[a-z,A-Z,0-9]{1,10}[.]{1}[a-z,A-Z,0-9]{1,10}");
            Regex regLicencia = new Regex("^(0-9){10,12}");
            Regex regBanco = new Regex("^(A-Z){2}(0-9){2}[-](0-9){4}[-](0-9){4}[-](0-9){4}[-](0-9){4}");

            //DateTime.TryParse("fecha", out DateTime laBuena);
            if (regNombre.IsMatch(nome) && regDni.IsMatch(dni) && regContrasena.IsMatch(contra) && regFecha.IsMatch(fecha) && regTelefono.IsMatch(telef) && regEmail.IsMatch(mail) && regLicencia.IsMatch(licencia) && regBanco.IsMatch(banco))
            {
                return true;
            }
            return false;
        }
        // Se crea un metodo GetRequest 
        public static string GetRequest(string url)
        {

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "GET";
            httpWebRequest.Accept = "application/json; charset=utf-8";
            HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string respuesta = reader.ReadToEnd();

            return respuesta;
        }

        // Se crea un PostRequest
        public static string PostRequest(string url, Object datos)
        {
            // Se crea un httpWebRequest para mandar la url recibida para conectar con el controlador
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json; charset=utf-8";
            //Se cre un nuevo
            var writer = new StreamWriter(httpWebRequest.GetRequestStream());
            writer.Write(datos);
            writer.Flush();
            writer.Close();
            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            var resultado = reader.ReadToEnd();
            return resultado;
        }
        //GET CON AUTHORIZATION
        public static string GetRequestJWT(string url)
        {
            HttpClient cliente = (HttpClient)new HttpClient();
            cliente.BaseAddress = new Uri("http://localhost:50414/");
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //    ("application/json; charset=utf-8";
            //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + TokenClass.Token);
            //HttpResponseMessage respuesta = cliente.GetAsync("api/prueba/au").Result;
            string respuesta = "";
            try
            {
                respuesta = cliente.GetAsync(url).Result.ToString();
            }
            catch (Exception e)
            {
                respuesta = e.StackTrace;
            }
            return respuesta.ToString();
        }

        //POST CON AUTHORIZATION
        public static async Task<string> PostRequestJWT(string url, string salida)
        {
            //string jsonString = JsonConvert.SerializeObject(salida, Newtonsoft.Json.Formatting.Indented);
            var jsonContent = new StringContent(salida, UnicodeEncoding.UTF8, "application/json");
            //HttpContent salida2 = new HttpContent. (salida1);
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("http://localhost:50414/");
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //    ("application/json; charset=utf-8";
            //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + TokenClass.Token);
            //HttpResponseMessage respuesta = cliente.GetAsync("api/prueba/au").Result;

            //var respuesta = cliente.PostAsync(url,salida2);
            var respuesta = await cliente.PostAsync(url, jsonContent);

            return respuesta.ToString();
        }
    }

    static class TokenClass
    {
        public static string Token { get; set; }
    }

    public class ConductorPrueba
    {
        public string Id;
        public string Nombre;
        public string DNI;
        public DateTime Fechanacimiento;
        public string Contrasena;
        public string Telefono;
        public string Email;
        public string Licencia;
        public string Banca;
        public int Puntuacion;
        public string Estado;

        public ConductorPrueba(string id,string nombre, string dni, DateTime fechanacimiento, string contrasena, string telefono, string email, string licencia, string banca)
        {
            Id = id;
            Nombre = nombre;
            DNI = dni;
            Fechanacimiento = fechanacimiento;
            Contrasena = contrasena;
            Telefono = telefono;
            Email = email;
            Licencia = licencia;
            Banca = banca;
        }
        public ConductorPrueba()
        {

        }
    }
    public class ViajePrueba
    {
        public int IdViaje;
        public string Origen;
        public string Destino;
        public string Distancia;
        public DateTime HoraInicio;
        public DateTime HoraFinal;
        public string Tarifa;
        public int Puntuacion;
        public int IdCliente;
        public int IdConductor;
        public string EstadoViaje;
        public string ComentarioCliente;

        public ViajePrueba(int idviaje, string origen, string destino, string distancia, DateTime horainicio, DateTime horafinal, string tarifa, int puntuacion, int idCliente, int idConductor, string estadoViaje, string comentarioCliente)
        {
            IdViaje = idviaje;
            Origen = origen;
            Destino = destino;
            Distancia = distancia;
            HoraInicio = horainicio;
            HoraFinal = horafinal;
            Tarifa = tarifa;
            Puntuacion = puntuacion;
            IdCliente = idCliente;
            IdConductor = idConductor;
            EstadoViaje = estadoViaje;
            ComentarioCliente = comentarioCliente;
        }
        public ViajePrueba()
        {

        }
    }

    class EncuesFlota
    {
        public bool ReservaAhora { get; set; }
        public bool Fumadores { get; set; }
        public bool Mascotas { get; set; }
        public int? HoraSalida { get; set; }
        public int? MinutosSalida { get; set; }
        public int? Plazas { get; set; }
        public string Pago { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public EncuesFlota(bool reservarAhora, bool fumadores, bool mascotas, int? horaSalida, int? minutosSalida, int? plazas, string pago, string origen, string destino)
        {
            ReservaAhora = reservarAhora;
            Fumadores = fumadores;
            Mascotas = mascotas;
            HoraSalida = horaSalida;
            MinutosSalida = minutosSalida;
            Plazas = plazas;
            Pago = pago;
            Origen = origen;
            Destino = destino;
        }
    }
}