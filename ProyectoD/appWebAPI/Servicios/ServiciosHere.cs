using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace appWebAPI.Servicios
{
    public class ServiciosHere
    {
        public static string appid = "tM0GB6VCMDFpsL8GMWK3";  ///Claves Apis de Here para poder invocar su su server WebApi
        public static string appCode = "B0PFIVeHWyMR4aDRR3McnQ";  ///Claves Apis de Here para poder invocar su su server WebApi

        public static int DistanciaHere(string direccionOrigen, string coordenadasD)
        {
            string coordenadasO = Direccion_CordenadasHere(direccionOrigen);
            string urlHere = "https://route.api.here.com/routing/7.2/calculateroute.json?waypoint0=" + coordenadasO + "&waypoint1=" + coordenadasD + "&mode=fastest%3Bcar%3Btraffic%3Aenabled&app_id=" + appid + "&app_code=" + appCode + "&departure=now";
            Uri UriDireccion = new Uri(urlHere);
            WebRequest encuesta = WebRequest.Create(UriDireccion);
            WebResponse respuesta = encuesta.GetResponse();
            StreamReader reader = new StreamReader(respuesta.GetResponseStream());
            string json = reader.ReadToEnd();
            var objetojson = JObject.Parse(json);
            int distancia = (int)objetojson.SelectToken("response.route[0].leg[0].length"); 
            return distancia;
        }

        public static int[] ObtenDistanciaTiempoHere(string direccionOrigen, string direccionDestino)
        {
            string coordenadasO = Direccion_CordenadasHere(direccionOrigen);
            string coordenadasD = Direccion_CordenadasHere(direccionDestino);
            string urlHere = "https://route.api.here.com/routing/7.2/calculateroute.json?waypoint0=" + coordenadasO + "&waypoint1=" + coordenadasD + "&mode=fastest%3Bcar%3Btraffic%3Aenabled&app_id=" + appid + "&app_code=" + appCode + "&departure=now";
            Uri UriDireccion = new Uri(urlHere);
            WebRequest encuesta = WebRequest.Create(UriDireccion);
            WebResponse respuesta = encuesta.GetResponse();
            StreamReader reader = new StreamReader(respuesta.GetResponseStream());
            string json = reader.ReadToEnd();
            var objetojson = JObject.Parse(json);
            int[] salida = new int[2];
            var distancia = (int)objetojson.SelectToken("response.route[0].leg[0].length") / 1000;
            var tiempo = (int)objetojson.SelectToken("response.route[0].leg[0].travelTime") / 60;
            salida[0] = distancia;
            salida[1] = tiempo;
            return salida;
        }

        public static string Direccion_CordenadasHere(string direccion)  ////hace una encuesta a la WebApi de here introduciendole una dirección y responde con un json.
        {
            string urlHere = "https://geocoder.api.here.com/6.2/geocode.json?searchtext=" + direccion + "&app_id=" + appid + "&app_code=" + appCode;
            Uri UriDireccion = new Uri(urlHere);
            Encuesta encuesta = new Encuesta();
            string respuesta = encuesta.EncuestaHttp(urlHere, "GET");
            var respuestajson = JObject.Parse(respuesta);
            var Latitud = (string)respuestajson.SelectToken("Response.View[0].Result[0].Location.DisplayPosition.Latitude");
            var Longitud = (string)respuestajson.SelectToken("Response.View[0].Result[0].Location.DisplayPosition.Longitude");
            string coordenadasrespuesta = Latitud + "," + Longitud;
            return coordenadasrespuesta;

        }
  
    }


   
    /// <summary>
    /// /Clase para resivir datos de los conductores.
    /// </summary>
    public partial class InfoConductores
    {
        public string NombreConductor { get; set; }
        public string DniConductor { get; set; }
        public string MatriculaCoche { get; set; }
        public string ModeloCoche { get; set; }
        public string ColorCoche { get; set; }
        public string DistintivoAmbiental { get; set; }
        public string PuntuacionConductor { get; set; }
    }

    /// <summary>
    /// Clase para manejar diferentes tipos de POST y GETS
    /// </summary>

    public partial class Encuesta
    {
        public string EncuestaHttp(string url, string tipo_encuesta, object datos = null)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url); ///CREO CONEXIÓN DE ENCUESTA CON URL Y METODO DE EN CUESTA POST
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = tipo_encuesta;
            httpWebRequest.Accept = "application/json; charset=utf-8";
            if (datos != null)
            {
                var cadenaescritura = new StreamWriter(httpWebRequest.GetRequestStream()); /// GUARDO EN BUFFER LO QUE QUIERO ENCUESTAR.
                cadenaescritura.Write(datos); ///escribo datos en buffer
                cadenaescritura.Flush(); ///borro buffer
                cadenaescritura.Close(); ///cierro buffer
            }
            try
            {
                var respuesta = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader cadenalectura = new StreamReader(respuesta.GetResponseStream());/////CAPTURO LA RESPUESTA EN EL STREAM READER
                string salida = cadenalectura.ReadToEnd();
                return salida;
            }
            catch (Exception)
            {
                string salida = "";
                return salida;
            }

        }

        public JObject EncuestaHttp(string url, object datos = null)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url); ///CREO CONEXIÓN DE ENCUESTA CON URL Y METODO DE EN CUESTA POST
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json; charset=utf-8";
            if (datos != null)
            {
                var cadenaescritura = new StreamWriter(httpWebRequest.GetRequestStream()); /// GUARDO EN BUFFER LO QUE QUIERO ENCUESTAR.
                cadenaescritura.Write(datos); ///escribo datos en buffer
                cadenaescritura.Flush(); ///borro buffer
                cadenaescritura.Close(); ///cierro buffer
            }
            try
            {
                var respuesta = (HttpWebResponse)httpWebRequest.GetResponse();
                JObject jObject = JObject.FromObject(respuesta);
                return jObject;
            }
            catch (Exception)
            {
                JObject jObject = null;
                return jObject;
            }

        }

        public double EncuestaHttp(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url); ///CREO CONEXIÓN DE ENCUESTA CON URL Y METODO DE EN CUESTA POST
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "GET";
            httpWebRequest.Accept = "application/json; charset=utf-8";
            try
            {
                var respuesta = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader cadenalectura = new StreamReader(respuesta.GetResponseStream());/////CAPTURO LA RESPUESTA EN EL STREAM READER
                string salida = cadenalectura.ReadToEnd();
                double resultado = Convert.ToDouble(salida) / 1000;
                return resultado;
            }
            catch (Exception)
            {
                double resultado = 0;
                return resultado;
            }

        }

    
    }




}