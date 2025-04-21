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

            monthCalendar1.DateChanged += MonthCalendar1_DateChanged;
            this.Load += (s, e) => {
                CargarFichaRapida();
                IniciarTemporizadorFichaRapida();
                CargarResumenAlertas();
                IniciarTemporizadorAlertas();
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
            string data;
            lock (serialPort)
            {
                data = serialPort.ReadLine();
            }
            this.BeginInvoke(new MethodInvoker(() => ProcesarSerial(data)));
        }

        // Mostrar en DataGridView1 tarjetas activas con nombre de animal
        private Dictionary<string, string> tarjetasActivas = new Dictionary<string, string>();

        private void ProcesarSerial(string data)
        {
            if (data.StartsWith("data:"))
            {
                string[] partes = data.Replace("data:", "").Trim().Split(' ');
                if (partes.Length == 3 && float.TryParse(partes[0], out float porcentaje) && float.TryParse(partes[1], out float consumido) && int.TryParse(partes[2], out int idComida))
                {
                    string fecha = DateTime.Now.ToString("dd/MM/yyyy");
                    string hora = DateTime.Now.ToString("HH:mm");

                    using (var conn = conectar.conex())
                    {
                        

                        if (!string.IsNullOrEmpty(tarjetaId))
                        {
                            string getAnimal = "SELECT id_animal FROM animales WHERE rfid_tag = @rfid";
                            using (var cmdAnimal = new MySqlCommand(getAnimal, conn))
                            {
                                cmdAnimal.Parameters.AddWithValue("@rfid", tarjetaId);
                                var result = cmdAnimal.ExecuteScalar();
                                if (result != null)
                                {
                                    int idAnimal = Convert.ToInt32(result);
                                    string insertAlim = "INSERT INTO alimentacion (id_animal, id_comida, cantidad, fecha, hora) VALUES (@id_animal, @id_comida, @cantidad, @fecha, @hora)";
                                    using (var cmdInsert = new MySqlCommand(insertAlim, conn))
                                    {
                                        cmdInsert.Parameters.AddWithValue("@id_animal", idAnimal);
                                        cmdInsert.Parameters.AddWithValue("@id_comida", idComida);
                                        cmdInsert.Parameters.AddWithValue("@cantidad", consumido);
                                        cmdInsert.Parameters.AddWithValue("@fecha", fecha);
                                        cmdInsert.Parameters.AddWithValue("@hora", hora);
                                        cmdInsert.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        string updateInv = "UPDATE inventario SET cantidad_actual = @cant, fecha_actualizacion = @fecha WHERE id_comida = @id_comida";
                        using (var cmdInv = new MySqlCommand(updateInv, conn))
                        {
                            cmdInv.Parameters.AddWithValue("@cant", porcentaje);
                            cmdInv.Parameters.AddWithValue("@fecha", fecha);
                            cmdInv.Parameters.AddWithValue("@id_comida", idComida);
                            cmdInv.ExecuteNonQuery();
                        }
                    }

                    if (gaugeComida != null)
                    {
                        gaugeComida.Value = porcentaje;
                        gaugeComida.FromValue = 0;
                        gaugeComida.ToValue = 100;
                        gaugeComida.TicksStep = 10;
                    }
                }
            }
            else if (data.StartsWith("rfid:"))
            {
                string estado = data.Contains("inactivo") ? "inactivo" : "activo";
                string tag = data.Replace("rfid:", "").Replace("activo", "").Replace("inactivo", "").Trim();

                if (estado == "activo")
                {
                    tarjetaId = tag;
                    using (var conn = conectar.conex())
                    {
                        
                        string query = "SELECT nombre FROM animales WHERE rfid_tag = @tag";
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@tag", tag);
                            var nombre = cmd.ExecuteScalar()?.ToString();
                            if (!string.IsNullOrEmpty(nombre))
                                tarjetasActivas[tag] = nombre;
                        }
                    }

                    ActualizarDataGridView1();
                }
            }
        }

        private void ActualizarDataGridView1()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("RFID", typeof(string));
            tabla.Columns.Add("Nombre", typeof(string));

            foreach (var kvp in tarjetasActivas)
            {
                tabla.Rows.Add(kvp.Key, kvp.Value);
            }

            dataGridView1.DataSource = tabla;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].Width = 200;
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
            int idAnimalSeleccionado = 1; // Ejemplo
            OpenChildForm(new Dieta(idAnimalSeleccionado));
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

        //Cantidad por fecha dispensada
        private void MonthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            string fechaSeleccionada = e.Start.ToString("dd/MM/yyyy");
            string query = "SELECT cantidad FROM alimentacion WHERE fecha = @fecha";

            try
            {
                using (MySqlConnection conn = conectar.conex())
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha", fechaSeleccionada);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        chart1.Series[0].Points.Clear();
                        int i = 1;
                        foreach (DataRow row in dt.Rows)
                        {
                            chart1.Series[0].Points.AddXY(i++, row["cantidad"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar la base de datos: " + ex.Message);
            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Alertas());
        }

        //Ficha de estado de alimentacion por dia y hora
        private void CargarFichaRapida()
        {
            string fechaHoy = DateTime.Now.ToString("dd/MM/yyyy"); 

            string query = @"
        SELECT 
            a.nombre AS Caballo,
            al.hora AS Hora,
            CONCAT(al.cantidad, ' kg') AS 'Cantidad dispensada',
            CASE 
                WHEN dc.cantidad_permitida IS NULL THEN '❓ Sin dieta asignada'
                WHEN al.cantidad >= dc.cantidad_permitida THEN '✅ Dieta completada hoy'
                ELSE '⚠️ Dieta incompleta'
            END AS Estado
        FROM alimentacion al
        JOIN animales a ON al.id_animal = a.id_animal
        LEFT JOIN dieta_comida dc 
               ON a.id_dieta = dc.id_dieta AND al.id_comida = dc.id_comida
        WHERE al.fecha = @fechaHoy
        ORDER BY al.hora DESC
        LIMIT 4";

            try
            {
                using (MySqlConnection conn = conectar.conex())
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fechaHoy", fechaHoy);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView2.DataSource = dt;
                        PersonalizarDataGrid(dataGridView2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la ficha rápida: " + ex.Message);
            }
        }

        private void PersonalizarDataGrid(DataGridView grid)
        {
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;

            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        //para consultar cada 10segundos los datos de la bdd
        private void IniciarTemporizadorFichaRapida()
        {
            refrescoFichaRapida = new Timer();
            refrescoFichaRapida.Interval = 10000;
            refrescoFichaRapida.Tick += (s, e) => CargarFichaRapida();
            refrescoFichaRapida.Start();
        }

        private void CargarResumenAlertas()
        {
            DataTable tablaAlertas = new DataTable();
            tablaAlertas.Columns.Add("Alerta", typeof(string));
            tablaAlertas.Columns.Add("Gravedad", typeof(string));

            // 1. Caballos no alimentados desde ayer
            string alertaNoAlimentado = @"
            SELECT nombre 
             FROM animales a 
             LEFT JOIN alimentacion al ON a.id_animal = al.id_animal
             GROUP BY a.id_animal
            HAVING DATEDIFF(CURDATE(), MAX(STR_TO_DATE(al.fecha, '%d/%m/%Y'))) >= 1";

            using (var conn = conectar.conex())
            {
                using (var cmd = new MySqlCommand(alertaNoAlimentado, conn))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tablaAlertas.Rows.Add($"⚠️ El caballo {reader.GetString(0)} no ha sido alimentado desde ayer.", "⚠️ Medio");
                        }
                    }
                }
            }


            string alertaInventario = @"
             SELECT 
             c.nombre, 
             i.cantidad_actual,
             ROUND(i.cantidad_actual, 1) AS porcentaje
             FROM inventario i
             JOIN comidas c ON c.id_comida = i.id_comida
             WHERE STR_TO_DATE(i.fecha_actualizacion, '%d/%m/%Y') = CURDATE()
             AND i.cantidad_actual < 25";

            using (var conn = conectar.conex())
            {
                using (var cmd = new MySqlCommand(alertaInventario, conn))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string comida = reader.GetString(0);
                            double porcentaje = reader.GetDouble(2);
                            string mensaje;

                            if (porcentaje >= 10)
                                mensaje = $"🟠 {comida}: Inventario bajo ({porcentaje}%).";
                            else if (porcentaje > 0)
                                mensaje = $"🔴 {comida}: Inventario crítico ({porcentaje}%).";
                            else
                                mensaje = $"🟥 Sin {comida} disponible en inventario.";

                            tablaAlertas.Rows.Add(mensaje, "🔴 Crítico");
                        }
                    }
                }
            }

            // 3. Intentos bloqueados
            string alertaBloqueo = @"
            SELECT 
            a.nombre, 
            ib.motivo, 
            COUNT(*) AS intentos
            FROM intentos_bloqueados ib
            JOIN animales a ON a.id_animal = ib.id_animal
            WHERE STR_TO_DATE(ib.fecha, '%d/%m/%Y') = CURDATE()
            GROUP BY a.id_animal, ib.motivo
            HAVING intentos >= 2";

            using (var conn = conectar.conex())
            {
                using (var cmd = new MySqlCommand(alertaBloqueo, conn))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombre = reader.GetString(0);
                            string motivo = reader.GetString(1);
                            int intentos = reader.GetInt32(2);

                            tablaAlertas.Rows.Add(
                                $"⚠️ El caballo {nombre} fue bloqueado {intentos} veces hoy. Motivo: {motivo}.",
                                "⚠️ Medio"
                            );
                        }
                    }
                }
            }

            // 4. Consumo menor al 50%
            string alertaDieta = @"
            SELECT 
            a.nombre, 
            SUM(al.cantidad) AS total, 
            SUM(dc.cantidad_permitida) AS esperado,
            ROUND((SUM(al.cantidad) / SUM(dc.cantidad_permitida)) * 100, 1) AS porcentaje
            FROM animales a
            JOIN alimentacion al ON al.id_animal = a.id_animal AND STR_TO_DATE(al.fecha, '%d/%m/%Y') = CURDATE()
            JOIN dieta_comida dc ON a.id_dieta = dc.id_dieta AND al.id_comida = dc.id_comida
            GROUP BY a.id_animal
            HAVING porcentaje < 100";

            using (var conn = conectar.conex())
            {
                using (var cmd = new MySqlCommand(alertaDieta, conn))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombre = reader.GetString(0);
                            double porcentaje = reader.GetDouble(3);
                            string mensaje;
                            string gravedad;

                            if (porcentaje >= 75)
                            {
                                mensaje = $"🟡 El caballo {nombre} consumió el {porcentaje}% de su dieta hoy.";
                                gravedad = "🟡 Leve";
                            }
                            else if (porcentaje >= 50)
                            {
                                mensaje = $"🟠 El caballo {nombre} consumió solo el {porcentaje}% de su dieta hoy.";
                                gravedad = "🟠 Medio";
                            }
                            else
                            {
                                mensaje = $"🔴 El caballo {nombre} consumió solo el {porcentaje}% de su dieta hoy.";
                                gravedad = "🔴 Crítico";
                            }

                            tablaAlertas.Rows.Add(mensaje, gravedad);
                        }
                    }
                }
            }

            dataGridView3.DataSource = tablaAlertas;
            dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView3.Columns[1].Width = 100;
        }

        private void IniciarTemporizadorAlertas()
        {
            refrescoAlertas = new Timer();
            refrescoAlertas.Interval = 10000; // 10 segundos
            refrescoAlertas.Tick += (s, e) => CargarResumenAlertas();
            refrescoAlertas.Start();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Aquí va la lógica al hacer clic en una celda
        }


    }
}
