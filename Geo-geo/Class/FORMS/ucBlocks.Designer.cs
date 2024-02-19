namespace Geo_geo.Class.FORMS {
    partial class ucBlocks {
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
            this.cboBlok = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboBlok
            // 
            this.cboBlok.FormattingEnabled = true;
            this.cboBlok.Location = new System.Drawing.Point(8, 3);
            this.cboBlok.Name = "cboBlok";
            this.cboBlok.Size = new System.Drawing.Size(170, 21);
            this.cboBlok.TabIndex = 11;
            this.cboBlok.SelectedIndexChanged += new System.EventHandler(this.cboBlok_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ucBlocks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cboBlok);
            this.Name = "ucBlocks";
            this.Size = new System.Drawing.Size(181, 59);
            this.Load += new System.EventHandler(this.ucBlocks_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboBlok;
        private System.Windows.Forms.Button button1;
    }
}
