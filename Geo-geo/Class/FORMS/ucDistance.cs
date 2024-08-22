using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo_geo.Class.FORMS {
    public partial class ucDistance : UserControl {
        public ObjectId brId { get; private set; }
        public List<ObjectId> brIds { get; private set; }

        public ucDistance() {
            InitializeComponent();

        }

        private void btnTakeDistance_Click(object sender, EventArgs e) {

            double value = get_distance();
            double sum = double.Parse(this.txtSuma.Text);
            sum = sum + value;

            this.txtLast.Text = $"{value:0.00}";
            this.txtSuma.Text = $"{sum:0.00}";

            try {

                brIds.Add(brId);

            } catch {

                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;
                Editor ed = doc.Editor;

                ed.WriteMessage($"List Error");


            }



        }

        private void btnReset_Click(object sender, EventArgs e) {
            this.txtLast.Text = "0";
            this.txtSuma.Text = "0";

        }

        public double get_distance() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            double distance = 0;

            List<Point3d> pts = new List<Point3d>();

            try {

                while (true) {
                    PromptPointResult pPtRes = ed.GetPoint("Wskaż punkt\n");

                    if (pPtRes.Status == PromptStatus.Cancel) {
                        ed.WriteMessage("\nStoped");
                        break;
                    }

                    Point3d pt = pPtRes.Value;
                    pts.Add(pt);
                    ed.WriteMessage($"\nNew point at: {pt.X}\t{pt.Y}");
                }
            } catch { return 0.0; }


            Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();

            int i = 0;

            foreach (Point3d lpt in pts) {

                Point2d pt2d = new Point2d(lpt.X, lpt.Y);

                pline.AddVertexAt(i, pt2d, 0.0, 0.0, 0.0);

                i++;
            }

            if (this.chDraw.Checked) {

                using (DocumentLock acLckDoc = doc.LockDocument()) {
                    using (Transaction tr = db.TransactionManager.StartTransaction()) {
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                        brId = btr.AppendEntity(pline);
                        tr.AddNewlyCreatedDBObject(pline, true);
                        tr.Commit();


                    }

                }
            }

                ed.WriteMessage($"\n");
            ed.WriteMessage(pline.Length.ToString());

            return pline.Length;
        }

        private void chDraw_CheckedChanged(object sender, EventArgs e) {
            if (chDraw.Checked) {
                btnDelLast.Enabled = true;
                btnDelAll.Enabled = true;
            }
            else { 
                btnDelLast.Enabled = false;
                btnDelAll.Enabled = false;
            }
        }

        private void btnDelLast_Click(object sender, EventArgs e) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            try {
                using (DocumentLock acLckDoc = doc.LockDocument()) {
                    using (Transaction tr = db.TransactionManager.StartTransaction()) {
                        Entity ent = tr.GetObject(brId, OpenMode.ForWrite) as Entity;

                        ent.Erase();
                        tr.Commit();

                    }
                }
            }
            catch { }

        }

        private void ucDistance_Load(object sender, EventArgs e) {

        }

        private void btnDelAll_Click(object sender, EventArgs e) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            foreach (ObjectId lokId in brIds) {

                try {
                    using (DocumentLock acLckDoc = doc.LockDocument()) {
                        using (Transaction tr = db.TransactionManager.StartTransaction()) {
                            Entity ent = tr.GetObject(lokId, OpenMode.ForWrite) as Entity;

                            ent.Erase();
                            tr.Commit();

                        }
                    }
                    brIds.Remove(lokId);

                } catch { 
                }
            }

        }
    }
}
