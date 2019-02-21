using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClienteAPI.Ventanas
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class RegistroCoche : Window
    {
       

        public RegistroCoche()
        {
            InitializeComponent();
           
        }

        private void Button_Registrar_Click(object sender, RoutedEventArgs e)
        {
            // Se crea el objeto con todos los textBox de la ventana
            var prueba = new Coche(this.TextMatricula.Text, this.TextModelo.Text, this.TextColor.Text, this.TextPLazas.Text, this.TextDistintivo.Text, this.TextMascotas.Text, this.TextFumar.Text);
            // Se serializa el objeto para poder enviarlo en formato Json
            string coche_JSON = JsonConvert.SerializeObject(prueba, Formatting.Indented);
            //Se manda al servicio PostRequest que tenemos en servicios  con la ruta y el objeto serializado
            string salida = Servicios.Servicios.PostRequest("http://localhost:50414/api/registrarConductor/coche", coche_JSON);
            ////request y response para que devuelva el ID
           


            RegistroConductor coche = new RegistroConductor(salida);
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
            coche.Visibility = Visibility.Visible;
        }

        private void Button_Volver_Click(object sender, RoutedEventArgs e)
        {
            LoginConductor login =new LoginConductor();
            login.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
        }
    }

    //Se crea la clase coche para tener los mismos datos que en la base de datos para poder registrarlos
    public class Coche
    {

        public string Matricula;
        public string Modelo;
        public string Color;
        public string Plaza;
        public string DistintivoAmbiental;
        public string Mascota;
        public string Fumar;

        public Coche(string matricula, string modelo, string color, string plaza, string distintivoAmbiental, string mascota, string fumar)
        {

            Matricula = matricula;
            Modelo = modelo;
            Color = color;
            Plaza = plaza;
            DistintivoAmbiental = distintivoAmbiental;
            Mascota = mascota;
            Fumar = fumar;
        }
    }
}
