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
        public menu_turnos()
        {
            InitializeComponent();
            try
            {
                conexion.Open();
                
                MySqlCommand comando = new MySqlCommand("SELECT id_turno, DATE_FORMAT(fecha_turno,'%d/%m/%Y') fecha_turno,TIME_FORMAT(hora_turno, '%H:%i') hora_turno,nya_cliente,telef_cliente,nya_tatuador,cod_tatuaje,taman_tatuaje,lugar_cuerpo, TIME_FORMAT(tiempo_tatuaje, '%H:%i') duracion,costo_tatuaje " +
                    "FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador = tatuadores.id_tatuador " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion = atenciones.id_atencion;", conexion);
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
            try
            {
                conexion.Open();

                MySqlCommand comando = new MySqlCommand("SELECT id_turno, DATE_FORMAT(fecha_turno,'%d/%m/%Y') fecha_turno,TIME_FORMAT(hora_turno, '%H:%i') hora_turno,nya_cliente,telef_cliente,nya_tatuador,cod_tatuaje,taman_tatuaje,lugar_cuerpo, TIME_FORMAT(tiempo_tatuaje, '%H:%i') duracion,costo_tatuaje " +
                    "FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador = tatuadores.id_tatuador " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion = atenciones.id_atencion " +
                    "WHERE nya_cliente='"+txt_nombrecliente.Text+"';", conexion);
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

        private void Btn_eliminarturno_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Open();
                //Buscar el Id del turno para eliminar
                MySqlCommand comando0 = new MySqlCommand("SELECT id_turno FROM turnos INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente WHERE nya_cliente='" + txt_nombrecliente.Text + "' AND estado_turno='Reservado';", conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter();
                adaptador.SelectCommand = comando0;
                tabla_idfound.Clear();
                adaptador.Fill(tabla_idfound);
                int idfound_turno = Int32.Parse(tabla_idfound.Tables[0].Rows[0]["id_turno"].ToString());
                //MessageBox.Show("Se encotro el turno: "+idfound_turno);

                query = "DELETE FROM turnos WHERE id_turno = " + idfound_turno + ";";
                MySqlCommand comando1 = new MySqlCommand(query, conexion);
                comando1.ExecuteNonQuery();
                MessageBox.Show("Se elimino turno");

                query = "DELETE FROM atenciones WHERE id_atencion = " + idfound_turno + ";";
                MySqlCommand comando2 = new MySqlCommand(query, conexion);
                comando2.ExecuteNonQuery();
                MessageBox.Show("Se elimino atencion");

                conexion.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
            }
        }

        private void Btn_modifturno_Click(object sender, RoutedEventArgs e)
        {
            vtn_modificar_turno ModificarTurno = new vtn_modificar_turno();
            ModificarTurno.ShowDialog();

        }
    }
}
