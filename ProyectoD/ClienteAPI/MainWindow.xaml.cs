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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClienteAPI.Ventanas;


namespace ClienteAPI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bienvenida bienvenida = new Bienvenida();
        public MainWindow()
        {
            InitializeComponent();
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
            GotoBienvenida();
        }

        public void GotoBienvenida()
        {
            bienvenida.Visibility = Visibility.Visible;
        }
    }
}
