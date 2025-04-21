using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using UIDC;
using MySql.Data.MySqlClient;
using LiveCharts.Wpf;

namespace ProyectoIoT
{
    public partial class Inicio : Form
    {
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;
        private IconPictureBox iconCurrentChildForm;
        private SerialPort serialPort;
        private string tarjetaId;
        private float valorAgua;
        private float valorComida;
        private Timer refrescoFichaRapida;
        private Timer refrescoAlertas;
        private Timer refrescoContadorAnimales;

        private AngularGauge gaugeComida;

        public Inicio()
        {
            InitializeComponent();

            panelMenu.Dock = DockStyle.Left;
            panelMenu.Width = 200;
            panelMenu.BackColor = Color.MidnightBlue;

            panelDesktop.Dock = DockStyle.Fill;

            iconCurrentChildForm = new IconPictureBox();
            iconCurrentChildForm.IconChar = IconChar.Home;
            iconCurrentChildForm.IconColor = Color.Gainsboro;
            iconCurrentChildForm.Size = new Size(32, 32);
            iconCurrentChildForm.Location = new Point(10, 10);
            panelDesktop.Controls.Add(iconCurrentChildForm);

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);

            this.Load += (s, e) => {
                CargarCantidadAnimales();
                IniciarTemporizadorCantidadAnimales();
                InicializarSerial();
            };
        }

        private void InicializarSerial()
        {
            serialPort = new SerialPort("COM3", 9600);
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string mensaje = serialPort.ReadLine().Trim();

                // --- Caso 1: data: xx
                if (mensaje.StartsWith("data:"))
                {
                    string datosStr = mensaje.Substring(5).Trim();
                    string[] partes = datosStr.Split(' ');

                    if (partes.Length >= 1)
                    {
                        if (int.TryParse(partes[0], out int primerValor))
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                GuardarEnBaseDeDatos(primerValor);
                            }));
                        }
                    }
                }

                // --- Caso 2: peso: xx.x rfid: XXXXXXXX
                else if (mensaje.StartsWith("peso:"))
                {
                    string[] partes = mensaje.Split(new[] { "peso:", "rfid:" }, StringSplitOptions.RemoveEmptyEntries);

                    if (partes.Length == 2)
                    {
                        string pesoStr = partes[0].Trim();
                        string rfidStr = partes[1].Trim();

                        if (float.TryParse(pesoStr, out float peso))
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                GuardarPesoBDD(peso, rfidStr);
                            }));
                        }
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error en la lectura del puerto serial: " + error.Message);
            }
        }

        private void GuardarPesoBDD(float peso, string rfid)
        {
            try
            {
                // Paso 1: Buscar id_animal desde la tabla 'animales'
                string queryBuscar = "SELECT id_animal FROM animales WHERE rfid_tag = @rfid LIMIT 1";
                MySqlCommand cmdBuscar = new MySqlCommand(queryBuscar, conectar.conex());
                cmdBuscar.Parameters.AddWithValue("@rfid", rfid);

                object resultado = cmdBuscar.ExecuteScalar();

                if (resultado != null)
                {
                    int id_animal = Convert.ToInt32(resultado);

                    // Paso 2: Obtener fecha y hora actual
                    string fecha = DateTime.Now.ToString("dd/MM/yyyy");
                    string hora = DateTime.Now.ToString("HH:mm:ss");

                    // Paso 3: Insertar en 'cantidad_consumida'
                    string queryInsertar = @"INSERT INTO cantidad_consumida (id_animal, cantidac, fecha, hora) 
                                     VALUES (@id_animal, @peso, @fecha, @hora)";
                    MySqlCommand cmdInsertar = new MySqlCommand(queryInsertar, conectar.conex());
                    cmdInsertar.Parameters.AddWithValue("@id_animal", id_animal);
                    cmdInsertar.Parameters.AddWithValue("@peso", peso);
                    cmdInsertar.Parameters.AddWithValue("@fecha", fecha);
                    cmdInsertar.Parameters.AddWithValue("@hora", hora);

                    cmdInsertar.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("No se encontró un animal con el RFID: " + rfid);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al guardar peso: " + ex.Message);
            }
        }



        private void GuardarEnBaseDeDatos(int valor)
        {
            try
            {
                string query = "INSERT INTO inventario (dispensador) VALUES ('" + valor + "')";
                MySqlCommand comando = new MySqlCommand(query, conectar.conex());
                comando.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al insertar datos: " + ex.Message);
            }
        }

        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(192, 18, 28, 62);
        }

        private void CargarCantidadAnimales()
        {
            string query = "SELECT COUNT(*) FROM animales";

            try
            {
                using (MySqlConnection conn = conectar.conex())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        int cantidad = Convert.ToInt32(cmd.ExecuteScalar());
                        label8.Text = $"Total: {cantidad}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al contar animales: " + ex.Message);
            }
        }

        private void IniciarTemporizadorCantidadAnimales()
        {
            refrescoContadorAnimales = new Timer();
            refrescoContadorAnimales.Interval = 10000;
            refrescoContadorAnimales.Tick += (s, e) => CargarCantidadAnimales();
            refrescoContadorAnimales.Start();
        }


        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                currentBtn = (IconButton)senderBtn;

                currentBtn.ForeColor = Color.FromArgb(255, 194, 14);
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = Color.FromArgb(255, 194, 14);
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleCenter;

                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();

                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                iconCurrentChildForm.IconColor = color;
            }
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Gainsboro;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void bt_dashboard_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
        }

        private void bt_animales_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Informacion());
        }

        private void bt_dietas_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Dieta());
        }

        private void bt_alertas_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Alertas());
        }

        private void panelMenu_Paint(object sender, PaintEventArgs e) { }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Informacion());
        }
        private void bt_historial_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Historial());
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "¿Estás seguro de activar el dispensador manualmente?",
        "Confirmación requerida",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    );

            if (result == DialogResult.Yes)
            {
                Dispensador form = new Dispensador();
                form.Show(); // Mostrar la ventana flotante solo si confirma
            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Alertas());
        }

       
        
        
    }
}
