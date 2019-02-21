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
    /// Lógica de interacción para Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void Btclientes(object sender, RoutedEventArgs e)
        {

            string clientes = Servicios.Servicios.GetRequest("http://localhost:50414/api/cliente");
            Contenido.Text = clientes;
        }

        private void BtConductores(object sender, RoutedEventArgs e)
        {
            string conductores = Servicios.Servicios.GetRequest("http://localhost:50414/api/conductor");
            Contenido.Text = conductores;
        }

        private void BtCoches(object sender, RoutedEventArgs e)
        {
            string coches = Servicios.Servicios.GetRequest("http://localhost:50414/api/coche");
            Contenido.Text = coches;
        }

        private void BtViajes(object sender, RoutedEventArgs e)
        {
            string viajes = Servicios.Servicios.GetRequest("http://localhost:50414/api/viaje");
            Contenido.Text = viajes;
        }

        private void BtMostrarClienteDni(object sender, RoutedEventArgs e)
        {
            string salida = Servicios.Servicios.GetRequest("http://localhost:50414/api/cliente/dni/"+ TextDniCliente.Text);
            Contenido.Text = salida;
        }

        private void BtBorrarClienteDni(object sender, RoutedEventArgs e)
        {

        }

        private void BtMostrarConductorDni(object sender, RoutedEventArgs e)
        {
            string salida = Servicios.Servicios.GetRequest("http://localhost:50414/api/conductor/dni/" + TextDniConductor.Text);
            Contenido.Text = salida;
        }

        private void BtBorrarConductoresDni(object sender, RoutedEventArgs e)
        {

        }

        private void BtMostrarCocheMatricula(object sender, RoutedEventArgs e)
        {
            string salida = Servicios.Servicios.GetRequest("http://localhost:50414/api/coche/matricula/" + TextMatricula.Text);
            Contenido.Text = salida;
        }

        private void BtBorrarConcheMatricula(object sender, RoutedEventArgs e)
        {

        }

        private void BtMostrarViajeId(object sender, RoutedEventArgs e)
        {
            string salida = Servicios.Servicios.GetRequest("http://localhost:50414/api/viaje/" + TextIdViaje.Text);
            Contenido.Text = salida;
        }

        private void BtBorrarVaijeId(object sender, RoutedEventArgs e)
        {

        }
    }
   
}
