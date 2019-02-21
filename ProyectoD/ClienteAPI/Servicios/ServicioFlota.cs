using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Threading;


namespace ClienteAPI.Servicios
{
    class ServicioFlota
    {
        public static string appid = "tM0GB6VCMDFpsL8GMWK3";  ///Claves Apis de Here para poder invocar su su server WebApi
        public static string appCode = "B0PFIVeHWyMR4aDRR3McnQ";  ///Claves Apis de Here para poder invocar su su server WebApi

        public static string CancelarViajesPendientes(DispatcherTimer temporizador, string IdviajeActual, string PuertoServerAPI)
        {
            EncuestaFlota creaViaje = new EncuestaFlota();
            temporizador.Stop();
            string idviajeCancelar = IdviajeActual;
            string url = "http://" + PuertoServerAPI + "/api/Flota/EstadoViaje/Cancelar/" + idviajeCancelar;
            string respuestaCancelacion = creaViaje.EncuestaHttp(url, "GET"); /////CANCELO EL VIAJE ACTUAL Y ACTUALIZO LA BASE DE DATOS.
            MessageBox.Show("EL VIAJE YA HA SIDO CANCELADO");
            return respuestaCancelacion;

        }


        public static string PostRequest(string url, object datos) 
        {
            EncuestaFlota encuesta = new EncuestaFlota();
            string salida = encuesta.EncuestaHttp(url,"POST", datos);
            return salida;
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
            var distancia = (int)objetojson.SelectToken("response.route[0].leg[0].length");
            var tiempo = (int)objetojson.SelectToken("response.route[0].leg[0].travelTime");
            salida[0]= distancia;
            salida[1] = tiempo;
            return salida;
        }

        public static Uri ObtenMapaRutaHere(string direccionOrigen, string direccionDestino, int anchoimagen, int altoimagen)
        {  
            string coordenadasO = Direccion_CordenadasHere(direccionOrigen);
            string coordenadasD = Direccion_CordenadasHere(direccionDestino);
            string urlHere = "https://image.maps.api.here.com/mia/1.6/route?r0=" + coordenadasO + "," + coordenadasD + "&&m0=" + coordenadasO + "," + coordenadasD + "&&lc0=00ff00&sc0=000000&lw0=6&&&"+ "&w=" + anchoimagen + "&h=" + altoimagen + "&app_id=" + appid + "&app_code=" + appCode;
            Uri UriDireccion = new Uri(urlHere);
            return UriDireccion;
        }


        public static string Direccion_CordenadasHere(string direccion)  ////hace una encuesta a la WebApi de here introduciendole una dirección y responde con un json.
        {
            string urlHere = "https://geocoder.api.here.com/6.2/geocode.json?searchtext=" + direccion + "&app_id=" + appid + "&app_code=" + appCode;
            Uri UriDireccion = new Uri(urlHere);
            EncuestaFlota encuesta = new EncuestaFlota();
            string respuesta = encuesta.EncuestaHttp(urlHere, "GET");
            var respuestajson = JObject.Parse(respuesta);
            var Latitud = (string)respuestajson.SelectToken("Response.View[0].Result[0].Location.DisplayPosition.Latitude");
            var Longitud = (string)respuestajson.SelectToken("Response.View[0].Result[0].Location.DisplayPosition.Longitude");
            string coordenadasrespuesta = Latitud + "," + Longitud;
            return coordenadasrespuesta;

        }

        public static Uri ObtenMapaHere(string direccion, int anchoimagen, int altoimagen, int zoom)  ///encuesta a la WebApi de Here debe devolver solo la uri de un mapa para insertarla como referencia en el elemento imagen.
        {
            string coordenadas = Direccion_CordenadasHere(direccion);
            string urlHere = "https://image.maps.api.here.com/mia/1.6/mapview?c=" + coordenadas + "&z=" + zoom + "&w=" + anchoimagen + "&h=" + altoimagen + "&f=" + 1 + "&u=100" + "&app_id=" + appid + "&app_code=" + appCode;
            Uri UriDireccion = new Uri(urlHere);
            return UriDireccion;


        }

        public static string EncuestaHereJSON(string direccion)  ////hace una encuesta a la WebApi de here introduciendole una dirección y responde con un json.
        {
            string urlHere = "https://geocoder.api.here.com/6.2/geocode.json?searchtext=" + direccion + "&app_id=" + appid + "&app_code=" + appCode;
            Uri UriDireccion = new Uri(urlHere);
            EncuestaFlota encuesta = new EncuestaFlota();
            string respuesta = encuesta.EncuestaHttp(urlHere, "GET");
            return respuesta;

        }

        public static double EncuestaServidorTarifa(string url)  ////hace una encuesta al servidor para obtener la tarifa actual del cobro del servicio.
        {
            EncuestaFlota encuesta = new EncuestaFlota();
            double respuesta = encuesta.EncuestaHttp(url);
            return respuesta;

        }



    }

    /// <summary>
    /// /Clase para enviar los datos necesarios para crear un viaje nuevo
    /// </summary>

    public partial class Viaje
    {
        public string origen { get; set; }
        public string destino { set; get; }
        public string distancia { get; set; }
        public DateTime horainicio { set; get; }
        public DateTime horafinal { set; get; }
        public string tarifa { get; set; }
        public string IdConductor { get; set; }
        public string IdCliente { get; set; }     
        public string estadoViaje { set; get; }
       
    }

  
    /// <summary>
    /// clase para obtener iformación sobre los conductores de la flota.
    /// </summary>
    public partial class InfoConductores
    {
        public string NombreConductor { get; set;}
        public string DniConductor { get; set; }
        public string MatriculaCoche { get; set; }
        public string ModeloCoche { get; set; }
        public string ColorCoche { get; set; }
        public string DistintivoAmbiental { get; set; }
        public string PuntuacionConductor { get; set; }
    }

    /// <summary>
    /// /CLASE PARA RECOGER Y GESTINAR ENCUESTAS GET Y POST Y OBTENER INFORMACIÓN DE LA FLOTA.
    /// </summary>
    public partial class EncuestaFlota
    {
        public bool ReservaAhora { get; set; }
        public string Fumadores { get; set; }
        public string Mascotas { get; set; }
        public DateTime HoraSalida { get; set; }
        public string Plazas { get; set; }
        public string Pago { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string CoordenadasOrigen { get; set; }
        public string CoordenadasDestino { get; set; }

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
                string salida = "No hay respuesta de la Api";
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
                double resultado = Convert.ToDouble(salida)/1000;
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