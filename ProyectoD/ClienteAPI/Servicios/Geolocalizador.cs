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
using ClienteAPI.Servicios;

/// <summary>
/// //// ESTE SERVICIO AÚN NO SE USA EN EL PROYECTO DEBIDO A QUE AUN REQUIERE UNA SOLUCIÓN MEJOR QUE LA QUE ESTÁ ESCRITA Y DEBIDO A QUE NO ES URGENTE SU IMPLEMENTACIÓN QUEDA PENDIENTE DE MODIFICAR.
/// </summary>

namespace ClienteAPI.Servicios
{
    class Geolocalizador
    {
        public static string appid = "tM0GB6VCMDFpsL8GMWK3";  ///Claves Apis de Here para poder invocar su su server WebApi
        public static string appCode = "B0PFIVeHWyMR4aDRR3McnQ";  ///Claves Apis de Here para poder invocar su su server WebApi

        public static string ObtenInterfasRed()///obtiene la interfas de red(MAC) para poder pasarsela al servicio de geolocalizacion y detecte nuestra ubicacion.
        {
            NetworkInterface[] interfacesRed = NetworkInterface.GetAllNetworkInterfaces();
            string DireccionMac = string.Empty;
            foreach (NetworkInterface adapter in interfacesRed)
            {
                if (DireccionMac == string.Empty) // detecta la direccion mac activa.
                {
                    IPInterfaceProperties propiedades = adapter.GetIPProperties();
                    DireccionMac = adapter.GetPhysicalAddress().ToString();
                }
            }
            string primer_elemento = DireccionMac.Substring(0, 2);
            string segundo_elemento = DireccionMac.Substring(2, 2);
            string tercer_elemento = DireccionMac.Substring(4, 2);
            string cuarto_elemento = DireccionMac.Substring(6, 2);
            string quinto_elemento = DireccionMac.Substring(8, 2);
            string sexto_elemento = DireccionMac.Substring(10, 2);
            string interfazformateada = primer_elemento + "-" + segundo_elemento + "-" + tercer_elemento + "-" + cuarto_elemento + "-" + quinto_elemento + "-" + sexto_elemento;
            return interfazformateada;
        }

        public static string GeolocalizacionHere(string url, object datos) //TODO: Terminar geolocalizacion////Metodo para obtener ubicación de la persona que sua la aplicacion y obtener la direccion inicial.
        {
            string interfazred = ObtenInterfasRed();
            string jsonstring = "{\"wlan\": [{\"mac\":\"" + interfazred + "\"}]}"; ////para mandar en el cuerpo del POST  que le envio a HERE.
            EncuestaFlota encuesta = new EncuestaFlota();
            string salida = encuesta.EncuestaHttp(url, "POST", datos);
            return salida;
        }





    }


}
