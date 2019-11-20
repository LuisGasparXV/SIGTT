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
            btn_atenderturno.IsEnabled = false;
        }

        private void Btn_buscarturno_Click(object sender, RoutedEventArgs e)
        {
            txtbl_buscarturno.Text = "";
            btn_atenderturno.IsEnabled = false;

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
                    query = "SELECT id_turno, edad_cliente, sexo_cliente, profesion, cant_tatuajes, hemofilia, daltonismo FROM turnos " +
                        "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                        "INNER JOIN atenciones ON turnos.idfk_atencion=atenciones.id_atencion " +
                        "INNER JOIN tatuadores ON turnos.idfk_tatuador=tatuadores.id_tatuador WHERE nya_cliente='" + txt_nombrecliente.Text + "' AND telef_cliente='" + txt_telefcliente.Text + "' AND estado_turno='Reservado';";
                }
                else
                {
                    if (txt_nombrecliente.Text != "")
                    {
                        query = "SELECT id_turno, edad_cliente, sexo_cliente, profesion, cant_tatuajes, hemofilia, daltonismo FROM turnos " +
                        "INNER JOIN clientes ON turnos.idfk_cliente = clientes.id_cliente " +
                        "INNER JOIN atenciones ON turnos.idfk_atencion=atenciones.id_atencion " +
                        "INNER JOIN tatuadores ON turnos.idfk_tatuador=tatuadores.id_tatuador WHERE nya_cliente='" + txt_nombrecliente.Text + "' AND estado_turno='Reservado';";
                    }
                    else
                    {
                        query = "SELECT id_turno, edad_cliente, sexo_cliente, profesion, cant_tatuajes, hemofilia, daltonismo FROM turnos " +
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
                    //Buscar el Id del turno para cargar los text box y check box y luego modificar
                    /*MySqlCommand comando0 = new MySqlCommand("SELECT id_turno, edad_cliente, sexo_cliente, profesion, cant_tatuajes, hemofilia, daltonismo FROM turnos " +
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
                        //btn_atenderturno.IsEnabled = false;
                        //MessageBox.Show("No existe el turno reservado para el nombre ingesado");
                    }
                    else
                    {
                        if (tabla_idfound.Tables[0].Rows[0]["edad_cliente"] != null)
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
            
        }

        private void Btn_atenderturno_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos() == false)
            {
                txtbl_camposvacios.Text = "Todos los campos de cliente son obligatorios";
                txtbl_camposvacios.Foreground = Brushes.Red;
            }
            else
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

                    query = "UPDATE turnos SET estado_turno='Atendido' WHERE id_turno = " + idfound_turno + ";";
                    MySqlCommand comando1 = new MySqlCommand(query, conexion);
                    comando1.ExecuteNonQuery();

                    query = "UPDATE clientes SET edad_cliente='" + txt_edadcliente.Text +
                        "', sexo_cliente='" + cbx_sexocliente.Text +
                        "', profesion='" + txt_profesioncliente.Text +
                        "', cant_tatuajes='" + txt_canttatoocliente.Text +
                        "', hemofilia='" + cbx_hemofilia.Text +
                        "', daltonismo='" + cbx_daltonismo.Text + "' WHERE id_cliente = " + idfound_cliente + ";";
                    MySqlCommand comando2 = new MySqlCommand(query, conexion);
                    comando2.ExecuteNonQuery();

                    conexion.Close();

                    MessageBox.Show("Los datos han sido registrados correctamente");
                    this.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
                }
            }

            
        }

        private void Btn_cancelaratender_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidarCampos()
        {
            
            if (txt_edadcliente.Text != "" && cbx_sexocliente.Text != "" && txt_canttatoocliente.Text != "" && txt_profesioncliente.Text != "" && cbx_hemofilia.Text != "" && cbx_daltonismo.Text != "" )
            {
                return true;
            }
            else
            {
                return false;
            }

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

        private void Txt_edadcliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int output;
            if (!(int.TryParse(e.Text, out output)))
            {
                e.Handled = true;
            }
        }

        private void Txt_canttatoocliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int output;
            if (!(int.TryParse(e.Text, out output)))
            {
                e.Handled = true;
            }
        }

        private void Txt_profesioncliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.Key >= Key.A && e.Key <= Key.Z))
            {
                e.Handled = true;
            }
        }
    }
}
