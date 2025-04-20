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
    public partial class FormAgregarAnimal : Form
    {
        public FormAgregarAnimal()
        {
            InitializeComponent();
            
            cbGenero.Items.AddRange(new string[] { "Macho", "Hembra" });

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validar RFID no vacío
            if (string.IsNullOrWhiteSpace(txtRFID.Text))
            {
                MessageBox.Show("Por favor, ingrese un código RFID.");
                return;
            }

            using (MySqlConnection conexion = conectar.conex())
            {
                // 1. Verificar si el RFID ya existe
                string verificarQuery = "SELECT COUNT(*) FROM animales WHERE rfid_tag = @rfid";
                MySqlCommand verificarCmd = new MySqlCommand(verificarQuery, conexion);
                verificarCmd.Parameters.AddWithValue("@rfid", txtRFID.Text);
                int existe = Convert.ToInt32(verificarCmd.ExecuteScalar());

                if (existe > 0)
                {
                    MessageBox.Show("Este código RFID ya está registrado. Ingrese uno diferente.", "RFID duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Insertar si no existe
                string query = "INSERT INTO animales (nombre, distintivo, raza, genero, edad, peso, rfid_tag, id_usuario) " +
                               "VALUES (@nombre, @distintivo, @raza, @genero, @edad, @peso, @rfid_tag, @id_usuario)";
                MySqlCommand cmd = new MySqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@distintivo", txtDistintivo.Text);
                cmd.Parameters.AddWithValue("@raza", txtRaza.Text);
                cmd.Parameters.AddWithValue("@genero", cbGenero.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@edad", Convert.ToInt32(txtEdad.Text));
                cmd.Parameters.AddWithValue("@peso", Convert.ToDecimal(txtPeso.Text));
                cmd.Parameters.AddWithValue("@rfid_tag", txtRFID.Text);
                cmd.Parameters.AddWithValue("@id_usuario", Sesion.IdUsuario);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Animal agregado correctamente.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar: " + ex.Message);
                }
            }

        }

        private void FormAgregarAnimal_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void txtRFID_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbGenero_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
