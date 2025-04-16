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
                            // Crear panel o tarjeta individual para cada animal
                            Panel tarjeta = new Panel();
                            tarjeta.Size = new Size(400, 150);
                            tarjeta.BorderStyle = BorderStyle.FixedSingle;
                            tarjeta.BackColor = Color.LightGreen;
                            tarjeta.Margin = new Padding(10);

                            // Nombre
                            Label lblNombre = new Label();
                            lblNombre.Text = "Nombre: " + reader.GetString("nombre");
                            lblNombre.Location = new Point(10, 10);
                            lblNombre.AutoSize = true;

                            // ID
                            Label lblID = new Label();
                            lblID.Text = "ID: " + reader.GetInt32("id_animal").ToString();
                            lblID.Location = new Point(10, 30);
                            lblID.AutoSize = true;

                            // Detalles dieta
                            Label lblComida1 = new Label();
                            lblComida1.Text = "Comida 1: " + reader.GetString("alimento_1") + " gr.";
                            lblComida1.Location = new Point(10, 60);
                            lblComida1.AutoSize = true;

                            Label lblComida2 = new Label();
                            lblComida2.Text = "Comida 2: " + reader.GetString("alimento_2") + " gr.";
                            lblComida2.Location = new Point(10, 80);
                            lblComida2.AutoSize = true;

                            Label lblComida3 = new Label();
                            lblComida3.Text = "Comida 3: " + reader.GetString("alimento_3") + " gr.";
                            lblComida3.Location = new Point(10, 100);
                            lblComida3.AutoSize = true;

                            Label lblAgua = new Label();
                            lblAgua.Text = "Agua: " + reader.GetString("agua") + " ml.";
                            lblAgua.Location = new Point(10, 120);
                            lblAgua.AutoSize = true;

                            // Agregar controles al panel
                            tarjeta.Controls.Add(lblNombre);
                            tarjeta.Controls.Add(lblID);
                            tarjeta.Controls.Add(lblComida1);
                            tarjeta.Controls.Add(lblComida2);
                            tarjeta.Controls.Add(lblComida3);
                            tarjeta.Controls.Add(lblAgua);

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


        private void button1_Click(object sender, EventArgs e)
        {
            EditarDieta editar = new EditarDieta();
            editar.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanelTarjetas_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}