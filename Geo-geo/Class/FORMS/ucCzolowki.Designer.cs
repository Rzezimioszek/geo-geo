namespace Geo_geo.Class.FORMS {
    partial class ucCzolowki {
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
            this.cboPrec = new System.Windows.Forms.ComboBox();
            this.lblPrec = new System.Windows.Forms.Label();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.lblSufix = new System.Windows.Forms.Label();
            this.tbPrefix = new System.Windows.Forms.TextBox();
            this.tbSufix = new System.Windows.Forms.TextBox();
            this.btnCzolowki = new System.Windows.Forms.Button();
            this.tbHeight = new System.Windows.Forms.TextBox();
            this.lblHeight = new System.Windows.Forms.Label();
            this.cbDuplicate = new System.Windows.Forms.CheckBox();
            this.tbWF = new System.Windows.Forms.TextBox();
            this.lblSmukl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboPrec
            // 
            this.cboPrec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.cboPrec.FormattingEnabled = true;
            this.cboPrec.Location = new System.Drawing.Point(6, 23);
            this.cboPrec.Name = "cboPrec";
            this.cboPrec.Size = new System.Drawing.Size(94, 23);
            this.cboPrec.TabIndex = 3;
            // 
            // lblPrec
            // 
            this.lblPrec.Location = new System.Drawing.Point(5, 6);
            this.lblPrec.Name = "lblPrec";
            this.lblPrec.Size = new System.Drawing.Size(74, 24);
            this.lblPrec.TabIndex = 4;
            this.lblPrec.Text = "Miejsca po \".\"";
            this.lblPrec.Click += new System.EventHandler(this.lblFormat_Click);
            // 
            // lblPrefix
            // 
            this.lblPrefix.Location = new System.Drawing.Point(3, 49);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(40, 24);
            this.lblPrefix.TabIndex = 5;
            this.lblPrefix.Text = "Prefix";
            this.lblPrefix.Click += new System.EventHandler(this.lblPrefix_Click);
            // 
            // lblSufix
            // 
            this.lblSufix.Location = new System.Drawing.Point(50, 49);
            this.lblSufix.Name = "lblSufix";
            this.lblSufix.Size = new System.Drawing.Size(43, 24);
            this.lblSufix.TabIndex = 6;
            this.lblSufix.Text = "Sufix";
            this.lblSufix.Click += new System.EventHandler(this.lblSufix_Click);
            // 
            // tbPrefix
            // 
            this.tbPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.tbPrefix.Location = new System.Drawing.Point(6, 63);
            this.tbPrefix.Name = "tbPrefix";
            this.tbPrefix.Size = new System.Drawing.Size(46, 21);
            this.tbPrefix.TabIndex = 8;
            this.tbPrefix.Text = "-";
            this.tbPrefix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbSufix
            // 
            this.tbSufix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.tbSufix.Location = new System.Drawing.Point(53, 63);
            this.tbSufix.Name = "tbSufix";
            this.tbSufix.Size = new System.Drawing.Size(46, 21);
            this.tbSufix.TabIndex = 9;
            this.tbSufix.Text = "-";
            this.tbSufix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCzolowki
            // 
            this.btnCzolowki.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCzolowki.Location = new System.Drawing.Point(6, 106);
            this.btnCzolowki.Margin = new System.Windows.Forms.Padding(5);
            this.btnCzolowki.Name = "btnCzolowki";
            this.btnCzolowki.Size = new System.Drawing.Size(184, 25);
            this.btnCzolowki.TabIndex = 10;
            this.btnCzolowki.Text = "Wybierz linie";
            this.btnCzolowki.UseVisualStyleBackColor = true;
            this.btnCzolowki.Click += new System.EventHandler(this.btnCzolowki_Click);
            // 
            // tbHeight
            // 
            this.tbHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.tbHeight.Location = new System.Drawing.Point(100, 23);
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new System.Drawing.Size(90, 21);
            this.tbHeight.TabIndex = 11;
            this.tbHeight.Text = "1.0";
            this.tbHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblHeight
            // 
            this.lblHeight.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblHeight.Location = new System.Drawing.Point(95, 6);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(64, 21);
            this.lblHeight.TabIndex = 12;
            this.lblHeight.Text = "Wysokość";
            this.lblHeight.Click += new System.EventHandler(this.lblHeight_Click);
            // 
            // cbDuplicate
            // 
            this.cbDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDuplicate.AutoSize = true;
            this.cbDuplicate.Location = new System.Drawing.Point(76, 90);
            this.cbDuplicate.Name = "cbDuplicate";
            this.cbDuplicate.Size = new System.Drawing.Size(106, 17);
            this.cbDuplicate.TabIndex = 13;
            this.cbDuplicate.Text = "Usuwaj duplikaty";
            this.cbDuplicate.UseVisualStyleBackColor = true;
            this.cbDuplicate.CheckedChanged += new System.EventHandler(this.cbDuplicate_CheckedChanged);
            // 
            // tbWF
            // 
            this.tbWF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.tbWF.Location = new System.Drawing.Point(100, 63);
            this.tbWF.Name = "tbWF";
            this.tbWF.Size = new System.Drawing.Size(90, 21);
            this.tbWF.TabIndex = 14;
            this.tbWF.Text = "1.0";
            this.tbWF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSmukl
            // 
            this.lblSmukl.Location = new System.Drawing.Point(97, 49);
            this.lblSmukl.Name = "lblSmukl";
            this.lblSmukl.Size = new System.Drawing.Size(64, 21);
            this.lblSmukl.TabIndex = 15;
            this.lblSmukl.Text = "Smukłość";
            // 
            // ucCzolowki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tbWF);
            this.Controls.Add(this.cbDuplicate);
            this.Controls.Add(this.tbHeight);
            this.Controls.Add(this.btnCzolowki);
            this.Controls.Add(this.tbSufix);
            this.Controls.Add(this.tbPrefix);
            this.Controls.Add(this.lblSufix);
            this.Controls.Add(this.lblPrefix);
            this.Controls.Add(this.cboPrec);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.lblPrec);
            this.Controls.Add(this.lblSmukl);
            this.MinimumSize = new System.Drawing.Size(194, 143);
            this.Name = "ucCzolowki";
            this.Size = new System.Drawing.Size(194, 143);
            this.Load += new System.EventHandler(this.ucCzolowki_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboPrec;
        private System.Windows.Forms.Label lblPrec;
        private System.Windows.Forms.Label lblPrefix;
        private System.Windows.Forms.Label lblSufix;
        private System.Windows.Forms.TextBox tbPrefix;
        private System.Windows.Forms.TextBox tbSufix;
        private System.Windows.Forms.Button btnCzolowki;
        private System.Windows.Forms.TextBox tbHeight;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.CheckBox cbDuplicate;
        private System.Windows.Forms.TextBox tbWF;
        private System.Windows.Forms.Label lblSmukl;
    }
}
