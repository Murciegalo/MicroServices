using ClienteAPI.Servicios;
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
    /// Lógica de interacción para ConductorViaje.xaml
    /// </summary>
    public partial class Entrar
    {
        public string Mail;
        public string Contrasena;
        public Entrar(string mail, string contrasena)
        {
            Mail = mail;
            Contrasena = contrasena;
        }
    }
    public partial class ConductorViaje : Window
    {
        public string idConductor { get; set; }

        ConductorPrueba viajeprueba = new ConductorPrueba();
        DispatcherTimer timer = new DispatcherTimer();
        ConductorPrueba conductorPrueba = new ConductorPrueba();

        int i = 1;
        // nuevoViaje viene con la información del viaje solicitado por el cliente
        string idViaje;
        //aqui se guardará la información con el viaje que el conductor tendrá que aceptar o rechazar
        string informe = "";
        //aquí se guardarán los atributos del viaje para que se puedan variar en la DB
        string salida_salida;

        dynamic jsonObj_salida;

        // este diccionario relaciona un numero con nuevoViaje
        Dictionary<int, string> dicViaje = new Dictionary<int, string>()
        {
            {1,"Vacío"},
            {2,"Vacío"},
            {3,"Vacío"},
            {4,"Vacío"},
            {5,"Vacío"},
        };

        //este segundo diccionario es para obtener el id de los viajes
        Dictionary<int, string> dicViajeId = new Dictionary<int, string>()
        {
            {1,"Vacío"},
            {2,"Vacío"},
            {3,"Vacío"},
            {4,"Vacío"},
            {5,"Vacío"},
        };


        public void inicio()
        {
            Button_aceptar.IsEnabled = false;
            Button_rechazar.IsEnabled = false;
            Button_terminar.IsEnabled = false;
            Button_iniciar.IsEnabled = false;
            Button_mostrar.IsEnabled = false;
            Combobox_cola.IsEnabled = false;
        }
        //situacion en la que el conductor tiene almacenados viajes y desde donde puede gestionarlos
        public void hay_viajes()
        {
            Button_iniciar.IsEnabled = true;
            Button_mostrar.IsEnabled = true;
            Combobox_cola.IsEnabled = true;

            if (nuevo_viaje.Text == "Buscando")
            {
                Button_aceptar.IsEnabled = false;
                Button_rechazar.IsEnabled = false;
            }

        }
        //situacion en la que el conductor ha empezado el viaje
        public void iniciado()
        {
            Button_terminar.IsEnabled = true;
            Button_iniciar.IsEnabled = false;
            Combobox_cola.IsEnabled = false;

            if (nuevo_viaje.Text == "Buscando")
            {
                Button_aceptar.IsEnabled = false;
                Button_rechazar.IsEnabled = false;
            }
        }
        //se comprueba si el conductor tiene más viajes reservados
        public void comprobar_viajes()
        {
            for (int i = 1; i <= 5; i++)
            {
                if (dicViaje[i] != "Vacío")
                {
                    hay_viajes();
                    break;
                }
                else
                {
                    inicio();
                }
            }
        }
        //timer para buscar nuevos viajes
        public void buscador()
        {
            try
            {// hay que modificar que si esta vacio no pete
                if (idConductor != "")
                {
                    //En un intervalo de tiempo va a conectar con el controlador y le devuelve el nombre del conductor, el estado del conductor y el estado del viaje
                    timer.Interval = new TimeSpan(0, 0, 2);
                    timer.Tick += (s, a) =>
                    {
                        conductorPrueba.Id = idConductor;
                        string n = conductorPrueba.Id;
                        string salida = Servicios.Servicios.GetRequest("http://localhost:50414/api/viaje/pruebaviaje/" + n);
                        // si el viaje es reservado se muestra el resultado
                        dynamic jsonObj = JsonConvert.DeserializeObject(salida);

                        if (salida.Contains("solicitado"))
                        {

                            string idViaje_salida = jsonObj["IdViaje"];
                            string origen = jsonObj["origen"];
                            string destino = jsonObj["destino"];
                            string distancia = jsonObj["distancia"];
                            string horainicio1 = jsonObj["horainicio"];
                            DateTime horafinal = jsonObj["horafinal"];
                            string tarifa = jsonObj["tarifa"];
                            string puntuacion = jsonObj["puntuacion"];
                            string idCliente = jsonObj["IdCliente"];
                            string idConductor = jsonObj["IdConductor"];
                            string estadoViaje = jsonObj["estadoViaje"];
                            string comentarioCliente = jsonObj["comentarioCliente"];

                            //Se quitan las "" que salen de más por defecto
                            string horainicio2 = horainicio1.Remove(0, 11);
                            int horainicio2len = horainicio2.Length;
                            //en idlogin queda almacenado el id del usuario estrado
                            string horainicio = horainicio2.Remove(horainicio2len - 5, 2);

                            //string salida2 = viaje[seleccionado].Remove(0, 1);
                            //int len = salida2.Length;
                            //esta es la salida buena
                            //string salida3 = salida2.Remove(len - 1);
                            //  var inicioViaje = new ViajePrueba(idViaje + origen + destino + distancia + horainicio + horafinal + tarifa + puntuacion + idCliente + idConductor + estadoViaje + comentarioCliente);



                            string inform = ("TIENES UN VIAJE NUEVO:" + "\n" + "\n" + "Origen: " + origen + "\n" +
                                "Destino: " + destino + "\n" +
                               "Distancia: " + distancia + "\n" +
                                "Hora de inicio: " + horainicio + "\n" +
                                "Tarifa: " + tarifa);

                            informe = inform;
                            idViaje = idViaje_salida.ToString();
                            salida_salida = salida;
                            jsonObj_salida = jsonObj;

                            nuevo_viaje.Text = informe;

                        }
                        // Si no se muestra el viaje, el boton tendria que estar deshabilitado
                        else
                        {
                            nuevo_viaje.Text = "Buscando";
                            Button_aceptar.IsEnabled = false;
                            Button_rechazar.IsEnabled = false;
                        }
                        //timer.Tick += new EventHandler(Timer_Tick);
                        if (nuevo_viaje.Text != "Buscando")
                        {
                            Button_aceptar.IsEnabled = true;
                            Button_rechazar.IsEnabled = true;
                        }
                    };
                    timer.Start();

                    nuevo_viaje.Text = "Buscando";

                }
                else { }
            }
            catch (Exception)
            {
                nuevo_viaje.Text = "El campo no puede estar vacio";
            }
        }


        public ConductorViaje(string idlogin)
        {
            // al principio está todo desabilitado hasta que nuevoViaje toma un valor
            InitializeComponent();
            idConductor = idlogin;
            inicio();
            buscador();


        }

        private void Button_aceptar_Click(object sender, RoutedEventArgs e)
        {
            //valueDic es para darle valores al diccionario
            string valueDicViaje = informe;
            string valueDicViajeId = idViaje;

            jsonObj_salida["estadoViaje"] = "aceptado";
            var r = JsonConvert.DeserializeObject(salida_salida);
            string cliente_JSON = JsonConvert.SerializeObject(r, Formatting.Indented);
            string salida2 = Servicios.Servicios.PostRequest("http://localhost:50414/api/viajes/modificar/", jsonObj_salida);

            //--------------------------------------------------------HACER UN EVENTO PARA LLAMAR A UN VIAJE CONCRETO-----------------------

            //se activan los botones de cambio de estado
            hay_viajes();

            //se guarda el nuevo viaje en la primera Key vacia del diccionario
            for (i = 1; i <= 5; i++)
            {
                if (dicViaje[i] == "Vacío")
                {
                    dicViaje[i] = valueDicViaje;
                    dicViajeId[i] = valueDicViajeId;

                    break;
                }
            }
            Combobox_cola.IsEnabled = true;
            var viajes = new List<int> { 1, 2, 3, 4, 5 };
            Combobox_cola.ItemsSource = viajes;

            nuevo_viaje.Text = "Buscando";

            if (nuevo_viaje.Text == "Buscando")
            {
                Button_aceptar.IsEnabled = false;
                Button_rechazar.IsEnabled = false;
            }

        }

        private void Button_rechazar_Click(object sender, RoutedEventArgs e)
        {
            jsonObj_salida["estadoViaje"] = "rechazado";
            var r = JsonConvert.DeserializeObject(salida_salida);
            string cliente_JSON = JsonConvert.SerializeObject(r, Formatting.Indented);
            string salida2 = Servicios.Servicios.PostRequest("http://localhost:50414/api/viajes/modificar/", jsonObj_salida);

            comprobar_viajes();
            //se vacia la llegada de informacion para un nuevo viaje
            idViaje = "";
            nuevo_viaje.Text = "Buscando";

            //en el caso de que no queden viajes, se vuelve a la situación inicial
            comprobar_viajes();

        }

        private void ComboBox_cola_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_iniciar_Click(object sender, RoutedEventArgs e)
        {
            iniciado();

            //se selecciona el viaje que se ha escogido en el combobox
            int seleccionado = (int)Combobox_cola.SelectedItem.GetHashCode();


            //Se cambia el estado del viaje que se ha iniciado
            conductorPrueba.Id = dicViajeId[seleccionado];
            string n = conductorPrueba.Id;
            string salida = Servicios.Servicios.GetRequest("http://localhost:50414/api/viaje/iniciado/" + n);


            // si el viaje es reservado se muestra el resultado

            if (salida.Contains(seleccionado.ToString()))
            {
                dynamic jsonObj = JsonConvert.DeserializeObject(salida);
                string idViaje_salida = jsonObj["IdViaje"];
                string origen = jsonObj["origen"];
                string destino = jsonObj["destino"];
                string distancia = jsonObj["distancia"];
                string horainicio1 = jsonObj["horainicio"];
                DateTime horafinal = jsonObj["horafinal"];
                string tarifa = jsonObj["tarifa"];
                string puntuacion = jsonObj["puntuacion"];
                string idCliente = jsonObj["IdCliente"];
                string idConductor = jsonObj["IdConductor"];
                string estadoViaje = jsonObj["estadoViaje"];


                idViaje = idViaje_salida.ToString();
                salida_salida = salida;
                jsonObj_salida = jsonObj;

                jsonObj_salida["estadoViaje"] = "iniciado";
                var r = JsonConvert.DeserializeObject(salida_salida);
                string cliente_JSON = JsonConvert.SerializeObject(r, Formatting.Indented);
                string salida2 = Servicios.Servicios.PostRequest("http://localhost:50414/api/viajes/modificar/", jsonObj_salida);

            }

            //al aceptar, se activa el boton de terminar
            Button_terminar.IsEnabled = true;
        }

        private void Button_terminar_Click(object sender, RoutedEventArgs e)
        {


            //se selecciona el viaje que se ha escogido en el combobox
            int seleccionado = (int)Combobox_cola.SelectedItem.GetHashCode();


            //Se cambia el estado del viaje que se ha iniciado
            conductorPrueba.Id = dicViajeId[seleccionado];
            string n = conductorPrueba.Id;
            string salida = Servicios.Servicios.GetRequest("http://localhost:50414/api/viaje/finalizado/" + n);
            // si el viaje es reservado se muestra el resultado

            if (salida.Contains(seleccionado.ToString()))
            {
                dynamic jsonObj = JsonConvert.DeserializeObject(salida);
                string idViaje_salida = jsonObj["IdViaje"];
                string origen = jsonObj["origen"];
                string destino = jsonObj["destino"];
                string distancia = jsonObj["distancia"];
                string horainicio1 = jsonObj["horainicio"];
                DateTime horafinal = jsonObj["horafinal"];
                string tarifa = jsonObj["tarifa"];
                string puntuacion = jsonObj["puntuacion"];
                string idCliente = jsonObj["IdCliente"];
                string idConductor = jsonObj["IdConductor"];
                string estadoViaje = jsonObj["estadoViaje"];


                idViaje = idViaje_salida.ToString();
                salida_salida = salida;
                jsonObj_salida = jsonObj;

                jsonObj_salida["estadoViaje"] = "finalizado";
                jsonObj_salida["horafinal"] = DateTime.Now;
                var r = JsonConvert.DeserializeObject(salida_salida);
                string cliente_JSON = JsonConvert.SerializeObject(r, Formatting.Indented);
                string salida2 = Servicios.Servicios.PostRequest("http://localhost:50414/api/viajes/modificar/", jsonObj_salida);

            }
            //se elimina el viaje terminado
            dicViaje[seleccionado] = "Vacío";
            dicViajeId[seleccionado] = "Vacío";

            comprobar_viajes();
        }

        private void Button_mostrar_Click(object sender, RoutedEventArgs e)
        {
            int seleccionado = (int)Combobox_cola.SelectedItem.GetHashCode();

            //se muestra informacion de los viajes seleccionados en el combobox
            MessageBox.Show(dicViaje[seleccionado]);
        }

        private void Button_Salir(object sender, RoutedEventArgs e)
        {
            LoginConductor loginC = new LoginConductor();
            loginC.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Hidden;
        }
    }
}