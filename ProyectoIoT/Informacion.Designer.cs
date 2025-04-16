namespace ProyectoIoT
{
    partial class Informacion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dgvAnimales = new System.Windows.Forms.DataGridView();
            this.btnAgregarAnimal = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnimales)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(435, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 0;
            // 
            // dgvAnimales
            // 
            this.dgvAnimales.AllowUserToAddRows = false;
            this.dgvAnimales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAnimales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAnimales.Location = new System.Drawing.Point(24, 25);
            this.dgvAnimales.Name = "dgvAnimales";
            this.dgvAnimales.ReadOnly = true;
            this.dgvAnimales.RowHeadersWidth = 51;
            this.dgvAnimales.RowTemplate.Height = 24;
            this.dgvAnimales.Size = new System.Drawing.Size(797, 492);
            this.dgvAnimales.TabIndex = 1;
            this.dgvAnimales.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btnAgregarAnimal
            // 
            this.btnAgregarAnimal.Location = new System.Drawing.Point(843, 13);
            this.btnAgregarAnimal.Name = "btnAgregarAnimal";
            this.btnAgregarAnimal.Size = new System.Drawing.Size(75, 23);
            this.btnAgregarAnimal.TabIndex = 2;
            this.btnAgregarAnimal.Text = "Agregar";
            this.btnAgregarAnimal.UseVisualStyleBackColor = true;
            this.btnAgregarAnimal.Click += new System.EventHandler(this.button1_Click);
            // 
            // Informacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 577);
            this.Controls.Add(this.btnAgregarAnimal);
            this.Controls.Add(this.dgvAnimales);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Informacion";
            this.Text = "Informacion";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAnimales)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvAnimales;
        private System.Windows.Forms.Button btnAgregarAnimal;
    }
}