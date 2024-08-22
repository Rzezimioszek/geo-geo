using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Geo_geo.Class {
    internal class cSimple {

        public void get_cid(string format = "xy") {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            //Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptPointResult pPtRes = ed.GetPoint("Wskaż punkt\n");
            Point3d ptStart = pPtRes.Value;

            //ptStart.X, ptStart.Y

            string result = "";

            switch (format) {

                case ("xy"):
                    result = $"{ptStart.Y}\t{ptStart.X}";
                    break;

                case ("yx"):
                    result = $"{ptStart.X}\t{ptStart.Y}";
                    break;

                case ("xyh"):
                    result = $"{ptStart.Y}\t{ptStart.X}\t{ptStart.Z}";
                    break;

                case ("yxh"):
                    result = $"{ptStart.X}\t{ptStart.Y}\t{ptStart.Z}";
                    break;

                case ("h"):
                    result = $"{ptStart.Z}";
                    break;


            }

            ed.WriteMessage($"{result}\n");
            Clipboard.SetText($"{result}");


        }

        public void set_vid(bool reverse = false) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;


            string result = "";

            result = Clipboard.GetText();

            //ptStart.X, ptStart.Y

            string[] r;

            int x = 1;
            int y = 0;

            if (reverse) {
                x = 0;
                y = 1;
            }

            if ((result.Contains('.')) && (result.Contains(','))) {
                r = result.Split(',');

            } else if (result.Contains(' ')) {
                r = result.Split(' ');
            } else { r = result.Split('\t'); }

            double h = 0.0;


            if (r.Length > 2) { h = double.Parse(r[2]); }

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string sourceFileName = $"{name}\\pin.dwg";

            if (System.IO.File.Exists(sourceFileName)) {

                InsertMarker(new Point3d(double.Parse(r[x]), double.Parse(r[y]), h));

            } else {

                Circle acCirc = new Circle();
                acCirc.Center = new Point3d(double.Parse(r[x]), double.Parse(r[y]), h);
                acCirc.Radius = 0.5;
                acCirc.ColorIndex = 3;

                /*

                string fileName = $"C:\\Users\\Public\\Documents\\acMarkers.txt";

                if (System.IO.File.Exists(fileName)) 
                {
                    using (StreamWriter streamWriter = File.AppendText(fileName)) {

                        streamWriter.WriteLine($"{double.Parse(r[x])} {double.Parse(r[y])} {h}");

                    }

                }
                */

                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                    btr.AppendEntity(acCirc);
                    tr.AddNewlyCreatedDBObject(acCirc, true);
                    tr.Commit();

                }
            }

            Point2d min2d = new Point2d(double.Parse(r[x]) - 2, double.Parse(r[y]) - 2);

            Point2d max2d = new Point2d(double.Parse(r[x]) + 2, double.Parse(r[y]) + 2);


            ViewTableRecord view = new ViewTableRecord();


            view.CenterPoint = min2d + ((max2d - min2d) / 2.0);

            view.Height = max2d.Y - min2d.Y;

            view.Width = max2d.X - min2d.X;

            ed.SetCurrentView(view);

            ed.WriteMessage($"{result}\n");


        }

        public void GoToMarker(string value, bool reverse = false, bool show = true, bool blok = true, double zoomLvl = 2.0, bool remove = true, double scale = 1.0) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            double zoom = 2.0;

            // zoom = (200.0 / zoomLvl);

            switch (zoomLvl) {
                case 1.0:
                    zoom = 4;
                    break;
                case 2.0:
                    zoom = 8;
                    break;
                case 3.0:
                    zoom = 20;
                    break;
                case 4.0:
                    zoom = 40;
                    break;
                case 5.0:
                    zoom = 80;
                    break;
            }


            // zoom *  (200 / zoomPercent)

            //ed.WriteMessage($"\n{value}");

            string[] points = value.Split(' ');

            double h = 0.0;
            double x = 0.0;
            double y = 0.0;

            if (!reverse) {
                x = double.Parse(points[0]);
                y = double.Parse(points[1]);
            } else {

                x = double.Parse(points[1]);
                y = double.Parse(points[0]);


            }

            if (points.Length > 2) {

                h = double.Parse(points[2]);
            }

            doc.LockDocument();

            if (show) {

                string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string sourceFileName = $"{name}\\pin.dwg";

                if (System.IO.File.Exists(sourceFileName) && blok) {

                    if (remove) {
                        DeleteBlockByName("PIN2");
                    }

                    InsertMarker(new Point3d(x, y, h), scale);

                } else {

                    using (Transaction tr = db.TransactionManager.StartTransaction()) {

                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                        Circle acCirc = new Circle();
                        acCirc.Center = new Point3d(x, y, h);
                        acCirc.Radius = 0.5;
                        acCirc.ColorIndex = 3;

                        btr.AppendEntity(acCirc);
                        tr.AddNewlyCreatedDBObject(acCirc, true);
                        tr.Commit();

                    }
                }

            }

            Point2d min2d = new Point2d(x - zoom, y - zoom);
            Point2d max2d = new Point2d(x + zoom, y + zoom);

            ViewTableRecord view = new ViewTableRecord();

            view.CenterPoint = min2d + ((max2d - min2d) / 2.0);
            view.Height = max2d.Y - min2d.Y;
            view.Width = max2d.X - min2d.X;

            ed.SetCurrentView(view);
        }

        public void InsertMarker(Point3d point, double scale = 1.0) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            //PromptPointResult pPtRes = ed.GetPoint("\nWskaż punkt");

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string sourceFileName = $"{name}\\pin.dwg";

            doc.LockDocument();

            try {

                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                    ObjectId blockId = bt.Has("PIN2") ? bt["PIN2"] : ImportBlock("PIN2", sourceFileName);


                    if (blockId != ObjectId.Null) {

                        BlockTableRecord space = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                        BlockReference blockRef = new BlockReference(point, blockId);
                        blockRef.ScaleFactors = new Scale3d(scale);
                        ObjectId brId = space.AppendEntity(blockRef);

                        tr.AddNewlyCreatedDBObject(blockRef, true);

                    }

                    tr.Commit();

                }
            } catch {


            }

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

        public double get_distance() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;



            double distance = 0;

            List<Point3d> pts = new List<Point3d>();

            ObjectId brId = new ObjectId();

            Point3d pt = new Point3d();

            try {

                while (true) {
                    PromptPointResult pPtRes = ed.GetPoint("\nWskaż punkt");

                    if (pPtRes.Status == PromptStatus.Cancel) {
                        ed.WriteMessage("\nStoped");
                        break;
                    }
                    else if (pPtRes.Status != PromptStatus.None) {
                        ed.WriteMessage("\nNone");

                    } else if (pPtRes.Status != PromptStatus.Error) {
                        ed.WriteMessage("\nError");

                    } else if (pPtRes.Status != PromptStatus.Other) {
                        ed.WriteMessage("\nOther");

                    } else if (pPtRes.Status != PromptStatus.Keyword) {
                        ed.WriteMessage("\nKeyword");

                    } else if (pPtRes.Status != PromptStatus.Modeless) {
                        ed.WriteMessage("\nModeless");

                    }


                    pt = pPtRes.Value;

                    if (pts.Last().X == pt.X && pts.Last().Y == pt.Y && pts.Last().Z == pt.Z) {
                        pts.Remove(pts.Last());
                    }
                    else {

                        pts.Add(pt);

                    }
                    

                    //ed.WriteMessage($"\nNew point at: {pt.X}\t{pt.Y}");

                    if (pts.Count > 2) {

                        DeleteTempline(brId);
                    }

                    if (pts.Count > 1) {

                        brId = InsertTempLine(pts);
                    }
                }
            } catch { return 0.0; }


            Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();

            int i = 0;

            foreach (Point3d lpt in pts) {

                Point2d pt2d = new Point2d(lpt.X, lpt.Y);

                pline.AddVertexAt(i, pt2d, 0.0, 0.0, 0.0);

                i++;
             
            }

            if (pts.Count < 2) {

                return 0.0;
            }

            return pline.Length;
        }


        private ObjectId InsertTempLine(List<Point3d> pts) {

            ObjectId brId;

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int i = 0;

            Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();


            foreach (Point3d lpt in pts) {

                Point2d pt2d = new Point2d(lpt.X, lpt.Y);
                pline.AddVertexAt(i, pt2d, 0.0, 0.0, 0.0);

                i++;
            }

            using (DocumentLock acLckDoc = doc.LockDocument()) {
                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                    brId = btr.AppendEntity(pline);
                    tr.AddNewlyCreatedDBObject(pline, true);
                    tr.Commit();
                }
            }

            ed.WriteMessage($"L: {pline.Length:0.000}");

            return brId;

        }

        private void DeleteTempline(ObjectId brId) {

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
                } catch { }

            
        }

        public void DeleteBlockByName(string name) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            long i = 0;

            using (Transaction tr = db.TransactionManager.StartTransaction()) {

                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                RXClass rxBlockReference = RXClass.GetClass(typeof(BlockReference));
                ObjectId modelId = bt[BlockTableRecord.ModelSpace];

                using (BlockTableRecord model = tr.GetObject(modelId, OpenMode.ForRead) as BlockTableRecord) {


                    foreach (ObjectId id in model) {
                        if (id.IsNull || id.IsErased || id.IsEffectivelyErased || !id.IsValid)
                            continue;
                        try {
                            if (id.ObjectClass == rxBlockReference) {
                                using (BlockReference reference = tr.GetObject(id, OpenMode.ForRead) as BlockReference) {
                                    //ed.WriteMessage($"\n{reference.Name.ToUpper()}");

                                    if (reference.Name.ToUpper() == name.ToUpper()) {
                                        using (DocumentLock acLckDoc = doc.LockDocument()) {

                                            using (Transaction trans = db.TransactionManager.StartTransaction()) {
                                                Entity ent = trans.GetObject(id, OpenMode.ForWrite) as Entity;

                                                ent.Erase();
                                                trans.Commit();
                                                i++;
                                            }
                                        }
                                    }

                                }
                            }
                        } catch (Autodesk.AutoCAD.Runtime.Exception e) { ed.WriteMessage($"\n{e}"); }
                    }
                    tr.Commit();

                    
                }
            }
            // ed.WriteMessage($"\nUsunięto bloków: {i}\n");
        }


        public void RemovePolilinesVertex() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionResult selection = ed.GetSelection();
            if (selection.Status != PromptStatus.OK)
                return;

            PromptPointResult pPtRes = ed.GetPoint("Wskaż punkt\n");

            if (pPtRes.Status == PromptStatus.Cancel) {
                ed.WriteMessage("\nPoint getted");
                return;
            }

            Point3d pt = pPtRes.Value;

            List<ObjectId> line_ids = new List<ObjectId>();
            cObrot cO = new cObrot();
            line_ids = cO.GetIds(selection, "Polyline");
            int allObj = line_ids.Count;

            if (line_ids.Count > 0) {

                foreach (ObjectId line_id in line_ids) {
                    using (Transaction trans = db.TransactionManager.StartTransaction()) {
                        using (Entity entity = trans.GetObject(line_id, OpenMode.ForWrite) as Entity) {

 

                            Autodesk.AutoCAD.DatabaseServices.Polyline pline = entity as Autodesk.AutoCAD.DatabaseServices.Polyline;

                            for (int jk = 0; jk < (pline.NumberOfVertices - 1); jk++) {

                                double wspX = Math.Round(pline.GetPoint2dAt(jk).X, 3);
                                double wspY = Math.Round(pline.GetPoint2dAt(jk).Y, 3);

                                ed.WriteMessage($"\nPoint at: {wspX}\t{wspY}");

                                if ((Math.Round(wspX, 2) == Math.Round(pt.X, 2)) && (Math.Round(wspY, 2) == Math.Round(pt.Y,2))) {

                                    ed.WriteMessage($"\nDeleted: {wspX}\t{wspY}");


                                    pline.RemoveVertexAt(jk);
                                    trans.Commit();
                                    break;
                                }

                                }
                            }

                        }
                    }
                }
            }
        }
    }


