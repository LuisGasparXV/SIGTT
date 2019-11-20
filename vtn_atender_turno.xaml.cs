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

namespace prototipo_interfaz
{
    /// <summary>
    /// Lógica de interacción para vtn_atender_turno.xaml
    /// </summary>
    public partial class vtn_atender_turno : Window
    {
        MySqlConnection conexion = new MySqlConnection("Server = localhost; Database=gestionturnos; Uid=root; Pwd=superad;");
        DataSet tabla_idfound = new DataSet();
        int idfound_turno = 0;
        string query = "";
        string telefound_cliente = "";


        public vtn_atender_turno()
        {
            InitializeComponent();
            
        }

        private void Btn_buscarturno_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                idfound_turno = 0;
                telefound_cliente = "";
                
                conexion.Open();
                //Buscar el Id del turno para cargar los text box y check box y luego modificar
                MySqlCommand comando0 = new MySqlCommand("SELECT id_turno, telef_cliente, edad_cliente, sexo_cliente, profesion, cant_tatuajes, hemofilia, daltonismo FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion=atenciones.id_atencion " +
                    "INNER JOIN tatuadores ON turnos.idfk_tatuador=tatuadores.id_tatuador WHERE nya_cliente='" + txt_nombrecliente.Text + "' AND estado_turno='Reservado';", conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter();
                adaptador.SelectCommand = comando0;
                tabla_idfound.Clear();
                adaptador.Fill(tabla_idfound);
                conexion.Close();


                if (tabla_idfound.Tables[0].Rows.Count == 0)
                {
                    btn_atenderturno.IsEnabled = false;
                    MessageBox.Show("No existe el turno reservado para el nombre ingesado");
                }
                else
                {
                    if (tabla_idfound.Tables[0].Rows[0]["edad_cliente"] !=null)
                    {
                        txt_edadcliente.Text = tabla_idfound.Tables[0].Rows[0]["edad_cliente"].ToString();
                        cbx_sexocliente.Text = tabla_idfound.Tables[0].Rows[0]["sexo_cliente"].ToString();
                        txt_canttatoocliente.Text = tabla_idfound.Tables[0].Rows[0]["cant_tatuajes"].ToString();
                        txt_profesioncliente.Text = tabla_idfound.Tables[0].Rows[0]["profesion"].ToString();
                        cbx_hemofilia.Text = tabla_idfound.Tables[0].Rows[0]["hemofilia"].ToString();
                        cbx_daltonismo.Text = tabla_idfound.Tables[0].Rows[0]["daltonismo"].ToString();
                    }
                    btn_atenderturno.IsEnabled = true;
                }
                
                
            }
            catch (Exception error)
            {
                
                MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
            }

            
        }

        private void Btn_atenderturno_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Open();
                //Buscar el Id del turno para eliminar
                MySqlCommand comando0 = new MySqlCommand("SELECT id_turno, id_cliente FROM turnos " +
                    "INNER JOIN clientes ON turnos.idfk_cliente=clientes.id_cliente WHERE nya_cliente='" + txt_nombrecliente.Text + "';", conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter();
                adaptador.SelectCommand = comando0;
                tabla_idfound.Clear();
                adaptador.Fill(tabla_idfound);
                int idfound_turno = Int32.Parse(tabla_idfound.Tables[0].Rows[0]["id_turno"].ToString());
                int idfound_cliente = Int32.Parse(tabla_idfound.Tables[0].Rows[0]["id_cliente"].ToString());
                
                MessageBox.Show("Se encotro el id");

                query = "UPDATE turnos SET estado_turno='Atendido' WHERE id_turno = " + idfound_turno + ";";
                MySqlCommand comando1 = new MySqlCommand(query, conexion);
                comando1.ExecuteNonQuery();
                MessageBox.Show("Se cambio el estado de turno");

                query = "UPDATE clientes SET edad_cliente='"+txt_edadcliente.Text+
                    "', sexo_cliente='"+cbx_sexocliente.Text+
                    "', profesion='"+txt_profesioncliente.Text+
                    "', cant_tatuajes='"+txt_canttatoocliente.Text+
                    "', hemofilia='"+cbx_hemofilia.Text+
                    "', daltonismo='"+cbx_daltonismo.Text+"' WHERE id_cliente = " + idfound_cliente + ";";
                MySqlCommand comando2 = new MySqlCommand(query, conexion);
                comando2.ExecuteNonQuery();
                MessageBox.Show("Se guardo los datos del ciente");

                conexion.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
            }

            MessageBox.Show("Los datos han sido registrados correctamente");
            this.Close();
        }

        private void Btn_cancelaratender_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
