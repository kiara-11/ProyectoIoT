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

        public Inicio()
        {
            InitializeComponent();

            panelMenu.Dock = DockStyle.Left;
            panelMenu.Width = 200;
            panelMenu.BackColor = Color.MidnightBlue;

            panelDesktop.Dock = DockStyle.Fill;
            panelDesktop.BackColor = Color.WhiteSmoke;

            iconCurrentChildForm = new IconPictureBox();
            iconCurrentChildForm.IconChar = IconChar.Home;
            iconCurrentChildForm.IconColor = Color.Gainsboro;
            iconCurrentChildForm.Size = new Size(32, 32);
            iconCurrentChildForm.Location = new Point(10, 10);
            panelDesktop.Controls.Add(iconCurrentChildForm);

            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);

            monthCalendar1.DateChanged += MonthCalendar1_DateChanged;
        }

        private struct RGBColors
        {
            public static Color color1 = ColorTranslator.FromHtml("#FFC20E");
        }

        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(255, 248, 212);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;

                leftBorderBtn.BackColor = color;
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
                currentBtn.BackColor = Color.FromArgb(31, 30, 68);
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

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Informacion());
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            OpenChildForm(new Alertas());
        }

        private void MonthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            string fechaSeleccionada = e.Start.ToString("dd/MM/yyyy");
            string query = "SELECT cantidad FROM alimentacion WHERE fecha = @fecha";

            try
            {
                using (MySqlConnection conn = conectar.conex())
                {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar la base de datos: " + ex.Message);
            }
        }
    }
}