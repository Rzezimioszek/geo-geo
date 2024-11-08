namespace Geo_geo.Class.FORMS {
    partial class fLayoutSelector {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.btn_return = new System.Windows.Forms.Button();
            this.lbLayouts = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chVPLock = new System.Windows.Forms.CheckBox();
            this.cbScale = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblOwn = new System.Windows.Forms.Label();
            this.tbOwn = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.chActive = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_return
            // 
            this.btn_return.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_return.Location = new System.Drawing.Point(12, 378);
            this.btn_return.Name = "btn_return";
            this.btn_return.Size = new System.Drawing.Size(241, 23);
            this.btn_return.TabIndex = 0;
            this.btn_return.Text = "Wczytaj na papier";
            this.btn_return.UseVisualStyleBackColor = true;
            this.btn_return.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbLayouts
            // 
            this.lbLayouts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLayouts.FormattingEnabled = true;
            this.lbLayouts.Location = new System.Drawing.Point(12, 41);
            this.lbLayouts.Name = "lbLayouts";
            this.lbLayouts.Size = new System.Drawing.Size(241, 251);
            this.lbLayouts.TabIndex = 1;
            this.lbLayouts.SelectedIndexChanged += new System.EventHandler(this.lbLayouts_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Dostępne arkusze";
            // 
            // chVPLock
            // 
            this.chVPLock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chVPLock.Checked = true;
            this.chVPLock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chVPLock.Location = new System.Drawing.Point(12, 298);
            this.chVPLock.Name = "chVPLock";
            this.chVPLock.Size = new System.Drawing.Size(238, 24);
            this.chVPLock.TabIndex = 4;
            this.chVPLock.Text = "Zablokuj Viewport";
            this.chVPLock.UseVisualStyleBackColor = true;
            this.chVPLock.CheckedChanged += new System.EventHandler(this.chVPLock_CheckedChanged);
            // 
            // cbScale
            // 
            this.cbScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbScale.FormattingEnabled = true;
            this.cbScale.Location = new System.Drawing.Point(60, 352);
            this.cbScale.Name = "cbScale";
            this.cbScale.Size = new System.Drawing.Size(90, 21);
            this.cbScale.TabIndex = 5;
            this.cbScale.SelectedIndexChanged += new System.EventHandler(this.cbScale_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 352);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "Skala";
            // 
            // lblOwn
            // 
            this.lblOwn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOwn.Location = new System.Drawing.Point(153, 352);
            this.lblOwn.Name = "lblOwn";
            this.lblOwn.Size = new System.Drawing.Size(20, 23);
            this.lblOwn.TabIndex = 7;
            this.lblOwn.Text = "1:";
            this.lblOwn.Visible = false;
            // 
            // tbOwn
            // 
            this.tbOwn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOwn.Location = new System.Drawing.Point(172, 352);
            this.tbOwn.Name = "tbOwn";
            this.tbOwn.Size = new System.Drawing.Size(78, 20);
            this.tbOwn.TabIndex = 8;
            this.tbOwn.Text = "1";
            this.tbOwn.Visible = false;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(135, 10);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(118, 23);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Anuluj ten i kolejne";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // chActive
            // 
            this.chActive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chActive.Checked = true;
            this.chActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chActive.Location = new System.Drawing.Point(12, 322);
            this.chActive.Name = "chActive";
            this.chActive.Size = new System.Drawing.Size(238, 24);
            this.chActive.TabIndex = 10;
            this.chActive.Text = "Zrzut na aktywną warstwę";
            this.chActive.UseVisualStyleBackColor = true;
            this.chActive.CheckedChanged += new System.EventHandler(this.chActive_CheckedChanged);
            // 
            // fLayoutSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 413);
            this.Controls.Add(this.chActive);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.tbOwn);
            this.Controls.Add(this.lblOwn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbScale);
            this.Controls.Add(this.chVPLock);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbLayouts);
            this.Controls.Add(this.btn_return);
            this.Name = "fLayoutSelector";
            this.Text = "fLayoutSelector";
            this.Load += new System.EventHandler(this.fLayoutSelector_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_return;
        private System.Windows.Forms.ListBox lbLayouts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chVPLock;
        private System.Windows.Forms.ComboBox cbScale;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblOwn;
        private System.Windows.Forms.TextBox tbOwn;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox chActive;
    }
}