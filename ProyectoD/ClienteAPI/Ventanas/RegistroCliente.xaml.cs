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



namespace ClienteAPI.Ventanas
{
    /// <summary>
    /// Lógica de interacción para RegistroCliente.xaml
    /// </summary>
    public partial class RegistroCliente : Window
    {
        
        public RegistroCliente()
        {
            InitializeComponent();
        }

        public void Button_Registrar(object sender, RoutedEventArgs e)
        {
           
            // Se crean los Regex para poder meter expresiones regulares 
            // en sus respectivos label
            Regex regNombre = new Regex("[a-z,A-Z]{3,20}");
            Regex regDni = new Regex("^[A-Z]{0,1}[0-9]{6,9}[A-Z]{1}$");
            Regex regContrasena = new Regex("[a-z,A-Z,0-9]{5,30}");
            Regex regFecha = new Regex("^[0-3]{1}[0-9]{1}[-/.]{1}[0-1]{1}[0-9]{1}[-/.]{1}[0-9]{4}"); // Expresion fecha mal revisar
            Regex regTelefono = new Regex("[0-9]{7,12}"); // Expresion telefono mal revisar
            Regex regEmail = new Regex("^[a-z,A-Z,0-9]{1,20}[@]{1}[a-z,A-Z,0-9]{1,10}[.]{1}[a-z,A-Z,0-9]{1,10}");
            // Se crean los campos vacios porque no son necesarios en cliente
            //Se convierte la fecha a DateTime
            DateTime Fechanacimiento = Convert.ToDateTime(TextFechaNacimiento.Text);
            // if con todas las condicones para guardar el objeto
            if (regNombre.IsMatch(this.TextNombre.Text) && regDni.IsMatch(this.TextDNI.Text) && regContrasena.IsMatch(this.TextContrasena.Text) && regFecha.IsMatch(this.TextFechaNacimiento.Text) && regTelefono.IsMatch(this.TextTelefono.Text) && regEmail.IsMatch(this.TextEmail.Text) && this.TextContrasena.Text == this.TextRepetirContrasena.Text)
            {
                //Se instancia Rc con los textbox y los datos vacios para enviar los datos necesarios
              var prueba = new Rc(this.TextNombre.Text, this.TextDNI.Text, Fechanacimiento, this.TextContrasena.Text, this.TextTelefono.Text, this.TextEmail.Text);
                // Se serializa el objeto para poder enviarlo en formato Json
                string cliente_JSON = JsonConvert.SerializeObject(prueba, Formatting.Indented);
                //Se manda al servicio PostRequest que tenemos en servicios  con la ruta y el objeto serializado
                string salida = Servicios.Servicios.PostRequest("http://localhost:50414/api/registrarCliente", cliente_JSON);

                //Volver al login una vez el registro este bien
                if (salida == "\"true\"") {
                    MessageBox.Show("El registro ha sido un éxito");
                    Login login = new Login();
                    login.Visibility = Visibility.Visible;
                    this.Visibility = Visibility.Collapsed;
                    this.Visibility = Visibility.Hidden;
                }


            }
            else { MessageBox.Show("El registro no se ha podido realizar, porque tiene algún campo mal"); }
        }
       
        //Hay que modificar este boton para que regrese al login
        private void Button_Volver(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
        }

      
    }

    //Se crea objeto identico a cliente para enviarlo
    public class Rc
    {
       
        public string Nombre;
        public string DNI;
        public DateTime Fechanacimiento;
        public string Contrasena;
        public string Telefono;
        public string Email;

        public Rc(string nombre, string dni, DateTime fechanacimiento, string contrasena, string telefono, string email)
        {
            
            Nombre = nombre;
            DNI = dni;
            Fechanacimiento = fechanacimiento;
            Contrasena = contrasena;
            Telefono = telefono;
            Email = email;
        }
        

    }
}
