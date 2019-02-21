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
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ClienteAPI.Servicios;

namespace ClienteAPI.Ventanas
{
    /// <summary>
    /// Lógica de interacción para RegistroConductor.xaml
    /// </summary>
    public partial class RegistroConductor : Window
    {
        public string Idcoche { get; set; }

        public RegistroConductor(string idlogin)
        {
            InitializeComponent();
            Idcoche = idlogin;
        }

        private void Button_Registrar(object sender, RoutedEventArgs e)
        {
            DateTime Fechanacimiento = Convert.ToDateTime(TextFechaNacimiento.Text);
            // if con todas las condicones para guardar el objeto
            
            if (Servicios.Servicios.validador(this.TextNombre.Text, this.TextDNI.Text, this.TextContrasena.Text, this.TextFechaNacimiento.Text, this.TextTelefono.Text, this.TextEmail.Text) && this.TextContrasena.Text == this.TextRepetirContrasena.Text)
            {
                int id = Int32.Parse(Idcoche);
                var prueba = new Conductor(id,this.TextNombre.Text, this.TextDNI.Text, Fechanacimiento, this.TextContrasena.Text, this.TextTelefono.Text, this.TextEmail.Text, this.Licencia.Text, this.DatosBanca.Text);
                Console.WriteLine(prueba);

                string cliente_JSON = JsonConvert.SerializeObject(prueba, Formatting.Indented);

                string salida = Servicios.Servicios.PostRequest("http://localhost:50414/api/registrarConductor", cliente_JSON);
            }
            else
            {
                Console.WriteLine("error");
            }
            LoginConductor login = new LoginConductor();
            login.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
        }

        //CONTROLADOR PARA PASAR A LA PANTALLA AUTOM'OVIL DE REGISTRO DE CONDUCTOR, EVENTO DE AQUI PARA SER ELIMINADO
        private void Button_Volver(object sender, RoutedEventArgs e)
        {
            //string salida = Servicios.Servicios.GetRequest("http://localhost:50414/api/conductores");
            RegistroCoche regCoche = new RegistroCoche();
            regCoche.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;

        }
        
    }

    //Se crea objeto identico a cliente para enviarlo
    public class Conductor
    {
        public int IdCoche;
        public string Nombre;
        public string DNI;
        public DateTime Fechanacimiento;
        public string Contrasena;
        public string Telefono;
        public string Email;
        public string Licencia;
        public string Banca;


        public Conductor(int idcoche,string nombre, string dni, DateTime fechanacimiento, string contrasena, string telefono, string email, string licencia, string banca)
        {
            IdCoche = idcoche;
            Nombre = nombre;
            DNI = dni;
            Fechanacimiento = fechanacimiento;
            Contrasena = contrasena;
            Telefono = telefono;
            Email = email;
            Licencia = licencia;
            Banca = banca;
        }

    }
}

