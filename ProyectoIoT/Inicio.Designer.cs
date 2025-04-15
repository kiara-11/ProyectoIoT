namespace ProyectoIoT
{
    partial class Inicio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inicio));
            this.panelMenu = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bt_dashboard = new FontAwesome.Sharp.IconButton();
            this.bt_animales = new FontAwesome.Sharp.IconButton();
            this.bt_dietas = new FontAwesome.Sharp.IconButton();
            this.bt_alertas = new FontAwesome.Sharp.IconButton();
            this.panelDesktop = new System.Windows.Forms.Panel();
            this.panelMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(28)))), ((int)(((byte)(62)))));
            this.panelMenu.Controls.Add(this.bt_alertas);
            this.panelMenu.Controls.Add(this.bt_dietas);
            this.panelMenu.Controls.Add(this.bt_animales);
            this.panelMenu.Controls.Add(this.bt_dashboard);
            this.panelMenu.Controls.Add(this.label2);
            this.panelMenu.Controls.Add(this.label1);
            this.panelMenu.Controls.Add(this.pictureBox1);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(253, 687);
            this.panelMenu.TabIndex = 0;
            this.panelMenu.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMenu_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(100, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Admin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(96, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Aman Admin";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(16, 35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(74, 67);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // bt_dashboard
            // 
            this.bt_dashboard.BackColor = System.Drawing.Color.Transparent;
            this.bt_dashboard.FlatAppearance.BorderSize = 0;
            this.bt_dashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_dashboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.bt_dashboard.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bt_dashboard.IconChar = FontAwesome.Sharp.IconChar.None;
            this.bt_dashboard.IconColor = System.Drawing.Color.Black;
            this.bt_dashboard.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.bt_dashboard.Location = new System.Drawing.Point(0, 143);
            this.bt_dashboard.Name = "bt_dashboard";
            this.bt_dashboard.Size = new System.Drawing.Size(226, 50);
            this.bt_dashboard.TabIndex = 0;
            this.bt_dashboard.Text = "Dashboard";
            this.bt_dashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bt_dashboard.UseVisualStyleBackColor = false;
            this.bt_dashboard.Click += new System.EventHandler(this.bt_dashboard_Click_1);
            // 
            // bt_animales
            // 
            this.bt_animales.BackColor = System.Drawing.Color.Transparent;
            this.bt_animales.FlatAppearance.BorderSize = 0;
            this.bt_animales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_animales.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.bt_animales.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bt_animales.IconChar = FontAwesome.Sharp.IconChar.None;
            this.bt_animales.IconColor = System.Drawing.Color.Black;
            this.bt_animales.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.bt_animales.Location = new System.Drawing.Point(0, 199);
            this.bt_animales.Name = "bt_animales";
            this.bt_animales.Size = new System.Drawing.Size(226, 50);
            this.bt_animales.TabIndex = 8;
            this.bt_animales.Text = "Animales info.";
            this.bt_animales.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bt_animales.UseVisualStyleBackColor = false;
            this.bt_animales.Click += new System.EventHandler(this.bt_animales_Click_1);
            // 
            // bt_dietas
            // 
            this.bt_dietas.BackColor = System.Drawing.Color.Transparent;
            this.bt_dietas.FlatAppearance.BorderSize = 0;
            this.bt_dietas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_dietas.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.bt_dietas.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bt_dietas.IconChar = FontAwesome.Sharp.IconChar.None;
            this.bt_dietas.IconColor = System.Drawing.Color.Black;
            this.bt_dietas.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.bt_dietas.Location = new System.Drawing.Point(6, 255);
            this.bt_dietas.Name = "bt_dietas";
            this.bt_dietas.Size = new System.Drawing.Size(226, 50);
            this.bt_dietas.TabIndex = 9;
            this.bt_dietas.Text = "Diestas conf.";
            this.bt_dietas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bt_dietas.UseVisualStyleBackColor = false;
            this.bt_dietas.Click += new System.EventHandler(this.bt_dietas_Click_1);
            // 
            // bt_alertas
            // 
            this.bt_alertas.BackColor = System.Drawing.Color.Transparent;
            this.bt_alertas.FlatAppearance.BorderSize = 0;
            this.bt_alertas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_alertas.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.bt_alertas.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bt_alertas.IconChar = FontAwesome.Sharp.IconChar.None;
            this.bt_alertas.IconColor = System.Drawing.Color.Black;
            this.bt_alertas.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.bt_alertas.Location = new System.Drawing.Point(3, 311);
            this.bt_alertas.Name = "bt_alertas";
            this.bt_alertas.Size = new System.Drawing.Size(226, 50);
            this.bt_alertas.TabIndex = 10;
            this.bt_alertas.Text = "Alertas";
            this.bt_alertas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bt_alertas.UseVisualStyleBackColor = false;
            this.bt_alertas.Click += new System.EventHandler(this.bt_alertas_Click_1);
            // 
            // panelDesktop
            // 
            this.panelDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDesktop.Location = new System.Drawing.Point(253, 0);
            this.panelDesktop.Name = "panelDesktop";
            this.panelDesktop.Size = new System.Drawing.Size(933, 687);
            this.panelDesktop.TabIndex = 1;
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 687);
            this.Controls.Add(this.panelDesktop);
            this.Controls.Add(this.panelMenu);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Inicio";
            this.Text = "Inicio";
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private FontAwesome.Sharp.IconButton bt_dashboard;
        private FontAwesome.Sharp.IconButton bt_alertas;
        private FontAwesome.Sharp.IconButton bt_dietas;
        private FontAwesome.Sharp.IconButton bt_animales;
        private System.Windows.Forms.Panel panelDesktop;
    }
}