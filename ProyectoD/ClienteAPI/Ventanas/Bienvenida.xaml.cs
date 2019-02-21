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
    /// Lógica de interacción para Bienvenida.xaml
    /// </summary>
    public partial class Bienvenida : Window
    {
        public Bienvenida()
        {
            InitializeComponent();
        }

        private void Button_Cliente_Click(object sender, RoutedEventArgs e)
        {
            //ConductorViaje prueba = new ConductorViaje();
            //this.Visibility = Visibility.Collapsed;
            //this.Visibility = Visibility.Hidden;
            //prueba.Visibility = Visibility.Visible;

            Login logincliente = new Login();
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
            logincliente.Visibility = Visibility.Visible;
        }

        private void Button_Conductor_Click(object sender, RoutedEventArgs e)
        {
            LoginConductor loginConductor = new LoginConductor();
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
            loginConductor.Visibility = Visibility.Visible;
        }

        private void Button_Admin_Click(object sender, RoutedEventArgs e)
        {
            LoginAdmin loginAdmin = new LoginAdmin();
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
            loginAdmin.Visibility = Visibility.Visible;
        }
    }
}