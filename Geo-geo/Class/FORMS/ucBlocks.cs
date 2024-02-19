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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Geo_geo.Class.FORMS {
    public partial class ucBlocks : UserControl {
        public ucBlocks() {
            InitializeComponent();


            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int i = 0;

            List<string> blocks = new List<string>();

            using (Transaction trans = db.TransactionManager.StartTransaction()) {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                foreach (ObjectId btrId in bt) {
                    BlockTableRecord btr = trans.GetObject(btrId, OpenMode.ForRead) as BlockTableRecord;

                    if (btr.IsLayout || btr.IsAnonymous || btr.IsFromExternalReference)
                        continue;

                    blocks.Add(btr.Name.ToString());

                }

                foreach (string bl in blocks) {

                    cboBlok.Items.Insert(i, $"{bl}");
                    ed.WriteMessage($"Block: {bl}\n");
                    i++;
                }


            }

            cboBlok.SelectedIndex = 0;

        }

        private void cboBlok_SelectedIndexChanged(object sender, EventArgs e) {


        }

        private void ucBlocks_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int i = 0;

            List<string> blocks = new List<string>();

            string chosen = this.cboBlok.GetItemText(this.cboBlok.SelectedItem);

            using (Transaction trans = db.TransactionManager.StartTransaction()) {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForWrite) as BlockTable;

                foreach (ObjectId btrId in bt) {
                    BlockTableRecord btr = trans.GetObject(btrId, OpenMode.ForWrite) as BlockTableRecord;

                    if (btr.IsLayout || btr.IsAnonymous || btr.IsFromExternalReference)
                        continue;

                    if (btr.Name.ToString() == chosen) {
                        Scale3d blkScale = new Scale3d(1.0, 1.0, 1.0);
                        ObjectId bdId = bt[btr.Name.ToString()];
                        Point3d pt = new Point3d(0, 0, 0);

                        BlockReference insblkref = new BlockReference(pt, bdId);
                        insblkref.ScaleFactors = blkScale;
                        insblkref.Rotation = 0;

                        trans.AddNewlyCreatedDBObject(insblkref, true);
                        trans.Commit();


                    }
                }
            }
        }
    }
}
