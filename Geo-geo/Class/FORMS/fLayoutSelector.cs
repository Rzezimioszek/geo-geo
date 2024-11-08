using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
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
using static Autodesk.AutoCAD.DatabaseServices.GripData;

namespace Geo_geo.Class.FORMS {
    public partial class fLayoutSelector : Form {

        private int desiredStartLocationX;
        private int desiredStartLocationY;
        private string defScale;
        private string defAcitive;


        public fLayoutSelector(int x, int y, string defScale, string defAcitive)
       : this() {
            // here store the value for x & y into instance variables
            this.desiredStartLocationX = x;
            this.desiredStartLocationY = y;

            Load += new EventHandler(fLayoutSelector_Load);
            this.defScale = defScale;
            this.defAcitive = defAcitive;
        }
        public string ReturnValue { get; set; }

        public fLayoutSelector() {
            InitializeComponent();
        }



        private void fLayoutSelector_Load(object sender, EventArgs e) {
            this.SetDesktopLocation(desiredStartLocationX, desiredStartLocationY);
            loadLayoutsList();

            this.cbScale.Items.Clear();
            this.cbScale.Items.Insert(0, "2:1");
            this.cbScale.Items.Insert(1, "1:1");
            this.cbScale.Items.Insert(2, "1:2");
            this.cbScale.Items.Insert(3, "1:5");
            this.cbScale.Items.Insert(4, "1:10");
            this.cbScale.Items.Insert(5, "1:20");
            this.cbScale.Items.Insert(6, "1:25");
            this.cbScale.Items.Insert(7, "1:50");
            this.cbScale.Items.Insert(8, "1:100");
            this.cbScale.Items.Insert(9, "Własne");
            //this.cbScale.SelectedIndex = 1;
            this.cbScale.SelectedIndex = formDefScaleToNormal();
            this.chActive.Checked = bool.Parse(this.defAcitive);
        }

        private int formDefScaleToNormal() {

            switch (this.defScale) {

                case "2":
                    return 0;
                case "1":
                    return 1;
                case "0.5":
                    return 2;
                case "0.2":
                    return 3;
                case "0.1":
                    return 4;
                case "0.05":
                    return 5;
                case "0.04":
                    return 6;
                case "0.02":
                    return 7;
                case "0.01":
                    return 8;
            }

            return 1;
        }



            

        private string getScale() {

            string current_scale = this.cbScale.SelectedItem.ToString();

            switch (current_scale) {
                case "2:1":
                    return "2";
                case "1:1":
                    return "1";
                case "1:2":
                    return "0.5";
                case "1:5":
                    return "0.2";
                case "1:10":
                    return "0.1";
                case "1:20":
                    return "0.05";
                case "1:25":
                    return "0.04";
                case "1:50":
                    return "0.02";
                case "1:100":
                    return "0.01";
                case "Własne":

                    try {


                        double temp = (1.0 / double.Parse(tbOwn.Text));
                        return $"{temp}";
                    }
                    catch { break; }
            }



            return "1";

        }

        private string getVPLock() {

            if (this.chVPLock.Checked) {
                return bool.TrueString;
            }

            return bool.FalseString;

        }

        private string getAcitve() {

            if (this.chActive.Checked) {
                return bool.TrueString;
            }

            return bool.FalseString;

        }

        private void button1_Click(object sender, EventArgs e) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            ReturnValue = $"{this.lbLayouts.GetItemText(this.lbLayouts.SelectedItem)};{getScale()};{getVPLock()};{getAcitve()}";
            // ed.WriteMessage($"\n{ReturnValue}\n");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }



        private void lbLayouts_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void saveSettings() {

            string fileName = "C:\\Users\\Public\\Documents\\acad_layout_settings.txt";
            using (StreamWriter streamWriter = File.CreateText(fileName)) {
                streamWriter.WriteLine(this.cbScale.SelectedItem.ToString());
                streamWriter.WriteLine(this.chVPLock.Checked);
            }
        }


        private void loadLayoutsList() {

            this.lbLayouts.Items.Clear();
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            LayoutManager layoutMgr = LayoutManager.Current;

            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                // https://adndevblog.typepad.com/autocad/2012/05/listing-the-layout-names.html

                DBDictionary layoutDic = tr.GetObject(db.LayoutDictionaryId, OpenMode.ForRead, false) as DBDictionary;
                Dictionary<int, string> olLayout = new Dictionary<int, string>();
                int j = 0;

                foreach (DBDictionaryEntry entry in layoutDic) {

                    ObjectId layoutId = entry.Value;
                    Layout layout = tr.GetObject(layoutId, OpenMode.ForRead) as Layout;
                    olLayout.Add(layout.TabOrder, layout.LayoutName); //this.lbLayouts.Items.Insert(j, layout.LayoutName);
                }

                for (int i = 1; i < olLayout.Count; i++) {this.lbLayouts.Items.Insert(i - 1, olLayout[i]);}

                try { this.lbLayouts.SelectedIndex = 0; }
                catch { }
                
                tr.Commit();
            }
        }

        private void chVPLock_CheckedChanged(object sender, EventArgs e) {
            saveSettings();
        }

        private void cbScale_SelectedIndexChanged(object sender, EventArgs e) {
            saveSettings();

            if (this.cbScale.SelectedItem.ToString() == "Własne") {
                lblOwn.Visible = true;
                tbOwn.Visible = true;
            }
            else {
                lblOwn.Visible = false;
                tbOwn.Visible = false;
            }
        }

        private void btnStop_Click(object sender, EventArgs e) {

            ReturnValue = "ForceStop!";
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void chActive_CheckedChanged(object sender, EventArgs e) {

        }
    }
}
