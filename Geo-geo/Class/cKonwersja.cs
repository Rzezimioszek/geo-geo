using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo_geo.Class {
    internal class cKonwersja {

        public void PolilineToPolilineLW() {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            List<ObjectId> line_ids = new List<ObjectId>();

            line_ids = GetIds(selectionResult, "line");

            foreach (ObjectId line_id in line_ids) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    Entity lineEntity = trans.GetObject(line_id, OpenMode.ForWrite) as Entity;
                    if (lineEntity == null) {
                        continue;
                    }

                    Line line = new Line();
                    Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                    Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                    Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();



                    if (lineEntity == null) { continue; }

                    if (lineEntity.GetType().Name == "Line") {

                        line = lineEntity as Line;

                        Autodesk.AutoCAD.DatabaseServices.Polyline newLine = new Autodesk.AutoCAD.DatabaseServices.Polyline();

                        int i = 0;

                        Point2d pt2d = new Point2d(line.StartPoint.X, line.StartPoint.Y);
                        newLine.AddVertexAt(i, pt2d, 0.0, 0.0, 0.0);

                        i = 1;

                        pt2d = new Point2d(line.EndPoint.X, line.EndPoint.Y);
                        newLine.AddVertexAt(i, pt2d, 0.0, 0.0, 0.0);

                        newLine.Layer = line.Layer;

                        addPolilineLW(newLine);

                        line.Erase();

                    } else if (lineEntity.GetType().Name == "Polyline") {

                        pline = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline;

                        // TODO poprawa istniejacych linii z zerowymi segementami

                        
                    } else if (lineEntity.GetType().Name == "Polyline3d") {

                        //Polyline
                        pline3d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline3d;

                        Autodesk.AutoCAD.DatabaseServices.Polyline newLine = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                        int i = 0;
                        double lastX = 0.001;
                        double lastY = 0.001;

                        if (pline3d != null) {

                            foreach (ObjectId vId in pline3d) {

                                PolylineVertex3d v3d = (PolylineVertex3d)trans.GetObject(vId, OpenMode.ForRead);

                                if ((Math.Round(v3d.Position.X, 3) == lastX) && (Math.Round(v3d.Position.Y, 3) == lastY)) {
                                    continue;
                                } else {
                                    lastX = Math.Round(v3d.Position.X, 3);
                                    lastY = Math.Round(v3d.Position.Y, 3);
                                }


                                Point2d pt2d = new Point2d(v3d.Position.X, v3d.Position.Y);
                                newLine.AddVertexAt(i, pt2d, 0.0, 0.0, 0.0);
                                i ++;
                                

                            }
                            newLine.Layer = pline3d.Layer;

                            addPolilineLW(newLine);

                            pline3d.Erase();

                        }
                    } else if (lineEntity.GetType().Name == "Polyline2d") {

                        pline2d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                        Autodesk.AutoCAD.DatabaseServices.Polyline newLine = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                        int i = 0;
                        double lastX = 0.001;
                        double lastY = 0.001;

                        if (pline2d != null) {

                            foreach (ObjectId vId in pline2d) {

                                Vertex2d v2d = (Vertex2d)trans.GetObject(vId, OpenMode.ForRead);


                                if ((Math.Round(v2d.Position.X, 3) == lastX) && (Math.Round(v2d.Position.Y, 3) == lastY)) {
                                    continue;
                                } else {
                                    lastX = Math.Round(v2d.Position.X, 3);
                                    lastY = Math.Round(v2d.Position.Y, 3);
                                }

                                Point2d pt2d = new Point2d(v2d.Position.X, v2d.Position.Y);
                                newLine.AddVertexAt(i, pt2d, 0.0, 0.0, 0.0);
                                i++;

                            }

                            newLine.Layer = pline2d.Layer;

                            addPolilineLW(newLine);

                            pline2d.Erase();

                        }
                        
                    } else {
                        continue;
                        // lineEntity.GetType().Name != "Polyline"
                    }


                    trans.Commit();
                }
            }
        }

        public void addPolilineLW(Autodesk.AutoCAD.DatabaseServices.Polyline newLine) {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            if (newLine.Length <= 0.0) {
                return;
            }

            using (DocumentLock acLckDoc = doc.LockDocument()) {
                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);


                    btr.AppendEntity(newLine);
                    tr.AddNewlyCreatedDBObject(newLine, true);
                    tr.Commit();
                }
            }


        }

        public List<ObjectId> GetIds(PromptSelectionResult selectionResult, string type = "line") {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            List<ObjectId> list_ids = new List<ObjectId>();

            SelectionSet selectionSet = selectionResult.Value;

            long lid = 0;

            //List<(ObjectId, string)> lt = new List<(ObjectId, string)> ();


            foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    using (Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity) {



                        if (type == "line") {
                            if (entity.GetType().Name == "Line" ||
                                entity.GetType().Name == "Polyline" ||
                                entity.GetType().Name == "Polyline3d" ||
                                entity.GetType().Name == "Polyline2d") {

                                list_ids.Add(selectedObject.ObjectId);

                                //(ObjectId, string) temp_tuple = (selectedObject.ObjectId, entity.GetType().Name);
                                //lt.Add(temp_tuple);
                                //ed.WriteMessage($"{temp_tuple.Item1}---{temp_tuple.Item2}\n");

                                lid++;
                            }
                        } else if (type == "text") {

                            if (entity.GetType().Name == "BlockReference" ||
                                entity.GetType().Name == "DBText" ||
                                entity.GetType().Name == "MText") {

                                list_ids.Add(selectedObject.ObjectId);
                                lid++;
                            }
                        } else if (type == "block") {

                            if (entity.GetType().Name == "BlockReference") {

                                list_ids.Add(selectedObject.ObjectId);
                                lid++;
                            }
                        } else if (type == "line3d") {

                            if (entity.GetType().Name == "Line" ||
                                entity.GetType().Name == "Polyline3d") {

                                list_ids.Add(selectedObject.ObjectId);
                                lid++;
                            }
                        } else if (type == "lineCircle") {
                            if (entity.GetType().Name == "Line" ||
                                entity.GetType().Name == "Polyline" ||
                                entity.GetType().Name == "Polyline3d" ||
                                entity.GetType().Name == "Polyline2d" ||
                                entity.GetType().Name == "Circle" ||
                                entity.GetType().Name == "Arc") {

                                list_ids.Add(selectedObject.ObjectId);
                                lid++;
                            }

                        } else if (type == "CircleArc") {
                            if (entity.GetType().Name == "Arc" ||
                                entity.GetType().Name == "Circle") {

                                list_ids.Add(selectedObject.ObjectId);
                                lid++;
                            }
                        } else if (type == "Polyline") {
                            if (entity.GetType().Name == "Polyline") {

                                list_ids.Add(selectedObject.ObjectId);
                                lid++;
                            }

                        }
                    }
                }
            }
            ed.WriteMessage($"\nElementów typu {type}: {lid}");

            //foreach (ObjectId oi in list_ids) { ed.WriteMessage(oi.ToString()); }


            return list_ids;
        }


    }
}
