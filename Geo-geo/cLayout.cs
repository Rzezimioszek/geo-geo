using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Geo_geo.Class;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Geo_geo {
    internal class cLayout {

        public void VieportFromPolyline() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptSelectionOptions pso = new PromptSelectionOptions();

            pso.MessageForAdding = "\nWybierz zamknętą linię: ";
            pso.AllowDuplicates = false;
            pso.AllowSubSelections = true;
            pso.RejectObjectsFromNonCurrentSpace = true;
            pso.RejectObjectsOnLockedLayers = false;

            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status != PromptStatus.OK) { return; }
            if (psr.Value.Count < 1) { return; }

            List<ObjectId> poly_ids = new List<ObjectId>();
            poly_ids = GetPolylineIds(psr, "poly");
            if (poly_ids.Count < 1) { return; }

            foreach (ObjectId poly_id in poly_ids) {
                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    Entity ent = (Entity)tr.GetObject(poly_id, OpenMode.ForRead);
                    string parent_layer = ent.Layer;
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                    int i = 0;

                    Line longest_line = new Line(new Point3d(0.0, 0.0, 0.0), new Point3d(0.001, 0.0, 0.0));
                    Point2d origin2d = getVrt2d(tr, ent, 2);
                    Point3d origin = new Point3d(origin2d.X, origin2d.Y, 0.0);



                    ent.Highlight(); // wł podświetlenie bloku
                    ed.UpdateScreen();

                    cFileDlg selector = new cFileDlg();
                    string stg = selector.VieportDialog();

                    ent.Unhighlight(); // wył podświetlenie bloku

                    if (stg == "err") { continue; } // ominięcie rekordu
                    if (stg == "ForceStop!") { return; } // koniec działania programu

                    string[] sstg = stg.Split(';');
                    string layout_name = sstg[0];
                    double vieport_scale = double.Parse(sstg[1]);
                    bool VPlock = bool.Parse(sstg[2]);

                    Point2d[] raw = getVrt2dAll(tr, ent);
                    (Point2d min2d, Point2d max2d) = getMinMax2dFromArray(raw); // określenie punktów min i max z array
                    zoomToXY(ed, min2d, max2d); // zoom do bloku

                    double h = max2d.Y - min2d.Y;
                    double w = max2d.X - min2d.X;

                    // https://adndevblog.typepad.com/autocad/2012/05/listing-the-layout-names.html
                    LayoutManager lm = LayoutManager.Current;
                    lm.CurrentLayout = layout_name;
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord ps = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.PaperSpace], OpenMode.ForWrite);

                    Entity ent2 = ent.Clone() as Entity;


                    ObjectId id = ps.AppendEntity(ent2);
                    tr.AddNewlyCreatedDBObject(ent2, true);

                    Viewport vp = new Viewport();
                    vp.Layer = parent_layer;

                    ps.AppendEntity(vp);
                    tr.AddNewlyCreatedDBObject(vp, true);

                    // Usytuowanie maski viewportu
                    // var resMat = Matrix3d.Identity;
                    // Point3d p1 = new Point3d(min2d.X, min2d.Y, 0);
                    Point3d p2 = new Point3d(0, 0, 0);
                    Vector3d v = (p2 - origin);
                    Matrix3d mat = Matrix3d.Displacement(v); // ed.WriteMessage($"\nvector: {v}");
                    ent2.TransformBy(mat);
                    //
                    Tuple<double, double> sumXY = sumVrt2(tr, ent2);
                    double sumX = sumXY.Item1;
                    double sumY = sumXY.Item2;
                    int vrt = countVrt(tr, ent2);

                    sumX = sumX / vrt;
                    sumY = sumY / vrt;

                    Point3d vieport_centroid = new Point3d(sumX, sumY, 0.0);

                    // skala
                    if (vieport_scale != 1.0) {
                        Matrix3d scalingMatrix = Matrix3d.Scaling(vieport_scale, vieport_centroid);
                        ent2.TransformBy(scalingMatrix);
                    }
                    //

                    vp.CenterPoint = vieport_centroid; // vieport position      
                    vp.Height = h * vieport_scale; //vp.Height = h;
                    vp.ViewHeight = h;
                    vp.ViewCenter = min2d + ((max2d - min2d) / 2.0); // model position
                    vp.NonRectClipEntityId = id;
                    vp.NonRectClipOn = true;
                    vp.Locked = true; // Blokowanie rzutni
                    vp.On = VPlock;

                    // Rotacja plinii
                    // Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
                    // CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;
                    // double angle = longest_line.Angle;
                    // if (angle > Math.PI) {angle = (2 * Math.PI) - longest_line.Angle;}
                    // pline.TransformBy(Matrix3d.Rotation(angle, curUCS.Zaxis, new Point3d(0, 0, 0)));
                    // ed.WriteMessage($"\n{longest_line.Angle}\t{angle}\n");

                    tr.Commit();
                }
                if (doc.Database.TileMode == false) { db.TileMode = true; } // model
            }
            db.TileMode = false;
        }


        public void VieportFromModelspaceBlock() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptSelectionOptions pso = new PromptSelectionOptions();

            pso.MessageForAdding = "\nWybierz bloki: ";
            pso.AllowDuplicates = false;
            pso.AllowSubSelections = true;
            pso.RejectObjectsFromNonCurrentSpace = true;
            pso.RejectObjectsOnLockedLayers = false;

            string defScale = "1";
            string defActive = bool.TrueString;

            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status != PromptStatus.OK) { return; }
            if (psr.Value.Count < 1) { return; }

            // Podział bloków na podobiekty: Pętla przez pod obiekty
            foreach (SelectedObject so in psr.Value) {

                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    DBObjectCollection objs = new DBObjectCollection();
                    Entity ent = (Entity)tr.GetObject(so.ObjectId, OpenMode.ForRead);
                    BlockReference blockRef = ent as BlockReference;
                    Point3d origin = new Point3d(blockRef.Position.X, blockRef.Position.Y, 0.0);

                    ent.Explode(objs); // Rozbicie do kolekcji
                    string parent_layer = ent.Layer; // Warstwa nadrzędego obiektu

                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                    Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                    Point2d first = new Point2d();
                    Point2dCollection unsorted2d = new Point2dCollection();
                    List<string> used = new List<string>();

                    int i = 0;
                    double h = 0.0;
                    double w = 0.0;
                    Line longest_line = new Line(new Point3d(0.0, 0.0, 0.0), new Point3d(0.001, 0.0, 0.0));

                    foreach (DBObject obj in objs) {

                        if (obj.GetType().Name != "Line") { continue; } // Tylko linie są brane pod uwagę
                        Line nent = (Line)obj;

                        if (nent.Visible) {

                            if (i == 0) { first = new Point2d(nent.StartPoint.X, nent.StartPoint.Y); }

                            string temp = $"{Math.Round(nent.StartPoint.X, 2)}{Math.Round(nent.StartPoint.Y, 2)}";
                            Point2d pt2d = new Point2d(nent.StartPoint.X, nent.StartPoint.Y);

                            if (!used.Contains(temp)) {
                                used.Add(temp);
                                unsorted2d.Add(pt2d);
                            }

                            temp = $"{Math.Round(nent.EndPoint.X, 2)}{Math.Round(nent.EndPoint.Y, 2)}";
                            pt2d = new Point2d(nent.EndPoint.X, nent.EndPoint.Y);

                            if (!used.Contains(temp)) {
                                used.Add(temp);
                                unsorted2d.Add(pt2d);
                            }

                            double line_l = nent.Length;
                            if ((h == 0.0) || (line_l < h)) { h = line_l; }
                            if ((w == 0.0) || (line_l > w)) {
                                w = line_l;
                                longest_line = nent;
                            }
                        }
                    }

                    //pline.Layer = parent_layer;
                    pline.Closed = true;

                    ent.Highlight(); // wł podświetlenie bloku
                    ed.UpdateScreen();

                    Point2d[] raw = unsorted2d.ToArray();
                    foreach (Point2d s2d in unsorted2d) {pline.AddVertexAt(i, s2d, 0.0, 0.0, 0.0);} // Polilinia z kolekcji punktów 2d
                    (Point2d min2d, Point2d max2d) = getMinMax2dFromArray(raw); // określenie punktów min i max z array
                    zoomToXY(ed, min2d, max2d); // zoom do bloku

                    cFileDlg selector = new cFileDlg();
                    string stg = selector.VieportDialog(defScale, defActive);

                    ent.Unhighlight(); // wył podświetlenie bloku

                    if (stg == "err") { continue; } // ominięcie rekordu
                    if (stg == "ForceStop!") { return; } // koniec działania programu

                    string[] sstg = stg.Split(';');
                    string layout_name = sstg[0];
                    double vieport_scale = double.Parse(sstg[1]);
                    bool VPlock = bool.Parse(sstg[2]);
                    bool activeLayer = bool.Parse(sstg[3]);
                    defScale = sstg[1]; // Zmiana defult scale na ostatnią
                    defActive = sstg[3];

                    // https://adndevblog.typepad.com/autocad/2012/05/listing-the-layout-names.html
                    LayoutManager lm = LayoutManager.Current;
                    if (!activeLayer) { 
                        pline.Layer = parent_layer;
                    }

                    lm.CurrentLayout = layout_name;

                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord ps = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.PaperSpace], OpenMode.ForWrite);

                    ObjectId id = ps.AppendEntity(pline);
                    tr.AddNewlyCreatedDBObject(pline, true);

                    Viewport vp = new Viewport();
                    

                    if (!activeLayer) {
                        vp.Layer = parent_layer;
                    }

                    ps.AppendEntity(vp);
                    tr.AddNewlyCreatedDBObject(vp, true);

                    // Usytuowanie maski viewportu
                    // var resMat = Matrix3d.Identity;
                    // Point3d p1 = new Point3d(min2d.X, min2d.Y, 0);
                    Point3d p2 = new Point3d(0, 0, 0);
                    Vector3d v = (p2 - origin);
                    Matrix3d mat = Matrix3d.Displacement(v); // ed.WriteMessage($"\nvector: {v}");
                    pline.TransformBy(mat);
                    //

                    double sumX = 0.0;
                    double sumY = 0.0;
                    int vrt = pline.NumberOfVertices;

                    for (int jk = 0; jk < (pline.NumberOfVertices); jk++) {
                        sumX += Math.Round(pline.GetPoint2dAt(jk).X, 3);
                        sumY += Math.Round(pline.GetPoint2dAt(jk).Y, 3);
                    }

                    sumX = sumX / vrt;
                    sumY = sumY / vrt;
                    Point3d vieport_centroid = new Point3d(sumX, sumY, 0.0);

                    // skala
                    if (vieport_scale != 1.0) {
                        Matrix3d scalingMatrix = Matrix3d.Scaling(vieport_scale, vieport_centroid);
                        pline.TransformBy(scalingMatrix);
                    }
                    //

                    vp.CenterPoint = vieport_centroid; // vieport position      
                    vp.Height = h * vieport_scale; //vp.Height = h;
                    vp.ViewHeight = h;
                    vp.ViewCenter = min2d + ((max2d - min2d) / 2.0); // model position
                    vp.NonRectClipEntityId = id;
                    vp.NonRectClipOn = true;
                    vp.Locked = true; // Blokowanie rzutni
                    vp.On = VPlock;

                    // Rotacja plinii
                    // Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
                    // CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;
                    // double angle = longest_line.Angle;
                    // if (angle > Math.PI) {angle = (2 * Math.PI) - longest_line.Angle;}
                    // pline.TransformBy(Matrix3d.Rotation(angle, curUCS.Zaxis, new Point3d(0, 0, 0)));
                    // ed.WriteMessage($"\n{longest_line.Angle}\t{angle}\n");

                    tr.Commit();
                }
                if (doc.Database.TileMode == false) { db.TileMode = true; } // model
            }
            db.TileMode = false;
        }

        private void zoomToXY(Editor ed, Point2d min2d, Point2d max2d) {

            ViewTableRecord view = new ViewTableRecord();
            view.CenterPoint = min2d + ((max2d - min2d) / 2.0);
            view.Height = max2d.Y - min2d.Y;
            view.Width = max2d.X - min2d.X;
            ed.SetCurrentView(view);

        }

        private Tuple<Point2d, Point2d> getMinMax2dFromArray(Point2d[] raw) {
            Array.Sort(raw, new cPointSort.sort2dByX());
            double xmin = raw[0].X;
            double xmax = raw.Last().X;
            Array.Sort(raw, new cPointSort.sort2dByY());

            Point2d min2d = new Point2d(xmin, raw[0].Y);
            Point2d max2d = new Point2d(xmax, raw.Last().Y);

            return Tuple.Create(min2d, max2d);
        }

        public List<ObjectId> GetPolylineIds(PromptSelectionResult selectionResult, string type = "poly") {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            List<ObjectId> list_ids = new List<ObjectId>();
            SelectionSet selectionSet = selectionResult.Value;
            long lid = 0;

            foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    using (Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity) {
                        if (type == "poly") {
                            if (entity.GetType().Name == "Line" ||
                                entity.GetType().Name == "Polyline" ||
                                entity.GetType().Name == "Polyline3d" ||
                                entity.GetType().Name == "Polyline2d") {
                                list_ids.Add(selectedObject.ObjectId);
                                lid++;
                            }
                        }
                    }
                }
            }
            //ed.WriteMessage($"\nElementów typu {type}: {lid}");
            return list_ids;
        }

        private Point2d getVrt2d(Transaction tr, Entity ent, int idx) {

            string name = ent.GetType().Name;
            int i = 0;

            switch (name) {
                case "Polyline":

                    Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                    pline = ent as Autodesk.AutoCAD.DatabaseServices.Polyline;
                    return pline.GetPoint2dAt(idx);
                case "Polyline3d":

                    Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                    pline3d = ent as Autodesk.AutoCAD.DatabaseServices.Polyline3d;

                    foreach (ObjectId vId in pline3d) {

                        PolylineVertex3d v3d = (PolylineVertex3d)tr.GetObject(vId, OpenMode.ForRead);
                        if (i == idx) { return new Point2d(v3d.Position.X, v3d.Position.Y); }
                        i++;

                    }
                    break;
                case "Polyline2d":

                    Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();
                    pline2d = ent as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                    foreach (ObjectId vId in pline2d) {

                        Vertex2d v2d = (Vertex2d)tr.GetObject(vId, OpenMode.ForRead);
                        if (i == idx) { return new Point2d(v2d.Position.X, v2d.Position.Y); }
                        i++;

                    }
                    break;

            }

            return new Point2d(0, 0);

        }

        private Point2d[] getVrt2dAll(Transaction tr, Entity ent) {

            string name = ent.GetType().Name;
            int i = 0;
            int j = 0;



            switch (name) {
                case "Polyline":

                    Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                    pline = ent as Autodesk.AutoCAD.DatabaseServices.Polyline;

                    Point2d[] resultPoly = new Point2d[pline.NumberOfVertices];
                    for (int jk = 0; jk < (pline.NumberOfVertices); jk++) {
                        resultPoly[jk] = pline.GetPoint2dAt(jk);
                    }
                    return resultPoly;

                case "Polyline3d":

                    Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                    pline3d = ent as Autodesk.AutoCAD.DatabaseServices.Polyline3d;

                    foreach (ObjectId vId in pline3d) { i++; }
                    Point2d[] result3d = new Point2d[i];
                    foreach (ObjectId vId in pline3d) {

                        PolylineVertex3d v3d = (PolylineVertex3d)tr.GetObject(vId, OpenMode.ForRead);
                        Point2d temp3d = new Point2d(v3d.Position.X, v3d.Position.Y);
                        result3d[j] = temp3d;
                        j++;
                    }
                    return result3d;
                case "Polyline2d":

                    Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();
                    pline2d = ent as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                    foreach (ObjectId vId in pline2d) { i++; }
                    Point2d[] result2d = new Point2d[i];


                    foreach (ObjectId vId in pline2d) {

                        Vertex2d v2d = (Vertex2d)tr.GetObject(vId, OpenMode.ForRead);
                        Point2d temp2d = new Point2d(v2d.Position.X, v2d.Position.Y);
                        result2d[j] = temp2d;
                        j++;
                    }
                    return result2d;

            }

            Point2d[] result = new Point2d[2];

            return result;

        }

        private int countVrt(Transaction tr, Entity ent) {
            string name = ent.GetType().Name;
            int i = 0;

            switch (name) {
                case "Polyline":

                    Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                    pline = ent as Autodesk.AutoCAD.DatabaseServices.Polyline;

                    i =pline.NumberOfVertices;
                    break;

                case "Polyline3d":

                    Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                    pline3d = ent as Autodesk.AutoCAD.DatabaseServices.Polyline3d;

                    foreach (ObjectId vId in pline3d) { i++; }
                    break;
                case "Polyline2d":

                    Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();
                    pline2d = ent as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                    foreach (ObjectId vId in pline2d) { i++; }
                    break;

            }

            return i;

        }

        private Tuple<double, double> sumVrt2(Transaction tr, Entity ent) {

            string name = ent.GetType().Name;
            int i = 0;
            double sumX = 0.0;
            double sumY = 0.0;

            switch (name) {
                case "Polyline":

                    Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                    pline = ent as Autodesk.AutoCAD.DatabaseServices.Polyline;

                    Point2d[] resultPoly = new Point2d[pline.NumberOfVertices];
                    for (int jk = 0; jk < (pline.NumberOfVertices); jk++) {
                        sumX += pline.GetPoint2dAt(jk).X;
                        sumY += pline.GetPoint2dAt(jk).Y;
                    }
                    break;

                case "Polyline3d":

                    Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                    pline3d = ent as Autodesk.AutoCAD.DatabaseServices.Polyline3d;

                    foreach (ObjectId vId in pline3d) { i++; }
                    Point2d[] result3d = new Point2d[i];
                    foreach (ObjectId vId in pline3d) {

                        PolylineVertex3d v3d = (PolylineVertex3d)tr.GetObject(vId, OpenMode.ForRead);
                        sumX += v3d.Position.X;
                        sumY += v3d.Position.Y;
                    }
                    break;
                case "Polyline2d":

                    Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();
                    pline2d = ent as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                    foreach (ObjectId vId in pline2d) { i++; }
                    Point2d[] result2d = new Point2d[i];


                    foreach (ObjectId vId in pline2d) {

                        Vertex2d v2d = (Vertex2d)tr.GetObject(vId, OpenMode.ForRead);
                        sumX += v2d.Position.X;
                        sumY += v2d.Position.Y;
                    }
                    break;

            }

            return Tuple.Create(sumX, sumY);

        }
    }
}
