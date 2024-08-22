namespace Geo_geo.Class.FORMS {
    partial class fEWmapa {
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
            this.btnOp1 = new System.Windows.Forms.Button();
            this.btnOp2 = new System.Windows.Forms.Button();
            this.btnOp3 = new System.Windows.Forms.Button();
            this.btnOp4 = new System.Windows.Forms.Button();
            this.btnOptOwn = new System.Windows.Forms.Button();
            this.txtMyCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOp1
            // 
            this.btnOp1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOp1.Location = new System.Drawing.Point(12, 12);
            this.btnOp1.Name = "btnOp1";
            this.btnOp1.Size = new System.Drawing.Size(160, 23);
            this.btnOp1.TabIndex = 0;
            this.btnOp1.Text = "OTRN";
            this.btnOp1.UseVisualStyleBackColor = true;
            this.btnOp1.Click += new System.EventHandler(this.btnOp1_Click);
            // 
            // btnOp2
            // 
            this.btnOp2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOp2.Location = new System.Drawing.Point(12, 41);
            this.btnOp2.Name = "btnOp2";
            this.btnOp2.Size = new System.Drawing.Size(160, 23);
            this.btnOp2.TabIndex = 1;
            this.btnOp2.Text = "OTRS";
            this.btnOp2.UseVisualStyleBackColor = true;
            this.btnOp2.Click += new System.EventHandler(this.btnOp2_Click);
            // 
            // btnOp3
            // 
            this.btnOp3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOp3.Location = new System.Drawing.Point(12, 70);
            this.btnOp3.Name = "btnOp3";
            this.btnOp3.Size = new System.Drawing.Size(160, 23);
            this.btnOp3.TabIndex = 2;
            this.btnOp3.Text = "RTPW01";
            this.btnOp3.UseVisualStyleBackColor = true;
            this.btnOp3.Click += new System.EventHandler(this.btnOp3_Click);
            // 
            // btnOp4
            // 
            this.btnOp4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOp4.Location = new System.Drawing.Point(12, 99);
            this.btnOp4.Name = "btnOp4";
            this.btnOp4.Size = new System.Drawing.Size(160, 23);
            this.btnOp4.TabIndex = 3;
            this.btnOp4.Text = "RTPW02";
            this.btnOp4.UseVisualStyleBackColor = true;
            this.btnOp4.Click += new System.EventHandler(this.btnOp4_Click_1);
            // 
            // btnOptOwn
            // 
            this.btnOptOwn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptOwn.Location = new System.Drawing.Point(145, 128);
            this.btnOptOwn.Name = "btnOptOwn";
            this.btnOptOwn.Size = new System.Drawing.Size(27, 23);
            this.btnOptOwn.TabIndex = 4;
            this.btnOptOwn.Text = "->";
            this.btnOptOwn.UseVisualStyleBackColor = true;
            this.btnOptOwn.Click += new System.EventHandler(this.btnOptOwn_Click);
            // 
            // txtMyCode
            // 
            this.txtMyCode.Location = new System.Drawing.Point(12, 131);
            this.txtMyCode.Name = "txtMyCode";
            this.txtMyCode.Size = new System.Drawing.Size(127, 20);
            this.txtMyCode.TabIndex = 5;
            this.txtMyCode.Text = "mój_kod";
            // 
            // fEWmapa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 158);
            this.ControlBox = false;
            this.Controls.Add(this.txtMyCode);
            this.Controls.Add(this.btnOptOwn);
            this.Controls.Add(this.btnOp4);
            this.Controls.Add(this.btnOp3);
            this.Controls.Add(this.btnOp2);
            this.Controls.Add(this.btnOp1);
            this.MaximumSize = new System.Drawing.Size(300, 197);
            this.MinimumSize = new System.Drawing.Size(200, 197);
            this.Name = "fEWmapa";
            this.ShowIcon = false;
            this.Text = "Kod eksportowanych punktów";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOp1;
        private System.Windows.Forms.Button btnOp2;
        private System.Windows.Forms.Button btnOp3;
        private System.Windows.Forms.Button btnOp4;
        private System.Windows.Forms.Button btnOptOwn;
        private System.Windows.Forms.TextBox txtMyCode;
    }
}