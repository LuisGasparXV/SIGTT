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
using prototipo_interfaz.Properties;
using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace prototipo_interfaz
{
    /// <summary>
    /// Lógica de interacción para menu_turnos.xaml
    /// </summary>
    public partial class menu_turnos : Page
    {
        MySqlConnection conexion = new MySqlConnection("Server=localhost; Database=gestionturnos; Uid=root; Pwd=superad;");
        DataSet tabla_idfound = new DataSet();
        string query;
        string fecha_turno_mail;
        string hora_turno_mail;
        public menu_turnos()
        {
            InitializeComponent();
            btn_eliminarturno.IsEnabled = false;
            InicializarTabla();
            
        }

        private void InicializarTabla()
        {
            try
            {
                conexion.Open();

                MySqlCommand comando = new MySqlCommand("SELECT id_turno ID, DATE_FORMAT(fecha_turno,'%d/%m/%Y') Fecha,TIME_FORMAT(hora_turno, '%H:%i') Hora,nya_cliente Cliente,telef_cliente Tel_Cliente,nya_tatuador Tatuador,cod_tatuaje Tatuaje,taman_tatuaje Tamaño,lugar_cuerpo Lugar, TIME_FORMAT(tiempo_tatuaje, '%H:%i') Duración,costo_tatuaje Costo " +
                    "FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador = tatuadores.id_tatuador " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion = atenciones.id_atencion WHERE estado_turno = 'Reservado';", conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter();
                adaptador.SelectCommand = comando;
                DataSet tabla = new DataSet();
                adaptador.Fill(tabla);
                grd_turnos.ItemsSource = tabla.Tables[0].DefaultView;
                conexion.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
            }
        }


        private void Btn_agregarturno_Click(object sender, RoutedEventArgs e)
        {
            vtn_agregar_turno NuevoTurno = new vtn_agregar_turno();
            NuevoTurno.ShowDialog();
        }

        private void Btn_atenderturno_Click(object sender, RoutedEventArgs e)
        {

            vtn_atender_turno AtenderTurno = new vtn_atender_turno();
            //AtenderTurno.txt_nombrecliente = txt_nombrecliente;
            AtenderTurno.ShowDialog();
        }

        private void Grd_turnos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Btn_buscarturno_Click(object sender, RoutedEventArgs e)
        {
            txtbl_buscarturno.Text = "";
            query = "";
            btn_eliminarturno.IsEnabled = false;
            if (txt_nombrecliente.Text == "" && txt_telefcliente.Text == "")
            {
                query = "SELECT id_turno ID, DATE_FORMAT(fecha_turno,'%d/%m/%Y') Fecha,TIME_FORMAT(hora_turno, '%H:%i') Hora,nya_cliente Cliente,telef_cliente Tel_Cliente,nya_tatuador Tatuador,cod_tatuaje Tatuaje,taman_tatuaje Tamaño,lugar_cuerpo Lugar, TIME_FORMAT(tiempo_tatuaje, '%H:%i') Duración,costo_tatuaje Costo " +
                    "FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador = tatuadores.id_tatuador " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion = atenciones.id_atencion " +
                    "WHERE estado_turno='Reservado';";
            }
            else
            {
                
                if (txt_nombrecliente.Text != "" && txt_telefcliente.Text != "")
                {
                    query = "SELECT id_turno ID, DATE_FORMAT(fecha_turno,'%d/%m/%Y') Fecha,TIME_FORMAT(hora_turno, '%H:%i') Hora,nya_cliente Cliente,telef_cliente Tel_Cliente,nya_tatuador Tatuador,cod_tatuaje Tatuaje,taman_tatuaje Tamaño,lugar_cuerpo Lugar, TIME_FORMAT(tiempo_tatuaje, '%H:%i') Duración,costo_tatuaje Costo " +
                    "FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador = tatuadores.id_tatuador " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion = atenciones.id_atencion " +
                    "WHERE nya_cliente='" + txt_nombrecliente.Text + "' AND telef_cliente='" + txt_telefcliente.Text + "' AND estado_turno='Reservado';";
                }
                else
                {
                    if (txt_nombrecliente.Text != "")
                    {
                        query = "SELECT id_turno ID, DATE_FORMAT(fecha_turno,'%d/%m/%Y') Fecha,TIME_FORMAT(hora_turno, '%H:%i') Hora,nya_cliente Cliente,telef_cliente Tel_Cliente,nya_tatuador Tatuador,cod_tatuaje Tatuaje,taman_tatuaje Tamaño,lugar_cuerpo Lugar, TIME_FORMAT(tiempo_tatuaje, '%H:%i') Duración,costo_tatuaje Costo " +
                    "FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador = tatuadores.id_tatuador " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion = atenciones.id_atencion " +
                    "WHERE nya_cliente='" + txt_nombrecliente.Text + "' AND estado_turno='Reservado';";
                    }
                    else
                    {
                        query = "SELECT id_turno ID, DATE_FORMAT(fecha_turno,'%d/%m/%Y') Fecha,TIME_FORMAT(hora_turno, '%H:%i') Hora,nya_cliente Cliente,telef_cliente Tel_Cliente,nya_tatuador Tatuador,cod_tatuaje Tatuaje,taman_tatuaje Tamaño,lugar_cuerpo Lugar, TIME_FORMAT(tiempo_tatuaje, '%H:%i') Duración,costo_tatuaje Costo " +
                    "FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador = tatuadores.id_tatuador " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion = atenciones.id_atencion " +
                    "WHERE telef_cliente='" + txt_telefcliente.Text + "' AND estado_turno='Reservado';";
                    }
                }
                

            }
            try
            {
                conexion.Open();

                /*MySqlCommand comando = new MySqlCommand("SELECT id_turno, DATE_FORMAT(fecha_turno,'%d/%m/%Y') fecha_turno,TIME_FORMAT(hora_turno, '%H:%i') hora_turno,nya_cliente,telef_cliente,nya_tatuador,cod_tatuaje,taman_tatuaje,lugar_cuerpo, TIME_FORMAT(tiempo_tatuaje, '%H:%i') duracion,costo_tatuaje " +
                    "FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador = tatuadores.id_tatuador " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion = atenciones.id_atencion " +
                    "WHERE nya_cliente='" + txt_nombrecliente.Text + "';", conexion);*/

                MySqlCommand comando = new MySqlCommand(query, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter();
                adaptador.SelectCommand = comando;
                DataSet tabla = new DataSet();
                adaptador.Fill(tabla);

                if (tabla.Tables[0].Rows.Count == 0)
                {
                    txtbl_buscarturno.Text = "No se encontró ningun turno para los datos especificados";
                    txtbl_buscarturno.Foreground = Brushes.Red;
                    //btn_eliminarturno.IsEnabled = false;
                }
                else
                {
                    if (tabla.Tables[0].Rows.Count == 1)
                    {
                        btn_eliminarturno.IsEnabled = true;
                    }
                    grd_turnos.ItemsSource = tabla.Tables[0].DefaultView;
                    
                }

                conexion.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
            }


        }

        private void Btn_eliminarturno_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fecha_turno_mail = "";
                hora_turno_mail = "";
                conexion.Open();
                //Buscar el Id del turno para eliminar
                MySqlCommand comando0 = new MySqlCommand("SELECT id_turno, email_tatuador, fecha_turno, hora_turno FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador=tatuadores.id_tatuador " +
                    "WHERE nya_cliente='" + txt_nombrecliente.Text + "' AND estado_turno='Reservado';", conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter();
                adaptador.SelectCommand = comando0;
                tabla_idfound.Clear();
                adaptador.Fill(tabla_idfound);
                int idfound_turno = Int32.Parse(tabla_idfound.Tables[0].Rows[0]["id_turno"].ToString());
                string correofound_tatuador = tabla_idfound.Tables[0].Rows[0]["email_tatuador"].ToString();
                fecha_turno_mail = tabla_idfound.Tables[0].Rows[0]["fecha_turno"].ToString();
                hora_turno_mail = tabla_idfound.Tables[0].Rows[0]["hora_turno"].ToString();

                //MessageBox.Show("Se encotro el turno: "+idfound_turno);

                query = "DELETE FROM turnos WHERE id_turno = " + idfound_turno + ";";
                MySqlCommand comando1 = new MySqlCommand(query, conexion);
                comando1.ExecuteNonQuery();

                query = "DELETE FROM atenciones WHERE id_atencion = " + idfound_turno + ";";
                MySqlCommand comando2 = new MySqlCommand(query, conexion);
                comando2.ExecuteNonQuery();

                conexion.Close();
                EnviarCorreo(correofound_tatuador);
                MessageBox.Show("El turno ha sido cancelado correctamente y se ha enviado una notificación al tatuador");
                InicializarTabla();
            }
            catch (Exception error)
            {
                MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
            }
        }

        private void EnviarCorreo(string correo_tatuador)
        {
            MailMessage mail_turno = new MailMessage();
            mail_turno.To.Add(correo_tatuador);
            mail_turno.Subject = "Charly Music: Turno cancelado";
            mail_turno.SubjectEncoding = Encoding.UTF8;
            mail_turno.Body = "Fecha: " + fecha_turno_mail + "     Hora: " + hora_turno_mail;
                
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

        private void Btn_modifturno_Click(object sender, RoutedEventArgs e)
        {
            vtn_modificar_turno ModificarTurno = new vtn_modificar_turno();
            ModificarTurno.ShowDialog();

        }

        private void Txt_nombrecliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.Key >= Key.A && e.Key <= Key.Z))
            {
                e.Handled = true;
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
    }
}
