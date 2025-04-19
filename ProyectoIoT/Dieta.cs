using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace ProyectoIoT
{
    public partial class Dieta : Form
    {
        private int idAnimal;
        private string connectionString = "server=localhost;user=root;password=;database=iotbdd;";

        public Dieta(int id)
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.Dieta_Load);
            idAnimal = id;
        }

        private void Dieta_Load(object sender, EventArgs e)
        {
            CargarDatosAnimalesYDieta();
        }

        private void CargarDatosAnimalesYDieta()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT a.id_animal, a.nombre, d.alimento_1, d.alimento_2, d.alimento_3, d.agua 
                     FROM animales a
                     JOIN dieta_comida d ON a.id_dieta = d.id_dieta";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // 💡 Crear la tarjeta bonita usando el UserControl
                            TarjetaAnimal tarjeta = new TarjetaAnimal();
                            tarjeta.Nombre = reader.GetString("nombre");
                            tarjeta.ID = reader.GetInt32("id_animal").ToString();
                            tarjeta.Comida1 = reader["alimento_1"].ToString();
                            tarjeta.Comida2 = reader["alimento_2"].ToString();
                            tarjeta.Comida3 = reader["alimento_3"].ToString();
                            tarjeta.Agua = reader["agua"].ToString();

                            // Opcional: cargar imagen si tienes una ruta o lógica
                            // tarjeta.Foto = Image.FromFile("ruta/a/la/imagen.jpg");

                            // Agregar tarjeta al flowLayoutPanel
                            flowLayoutPanelTarjetas.Controls.Add(tarjeta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }



     

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanelTarjetas_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}