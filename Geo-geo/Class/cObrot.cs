using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Geo_geo.Class {
    public class cObrot {

        public void RotateTextToLine() {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            double prec = 0.05;
            bool on_line = false;

            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            SelectionSet selectionSet = selectionResult.Value;
            int liczba_porzadkowa = 0;

            foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity;
                    if (entity == null) {
                        continue;
                    }

                    string objType = entity.GetType().Name;
                    double wspXP = 0.0;
                    double wspYP = 0.0;
                    double orignal_rotation = 0;


                    if (objType == "BlockReference") {
                        BlockReference blockRef = entity as BlockReference;
                        wspXP = blockRef.Position.X;
                        wspYP = blockRef.Position.Y;
                        orignal_rotation = blockRef.Rotation;
                    } else if (objType == "DBText") {
                        DBText textObj = entity as DBText;
                        wspXP = textObj.Position.X;
                        wspYP = textObj.Position.Y;
                        orignal_rotation = textObj.Rotation;
                    } else if (objType == "MText") {
                        MText textMObj = entity as MText;
                        wspXP = textMObj.Location.X;
                        wspXP = textMObj.Location.Y;
                        orignal_rotation = textMObj.Rotation;
                    } else {
                        continue;
                    }
                    ed.WriteMessage(objType + $"\n");


                    foreach (SelectedObject lineSelectedObject in selectionSet) {
                        using (Entity lineEntity = trans.GetObject(lineSelectedObject.ObjectId, OpenMode.ForRead) as Entity) {

                            ed.WriteMessage(lineEntity.GetType().Name + $"\n");


                            Line line = new Line();

                            Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                            Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                            Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();

                            double angle = 0.0;

                            double wspX0 = 0.0;
                            double wspY0 = 0.0;
                            double wspX1 = 0.0;
                            double wspY1 = 0.0;

                            bool change = true;

                            double deltaX01 = 0.0;
                            double deltaY01 = 0.0;

                            double d01 = 0.0;
                            double deltaX0 = 0.0;
                            double deltaY0 = 0.0;

                            double d0P = Math.Sqrt((deltaX0 * deltaX0) + (deltaY0 * deltaY0));

                            if (lineEntity == null) { continue; }


                            if (lineEntity.GetType().Name == "Line") {

                                //Line 
                                line = lineEntity as Line;
                                angle = line.Angle;

                                Point3d startPoint = line.StartPoint;
                                Point3d endPoint = line.EndPoint;

                                wspX0 = startPoint.X;
                                wspY0 = startPoint.Y;
                                wspX1 = endPoint.X;
                                wspY1 = endPoint.Y;

                                change = ChangeRotation(wspXP, wspX0, wspX1, prec);
                                if (!change) {
                                    continue;
                                }

                                change = ChangeRotation(wspYP, wspY0, wspY1, prec);
                                if (!change) {
                                    continue;
                                }

                                deltaX01 = wspX1 - wspX0;
                                deltaY01 = wspY1 - wspY0;

                                d01 = Math.Sqrt((deltaX01 * deltaX01) + (deltaY01 * deltaY01));
                                deltaX0 = wspXP - wspX0;
                                deltaY0 = wspYP - wspY0;

                                d0P = Math.Sqrt((deltaX0 * deltaX0) + (deltaY0 * deltaY0));

                                if (d01 != 0) {
                                    double deltaXP = d0P * (deltaX01 / d01);
                                    double deltaYP = d0P * (deltaY01 / d01);

                                    double new_point_x = wspX0 + deltaXP;
                                    double new_point_y = wspY0 + deltaYP;

                                    // ed.WriteMessage($"First check, {new_point_x}, {new_point_y} :: {wspXP}, {wspYP}\n");

                                    on_line = IsOnLine(wspXP, wspYP, new_point_x, new_point_y, prec);

                                    if (on_line && (orignal_rotation == 0)) {
                                        angle = ValidAngle(angle);

                                        // ed.WriteMessage($"Second check\n");

                                        using (Transaction transModify = db.TransactionManager.StartTransaction()) {


                                            if (objType == "BlockReference") {
                                                BlockReference blockRef = entity as BlockReference;
                                                blockRef.UpgradeOpen();

                                                Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
                                                CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;

                                                if (blockRef.Rotation == 0) {
                                                    blockRef.TransformBy(Matrix3d.Rotation(angle, curUCS.Zaxis, blockRef.Position));
                                                }

                                                transModify.Commit();
                                            } else if (objType == "DBText") {
                                                DBText textObj = entity as DBText;
                                                textObj.UpgradeOpen();
                                                textObj.Rotation = angle;
                                                transModify.Commit();
                                                // ed.WriteMessage($"Third check\n");
                                            } else if (objType == "MText") {
                                                MText textMObj = entity as MText;
                                                textMObj.UpgradeOpen();
                                                textMObj.Rotation = angle;
                                                transModify.Commit();
                                            }
                                        }

                                        liczba_porzadkowa++;
                                        ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} Line");
                                    }
                                }

                            } else if (lineEntity.GetType().Name == "Polyline") {

                                //Polyline
                                pline = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline;


                                for (int jk = 0; jk < (pline.NumberOfVertices - 1); jk++) {

                                    wspX0 = Math.Round(pline.GetPoint2dAt(jk).X, 3);
                                    wspY0 = Math.Round(pline.GetPoint2dAt(jk).Y, 3);
                                    wspX1 = Math.Round(pline.GetPoint2dAt(jk + 1).X, 3);
                                    wspY1 = Math.Round(pline.GetPoint2dAt(jk + 1).Y, 3);

                                    change = ChangeRotation(wspXP, wspX0, wspX1, prec);
                                    if (!change) {
                                        continue;
                                    }

                                    change = ChangeRotation(wspYP, wspY0, wspY1, prec);
                                    if (!change) {
                                        continue;
                                    }


                                    deltaX01 = wspX1 - wspX0;
                                    deltaY01 = wspY1 - wspY0;

                                    d01 = Math.Sqrt((deltaX01 * deltaX01) + (deltaY01 * deltaY01));
                                    deltaX0 = wspXP - wspX0;
                                    deltaY0 = wspYP - wspY0;

                                    d0P = Math.Sqrt((deltaX0 * deltaX0) + (deltaY0 * deltaY0));

                                    if (deltaX01 == 0 && deltaY01 == 0) {

                                        angle = 0;

                                    } else {
                                        angle = Math.Atan2(deltaY01, deltaX01);
                                    }

                                    if (d01 != 0) {
                                        double deltaXP = d0P * (deltaX01 / d01);
                                        double deltaYP = d0P * (deltaY01 / d01);

                                        double new_point_x = wspX0 + deltaXP;
                                        double new_point_y = wspY0 + deltaYP;

                                        // ed.WriteMessage($"First check, {new_point_x}, {new_point_y} :: {wspXP}, {wspYP}\n");

                                        on_line = IsOnLine(wspXP, wspYP, new_point_x, new_point_y, prec);

                                        if (on_line && (orignal_rotation == 0)) {

                                            angle = ValidAngle(angle);

                                            // ed.WriteMessage($"Second check\n");

                                            using (Transaction transModify = db.TransactionManager.StartTransaction()) {


                                                if (objType == "BlockReference") {
                                                    BlockReference blockRef = entity as BlockReference;
                                                    blockRef.UpgradeOpen();

                                                    Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
                                                    CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;

                                                    if (blockRef.Rotation == 0) {
                                                        blockRef.TransformBy(Matrix3d.Rotation(angle, curUCS.Zaxis, blockRef.Position));
                                                    }

                                                    transModify.Commit();
                                                } else if (objType == "DBText") {
                                                    DBText textObj = entity as DBText;
                                                    textObj.UpgradeOpen();
                                                    textObj.Rotation = angle;
                                                    transModify.Commit();
                                                    // ed.WriteMessage($"Third check\n");
                                                } else if (objType == "MText") {
                                                    MText textMObj = entity as MText;
                                                    textMObj.UpgradeOpen();
                                                    textMObj.Rotation = angle;
                                                    transModify.Commit();
                                                }
                                            }

                                            liczba_porzadkowa++;
                                            ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} Line");
                                        }
                                    }


                                }


                            } else if (lineEntity.GetType().Name == "Polyline3d") {



                                //Polyline
                                pline3d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline3d;


                                int counter_of_vertex = 0;

                                List<double> wspX = new List<double>();
                                List<double> wspY = new List<double>();
                                List<double> wspZ = new List<double>();

                                if (pline3d != null) {

                                    // Use foreach to get each contained vertex

                                    foreach (ObjectId vId in pline3d) {

                                        PolylineVertex3d v3d = (PolylineVertex3d)trans.GetObject(vId, OpenMode.ForRead);
                                        counter_of_vertex++;

                                        wspX.Add(v3d.Position.X);
                                        wspY.Add(v3d.Position.Y);
                                        wspZ.Add(v3d.Position.Z);

                                        //ed.WriteMessage($"\n{counter_of_vertex}" + v3d.Position.X.ToString() + "\n");

                                    }


                                    //continue;

                                    for (int jk = 0; jk < (counter_of_vertex - 1); jk++) {

                                        wspX0 = Math.Round(wspX[jk], 3);
                                        wspY0 = Math.Round(wspY[jk], 3);
                                        //wspZ0 = Math.Round(wspZ[jk], 3);

                                        wspX1 = Math.Round(wspX[jk + 1], 3);
                                        wspY1 = Math.Round(wspY[jk + 1], 3);
                                        //wspZ1 = Math.Round(wspZ[jk + 1], 3);

                                        change = ChangeRotation(wspXP, wspX0, wspX1, prec);
                                        if (!change) {
                                            continue;
                                        }

                                        change = ChangeRotation(wspYP, wspY0, wspY1, prec);
                                        if (!change) {
                                            continue;
                                        }


                                        deltaX01 = wspX1 - wspX0;
                                        deltaY01 = wspY1 - wspY0;

                                        d01 = Math.Sqrt((deltaX01 * deltaX01) + (deltaY01 * deltaY01));
                                        deltaX0 = wspXP - wspX0;
                                        deltaY0 = wspYP - wspY0;

                                        d0P = Math.Sqrt((deltaX0 * deltaX0) + (deltaY0 * deltaY0));

                                        if (deltaX01 == 0 && deltaY01 == 0) {

                                            angle = 0;

                                        } else {
                                            angle = Math.Atan2(deltaY01, deltaX01);
                                        }

                                        if (d01 != 0) {
                                            double deltaXP = d0P * (deltaX01 / d01);
                                            double deltaYP = d0P * (deltaY01 / d01);

                                            double new_point_x = wspX0 + deltaXP;
                                            double new_point_y = wspY0 + deltaYP;

                                            // ed.WriteMessage($"First check, {new_point_x}, {new_point_y} :: {wspXP}, {wspYP}\n");

                                            on_line = IsOnLine(wspXP, wspYP, new_point_x, new_point_y, prec);

                                            if (on_line && (orignal_rotation == 0)) {

                                                angle = ValidAngle(angle);

                                                // ed.WriteMessage($"Second check\n");

                                                using (Transaction transModify = db.TransactionManager.StartTransaction()) {


                                                    if (objType == "BlockReference") {
                                                        BlockReference blockRef = entity as BlockReference;
                                                        blockRef.UpgradeOpen();

                                                        Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
                                                        CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;

                                                        if (blockRef.Rotation == 0) {
                                                            blockRef.TransformBy(Matrix3d.Rotation(angle, curUCS.Zaxis, blockRef.Position));
                                                        }

                                                        transModify.Commit();
                                                    } else if (objType == "DBText") {
                                                        DBText textObj = entity as DBText;
                                                        textObj.UpgradeOpen();
                                                        textObj.Rotation = angle;
                                                        transModify.Commit();
                                                        // ed.WriteMessage($"Third check\n");
                                                    } else if (objType == "MText") {
                                                        MText textMObj = entity as MText;
                                                        textMObj.UpgradeOpen();
                                                        textMObj.Rotation = angle;
                                                        transModify.Commit();
                                                    }
                                                }

                                                liczba_porzadkowa++;
                                                ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} Line");
                                            }
                                        }
                                    }

                                }


                            } else if (lineEntity.GetType().Name == "Polyline2d") {



                                //Polyline
                                pline2d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline2d;


                                int counter_of_vertex = 0;

                                List<double> wspX = new List<double>();
                                List<double> wspY = new List<double>();
                                List<double> wspZ = new List<double>();

                                if (pline2d != null) {

                                    // Use foreach to get each contained vertex

                                    foreach (ObjectId vId in pline2d) {

                                        Vertex2d v2d = (Vertex2d)trans.GetObject(vId, OpenMode.ForRead);
                                        counter_of_vertex++;

                                        wspX.Add(v2d.Position.X);
                                        wspY.Add(v2d.Position.Y);
                                        wspZ.Add(0.0);

                                        //ed.WriteMessage($"\n{counter_of_vertex}" + v3d.Position.X.ToString() + "\n");

                                    }


                                    //continue;

                                    for (int jk = 0; jk < (counter_of_vertex - 1); jk++) {

                                        wspX0 = Math.Round(wspX[jk], 3);
                                        wspY0 = Math.Round(wspY[jk], 3);
                                        //wspZ0 = Math.Round(wspZ[jk], 3);

                                        wspX1 = Math.Round(wspX[jk + 1], 3);
                                        wspY1 = Math.Round(wspY[jk + 1], 3);
                                        //wspZ1 = Math.Round(wspZ[jk + 1], 3);

                                        change = ChangeRotation(wspXP, wspX0, wspX1, prec);
                                        if (!change) {
                                            continue;
                                        }

                                        change = ChangeRotation(wspYP, wspY0, wspY1, prec);
                                        if (!change) {
                                            continue;
                                        }


                                        deltaX01 = wspX1 - wspX0;
                                        deltaY01 = wspY1 - wspY0;

                                        d01 = Math.Sqrt((deltaX01 * deltaX01) + (deltaY01 * deltaY01));
                                        deltaX0 = wspXP - wspX0;
                                        deltaY0 = wspYP - wspY0;

                                        d0P = Math.Sqrt((deltaX0 * deltaX0) + (deltaY0 * deltaY0));

                                        if (deltaX01 == 0 && deltaY01 == 0) {

                                            angle = 0;

                                        } else {
                                            angle = Math.Atan2(deltaY01, deltaX01);
                                        }

                                        if (d01 != 0) {
                                            double deltaXP = d0P * (deltaX01 / d01);
                                            double deltaYP = d0P * (deltaY01 / d01);

                                            double new_point_x = wspX0 + deltaXP;
                                            double new_point_y = wspY0 + deltaYP;

                                            // ed.WriteMessage($"First check, {new_point_x}, {new_point_y} :: {wspXP}, {wspYP}\n");

                                            on_line = IsOnLine(wspXP, wspYP, new_point_x, new_point_y, prec);

                                            if (on_line && (orignal_rotation == 0)) {

                                                angle = ValidAngle(angle);

                                                // ed.WriteMessage($"Second check\n");

                                                using (Transaction transModify = db.TransactionManager.StartTransaction()) {


                                                    if (objType == "BlockReference") {
                                                        BlockReference blockRef = entity as BlockReference;
                                                        blockRef.UpgradeOpen();

                                                        Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
                                                        CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;

                                                        if (blockRef.Rotation == 0) {
                                                            blockRef.TransformBy(Matrix3d.Rotation(angle, curUCS.Zaxis, blockRef.Position));
                                                        }

                                                        transModify.Commit();
                                                    } else if (objType == "DBText") {
                                                        DBText textObj = entity as DBText;
                                                        textObj.UpgradeOpen();
                                                        textObj.Rotation = angle;
                                                        transModify.Commit();
                                                        // ed.WriteMessage($"Third check\n");
                                                    } else if (objType == "MText") {
                                                        MText textMObj = entity as MText;
                                                        textMObj.UpgradeOpen();
                                                        textMObj.Rotation = angle;
                                                        transModify.Commit();
                                                    }
                                                }

                                                liczba_porzadkowa++;
                                                ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} Line");
                                            }
                                        }
                                    }

                                }


                            } else {
                                continue;
                                // lineEntity.GetType().Name != "Polyline"
                            }



                        }
                    }

                    trans.Commit();
                }
            }
        }

        public void RotateTextToLine2() {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            double prec = 0.05;
            bool on_line = false;

            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            SelectionSet selectionSet = selectionResult.Value;
            int liczba_porzadkowa = 0;

            foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity;
                    if (entity == null) {
                        continue;
                    }

                    string objType = entity.GetType().Name;
                    double wspXP = 0.0;
                    double wspYP = 0.0;
                    double orignal_rotation = 0;


                    if (objType == "BlockReference") {
                        BlockReference blockRef = entity as BlockReference;
                        wspXP = blockRef.Position.X;
                        wspYP = blockRef.Position.Y;
                        orignal_rotation = blockRef.Rotation;
                    } else if (objType == "DBText") {
                        DBText textObj = entity as DBText;
                        wspXP = textObj.Position.X;
                        wspYP = textObj.Position.Y;
                        orignal_rotation = textObj.Rotation;
                    } else if (objType == "MText") {
                        MText textMObj = entity as MText;
                        wspXP = textMObj.Location.X;
                        wspXP = textMObj.Location.Y;
                        orignal_rotation = textMObj.Rotation;
                    } else {
                        continue;
                    }
                    ed.WriteMessage(objType + $"\n");


                    foreach (SelectedObject lineSelectedObject in selectionSet) {
                        using (Entity lineEntity = trans.GetObject(lineSelectedObject.ObjectId, OpenMode.ForRead) as Entity) {

                            ed.WriteMessage(lineEntity.GetType().Name + $"\n");

                            Line line = new Line();

                            Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                            Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                            Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();

                            double angle = 0.0;

                            double wspX0 = 0.0;
                            double wspY0 = 0.0;
                            double wspX1 = 0.0;
                            double wspY1 = 0.0;

                            double deltaX01 = 0.0;
                            double deltaY01 = 0.0;


                            if (lineEntity == null) { continue; }

                            if (lineEntity.GetType().Name == "Line") {

                                line = lineEntity as Line;
                                angle = line.Angle;

                                Point3d startPoint = new Point3d(line.StartPoint.X, line.StartPoint.Y, 0.0);
                                Point3d endPoint = new Point3d(line.EndPoint.X, line.EndPoint.Y, 0.0);
                                Line dummy = new Line(startPoint, endPoint);
                                Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                //if (endPoint == objPts) { continue; }

                                if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                Point3d vec = new Point3d(dummy.GetClosestPointTo(objPts, true).X, dummy.GetClosestPointTo(objPts, true).Y, 0.0);
                                on_line = vec.DistanceTo(objPts) < prec;

                                if (on_line && (orignal_rotation == 0)) {
                                    angle = ValidAngle(angle);

                                    InserRotation(db, doc, entity, objType, angle);

                                    liczba_porzadkowa++;
                                    //ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} -> {lineEntity.GetType().Name}");
                                }

                            } else if (lineEntity.GetType().Name == "Polyline") {

                                pline = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline;

                                for (int jk = 0; jk < (pline.NumberOfVertices - 1); jk++) {

                                    wspX0 = Math.Round(pline.GetPoint2dAt(jk).X, 3);
                                    wspY0 = Math.Round(pline.GetPoint2dAt(jk).Y, 3);
                                    wspX1 = Math.Round(pline.GetPoint2dAt(jk + 1).X, 3);
                                    wspY1 = Math.Round(pline.GetPoint2dAt(jk + 1).Y, 3);

                                    deltaX01 = wspX1 - wspX0;
                                    deltaY01 = wspY1 - wspY0;

                                    if (deltaX01 == 0 && deltaY01 == 0) {

                                        angle = 0;

                                    } else {
                                        angle = Math.Atan2(deltaY01, deltaX01);
                                    }

                                    Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                                    Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                                    Line dummy = new Line(startPoint, endPoint);
                                    Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                    //if (endPoint == objPts) { continue; }

                                    if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                    Point3d vec = new Point3d(dummy.GetClosestPointTo(objPts, true).X, dummy.GetClosestPointTo(objPts, true).Y, 0.0);
                                    on_line = vec.DistanceTo(objPts) < prec;

                                    if (on_line && (orignal_rotation == 0)) {

                                        angle = ValidAngle(angle);

                                        InserRotation(db, doc, entity, objType, angle);

                                        liczba_porzadkowa++;
                                        //ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} -> {lineEntity.GetType().Name}");
                                    }
                                }
                            } else if (lineEntity.GetType().Name == "Polyline3d") {



                                //Polyline
                                pline3d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline3d;


                                int counter_of_vertex = 0;

                                List<double> wspX = new List<double>();
                                List<double> wspY = new List<double>();
                                List<double> wspZ = new List<double>();

                                if (pline3d != null) {

                                    foreach (ObjectId vId in pline3d) {

                                        PolylineVertex3d v3d = (PolylineVertex3d)trans.GetObject(vId, OpenMode.ForRead);
                                        counter_of_vertex++;

                                        wspX.Add(v3d.Position.X);
                                        wspY.Add(v3d.Position.Y);
                                        wspZ.Add(v3d.Position.Z);

                                    }

                                    for (int jk = 0; jk < (counter_of_vertex - 1); jk++) {

                                        wspX0 = Math.Round(wspX[jk], 3);
                                        wspY0 = Math.Round(wspY[jk], 3);
                                        //wspZ0 = Math.Round(wspZ[jk], 3);

                                        wspX1 = Math.Round(wspX[jk + 1], 3);
                                        wspY1 = Math.Round(wspY[jk + 1], 3);
                                        //wspZ1 = Math.Round(wspZ[jk + 1], 3);

                                        deltaX01 = wspX1 - wspX0;
                                        deltaY01 = wspY1 - wspY0;

                                        if (deltaX01 == 0 && deltaY01 == 0) {

                                            angle = 0;

                                        } else {
                                            angle = Math.Atan2(deltaY01, deltaX01);
                                        }

                                        Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                                        Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                                        Line dummy = new Line(startPoint, endPoint);
                                        Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                        //if (endPoint == objPts) { continue; }

                                        if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                        Point3d vec = new Point3d(dummy.GetClosestPointTo(objPts, true).X, dummy.GetClosestPointTo(objPts, true).Y, 0.0);
                                        on_line = vec.DistanceTo(objPts) < prec;

                                        if (on_line && (orignal_rotation == 0)) {

                                            angle = ValidAngle(angle);

                                            InserRotation(db, doc, entity, objType, angle);

                                            liczba_porzadkowa++;
                                            //ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} -> {lineEntity.GetType().Name}");
                                        }
                                    }
                                }
                            } else if (lineEntity.GetType().Name == "Polyline2d") {

                                pline2d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                                int counter_of_vertex = 0;

                                List<double> wspX = new List<double>();
                                List<double> wspY = new List<double>();
                                List<double> wspZ = new List<double>();

                                if (pline2d != null) {

                                    foreach (ObjectId vId in pline2d) {

                                        Vertex2d v2d = (Vertex2d)trans.GetObject(vId, OpenMode.ForRead);
                                        counter_of_vertex++;

                                        wspX.Add(v2d.Position.X);
                                        wspY.Add(v2d.Position.Y);
                                        wspZ.Add(0.0);

                                    }

                                    for (int jk = 0; jk < (counter_of_vertex - 1); jk++) {

                                        wspX0 = Math.Round(wspX[jk], 3);
                                        wspY0 = Math.Round(wspY[jk], 3);
                                        //wspZ0 = Math.Round(wspZ[jk], 3);

                                        wspX1 = Math.Round(wspX[jk + 1], 3);
                                        wspY1 = Math.Round(wspY[jk + 1], 3);
                                        //wspZ1 = Math.Round(wspZ[jk + 1], 3);

                                        deltaX01 = wspX1 - wspX0;
                                        deltaY01 = wspY1 - wspY0;


                                        if (deltaX01 == 0 && deltaY01 == 0) {

                                            angle = 0;

                                        } else {
                                            angle = Math.Atan2(deltaY01, deltaX01);
                                        }

                                        Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                                        Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                                        Line dummy = new Line(startPoint, endPoint);
                                        Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                        //if (endPoint == objPts) { continue; }

                                        if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                        Point3d vec = new Point3d(dummy.GetClosestPointTo(objPts, true).X, dummy.GetClosestPointTo(objPts, true).Y, 0.0);
                                        on_line = vec.DistanceTo(objPts) < prec;

                                        if (on_line && (orignal_rotation == 0)) {

                                            angle = ValidAngle(angle);

                                            InserRotation(db, doc, entity, objType, angle);

                                            liczba_porzadkowa++;
                                            //ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} -> {lineEntity.GetType().Name}");
                                        }
                                    }
                                }
                            } else {
                                continue;
                                // lineEntity.GetType().Name != "Polyline"
                            }
                        }
                    }
                    trans.Commit();
                }
            }
        }


        public void RotateTextToLine3() {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            double prec = 0.05;
            bool on_line = false;
            double angleDist = 0.001;

            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            List<ObjectId> line_ids = new List<ObjectId>();
            List<ObjectId> text_ids = new List<ObjectId>();

            line_ids = GetIds(selectionResult, "lineCircle");
            text_ids = GetIds(selectionResult, "text");


            SelectionSet selectionSet = selectionResult.Value;
            int liczba_porzadkowa = 0;

            double wspXP = 0.0;
            double wspYP = 0.0;
            double orignal_rotation = 0;

            foreach (ObjectId line_id in line_ids) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    Entity lineEntity = trans.GetObject(line_id, OpenMode.ForRead) as Entity;
                    
                    if (lineEntity == null) {
                        continue;
                    }

                    double angle = 0.0;

                    double wspX0 = 0.0;
                    double wspY0 = 0.0;
                    double wspX1 = 0.0;
                    double wspY1 = 0.0;

                    double deltaX01 = 0.0;
                    double deltaY01 = 0.0;

                    if (lineEntity == null) { continue; }

                    if (lineEntity.GetType().Name == "Line") {

                        Line line = new Line();

                        line = lineEntity as Line;
                        angle = line.Angle;

                        Point3d startPoint = new Point3d(line.StartPoint.X, line.StartPoint.Y, 0.0);
                        Point3d endPoint = new Point3d(line.EndPoint.X, line.EndPoint.Y, 0.0);

                        Line dummy = new Line(startPoint, endPoint);

                        for (int i = 0; i < text_ids.Count; i++) {

                            Entity entity = trans.GetObject(text_ids[i], OpenMode.ForRead) as Entity;
                            string objType = entity.GetType().Name;

                            List<double> values = GetPositionAndAngle(entity);
                            wspXP = values[0];
                            wspYP = values[1];
                            orignal_rotation = values[2];

                            Point3d objPts = new Point3d(wspXP, wspYP, 0.0);
                            if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }
                            on_line = IsOnLineVec(dummy, objPts, prec);

                            if (on_line && (orignal_rotation == 0)) {
                                angle = ValidAngle(angle);

                                InserRotation(db, doc, entity, objType, angle);
                                text_ids.RemoveAt(i);
                                i--;
                                liczba_porzadkowa++;
                            }
                        }

                    } else if (lineEntity.GetType().Name == "Polyline") {

                        Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                        pline = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline;

                        for (int jk = 0; jk < (pline.NumberOfVertices - 1); jk++) {

                            wspX0 = Math.Round(pline.GetPoint2dAt(jk).X, 3);
                            wspY0 = Math.Round(pline.GetPoint2dAt(jk).Y, 3);
                            wspX1 = Math.Round(pline.GetPoint2dAt(jk + 1).X, 3);
                            wspY1 = Math.Round(pline.GetPoint2dAt(jk + 1).Y, 3);

                            Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                            Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                            Line dummy = new Line(startPoint, endPoint);

                            angle = dummy.Angle;

                            for (int i = 0; i < text_ids.Count; i++) {

                                Entity entity = trans.GetObject(text_ids[i], OpenMode.ForRead) as Entity;
                                string objType = entity.GetType().Name;

                                List<double> values = GetPositionAndAngle(entity);
                                wspXP = values[0];
                                wspYP = values[1];
                                orignal_rotation = values[2];

                                Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                on_line = IsOnLineVec(dummy, objPts, prec);

                                if (on_line && (orignal_rotation == 0)) {

                                    angle = ValidAngle(angle);

                                    InserRotation(db, doc, entity, objType, angle);

                                    text_ids.RemoveAt(i);
                                    i--;

                                    liczba_porzadkowa++;
                                }

                            }
                        }
                    } else if (lineEntity.GetType().Name == "Polyline3d") {


                        Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                        pline3d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline3d;


                        int counter_of_vertex = 0;

                        List<double> wspX = new List<double>();
                        List<double> wspY = new List<double>();
                        List<double> wspZ = new List<double>();

                        if (pline3d != null) {

                            foreach (ObjectId vId in pline3d) {

                                PolylineVertex3d v3d = (PolylineVertex3d)trans.GetObject(vId, OpenMode.ForRead);
                                counter_of_vertex++;

                                wspX.Add(v3d.Position.X);
                                wspY.Add(v3d.Position.Y);
                                wspZ.Add(v3d.Position.Z);

                            }

                            for (int jk = 0; jk < (counter_of_vertex - 1); jk++) {

                                wspX0 = Math.Round(wspX[jk], 3);
                                wspY0 = Math.Round(wspY[jk], 3);
                                //wspZ0 = Math.Round(wspZ[jk], 3);

                                wspX1 = Math.Round(wspX[jk + 1], 3);
                                wspY1 = Math.Round(wspY[jk + 1], 3);
                                //wspZ1 = Math.Round(wspZ[jk + 1], 3);

                                Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                                Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                                Line dummy = new Line(startPoint, endPoint);
                                angle = dummy.Angle;

                                for (int i = 0; i < text_ids.Count; i++) {

                                    Entity entity = trans.GetObject(text_ids[i], OpenMode.ForRead) as Entity;
                                    string objType = entity.GetType().Name;

                                    List<double> values = GetPositionAndAngle(entity);
                                    wspXP = values[0];
                                    wspYP = values[1];
                                    orignal_rotation = values[2];

                                    Point3d objPts = new Point3d(wspXP, wspYP, 0.0);


                                    if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                    on_line = IsOnLineVec(dummy, objPts, prec);

                                    if (on_line && (orignal_rotation == 0)) {

                                        angle = ValidAngle(angle);

                                        InserRotation(db, doc, entity, objType, angle);

                                        text_ids.RemoveAt(i);
                                        i--;

                                        liczba_porzadkowa++;
                                    }
                                }
                            }
                        }
                    } else if (lineEntity.GetType().Name == "Polyline2d") {

                        Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();
                        pline2d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                        int counter_of_vertex = 0;

                        List<double> wspX = new List<double>();
                        List<double> wspY = new List<double>();
                        List<double> wspZ = new List<double>();

                        if (pline2d != null) {

                            foreach (ObjectId vId in pline2d) {

                                Vertex2d v2d = (Vertex2d)trans.GetObject(vId, OpenMode.ForRead);
                                counter_of_vertex++;

                                wspX.Add(v2d.Position.X);
                                wspY.Add(v2d.Position.Y);
                                wspZ.Add(0.0);

                            }

                            for (int jk = 0; jk < (counter_of_vertex - 1); jk++) {

                                wspX0 = Math.Round(wspX[jk], 3);
                                wspY0 = Math.Round(wspY[jk], 3);
                                //wspZ0 = Math.Round(wspZ[jk], 3);

                                wspX1 = Math.Round(wspX[jk + 1], 3);
                                wspY1 = Math.Round(wspY[jk + 1], 3);
                                //wspZ1 = Math.Round(wspZ[jk + 1], 3);


                                Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                                Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                                Line dummy = new Line(startPoint, endPoint);
                                angle = dummy.Angle;

                                int idx = 0;
                                for (int i = 0; i < text_ids.Count; i++) {

                                    Entity entity = trans.GetObject(text_ids[i], OpenMode.ForRead) as Entity;
                                    string objType = entity.GetType().Name;

                                    List<double> values = GetPositionAndAngle(entity);
                                    wspXP = values[0];
                                    wspYP = values[1];
                                    orignal_rotation = values[2];

                                    Point3d objPts = new Point3d(wspXP, wspYP, 0.0);


                                    if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                    on_line = IsOnLineVec(dummy, objPts, prec);

                                    if (on_line && (orignal_rotation == 0)) {

                                        angle = ValidAngle(angle);

                                        InserRotation(db, doc, entity, objType, angle);

                                        text_ids.RemoveAt(i);
                                        i--;

                                        liczba_porzadkowa++;
                                    }
                                }
                            }
                        }
                    } else if (lineEntity.GetType().Name == "Circle") {

                        Circle acCirc = lineEntity as Circle;

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


                            for (int jk = 0; jk < (simplifyCircle.NumberOfVertices - 1); jk++) {

                                wspX0 = Math.Round(simplifyCircle.GetPoint2dAt(jk).X, 3);
                                wspY0 = Math.Round(simplifyCircle.GetPoint2dAt(jk).Y, 3);
                                wspX1 = Math.Round(simplifyCircle.GetPoint2dAt(jk + 1).X, 3);
                                wspY1 = Math.Round(simplifyCircle.GetPoint2dAt(jk + 1).Y, 3);

                                Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                                Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                                Line dummy = new Line(startPoint, endPoint);
                                angle = dummy.Angle;


                                for (int i = 0; i < text_ids.Count; i++) {

                                    Entity entity = trans.GetObject(text_ids[i], OpenMode.ForRead) as Entity;
                                    string objType = entity.GetType().Name;

                                    List<double> values = GetPositionAndAngle(entity);
                                    wspXP = values[0];
                                    wspYP = values[1];
                                    orignal_rotation = values[2];

                                    Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                    if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                    on_line = IsOnLineVec(dummy, objPts, prec);

                                    if (on_line && (orignal_rotation == 0)) {

                                        angle = ValidAngle(angle);

                                        InserRotation(db, doc, entity, objType, angle);

                                        text_ids.RemoveAt(i);
                                        i--;

                                        liczba_porzadkowa++;
                                    }
                                }
                            }
                        }

                    } else if (lineEntity.GetType().Name == "Arc") {

                        ed.WriteMessage("\nOh thats arc!\n");

                        Arc acArc = lineEntity as Arc;

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


                            for (int jk = 0; jk < (simplifyCircle.NumberOfVertices - 1); jk++) {

                                wspX0 = Math.Round(simplifyCircle.GetPoint2dAt(jk).X, 3);
                                wspY0 = Math.Round(simplifyCircle.GetPoint2dAt(jk).Y, 3);
                                wspX1 = Math.Round(simplifyCircle.GetPoint2dAt(jk + 1).X, 3);
                                wspY1 = Math.Round(simplifyCircle.GetPoint2dAt(jk + 1).Y, 3);


                                Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                                Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                                Line dummy2 = new Line(startPoint, endPoint);
                                angle = dummy2.Angle;


                                for (int i = 0; i < text_ids.Count; i++) {

                                    Entity entity = trans.GetObject(text_ids[i], OpenMode.ForRead) as Entity;
                                    string objType = entity.GetType().Name;

                                    List<double> values = GetPositionAndAngle(entity);
                                    wspXP = values[0];
                                    wspYP = values[1];
                                    orignal_rotation = values[2];

                                    Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                    if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                    on_line = IsOnLineVec(dummy2, objPts, prec);

                                    if (on_line && (orignal_rotation == 0)) {

                                        angle = ValidAngle(angle);

                                        InserRotation(db, doc, entity, objType, angle);

                                        text_ids.RemoveAt(i);
                                        i--;

                                        liczba_porzadkowa++;
                                    }
                                }
                            }
                        }

                    } else {
                        continue;
                    }


                    trans.Commit();
                }
            }
        }

        public List<double> GetPositionAndAngle(Entity entity) {

            List<double> result = new List<double>();

            double wspXP = 0.0;
            double wspYP = 0.0;
            double orignal_rotation = 0.0;

            string objType = entity.GetType().Name;

            if (objType == "BlockReference") {
                BlockReference blockRef = entity as BlockReference;
                wspXP = blockRef.Position.X;
                wspYP = blockRef.Position.Y;
                orignal_rotation = blockRef.Rotation;


            } else if (objType == "DBText") {
                DBText textObj = entity as DBText;

                wspXP = textObj.AlignmentPoint.X;
                wspYP = textObj.AlignmentPoint.Y;

                if ((Math.Round(wspXP, 2) == 0.00) && (Math.Round(wspYP, 2) == 0.00)) {

                    wspXP = textObj.Position.X;
                    wspYP = textObj.Position.Y;

                }
                orignal_rotation = textObj.Rotation;
            } else if (objType == "MText") {
                MText textMObj = entity as MText;
                wspXP = textMObj.Location.X;
                wspYP = textMObj.Location.Y;
                orignal_rotation = textMObj.Rotation;
            }

            result.Insert(0, wspXP);
            result.Insert(1, wspYP);
            result.Insert(2, orignal_rotation);

            return result;
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

        public List<string> GetLayerNameBySelected(PromptSelectionResult selectionResult, string type = "all") {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            List<string> list_ids = new List<string>();

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

                                if (!list_ids.Contains(entity.Layer)) {
                                    list_ids.Add(entity.Layer);
                                }

                                lid++;
                            }
                        } else if (type == "text") {

                            if (entity.GetType().Name == "BlockReference" ||
                                entity.GetType().Name == "DBText" ||
                                entity.GetType().Name == "MText") {

                                if (!list_ids.Contains(entity.Layer)) {
                                    list_ids.Add(entity.Layer);
                                }
                                lid++;
                            }
                        } else if (type == "block") {

                            if (entity.GetType().Name == "BlockReference") {

                                if (!list_ids.Contains(entity.Layer)) {
                                    list_ids.Add(entity.Layer);
                                }
                                lid++;
                            }
                        } else if (type == "line3d") {

                            if (entity.GetType().Name == "Line" ||
                                entity.GetType().Name == "Polyline3d") {

                                if (!list_ids.Contains(entity.Layer)) {
                                    list_ids.Add(entity.Layer);
                                }
                                lid++;
                            }
                        }

                        else {
                            if (!list_ids.Contains(entity.Layer)) {
                                list_ids.Add(entity.Layer);
                            }
                            lid++;
                        }
                    }
                }
            }
            ed.WriteMessage($"\nElementów typu {type}: {lid}");

            return list_ids;
        }

        private void InserRotation(Database db, Document doc, Entity entity, string objType, Double angle) {

            using (Transaction transModify = db.TransactionManager.StartTransaction()) {


                if (objType == "BlockReference") {
                    BlockReference blockRef = entity as BlockReference;
                    blockRef.UpgradeOpen();

                    Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
                    CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;

                    if (blockRef.Rotation == 0) {
                        blockRef.TransformBy(Matrix3d.Rotation(angle, curUCS.Zaxis, blockRef.Position));
                    }

                    transModify.Commit();
                } else if (objType == "DBText") {
                    DBText textObj = entity as DBText;
                    textObj.UpgradeOpen();
                    textObj.Rotation = angle;
                    transModify.Commit();
                    // ed.WriteMessage($"Third check\n");
                } else if (objType == "MText") {
                    MText textMObj = entity as MText;
                    textMObj.UpgradeOpen();
                    textMObj.Rotation = angle;
                    transModify.Commit();
                }
            }

        }

        public bool InBox(Point3d sP3D, Point3d eP3D, Point3d pP3D, double prec) {

            if (sP3D.X > eP3D.X) {
                if (pP3D.X > (sP3D.X + prec)) {
                    return false;
                } else if (pP3D.X < (eP3D.X - prec)) {
                    return false;
                }
            } else {
                if (pP3D.X > (eP3D.X + prec)) {
                    return false;
                } else if (pP3D.X < (sP3D.X - prec)) {
                    return false;
                }
            }

            if (sP3D.Y > eP3D.Y) {
                if (pP3D.Y > (sP3D.Y + prec)) {
                    return false;
                } else if (pP3D.Y < (eP3D.Y - prec)) {
                    return false;
                }
            } else {
                if (pP3D.Y > (eP3D.Y + prec)) {
                    return false;
                } else if (pP3D.Y < (sP3D.Y - prec)) {
                    return false;
                }
            }
            return true;
        }

        private bool ChangeRotation(double wspP, double wsp0, double wsp1, double diff) {
            bool change = true;


            if ((Math.Round(wspP, 2) < (Math.Round(wsp0, 2) - Math.Round(diff, 2))) && (Math.Round(wspP, 2) < (Math.Round(wsp1, 2) - Math.Round(diff, 2)))) {
                change = false;
            }
            if ((Math.Round(wspP, 2) > (Math.Round(wsp0, 2) + Math.Round(diff, 2))) && (Math.Round(wspP, 2) > (Math.Round(wsp1, 2) + Math.Round(diff, 2)))) {
                change = false;
            }

            return change;
        }

        private bool IsOnLine(double wspX0, double wspY0, double wspX1, double wspY1, double prec) {
            bool on_line = false;

            if (
            (Math.Round(wspX0, 2) <= (Math.Round(wspX1, 2) + prec)) &&
            (Math.Round(wspX0, 2) >= (Math.Round(wspX1, 2) - prec)) &&
            (Math.Round(wspY0, 2) <= (Math.Round(wspY1, 2) + prec)) &&
            (Math.Round(wspY0, 2) >= (Math.Round(wspY1, 2) - prec))
            ) {

                on_line = true;

            }




            return on_line;
        }

        private bool IsOnLineVec(Line dummy, Point3d objPts,double prec) {

            bool on_line;

            Point3d vec = new Point3d(dummy.GetClosestPointTo(objPts, true).X, dummy.GetClosestPointTo(objPts, true).Y, 0.0);
            on_line = vec.DistanceTo(objPts) < prec;


            return on_line;
        }
        private double ValidAngle(double angle) {
            if (angle < 0) {
                angle += 2 * Math.PI;
            }

            if (angle > (Math.PI / 2)) {
                if (angle <= (1.5 * Math.PI)) {
                    angle -= Math.PI;
                }
            }

            return angle;
        }


    }
}
