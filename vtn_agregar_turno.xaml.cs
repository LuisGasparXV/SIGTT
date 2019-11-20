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
using prototipo_interfaz.Properties;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;


namespace prototipo_interfaz
{
    /// <summary>
    /// Lógica de interacción para vtn_agregar_turno.xaml
    /// </summary>
    public partial class vtn_agregar_turno : Window
    {
        MySqlConnection conexion = new MySqlConnection("Server = localhost; Database=gestionturnos; Uid=root; Pwd=superad;");
        DataSet tabla_idfound = new DataSet();
        string query = "";
        
        

        public vtn_agregar_turno()
        {
            InitializeComponent();

            //Bloquea los dias pasados en el datapicker
            dtp_fechaturno.BlackoutDates.AddDatesInPast();
            
            
            CargarCBox cargaTat = new CargarCBox();
            cargaTat.CargarDuraciones(cbx_tiempotatu);
            cargaTat.CargarTatuadores(cbx_tatuador);

            
        }

        private void Btn_guardarturno_Click(object sender, RoutedEventArgs e)
        {

            if (ValidarCampos()==false)
            {
                txtbl_camposvacios.Text = "Todos los campos son obligatorios";
                txtbl_camposvacios.Foreground = Brushes.Red;
            }
            else
            {
                try
                {
                    string idfoundCliente;
                    idfoundCliente = "";
                    conexion.Open();

                    //Obtener el id del tatuador a partir del nombre en seleccionado del combobox
                    MySqlCommand comando0 = new MySqlCommand("SELECT email_tatuador FROM tatuadores WHERE id_tatuador='" + cbx_tatuador.SelectedValue.ToString() + "';", conexion);
                    MySqlDataAdapter adaptador = new MySqlDataAdapter();
                    adaptador.SelectCommand = comando0;
                    tabla_idfound.Clear();
                    adaptador.Fill(tabla_idfound);
                    //int idfound_tatuador = Int32.Parse(tabla_idfound.Tables[0].Rows[0]["id_tatuador"].ToString());
                    string correofound_tatuador = tabla_idfound.Tables[0].Rows[0]["email_tatuador"].ToString();
                    //MessageBox.Show("Encontró id tatu "+ idfound_tatuador);

                    //Buscamos en la tabla cliente si el cliente ya existe
                    MySqlCommand comandoA = new MySqlCommand("SELECT id_cliente FROM clientes WHERE telef_cliente='" + txt_telefcliente.Text + "' AND nya_cliente='"+txt_nombrecliente.Text+"';", conexion);
                    MySqlDataAdapter adaptadorA = new MySqlDataAdapter();
                    adaptadorA.SelectCommand = comandoA;
                    tabla_idfound.Clear();
                    adaptadorA.Fill(tabla_idfound);
                    //int idfound_tatuador = Int32.Parse(tabla_idfound.Tables[0].Rows[0]["id_tatuador"].ToString());
                    
                    //MessageBox.Show("Encontró id tatu "+ idfound_tatuador);

                    if (tabla_idfound.Tables[0].Rows.Count == 1)
                    {
                        idfoundCliente = tabla_idfound.Tables[0].Rows[0]["id_cliente"].ToString();
                    }
                    else
                    {
                        //Creando nuevo cliente
                        query = "INSERT INTO clientes (nya_cliente, telef_cliente) VALUES ('" + txt_nombrecliente.Text + "','" + txt_telefcliente.Text + "');";
                        MySqlCommand comando1 = new MySqlCommand(query, conexion);
                        comando1.ExecuteNonQuery();
                        //MessageBox.Show("Se agrego cliente");

                        //Buscando ultimo id cliente
                        MySqlCommand comando2 = new MySqlCommand("SELECT MAX(id_cliente) FROM clientes;", conexion);
                        MySqlDataAdapter adaptador2 = new MySqlDataAdapter();
                        adaptador2.SelectCommand = comando2;
                        tabla_idfound.Clear();
                        adaptador2.Fill(tabla_idfound);
                        idfoundCliente = tabla_idfound.Tables[0].Rows[0]["MAX(id_cliente)"].ToString();
                        //MessageBox.Show("Encontró id cliente " + idfound_cliente);
                    }

                    //Creando nueva atencion
                    query = "INSERT INTO atenciones (cod_tatuaje, taman_tatuaje, lugar_cuerpo, tiempo_tatuaje, costo_tatuaje, modulos_tiempo) VALUES ('" + cbx_motivo.Text + "','" + txt_tamañotatu.Text + "','" + cbx_lugar.Text + "','" + cbx_tiempotatu.Text + "','" + txt_costotatu.Text + "','" + cbx_tiempotatu.SelectedValue.ToString() + "');";
                    MySqlCommand comando3 = new MySqlCommand(query, conexion);
                    comando3.ExecuteNonQuery();
                    //MessageBox.Show("Se agrego atencion");
                    //Buscando ultimo id atencion
                    
                    MySqlCommand comando4 = new MySqlCommand("SELECT MAX(id_atencion), cod_tatuaje, taman_tatuaje, lugar_cuerpo, tiempo_tatuaje FROM atenciones;", conexion);
                    MySqlDataAdapter adaptador4 = new MySqlDataAdapter();
                    adaptador4.SelectCommand = comando4;
                    tabla_idfound.Clear();
                    adaptador4.Fill(tabla_idfound);
                    string idfound_atencion = tabla_idfound.Tables[0].Rows[0]["MAX(id_atencion)"].ToString();
                    
                    //Preparando los datos del turno
                    query = "INSERT INTO turnos (id_turno, fecha_turno, hora_turno, idfk_cliente, idfk_atencion, idfk_tatuador, estado_turno) VALUES (" + idfound_atencion + ",'" + dtp_fechaturno.SelectedDate.Value.ToString("yyyy/MM/dd") + "','" + cbx_horaturno.Text + "','" + idfoundCliente + "','" + idfound_atencion + "','" + cbx_tatuador.SelectedValue.ToString() + "','Reservado');";
                    MySqlCommand comando = new MySqlCommand(query, conexion);
                    comando.ExecuteNonQuery();
                    //MessageBox.Show("Se agrego turo");

                    conexion.Close();
                    EnviarCorreo(correofound_tatuador);
                    
                    MessageBox.Show("El turno ha sido registrado correctamente y se ha enviado una notificación al tatuador");
                }
                catch (Exception error)
                {
                    MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
                }
                this.Close();
            }

           
            
        }

        private void Btn_cancelarguardarturn_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }


        private void Cbx_tatuador_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //dtp_fechaturno.IsEnabled = true;
            try
            {
                if (dtp_fechaturno.SelectedDate.ToString() != "")
                {
                    
                    CargarCBox cargaHora = new CargarCBox();
                    cbx_horaturno.ItemsSource = null;
                    try
                    {
                        cargaHora.CargarHorarios(cbx_horaturno, cbx_tatuador.SelectedValue.ToString(), dtp_fechaturno.SelectedDate.Value.ToString("yyyy/MM/dd"));
                        
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Error de clase cargarCBox_Tatuador: " + error.Message);
                    }
                }
                
            }
            catch (Exception error2)
            {

                MessageBox.Show("Error de if: " + error2.Message);
            }
            
            
        }

        private void Dtp_fechaturno_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
            try
            {
                if (cbx_tatuador.Text != "")
                {
                    CargarCBox cargaHora = new CargarCBox();
                    cbx_horaturno.ItemsSource = null;
                    try
                    {
                        cargaHora.CargarHorarios(cbx_horaturno, cbx_tatuador.SelectedValue.ToString(), dtp_fechaturno.SelectedDate.Value.ToString("yyyy/MM/dd"));
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Error de clase cargarCBox: " + error.Message);
                    }
                }
            }
            catch (Exception error2)
            {

                MessageBox.Show("Error de if " + error2.Message);
            }
            
        }

        private void Dtp_fechaturno_CalendarOpened(object sender, RoutedEventArgs e)
        {

        }

        private void EnviarCorreo(string correo_tatuador)
        {
            MailMessage mail_turno = new MailMessage();
            mail_turno.To.Add(correo_tatuador);
            mail_turno.Subject = "Charly Music: Nuevo turno registrado";
            mail_turno.SubjectEncoding = Encoding.UTF8;
            mail_turno.Body = "Fecha: "+ dtp_fechaturno.SelectedDate.Value.ToString("yyyy/MM/dd") + "     Hora: "+cbx_horaturno.Text+"\n" +
                "Cliente: "+txt_nombrecliente.Text+"   Teléfono: "+txt_telefcliente.Text+"\n" +
                "Tatuaje: "+cbx_motivo.Text+"     Tamaño: "+txt_tamañotatu.Text+"     Lugar: "+cbx_lugar.Text+"\n" +
                "Duración de la sesión: "+cbx_tiempotatu.Text;
            mail_turno.BodyEncoding = Encoding.UTF8;
            mail_turno.From = new MailAddress("endoftheline.92@gmail.com");

            SmtpClient cliente_correo = new SmtpClient();
            cliente_correo.Credentials = new NetworkCredential("endoftheline.92@gmail.com","Charizard_92");
            cliente_correo.Port = 587;
            cliente_correo.EnableSsl = true;
            cliente_correo.Host = "smtp.gmail.com";

            try
            {
                cliente_correo.Send(mail_turno);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al enviar correo: " + error);
            }

        }

        private bool ValidarCampos()
        {
            if (cbx_tatuador.Text != "" && dtp_fechaturno.Text != "" && cbx_horaturno.Text != "" && txt_telefcliente.Text != "" && txt_nombrecliente.Text != "" && cbx_motivo.Text != "" && cbx_lugar.Text != "" && txt_tamañotatu.Text != "" && cbx_tiempotatu.Text != "" && txt_costotatu.Text != "")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void Txt_telefcliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int output;
            if (!(int.TryParse(e.Text, out output)))
            {
                e.Handled = true;
            }
        }

        private void Txt_telefcliente_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_costotatu_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int output;
            if (!(int.TryParse(e.Text, out output)))
            {
                e.Handled = true;
            }
        }

        private void Txt_nombrecliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.Key >= Key.A && e.Key <= Key.Z))
            {
                e.Handled = true;
            }
            
        }

        private void Txt_tamañotatu_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.X || e.Key == Key.Space))
            {
                e.Handled = true;
            }
        }

        
    }
}
