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
        }

        private void Btn_guardarturno_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Open();

                //>>>>>> PROBAR PRIMERO AGREGANDO LAS TABLAS mas pequeñas y luego la central

                //Obtener el id del tatuador a partir del nombre en seleccionado del combobox
                MySqlCommand comando0 = new MySqlCommand("SELECT id_tatuador, email_tatuador FROM tatuadores WHERE nya_tatuador='"+cbx_tatuador.Text+"';", conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter();
                adaptador.SelectCommand = comando0;
                tabla_idfound.Clear();
                adaptador.Fill(tabla_idfound);
                int idfound_tatuador = Int32.Parse(tabla_idfound.Tables[0].Rows[0]["id_tatuador"].ToString());
                string correofound_tatuador = tabla_idfound.Tables[0].Rows[0]["email_tatuador"].ToString();
                //MessageBox.Show("Encontró id tatu "+ idfound_tatuador);

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
                string idfound_cliente = tabla_idfound.Tables[0].Rows[0]["MAX(id_cliente)"].ToString();
                //MessageBox.Show("Encontró id cliente " + idfound_cliente);

                //Creando nueva atencion
                query = "INSERT INTO atenciones (cod_tatuaje, taman_tatuaje, lugar_cuerpo, tiempo_tatuaje, costo_tatuaje) VALUES ('"+cbx_motivo.Text+"','"+txt_tamañotatu.Text+"','"+cbx_lugar.Text+"','"+cbx_tiempotatu.Text+"','"+txt_costotatu.Text+"');";
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
                //Datos para el correo
                //string codigo_tatu = tabla_idfound.Tables[0].Rows[0]["cod_tatuaje"].ToString();
                //string tamano_tatu = tabla_idfound.Tables[0].Rows[0]["taman_tatuaje"].ToString();
                //string lugar_cuer = tabla_idfound.Tables[0].Rows[0]["lugar_cuerpo"].ToString();
                //string tiempo_tatu = tabla_idfound.Tables[0].Rows[0]["tiempo_tatuaje"].ToString();
                //MessageBox.Show("Encontró id atencion " + idfound_atencion);

                
                //query = "INSERT INTO turnos (id_turno, fecha, hora, idfk_cliente, idfk_atencion, idfk_tatuador, estado_turno, idfk_usuario) VALUES (5,'15/12/2019','00:30:00','4','2','1','Reservado', NULL);";
                query = "INSERT INTO turnos (id_turno, fecha_turno, hora_turno, idfk_cliente, idfk_atencion, idfk_tatuador, estado_turno) VALUES ("+idfound_atencion+",'"+ dtp_fechaturno.SelectedDate.Value.ToString("yyyy/MM/dd")+"','"+cbx_horaturno.Text+"','"+idfound_cliente+"','"+idfound_atencion+"','"+idfound_tatuador+"','Reservado');";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.ExecuteNonQuery();
                //MessageBox.Show("Se agrego turo");

                //Preparando los datos del turno
                //MySqlCommand comando5 = new MySqlCommand("SELECT fecha_turno, DATE_FORMAT(fecha_turno,'%d/%m/%Y') fecha_turno, TIME_FORMAT(hora_turno, '%h:%i') hora_turno FROM turnos WHERE id_turno=" + idfound_atencion+";", conexion);
                //MySqlDataAdapter adaptador5 = new MySqlDataAdapter();
                //adaptador5.SelectCommand = comando4;
                //tabla_idfound.Clear();
                //adaptador5.Fill(tabla_idfound);
                //Datos para el correo
                //string fecha_turn = tabla_idfound.Tables[0].Rows[0]["fecha_turno"].ToString();
                //string hora_turno = tabla_idfound.Tables[0].Rows[0]["hora_turno"].ToString();
                

                conexion.Close();
                //falta esta sentencia
                enviarCorreo(correofound_tatuador);
            }
            catch (Exception error)
            {
                MessageBox.Show("No se pudo operar sobre la BD, Error: "+error.Message);
            }

            MessageBox.Show("El turno ha sido registrado correctamente y se ha enviado una notificación al tatuador");
            this.Close();
        }

        private void Btn_cancelarguardarturn_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        private void Dtp_fechaturno_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Dtp_fechaturno_CalendarOpened(object sender, RoutedEventArgs e)
        {

        }

        private void enviarCorreo(string correo_tatuador)
        {
            MailMessage mail_turno = new MailMessage();
            mail_turno.To.Add(correo_tatuador);
            mail_turno.Subject = "Charly Music: Nuevo turno registrado";
            mail_turno.SubjectEncoding = System.Text.Encoding.UTF8;
            mail_turno.Body = "Fecha: "+ dtp_fechaturno.SelectedDate.Value.ToString("yyyy/MM/dd") + "     Hora: "+cbx_horaturno.Text+"\n" +
                "Cliente: "+txt_nombrecliente.Text+"   Teléfono: "+txt_telefcliente.Text+"\n" +
                "Tatuaje: "+cbx_motivo.Text+"     Tamaño: "+txt_tamañotatu.Text+"     Lugar: "+cbx_lugar.Text+"\n" +
                "Duración de la sesión: "+cbx_tiempotatu.Text;
            mail_turno.BodyEncoding = System.Text.Encoding.UTF8;
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
    }
}
