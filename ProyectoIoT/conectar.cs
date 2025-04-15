using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProyectoIoT
{
    class conectar
    {
        public static MySqlConnection conex()
        {
            string servidor = "server=localhost; database=registro11; Uid=root; pwd=;";
            MySqlConnection conexionBD = new MySqlConnection(servidor);

            try
            {
                conexionBD.Open();
                return conexionBD;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error de conexión: " + e.Message);
                return null;
            }
        }
    }
}