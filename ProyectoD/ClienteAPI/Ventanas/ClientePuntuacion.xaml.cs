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
using System.Windows.Threading;

namespace ClienteAPI.Ventanas
{
    /// <summary>
    /// Lógica de interacción para ClientePuntuacion.xaml
    /// </summary>
    public partial class ClientePuntuacion : Window
    {
        public string IdClientein { get; set; }
        //TODO ARREGLAR CONSTRUCTOR VENTANA MAPA
       

        public ClientePuntuacion(string IdCliente)
        {
            IdClientein = IdCliente;
            InitializeComponent();
            //viajeCliente.IdCliente = // id del cliente


            // si el viaje es reservado se muestra el resultado



            var puntuacion = new List<int> { 1, 2, 3, 4, 5 };
            ClPuntuacion.ItemsSource = puntuacion;
            ClPuntuacion.SelectedItem = 3;
        }



        private void BtEnviar_Click(object sender, RoutedEventArgs e)
        {
            int idconvertido = Int32.Parse(IdClientein);
            int puntuacion = (int)ClPuntuacion.SelectedItem.GetHashCode();
            //Hay que obtener el idViaje para poder modificar el resultado
            var viaje = new Vj(idconvertido, puntuacion, ClComentario.Text);        
            string viaje_JSON = JsonConvert.SerializeObject(viaje, Formatting.Indented);
            string salida = Servicios.Servicios.PostRequest("http://localhost:50414/api/viaje/modificarcomentario/", viaje_JSON);
            dynamic jsonObj = JsonConvert.DeserializeObject(salida);
            //string comentario = jsonObj[0]["comentarioCliente"] ;
            string p = jsonObj["estadoViaje"];
            string i = jsonObj["IdConductor"];
            string retorno = Servicios.Servicios.GetRequest("http://localhost:50414/api/viaje/Actualiza/Puntuacion/"+i);
            //TODO: PROBAR QUE LA ACTUALIZACION DEL PROMEDIO FUNCIONA.
            if (p != null)
            {
                MessageBox.Show("Mensaje enviado");
                VentanaMapa volverVentana = new VentanaMapa(IdClientein);
                this.Visibility = Visibility.Collapsed;
                this.Visibility = Visibility.Hidden;
                volverVentana.Visibility = Visibility.Visible;
            }

           
        }

        private void BtSalir_Click(object sender, RoutedEventArgs e)
        {
            VentanaMapa volverVentana = new VentanaMapa(IdClientein);
            volverVentana.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
        }
    }

    // Se crea la clase viaje con las mismas propiedades que en el modelo
    public class Vj
    {
        public int IdViaje { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Distancia { get; set; }
        public DateTime Horainicio { get; set; }
        public DateTime Horafinal { get; set; }
        public string Tarifa { get; set; }
        public int Puntuacion { get; set; }
        public int IdCliente { get; set; }
        public int IdConductor { get; set; }
        public string EstadoViaje { get; set; }
        public string ComentarioCliente { get; set; }

        public Vj(int idViaje, string origen, string destino, string distancia, DateTime horainicio, DateTime horafinal, string tarifa, int puntuacion, int idCliente, int idConductor, string estadoViaje, string comentarioCliente)
        {
            IdViaje = idViaje;
            Origen = origen;
            Destino = destino;
            Distancia = distancia;
            Horainicio = horainicio;
            Horafinal = horafinal;
            Tarifa = tarifa;
            Puntuacion = puntuacion;
            IdCliente = idCliente;
            IdConductor = idConductor;
            EstadoViaje = estadoViaje;
            ComentarioCliente = comentarioCliente;
        }
        public Vj(int idviaje, string estadoViaje, string comentarioCliente)
        {
            IdViaje = idviaje;
            EstadoViaje = estadoViaje;
            ComentarioCliente = comentarioCliente;
        }
        public Vj(int idviaje, int puntuacion, string comentarioCliente)
        {
            IdViaje = idviaje;
            Puntuacion = puntuacion;
            ComentarioCliente = comentarioCliente;
        }
    }
}
