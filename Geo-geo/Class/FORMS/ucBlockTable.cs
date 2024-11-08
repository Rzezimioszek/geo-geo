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
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Geo_geo.Class.FORMS {
    public partial class ucBlockTable : UserControl {
        public ucBlockTable() {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {

        }

        private void ucBlockTable_Load(object sender, EventArgs e) {


            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            SelectionSet selectionSet = selectionResult.Value;

            int lp = 0;

            this.listView1.Columns.Add("lp", 20);

            foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity;
                    if (entity == null) {
                        continue;
                    }

                    string objType = entity.GetType().Name;
                    double wspXP = 0.0;
                    double wspYP = 0.0;
                    string atr;

                    string id = "";

                    List<string> elem = new List<string>();

                    int j = this.listView1.Columns.Count;
                    int k = 1;

                    if (objType == "BlockReference") {
                        BlockReference blockRef = entity as BlockReference;
                        wspXP = blockRef.Position.X;
                        wspYP = blockRef.Position.Y;

                        Autodesk.AutoCAD.DatabaseServices.AttributeCollection attCol = blockRef.AttributeCollection;

                        string str = "";

                        str = "";


                        foreach (ObjectId attId in attCol) {

                            AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);

                            if (attRef.Tag == "ID") { 
                                id = attRef.TextString;
                                k++;
                                if (k > j) {
                                    this.listView1.Columns.Add(attRef.Tag, -2);
                                }

                                elem.Add(attRef.TextString);
                            }
                            else {

                                k++;
                                if (k > j) {
                                    this.listView1.Columns.Add(attRef.Tag, -2);
                                }

                                elem.Add(attRef.TextString);
                            }



                            //str = (str +  attRef.Tag + "\t");


                            str = (str + attRef.TextString + "\t");

                            /* Przypisanie wartości pola "Pochodzenie" na podstawie id 
                            if ((attRef.Tag == "POCHODZENIE") && (id == "E-W33")){
                                AttributeReference attWrt = (AttributeReference)trans.GetObject(attId, OpenMode.ForWrite);
                                attWrt.TextString = $"var: {id}";
                            }
                            */

                        }

                        var values = elem.ToArray();

                        ListViewItem item = new ListViewItem($"{lp}");

                        foreach (var v in values) {

                            item.SubItems.Add(v);
                        }
                        

                        this.listView1.Items.Add(item);

                        lp++;

                    } else {
                        continue;
                    }

                    ed.WriteMessage($"\n{lp}");

                    trans.Commit();
                }
            }

        }
    }
}
