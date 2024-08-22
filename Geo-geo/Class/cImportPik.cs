using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Geo_geo.Class {
    public class cImportPik {


        public void LoadPointsFromTxt() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            string fileName;
            string fileContent;
            string[] lines;
            string[] points;
            int i = 0;
            int j = 0;
            double[] origin = new double[3];
            double skala;
            string pts_type;
            string pts_format;

            PromptKeywordOptions opts = new PromptKeywordOptions("\nChoose point type: ");
            opts.Keywords.Add("Tekst");
            opts.Keywords.Add("MTekst");

            PromptResult result = ed.GetKeywords(opts);

            if (result.Status != PromptStatus.OK)
                return;

            pts_type = result.StringResult;


            PromptKeywordOptions opts2 = new PromptKeywordOptions("\nChoose point format: ");
            opts2.Keywords.Add("NYxh");
            opts2.Keywords.Add("NXyh");
            opts2.Keywords.Add("Xyh");
            opts2.Keywords.Add("Yxh");

            PromptResult formatResult = ed.GetKeywords(opts2);

            if (formatResult.Status != PromptStatus.OK)
                return;

            pts_format = formatResult.StringResult;

            PromptDoubleOptions scaleOpts = new PromptDoubleOptions("\nEnter scale factor: ");
            PromptDoubleResult scaleResult = ed.GetDouble(scaleOpts);

            if (scaleResult.Status != PromptStatus.OK)
                return;
            skala = scaleResult.Value;

            /*
            PromptOpenFileOptions fileOpts = new PromptOpenFileOptions("Select a text file: ");
            fileOpts.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            PromptFileNameResult fileResult = ed.GetFileNameForOpen(fileOpts);
            if (fileResult.Status != PromptStatus.OK) {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }
            fileName = fileResult.StringResult;

            if (string.IsNullOrEmpty(fileName))
                return;
            */

            cFileDlg dlg = new cFileDlg();
            fileName = dlg.OpenDlg();

            if (fileName == "return") {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }


            // Read the content of the text file
            using (StreamReader sr = new StreamReader(fileName)) {
                fileContent = sr.ReadToEnd();
            }

            // Split the file content into lines
            lines = fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // Iterate through the lines of the file
            for (i = 0; i < lines.Length; i++) {
                // Replace multiple spaces with a single space

                while (lines[i].Contains("  ")) {
                    lines[i] = lines[i].Replace("  ", " ");
                }

                // Replace periods with commas and tabs with spaces
                lines[i] = lines[i].Replace(",", ".");
                lines[i] = lines[i].Replace("\t", " ");

                points = lines[i].Split(' ');

                int nr = 0;
                int x = 1;
                int y = 2;
                int h = 3;

                if (pts_format == "NYxh") {
                    nr = 0;
                    x = 1;
                    y = 2;
                    h = 3;
                } else if (pts_format == "NXyh") {
                    nr = 0;
                    x = 2;
                    y = 1;
                    h = 3;
                } else if (pts_format == "Xyh") {
                    nr = 2;
                    x = 0;
                    y = 1;
                    h = 2;
                } else if (pts_format == "Yxh") {
                    nr = 2;
                    x = 1;
                    y = 0;
                    h = 2;
                }

                /*
                if (Import.CheckBox1) {
                    nr = h;
                }
                */

                try {

                    if (points.Length > 2) {
                        origin[0] = double.Parse(points[x]);
                        origin[1] = double.Parse(points[y]);
                        origin[2] = double.Parse(points[h]);
                    } else if (h == nr) {
                        origin[0] = double.Parse(points[x]);
                        origin[1] = double.Parse(points[y]);
                        origin[2] = double.Parse(points[h]);
                    } else {
                        origin[0] = double.Parse(points[x]);
                        origin[1] = double.Parse(points[y]);
                        origin[2] = 0.0;
                    }

                    if (pts_type == "Tekst") {
                        DBText text = new DBText();
                        text.TextString = points[nr];
                        text.Position = new Point3d(origin[0], origin[1], origin[2]);
                        text.Height = skala;
                        text.AdjustAlignment(db);

                        using (Transaction tr = db.TransactionManager.StartTransaction()) {
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                            btr.AppendEntity(text);
                            tr.AddNewlyCreatedDBObject(text, true);
                            tr.Commit();
                        }
                    } else if (pts_type == "MTekst") {
                        MText mtext = new MText();
                        mtext.Contents = points[nr];
                        mtext.Location = new Point3d(origin[0], origin[1], origin[2]);
                        mtext.TextHeight = skala;
                        mtext.Attachment = AttachmentPoint.MiddleCenter;

                        using (Transaction tr = db.TransactionManager.StartTransaction()) {
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                            btr.AppendEntity(mtext);
                            tr.AddNewlyCreatedDBObject(mtext, true);
                            tr.Commit();
                        }
                    } else if (pts_type == "Block") {


                        continue;

                    } else {
                        continue;
                    }

                    j++;
                } catch (System.Exception ex) {
                    continue;
                    ed.WriteMessage("\nError: " + ex.Message);

                }
            }

            ed.WriteMessage($"\nLoaded: {j} points");




        }

        public void LoadPointsFromTxtForm(string pts_type, string pts_format, double skala, bool NrH, bool h0, string separator, string typAcPoint,
            double ox = 0.0, double oy = 0.0, string blokname = "none", int skip_rows = 0) {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;



            ed.WriteMessage($"\nTyp: {pts_type}");
            ed.WriteMessage($"\nFormat: {pts_format}");
            ed.WriteMessage($"\nSkala: {skala}");

            string fileName;
            string fileContent;
            string[] lines;
            string[] points;
            int i = 0;
            int j = 0;
            double[] origin = new double[3];
            double height = 0.0;
            bool temp_h0 = false;


            /*
            PromptOpenFileOptions fileOpts = new PromptOpenFileOptions("Select a text file: ");
            fileOpts.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            PromptFileNameResult fileResult = ed.GetFileNameForOpen(fileOpts);
            if (fileResult.Status != PromptStatus.OK) {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }
            fileName = fileResult.StringResult;
            */

            cFileDlg dlg = new cFileDlg();
            fileName = dlg.OpenDlg();

            if (fileName == "return") {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }


            if (string.IsNullOrEmpty(fileName))
                return;

            // Read the content of the text file
            using (StreamReader sr = new StreamReader(fileName)) {
                fileContent = sr.ReadToEnd();
            }

            // Split the file content into lines
            lines = fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // Iterate through the lines of the file

            char sep = ' ';

            using (doc.LockDocument()) {

                switch (separator) {
                    case "Spacja":
                        sep = ' ';
                        break;
                    case "Tabulacja":
                        sep = '\t';
                        break;
                    case "Przecinek":
                        sep = ',';
                        break;
                    case "Średnik":
                        sep = ';';
                        break;
                    default:
                        sep = ' ';
                        break;
                }

                for (i = 0; i < lines.Length; i++) {

                    if (skip_rows > 0) {
                        ed.WriteMessage($"\nSkiped line: {i}");
                        skip_rows--;
                        continue;
                    }

                    ed.WriteMessage($"\nLoad line: {i}");

                    while (lines[i].Contains($"{sep}{sep}")) {
                        lines[i] = lines[i].Replace($"{sep}{sep}", $"{sep}");
                    }

                    lines[i] = lines[i].Replace(",", ".");

                    points = lines[i].Split(sep);

                    int nr = 0;
                    int x = 1;
                    int y = 2;
                    int h = 3;
                    int h2 = 4;

                    if (pts_format == "nyxh") {
                        nr = 0;
                        x = 1;
                        y = 2;
                        h = 3;
                        h2 = 4;
                    } else if (pts_format == "nxyh") {
                        nr = 0;
                        x = 2;
                        y = 1;
                        h = 3;
                        h2 = 4;
                    } else if (pts_format == "xyh") {
                        nr = 2;
                        x = 0;
                        y = 1;
                        h = 2;
                        h2 = 3;
                    } else if (pts_format == "yxh") {
                        nr = 2;
                        x = 1;
                        y = 0;
                        h = 2;
                        h2 = 3;
                    } else if (pts_format == "yx") {
                        nr = 1;
                        x = 1;
                        y = 0;
                        h = 2;
                        h2 = 3;
                        h0 = true;
                    } else if (pts_format == "xy") {
                        nr = 1;
                        x = 0;
                        y = 1;
                        h = 2;
                        h2 = 3;
                        h0 = true;
                    }


                    if (NrH) {
                        nr = h;
                    }


                    try {


                        // zmiana z 2 na 3
                        if (points.Length > 3) {
                            origin[0] = double.Parse(points[x]);
                            origin[1] = double.Parse(points[y]);
                            origin[2] = double.Parse(points[h]);
                        } else if (h == nr) {
                            origin[0] = double.Parse(points[x]);
                            origin[1] = double.Parse(points[y]);
                            origin[2] = double.Parse(points[h]);
                        } else if (points.Length == 2) {
                            origin[0] = double.Parse(points[x]);
                            origin[1] = double.Parse(points[y]);
                            origin[2] = 0.00;
                        } else {
                            origin[0] = double.Parse(points[x]);
                            origin[1] = double.Parse(points[y]);
                            origin[2] = 0.00;
                        }

                        origin[0] = origin[0] + ox;
                        origin[1] = origin[1] + oy;

                        if (pts_type == "Slup") {

                            height = double.Parse(points[h2]);

                        }



                        if (h0) {
                            temp_h0 = h0;
                            origin[2] = 0.00;


                        }

                        //ed.WriteMessage($" WSP: {origin[0]}, {origin[1]}, {origin[2]}");

                        if (pts_type == "Tekst") {
                            DBText text = new DBText();
                            text.TextString = points[nr];
                            text.Position = new Point3d(origin[0], origin[1], origin[2]);
                            text.Height = skala;
                            text.AdjustAlignment(db);

                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                btr.AppendEntity(text);
                                tr.AddNewlyCreatedDBObject(text, true);
                                tr.Commit();
                            }
                        } else if (pts_type == "MTekst") {
                            MText mtext = new MText();
                            mtext.Contents = points[nr];
                            mtext.Location = new Point3d(origin[0], origin[1], origin[2]);
                            mtext.TextHeight = skala;
                            mtext.Attachment = AttachmentPoint.BottomLeft;

                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                btr.AppendEntity(mtext);
                                tr.AddNewlyCreatedDBObject(mtext, true);
                                tr.Commit();
                            }
                        } else if (pts_type == "Blok") {

                            if (blokname == "none") {
                                break;
                            }

                            List<string> atr = new List<string>();

                            int jj = 3;

                            if ((pts_format == "xyh") || (pts_format == "yxh") || (pts_format == "xy") || (pts_format == "yx")) {
                                atr.Add($"{i}");
                                jj = 2;
                            } else {
                                atr.Add($"{points[nr]}");
                            }

                            ed.WriteMessage($"\nlenght: {points.Length}");

                            if (points.Length >= jj) {

                                for (int jp = jj; jp < points.Length; jp++) {
                                    atr.Add($"{points[jp]}");
                                }
                            }

                            Point3d pts_blok = new Point3d(origin[0], origin[1], origin[2]);
                            AddBlock(pts_blok, atr, blokname);

                            //ed.WriteMessage("\nElse Block");
                            //continue;
                        } else if (pts_type == "Sfera") {

                            Solid3d solid3d = new Solid3d();
                            solid3d.CreateSphere(skala);
                            solid3d.TransformBy(Matrix3d.Displacement(new Point3d(origin[0], origin[1], origin[2]) - Point3d.Origin));

                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                btr.AppendEntity(solid3d);
                                tr.AddNewlyCreatedDBObject(solid3d, true);
                                tr.Commit();
                            }

                        } else if (pts_type == "AcPoint") {

                            DBPoint acPoint = new DBPoint(new Point3d(origin[0], origin[1], origin[2]));

                            using (Transaction tr = db.TransactionManager.StartTransaction()) {

                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                btr.AppendEntity(acPoint);
                                tr.AddNewlyCreatedDBObject(acPoint, true);
                                tr.Commit();
                            }

                            db.Pdmode = Int32.Parse(typAcPoint);
                            db.Pdsize = skala;

                        } else if (pts_type == "Slup") {

                            Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
                            CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;

                            double cylinder_h = 0.0;
                            double origin_h = 0.0;



                            if (cylinder_h < 0) {
                                cylinder_h = -cylinder_h;
                            }

                            if (origin[2] < height) {

                                cylinder_h = (height - origin[2]);
                                origin_h = origin[2];

                            } else {

                                cylinder_h = (origin[2] - height);
                                origin_h = height;

                            }

                            Interval hC = new Interval(cylinder_h);

                            Cylinder cylinder = new Cylinder(skala, new Point3d(origin[0], origin[1], origin[2]), curUCS.Zaxis, curUCS.Zaxis, hC, 0, 90);

                            using (var tr = db.TransactionManager.StartTransaction()) {
                                using (var circle = new Circle(new Point3d(origin[0], origin[1], origin_h), Vector3d.ZAxis, skala)) {
                                    var curves = new DBObjectCollection();
                                    curves.Add(circle);
                                    var regions = Region.CreateFromCurves(curves);
                                    using (var region = (Region)regions[0]) {
                                        var solid = new Solid3d();
                                        solid.Extrude(region, cylinder_h, 0.0);
                                        var curSpace = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                        curSpace.AppendEntity(solid);
                                        tr.AddNewlyCreatedDBObject(solid, true);
                                    }
                                }
                                tr.Commit();
                            }

                            ed.WriteMessage($"\n{cylinder_h}\t{origin_h}");


                        } else if (pts_type == "Okrag") {

                            Circle acCirc = new Circle();
                            acCirc.Center = new Point3d(origin[0], origin[1], origin[2]);
                            acCirc.Radius = skala;


                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                btr.AppendEntity(acCirc);
                                tr.AddNewlyCreatedDBObject(acCirc, true);
                                tr.Commit();
                            }


                        } else {
                            ed.WriteMessage("\nElse");
                            continue;
                        }

                        j++;
                    } catch (System.Exception ex) {
                        ed.WriteMessage("\nError: " + ex.Message);
                        continue;
                    }
                }
            }


            ed.WriteMessage($"\nLoaded: {j} points");




        }

        public void AddBlock(Point3d point, List<string> atr, string blockName = "pikieta_test") {

            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            using (Transaction myT = db.TransactionManager.StartTransaction()) {

                BlockTable bt = db.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;

                BlockTableRecord blockDef = bt[blockName].GetObject(OpenMode.ForRead) as BlockTableRecord;

                //Also open modelspace - we'll be adding our BlockReference to it

                BlockTableRecord ms = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;

                //Create new BlockReference, and link it to our block definition

                using (BlockReference blockRef = new BlockReference(point, blockDef.ObjectId)) {

                    //Add the block reference to modelspace

                    ms.AppendEntity(blockRef);

                    myT.AddNewlyCreatedDBObject(blockRef, true);

                    //Iterate block definition to find all non-constant

                    // AttributeDefinitions

                    int lp = 0;


                    foreach (ObjectId id in blockDef) {

                        DBObject obj = id.GetObject(OpenMode.ForRead);

                        AttributeDefinition attDef = obj as AttributeDefinition;


                        if ((attDef != null) && (!attDef.Constant)) {

                            //This is a non-constant AttributeDefinition

                            //Create a new AttributeReference

                            using (AttributeReference attRef = new AttributeReference()) {

                                attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);

                                /*
                                attRef.TextString = atr[lp];
                                lp++;
                                */


                                if (lp < atr.Count) {

                                    if (attRef.Tag == "ID") {
                                        attRef.TextString = atr[0];
                                    } else {
                                        attRef.TextString = atr[lp];
                                    }

                                } else {
                                    attRef.TextString = "";
                                }

                                lp++;


                                /*
                                if (attRef.Tag == "ID")
                                { 
                                    attRef.TextString = atr; 
                                } 
                                else {
                                    attRef.TextString = "";
                                }
                                */

                                //Add the AttributeReference to the BlockReference

                                blockRef.AttributeCollection.AppendAttribute(attRef);

                                myT.AddNewlyCreatedDBObject(attRef, true);

                            }
                        }
                    }
                }

                myT.Commit();

            }

        }


        public void PinBlockExist(string blockName = "PIN (2D)") {

            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            try {
                using (Transaction myT = db.TransactionManager.StartTransaction()) {

                    BlockTable bt = db.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;

                    BlockTableRecord blockDef = bt[blockName].GetObject(OpenMode.ForRead) as BlockTableRecord;

                    ed.WriteMessage($"{blockDef}");


                }
            }
            catch (Exception ex) {

                ed.WriteMessage($"{ex.Message}");

                DocumentCollection dm = Application.DocumentManager;

                Database destDb = dm.MdiActiveDocument.Database;

                Database sourceDb = new Database(false, true);
                string sourceFileName;

                string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                sourceFileName = $"{name}\\pin.dwg";

                sourceDb.ReadDwgFile(sourceFileName, System.IO.FileShare.Read, true, "");

                ObjectIdCollection blockIds = new ObjectIdCollection();

                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = sourceDb.TransactionManager;

                using (Transaction myT = tm.StartTransaction()) {


                    BlockTable bt =

                        (BlockTable)tm.GetObject(sourceDb.BlockTableId,

                                                OpenMode.ForRead,

                                                false);


                    // Check each block in the block table


                    foreach (ObjectId btrId in bt) {

                        BlockTableRecord btr =

                          (BlockTableRecord)tm.GetObject(btrId,

                                                        OpenMode.ForRead,

                                                        false);

                        // Only add named & non-layout blocks to the copy list

                        if (!btr.IsAnonymous && !btr.IsLayout)

                            blockIds.Add(btrId);

                        btr.Dispose();

                    }

                }

                // Copy blocks from source to destination database

                IdMapping mapping = new IdMapping();

                sourceDb.WblockCloneObjects(blockIds,

                                            destDb.BlockTableId,

                                            mapping,

                                            DuplicateRecordCloning.Replace,

                                            false);

                ed.WriteMessage("\nCopied "

                                + blockIds.Count.ToString()

                                + " block definitions from "

                                + sourceFileName

                                + " to the current drawing.");

                sourceDb.Dispose();

            }


        }

        
        public void ImportBlocks() {

            DocumentCollection dm = Application.DocumentManager;

            Editor ed = dm.MdiActiveDocument.Editor;

            Database destDb = dm.MdiActiveDocument.Database;

            Database sourceDb = new Database(false, true);

            String sourceFileName;

            try {

                // Get name of DWG from which to copy blocks

                string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                sourceFileName = $"{name}\\bloki.dwg";

                //sourceFileName = "Z:\\Programy\\MAKRA\\geogeo\\bloki.dwg";

                // Read the DWG into a side database

                sourceDb.ReadDwgFile(sourceFileName, System.IO.FileShare.Read, true, "");


                // Create a variable to store the list of block identifiers

                ObjectIdCollection blockIds = new ObjectIdCollection();


                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm =

                  sourceDb.TransactionManager;


                using (Transaction myT = tm.StartTransaction()) {

                    // Open the block table

                    BlockTable bt =

                        (BlockTable)tm.GetObject(sourceDb.BlockTableId,

                                                OpenMode.ForRead,

                                                false);


                    // Check each block in the block table

                    foreach (ObjectId btrId in bt) {

                        BlockTableRecord btr =

                          (BlockTableRecord)tm.GetObject(btrId,

                                                        OpenMode.ForRead,

                                                        false);

                        // Only add named & non-layout blocks to the copy list

                        if (!btr.IsAnonymous && !btr.IsLayout)

                            blockIds.Add(btrId);

                        btr.Dispose();

                    }

                }

                // Copy blocks from source to destination database

                IdMapping mapping = new IdMapping();

                sourceDb.WblockCloneObjects(blockIds,

                                            destDb.BlockTableId,

                                            mapping,

                                            DuplicateRecordCloning.Replace,

                                            false);

                ed.WriteMessage("\nCopied "

                                + blockIds.Count.ToString()

                                + " block definitions from "

                                + sourceFileName

                                + " to the current drawing.");

            } catch (Autodesk.AutoCAD.Runtime.Exception ex) {

                ed.WriteMessage("\nError during copy: " + ex.Message);

            }

            sourceDb.Dispose();

        }

        public List<string> ListBlockReferences() {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            List<string> blocks = new List<string>();

            using (Transaction trans = db.TransactionManager.StartTransaction()) {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                foreach (ObjectId btrId in bt) {
                    BlockTableRecord btr = trans.GetObject(btrId, OpenMode.ForRead) as BlockTableRecord;

                    if (btr.IsLayout || btr.IsAnonymous || btr.IsFromExternalReference)
                        continue;

                    blocks.Add(btr.Name.ToString());

                }

                return blocks;


            }


        }

        public List<string> ListBlockAtr(string name) {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            List<string> blocks = new List<string>();

            //blocks.Add("test");

            using (Transaction trans = db.TransactionManager.StartTransaction()) {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                ed.WriteMessage($"\ntrans open");

                foreach (ObjectId btrId in bt) {

                    

                    BlockTableRecord btr = trans.GetObject(btrId, OpenMode.ForRead) as BlockTableRecord;
                    


                    if (btr.IsLayout || btr.IsAnonymous || btr.IsFromExternalReference)
                        continue;

                    if (btr.Name.ToString() == name) {

                        ed.WriteMessage($"\nbtrId: {btrId}");
                        ed.WriteMessage($"\nName: {btr.Name.ToString()} vs {name}");

                        string value = btr.ObjectId.ToString();
                        blocks.Add(value);
                        /*
                        BlockReference btrs = trans.GetObject(btrId, OpenMode.ForRead) as BlockReference;                        

                        ed.WriteMessage($"\nType: {btrs.GetType().Name}");

                        AttributeCollection attCol = (AttributeCollection)btrs.AttributeCollection;

                        foreach (ObjectId attId in attCol) {

                            AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);

                            ed.WriteMessage($"\natr: {attRef.Tag}");

                            blocks.Add(attRef.Tag.ToString());
                        }
                        */
                    } else {
                        continue;
                    }


                }

                if (blocks.Count > 0) { 

                return blocks;
                }

                blocks.Add("none");
                return blocks;


            }


        }

    }
}
