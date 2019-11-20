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
        

        public void CargarDuraciones (ComboBox cboxduracion)
        {
            List<duracion> duracion_sesion = new List<duracion>();
            duracion_sesion.Add(new duracion { IdDuracion = 1, Tiempoduracion = "00:30" });
            duracion_sesion.Add(new duracion { IdDuracion = 2, Tiempoduracion = "01:00" });
            duracion_sesion.Add(new duracion { IdDuracion = 3, Tiempoduracion = "01:30" });
            duracion_sesion.Add(new duracion { IdDuracion = 4, Tiempoduracion = "02:00" });
            duracion_sesion.Add(new duracion { IdDuracion = 5, Tiempoduracion = "02:30" });
            duracion_sesion.Add(new duracion { IdDuracion = 6, Tiempoduracion = "03:00" });
            duracion_sesion.Add(new duracion { IdDuracion = 7, Tiempoduracion = "03:30" });
            duracion_sesion.Add(new duracion { IdDuracion = 8, Tiempoduracion = "04:00" });
            duracion_sesion.Add(new duracion { IdDuracion = 9, Tiempoduracion = "04:30" });

            cboxduracion.ItemsSource = null;
            cboxduracion.DisplayMemberPath = "Tiempoduracion";
            cboxduracion.SelectedValuePath = "IdDuracion";
            cboxduracion.ItemsSource = duracion_sesion;
        }

        public class duracion
        {
            private int idDuracion;
            private string tiempoduracion;
            

            public int IdDuracion { get => idDuracion; set => idDuracion = value; }
            
            public string Tiempoduracion { get => this.tiempoduracion; set => this.tiempoduracion = value; }
            
        }

        public class horariosocupados
        {
            private int idHorario;
            private string horario;
            private string duracion;
            private int mododulos;


            public int IdHorario { get => idHorario; set => idHorario = value; }
            public string Horario { get => horario; set => horario = value; }

            public string Duracion { get => this.duracion; set => this.duracion = value; }
            public int Mododulos { get => mododulos; set => mododulos = value; }
            
        }


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

        public void CargarHorarios(ComboBox cboxhora, String tatuador, String fecha)
        {

            

            List<horariosocupados> horasocupadas = new List<horariosocupados>();
            List<string> horariosocup = new List<string>();
            List<string> horariosdisp = new List<string>();
            horariosdisp.Add("09:30");
            horariosdisp.Add("10:00");
            horariosdisp.Add("10:30");
            horariosdisp.Add("11:00");
            horariosdisp.Add("11:30");
            horariosdisp.Add("12:00");
            horariosdisp.Add("12:30");
            horariosdisp.Add("13:00");
           
            horariosdisp.Add("17:00");
            horariosdisp.Add("17:30");
            horariosdisp.Add("18:00");
            horariosdisp.Add("18:30");
            horariosdisp.Add("19:00");
            horariosdisp.Add("19:30");
            horariosdisp.Add("20:00");
            horariosdisp.Add("20:30");
            horariosdisp.Add("21:00");
            horariosdisp.Add("21:30");



            /*cboxhora.ItemsSource = null;
            cboxhora.DisplayMemberPath = "Tiempoduracion";
            cboxhora.SelectedValuePath = "IdDuracion";
            
            cboxhora.ItemsSource = horasturno;*/

            try
            {

                conexion.Open();
                MySqlCommand comando0 = new MySqlCommand("SELECT id_turno,TIME_FORMAT(hora_turno, '%H:%i') hora_turno,TIME_FORMAT(tiempo_tatuaje, '%H:%i') duracion,modulos_tiempo " +
                    "FROM turnos " +
                    "INNER JOIN atenciones ON turnos.idfk_atencion = atenciones.id_atencion " +
                    
                    "WHERE turnos.idfk_tatuador='" + tatuador + "' AND fecha_turno='" + fecha + "' ORDER BY hora_turno;", conexion);
                /*MySqlDataAdapter adaptador = new MySqlDataAdapter();
                adaptador.SelectCommand = comando0;
                DataSet tabla_tat = new DataSet();

                adaptador.Fill(tabla_tat);
                

                /*cboxhora.Items.Clear();
                cboxhora.ItemsSource = tabla_tat.Tables[0].DefaultView;
                cboxhora.DisplayMemberPath = tabla_tat.Tables[0].Columns["hora_turno"].ToString();
                cboxhora.SelectedValuePath = tabla_tat.Tables[0].Columns["modulos_tiempo"].ToString();*/

                MySqlDataReader lectoradap = comando0.ExecuteReader();
                horasocupadas.Clear();
                
                //cargamos un list con los campos necesarios de los horarios ocupados o reservados para el dia seleccionado en el datepicker
                while (lectoradap.Read())
                {
                    horasocupadas.Add(new horariosocupados { IdHorario = Convert.ToInt32(lectoradap.GetString(0)), Horario = lectoradap.GetString(1), Duracion = lectoradap.GetString(2), Mododulos = Convert.ToInt32(lectoradap.GetString(3)) });
                }

                /*cboxhora.ItemsSource = horasocupadas;
                cboxhora.DisplayMemberPath = "Horario";
                cboxhora.SelectedValuePath = "IdHorario";*/

                conexion.Clone();

            }
            catch (Exception error)
            {

                MessageBox.Show("No se pudo operar sobre la BD, Error: " + error.Message);
            }

            if (horasocupadas.Count()>=1)
            {
                //llenamos una lista de strings con los horarios ocupados
                horariosocup.Clear();
                for (int i = 0; i < horasocupadas.Count(); i++)
                {
                    
                    horariosocup.Add(horasocupadas[i].Horario);
                    if (horasocupadas[i].Mododulos > 1)
                    {
                        //Si la sesion dura mas de 30 minutos, se agregan a las lista de horarios ocupados los abarcados por la duracion de la sesion
                        DateTime pivothorario = DateTime.ParseExact(horasocupadas[i].Horario, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                        for (int j=2; j<= horasocupadas[i].Mododulos; j++)
                        {
                            pivothorario = pivothorario.AddHours(0.5);
                            horariosocup.Add(pivothorario.ToString("HH:mm"));
                        }
                    }
                    
                    
                }

                //quitamos los horarios ocupados de la lista de horarios disponibles
                foreach (string item in horariosocup) horariosdisp.Remove(item);
                
            }


            cboxhora.Items.Clear();
            
            
            cboxhora.ItemsSource = horariosdisp;
        }
    }
}
