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
            using (MySqlConnection conexion = conectar.conex())
            {
                string query = "INSERT INTO animales (nombre, distintivo, raza, genero, edad, peso, id_usuario) VALUES (@nombre, @distintivo, @raza, @genero, @edad, @peso, @id_usuario)";
                MySqlCommand cmd = new MySqlCommand(query, conexion);

                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@distintivo", txtDistintivo.Text);
                cmd.Parameters.AddWithValue("@raza", txtRaza.Text);
                cmd.Parameters.AddWithValue("@genero", cbGenero.SelectedItem?.ToString() ?? "");
                cmd.Parameters.AddWithValue("@edad", Convert.ToInt32(txtEdad.Text));
                cmd.Parameters.AddWithValue("@peso", Convert.ToDecimal(txtPeso.Text));
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
    }
}
