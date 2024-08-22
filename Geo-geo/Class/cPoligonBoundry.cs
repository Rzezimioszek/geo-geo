using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Geo_geo.Class {
    internal class cPoligonBoundry {

        public void IsPointInsidePoliline() {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionResult selection = ed.GetSelection();
            if (selection.Status != PromptStatus.OK)
                return;

            cObrot cO = new cObrot();
            List<ObjectId> text_ids = new List<ObjectId>();

            text_ids = cO.GetIds(selection, "text");

            int _index = 1;

            if (text_ids.Count > 0) {

                foreach (ObjectId text_id in text_ids) {
                    using (Transaction trans = db.TransactionManager.StartTransaction()) {
                        using (Entity entity = trans.GetObject(text_id, OpenMode.ForRead) as Entity) {

                            string objType = entity.GetType().Name;

                            if (objType == "DBText") {

                                DBText dbText = entity as DBText;

                                double wspX = dbText.Position.X;
                                double wspY = dbText.Position.Y;
                                double wspH = dbText.Position.Z;

                                Point3d point = new Point3d(wspX, wspY, wspH);


                                try {
                                    var objs = ed.TraceBoundary(point, true);




                                    if (objs.Count > 0) {

                                        ed.WriteMessage($"\nIs in!!!\t{objs.Count}");
                                        Transaction tr = doc.TransactionManager.StartTransaction();

                                        // Add our boundary objects to the drawing and
                                        // collect their ObjectIds for later use
                                        //ObjectIdCollection ids = new ObjectIdCollection();

                                        foreach (DBObject obj in objs) {

                                            Entity ent = obj as Entity;

                                            if (ent != null) {

                                                using (tr) {

                                                    // We'll add the objects to the model space
                                                    BlockTable bt = (BlockTable)tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
                                                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace],
                                                        OpenMode.ForWrite);

                                                    // Set our boundary objects to be of
                                                    // our auto-incremented colour index
                                                    //ent.UpgradeOpen(); // <-- Czy  to działa??? Nie
                                                    ent.ColorIndex = _index;


                                                    // Set the lineweight of our object

                                                    ent.LineWeight = LineWeight.LineWeight050;

                                                    // Add each boundary object to the modelspace
                                                    // and add its ID to a collection

                                                    //ids.Add(btr.AppendEntity(ent));
                                                    btr.AppendEntity(ent);
                                                    tr.AddNewlyCreatedDBObject(ent, true);
                                                    tr.Commit();
                                                }
                                            }
                                        }

                                        // Increment our colour index
                                        _index++;
                                        if (_index > 6) { _index = 1; }

                                        // Commit the transaction
                                        //tr.Commit();

                                    }

                                } catch { continue; }

                            }
                        }
                        trans.Commit();
                    }
                }
            }
        }


        public void SelectPoligonCrosingPoint() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionResult selection = ed.GetSelection();
            if (selection.Status != PromptStatus.OK)
                return;

            cObrot cO = new cObrot();
            List<ObjectId> text_ids = new List<ObjectId>();

            text_ids = cO.GetIds(selection, "text");

            TypedValue[] typedVal = new TypedValue[1];

            typedVal[0] = new TypedValue((int)DxfCode.Start, "Line, LWPOLYLINE, POLYLINE");
            SelectionFilter selFilter = new SelectionFilter(typedVal);

            int _index = 1;


            if (text_ids.Count > 0) {

                foreach (ObjectId text_id in text_ids) {


                    Point3dCollection pntCol = new Point3dCollection();
                    Point3d point = new Point3d();

                    using (Transaction trans = db.TransactionManager.StartTransaction()) {
                        using (Entity entity = trans.GetObject(text_id, OpenMode.ForRead) as Entity) {

                            string objType = entity.GetType().Name;


                            if (objType == "DBText") {


                                DBText dbText = entity as DBText;



                                double wspX = dbText.Position.X;
                                double wspY = dbText.Position.Y;
                                double wspH = dbText.Position.Z;


                                point = new Point3d(wspX, wspY, wspH);

                                /*
                                pntCol.Add(point);

                                point = new Point3d(wspX + 0.0001, wspY, wspH);
                                pntCol.Add(point);

                                point = new Point3d(wspX, wspY + 0.0001, wspH);
                                pntCol.Add(point);
                                */

                                var objs = ed.TraceBoundary(point, true);

                                foreach (DBObject obj in objs) {


                                    Autodesk.AutoCAD.DatabaseServices.Polyline ent = obj as Autodesk.AutoCAD.DatabaseServices.Polyline;

                                    if (ent != null) {

                                        for (int jk = 0; jk < (ent.NumberOfVertices - 1); jk++) {

                                            wspX = ent.GetPoint2dAt(jk).X;
                                            wspY = ent.GetPoint2dAt(jk).Y;

                                            point = new Point3d(wspX, wspY, 0.0);
                                            pntCol.Add(point);

                                        }
                                    }

                                }
                            }
                        }

                        try {
                            PromptSelectionResult pmtSelRes = null;
                            //pmtSelRes = ed.SelectCrossingPolygon(pntCol);//, selFilter);SelectAtPoint

                            //pmtSelRes = ed.SelectWindowPolygon(pntCol);
                            pmtSelRes = ed.SelectFence(pntCol);

                            if (pmtSelRes.Value.Count > 0) {

                                foreach (ObjectId objId in pmtSelRes.Value.GetObjectIds()) {

                                    using (Entity entity = trans.GetObject(objId, OpenMode.ForWrite) as Entity) {

                                        if (entity.GetType().Name == "Polyline") {

                                            entity.ColorIndex = 1;
                                            trans.Commit();

                                        }

                                        //entity.ColorIndex = 1;

                                    }
                                    //trans.Commit();

                                    //_index++;
                                    //if (_index > 6) { _index = 1; }

                                }
                            }
                        } catch { ed.WriteMessage($"\nerror?!"); }
                    }
                }
            }
        }

        public void MovePoligonIfPointInside() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionResult selection = ed.GetSelection();
            if (selection.Status != PromptStatus.OK)
                return;

            cObrot cO = new cObrot();
            List<ObjectId> line_ids = new List<ObjectId>();

            line_ids = cO.GetIds(selection, "line");

            int allObj = line_ids.Count;
            int corObj = 0;
            int erObj = 0;


            if (line_ids.Count > 0) {

                foreach (ObjectId line_id in line_ids) {


                    double wspX = 0.0;
                    double wspY = 0.0;
                    double wspH = 0.0;

                    using (Transaction trans = db.TransactionManager.StartTransaction()) {
                        using (Entity entity = trans.GetObject(line_id, OpenMode.ForWrite) as Entity) {

                            Point3dCollection pntCol = new Point3dCollection();
                            Point3d point = new Point3d();
                            string newLayer = "0";

                            if (entity.GetType().Name == "Polyline") {
                                Autodesk.AutoCAD.DatabaseServices.Polyline ent = entity as Autodesk.AutoCAD.DatabaseServices.Polyline;

                                for (int jk = 0; jk < (ent.NumberOfVertices - 1); jk++) {

                                    wspX = ent.GetPoint2dAt(jk).X;
                                    wspY = ent.GetPoint2dAt(jk).Y;

                                    point = new Point3d(wspX, wspY, 0.0);
                                    pntCol.Add(point);

                                }
                            } else if (entity.GetType().Name == "Polyline3d") {

                                Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = entity as Autodesk.AutoCAD.DatabaseServices.Polyline3d;

                                if (pline3d != null) {

                                    foreach (ObjectId vId in pline3d) {

                                        PolylineVertex3d v3d = (PolylineVertex3d)trans.GetObject(vId, OpenMode.ForRead);

                                        wspX = v3d.Position.X;
                                        wspY = v3d.Position.Y;
                                        wspH = v3d.Position.Z;
                                        point = new Point3d(wspX, wspY, 0.0);
                                        pntCol.Add(point);
                                    }

                                }
                            } else if (entity.GetType().Name == "Polyline2d") {

                                Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = entity as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                                if (pline2d != null) {

                                    foreach (ObjectId vId in pline2d) {

                                        Vertex2d v2d = (Vertex2d)trans.GetObject(vId, OpenMode.ForRead);

                                        wspX = v2d.Position.X;
                                        wspY = v2d.Position.Y;
                                        wspH = 0.0;
                                        point = new Point3d(wspX, wspY, 0.0);
                                        pntCol.Add(point);
                                    }
                                }
                            }

                            try {
                                PromptSelectionResult pmtSelRes = null;
                                pmtSelRes = ed.SelectWindowPolygon(pntCol);


                                if (pmtSelRes.Value.Count > 0) {

                                    bool create = false;

                                    foreach (ObjectId objId in pmtSelRes.Value.GetObjectIds()) {

                                        using (Entity en = trans.GetObject(objId, OpenMode.ForRead) as Entity) {

                                            ed.WriteMessage($"\n{en.GetType().Name}");

                                            if (en.GetType().Name == "DBText") {

                                                DBText textObj = en as DBText;
                                                newLayer = textObj.TextString;
                                                newLayer = newLayer.Replace("/", "_");

                                            } else if (en.GetType().Name == "MText") {

                                                MText textMObj = entity as MText;
                                                newLayer = textMObj.Contents;
                                                newLayer = newLayer.Replace("/", "_");

                                            } else if (en.GetType().Name == "BlockReference") {
                                                BlockReference blockRef = entity as BlockReference;

                                                AttributeCollection attCol = blockRef.AttributeCollection;
                                                foreach (ObjectId attId in attCol) {

                                                    AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);
                                                    if (attRef.Tag == "ID") { newLayer = attRef.TextString; }

                                                }
                                                newLayer = newLayer.Replace("/", "_");
                                            }

                                            using (LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead)) {
                                                create = !lt.Has(newLayer);
                                            }
                                            //ed.WriteMessage($"\nNew layer: {newLayer}");
                                        }
                                    }

                                    if (newLayer != "0") {
                                        if (create) {
                                            using (LayerTableRecord ltr = new LayerTableRecord()) {

                                                ltr.Name = newLayer;
                                                ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, 3);

                                                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                                    LayerTable wlt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForWrite);

                                                    //lt.UpgradeOpen();
                                                    wlt.Add(ltr);
                                                    tr.AddNewlyCreatedDBObject(ltr, true);
                                                    tr.Commit();
                                                }

                                                entity.Layer = newLayer;
                                                trans.Commit();
                                            }
                                        } else {

                                            entity.ColorIndex = 1;
                                            entity.Layer = newLayer;
                                            trans.Commit();
                                        }
                                    } else { trans.Commit(); }
                                    corObj++;
                                }
                            } catch {

                                ed.WriteMessage($"\nError {erObj}");
                                erObj++;
                                continue;
                            }
                        }
                    }
                }
            }
            ed.WriteMessage($"\n\nAll: {allObj}\nCorrect: {corObj}\nError: {erObj}");
        }
    }
}
