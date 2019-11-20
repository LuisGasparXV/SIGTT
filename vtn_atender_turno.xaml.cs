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

namespace prototipo_interfaz
{
    /// <summary>
    /// Lógica de interacción para vtn_atender_turno.xaml
    /// </summary>
    public partial class vtn_atender_turno : Window
    {
        public vtn_atender_turno()
        {
            InitializeComponent();
        }

        private void Btn_atenderturno_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Los datos han sido registrados correctamente");
            this.Close();
        }

        private void Btn_cancelaratender_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
