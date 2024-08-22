using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Geo_geo.Class {
    internal class cOpisyMapy {
        public void InsertCrossDescript() {


            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int j = 0;
            double[] origin = new double[3];

            string blockname = "";

            double scale = DescScale() / 10;

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string sourceFileName = $"{name}\\pin.dwg";

            ImportBlock("cross_description", sourceFileName);




            PromptPointResult pPtRes1 = PinFormMap();

            if (pPtRes1.Status != PromptStatus.OK) {
                ed.WriteMessage("\nNie wskazano punktu.");
                return;
            }

            Point3d pt1 = pPtRes1.Value;

            //AddLayerIfNotExists($"_krzyze_{scale:0}");

            double resx = pt1.X % scale;
            double resy = pt1.Y % scale;

            double value_x = (resx < (scale / 2)) ? pt1.X - resx : pt1.X - resx + scale;
            double value_y = (resy < (scale / 2)) ? pt1.Y - resy : pt1.Y - resy + scale;


            List<string> atr = new List<string>();

            ed.WriteMessage($"\n{value_x}, {value_y}");

            atr.Add($"{value_x:0}");
            atr.Add($"{value_y:0}");

            Point3d pts_blok = new Point3d(value_x, value_y, 0.0);
            InsertBlock(pts_blok, atr, "cross_description");



        }

        public void LoadCrossBySquare() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int j = 0;
            double[] origin = new double[3];

            string blockname = "";

            double scale = DescScale() / 10;

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string sourceFileName = $"{name}\\pin.dwg";

            ImportBlock("cross", sourceFileName);

            PromptPointResult pPtRes1 = PinFormMap();

            if (pPtRes1.Status != PromptStatus.OK) {
                ed.WriteMessage("\nNie wskazano punktu.");
                return;
            }

            Point3d pt1 = pPtRes1.Value;

            PromptPointResult pPtRes2 = PinFormMap();

            if (pPtRes2.Status != PromptStatus.OK) {
                ed.WriteMessage("\nNie wskazano punktu.");
                return;
            }

            Point3d pt2 = pPtRes2.Value;


            //AddLayerIfNotExists($"_krzyze_{scale:0}");

            double low_x = 0;
            double low_y = 0;

            double high_x = 0;
            double high_y = 0;

            if (pt1.X > pt2.X) {
                low_x = pt2.X;
                high_x = pt1.X;

            } else {
                low_x = pt1.X;
                high_x = pt2.X;
            }

            if (pt1.Y > pt2.Y) {
                low_y = pt2.Y;
                high_y = pt1.Y;

            } else {
                low_y = pt1.Y;
                high_y = pt2.Y;
            }

            double resx = low_x % scale;
            double resy = low_y % scale;

            //double value_x = low_x - resx;
            //double value_y = low_y - resy;

            double value_x = (resx < (scale / 2)) ? low_x - resx : low_x - resx + scale;
            double value_y = (resx < (scale / 2)) ? low_y - resy : low_y - resy + scale;

            double temp_y = value_y;




            double ogscale = DescScale();
            string newLayer = $"_krzyze_{ogscale:0}";
            AddLayerIfNotExists(newLayer, 0);

            for (double cur_x = value_x; cur_x <= high_x; cur_x += scale) {

                value_x = cur_x;

                for (double cur_y = value_y; cur_y <= high_y; cur_y += scale) {

                    //List<string> atr = new List<string>();

                    value_y = cur_y;

                    //atr.Add($"{value_x:0}");
                    //atr.Add($"{value_y:0}");

                    Point3d pts_blok = new Point3d(value_x, value_y, 0.0);
                    //InsertBlock(pts_blok, atr);
                    InsertLineCross(pts_blok, newLayer);
                }

                value_y = temp_y;

            }
        }

        public void InsertLineCross(Point3d point, string newLayer) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            Autodesk.AutoCAD.DatabaseServices.Polyline plineX = new Autodesk.AutoCAD.DatabaseServices.Polyline();
            Autodesk.AutoCAD.DatabaseServices.Polyline plineY = new Autodesk.AutoCAD.DatabaseServices.Polyline();

            Point2d pt = new Point2d(point.X - 2.5, point.Y);
            plineX.AddVertexAt(0, pt, 0.0, 0.0, 0.0);

            pt = new Point2d(point.X, point.Y);
            plineX.AddVertexAt(1, pt, 0.0, 0.0, 0.0);

            pt = new Point2d(point.X + 2.5, point.Y);
            plineX.AddVertexAt(2, pt, 0.0, 0.0, 0.0);

            pt = new Point2d(point.X, point.Y -2.5);
            plineY.AddVertexAt(0, pt, 0.0, 0.0, 0.0);

            pt = new Point2d(point.X, point.Y);
            plineY.AddVertexAt(1, pt, 0.0, 0.0, 0.0);

            pt = new Point2d(point.X, point.Y + 2.5);
            plineY.AddVertexAt(2, pt, 0.0, 0.0, 0.0);

            try {
                plineX.Layer = newLayer;
                plineY.Layer = newLayer;
            } catch (Exception e) { ed.WriteMessage($"\n{e}"); }

            using (DocumentLock acLckDoc = doc.LockDocument()) {
                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                    btr.AppendEntity(plineX);
                    tr.AddNewlyCreatedDBObject(plineX, true);

                    btr.AppendEntity(plineY);
                    tr.AddNewlyCreatedDBObject(plineY, true);
                    tr.Commit();
                }
            }
        }

        public PromptPointResult PinFormMap() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            // string blockname = "PIN (2D)";
            string blockname = "cross_chair";

            PromptPointResult pPtRes;

            Point3d temp = new Point3d(0, 0, 0);

            //PromptPointResult pPtRes = ed.GetPoint("\nWskaż punkt");

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string sourceFileName = $"{name}\\pin.dwg";

            try {

                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                    ObjectId blockId = bt.Has(blockname) ? bt[blockname] : ImportBlock(blockname, sourceFileName);


                    if (blockId != ObjectId.Null) {

                        BlockTableRecord space = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                        BlockReference blockRef = new BlockReference(Point3d.Origin, blockId);
                        ObjectId brId = space.AppendEntity(blockRef);
                        tr.AddNewlyCreatedDBObject(blockRef, true);

                        ObjectId[] ss = new ObjectId[1] { brId };
                        ed.SetImpliedSelection(ss);
                        PromptSelectionResult psr = ed.SelectImplied();

                        Point3d basePt = blockRef.Position;

                        DragCallback callback = (Point3d pt, ref Matrix3d xform) => {
                            if (pt.IsEqualTo(basePt))
                                return SamplerStatus.NoChange;
                            xform = Matrix3d.Displacement(basePt.GetVectorTo(pt));
                            return SamplerStatus.OK;
                        };
                        PromptPointResult ppr = ed.Drag(psr.Value, "\nWskaż punkt: ", callback);

                        pPtRes = ppr;

                        temp = pPtRes.Value;


                        blockRef.Erase();

                        return pPtRes;

                    }

                    tr.Commit();


                }
            } catch {

                pPtRes = ed.GetPoint("\nWskaż punkt");
                return pPtRes;

            }



            ed.WriteMessage($"\n{temp.X}, {temp.Y}");

            pPtRes = ed.GetPoint("\nWskaż punkt");
            return pPtRes;
        }

        private ObjectId ImportBlock(string blockName, string filenName) {
            if (!File.Exists(filenName))
                throw new FileNotFoundException("File not found", filenName);

            Database targetDb = HostApplicationServices.WorkingDatabase;
            ObjectId owner = SymbolUtilityServices.GetBlockModelSpaceId(targetDb);

            using (Database sourceDb = new Database(false, true)) {
                sourceDb.ReadDwgFile(filenName, FileShare.ReadWrite, false, "");
                using (Transaction tr = sourceDb.TransactionManager.StartTransaction()) {
                    BlockTable bt = (BlockTable)tr.GetObject(sourceDb.BlockTableId, OpenMode.ForRead);
                    if (!bt.Has(blockName))
                        throw new ArgumentException("Block definition not found", blockName);

                    ObjectIdCollection ids = new ObjectIdCollection();
                    ObjectId blockId = bt[blockName];
                    ids.Add(blockId);
                    IdMapping idMap = new IdMapping();
                    sourceDb.WblockCloneObjects(ids, owner, idMap, DuplicateRecordCloning.Ignore, false);

                    return idMap[blockId].Value;
                }
            }


        }


        public void InsertBlock(Point3d point, List<string> atr, string blockname = "cross") {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            ObjectId newbrId = new ObjectId();

            //PromptPointResult pPtRes = ed.GetPoint("\nWskaż punkt");

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string sourceFileName = $"{name}\\pin.dwg";


            double scale = DescScale();
            string newLayer = $"_krzyze_{scale:0}";


            int lp = 0;

            doc.LockDocument();

            try {



                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    // EXPERIMANTAL CODE

                    using (LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead)) {
                        if (!lt.Has(newLayer)) {
                            using (LayerTableRecord ltr = new LayerTableRecord()) {

                                ltr.Name = newLayer;
                                ltr.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByAci, 0);

                                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                                    LayerTable wlt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);

                                    wlt.Add(ltr);
                                    trans.AddNewlyCreatedDBObject(ltr, true);
                                    trans.Commit();
                                }


                            }
                        }
                    }
                    

                    //


                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                    ObjectId blockId = bt.Has(blockname) ? bt[blockname] : ImportBlock(blockname, sourceFileName);


                    if (blockId != ObjectId.Null) {

                        BlockTableRecord space = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                        BlockReference blockRef = new BlockReference(point, blockId);

                        blockRef.Layer = newLayer;
                        ObjectId brId = space.AppendEntity(blockRef);

                        newbrId = brId;

                        BlockTableRecord blockDef = bt[blockname].GetObject(OpenMode.ForRead) as BlockTableRecord;

                        tr.AddNewlyCreatedDBObject(blockRef, true);

                        foreach (ObjectId id in blockDef) {

                            DBObject obj = id.GetObject(OpenMode.ForRead);

                            AttributeDefinition attDef = obj as AttributeDefinition;


                            if ((attDef != null) && (!attDef.Constant)) {


                                using (AttributeReference attRef = new AttributeReference()) {

                                    attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);


                                    if (lp < atr.Count) {

                                        attRef.TextString = atr[lp];

                                    } else {
                                        attRef.TextString = "";
                                    }

                                    lp++;

                                    blockRef.AttributeCollection.AppendAttribute(attRef);

                                    tr.AddNewlyCreatedDBObject(attRef, true);

                                }
                            }
                        }



                    }

                    tr.Commit();

                }


            } catch {


            }

        }

        public void AddLayerIfNotExists(string newLayer, short color = 0) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            doc.LockDocument();

            using (Transaction tr = db.TransactionManager.StartTransaction()) {

                // EXPERIMANTAL CODE

                using (LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead)) {
                    if (!lt.Has(newLayer)) {
                        using (LayerTableRecord ltr = new LayerTableRecord()) {

                            ltr.Name = newLayer;
                            ltr.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByAci, color);

                            using (Transaction trans = db.TransactionManager.StartTransaction()) {
                                LayerTable wlt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);

                                wlt.Add(ltr);
                                trans.AddNewlyCreatedDBObject(ltr, true);
                                trans.Commit();
                            }
                        }
                    }
                }
                tr.Commit();
            }
        }


        public void GridGodlo() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptPointResult pPtRes = PinFormMap();

            if (pPtRes.Status != PromptStatus.OK) {
                ed.WriteMessage("\nNie wskazano punktu.");
                return;
            }

            PromptPointResult pPtRes2 = PinFormMap();

            if (pPtRes2.Status != PromptStatus.OK) {
                ed.WriteMessage("\nNie wskazano punktu.");
                return;
            }

            Point3d pt2 = pPtRes2.Value;
            Point3d pt1 = pPtRes.Value;

            AddLayerIfNotExists("_sekcje_500", 254);

            double low_x = 0;
            double low_y = 0;

            double high_x = 0;
            double high_y = 0;

            if (pt1.X > pt2.X) {
                low_x = pt2.X;
                high_x = pt1.X;

            } else {
                low_x = pt1.X;
                high_x = pt2.X;
            }

            if (pt1.Y > pt2.Y) {
                low_y = pt2.Y;
                high_y = pt1.Y;

            } else {
                low_y = pt1.Y;
                high_y = pt2.Y;
            }

            if (low_x % 10 == 0) {

                low_x = low_x + 1.0;
            }

            if (low_y % 10 == 0) {

                low_y = low_y + 1.0;
            }

            double value_x = low_x;
            double value_y = low_y;

            double temp_y = value_y;


            for (double cur_x = value_x; cur_x <= high_x; cur_x += 400) {


                value_x = cur_x;

                for (double cur_y = value_y; cur_y <= high_y; cur_y += 250) {

                    value_y = cur_y;


                    GetGodlo(new Point3d(value_x, value_y, 0.0));

                    ed.WriteMessage($"\n{value_x}, {value_y}");
                }

                value_y = temp_y;

            }

            




        }

        public string GetGodlo(Point3d pt) {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            double value_x = pt.Y;
            double value_y = pt.X;

            string result = value_y.ToString()[0].ToString();

            double main_x = 4920 * 1000;
            double main_y = (332 + (double.Parse(value_y.ToString()[0].ToString()) * 1000)) * 1000;

            double temp = 0.0;
            double temp_1x = 0.0;
            double temp_1y = 0.0;

            double msg = 0.0;

            temp = Math.Floor((value_x - main_x) / 5000);

            // 1:10 000 part 1

            result = $"{result}.{temp:0}";

            double temp_x = Math.Ceiling((value_x - ((temp * 5000) + main_x)) / 1000);
            temp_1x = ((temp * 5000) + main_x);

            // 1:10 000 part 2

            temp = Math.Floor((value_y - main_y)/ 8000);
            result = $"{result}.{temp:0}";

            double temp_y = Math.Ceiling((value_y - ((temp * 8000)+ main_y)) / 1600);
            temp_1y = ((temp * 8000) + main_y);

            double temp_2x = temp_x;

            if (temp_x == 5.0) {
                temp_2x = 1.0;
            } else if (temp_x == 4.0) {
                temp_2x = 2.0;
            } else if (temp_x == 3.0) {
                temp_2x = 3.0;
            } else if (temp_x == 2.0) {
                temp_2x = 4.0;
            } else if (temp_x == 1.0) {
                temp_2x = 5.0;
            }

            // 1:2 000

            temp = Math.Floor(temp_y + ((temp_2x - 1) * 5.0));

            if (temp < 10) {
                result = $"{result}.0{temp:0}";
            } else {
                result = $"{result}.{temp:0}";
            }

            temp_1y = ((1600 * temp_y) + temp_1y);
            temp_y = value_y - temp_1y;
            temp_y = (temp_y > 0.0) ? temp_y : -temp_y;

            temp_1x = ((1000 * temp_x) + temp_1x);
            temp_x = value_x - temp_1x;
            temp_x = (temp_x > 0.0) ? temp_x : -temp_x;

            temp_y = Math.Ceiling(temp_y / 800);
            temp_x = Math.Ceiling(temp_x / 500);

            //ed.WriteMessage($"\n{temp_y}...{temp_x}");

            // 1:1 000

            temp = Math.Floor((3 - temp_y) + ((temp_x - 1) * 2.0));

            result = $"{result}.{temp:0}";

            // 1:500

            temp_1y = ((800 * (3-temp_y)) + temp_1y);
            temp_y = value_y - temp_1y;
            temp_y = (temp_y > 0.0) ? temp_y : -temp_y;

            temp_1x = ((500 * temp_x) + temp_1x);
            temp_x = value_x - temp_1x;
            temp_x = (temp_x > 0.0) ? temp_x : -temp_x;

            temp_y = Math.Ceiling(temp_y / 400);
            temp_x = Math.Ceiling(temp_x / 250);

            //temp_y = ((3 - temp_y)> 8.0) ? temp_y - 8.0: -temp_y;


            //ed.WriteMessage($"\n{temp_y}...{temp_x}");

            temp = Math.Floor((3 - temp_y) + ((temp_x - 1) * 2.0));

            while (temp > 4.0) {
                temp = temp - 4.0;
            }

            result = $"{result}.{temp:0}";


            //ed.WriteMessage($"\n{temp_y}...{temp_x}");

            InsertGodlo(result, value_x, value_y);

            return result;
        }

        public void InsertGodlo(string result, double value_x, double value_y) {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            double resx = value_x % 250;
            double resy = value_y % 400;

            value_x = value_x - resx;
            value_y = value_y - resy;

            Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();

            Point2d pt2d = new Point2d(value_y, value_x);
            pline.AddVertexAt(0, pt2d, 0.0, 0.0, 0.0);

            pt2d = new Point2d(value_y + 400, value_x);
            pline.AddVertexAt(1, pt2d, 0.0, 0.0, 0.0);

            pt2d = new Point2d(value_y + 400, value_x + 250);
            pline.AddVertexAt(2, pt2d, 0.0, 0.0, 0.0);

            pt2d = new Point2d(value_y, value_x + 250);
            pline.AddVertexAt(3, pt2d, 0.0, 0.0, 0.0);

            pt2d = new Point2d(value_y, value_x);
            pline.AddVertexAt(4, pt2d, 0.0, 0.0, 0.0);


            DBText text = new DBText();
            text.TextString = result;
            // text.Position = new Point3d(value_y + 200, value_x + 125, 0);
            text.Position = new Point3d(value_y + 37, value_x + 92, 0);
            text.Height = 50;
            text.WidthFactor = 0.66;
            text.AdjustAlignment(db);

            try {
                string newLayer = "_sekcje_500";
                text.Layer = newLayer;
                pline.Layer = newLayer;
            }
            catch (Exception e) { ed.WriteMessage($"\n{e}"); }

            using (DocumentLock acLckDoc = doc.LockDocument()) {
                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);


                    btr.AppendEntity(text);
                    tr.AddNewlyCreatedDBObject(text, true);

                    btr.AppendEntity(pline);
                    tr.AddNewlyCreatedDBObject(pline, true);
                    tr.Commit();


                }

            }

        }

        public double DescScale() {

            string fileContent = "";
            string fileName = "C:\\Users\\Public\\Documents\\acScale.txt";

            double scale = 500.0;

            if (System.IO.File.Exists(fileName)) {

                using (StreamReader sr = new StreamReader(fileName)) {
                    fileContent = sr.ReadToEnd();
                }

                try {
                    scale = Convert.ToDouble(fileContent);
                } catch {

                    scale = 500.0;

                }


            } else {
                System.IO.File.WriteAllText(fileName, "500");

            }

            return scale;
        }

        public void SetDescScale() {

            string fileContent = "";
            string fileName = "C:\\Users\\Public\\Documents\\acScale.txt";

            double scale = 500.0;

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            var doubleOptions = new PromptDoubleOptions("Wpisz nową wartość skali: ");
            doubleOptions.DefaultValue = 500.0;

            PromptDoubleResult newScale = ed.GetDouble(doubleOptions);

            if (System.IO.File.Exists(fileName)) {

                using (StreamWriter sw = new StreamWriter(fileName)) {

                    sw.WriteLine(newScale.Value);
                }
            }
        }
    }

}
