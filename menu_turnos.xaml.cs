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

namespace prototipo_interfaz
{
    /// <summary>
    /// Lógica de interacción para menu_turnos.xaml
    /// </summary>
    public partial class menu_turnos : Page
    {
        public menu_turnos()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Btn_agregarturno_Click(object sender, RoutedEventArgs e)
        {
            vtn_agregar_turno NuevoTurno = new vtn_agregar_turno();
            NuevoTurno.ShowDialog();
        }

        private void Btn_atenderturno_Click(object sender, RoutedEventArgs e)
        {
            vtn_atender_turno AtenderTurno = new vtn_atender_turno();
            AtenderTurno.ShowDialog();
        }
    }
}
