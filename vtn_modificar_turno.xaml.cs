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
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace prototipo_interfaz
{
    /// <summary>
    /// Lógica de interacción para vtn_modificar_turno.xaml
    /// </summary>
    public partial class vtn_modificar_turno : Window
    {
        MySqlConnection conexion = new MySqlConnection("Server = localhost; Database=gestionturnos; Uid=root; Pwd=superad;");
        DataSet tabla_idfound = new DataSet();
        int idfound_turno=0;
        string query = "";
        string telefound_cliente="";

        public vtn_modificar_turno()
        {
            InitializeComponent();

            btn_atcualizarturno.IsEnabled = false;
            //Bloquea los dias pasados en el datapicker
            dtp_fechaturno.BlackoutDates.AddDatesInPast();
            
            CargarCBox cargaCbox = new CargarCBox();
            cargaCbox.CargarDuraciones(cbx_tiempotatu);
            cargaCbox.CargarTatuadores(cbx_tatuador);
            
        }

        

        private void Btn_buscarturno_Click(object sender, RoutedEventArgs e)
        {
            txtbl_buscarturno.Text = "";
            
            if (txt_nombrecliente.Text == "" && txt_telefcliente.Text == "")
            {
                txtbl_buscarturno.Text = "Es necesario completar al menos uno de los campos";
                txtbl_buscarturno.Foreground = Brushes.Red;
            }
            else
            {
                query = "";
                if (txt_nombrecliente.Text != "" && txt_telefcliente.Text != "")
                {
                    query = "SELECT id_turno, telef_cliente, cod_tatuaje, nya_tatuador, lugar_cuerpo, taman_tatuaje, TIME_FORMAT(tiempo_tatuaje, '%H:%i') tiempo_tatuaje, costo_tatuaje, DATE_FORMAT(fecha_turno,'%d/%m/%Y') fecha_turno, TIME_FORMAT(hora_turno, '%H:%i') hora_turno FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion=atenciones.id_atencion " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador=tatuadores.id_tatuador WHERE nya_cliente='" + txt_nombrecliente.Text + "' AND telef_cliente='"+txt_telefcliente.Text+"' AND estado_turno='Reservado';";
                }
                else
                {
                    if (txt_nombrecliente.Text != "")
                    {
                        query = "SELECT id_turno, telef_cliente, cod_tatuaje, nya_tatuador, lugar_cuerpo, taman_tatuaje, TIME_FORMAT(tiempo_tatuaje, '%H:%i') tiempo_tatuaje, costo_tatuaje, DATE_FORMAT(fecha_turno,'%d/%m/%Y') fecha_turno, TIME_FORMAT(hora_turno, '%H:%i') hora_turno FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion=atenciones.id_atencion " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador=tatuadores.id_tatuador WHERE nya_cliente='" + txt_nombrecliente.Text + "' AND estado_turno='Reservado';";
                    }
                    else
                    {
                        query = "SELECT id_turno, telef_cliente, cod_tatuaje, nya_tatuador, lugar_cuerpo, taman_tatuaje, TIME_FORMAT(tiempo_tatuaje, '%H:%i') tiempo_tatuaje, costo_tatuaje, DATE_FORMAT(fecha_turno,'%d/%m/%Y') fecha_turno, TIME_FORMAT(hora_turno, '%H:%i') hora_turno FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion=atenciones.id_atencion " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador=tatuadores.id_tatuador WHERE telef_cliente='" + txt_telefcliente.Text + "' AND estado_turno='Reservado';";
                    }
                }

                try
                {
                    idfound_turno = 0;
                    telefound_cliente = "";
                    conexion.Open();
                    //Buscar el Id del turno para cargar los text box y combo box y luego modificar
                    /*MySqlCommand comando0 = new MySqlCommand("SELECT id_turno, telef_cliente, cod_tatuaje, nya_tatuador, lugar_cuerpo, taman_tatuaje, TIME_FORMAT(tiempo_tatuaje, '%H:%i') tiempo_tatuaje, costo_tatuaje, DATE_FORMAT(fecha_turno,'%d/%m/%Y') fecha_turno, TIME_FORMAT(hora_turno, '%H:%i') hora_turno FROM turnos " +
                        "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                        "INNER JOIN atenciones ON turnos.idfk_atencion=atenciones.id_atencion " +
                        "INNER JOIN tatuadores ON turnos.idfk_tatuador=tatuadores.id_tatuador WHERE nya_cliente='" + txt_nombrecliente.Text + "' AND estado_turno='Reservado';", conexion);*/

                    MySqlCommand comando0 = new MySqlCommand(query, conexion);
                    MySqlDataAdapter adaptador = new MySqlDataAdapter();
                    adaptador.SelectCommand = comando0;
                    tabla_idfound.Clear();
                    adaptador.Fill(tabla_idfound);
                    conexion.Close();

                    if (tabla_idfound.Tables[0].Rows.Count == 0)
                    {
                        txtbl_buscarturno.Text = "No se encontró ningun turno para los datos especificados";
                        txtbl_buscarturno.Foreground = Brushes.Red;
                        btn_atcualizarturno.IsEnabled = false;
                        //Limpiar_Campos();
                    }
                    else
                    {
                        btn_atcualizarturno.IsEnabled = true;
                        idfound_turno = Int32.Parse(tabla_idfound.Tables[0].Rows[0]["id_turno"].ToString());

                        cbx_motivo.Text = tabla_idfound.Tables[0].Rows[0]["cod_tatuaje"].ToString();
                        //cambio de text por display
                        cbx_tatuador.Text = tabla_idfound.Tables[0].Rows[0]["nya_tatuador"].ToString();
                        cbx_lugar.Text = tabla_idfound.Tables[0].Rows[0]["lugar_cuerpo"].ToString();
                        txt_tamañotatu.Text = tabla_idfound.Tables[0].Rows[0]["taman_tatuaje"].ToString();
                        cbx_tiempotatu.Text = tabla_idfound.Tables[0].Rows[0]["tiempo_tatuaje"].ToString();
                        txt_costotatu.Text = tabla_idfound.Tables[0].Rows[0]["costo_tatuaje"].ToString();
                        dtp_fechaturno.Text = tabla_idfound.Tables[0].Rows[0]["fecha_turno"].ToString();
                        //dtp_fechaturno.SelectedDate = DateTime.ParseExact(tabla_idfound.Tables[0].Rows[0]["fecha_turno"].ToString(), "dd/MM/yyy", System.Globalization.CultureInfo.InvariantCulture);
                        cbx_horaturno.Text = tabla_idfound.Tables[0].Rows[0]["hora_turno"].ToString();
                        telefound_cliente = tabla_idfound.Tables[0].Rows[0]["telef_cliente"].ToString();

                        CargarCBox cboxHorarios = new CargarCBox();
                        try
                        {
                            cbx_horaturno.ItemsSource = null;
                            cboxHorarios.CargarHorarios(cbx_horaturno, cbx_tatuador.SelectedValue.ToString(), dtp_fechaturno.SelectedDate.Value.ToString("yyyy/MM/dd"));
                        }
                        catch (Exception error)
                        {
                            MessageBox.Show("Error de clase cargarCBox Linea 135: " + error.Message);
                        }
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("No se pudo operar sobre la BD, Linea 141 Error: " + error.Message);
                }
            }
        }

        private void Btn_atcualizarturno_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos() == false)
            {
                txtbl_camposvacios.Text = "Todos los campos son obligatorios";
                txtbl_camposvacios.Foreground = Brushes.Red;
            }
            else
            {
                try
                {
                    
                    conexion.Open();
                    //Obtener el id del tatuador a partir del nombre en seleccionado del combobox
                    MySqlCommand comando0 = new MySqlCommand("SELECT id_tatuador, email_tatuador FROM tatuadores WHERE id_tatuador='" + cbx_tatuador.SelectedValue.ToString() + "';", conexion);
                    MySqlDataAdapter adaptador = new MySqlDataAdapter();
                    adaptador.SelectCommand = comando0;
                    DataSet tabla_idfoundtatuador = new DataSet();
                    adaptador.Fill(tabla_idfoundtatuador);
                    int idfound_tatuador = Int32.Parse(tabla_idfoundtatuador.Tables[0].Rows[0]["id_tatuador"].ToString());
                    string correofound_tatuador = tabla_idfoundtatuador.Tables[0].Rows[0]["email_tatuador"].ToString();

                    //Actualizar la atencion o detalles de la atencion
                    query = "UPDATE atenciones SET cod_tatuaje='" + cbx_motivo.Text +
                        "', lugar_cuerpo='" + cbx_lugar.Text +
                        "', taman_tatuaje='" + txt_tamañotatu.Text +
                        "', tiempo_tatuaje='" + cbx_tiempotatu.Text +
                        "', costo_tatuaje='" + txt_costotatu.Text + "' WHERE id_atencion = " + idfound_turno + ";";
                    MySqlCommand comando1 = new MySqlCommand(query, conexion);
                    comando1.ExecuteNonQuery();
                    //MessageBox.Show("Se cambiaron los detalles de tatuaje");

                    //Actualizar el turno
                    query = "UPDATE turnos SET fecha_turno='" + dtp_fechaturno.SelectedDate.Value.ToString("yyyy/MM/dd") +
                        "', hora_turno='" + cbx_horaturno.Text +
                        "', idfk_tatuador=" + cbx_tatuador.SelectedValue.ToString() + " WHERE id_turno = " + idfound_turno + ";";
                    MySqlCommand comando2 = new MySqlCommand(query, conexion);
                    comando2.ExecuteNonQuery();
                    //MessageBox.Show("Se cambio el turno");

                    conexion.Close();

                    EnviarCorreo(correofound_tatuador, telefound_cliente);
                    MessageBox.Show("El turno ha sido modificado correctamente y se ha enviado una notificación al tatuador");
                    this.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
                }
            }
            
           
            
        }

        private void Btn_cancelaragregarturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EnviarCorreo(string correo_tatuador, string telefono_cliente)
        {
            MailMessage mail_turno = new MailMessage();
            mail_turno.To.Add(correo_tatuador);
            mail_turno.Subject = "Charly Music: Actualizacion del Turno Registrado";
            mail_turno.SubjectEncoding = Encoding.UTF8;
            mail_turno.Body = "Fecha: " + dtp_fechaturno.SelectedDate.Value.ToString("yyyy/MM/dd") + "     Hora: " + cbx_horaturno.Text + "\n" +
                "Cliente: " + txt_nombrecliente.Text + "     Teléfono: " + telefono_cliente + "\n" +
                "Tatuaje: " + cbx_motivo.Text + "     Tamaño: " + txt_tamañotatu.Text + "     Lugar: " + cbx_lugar.Text + "\n" +
                "Duración de la sesión: " + cbx_tiempotatu.Text;
            mail_turno.BodyEncoding = Encoding.UTF8;
            mail_turno.From = new MailAddress("endoftheline.92@gmail.com");

            SmtpClient cliente_correo = new SmtpClient();
            cliente_correo.Credentials = new NetworkCredential("endoftheline.92@gmail.com", "Charizard_92");
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
            //
            if (cbx_tatuador.Text != "" && dtp_fechaturno.Text != "" && cbx_horaturno.Text != "" && cbx_motivo.Text != "" && cbx_lugar.Text != "" && txt_tamañotatu.Text != "" && cbx_tiempotatu.Text != "" && txt_costotatu.Text != "")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void Limpiar_Campos()
        {
            cbx_motivo = null;
            cbx_tatuador.Text = null;
            cbx_lugar.Text = null;
            txt_tamañotatu.Text = null;
            cbx_tiempotatu.Text = null;
            txt_costotatu.Text = null;
            dtp_fechaturno.Text = null;
            cbx_horaturno.Text = null;

        }

        

        private void Cbx_tatuador_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                        MessageBox.Show("Error de clase cargarCBox tatuador: " + error.Message);
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
                        MessageBox.Show("Error de clase cargarCBox fecha: " + error.Message);
                    }
                }
            }
            catch (Exception error2)
            {

                MessageBox.Show("Error de if " + error2.Message);
            }
        }

        private void Txt_telefcliente_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Txt_telefcliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void Txt_costotatu_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int output;
            if (!(int.TryParse(e.Text, out output)))
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
