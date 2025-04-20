using MySql.Data.MySqlClient;
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
    public partial class TarjetaAnimal : UserControl
    {
        private string connectionString = "server=localhost;user=root;password=;database=iotbdd;";
        public TarjetaAnimal()
        {
            InitializeComponent();
        }

        public string Nombre
        {
            get => lblNombre.Text;
            set => lblNombre.Text =value;
        }

        public string ID
        {
            get => lblID.Text;
            set => lblID.Text =value;
        }

        public string Comida1
        {
            get => lblComida1.Text;
            set => lblComida1.Text = value + " gr.";
        }

        public string Comida2
        {
            get => lblComida2.Text;
            set => lblComida2.Text = value + " gr.";
        }

        public string Comida3
        {
            get => lblComida3.Text;
            set => lblComida3.Text = value + " gr.";
        }

        public string Agua
        {
            get => lblAgua.Text;
            set => lblAgua.Text = value + " ml.";
        }

        private void ActualizarDietaEnBD(string idAnimal, string comida1, string comida2, string comida3, string agua)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"UPDATE dieta_comida d 
                             JOIN animales a ON a.id_dieta = d.id_dieta
                             SET d.alimento_1 = @c1, d.alimento_2 = @c2, d.alimento_3 = @c3, d.agua = @agua
                             WHERE a.id_animal = @id";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@c1", comida1);
                    cmd.Parameters.AddWithValue("@c2", comida2);
                    cmd.Parameters.AddWithValue("@c3", comida3);
                    cmd.Parameters.AddWithValue("@agua", agua);
                    cmd.Parameters.AddWithValue("@id", idAnimal);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error actualizando dieta: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Elimina los "gr." y "ml." para no pasarlos al editor
            string comida1 = lblComida1.Text.Replace(" gr.", "");
            string comida2 = lblComida2.Text.Replace(" gr.", "");
            string comida3 = lblComida3.Text.Replace(" gr.", "");
            string agua = lblAgua.Text.Replace(" ml.", "");

            FormEditarDieta editor = new FormEditarDieta(comida1, comida2, comida3, agua);

            if (editor.ShowDialog() == DialogResult.OK)
            {
                // Actualiza visualmente
                Comida1 = editor.Comida1;
                Comida2 = editor.Comida2;
                Comida3 = editor.Comida3;
                Agua = editor.Agua;

                // Aquí va el update en BD
                ActualizarDietaEnBD(ID, editor.Comida1, editor.Comida2, editor.Comida3, editor.Agua);
            }
        }


        private void TarjetaAnimal_Load(object sender, EventArgs e)
        {

        }
    }
}
