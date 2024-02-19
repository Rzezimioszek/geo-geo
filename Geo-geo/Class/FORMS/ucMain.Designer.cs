namespace Geo_geo.Class.FORMS {
    partial class ucMain {
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
            this.btnImport = new System.Windows.Forms.Button();
            this.cboFormat = new System.Windows.Forms.ComboBox();
            this.lblFormat = new System.Windows.Forms.Label();
            this.cboTyp = new System.Windows.Forms.ComboBox();
            this.lblTyp = new System.Windows.Forms.Label();
            this.cbZamianaH = new System.Windows.Forms.CheckBox();
            this.tbSkala = new System.Windows.Forms.TextBox();
            this.lblSkala = new System.Windows.Forms.Label();
            this.cbH0 = new System.Windows.Forms.CheckBox();
            this.cboSep = new System.Windows.Forms.ComboBox();
            this.lbSep = new System.Windows.Forms.Label();
            this.lblAcPoint = new System.Windows.Forms.Label();
            this.cboAcPoint = new System.Windows.Forms.ComboBox();
            this.nupRow = new System.Windows.Forms.NumericUpDown();
            this.lbRow = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbOX = new System.Windows.Forms.TextBox();
            this.tbOY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboBlok = new System.Windows.Forms.ComboBox();
            this.cboAtr = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nupRow)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(6, 219);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(229, 25);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Wczytaj pkt";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cboFormat
            // 
            this.cboFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFormat.FormattingEnabled = true;
            this.cboFormat.Location = new System.Drawing.Point(65, 7);
            this.cboFormat.Name = "cboFormat";
            this.cboFormat.Size = new System.Drawing.Size(170, 21);
            this.cboFormat.TabIndex = 2;
            this.cboFormat.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblFormat
            // 
            this.lblFormat.Location = new System.Drawing.Point(3, 10);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(71, 24);
            this.lblFormat.TabIndex = 3;
            this.lblFormat.Text = "Format";
            // 
            // cboTyp
            // 
            this.cboTyp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTyp.FormattingEnabled = true;
            this.cboTyp.Location = new System.Drawing.Point(65, 33);
            this.cboTyp.Name = "cboTyp";
            this.cboTyp.Size = new System.Drawing.Size(170, 21);
            this.cboTyp.TabIndex = 4;
            this.cboTyp.SelectedIndexChanged += new System.EventHandler(this.cboTyp_SelectedIndexChanged);
            // 
            // lblTyp
            // 
            this.lblTyp.Location = new System.Drawing.Point(3, 33);
            this.lblTyp.Name = "lblTyp";
            this.lblTyp.Size = new System.Drawing.Size(56, 24);
            this.lblTyp.TabIndex = 5;
            this.lblTyp.Text = "Typ";
            // 
            // cbZamianaH
            // 
            this.cbZamianaH.AutoSize = true;
            this.cbZamianaH.Location = new System.Drawing.Point(110, 89);
            this.cbZamianaH.Name = "cbZamianaH";
            this.cbZamianaH.Size = new System.Drawing.Size(116, 17);
            this.cbZamianaH.TabIndex = 6;
            this.cbZamianaH.Text = "Kartuj H zamiast Nr";
            this.cbZamianaH.UseVisualStyleBackColor = true;
            // 
            // tbSkala
            // 
            this.tbSkala.Location = new System.Drawing.Point(65, 87);
            this.tbSkala.Name = "tbSkala";
            this.tbSkala.Size = new System.Drawing.Size(39, 20);
            this.tbSkala.TabIndex = 7;
            this.tbSkala.Text = "1.5";
            // 
            // lblSkala
            // 
            this.lblSkala.Location = new System.Drawing.Point(3, 87);
            this.lblSkala.Name = "lblSkala";
            this.lblSkala.Size = new System.Drawing.Size(62, 24);
            this.lblSkala.TabIndex = 8;
            this.lblSkala.Text = "Skala:";
            // 
            // cbH0
            // 
            this.cbH0.AutoSize = true;
            this.cbH0.Location = new System.Drawing.Point(110, 112);
            this.cbH0.Name = "cbH0";
            this.cbH0.Size = new System.Drawing.Size(106, 17);
            this.cbH0.TabIndex = 9;
            this.cbH0.Text = "Ustaw H na 0.00";
            this.cbH0.UseVisualStyleBackColor = true;
            this.cbH0.CheckedChanged += new System.EventHandler(this.cbH0_CheckedChanged);
            // 
            // cboSep
            // 
            this.cboSep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSep.FormattingEnabled = true;
            this.cboSep.Location = new System.Drawing.Point(65, 60);
            this.cboSep.Name = "cboSep";
            this.cboSep.Size = new System.Drawing.Size(170, 21);
            this.cboSep.TabIndex = 10;
            // 
            // lbSep
            // 
            this.lbSep.Location = new System.Drawing.Point(3, 60);
            this.lbSep.Name = "lbSep";
            this.lbSep.Size = new System.Drawing.Size(62, 24);
            this.lbSep.TabIndex = 11;
            this.lbSep.Text = "Separator";
            // 
            // lblAcPoint
            // 
            this.lblAcPoint.Location = new System.Drawing.Point(3, 112);
            this.lblAcPoint.Name = "lblAcPoint";
            this.lblAcPoint.Size = new System.Drawing.Size(62, 24);
            this.lblAcPoint.TabIndex = 12;
            this.lblAcPoint.Text = "Typ pkt";
            this.lblAcPoint.Visible = false;
            // 
            // cboAcPoint
            // 
            this.cboAcPoint.FormattingEnabled = true;
            this.cboAcPoint.Location = new System.Drawing.Point(65, 112);
            this.cboAcPoint.Name = "cboAcPoint";
            this.cboAcPoint.Size = new System.Drawing.Size(39, 21);
            this.cboAcPoint.TabIndex = 13;
            this.cboAcPoint.Visible = false;
            // 
            // nupRow
            // 
            this.nupRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nupRow.Location = new System.Drawing.Point(176, 188);
            this.nupRow.Name = "nupRow";
            this.nupRow.Size = new System.Drawing.Size(59, 20);
            this.nupRow.TabIndex = 16;
            // 
            // lbRow
            // 
            this.lbRow.Location = new System.Drawing.Point(6, 188);
            this.lbRow.Name = "lbRow";
            this.lbRow.Size = new System.Drawing.Size(229, 28);
            this.lbRow.TabIndex = 17;
            this.lbRow.Text = "Wiersze do pominięcia";
            this.lbRow.Click += new System.EventHandler(this.lbRow_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 24);
            this.label1.TabIndex = 18;
            this.label1.Text = "Odsunięcie wstawienia punktu";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 24);
            this.label2.TabIndex = 19;
            this.label2.Text = "W prawo";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // tbOX
            // 
            this.tbOX.Location = new System.Drawing.Point(79, 157);
            this.tbOX.Name = "tbOX";
            this.tbOX.Size = new System.Drawing.Size(39, 20);
            this.tbOX.TabIndex = 20;
            this.tbOX.Text = "0.0";
            this.tbOX.TextChanged += new System.EventHandler(this.tbOX_TextChanged);
            // 
            // tbOY
            // 
            this.tbOY.Location = new System.Drawing.Point(187, 157);
            this.tbOY.Name = "tbOY";
            this.tbOY.Size = new System.Drawing.Size(39, 20);
            this.tbOY.TabIndex = 22;
            this.tbOY.Text = "0.0";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(124, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 24);
            this.label3.TabIndex = 21;
            this.label3.Text = "W górę";
            // 
            // cboBlok
            // 
            this.cboBlok.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBlok.FormattingEnabled = true;
            this.cboBlok.Location = new System.Drawing.Point(65, 86);
            this.cboBlok.Name = "cboBlok";
            this.cboBlok.Size = new System.Drawing.Size(170, 21);
            this.cboBlok.TabIndex = 23;
            this.cboBlok.SelectedIndexChanged += new System.EventHandler(this.cboBlok_SelectedIndexChanged);
            // 
            // cboAtr
            // 
            this.cboAtr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAtr.FormattingEnabled = true;
            this.cboAtr.Location = new System.Drawing.Point(6, 112);
            this.cboAtr.Name = "cboAtr";
            this.cboAtr.Size = new System.Drawing.Size(53, 21);
            this.cboAtr.TabIndex = 24;
            this.cboAtr.Visible = false;
            // 
            // ucMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.cboAtr);
            this.Controls.Add(this.cboBlok);
            this.Controls.Add(this.nupRow);
            this.Controls.Add(this.tbOY);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbOX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboFormat);
            this.Controls.Add(this.lbRow);
            this.Controls.Add(this.cboAcPoint);
            this.Controls.Add(this.lblAcPoint);
            this.Controls.Add(this.lbSep);
            this.Controls.Add(this.cboSep);
            this.Controls.Add(this.cbH0);
            this.Controls.Add(this.lblSkala);
            this.Controls.Add(this.tbSkala);
            this.Controls.Add(this.cbZamianaH);
            this.Controls.Add(this.lblTyp);
            this.Controls.Add(this.cboTyp);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.btnImport);
            this.MinimumSize = new System.Drawing.Size(243, 250);
            this.Name = "ucMain";
            this.Size = new System.Drawing.Size(243, 250);
            this.Load += new System.EventHandler(this.ucMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nupRow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ComboBox cboFormat;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.ComboBox cboTyp;
        private System.Windows.Forms.Label lblTyp;
        private System.Windows.Forms.CheckBox cbZamianaH;
        private System.Windows.Forms.TextBox tbSkala;
        private System.Windows.Forms.Label lblSkala;
        private System.Windows.Forms.CheckBox cbH0;
        private System.Windows.Forms.ComboBox cboSep;
        private System.Windows.Forms.Label lbSep;
        private System.Windows.Forms.Label lblAcPoint;
        private System.Windows.Forms.ComboBox cboAcPoint;
        private System.Windows.Forms.NumericUpDown nupRow;
        private System.Windows.Forms.Label lbRow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbOX;
        private System.Windows.Forms.TextBox tbOY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboBlok;
        private System.Windows.Forms.ComboBox cboAtr;
    }
}
