namespace FormApostas
{
    partial class ValorAposta
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
            this.lblValorApostado = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtValorApostado = new FormApostas.Controles.NumericTextBox();
            this.SuspendLayout();
            // 
            // lblValorApostado
            // 
            this.lblValorApostado.AutoSize = true;
            this.lblValorApostado.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorApostado.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblValorApostado.Location = new System.Drawing.Point(106, 9);
            this.lblValorApostado.Name = "lblValorApostado";
            this.lblValorApostado.Size = new System.Drawing.Size(215, 24);
            this.lblValorApostado.TabIndex = 0;
            this.lblValorApostado.Text = "Quanto quer Apostar?";
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(319, 53);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(56, 29);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtValorApostado
            // 
            this.txtValorApostado.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorApostado.Location = new System.Drawing.Point(113, 53);
            this.txtValorApostado.Name = "txtValorApostado";
            this.txtValorApostado.Size = new System.Drawing.Size(200, 29);
            this.txtValorApostado.TabIndex = 1;
            this.txtValorApostado.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ValorAposta
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(178)))));
            this.ClientSize = new System.Drawing.Size(444, 104);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtValorApostado);
            this.Controls.Add(this.lblValorApostado);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ValorAposta";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controles.NumericTextBox txtValorApostado;
        private System.Windows.Forms.Label lblValorApostado;
        private System.Windows.Forms.Button btnOk;
    }
}