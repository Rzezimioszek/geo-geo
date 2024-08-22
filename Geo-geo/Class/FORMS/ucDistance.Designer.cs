namespace Geo_geo.Class.FORMS {
    partial class ucDistance {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btnReset = new System.Windows.Forms.Button();
            this.btnTakeDistance = new System.Windows.Forms.Button();
            this.txtLast = new System.Windows.Forms.TextBox();
            this.txtSuma = new System.Windows.Forms.TextBox();
            this.lblLast = new System.Windows.Forms.Label();
            this.lblSuma = new System.Windows.Forms.Label();
            this.chDraw = new System.Windows.Forms.CheckBox();
            this.btnDelLast = new System.Windows.Forms.Button();
            this.txtEntity = new System.Windows.Forms.TextBox();
            this.btnDelAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(3, 203);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(159, 23);
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnTakeDistance
            // 
            this.btnTakeDistance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTakeDistance.Location = new System.Drawing.Point(3, 146);
            this.btnTakeDistance.Name = "btnTakeDistance";
            this.btnTakeDistance.Size = new System.Drawing.Size(159, 23);
            this.btnTakeDistance.TabIndex = 1;
            this.btnTakeDistance.Text = "Mierz";
            this.btnTakeDistance.UseVisualStyleBackColor = true;
            this.btnTakeDistance.Click += new System.EventHandler(this.btnTakeDistance_Click);
            // 
            // txtLast
            // 
            this.txtLast.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLast.Location = new System.Drawing.Point(3, 26);
            this.txtLast.Name = "txtLast";
            this.txtLast.Size = new System.Drawing.Size(159, 20);
            this.txtLast.TabIndex = 2;
            this.txtLast.Text = "0";
            // 
            // txtSuma
            // 
            this.txtSuma.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSuma.Location = new System.Drawing.Point(3, 70);
            this.txtSuma.Name = "txtSuma";
            this.txtSuma.Size = new System.Drawing.Size(158, 20);
            this.txtSuma.TabIndex = 3;
            this.txtSuma.Text = "0";
            // 
            // lblLast
            // 
            this.lblLast.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLast.AutoSize = true;
            this.lblLast.Location = new System.Drawing.Point(6, 6);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(40, 13);
            this.lblLast.TabIndex = 4;
            this.lblLast.Text = "Ostatni";
            // 
            // lblSuma
            // 
            this.lblSuma.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSuma.AutoSize = true;
            this.lblSuma.Location = new System.Drawing.Point(3, 51);
            this.lblSuma.Name = "lblSuma";
            this.lblSuma.Size = new System.Drawing.Size(34, 13);
            this.lblSuma.TabIndex = 5;
            this.lblSuma.Text = "Suma";
            // 
            // chDraw
            // 
            this.chDraw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chDraw.AutoSize = true;
            this.chDraw.Checked = true;
            this.chDraw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chDraw.Location = new System.Drawing.Point(6, 97);
            this.chDraw.Name = "chDraw";
            this.chDraw.Size = new System.Drawing.Size(95, 17);
            this.chDraw.TabIndex = 6;
            this.chDraw.Text = "Rysowanie linii";
            this.chDraw.UseVisualStyleBackColor = true;
            this.chDraw.CheckedChanged += new System.EventHandler(this.chDraw_CheckedChanged);
            // 
            // btnDelLast
            // 
            this.btnDelLast.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnDelLast.Location = new System.Drawing.Point(3, 175);
            this.btnDelLast.Name = "btnDelLast";
            this.btnDelLast.Size = new System.Drawing.Size(158, 22);
            this.btnDelLast.TabIndex = 7;
            this.btnDelLast.Text = "Usuń ostatnią linie";
            this.btnDelLast.UseVisualStyleBackColor = true;
            this.btnDelLast.Click += new System.EventHandler(this.btnDelLast_Click);
            // 
            // txtEntity
            // 
            this.txtEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEntity.Location = new System.Drawing.Point(3, 120);
            this.txtEntity.Name = "txtEntity";
            this.txtEntity.Size = new System.Drawing.Size(158, 20);
            this.txtEntity.TabIndex = 8;
            this.txtEntity.Text = "0";
            this.txtEntity.Visible = false;
            // 
            // btnDelAll
            // 
            this.btnDelAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelAll.Location = new System.Drawing.Point(85, 175);
            this.btnDelAll.Name = "btnDelAll";
            this.btnDelAll.Size = new System.Drawing.Size(76, 22);
            this.btnDelAll.TabIndex = 9;
            this.btnDelAll.Text = "Usuń All";
            this.btnDelAll.UseVisualStyleBackColor = true;
            this.btnDelAll.Visible = false;
            this.btnDelAll.Click += new System.EventHandler(this.btnDelAll_Click);
            // 
            // ucDistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelAll);
            this.Controls.Add(this.txtEntity);
            this.Controls.Add(this.btnDelLast);
            this.Controls.Add(this.chDraw);
            this.Controls.Add(this.lblSuma);
            this.Controls.Add(this.lblLast);
            this.Controls.Add(this.txtSuma);
            this.Controls.Add(this.txtLast);
            this.Controls.Add(this.btnTakeDistance);
            this.Controls.Add(this.btnReset);
            this.MinimumSize = new System.Drawing.Size(69, 160);
            this.Name = "ucDistance";
            this.Size = new System.Drawing.Size(165, 232);
            this.Load += new System.EventHandler(this.ucDistance_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnTakeDistance;
        private System.Windows.Forms.TextBox txtLast;
        private System.Windows.Forms.TextBox txtSuma;
        private System.Windows.Forms.Label lblLast;
        private System.Windows.Forms.Label lblSuma;
        private System.Windows.Forms.CheckBox chDraw;
        private System.Windows.Forms.Button btnDelLast;
        private System.Windows.Forms.TextBox txtEntity;
        private System.Windows.Forms.Button btnDelAll;
    }
}
