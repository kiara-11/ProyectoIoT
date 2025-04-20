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
    public partial class FormEditarDieta : Form
    {
        public FormEditarDieta()
        {
            InitializeComponent();
        }
        public string Comida1 { get; set; }
        public string Comida2 { get; set; }
        public string Comida3 { get; set; }
        public string Agua { get; set; }

        public FormEditarDieta(string comida1, string comida2, string comida3, string agua)
        {
            InitializeComponent();

            txtComida1.Text = comida1;
            txtComida2.Text = comida2;
            txtComida3.Text = comida3;
            txtAgua.Text = agua;
        }


        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            Comida1 = txtComida1.Text;
            Comida2 = txtComida2.Text;
            Comida3 = txtComida3.Text;
            Agua = txtAgua.Text;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}