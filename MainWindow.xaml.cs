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
using System.Data;
using System.Net;
using System.Net.Mail;

namespace prototipo_interfaz
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_turnos_Click(object sender, RoutedEventArgs e)
        {
            panel_ppal.Content = new menu_turnos();
        }

        private void Btn_catalogo_Click(object sender, RoutedEventArgs e)
        {
            panel_ppal.Content = new menu_catalogo();
        }

        private void Btn_estadistica_Click(object sender, RoutedEventArgs e)
        {
            panel_ppal.Content = new menu_estadisticas();
        }

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Btn_cerrar_Click(object sender, RoutedEventArgs e)
        {
            
            Application.Current.Shutdown();            
        }


        private void Btn_minimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Btn_sesion_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Panel_ppal_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
