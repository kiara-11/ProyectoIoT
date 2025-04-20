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
                    txtRFID.Text = reader["rfid_tag"].ToString(); // NUEVO
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
                string.IsNullOrWhiteSpace(txtRFID.Text) ||
                cbGenero.SelectedItem == null)
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtEdad.Text, out int edad))
            {
                MessageBox.Show("Edad inválida. Debe ser un número entero.");
                return;
            }

            if (!decimal.TryParse(txtPeso.Text, out decimal peso))
            {
                MessageBox.Show("Peso inválido. Debe ser un número decimal.");
                return;
            }

            using (MySqlConnection conexion = conectar.conex())
            {
                // VALIDAR RFID duplicado (excluyendo el actual animal)
                string checkQuery = "SELECT COUNT(*) FROM animales WHERE rfid_tag = @rfid AND id_animal != @id";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conexion);
                checkCmd.Parameters.AddWithValue("@rfid", txtRFID.Text);
                checkCmd.Parameters.AddWithValue("@id", idAnimal);
                int duplicado = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (duplicado > 0)
                {
                    MessageBox.Show("El código RFID ya está asignado a otro animal. Ingrese uno diferente.");
                    return;
                }

                // HACER UPDATE
                string query = "UPDATE animales SET nombre = @nombre, distintivo = @distintivo, raza = @raza, genero = @genero, edad = @edad, peso = @peso, rfid_tag = @rfid_tag, id_usuario = @id_usuario " +
                               "WHERE id_animal = @id";

                MySqlCommand cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@distintivo", txtDistintivo.Text);
                cmd.Parameters.AddWithValue("@raza", txtRaza.Text);
                cmd.Parameters.AddWithValue("@genero", cbGenero.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@edad", edad);
                cmd.Parameters.AddWithValue("@peso", peso);
                cmd.Parameters.AddWithValue("@rfid_tag", txtRFID.Text);
                cmd.Parameters.AddWithValue("@id_usuario", Sesion.IdUsuario);
                cmd.Parameters.AddWithValue("@id", idAnimal);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Animal actualizado correctamente.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el animal: " + ex.Message);
                }
            }
        }

        private void FormEditarAnimal_Load(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
