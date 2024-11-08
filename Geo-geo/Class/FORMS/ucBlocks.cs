using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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

            string blockName = this.cboBlok.GetItemText(this.cboBlok.SelectedItem);

            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                if (!bt.Has(blockName))
                    throw new ArgumentException("Block definition not found", blockName);

                ObjectId blockId = bt[blockName];

                Entity entity = tr.GetObject(blockId, OpenMode.ForRead) as Entity;

                string objType = entity.GetType().Name;

                ed.WriteMessage($"\n{objType}\n");


            }
        }

        private void button1_Click_1(object sender, EventArgs e) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;


            string blockName = this.cboBlok.GetItemText(this.cboBlok.SelectedItem);

            ed.WriteMessage($"\n{blockName}\n");

            try {

                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                    ed.WriteMessage($"\nBlockTable connected\n");

                    if (!bt.Has(blockName))
                        throw new ArgumentException("Block definition not found", blockName);

                    ed.WriteMessage($"\n1\n");

                    ObjectId blockId = bt[blockName];

                    ed.WriteMessage($"\n2\n");


                    BlockTableRecord btr = tr.GetObject(blockId, OpenMode.ForRead) as BlockTableRecord;

                    ed.WriteMessage($"\n3\n");


                    

                    ed.WriteMessage($"\n4{btr.GetType().Name}\n");




                    //string objType = entity.GetType().Name;

                    //ed.WriteMessage($"\n{objType}\n");


                }
            } catch (Exception er) {

                ed.WriteMessage($"\n{er}\n");
            }
        }
    }
}
