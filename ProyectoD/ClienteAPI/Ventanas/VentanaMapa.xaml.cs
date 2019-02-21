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
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ClienteAPI.Servicios;
using System.Globalization;


namespace ClienteAPI.Ventanas
{
    /// <summary>
    /// Lógica de interacción para VentanaMapa.xaml
    /// </summary>
    public partial class VentanaMapa : Window
    {
        DispatcherTimer temporizador = new DispatcherTimer(); // termporizador para crear rutinas que se comuniquen con la base de datos en busca de cambios en estado de un viaje.
        public string PuertoServerAPI { get; set; }
        public bool ReservarAhora_Value { get; set; }
        public string Fumadores_Value { get; set; }
        public string Mascotas_Value { get; set; }
        public string CoordenadasOrigen { get; set; }
        public string CoordenadasDestino { get; set; }
        public double PrecioViaje { get; set; } // viaje
        public string[] IdConductores { get; set; } //viaje
        public List<InfoConductores> ListaInfoConductores { get; set; }
        public string DistanciaViaje { get; set; } //viaje
        public string DireccionOrigenViaje { set; get; }//viaje
        public string DireccionDestinoViaje { set; get; }//viaje
        public DateTime HoraInicioViaje { set; get; }//viaje
        public string IdCliente { set; get; }
        public string IdviajeActual { get; set; }
        public int TiempoViajeActual { get; set; }
        public string EstadoViajeActual { get; set; }


        public VentanaMapa(string idlogin)
        {   
            InitializeComponent();
            ////Inicializando todos los valores Predeterminados de la vista///
            ///
            //inicializo el puerto de nuestro servidor a trabajar.
            PuertoServerAPI = "localhost:50414";
            ///
            TexBoxOrigen.Text = "Calle Anabel Segura 11, Madrid"; //TODO: QUITAR ESTA LINEA DE CÓDIGO
            TexBoxDestino.Text = "Calle Barbastro, 6, 28108 Alcobendas, Madrid"; //TODO: Quitar esta Linea.

            IdCliente = idlogin;
            TexBoxOrigen.Focus();
            CheckBoxReservarAhora.IsChecked = true;
            ReservarAhora_Value = true;
            CheckBoxMascota.IsChecked = false;
            Mascotas_Value = "No";
            CheckBoxFumadores.IsChecked = false;
            Fumadores_Value = "No";
            ComboBoxHora.IsEnabled = false;
            ComboBoxMinutos.IsEnabled = false;

            var plazas = new List<int> { 4, 6, 8, 10 };
            ComboBoxPlazas.ItemsSource = plazas;
            ComboBoxPlazas.SelectedItem = 4;

            var horas = new List<int> { };
            for (int h = 1; h < 24; h++) { horas.Add(h); }
            ComboBoxHora.ItemsSource = horas;

            var minutoshora = new List<int> { };
            for (int m = 0; m < 60; m++){minutoshora.Add(m);}
            ComboBoxMinutos.ItemsSource = minutoshora;

            var pago = new List<string> { "Efectivo", "Tarjeta" };
            ComboBoxPago.ItemsSource = pago;
            ComboBoxPago.SelectedItem = "Efectivo";
            ////fin de inicialización componentes de la ventana.

           
        }

        private void ListBoxOrigen_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ListBoxDestino_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBoxHora_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxMinutos.Focus();

        }

        private void ComboBoxMinutos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxPago.Focus();

        }

        private void ComboBoxPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBoxMascota_Checked(object sender, RoutedEventArgs e)
        {   
        }

        private void CheckBoxMascota_UnChecked(object sender, RoutedEventArgs e)
        {   
        }

        private void CheckBoxFumadores_Checked(object sender, RoutedEventArgs e)
        {  
        }

        private void CheckBoxFumadores_UnChecked(object sender, RoutedEventArgs e)
        {   
        }

        private void ComboBoxPlazas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void ButtonBuscar_Click(object sender, RoutedEventArgs e) ///////////coge todos datos que se encuentran en los elementos de la ventana y se lo envía al servidor Api mediante un metodo POST que se encuentra en la carpeta servicios clase flota.
        {
            TexBoxEstadoViaje.Text = "BUSCANDO CONDUCTORES CERCA .....";

            EncuestaFlota encuesta = new EncuestaFlota(); //objeto que contendrá los datos a encuestar.
                                                          ///obtengo datos de la ventana///
            try
            {
                string direccionOrigen = TexBoxOrigen.Text; // se guarda en variable para luego enviarlo a here mediante esta variable
                encuesta.Origen = direccionOrigen;
                DireccionOrigenViaje = direccionOrigen;
                string direccionDestino = TexBoxDestino.Text;
                encuesta.Destino = direccionDestino;
                DireccionDestinoViaje = direccionDestino;
                encuesta.ReservaAhora = ReservarAhora_Value;  ///se envía tipo bool 
                encuesta.Fumadores = Fumadores_Value.ToString(); //se envía tipo string
                encuesta.Mascotas = Mascotas_Value.ToString(); //se envia tipo string
                encuesta.Plazas = ComboBoxPlazas.SelectedItem.GetHashCode().ToString(); //
                encuesta.Pago = ComboBoxPago.SelectedItem.ToString();
                ///obtener las coordenadas del origen para enviarlas al servidor y poder encontrar chofer serca.
                CoordenadasOrigen = ServicioFlota.Direccion_CordenadasHere(direccionOrigen);
                CoordenadasDestino = ServicioFlota.Direccion_CordenadasHere(direccionDestino);
                if (CoordenadasOrigen == ",")
                {
                    MessageBox.Show("Escriba una dirección de ORIGEN válida");
                    TexBoxOrigen.Text = "";
                    TexBoxOrigen.Focus();
                }

                if (CoordenadasDestino == ",")
                {
                    MessageBox.Show("Escriba una dirección de DESTINO válida");
                    TexBoxDestino.Text = "";
                    TexBoxDestino.Focus();
                }

                if (ReservarAhora_Value == false)
                {
                    int hora = ComboBoxHora.SelectedItem.GetHashCode();
                    int minutos = ComboBoxMinutos.SelectedItem.GetHashCode();
                    DateTime horactual = DateTime.Now;
                    DateTime horasMinutos = new DateTime(horactual.Year, horactual.Month, horactual.Day,hora,minutos,00);
                    HoraInicioViaje = horasMinutos;
                    encuesta.HoraSalida = horasMinutos;
                    
                }
                else
                {
                    DateTime horactual = DateTime.Now;
                    DateTime horasMinutos = new DateTime(horactual.Year, horactual.Month, horactual.Day, horactual.Hour,horactual.Minute,horactual.Second);
                    encuesta.HoraSalida = horasMinutos;
                    HoraInicioViaje = horasMinutos;
                    TexBoxOrigen.Focus();
                }
                encuesta.CoordenadasOrigen = CoordenadasOrigen;
                encuesta.CoordenadasDestino = CoordenadasDestino;
               
            }
            catch (Exception)
            {
                MessageBox.Show("Introduzca todos los datos correctamente por favor");
                TexBoxOrigen.Focus();
            }
            ///introducido todo los valores de la ventana lo serializo y lo envío mediante la clase Flota definida en la carpeta servicios.
            string datos = JsonConvert.SerializeObject(encuesta, Formatting.Indented);
            string respuesta = ServicioFlota.PostRequest("http://"+PuertoServerAPI+"/api/Flota",datos);
            
            char[] caracterdelete = { '[', ']'}; ///para quitar los corchetes a la respuesta
            string cochesRespuesta = respuesta.Trim(caracterdelete);
           
            IdConductores = cochesRespuesta.Split(','); //separar la respuesta por las comas.
            InfoConductores agregaconductor = new InfoConductores(); //instancia para salvar los datos de los conductores
            List<InfoConductores> listainfo = new List<InfoConductores>();
            try
            {
                listBoxConductores.Items.Clear();
                for (int id = 0; id < IdConductores.Length; id++)  ///obtengo todos los conductores sercas y aptos para el servicio
                {
                    string url = "http://"+PuertoServerAPI+"/api/InfoConductores/" + IdConductores[id];
                    string infoconductor = encuesta.EncuestaHttp(url, "GET");
                    var respuestajson = JObject.Parse(infoconductor);
                    agregaconductor.NombreConductor = (string)respuestajson.SelectToken("NombreConductor");
                    agregaconductor.DniConductor = (string)respuestajson.SelectToken("DniConductor");
                    agregaconductor.MatriculaCoche = (string)respuestajson.SelectToken("MatriculaCoche");
                    agregaconductor.ModeloCoche = (string)respuestajson.SelectToken("ModeloCoche");
                    agregaconductor.ColorCoche = (string)respuestajson.SelectToken("ColorCoche");
                    agregaconductor.DistintivoAmbiental = (string)respuestajson.SelectToken("DistintivoAmbiental");
                    agregaconductor.PuntuacionConductor = (string)respuestajson.SelectToken("PuntuacionConductor");
                    listainfo.Add(agregaconductor);
                    listBoxConductores.Items.Add("\n  Nombre Chofer: " + agregaconductor.NombreConductor
                                               + "\n  DNI: " + agregaconductor.DniConductor
                                               + "\n  Matricula: " + agregaconductor.MatriculaCoche
                                               + "\n  Modelo Coche: " + agregaconductor.ModeloCoche
                                               + "\n  Color Coche: " + agregaconductor.ColorCoche
                                               + "\n  Distintivo Ambiental: " + agregaconductor.DistintivoAmbiental
                                               + "\n  Puntuación: " + agregaconductor.PuntuacionConductor);
                    listBoxConductores.SelectedIndex = id;

                    
                }

                ListaInfoConductores = listainfo;
               

            }
            catch(Exception)
            {
                MessageBox.Show("NO HAY CONDUCTORES CON ESAS CARACTERÍSTICAS Y UBICACIONES");
            }
           
        }

        private void Inizializar_MapaImagen(object sender, EventArgs e)
        {

        }

        private void CheckBoxReservarAhora_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxReservarAhora.IsChecked == false)
            {
                ComboBoxHora.IsEnabled = true;   ////habilitando la edición de hora ya la reserva no es ahora.
                ComboBoxMinutos.IsEnabled = true; /////habilitando la edición de minutos.
                ReservarAhora_Value = false;  /////poner la variable a Reservar Ahora a descheckeado.
            }
            else
            {
                ComboBoxHora.IsEnabled = false;
                ComboBoxMinutos.IsEnabled = false;
                ReservarAhora_Value = true;

            }

        }

        private void CheckBoxReservarAhora_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxReservarAhora.IsChecked == true)
            {
                ComboBoxHora.IsEnabled = false;  ////Deshabilitando la edición de la hora y de los minutos ya que la reserva es ahora.
                ComboBoxMinutos.IsEnabled = false;
                ReservarAhora_Value = true;/////poner la variable a Reservar Ahora a chequeado.
            }
            else
            {
                ComboBoxHora.IsEnabled = true;
                ComboBoxMinutos.IsEnabled = true;
                ReservarAhora_Value = false;
                ComboBoxHora.Focus();

            }
        }

        /// <summary>
        /// CÓDIGO PARA SOLICITAR UN CONDUCTOR EN ESPECÍFICO AL SERVIDOR Y CREAR LA CLASE VIAJE.
        /// </summary>
        
        private void BotonTaxi_Click(object sender, RoutedEventArgs e)
        {
            TexBoxEstadoViaje.Text = "SOLICITANDO VIAJE...";
            int conductorseleccionado = listBoxConductores.SelectedIndex; //veo que conductor fue seleccionado
            string[] ides = IdConductores;
            string idchofer = ides[conductorseleccionado];
            string tarifaviaje = PrecioViaje.ToString();
           
            EncuestaFlota creaViaje = new EncuestaFlota();
            Viaje datoViaje = new Viaje()
            {
                origen = DireccionOrigenViaje,
                destino = DireccionDestinoViaje,
                distancia = DistanciaViaje,
                horainicio = HoraInicioViaje,
                horafinal = HoraInicioViaje,
                tarifa = tarifaviaje,
                IdConductor = idchofer,
                IdCliente = IdCliente,
                estadoViaje = "solicitado",
               
            };
     
            string datos = JsonConvert.SerializeObject(datoViaje, Formatting.Indented);
            string respuestaViaje = ServicioFlota.PostRequest("http://"+ PuertoServerAPI+ "/api/Flota/registrarViaje", datos);
            IdviajeActual = respuestaViaje; ///salvo el id de viaje.
            int contadorintentos = 0;
            bool continuar = true;
           
            temporizador.Interval = new TimeSpan(0,0,2);
            temporizador.Tick += (s, a) =>    // Temporizador que me detecta cuando un viaje es iniciado o rechazado.
            {
                contadorintentos++;
                string url = "http://" + PuertoServerAPI + "/api/Flota/EstadoViaje/" + respuestaViaje;
                char[] caracterdelete = { '"' }; ///para quitar los corchetes a la respuesta
                
                
                string estado = creaViaje.EncuestaHttp(url, "GET"); /////envio un get para obtener información del viaje actual.
                
                string estados = estado.Trim(caracterdelete);

                EstadoViajeActual = estados;
                
                switch (estados)
                {
                    case "cancelado":
                              TexBoxEstadoViaje.Text = "VIAJE CANSELADO";
                              MessageBox.Show("EL VIAJE YA HA SIDO CANCELADO");
                              continuar = false;
                              temporizador.Stop();
                        break;

                    case "aceptado":
                              TexBoxEstadoViaje.Text = "EL CONDUCTOR ESTÁ EN CAMINO";
                              continuar = true;
                        break;

                    case "rechazado":
                              TexBoxEstadoViaje.Text = "SOLICITUD RECHAZADA";
                              MessageBox.Show("SOLICITUD RECHAZADA");
                              continuar = false;
                              temporizador.Stop();
                        break;
                    case "iniciado":
                              TexBoxEstadoViaje.Text = "EL VIAJE SE HA INICIADO";
                              MessageBox.Show("EL VIAJE SE HA INICIADO");
                              //temporizador.Interval = new TimeSpan(0, 0 , TiempoViajeActual);  ///Si el viaje ha iniciado deja el tiempo de proxima encuesta al tiempor estimado del viaje para volver a comprobar el estado del viaje y no sobrecargar al servidor.
                              continuar = true;
                              temporizador.Stop();
                                                          
                        break;

                    case "finalizado":
                             TexBoxEstadoViaje.Text = "EL VIAJE HA FINALIZADO";
                             MessageBox.Show("EL VIAJE HA FINALIZADO");
                             continuar = false;
                             ClientePuntuacion clientePuntuacion = new ClientePuntuacion(respuestaViaje);
                             clientePuntuacion.Visibility = Visibility.Visible;
                             Visibility = Visibility.Collapsed;
                             temporizador.Stop();

                        break;

                    default:

                        if (contadorintentos >= 10)
                        {   //TODO: DISMINUIR TIEMPO DEL TIMER  //TODO: HAY QUE BORRAR DE LA BASE DE DATOS
                            TexBoxEstadoViaje.Text = "SU SOLICITUD NO HA SIDO CONTESTADA";  //TODO: HAY QUE BORRAR DE LA BASE DE DATOS PARA QUE NO DE CONFLICTO EN FUTUROS PEDIDOS
                            MessageBox.Show("SU SOLICITUD NO HA SIDO CONTESTADA");
                            if ((IdviajeActual != null) & (EstadoViajeActual != "iniciado"))
                            {
                                TexBoxEstadoViaje.Text = "cancelando el Viaje ....";
                                string respuestacancelacion = ServicioFlota.CancelarViajesPendientes(temporizador, IdviajeActual, PuertoServerAPI);
                                TexBoxEstadoViaje.Text = respuestacancelacion;
                                TexBoxEstadoViaje.Text = "VIAJE CANCELADO";
                                IdviajeActual = null;
                            }
                            continuar = false;
                            temporizador.Stop();
                        }

                        break;
                }
                               
                             
                // DatosTarifa.Text = estado; //TODO QUITAR AL TERMINAR.
            };
            contadorintentos = 0;
            if (continuar == true)
            {
                temporizador.Start();
            }
            else
            {
                temporizador.Stop();
            } 
            
            

        }

        private void TexBoxOrigen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    string direccionOrigen = TexBoxOrigen.Text;
                    DireccionOrigenViaje = direccionOrigen;
                    CoordenadasOrigen = ServicioFlota.Direccion_CordenadasHere(direccionOrigen);
                    DatosTarifa.Text = CoordenadasOrigen; //TODO: Quitar al terminar
                    if (CoordenadasOrigen == ",")
                    {
                        MessageBox.Show("Escriba una dirección de ORIGEN válida");
                        TexBoxOrigen.Text = "";
                        TexBoxOrigen.Focus();
                    }
                    ImagenMapa.Source = (new BitmapImage(ServicioFlota.ObtenMapaHere(TexBoxOrigen.Text, (int)ImagenMapa.Width, (int)ImagenMapa.Height,16)));
                   //TODO: Quitar MessageBox.Show(Geolocalizador.ObtenInterfasRed().ToString());
                   TexBoxDestino.Focus(); ////cambiar el foco al proximo campo una vez rellenado el primero.
                }
                catch
                {
                    MessageBox.Show("Introduzca Una Dirección de Origen por favor.");
                    TexBoxOrigen.Focus();
                }
                

            }
        }

        private void TexBoxDestino_KeyDown(object sender, KeyEventArgs e)
        {
            TexBoxEstadoViaje.Text = "";
            if (e.Key == Key.Enter)
            {
               try
               {
                    string direccionDestino = TexBoxDestino.Text;
                    DireccionDestinoViaje = direccionDestino;
                    CoordenadasDestino = ServicioFlota.Direccion_CordenadasHere(direccionDestino);
                    DatosTarifa.Text = CoordenadasDestino;  //TODO:Quitar al terminar
                    CheckBoxReservarAhora.Focus();
                    if (CoordenadasDestino == ",")
                    {
                        MessageBox.Show("Escriba una dirección de DESTINO válida");
                        TexBoxDestino.Text = "";
                        TexBoxDestino.Focus();
                    }
                    ImagenMapa.Source = (new BitmapImage(ServicioFlota.ObtenMapaRutaHere(TexBoxOrigen.Text, TexBoxDestino.Text, (int)ImagenMapa.Width, (int)ImagenMapa.Height)));
                    int[] distanciaTiempoViaje = ServicioFlota.ObtenDistanciaTiempoHere(TexBoxOrigen.Text, TexBoxDestino.Text);

                    double TarifaDistancia = ServicioFlota.EncuestaServidorTarifa("http://"+PuertoServerAPI+"/api/Tarifa/Distancia");
                    double TarifaTiempo = ServicioFlota.EncuestaServidorTarifa("http://"+PuertoServerAPI+"/api/Tarifa/Tiempo");

                    PrecioViaje = (distanciaTiempoViaje[0]/1000)*TarifaDistancia + (distanciaTiempoViaje[1]/60)*TarifaTiempo;
                    DatosTarifa.Text = " Distancia: "+(distanciaTiempoViaje[0]/1000).ToString()+" Km"
                                            +"\n Tiempo: "+(distanciaTiempoViaje[1]/60).ToString()+" minutos"
                                            + "\n Precio: "+PrecioViaje+" EUR" ;
                    DistanciaViaje = (distanciaTiempoViaje[0] / 1000).ToString() + " Km";
                    TiempoViajeActual = distanciaTiempoViaje[1];


               }
               catch(Exception)
               {
                    MessageBox.Show("Introduzca una dirección de Destino por favor.");
                    TexBoxDestino.Focus();
                    
               }
            }
        }

        private void CheckBoxReservarAhora_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBoxMascota_Click(object sender, RoutedEventArgs e)
        {
            if (Mascotas_Value == "No") { Mascotas_Value = "Si"; } /////// configuración coche con mascota puesta a true.
            else { Mascotas_Value = "No"; }

        }

        private void CheckBoxFumadores_Click(object sender, RoutedEventArgs e)
        {
            if (Fumadores_Value == "No") { Fumadores_Value = "Si"; }  /////// configuración coche para fumadores puesta a true.
            else { Fumadores_Value = "No"; }

        }

        private void ListBoxConductores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            
                

        }

        private void BotonTaxiCancelar_Click(object sender, RoutedEventArgs e)
        {
            TexBoxEstadoViaje.Text = "cancelando el Viaje....";
            EncuestaFlota creaViaje = new EncuestaFlota();
            temporizador.Stop();
            string idviajeCancelar = IdviajeActual;
            string url = "http://" + PuertoServerAPI + "/api/Flota/EstadoViaje/Cancelar/" + idviajeCancelar;
            string respuestaCancelacion = creaViaje.EncuestaHttp(url, "GET"); /////CANCELO EL VIAJE ACTUAL Y ACTUALIZO LA BASE DE DATOS.
            TexBoxEstadoViaje.Text = respuestaCancelacion;
            TexBoxEstadoViaje.Text = "VIAJE CANSELADO";
            MessageBox.Show("EL VIAJE YA HA SIDO CANCELADO");

        }

        private void ListBoxConductores_MouseEnter(object sender, MouseEventArgs e)
        { /*
            string numeropuntos = ListaInfoConductores[listBoxConductores.SelectedIndex].PuntuacionConductor.ToString();
            int estrellas = Int32.Parse(numeropuntos);
            switch (estrellas)
            {
                case 1:
                    ImagenPuntuacion1.Visibility = Visibility.Visible;
                    ImagenPuntuacion0.Visibility = Visibility.Hidden;
                    ImagenPuntuacion2.Visibility = Visibility.Hidden;
                    ImagenPuntuacion3.Visibility = Visibility.Hidden;
                    ImagenPuntuacion4.Visibility = Visibility.Hidden;
                    ImagenPuntuacion5.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    ImagenPuntuacion2.Visibility = Visibility.Visible;
                    ImagenPuntuacion0.Visibility = Visibility.Hidden;
                    ImagenPuntuacion1.Visibility = Visibility.Hidden;
                    ImagenPuntuacion3.Visibility = Visibility.Hidden;
                    ImagenPuntuacion4.Visibility = Visibility.Hidden;
                    ImagenPuntuacion5.Visibility = Visibility.Hidden;
                    break;
                case 3:
                    ImagenPuntuacion3.Visibility = Visibility.Visible;
                    ImagenPuntuacion0.Visibility = Visibility.Hidden;
                    ImagenPuntuacion2.Visibility = Visibility.Hidden;
                    ImagenPuntuacion1.Visibility = Visibility.Hidden;
                    ImagenPuntuacion4.Visibility = Visibility.Hidden;
                    ImagenPuntuacion5.Visibility = Visibility.Hidden;
                    break;
                case 4:
                    ImagenPuntuacion4.Visibility = Visibility.Visible;
                    ImagenPuntuacion0.Visibility = Visibility.Hidden;
                    ImagenPuntuacion2.Visibility = Visibility.Hidden;
                    ImagenPuntuacion3.Visibility = Visibility.Hidden;
                    ImagenPuntuacion1.Visibility = Visibility.Hidden;
                    ImagenPuntuacion5.Visibility = Visibility.Hidden;
                    break;
                case 5:
                    ImagenPuntuacion5.Visibility = Visibility.Visible;
                    ImagenPuntuacion0.Visibility = Visibility.Hidden;
                    ImagenPuntuacion2.Visibility = Visibility.Hidden;
                    ImagenPuntuacion3.Visibility = Visibility.Hidden;
                    ImagenPuntuacion4.Visibility = Visibility.Hidden;
                    ImagenPuntuacion1.Visibility = Visibility.Hidden;
                    break;
                default:
                    ImagenPuntuacion0.Visibility = Visibility.Visible;
                    ImagenPuntuacion1.Visibility = Visibility.Hidden;
                    ImagenPuntuacion2.Visibility = Visibility.Hidden;
                    ImagenPuntuacion3.Visibility = Visibility.Hidden;
                    ImagenPuntuacion4.Visibility = Visibility.Hidden;
                    ImagenPuntuacion5.Visibility = Visibility.Hidden;
                    break;

            
            }
            */

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if ((IdviajeActual != null) & (EstadoViajeActual != "iniciado"))
            {
                TexBoxEstadoViaje.Text = "cancelando el Viaje ....";
                string respuestacancelacion = ServicioFlota.CancelarViajesPendientes(temporizador, IdviajeActual, PuertoServerAPI);
                TexBoxEstadoViaje.Text = respuestacancelacion;
                TexBoxEstadoViaje.Text = "VIAJE CANCELADO";
            }
            
        }

        private void BotonLogOf_Click(object sender, RoutedEventArgs e)  ///cancelar o finalizar el viaje si se desloguea la persona.
        {

            if ((IdviajeActual != null) & (EstadoViajeActual != "iniciado"))
            {
                TexBoxEstadoViaje.Text = "cancelando el Viaje....";
                string respuestacancelacion = ServicioFlota.CancelarViajesPendientes(temporizador, IdviajeActual, PuertoServerAPI);
                TexBoxEstadoViaje.Text = respuestacancelacion;
                TexBoxEstadoViaje.Text = "VIAJE CANCELADO";
            }

            Login EntrardeNuevo = new Login { Visibility = Visibility.Visible };
            this.Visibility = Visibility.Collapsed;


        }
    }
}


