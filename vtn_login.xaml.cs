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


namespace prototipo_interfaz
{
    /// <summary>
    /// Lógica de interacción para vtn_login.xaml
    /// </summary>
    public partial class vtn_login : Window
    {
        MySqlConnection conexion = new MySqlConnection("Server=localhost; Database=gestionturnos; Uid=root; Pwd=superad;");
        public vtn_login()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Btn_cerrarlog_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            
        }

        private void Btn_iniciosesion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Open();
                
                conexion.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
