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
using Newtonsoft;
using Newtonsoft.Json.Linq;


namespace ClienteAPI.Servicios
{
    class ServiciosViaje
    {
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

        //    //POST CON AUTHORIZATION

        //    public static string PostRequestJWTlist(string url, List<string> salida)
        //    {
        //        string jsonString = JsonConvert.SerializeObject(salida, Newtonsoft.Json.Formatting.Indented);
        //        var jsonContent = new StringContent(jsonString, UnicodeEncoding.UTF8, "application/json");
        //        //HttpContent salida2 = new HttpContent. (salida1);
        //        HttpClient cliente = new HttpClient();
        //        cliente.BaseAddress = new Uri("http://localhost:50414/");
        //        cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //    ("application/json; charset=utf-8";
        //        //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        //        cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + TokenClass.Token);
        //        //HttpResponseMessage respuesta = cliente.GetAsync("api/prueba/au").Result;

        //        //var respuesta = cliente.PostAsync(url,salida2);
        //        var respuesta = cliente.PostAsync(url, jsonContent);

        //        return respuesta.ToString();
        //    }

        //    JArray jArray = JArray.FromObject;

        //    string miemail = (string)jObject.SelectToken("Mail");

        //    Cliente clientemail = (from c in db.Set<Cliente>()
        //                           where c.email == miemail
        //                           select c).FirstOrDefault();
    }
}