using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;

namespace Geo_geo.Class {
    internal class cGeneralize {

        public void SimplifyGeometry(double angleDist = 0.1805) {
                        //angleDist = 0.1805


            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            cObrot cO = new cObrot();


            PromptSelectionResult selection = ed.GetSelection();
            if (selection.Status != PromptStatus.OK)
                return;

            int lid = 0;

            List<ObjectId> line_ids = new List<ObjectId>();

            line_ids = cO.GetIds(selection, "CircleArc");
            int allObj = line_ids.Count;

            if (line_ids.Count > 0) {

                foreach (ObjectId line_id in line_ids) {
                    using (Transaction trans = db.TransactionManager.StartTransaction()) {
                        using (Entity entity = trans.GetObject(line_id, OpenMode.ForRead) as Entity) {

                            if (entity.GetType().Name == "Circle") {

                                //ed.WriteMessage($"\nIt's circle !!!");

                                Circle acCirc = entity as Circle;
                                //string layer = acCirc.Layer;
                                double radius = acCirc.Radius;
                                Point3d oldPoint = acCirc.Center;

                                Point2d tempPoint = new Point2d();
                                using (Polyline simplifyCircle = new Polyline()) {

                                    int vNum = 0;

                                    for (double itr = 0.0; itr < 6.28; itr += angleDist) {

                                        tempPoint = new Point2d(oldPoint.X + (radius * Math.Sin(itr)),
                                                                oldPoint.Y + (radius * Math.Cos(itr)));
                                        simplifyCircle.AddVertexAt(vNum, tempPoint, 0, 0, 0);
                                        //ed.WriteMessage($"\nVertex: {vNum}, {tempPoint.X}, {tempPoint.Y}");
                                        vNum++;
                                    }

                                    tempPoint = new Point2d(oldPoint.X + (radius * Math.Sin(Math.PI * 0.0)),
                                                            oldPoint.Y + (radius * Math.Cos(Math.PI * 0.0)));
                                    simplifyCircle.AddVertexAt(vNum, tempPoint, 0, 0, 0);
                                    //ed.WriteMessage($"\nVertex: {vNum}, {tempPoint.X}, {tempPoint.Y}");

                                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                    btr.AppendEntity(simplifyCircle);
                                    trans.AddNewlyCreatedDBObject(simplifyCircle, true);

                                    //ed.WriteMessage($"\nAdding simplify circle...");
                                }

                                trans.Commit();

                            } else if (entity.GetType().Name == "Arc") {
                                //ed.WriteMessage($"\nIt's arc !!!");

                                Arc acArc = entity as Arc;

                                double radius = acArc.Radius;

                                Point3d oldPoint = acArc.Center;
                                Point2d tempPoint = new Point2d();

                                double angleMin = acArc.StartAngle;
                                double angleMax = acArc.EndAngle;
                                Point3d fPoint = new Point3d(acArc.StartPoint.X, acArc.StartPoint.Y, 0);
                                Point3d lPoint = new Point3d(acArc.EndPoint.X, acArc.EndPoint.Y, 0);

                                Line dummy = new Line(oldPoint, fPoint);
                                double startAngle = -(dummy.Angle - (Math.PI / 2));

                                bool reverse = false;
                                double diff = acArc.TotalAngle;

                                if (diff < 0) { diff = -diff; } // can't be

                                using (Polyline simplifyCircle = new Polyline()) {

                                    int vNum = 0;

                                    for (double itr = 0.0; itr < diff; itr += angleDist) {

                                        tempPoint = new Point2d(oldPoint.X + (radius * Math.Sin(startAngle - itr)),
                                                                oldPoint.Y + (radius * Math.Cos(startAngle - itr)));
                                        simplifyCircle.AddVertexAt(vNum, tempPoint, 0, 0, 0);
                                        //ed.WriteMessage($"\nVertex: {vNum}, {tempPoint.X}, {tempPoint.Y}");
                                        vNum++;
                                    }

                                    tempPoint = new Point2d(oldPoint.X + (radius * Math.Sin(startAngle - diff)),
                                                            oldPoint.Y + (radius * Math.Cos(startAngle - diff)));
                                    simplifyCircle.AddVertexAt(vNum, tempPoint, 0, 0, 0);

                                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                    btr.AppendEntity(simplifyCircle);
                                    trans.AddNewlyCreatedDBObject(simplifyCircle, true);
                                }

                                trans.Commit();
                                //ed.WriteMessage($"\n{acArc.StartPoint}\t{acArc.StartAngle}\t{acArc.EndAngle}");

                            } else {
                                continue;
                            }

                            lid++;
                            //ed.WriteMessage($"\n{lid}/{allObj}");

                        }
                    }
                }
            }
        }
    }
}
