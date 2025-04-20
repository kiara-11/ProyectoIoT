namespace ProyectoIoT
{
    partial class Dispensador
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.bt_dispensar = new FontAwesome.Sharp.IconButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(374, 97);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(158, 24);
            this.comboBox1.TabIndex = 0;
            // 
            // bt_dispensar
            // 
            this.bt_dispensar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(194)))), ((int)(((byte)(14)))));
            this.bt_dispensar.FlatAppearance.BorderSize = 0;
            this.bt_dispensar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_dispensar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F);
            this.bt_dispensar.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bt_dispensar.IconChar = FontAwesome.Sharp.IconChar.CheckDouble;
            this.bt_dispensar.IconColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bt_dispensar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.bt_dispensar.IconSize = 25;
            this.bt_dispensar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bt_dispensar.Location = new System.Drawing.Point(231, 170);
            this.bt_dispensar.Name = "bt_dispensar";
            this.bt_dispensar.Size = new System.Drawing.Size(171, 41);
            this.bt_dispensar.TabIndex = 14;
            this.bt_dispensar.Text = "Dispensar";
            this.bt_dispensar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.bt_dispensar.UseVisualStyleBackColor = false;
            this.bt_dispensar.Click += new System.EventHandler(this.bt_dispensar_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(108, 97);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(169, 22);
            this.textBox1.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(104, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(188, 19);
            this.label11.TabIndex = 55;
            this.label11.Text = "Cantidad dispensada:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(358, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 19);
            this.label1.TabIndex = 56;
            this.label1.Text = "Seleccione el alimento:";
            // 
            // Dispensador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(627, 258);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.bt_dispensar);
            this.Controls.Add(this.comboBox1);
            this.Name = "Dispensador";
            this.Text = "Dispensador";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private FontAwesome.Sharp.IconButton bt_dispensar;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label1;
    }
}