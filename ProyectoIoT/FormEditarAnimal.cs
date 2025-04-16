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
    public partial class FormEditarAnimal : Form
    {
        private int idAnimal;
        public FormEditarAnimal(int id)
        {
            InitializeComponent();
            cbGenero.Items.AddRange(new string[] { "Macho", "Hembra" });
            idAnimal = id;
            CargarDatos();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void CargarDatos()
        {
            using (MySqlConnection conexion = conectar.conex())
            {
                string query = "SELECT * FROM animales WHERE id_animal = @id";
                MySqlCommand cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@id", idAnimal);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtNombre.Text = reader["nombre"].ToString();
                    txtDistintivo.Text = reader["distintivo"].ToString();
                    txtRaza.Text = reader["raza"].ToString();
                    cbGenero.SelectedItem = reader["genero"].ToString();
                    txtEdad.Text = reader["edad"].ToString();
                    txtPeso.Text = reader["peso"].ToString();
                }
                reader.Close();
            }
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtDistintivo.Text) ||
                string.IsNullOrWhiteSpace(txtRaza.Text) ||
                string.IsNullOrWhiteSpace(txtEdad.Text) ||
                string.IsNullOrWhiteSpace(txtPeso.Text) ||
                cbGenero.SelectedItem == null)
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar tipo de datos
            if (!int.TryParse(txtEdad.Text, out int edad))
            {
                MessageBox.Show("Edad inválida. Debe ser un número entero.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtPeso.Text, out decimal peso))
            {
                MessageBox.Show("Peso inválido. Debe ser un número decimal.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MySqlConnection conexion = conectar.conex())
            {
                string query = "INSERT INTO animales (nombre, distintivo, raza, genero, edad, peso, id_usuario) " +
                               "VALUES (@nombre, @distintivo, @raza, @genero, @edad, @peso, @id_usuario)";

                MySqlCommand cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@distintivo", txtDistintivo.Text);
                cmd.Parameters.AddWithValue("@raza", txtRaza.Text);
                cmd.Parameters.AddWithValue("@genero", cbGenero.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@edad", edad);
                cmd.Parameters.AddWithValue("@peso", peso);
                cmd.Parameters.AddWithValue("@id_usuario", Sesion.IdUsuario);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Animal agregado correctamente.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el animal: " + ex.Message);
                }
            }
        }
    }
}
