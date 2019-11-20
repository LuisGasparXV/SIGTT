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

namespace prototipo_interfaz
{
    class CargarCBox
    {
        MySqlConnection conexion = new MySqlConnection("Server = localhost; Database=gestionturnos; Uid=root; Pwd=superad;");
        DataSet tablacargada = new DataSet();
        

        public void CargarTatuadores(ComboBox cboxtat)
        {
            try
            {
                
                conexion.Open();
                MySqlCommand comando0 = new MySqlCommand("SELECT * FROM tatuadores;", conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter();
                adaptador.SelectCommand = comando0;
                DataSet tabla_tat = new DataSet();
                
                adaptador.Fill(tabla_tat);
                cboxtat.Items.Clear();
                cboxtat.ItemsSource = tabla_tat.Tables[0].DefaultView;
                cboxtat.DisplayMemberPath = tabla_tat.Tables[0].Columns["nya_tatuador"].ToString();
                cboxtat.SelectedValuePath = tabla_tat.Tables[0].Columns["id_tatuador"].ToString();

                conexion.Clone();
                
            }
            catch (Exception error)
            {

                MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
            }
           
            
        }
    }
}
