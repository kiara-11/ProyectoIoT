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
    public partial class Informacion : Form
    {
        public Informacion()
        {
            InitializeComponent();


            CargarAnimales();
        }
        private void CargarAnimales()
        {
            using (MySqlConnection conexion = conectar.conex())
            {
                string query = "SELECT id_animal, nombre, distintivo, raza, genero, edad, peso, rfid_tag FROM animales";
                MySqlDataAdapter adaptador = new MySqlDataAdapter(query, conexion);
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);
                dgvAnimales.DataSource = tabla;
                EstilizarDataGridView();
            }
            // Verificar que existan columnas antes de configurarlas
            if (dgvAnimales.Columns.Count > 0)
            {
                dgvAnimales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvAnimales.ColumnHeadersVisible = true;
                dgvAnimales.EnableHeadersVisualStyles = false;
                dgvAnimales.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                dgvAnimales.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvAnimales.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                dgvAnimales.Columns["id_animal"].Visible = false;

                dgvAnimales.Columns["nombre"].HeaderText = "Nombre";
                dgvAnimales.Columns["distintivo"].HeaderText = "Distintivo";
                dgvAnimales.Columns["raza"].HeaderText = "Raza";
                dgvAnimales.Columns["genero"].HeaderText = "Género";
                dgvAnimales.Columns["edad"].HeaderText = "Edad";
                dgvAnimales.Columns["peso"].HeaderText = "Peso";
                dgvAnimales.Columns["rfid_tag"].HeaderText = "RFID";
            }

            // Añadir botón de editar solo si no existe ya
            if (!dgvAnimales.Columns.Contains("Editar"))
            {
                DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();
                btnEditar.HeaderText = "Editar";
                btnEditar.Text = "Editar";
                btnEditar.Name = "Editar";
                btnEditar.UseColumnTextForButtonValue = true;
                dgvAnimales.Columns.Add(btnEditar);
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void EstilizarDataGridView()
        {
            dgvAnimales.EnableHeadersVisualStyles = false;
            dgvAnimales.BackgroundColor = Color.White;
            dgvAnimales.BorderStyle = BorderStyle.None;
            dgvAnimales.GridColor = Color.LightGray;
            dgvAnimales.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Encabezado
            dgvAnimales.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204); // Azul profesional
            dgvAnimales.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAnimales.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvAnimales.ColumnHeadersHeight = 35;

            // Celdas normales
            dgvAnimales.DefaultCellStyle.BackColor = Color.White;
            dgvAnimales.DefaultCellStyle.ForeColor = Color.Black;
            dgvAnimales.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgvAnimales.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvAnimales.DefaultCellStyle.Font = new Font("Segoe UI", 10);

            // Filas alternas (zebra)
            dgvAnimales.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;

            // Otras configuraciones
            dgvAnimales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAnimales.RowTemplate.Height = 30;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && dgvAnimales.Columns[e.ColumnIndex].Name == "Editar")
            {
                int idAnimal = Convert.ToInt32(dgvAnimales.Rows[e.RowIndex].Cells["id_animal"].Value);

                FormEditarAnimal form = new FormEditarAnimal(idAnimal);
                form.FormClosed += (s, args) => CargarAnimales();
                form.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAgregarAnimal agregar = new FormAgregarAnimal();
            agregar.FormClosed += (s, args) => CargarAnimales();
            agregar.ShowDialog();
        }

        private void Informacion_Load(object sender, EventArgs e)
        {

        }
    }
}
