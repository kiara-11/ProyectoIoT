namespace ProyectoIoT
{
    partial class Dieta
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
            this.flowLayoutPanelTarjetas = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanelTarjetas
            // 
            this.flowLayoutPanelTarjetas.AutoScroll = true;
            this.flowLayoutPanelTarjetas.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelTarjetas.Name = "flowLayoutPanelTarjetas";
            this.flowLayoutPanelTarjetas.Size = new System.Drawing.Size(1081, 575);
            this.flowLayoutPanelTarjetas.TabIndex = 6;
            this.flowLayoutPanelTarjetas.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanelTarjetas_Paint);
            // 
            // Dieta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1080, 576);
            this.Controls.Add(this.flowLayoutPanelTarjetas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Dieta";
            this.Text = "Dieta";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTarjetas;
    }
}