using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIoT
{
    public partial class Dispensador : Form
    {
        public Dispensador()
        {
            InitializeComponent();
            this.Text = "Modo Dispensador Manual";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.TopMost = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void bt_dispensar_Click(object sender, EventArgs e)
        {

        }
    }
}
