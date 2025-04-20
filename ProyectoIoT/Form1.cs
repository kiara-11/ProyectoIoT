using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProyectoIoT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
            txtuser.Text = "Usuario";
            txtuser.ForeColor = Color.Green;

            txtpass.Text = "Contraseña";
            txtpass.ForeColor = Color.Green;
            txtpass.UseSystemPasswordChar = false;

            
            txtuser.Enter += txtuser_Enter;
            txtuser.Leave += txtuser_Leave;
            txtpass.Enter += txtpass_Enter;
            txtpass.Leave += txtpass_Leave;
        }

        private void boton_acceder_Click(object sender, EventArgs e)
        {
            string usuario = txtuser.Text;
            string contra = txtpass.Text;

            if (usuario == "Usuario" || contra == "Contraseña")
            {
                MessageBox.Show("Por favor, ingrese sus credenciales.", "Campos vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MySqlConnection conexionBD = conectar.conex();

            try
            {
                MySqlCommand comando = new MySqlCommand("SELECT * FROM usuarios WHERE usuario = @usuario AND password = @contra", conexionBD);
                comando.Parameters.AddWithValue("@usuario", usuario);
                comando.Parameters.AddWithValue("@contra", contra);

                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    // Guardamos los datos del usuario actual
                    Sesion.IdUsuario = Convert.ToInt32(reader["id_usuario"]);
                    Sesion.NombreUsuario = reader["usuario"].ToString();

                    reader.Close();

                    // Abrimos el dashboard
                    Inicio formularioInicio = new Inicio();
                    formularioInicio.FormClosed += (s, args) => this.Show();
                    formularioInicio.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message);
            }
            finally
            {
                conexionBD.Close();
            }
        }

        private void txtuser_Enter(object sender, EventArgs e)
        {
            if (txtuser.Text == "Usuario")
            {
                txtuser.Text = "";
                txtuser.ForeColor = Color.Green;
            }
        }

        private void txtuser_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtuser.Text))
            {
                txtuser.Text = "Usuario";
                txtuser.ForeColor = Color.Green;
            }
        }

        private void txtpass_Enter(object sender, EventArgs e)
        {
            if (txtpass.Text == "Contraseña")
            {
                txtpass.Text = "";
                txtpass.ForeColor = Color.Green;
                txtpass.UseSystemPasswordChar = true;
            }
        }

        private void txtpass_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtpass.Text))
            {
                txtpass.Text = "Contraseña";
                txtpass.ForeColor = Color.Green;
                txtpass.UseSystemPasswordChar = false;
            }
        }

<<<<<<< HEAD
=======
        private void Form1_Load(object sender, EventArgs e)
        {

        }

>>>>>>> origin/reds
        private void txtuser_TextChanged(object sender, EventArgs e)
        {

        }
    }
}