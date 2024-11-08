namespace Geo_geo.Class.FORMS {
    partial class ucMarkery {
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
            this.lstMarkers = new System.Windows.Forms.ListBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnDelAll = new System.Windows.Forms.Button();
            this.chkGo = new System.Windows.Forms.CheckBox();
            this.chShow = new System.Windows.Forms.CheckBox();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.chReverse = new System.Windows.Forms.CheckBox();
            this.btnFMap = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnCopyAll = new System.Windows.Forms.Button();
            this.rb1 = new System.Windows.Forms.RadioButton();
            this.rb2 = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoadTxt = new System.Windows.Forms.Button();
            this.tB = new System.Windows.Forms.TrackBar();
            this.btnSelectObj = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnDelBloks = new System.Windows.Forms.Button();
            this.chDel = new System.Windows.Forms.CheckBox();
            this.tbScale = new System.Windows.Forms.TextBox();
            this.lblScale = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnDelFiltr = new System.Windows.Forms.Button();
            this.btnFiltr = new System.Windows.Forms.Button();
            this.tbFiltr = new System.Windows.Forms.TextBox();
            this.lstMarkersID = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chFiltr = new System.Windows.Forms.CheckBox();
            this.btnSwap = new System.Windows.Forms.Button();
            this.btnSwapAll = new System.Windows.Forms.Button();
            this.lblCounter = new System.Windows.Forms.Label();
            this.lblCounterID = new System.Windows.Forms.Label();
            this.lblidxID = new System.Windows.Forms.Label();
            this.lblidx = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstMarkers
            // 
            this.lstMarkers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMarkers.FormattingEnabled = true;
            this.lstMarkers.Location = new System.Drawing.Point(5, 4);
            this.lstMarkers.Name = "lstMarkers";
            this.lstMarkers.Size = new System.Drawing.Size(237, 199);
            this.lstMarkers.TabIndex = 1;
            this.lstMarkers.SelectedIndexChanged += new System.EventHandler(this.lstMarkers_SelectedIndexChanged);
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(264, 53);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(72, 23);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Idź do";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnDel
            // 
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel.Location = new System.Drawing.Point(342, 140);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(72, 23);
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "Usuń";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnDelAll
            // 
            this.btnDelAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelAll.Location = new System.Drawing.Point(342, 169);
            this.btnDelAll.Name = "btnDelAll";
            this.btnDelAll.Size = new System.Drawing.Size(72, 23);
            this.btnDelAll.TabIndex = 4;
            this.btnDelAll.Text = "Usuń All";
            this.btnDelAll.UseVisualStyleBackColor = true;
            this.btnDelAll.Click += new System.EventHandler(this.btnDelAll_Click);
            // 
            // chkGo
            // 
            this.chkGo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkGo.Checked = true;
            this.chkGo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGo.Location = new System.Drawing.Point(8, 8);
            this.chkGo.Name = "chkGo";
            this.chkGo.Size = new System.Drawing.Size(213, 17);
            this.chkGo.TabIndex = 5;
            this.chkGo.Text = "Idź automatycznie do wybranego";
            this.chkGo.UseVisualStyleBackColor = true;
            this.chkGo.UseWaitCursor = true;
            this.chkGo.CheckedChanged += new System.EventHandler(this.chkGo_CheckedChanged);
            // 
            // chShow
            // 
            this.chShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chShow.AutoEllipsis = true;
            this.chShow.Checked = true;
            this.chShow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chShow.Location = new System.Drawing.Point(6, 188);
            this.chShow.Name = "chShow";
            this.chShow.Size = new System.Drawing.Size(129, 29);
            this.chShow.TabIndex = 6;
            this.chShow.Text = "Wstawiaj symbol";
            this.chShow.UseVisualStyleBackColor = true;
            this.chShow.CheckedChanged += new System.EventHandler(this.chShow_CheckedChanged);
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.Location = new System.Drawing.Point(343, 198);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(72, 23);
            this.btnReload.TabIndex = 7;
            this.btnReload.Text = "Odśwież";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPaste.Location = new System.Drawing.Point(264, 82);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(72, 23);
            this.btnPaste.TabIndex = 8;
            this.btnPaste.Text = "Wklej";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // chReverse
            // 
            this.chReverse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chReverse.Location = new System.Drawing.Point(8, 55);
            this.chReverse.Name = "chReverse";
            this.chReverse.Size = new System.Drawing.Size(213, 18);
            this.chReverse.TabIndex = 9;
            this.chReverse.Text = "Zamień XY interpretacje kolumn";
            this.chReverse.UseVisualStyleBackColor = true;
            // 
            // btnFMap
            // 
            this.btnFMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFMap.Location = new System.Drawing.Point(342, 53);
            this.btnFMap.Name = "btnFMap";
            this.btnFMap.Size = new System.Drawing.Size(72, 23);
            this.btnFMap.TabIndex = 10;
            this.btnFMap.Text = "Z mapy";
            this.btnFMap.UseVisualStyleBackColor = true;
            this.btnFMap.Click += new System.EventHandler(this.btnFMap_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(342, 82);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(72, 23);
            this.btnCopy.TabIndex = 11;
            this.btnCopy.Text = "Kopiuj";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyAll.Location = new System.Drawing.Point(342, 111);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(72, 23);
            this.btnCopyAll.TabIndex = 12;
            this.btnCopyAll.Text = "Kopiuj All";
            this.btnCopyAll.UseVisualStyleBackColor = true;
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // rb1
            // 
            this.rb1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rb1.AutoSize = true;
            this.rb1.Checked = true;
            this.rb1.Location = new System.Drawing.Point(141, 193);
            this.rb1.Name = "rb1";
            this.rb1.Size = new System.Drawing.Size(46, 17);
            this.rb1.TabIndex = 13;
            this.rb1.TabStop = true;
            this.rb1.Text = "Blok";
            this.rb1.UseVisualStyleBackColor = true;
            this.rb1.CheckedChanged += new System.EventHandler(this.rb1_CheckedChanged);
            // 
            // rb2
            // 
            this.rb2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rb2.AutoSize = true;
            this.rb2.Location = new System.Drawing.Point(185, 193);
            this.rb2.Name = "rb2";
            this.rb2.Size = new System.Drawing.Size(54, 17);
            this.rb2.TabIndex = 14;
            this.rb2.Text = "Okrag";
            this.rb2.UseVisualStyleBackColor = true;
            this.rb2.CheckedChanged += new System.EventHandler(this.rb2_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(264, 169);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Zapisz";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoadTxt
            // 
            this.btnLoadTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadTxt.Location = new System.Drawing.Point(264, 111);
            this.btnLoadTxt.Name = "btnLoadTxt";
            this.btnLoadTxt.Size = new System.Drawing.Size(72, 23);
            this.btnLoadTxt.TabIndex = 16;
            this.btnLoadTxt.Text = "Wczytaj txt";
            this.btnLoadTxt.UseVisualStyleBackColor = true;
            this.btnLoadTxt.Click += new System.EventHandler(this.btnLoadTxt_Click);
            // 
            // tB
            // 
            this.tB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tB.LargeChange = 1;
            this.tB.Location = new System.Drawing.Point(6, 6);
            this.tB.Maximum = 8;
            this.tB.Minimum = 1;
            this.tB.Name = "tB";
            this.tB.Size = new System.Drawing.Size(409, 45);
            this.tB.TabIndex = 18;
            this.tB.Value = 2;
            this.tB.Scroll += new System.EventHandler(this.tB_Scroll);
            // 
            // btnSelectObj
            // 
            this.btnSelectObj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectObj.ForeColor = System.Drawing.Color.Black;
            this.btnSelectObj.Location = new System.Drawing.Point(264, 140);
            this.btnSelectObj.Name = "btnSelectObj";
            this.btnSelectObj.Size = new System.Drawing.Size(72, 23);
            this.btnSelectObj.TabIndex = 19;
            this.btnSelectObj.Text = "Z selekcji";
            this.btnSelectObj.UseVisualStyleBackColor = true;
            this.btnSelectObj.Click += new System.EventHandler(this.btnSelectObj_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnUp.Location = new System.Drawing.Point(264, 256);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(72, 23);
            this.btnUp.TabIndex = 20;
            this.btnUp.Text = "/\\";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnDown.Location = new System.Drawing.Point(343, 256);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(72, 23);
            this.btnDown.TabIndex = 21;
            this.btnDown.Text = "\\/";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnDelBloks
            // 
            this.btnDelBloks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelBloks.ForeColor = System.Drawing.Color.Black;
            this.btnDelBloks.Location = new System.Drawing.Point(343, 227);
            this.btnDelBloks.Name = "btnDelBloks";
            this.btnDelBloks.Size = new System.Drawing.Size(71, 23);
            this.btnDelBloks.TabIndex = 23;
            this.btnDelBloks.Text = "Czyść bloki";
            this.btnDelBloks.UseVisualStyleBackColor = true;
            this.btnDelBloks.Click += new System.EventHandler(this.btnDelBloks_Click);
            // 
            // chDel
            // 
            this.chDel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chDel.Checked = true;
            this.chDel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chDel.Location = new System.Drawing.Point(8, 31);
            this.chDel.Name = "chDel";
            this.chDel.Size = new System.Drawing.Size(213, 18);
            this.chDel.TabIndex = 24;
            this.chDel.Text = "Czyść bloki automatycznie";
            this.chDel.UseVisualStyleBackColor = true;
            // 
            // tbScale
            // 
            this.tbScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbScale.Location = new System.Drawing.Point(10, 302);
            this.tbScale.Name = "tbScale";
            this.tbScale.Size = new System.Drawing.Size(60, 20);
            this.tbScale.TabIndex = 25;
            this.tbScale.Text = "1";
            this.tbScale.TextChanged += new System.EventHandler(this.tbScale_TextChanged);
            // 
            // lblScale
            // 
            this.lblScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblScale.AutoSize = true;
            this.lblScale.Location = new System.Drawing.Point(7, 281);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(63, 13);
            this.lblScale.TabIndex = 26;
            this.lblScale.Text = "Skala bloku";
            this.lblScale.Click += new System.EventHandler(this.lblScale_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.LargeChange = 2;
            this.trackBar1.Location = new System.Drawing.Point(76, 285);
            this.trackBar1.Maximum = 20;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(339, 45);
            this.trackBar1.TabIndex = 27;
            this.trackBar1.Value = 10;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(6, 33);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(253, 246);
            this.tabControl1.TabIndex = 28;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblidx);
            this.tabPage1.Controls.Add(this.lblCounter);
            this.tabPage1.Controls.Add(this.lstMarkers);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(245, 220);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "XYZ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblidxID);
            this.tabPage2.Controls.Add(this.lblCounterID);
            this.tabPage2.Controls.Add(this.btnDelFiltr);
            this.tabPage2.Controls.Add(this.btnFiltr);
            this.tabPage2.Controls.Add(this.tbFiltr);
            this.tabPage2.Controls.Add(this.lstMarkersID);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(245, 220);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ID";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnDelFiltr
            // 
            this.btnDelFiltr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelFiltr.BackColor = System.Drawing.Color.Transparent;
            this.btnDelFiltr.Location = new System.Drawing.Point(145, 193);
            this.btnDelFiltr.Name = "btnDelFiltr";
            this.btnDelFiltr.Size = new System.Drawing.Size(47, 22);
            this.btnDelFiltr.TabIndex = 5;
            this.btnDelFiltr.Text = "Kasuj filtr";
            this.btnDelFiltr.UseVisualStyleBackColor = false;
            this.btnDelFiltr.Click += new System.EventHandler(this.btnDelFiltr_Click);
            // 
            // btnFiltr
            // 
            this.btnFiltr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFiltr.Location = new System.Drawing.Point(195, 193);
            this.btnFiltr.Name = "btnFiltr";
            this.btnFiltr.Size = new System.Drawing.Size(47, 22);
            this.btnFiltr.TabIndex = 4;
            this.btnFiltr.Text = "Filtruj";
            this.btnFiltr.UseVisualStyleBackColor = true;
            this.btnFiltr.Click += new System.EventHandler(this.btnFiltr_Click);
            // 
            // tbFiltr
            // 
            this.tbFiltr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFiltr.Location = new System.Drawing.Point(5, 194);
            this.tbFiltr.Name = "tbFiltr";
            this.tbFiltr.Size = new System.Drawing.Size(136, 20);
            this.tbFiltr.TabIndex = 3;
            // 
            // lstMarkersID
            // 
            this.lstMarkersID.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMarkersID.FormattingEnabled = true;
            this.lstMarkersID.Location = new System.Drawing.Point(5, 4);
            this.lstMarkersID.Name = "lstMarkersID";
            this.lstMarkersID.Size = new System.Drawing.Size(234, 173);
            this.lstMarkersID.TabIndex = 2;
            this.lstMarkersID.SelectedIndexChanged += new System.EventHandler(this.lstMarkersID_SelectedIndexChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chFiltr);
            this.tabPage3.Controls.Add(this.chkGo);
            this.tabPage3.Controls.Add(this.chReverse);
            this.tabPage3.Controls.Add(this.chDel);
            this.tabPage3.Controls.Add(this.chShow);
            this.tabPage3.Controls.Add(this.rb2);
            this.tabPage3.Controls.Add(this.rb1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(245, 220);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Ustawienia";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chFiltr
            // 
            this.chFiltr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chFiltr.Location = new System.Drawing.Point(8, 79);
            this.chFiltr.Name = "chFiltr";
            this.chFiltr.Size = new System.Drawing.Size(213, 18);
            this.chFiltr.TabIndex = 25;
            this.chFiltr.Text = "Dokładne dopasowanie filtra";
            this.chFiltr.UseVisualStyleBackColor = true;
            this.chFiltr.CheckedChanged += new System.EventHandler(this.chFiltr_CheckedChanged);
            // 
            // btnSwap
            // 
            this.btnSwap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSwap.ForeColor = System.Drawing.Color.Black;
            this.btnSwap.Location = new System.Drawing.Point(264, 198);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(72, 23);
            this.btnSwap.TabIndex = 29;
            this.btnSwap.Text = "X <N> Y";
            this.btnSwap.UseVisualStyleBackColor = true;
            this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
            // 
            // btnSwapAll
            // 
            this.btnSwapAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSwapAll.ForeColor = System.Drawing.Color.Black;
            this.btnSwapAll.Location = new System.Drawing.Point(264, 227);
            this.btnSwapAll.Name = "btnSwapAll";
            this.btnSwapAll.Size = new System.Drawing.Size(72, 23);
            this.btnSwapAll.TabIndex = 30;
            this.btnSwapAll.Text = "X <ALL> Y";
            this.btnSwapAll.UseVisualStyleBackColor = true;
            this.btnSwapAll.Click += new System.EventHandler(this.btnSwapAll_Click);
            // 
            // lblCounter
            // 
            this.lblCounter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCounter.Location = new System.Drawing.Point(7, 204);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(158, 13);
            this.lblCounter.TabIndex = 31;
            this.lblCounter.Text = "Liczba elementów: ";
            // 
            // lblCounterID
            // 
            this.lblCounterID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCounterID.Location = new System.Drawing.Point(4, 178);
            this.lblCounterID.Name = "lblCounterID";
            this.lblCounterID.Size = new System.Drawing.Size(155, 13);
            this.lblCounterID.TabIndex = 32;
            this.lblCounterID.Text = "Liczba elementów: ";
            // 
            // lblidxID
            // 
            this.lblidxID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblidxID.Location = new System.Drawing.Point(165, 177);
            this.lblidxID.Name = "lblidxID";
            this.lblidxID.Size = new System.Drawing.Size(74, 13);
            this.lblidxID.TabIndex = 33;
            this.lblidxID.Text = "idx: ";
            // 
            // lblidx
            // 
            this.lblidx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblidx.Location = new System.Drawing.Point(165, 204);
            this.lblidx.Name = "lblidx";
            this.lblidx.Size = new System.Drawing.Size(74, 13);
            this.lblidx.TabIndex = 34;
            this.lblidx.Text = "idx: ";
            // 
            // ucMarkery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.btnSwapAll);
            this.Controls.Add(this.btnSwap);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.lblScale);
            this.Controls.Add(this.tbScale);
            this.Controls.Add(this.btnDelBloks);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnSelectObj);
            this.Controls.Add(this.btnLoadTxt);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCopyAll);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnFMap);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnDelAll);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.tB);
            this.MinimumSize = new System.Drawing.Size(423, 333);
            this.Name = "ucMarkery";
            this.Size = new System.Drawing.Size(423, 333);
            this.Load += new System.EventHandler(this.ucMarkery_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lstMarkers;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnDelAll;
        private System.Windows.Forms.CheckBox chkGo;
        private System.Windows.Forms.CheckBox chShow;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.CheckBox chReverse;
        private System.Windows.Forms.Button btnFMap;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnCopyAll;
        private System.Windows.Forms.RadioButton rb1;
        private System.Windows.Forms.RadioButton rb2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoadTxt;
        private System.Windows.Forms.TrackBar tB;
        private System.Windows.Forms.Button btnSelectObj;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnDelBloks;
        private System.Windows.Forms.CheckBox chDel;
        private System.Windows.Forms.TextBox tbScale;
        private System.Windows.Forms.Label lblScale;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lstMarkersID;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnFiltr;
        private System.Windows.Forms.TextBox tbFiltr;
        private System.Windows.Forms.Button btnDelFiltr;
        private System.Windows.Forms.Button btnSwap;
        private System.Windows.Forms.Button btnSwapAll;
        private System.Windows.Forms.CheckBox chFiltr;
        private System.Windows.Forms.Label lblCounter;
        private System.Windows.Forms.Label lblCounterID;
        private System.Windows.Forms.Label lblidx;
        private System.Windows.Forms.Label lblidxID;
    }
}
