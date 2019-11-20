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

namespace prototipo_interfaz
{
    /// <summary>
    /// Lógica de interacción para vtn_agregar_turno.xaml
    /// </summary>
    public partial class vtn_agregar_turno : Window
    {
        public vtn_agregar_turno()
        {
            InitializeComponent();
        }

        private void Btn_guardarturno_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("El turno ha sido registrado correctamente y se ha enviado una notificación al tatuador");
            this.Close();
        }

        private void Btn_cancelarguardarturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
