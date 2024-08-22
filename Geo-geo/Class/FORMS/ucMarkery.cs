using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace Geo_geo.Class.FORMS {
    public partial class ucMarkery : UserControl {
        public ucMarkery() {
            InitializeComponent();
            loadList();



        }

        private void ucMarkery_Load(object sender, EventArgs e) {

        }

        private void btnGo_Click(object sender, EventArgs e) {

            bool reverse = !this.chReverse.Checked;
            bool show = this.chShow.Checked;

            bool blok = rb1.Checked;

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            try {
                string value = this.lstMarkers.GetItemText(this.lstMarkers.SelectedItem);

                double zoom = Convert.ToDouble(this.tB.Value);

                cSimple cS = new cSimple();
                cS.GoToMarker(value, reverse, show, blok, zoom, this.chDel.Checked, double.Parse(this.tbScale.Text));
            }
            catch (Exception ex) {

                ed.WriteMessage("\nNie można wczytać");

            }
           

        }

   

        private void btnDelAll_Click(object sender, EventArgs e) {

            string fileName = "C:\\Users\\Public\\Documents\\acMarkers.txt";

            if (System.IO.File.Exists(fileName)) {
                System.IO.File.WriteAllText(fileName, "");
            }

            this.lstMarkers.Items.Clear();
        }

        private void btnReload_Click(object sender, EventArgs e) {
            loadList();

        }

        private void loadList() {

            string fileContent = "";
            string fileName = "C:\\Users\\Public\\Documents\\acMarkers.txt";
            string[] lines;


            if (System.IO.File.Exists(fileName)) {


                using (StreamReader sr = new StreamReader(fileName)) {
                    fileContent = sr.ReadToEnd();
                }

                lines = fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                this.lstMarkers.Items.Clear();

                for (int i = 0; i < lines.Length; i++) {

                    string item = lines[i];
                    if (item == "") {
                        continue;
                    }
                    this.lstMarkers.Items.Insert(i, item);
                }
            } else {
                System.IO.File.WriteAllText(fileName, "");
            }

        }

        private void btnPaste_Click(object sender, EventArgs e) {



            GetClipboard();
            loadList();

        }



        private void btnFMap_Click(object sender, EventArgs e) {
            cSimple cO = new Geo_geo.Class.cSimple();

            bool reverse = !this.chReverse.Checked;
            if (reverse) {
                cO.get_cid("xyh");
            }
            else {
                cO.get_cid("yxh");
            }
           
            GetClipboard();
            loadList();
        }

        private void GetClipboard(string result = "") {

            string[] lines;

            //string result = "";

            if (result == "") {
                result = Clipboard.GetText();
            }

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            //ed.WriteMessage(result);

            //lines = result.Split('\n');

            lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            string fileName = $"C:\\Users\\Public\\Documents\\acMarkers.txt";

            int j = 0;



            if (System.IO.File.Exists(fileName)) {
                using (StreamWriter streamWriter = File.AppendText(fileName)) {

                    for (int i = 0; i < lines.Length; i++) {

                        try {

                            string item = lines[i].Replace(' ', '\t');

                            if (item.Contains(".") && item.Contains(",")) {
                                item = item.Replace(',', '\t');

                            }
                            else {
                                item = item.Replace(',', '.');
                            }

                            while (item.Contains("\t\t")) {

                                item = item.Replace("\t\t", "\t");

                            }
                            

                            string[] xyh = item.Split('\t');

                            double x = double.Parse(xyh[0]);
                            double y = double.Parse(xyh[1]);
                            double h = 0.0;

                            if (xyh.Length > 2) { h = double.Parse(xyh[2]); }

                            streamWriter.WriteLine($"{x:f4} {y:f4} {h:f4}");
                            j++;
                        } catch { continue; }
                    }
                }
                ed.WriteMessage($"\nWczytano punktów: {j}");
            }

        }

        private void btnDel_Click(object sender, EventArgs e) {

            this.lstMarkers.Items.RemoveAt(this.lstMarkers.SelectedIndex);

            string fileName = $"C:\\Users\\Public\\Documents\\acMarkers.txt";

            using (StreamWriter streamWriter = File.CreateText(fileName)) { 

                for (int i = 0; i < this.lstMarkers.Items.Count; i++) {


                    streamWriter.WriteLine(this.lstMarkers.Items[i]);


                }

            }

        }

        private void btnCopy_Click(object sender, EventArgs e) {


            string value = this.lstMarkers.GetItemText(this.lstMarkers.SelectedItem);

            value = value.Replace(" ", "\t");

            Clipboard.SetText($"{value}");

        }

        private void btnCopyAll_Click(object sender, EventArgs e) {

            string value = "";

            for (int i = 0; i < this.lstMarkers.Items.Count; i++) {

                if (this.lstMarkers.Items[i].ToString() == "") {
                    continue;
                }


                value = $"{value}{this.lstMarkers.Items[i]}{Environment.NewLine}";

            }

            Clipboard.SetText($"{value}");


        }

        private void lstMarkers_SelectedIndexChanged(object sender, EventArgs e) {


            if (chkGo.Checked) {

                bool reverse = !this.chReverse.Checked;
                bool show = this.chShow.Checked;

                bool blok = rb1.Checked;

                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;

                try {
                    string value = this.lstMarkers.GetItemText(this.lstMarkers.SelectedItem);

                    double zoom = Convert.ToDouble(this.tB.Value);

                    cSimple cS = new cSimple();
                    cS.GoToMarker(value, reverse, show, blok, zoom, this.chDel.Checked, double.Parse(this.tbScale.Text));
                } catch (Exception ex) {

                    ed.WriteMessage("\nNie można wczytać");
                }
            }
        }

        private void chShow_CheckedChanged(object sender, EventArgs e) {

            if (this.chShow.Checked) {

                this.rb1.Visible = true;
                this.rb2.Visible = true;

                if (this.rb1.Checked) {

                    this.lblScale.Visible = true;
                    this.tbScale.Visible = true;
                    this.trackBar1.Visible = true;
                }

            }
            else {

                this.rb1.Visible = false;
                this.rb2.Visible = false;
                this.lblScale.Visible = false;
                this.tbScale.Visible = false;
                this.trackBar1.Visible = false;
            }


        }

        private void rb2_CheckedChanged(object sender, EventArgs e) {

        }

        private void btnSave_Click(object sender, EventArgs e) {

            string value = "";

            for (int i = 0; i < this.lstMarkers.Items.Count; i++) {

                if (this.lstMarkers.Items[i].ToString() == "") {
                    continue;
                }


                value = $"{value}{this.lstMarkers.Items[i]}{Environment.NewLine}";

            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string fileName = "";

            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Zapisz współrzedne do pliku";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK) {


                fileName = saveFileDialog.FileName;

                if (!fileName.EndsWith(".txt")) {
                    fileName = fileName + ".txt";
                }

                using (StreamWriter streamWriter = new StreamWriter(fileName)) {
                    streamWriter.Write($"{value}");
                    streamWriter.Close();
                }
            }

        }

        private void btnLoadTxt_Click(object sender, EventArgs e) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;


            string fileName = "";
            string result = string.Empty;
            string[] lines;

            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    fileName = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream)) {
                        result = reader.ReadToEnd();
                    }

                }
            }

            lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            fileName = $"C:\\Users\\Public\\Documents\\acMarkers.txt";

            int j = 0;

            if (System.IO.File.Exists(fileName)) {
                using (StreamWriter streamWriter = File.AppendText(fileName)) {

                    

                    for (int i = 0; i < lines.Length; i++) {

                        try {

                            string item = lines[i].Replace(' ', '\t');

                            item = item.Trim();
                            //item = lines[i].Replace('&nbsp;', '\t');

                            if (item.Contains(".") && item.Contains(",")) {
                                item = item.Replace(',', '\t');

                            } else {
                                item = item.Replace(',', '.');
                            }

                            while (item.Contains("\t\t")) {

                                item = item.Replace("\t\t", "\t");

                            }


                            string[] xyh = item.Split('\t');

                            double x = double.Parse(xyh[0]);
                            double y = double.Parse(xyh[1]);
                            double h = 0.0;

                            if (xyh.Length > 2) { h = double.Parse(xyh[2]); }

                            streamWriter.WriteLine($"{x:f4} {y:f4} {h:f4}");
                            j++;
                        } catch { continue; }
                    }
                }
                ed.WriteMessage($"\nWczytano punktów: {j}");
            }

            loadList();
        }

        private void chkGo_CheckedChanged(object sender, EventArgs e) {

        }

        private void btnSelectObj_Click(object sender, EventArgs e) {

            bool reverse = !this.chReverse.Checked;

            cExportPik exportPik = new cExportPik();

            List<string> lines = new List<string>();

            lines = exportPik.ExportPointsToList(reverse=reverse);


            foreach (string line in lines) {
                // TODO: code is really slow, but easy i can make it faster
                GetClipboard(line);
            }
            loadList();
        }

        private void tB_Scroll(object sender, EventArgs e) {

            if ((chkGo.Checked) && (lstMarkers.Items.Count > 0)) {

                bool reverse = !this.chReverse.Checked;
                bool show = this.chShow.Checked;

                bool blok = rb1.Checked;

                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;

                try {
                    string value = this.lstMarkers.GetItemText(this.lstMarkers.SelectedItem);

                    double zoom = Convert.ToDouble(this.tB.Value);

                    cSimple cS = new cSimple();
                    cS.GoToMarker(value, reverse, false, false, zoom, this.chDel.Checked, double.Parse(this.tbScale.Text));
                } catch (Exception ex) {

                    ed.WriteMessage("\nNie można wczytać");
                }
            }

        }

        private void btnUp_Click(object sender, EventArgs e) {

            try {
                this.lstMarkers.SelectedIndex = this.lstMarkers.SelectedIndex - 1;
            }
            catch (Exception ex) {
            }
        }

        private void btnDown_Click(object sender, EventArgs e) {
            try {
                this.lstMarkers.SelectedIndex = this.lstMarkers.SelectedIndex + 1;
            } catch (Exception ex) {
            }
        }

        private void btnDelBloks_Click(object sender, EventArgs e) {
            cSimple remover = new cSimple();
            remover.DeleteBlockByName("PIN2");
        }

        private void rb1_CheckedChanged(object sender, EventArgs e) {
            if (this.rb1.Checked) {

                this.lblScale.Visible = true;
                this.tbScale.Visible = true;
                this.trackBar1.Visible = true;
            }
            else {
                this.lblScale.Visible = false;
                this.tbScale.Visible = false;
                this.trackBar1.Visible = false;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e) {

            double sc = this.trackBar1.Value;

            if (sc >= 10) {
                sc = sc - 9;
            }
            else {

                sc = sc / 10;
            }

            this.tbScale.Text = $"{sc}";

            try {

                bool reverse = !this.chReverse.Checked;
                string value = this.lstMarkers.GetItemText(this.lstMarkers.SelectedItem);
                double zoom = Convert.ToDouble(this.tB.Value);
                cSimple cS = new cSimple();
                bool blok = rb1.Checked;
                bool show = this.chShow.Checked;


                cS.GoToMarker(value, reverse, show, blok, zoom, this.chDel.Checked, double.Parse(this.tbScale.Text));

            }
            catch (Exception ex) { }
        }
    }
}
