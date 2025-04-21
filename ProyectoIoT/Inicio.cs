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
                gaugeComida = (AngularGauge)elementHost2.Child;
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

        //Cantidad por fecha dispensada
        private void MonthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            string fechaSeleccionada = e.Start.ToString("yyyy-MM-dd"); // formato compatible con MySQL
            string query = "SELECT cantidadc FROM cantidad_consumida WHERE fecha = @fecha";

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
                            chart1.Series[0].Points.AddXY(i++, row["cantidadc"]);
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
            string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd"); // formato estándar para MySQL

            string query = @"
           SELECT 
           a.nombre AS Caballo,
           MAX(cc.hora) AS Hora,
           CONCAT(SUM(cc.cantidadc), ' kg') AS 'Cantidad dispensada',
           CASE 
           WHEN d.id_dieta IS NULL THEN '❓ Sin dieta asignada'
           WHEN SUM(cc.cantidadc) >= (COALESCE(d.alfap,0) + COALESCE(d.alimentobal,0) + COALESCE(d.pelet,0)) THEN '✅ Dieta completada hoy'
           ELSE '⚠️ Dieta incompleta'
           END AS Estado
           FROM animales a
           LEFT JOIN cantidad_consumida cc ON cc.id_animal = a.id_animal AND cc.fecha = @fechaHoy
           LEFT JOIN dieta d ON d.id_animal = a.id_animal
           GROUP BY a.id_animal
           ORDER BY MAX(cc.hora) DESC
           LIMIT 4;
           ";

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

            string fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

            // 1. Animales no alimentados desde ayer
            string alertaNoAlimentado = @"
           SELECT a.nombre 
           FROM animales a 
           LEFT JOIN cantidad_consumida cc ON a.id_animal = cc.id_animal
           GROUP BY a.id_animal
           HAVING DATEDIFF(CURDATE(), MAX(STR_TO_DATE(cc.fecha, '%Y-%m-%d'))) >= 1";

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

            // 2. Intentos bloqueados (2 o más en el día)
            string alertaBloqueo = @"
           SELECT 
           a.nombre, 
           ib.motivo, 
           COUNT(*) AS intentos
           FROM intentos_bloqueados ib
           JOIN animales a ON a.id_animal = ib.id_animal
           WHERE STR_TO_DATE(ib.fecha, '%Y-%m-%d') = CURDATE()
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

            // 3. Dieta incompleta (< 100%)
            string alertaDieta = @"
           SELECT 
           a.nombre,
           SUM(cc.cantidadc) AS total_consumido,
           COALESCE(SUM(d.alfap + d.alimentobal + d.pelet), 0) AS esperado,
           ROUND((SUM(cc.cantidadc) / (SUM(d.alfap + d.alimentobal + d.pelet))) * 100, 1) AS porcentaje
           FROM animales a
           LEFT JOIN cantidad_consumida cc ON cc.id_animal = a.id_animal AND cc.fecha = @fecha
           LEFT JOIN dieta d ON d.id_animal = a.id_animal
           GROUP BY a.id_animal
           HAVING porcentaje < 100";

            using (var conn = conectar.conex())
            {
                using (var cmd = new MySqlCommand(alertaDieta, conn))
                {
                    cmd.Parameters.AddWithValue("@fecha", fechaHoy);

                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombre = reader.GetString(0);
                            double porcentaje = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
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

            // 4. Inventario crítico
            string alertaInventario = @"
           SELECT 
           id, 
           dispensador
           FROM inventario
           WHERE dispensador <= 25";

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
                            int id = reader.GetInt32(0);
                            double porcentaje = reader.GetDouble(1);

                            string mensaje;
                            string gravedad;

                            if (porcentaje == 0)
                            {
                                mensaje = $"🟥 Comedero #{id} sin alimento disponible.";
                                gravedad = "🟥 Crítico";
                            }
                            else if (porcentaje < 10)
                            {
                                mensaje = $"🔴 Comedero #{id} con nivel crítico ({porcentaje}%).";
                                gravedad = "🔴 Crítico";
                            }
                            else
                            {
                                mensaje = $"🟠 Comedero #{id} con nivel bajo ({porcentaje}%).";
                                gravedad = "🟠 Medio";
                            }

                            tablaAlertas.Rows.Add(mensaje, gravedad);
                        }
                    }
                }
            }

            // Mostrar las alertas
            dataGridView3.DataSource = tablaAlertas;
            dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView3.Columns[1].Width = 100;

            // Colorear filas según gravedad
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                string gravedad = row.Cells["Gravedad"].Value.ToString();

                if (gravedad.Contains("🟥") || gravedad.Contains("🔴"))
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                else if (gravedad.Contains("🟠"))
                    row.DefaultCellStyle.BackColor = Color.LemonChiffon;
                else if (gravedad.Contains("🟡"))
                    row.DefaultCellStyle.BackColor = Color.Honeydew;
                else if (gravedad.Contains("⚠️"))
                    row.DefaultCellStyle.BackColor = Color.LightCyan;
            }
        }

        private void IniciarTemporizadorAlertas()
        {
            refrescoAlertas = new Timer();
            refrescoAlertas.Interval = 10000; // 10 segundos
            refrescoAlertas.Tick += (s, e) => CargarResumenAlertas();
            refrescoAlertas.Start();
        }


    }
}

