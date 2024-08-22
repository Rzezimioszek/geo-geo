using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Geo_geo.Class {
    internal class cPointAtSurface {

        public void getPointAtSurface() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            int lp = 0;

            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            List<ObjectId> surface_ids = new List<ObjectId>();
            List<ObjectId> text_ids = new List<ObjectId>();

            surface_ids = GetIds(selectionResult, "surface");
            text_ids = GetIds(selectionResult, "text");

            foreach (ObjectId surf_id in surface_ids) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    Entity surfEntity = trans.GetObject(surf_id, OpenMode.ForRead) as Entity;
                    if (surfEntity == null) {
                        continue;
                    }

                    Autodesk.AutoCAD.DatabaseServices.Surface surf = surfEntity as Autodesk.AutoCAD.DatabaseServices.Surface;

                    var ucs = ed.CurrentUserCoordinateSystem;

                    for (int i = 0; i < text_ids.Count; i++) {

                        Entity entity = trans.GetObject(text_ids[i], OpenMode.ForRead) as Entity;
                        string objType = entity.GetType().Name;

                        Point3d curPoint = new Point3d();

                        if (objType == "BlockReference") {

                            BlockReference blockRef = entity as BlockReference;
                            curPoint = new Point3d(blockRef.Position.X, blockRef.Position.Y, 0.0);

                        } else if (objType == "DBText") {

                            DBText textObj = entity as DBText;
                            curPoint = new Point3d(textObj.Position.X, textObj.Position.Y, 0.0);

                        } else if (objType == "MText") {

                            MText textMObj = entity as MText;
                            curPoint = new Point3d(textMObj.Location.X, textMObj.Location.Y, 0.0);

                        } else {
                            continue;
                        }

                        /* For test only
                        if (curPoint.Z != 0.0) {
                            continue;
                        }
                        */

                        DoubleCollection heights;
                        SubentityId[] ids;

                        Point3d newPoint = curPoint.TransformBy(ucs);
                        double h = 0.0;

                        try {

                            if (surfEntity.GetType().Name == "PlaneSurface") {
                                PlaneSurface planeSurface = surfEntity as PlaneSurface;

                                planeSurface.RayTest(newPoint, ucs.CoordinateSystem3d.Zaxis, 0.01, out ids, out heights);

                            } else if (surfEntity.GetType().Name == "Surface") {

                                surf.RayTest(newPoint, ucs.CoordinateSystem3d.Zaxis, 0.01, out ids, out heights);

                            } else if (surfEntity.GetType().Name == "Face") {


                                Face face = surfEntity as Face;
                                Point3dCollection pts = new Point3dCollection();

                                //Point3d last = new Point3d();

                                for (short k = 0; k < 4; k++) {

                                    Point3d tp = face.GetVertexAt(k);

                                    /*if (last != tp) {
                                        pts.Add(tp);
                                        ed.WriteMessage($"\n{k}:\t{pts[k].X} {pts[k].Y}");
                                    }*/

                                    if (!pts.Contains(tp)) {
                                        pts.Add(tp);
                                        //ed.WriteMessage($"\n{k}:\t{pts[k].X} {pts[k].Y}");
                                    }

                                    //last = face.GetVertexAt(k);

                                }

                                if(pts.Count < 3)
                                    continue;


                                Polyline3d poly3d = new Polyline3d(Poly3dType.SimplePoly, pts, true);

                                Autodesk.AutoCAD.DatabaseServices.Surface tempSurf = Autodesk.AutoCAD.DatabaseServices.Surface.CreateFrom(poly3d);

                                tempSurf.RayTest(newPoint, ucs.CoordinateSystem3d.Zaxis, 0.01, out ids, out heights);


                                //continue; 
                            } else { continue; }

                            //face.IntersectWith(newPoint, Intersect.ExtendBoth, )


                            if (ids.Length == 0) {

                                ed.WriteMessage("\nNo intersections found.");
                                continue;
                            }

                            h = heights[0];
                            //ed.WriteMessage($"\nMoved to: {newPoint.X}, {newPoint.Y}, {h}");

                            if (h == 0.0) { continue; }

                            // doc.LockDocument();

                            using (Transaction transModify = db.TransactionManager.StartTransaction()) {

                                Entity entityModify = transModify.GetObject(text_ids[i], OpenMode.ForWrite) as Entity;

                                if (objType == "BlockReference") {

                                    BlockReference blockRef = entityModify as BlockReference;
                                    blockRef.UpgradeOpen();
                                    blockRef.ScaleFactors = new Scale3d(1, 1, 0.00001);
                                    blockRef.Position = new Point3d(curPoint.X, curPoint.Y, h);
                                    transModify.Commit();

                                } else if (objType == "DBText") {

                                    DBText textObj = entityModify as DBText;
                                    textObj.UpgradeOpen();
                                    textObj.Position = new Point3d(curPoint.X, curPoint.Y, h);
                                    transModify.Commit();

                                } else if (objType == "MText") {

                                    MText textMObj = entityModify as MText;
                                    textMObj.UpgradeOpen();
                                    textMObj.Location = new Point3d(curPoint.X, curPoint.Y, h);
                                    transModify.Commit();
                                }

                            }

                            text_ids.RemoveAt(i);
                            i--;


                            /*

                            if (i < 0) 
                                i = 0;
                            */

                            lp++;

                            //ed.WriteMessage($"\n::{lp}");

                        } catch (Exception ex) {
                            ed.WriteMessage($"\nCatch:\n{ex.Message}");
                            continue;
                        }
                    }

                    trans.Commit();
                }
            }
            ed.WriteMessage($"\nPunkty przesunięte: {lp}");
        }

        public List<ObjectId> GetIds(PromptSelectionResult selectionResult, string type = "text") {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            List<ObjectId> list_ids = new List<ObjectId>();

            SelectionSet selectionSet = selectionResult.Value;

            long lid = 0;


            foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    using (Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity) {

                        /*(entity.GetType().Name == "AcDbFace" ||
                                entity.GetType().Name == "Face")*/

                        if (type == "surface") {
                            if (entity.GetType().Name == "Surface" ||
                                entity.GetType().Name == "PlaneSurface" ||
                                entity.GetType().Name == "Face") {

                                list_ids.Add(selectedObject.ObjectId);
                                lid++;
                            }
                        } else if (type == "text") {

                            if (entity.GetType().Name == "BlockReference" ||
                                entity.GetType().Name == "DBText" ||
                                entity.GetType().Name == "MText") {

                                list_ids.Add(selectedObject.ObjectId);
                                lid++;
                            }
                        }
                    }
                }
            }
            ed.WriteMessage($"\nElementów typu {type}: {lid}");



            return list_ids;
        }


        public void PointOnXY() {

            var doc = Application.DocumentManager.MdiActiveDocument;

            var db = doc.Database;

            var ed = doc.Editor;



            // Ask the user to pick a surface



            var peo = new PromptEntityOptions("\nSelect a surface");

            peo.SetRejectMessage("\nMust be a surface.");

            peo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Surface), false);



            var per = ed.GetEntity(peo);

            if (per.Status != PromptStatus.OK)

                return;



            using (var tr = db.TransactionManager.StartTransaction()) {

                // We'll start by getting the block table and modelspace

                // (which are really only needed for results we'll add to

                // the database)



                var bt =

                  (BlockTable)tr.GetObject(

                    db.BlockTableId, OpenMode.ForRead

                  );



                var btr =

                  (BlockTableRecord)tr.GetObject(

                    bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite

                  );



                var obj = tr.GetObject(per.ObjectId, OpenMode.ForRead);

                var surf = obj as Autodesk.AutoCAD.DatabaseServices.Surface;

                if (surf == null) {

                    ed.WriteMessage("\nFirst object must be a surface.");

                } else {

                    DoubleCollection heights;

                    SubentityId[] ids;



                    // Fire ray for each selected line



                    var ucs = ed.CurrentUserCoordinateSystem;

                    PromptPointResult ppr;

                    do {

                        ppr = ed.GetPoint("\nSelect point beneath surface");

                        if (ppr.Status != PromptStatus.OK) {

                            break;

                        }



                        // Our selected point needs to be transformed to WCS

                        // from the current UCS



                        var start = ppr.Value.TransformBy(ucs);



                        // Fire a ray from the selected point in the direction

                        // of the UCS' Z-axis



                        surf.RayTest(

                          start,

                          ucs.CoordinateSystem3d.Zaxis,

                          0.01,

                          out ids,

                          out heights

                        );



                        if (ids.Length == 0) {

                            ed.WriteMessage("\nNo intersections found.");

                        } else {

                            // Add point at each intersection found



                            for (int i = 0; i < ids.Length; i++) {

                                // Create a point at the intersection



                                var end =

                                  start +

                                  new Vector3d(0, 0, heights[i]).TransformBy(ucs);



                                var pt = new DBPoint(end);

                                var ln = new Line(start, end);



                                // Add the new objects to the block table

                                // record and the transaction



                                btr.AppendEntity(pt);

                                tr.AddNewlyCreatedDBObject(pt, true);

                                btr.AppendEntity(ln);

                                tr.AddNewlyCreatedDBObject(ln, true);

                            }

                        }

                    }

                    while (ppr.Status == PromptStatus.OK);

                }

                tr.Commit();

            }

        }
    }
}



