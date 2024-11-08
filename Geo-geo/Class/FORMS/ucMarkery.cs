using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Geo_geo.Class.FORMS {
    public partial class ucMarkery : UserControl {
        public ucMarkery() {

            InitializeComponent();
            loadList();
            loadList(1);



        }

        private void ucMarkery_Load(object sender, EventArgs e) {

        }

        private void tabControl1_Change(object sender, EventArgs e) {
            //printMessege($"{tabControl1.SelectedIndex}");

            loadList();
        }

        private void btnGo_Click(object sender, EventArgs e) {

            bool reverse = !this.chReverse.Checked;
            bool show = this.chShow.Checked;

            bool blok = rb1.Checked;

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            try {
                string value = currentLstValue();

                double zoom = Convert.ToDouble(this.tB.Value);

                cSimple cS = new cSimple();
                cS.GoToMarker(value, reverse, show, blok, zoom, this.chDel.Checked, double.Parse(this.tbScale.Text));
            } catch (Exception ex) {

                ed.WriteMessage("\nNie można wczytać\n");

            }


        }



        private void btnDelAll_Click(object sender, EventArgs e) {

            if (tabControl1.SelectedIndex == 0) {

                string fileName = "C:\\Users\\Public\\Documents\\acMarkers.txt";

                if (System.IO.File.Exists(fileName)) {
                    System.IO.File.WriteAllText(fileName, "");
                }

                this.lstMarkers.Items.Clear();

            } else if (tabControl1.SelectedIndex == 1) {

                string fileName = "C:\\Users\\Public\\Documents\\acMarkersID.txt";

                if (System.IO.File.Exists(fileName)) {
                    System.IO.File.WriteAllText(fileName, "");
                }

                this.lstMarkersID.Items.Clear();

            }
        }

        private void btnReload_Click(object sender, EventArgs e) {
            loadList();
            loadList(1);

        }

        private void loadList(int idx = 0, string filtr = "") {

            string fileName = "";
            string fileContent = "";
            string[] lines;


            int j = 0;


            if ((tabControl1.SelectedIndex == 1) || (idx == 1)) {

                fileName = "C:\\Users\\Public\\Documents\\acMarkersID.txt";

                


                if (System.IO.File.Exists(fileName)) {


                    using (StreamReader sr = new StreamReader(fileName)) {
                        fileContent = sr.ReadToEnd();
                    }

                    lines = fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    this.lstMarkersID.Items.Clear();

                    j = 0;

                    for (int i = 0; i < lines.Length; i++) {

                        string item = lines[i];
                        if (item == "") {
                            continue;
                        }

                        if (filtr != "") {
                            string[] temp = item.Split(' ');

                            if (this.chFiltr.Checked) {
                                if (temp[0] != filtr) {
                                    continue;
                                }

                            } else {
                                if (!temp[0].Contains(filtr)) {
                                    continue;
                                }
                            }


                        }

                        this.lstMarkersID.Items.Insert(j, item);
                        j++;
                    }

                    lblCounterID.Text = $"Liczba elementów: {j}";
                } else {
                    System.IO.File.WriteAllText(fileName, "");
                }

            } else {


                fileName = "C:\\Users\\Public\\Documents\\acMarkers.txt";


                if (System.IO.File.Exists(fileName)) {


                    using (StreamReader sr = new StreamReader(fileName)) {
                        fileContent = sr.ReadToEnd();
                    }

                    lines = fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    this.lstMarkers.Items.Clear();

                    j = 0;

                    for (int i = 0; i < lines.Length; i++) {

                        string item = lines[i];
                        if (item == "") {
                            continue;
                        }
                        this.lstMarkers.Items.Insert(j, item);
                        j++;
                    }

                    lblCounter.Text = $"Liczba elementów: {j}";
                } else {
                    System.IO.File.WriteAllText(fileName, "");
                }
            }

            

        }

        private void btnPaste_Click(object sender, EventArgs e) {

            loadList(filtr: "");

            GetClipboard();
            loadList();

        }



        private void btnFMap_Click(object sender, EventArgs e) {
            cSimple cO = new Geo_geo.Class.cSimple();

            bool reverse = !this.chReverse.Checked;
            if (reverse) {
                cO.get_cid("xyh");
            } else {
                cO.get_cid("yxh");
            }

            loadList(filtr: "");

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

            if (tabControl1.SelectedIndex == 0) {

                string fileName = $"C:\\Users\\Public\\Documents\\acMarkers.txt";

                int j = 0;



                if (System.IO.File.Exists(fileName)) {
                    using (StreamWriter streamWriter = File.AppendText(fileName)) {

                        for (int i = 0; i < lines.Length; i++) {

                            try {

                                string item = lines[i].Replace(' ', '\t');

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
                    ed.WriteMessage($"\nWczytano punktów: {j}\n");
                }
            } else if (tabControl1.SelectedIndex == 1) {
                string fileName = $"C:\\Users\\Public\\Documents\\acMarkersID.txt";

                int j = 0;



                if (System.IO.File.Exists(fileName)) {
                    using (StreamWriter streamWriter = File.AppendText(fileName)) {

                        for (int i = 0; i < lines.Length; i++) {

                            try {

                                string item = lines[i].Replace(' ', '\t');

                                if (item.Contains(".") && item.Contains(",")) {
                                    item = item.Replace(',', '\t');

                                } else {
                                    item = item.Replace(',', '.');
                                }

                                while (item.Contains("\t\t")) {

                                    item = item.Replace("\t\t", "\t");

                                }



                                string[] xyh = item.Split('\t');

                                string id = "";

                                if (xyh.Length > 2) {
                                    id = xyh[0];
                                } else {
                                    id = "X";
                                }


                                double x = double.Parse(xyh[1]);
                                double y = double.Parse(xyh[2]);
                                double h = 0.0;

                                if (xyh.Length > 3) { h = double.Parse(xyh[3]); }

                                streamWriter.WriteLine($"{id} {x:f4} {y:f4} {h:f4}");
                                j++;
                            } catch { continue; }
                        }
                    }
                    ed.WriteMessage($"\nWczytano punktów: {j}\n");
                }



            }

        }

        private void btnDel_Click(object sender, EventArgs e) {

            if (tabControl1.SelectedIndex == 0) {

                this.lstMarkers.Items.RemoveAt(this.lstMarkers.SelectedIndex);

                string fileName = $"C:\\Users\\Public\\Documents\\acMarkers.txt";

                using (StreamWriter streamWriter = File.CreateText(fileName)) {

                    for (int i = 0; i < this.lstMarkers.Items.Count; i++) {

                        streamWriter.WriteLine(this.lstMarkers.Items[i]);

                    }

                }
            } else if (tabControl1.SelectedIndex == 1) {

                this.lstMarkersID.Items.RemoveAt(this.lstMarkersID.SelectedIndex);

                string fileName = $"C:\\Users\\Public\\Documents\\acMarkersID.txt";

                using (StreamWriter streamWriter = File.CreateText(fileName)) {

                    for (int i = 0; i < this.lstMarkersID.Items.Count; i++) {

                        streamWriter.WriteLine(this.lstMarkersID.Items[i]);

                    }

                }

            }

        }

        private void btnCopy_Click(object sender, EventArgs e) {


            string value = currentLstValue();

            value = value.Replace(" ", "\t");

            Clipboard.SetText($"{value}");

        }

        private void btnCopyAll_Click(object sender, EventArgs e) {

            string value = "";

            if (tabControl1.SelectedIndex == 0) {

                for (int i = 0; i < this.lstMarkers.Items.Count; i++) {

                    if (this.lstMarkers.Items[i].ToString() == "") {
                        continue;
                    }
                    value = $"{value}{this.lstMarkers.Items[i]}{Environment.NewLine}";

                }
            } else if (tabControl1.SelectedIndex == 1) {

                for (int i = 0; i < this.lstMarkersID.Items.Count; i++) {

                    if (this.lstMarkersID.Items[i].ToString() == "") {
                        continue;
                    }
                    value = $"{value}{this.lstMarkersID.Items[i]}{Environment.NewLine}";

                }
            }

            Clipboard.SetText($"{value}");

        }

        private void lstMarkers_SelectedIndexChanged(object sender, EventArgs e) {


            this.lblidx.Text = $"idx: {this.lstMarkers.SelectedIndex}";



            if (chkGo.Checked) {

                bool reverse = !this.chReverse.Checked;
                bool show = this.chShow.Checked;

                bool blok = rb1.Checked;

                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;

                try {
                    string value = currentLstValue();

                    double zoom = Convert.ToDouble(this.tB.Value);

                    cSimple cS = new cSimple();
                    cS.GoToMarker(value, reverse, show, blok, zoom, this.chDel.Checked, double.Parse(this.tbScale.Text));


                } catch (Exception ex) {

                    ed.WriteMessage("\nNie można wczytać\n");
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

            } else {

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

            if (tabControl1.SelectedIndex == 0) {

                for (int i = 0; i < this.lstMarkers.Items.Count; i++) {

                    if (this.lstMarkers.Items[i].ToString() == "") {
                        continue;
                    }

                    value = $"{value}{this.lstMarkers.Items[i]}{Environment.NewLine}";

                }
            } else if (tabControl1.SelectedIndex == 1) {

                for (int i = 0; i < this.lstMarkersID.Items.Count; i++) {

                    if (this.lstMarkersID.Items[i].ToString() == "") {
                        continue;
                    }

                    value = $"{value}{this.lstMarkersID.Items[i]}{Environment.NewLine}";

                }

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

            loadList(filtr: "");

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
            int j = 0;

            if (tabControl1.SelectedIndex == 0) {
                fileName = $"C:\\Users\\Public\\Documents\\acMarkers.txt";


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
                } else if (tabControl1.SelectedIndex == 1) {

                    fileName = $"C:\\Users\\Public\\Documents\\acMarkersID.txt";


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

                                    string id = xyh[0];
                                    double x = double.Parse(xyh[0]);
                                    double y = double.Parse(xyh[1]);
                                    double h = 0.0;

                                    if (xyh.Length > 2) { h = double.Parse(xyh[2]); }

                                    streamWriter.WriteLine($"{id} {x:f4} {y:f4} {h:f4}");
                                    j++;
                                } catch { continue; }
                            }
                        }


                    }
                }
                ed.WriteMessage($"\nWczytano punktów: {j}\n");
            }

            loadList();
        }

        private void chkGo_CheckedChanged(object sender, EventArgs e) {

        }

        private void btnSelectObj_Click(object sender, EventArgs e) {

            bool reverse = !this.chReverse.Checked;

            cExportPik exportPik = new cExportPik();

            List<string> lines = new List<string>();

            bool withName = (tabControl1.SelectedIndex == 1);
            lines = exportPik.ExportPointsToList(reverse = reverse, withName);


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
                    string value = currentLstValue();

                    double zoom = Convert.ToDouble(this.tB.Value);

                    cSimple cS = new cSimple();
                    cS.GoToMarker(value, reverse, false, false, zoom, this.chDel.Checked, double.Parse(this.tbScale.Text));
                } catch (Exception ex) {

                    ed.WriteMessage("\nNie można wczytać\n");
                }
            }

        }

        private void btnUp_Click(object sender, EventArgs e) {

            try {
                if (tabControl1.SelectedIndex == 0) {
                    this.lstMarkers.SelectedIndex = this.lstMarkers.SelectedIndex - 1;
                } else if (tabControl1.SelectedIndex == 1) {
                    this.lstMarkersID.SelectedIndex = this.lstMarkersID.SelectedIndex - 1;
                }

            } catch (Exception ex) {
            }
        }

        private void btnDown_Click(object sender, EventArgs e) {
            try {
                if (tabControl1.SelectedIndex == 0) {
                    this.lstMarkers.SelectedIndex = this.lstMarkers.SelectedIndex + 1;
                } else if (tabControl1.SelectedIndex == 1) {
                    this.lstMarkersID.SelectedIndex = this.lstMarkersID.SelectedIndex + 1;
                }
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
            } else {
                this.lblScale.Visible = false;
                this.tbScale.Visible = false;
                this.trackBar1.Visible = false;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e) {

            double sc = this.trackBar1.Value;

            if (sc >= 10) {
                sc = sc - 9;
            } else {

                sc = sc / 10;
            }

            this.tbScale.Text = $"{sc}";

            //resizeBlock();

        }

        private void resizeBlock() {

            try {

                bool reverse = !this.chReverse.Checked;

                string value = currentLstValue();
                double zoom = Convert.ToDouble(this.tB.Value);
                cSimple cS = new cSimple();
                bool blok = rb1.Checked;
                bool show = this.chShow.Checked;


                cS.GoToMarker(value, reverse, show, blok, zoom, this.chDel.Checked, double.Parse(this.tbScale.Text));

            } catch (Exception ex) { }

        }

        private void printMessege(string messege) {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            ed.WriteMessage($"\n{messege}\n");

        }

        private string currentLstValue(bool raw = false) {
            string result = "";

            // printMessege($"{tabControl1.SelectedIndex}");



            if (tabControl1.SelectedIndex == 1) {
                string temp = this.lstMarkersID.GetItemText(this.lstMarkersID.SelectedItem);

                if (raw) { return temp; }

                string[] splited = temp.Split(' ');

                result = $"{splited[1]} {splited[2]} {splited[3]}";

            } else if (tabControl1.SelectedIndex == 0) {
                result = this.lstMarkers.GetItemText(this.lstMarkers.SelectedItem);
            }

            //printMessege($"{tabControl1.SelectedIndex}:\t{result}:");


            return result;
        }

        private void lstMarkersID_SelectedIndexChanged(object sender, EventArgs e) {

            this.lblidxID.Text = $"idx: {this.lstMarkersID.SelectedIndex}";

            if (chkGo.Checked) {

                bool reverse = !this.chReverse.Checked;
                bool show = this.chShow.Checked;

                bool blok = rb1.Checked;

                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;

                try {
                    string value = currentLstValue();

                    double zoom = Convert.ToDouble(this.tB.Value);

                    cSimple cS = new cSimple();
                    cS.GoToMarker(value, reverse, show, blok, zoom, this.chDel.Checked, double.Parse(this.tbScale.Text));
                } catch (Exception ex) {

                    ed.WriteMessage($"\nNie można wczytać\n");
                }
            }

        }

        private void lblScale_Click(object sender, EventArgs e) {

        }

        private void btnFiltr_Click(object sender, EventArgs e) {
            loadList(filtr: this.tbFiltr.Text);
        }

        private void btnDelFiltr_Click(object sender, EventArgs e) {
            loadList(filtr: "");
            this.tbFiltr.Text = "";
        }

        private void tbScale_TextChanged(object sender, EventArgs e) {
            resizeBlock();
        }

        private void btnSwap_Click(object sender, EventArgs e) {

            string value = currentLstValue(raw: true);
            string[] spl = value.Split(' ');

            if (tabControl1.SelectedIndex == 0) {


                value = $"{spl[1]} {spl[0]} {spl[2]}";

                int idx = this.lstMarkers.SelectedIndex;
                this.lstMarkers.Items.RemoveAt(idx);
                this.lstMarkers.Items.Insert(idx, value);
                this.lstMarkers.SelectedIndex = idx;

                string fileName = $"C:\\Users\\Public\\Documents\\acMarkers.txt";

                using (StreamWriter streamWriter = File.CreateText(fileName)) {

                    for (int i = 0; i < this.lstMarkers.Items.Count; i++) {

                        streamWriter.WriteLine(this.lstMarkers.Items[i]);

                    }

                }
            } else if (tabControl1.SelectedIndex == 1) {

                value = $"{spl[0]} {spl[2]} {spl[1]} {spl[3]}";

                int idx = this.lstMarkersID.SelectedIndex;
                this.lstMarkersID.Items.RemoveAt(idx);
                this.lstMarkersID.Items.Insert(idx, value);
                this.lstMarkersID.SelectedIndex = idx;

                string fileName = $"C:\\Users\\Public\\Documents\\acMarkersID.txt";

                using (StreamWriter streamWriter = File.CreateText(fileName)) {

                    for (int i = 0; i < this.lstMarkersID.Items.Count; i++) {

                        streamWriter.WriteLine(this.lstMarkersID.Items[i]);

                    }

                }

            }

        }

        private void btnSwapAll_Click(object sender, EventArgs e) {


            if (tabControl1.SelectedIndex == 0) {

                for (int i = 0; i < this.lstMarkers.Items.Count; i++) {

                    string[] spl = this.lstMarkers.Items[i].ToString().Split(' ');
                    this.lstMarkers.Items[i] = $"{spl[1]} {spl[0]} {spl[2]}";
                }

                string fileName = $"C:\\Users\\Public\\Documents\\acMarkers.txt";

                using (StreamWriter streamWriter = File.CreateText(fileName)) {

                    for (int i = 0; i < this.lstMarkers.Items.Count; i++) {

                        streamWriter.WriteLine(this.lstMarkers.Items[i]);

                    }
                }


            } else if (tabControl1.SelectedIndex == 1) {
                for (int i = 0; i < this.lstMarkersID.Items.Count; i++) {

                    string[] spl = this.lstMarkersID.Items[i].ToString().Split(' ');
                    this.lstMarkersID.Items[i] = $"{spl[0]} {spl[2]} {spl[1]} {spl[3]}";

                }

                string fileName = $"C:\\Users\\Public\\Documents\\acMarkersID.txt";

                using (StreamWriter streamWriter = File.CreateText(fileName)) {

                    for (int i = 0; i < this.lstMarkersID.Items.Count; i++) {

                        streamWriter.WriteLine(this.lstMarkersID.Items[i]);

                    }

                }
            }
        }

        private void chFiltr_CheckedChanged(object sender, EventArgs e) {

        }
    }

}
