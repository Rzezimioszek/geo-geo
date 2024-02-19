using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace Geo_geo.Class {
    internal class cRzutowanie {

        public void PlaceTextOnLine() {
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
                    double wspZP = 0.0;

                    if (objType == "BlockReference") {
                        BlockReference blockRef = entity as BlockReference;
                        wspXP = blockRef.Position.X;
                        wspYP = blockRef.Position.Y;
                        wspZP = blockRef.Position.Z;
                    } else if (objType == "DBText") {
                        DBText textObj = entity as DBText;
                        wspXP = textObj.Position.X;
                        wspYP = textObj.Position.Y;
                        wspZP = textObj.Position.Z;
                    } else if (objType == "MText") {
                        MText textMObj = entity as MText;
                        wspXP = textMObj.Location.X;
                        wspYP = textMObj.Location.Y;
                        wspZP = textMObj.Location.Z;
                    } else {
                        continue;
                    }
                    //ed.WriteMessage(objType + $"\n");


                    foreach (SelectedObject lineSelectedObject in selectionSet) {
                        using (Entity lineEntity = trans.GetObject(lineSelectedObject.ObjectId, OpenMode.ForRead) as Entity) {

                            //ed.WriteMessage(lineEntity.GetType().Name + $"\n");

                            Line line = new Line();

                            Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                            Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();

                            double wspX0 = 0.0;
                            double wspY0 = 0.0;
                            double wspZ0 = 0.0;
                            double wspX1 = 0.0;
                            double wspY1 = 0.0;
                            double wspZ1 = 0.0;

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

                                Point3d startPoint = line.StartPoint;
                                Point3d endPoint = line.EndPoint;

                                wspX0 = startPoint.X;
                                wspY0 = startPoint.Y;
                                wspZ0 = startPoint.Z;

                                wspX1 = endPoint.X;
                                wspY1 = endPoint.Y;
                                wspZ1 = endPoint.Z; //zmiana z start na end

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

                                    double HX;
                                    double newPointZ;

                                    on_line = IsOnLine(wspXP, wspYP, new_point_x, new_point_y, prec);

                                    if (on_line) {

                                        HX = ((Math.Abs((wspZ1 - wspZ0)) * d0P) / d01);

                                        

                                        if (wspZ1 > wspZ0) {

                                            newPointZ = wspZ0 + HX;

                                        }
                                        else {

                                            newPointZ = wspZ0 - HX;

                                        }

                                        // ed.WriteMessage($"1: {wspZ1}; 0:{wspZ0}; HX: {HX};d0p: {d0P}; d01: {d01}\n");

                                        using (Transaction transModify = db.TransactionManager.StartTransaction()) {


                                            if (objType == "BlockReference") {
                                                BlockReference blockRef = entity as BlockReference;
                                                blockRef.UpgradeOpen();
                                                blockRef.ScaleFactors = new Scale3d(1, 1, 0.00001);
                                                blockRef.Position = new Point3d(wspXP, wspYP, newPointZ);
                                                transModify.Commit();
                                            } else if (objType == "DBText") {
                                                DBText textObj = entity as DBText;
                                                textObj.UpgradeOpen();
                                                textObj.Position = new Point3d(wspXP, wspYP, newPointZ);
                                                transModify.Commit();
                                            } else if (objType == "MText") {
                                                MText textMObj = entity as MText;
                                                textMObj.UpgradeOpen();
                                                textMObj.Location = new Point3d(wspXP, wspYP, newPointZ);
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


                                    if (d01 != 0) {
                                        double deltaXP = d0P * (deltaX01 / d01);
                                        double deltaYP = d0P * (deltaY01 / d01);

                                        double new_point_x = wspX0 + deltaXP;
                                        double new_point_y = wspY0 + deltaYP;

                                        double HX;
                                        double newPointZ;


                                        on_line = IsOnLine(wspXP, wspYP, new_point_x, new_point_y, prec);

                                        if (on_line) {


                                            if (wspZ1 > wspZ0) {

                                                HX = (Math.Abs(wspZ1 - wspZ0) * d0P) / d01;
                                                newPointZ = wspZ0 + HX;

                                            } else {

                                                HX = (Math.Abs(wspZ1 - wspZ0) * (d01 - d0P)) / d01;
                                                newPointZ = wspZ1 + HX;

                                            }

                                            newPointZ = pline.Elevation;

                                            // ed.WriteMessage($"Second check\n");

                                            using (Transaction transModify = db.TransactionManager.StartTransaction()) {


                                                if (objType == "BlockReference") {
                                                    BlockReference blockRef = entity as BlockReference;
                                                    blockRef.UpgradeOpen();
                                                    blockRef.ScaleFactors = new Scale3d(1, 1, 0.00001);
                                                    blockRef.Position = new Point3d(wspXP, wspYP, newPointZ);
                                                    transModify.Commit();
                                                } else if (objType == "DBText") {
                                                    DBText textObj = entity as DBText;
                                                    textObj.UpgradeOpen();
                                                    textObj.Position = new Point3d(wspXP, wspYP, newPointZ);
                                                    transModify.Commit();
                                                } else if (objType == "MText") {
                                                    MText textMObj = entity as MText;
                                                    textMObj.UpgradeOpen();
                                                    textMObj.Location = new Point3d(wspXP, wspYP, newPointZ);
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
                                        wspZ0 = Math.Round(wspZ[jk], 3);

                                        wspX1 = Math.Round(wspX[jk + 1], 3);
                                        wspY1 = Math.Round(wspY[jk + 1], 3);
                                        wspZ1 = Math.Round(wspZ[jk + 1], 3);


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

                                            double HX;
                                            double newPointZ;

                                            on_line = IsOnLine(wspXP, wspYP, new_point_x, new_point_y, prec);

                                            ed.WriteMessage($"{on_line}\n");

                                            if (on_line) {

                                                if (wspZ1 > wspZ0) {

                                                    HX = (Math.Abs(wspZ1 - wspZ0) * d0P) / d01;
                                                    newPointZ = wspZ0 + HX;

                                                } else {

                                                    HX = (Math.Abs(wspZ1 - wspZ0) * (d01 - d0P)) / d01;
                                                    newPointZ = wspZ1 + HX;

                                                }

                                                ed.WriteMessage($"{HX}\n");

                                                using (Transaction transModify = db.TransactionManager.StartTransaction()) {


                                                    if (objType == "BlockReference") {
                                                        BlockReference blockRef = entity as BlockReference;
                                                        blockRef.UpgradeOpen();
                                                        blockRef.ScaleFactors = new Scale3d(1, 1, 0.00001);
                                                        blockRef.Position = new Point3d(wspXP, wspYP, newPointZ);
                                                        transModify.Commit();
                                                    } else if (objType == "DBText") {
                                                        DBText textObj = entity as DBText;
                                                        textObj.UpgradeOpen();
                                                        textObj.Position = new Point3d(wspXP, wspYP, newPointZ);
                                                        transModify.Commit();
                                                    } else if (objType == "MText") {
                                                        MText textMObj = entity as MText;
                                                        textMObj.UpgradeOpen();
                                                        textMObj.Location = new Point3d(wspXP, wspYP, newPointZ);
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
                                //ed.WriteMessage($"else state{lineEntity.GetType().Name}\n");
                                continue;

                                // lineEntity.GetType().Name != "Polyline"
                            }



                        }
                    }

                    trans.Commit();
                }
            }
        }

        public void PlaceLineOnText() {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            double prec = 0.025;
            bool on_line = false;

            string fileName;
            string fileContent;
            string[] lines;
            string[] points;

            char sep = ' ';
            long ent_moved = 0;

            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            SelectionSet selectionSet = selectionResult.Value;
            int liczba_porzadkowa = 0;


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

            // Read the content of the text file
            using (StreamReader sr = new StreamReader(fileName)) {
                fileContent = sr.ReadToEnd();
            }
            string separator = "Tabulacja";

            // Split the file content into lines
            lines = fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

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

            for (int i = 0; i < lines.Length; i++) {

                ed.WriteMessage($"\nLoad line: {i}");

                while (lines[i].Contains($"{sep}{sep}")) {
                    lines[i] = lines[i].Replace($"{sep}{sep}", $"{sep}");
                }

                lines[i] = lines[i].Replace(",", ".");

                //points = lines[i].Split(sep);
            }

                foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {

                    Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity;

                    if (entity == null) {
                        continue;
                    }

                    string objType = entity.GetType().Name;
                    double wspXP = 0.0;
                    double wspYP = 0.0;
                    double wspZP = 0.0;

                    double wspX0 = 0.0;
                    double wspY0 = 0.0;
                    double wspZ0 = 0.0;

                    double wspX1 = 0.0;
                    double wspY1 = 0.0;
                    double wspZ1 = 0.0;


                    Line line = new Line();
                    Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                    Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                    Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();

                    int nr = 0;
                    int x = 1;
                    int y = 2;
                    int h = 3;
                    int h2 = 4;

                    if (objType == "Line") {
                        line = entity as Line;

                        Point3d startPoint = line.StartPoint;
                        Point3d endPoint = line.EndPoint;

                        wspX0 = startPoint.X;
                        wspY0 = startPoint.Y;
                        wspZ0 = startPoint.Z;

                        wspX1 = endPoint.X;
                        wspY1 = endPoint.Y;
                        wspZ1 = endPoint.Z;

                        for (int i = 0; i < lines.Length; i++) {

                            points = lines[i].Split(sep);

                            wspXP = double.Parse(points[x]);
                            wspYP = double.Parse(points[y]);
                            wspZP = double.Parse(points[h]);

                            Point3d newPoint = new Point3d(wspX0, wspY0, wspZP);

                            if (IsOnLine(wspX0, wspY0, wspXP, wspYP, prec)) {


                                using (Transaction transModify = db.TransactionManager.StartTransaction()) {

                                        line.UpgradeOpen();
                                        line.StartPoint = newPoint;
                                        transModify.Commit();
                                }
                                ent_moved++;
                                ed.WriteMessage("\nLine object moved");

                            }

                            newPoint = new Point3d(wspX1, wspY1, wspZP);

                            if (IsOnLine(wspX1, wspY1, wspXP, wspYP, prec)) {

                                using (Transaction transModify = db.TransactionManager.StartTransaction()) {

                                    line.UpgradeOpen();  
                                    line.EndPoint = newPoint;
                                    transModify.Commit();
                                }
                                ent_moved++;
                                ed.WriteMessage("\nLine object moved");

                            }
                        }

                    } else if (objType == "Polyline3d") {

                        pline3d = entity as Autodesk.AutoCAD.DatabaseServices.Polyline3d;

                        int counter_of_vertex = 0;

                        List<double> wspX = new List<double>();
                        List<double> wspY = new List<double>();
                        List<double> wspZ = new List<double>();

                        if (pline3d != null) {

                            ed.WriteMessage("\nChecking Polyline3D obj");


                            foreach (ObjectId vId in pline3d) {

                                PolylineVertex3d v3d = (PolylineVertex3d)trans.GetObject(vId, OpenMode.ForRead);

                                wspX0 = v3d.Position.X;
                                wspY0 = v3d.Position.Y;
                                wspZ0 = v3d.Position.Z;

                                for (int i = 0; i < lines.Length; i++) {

                                    points = lines[i].Split(sep);

                                    wspXP = double.Parse(points[x]);
                                    wspYP = double.Parse(points[y]);
                                    wspZP = double.Parse(points[h]);

                                    Point3d newPoint = new Point3d(wspX0, wspY0, wspZP);

                                    if (IsOnLine(wspX0,wspY0,wspXP, wspYP, prec)) {
                                        
                                        using (Transaction transModify = db.TransactionManager.StartTransaction()) {

                                            //pline3d.UpgradeOpen();
                                            v3d.UpgradeOpen();

                                            v3d.Position = newPoint;
                                            //line.TransformBy(Matrix3d.Displacement(startPoint.GetVectorTo(newPoint)));
                                            transModify.Commit();

                                        }
                                        ent_moved++;
                                        ed.WriteMessage("\nPolyline3D object moved");
                                    }
                                }
                            } 
                        }

                    } else {
                        ed.WriteMessage(objType + $"\n");
                        continue;
                    }

                    ed.WriteMessage($"\nMoved {ent_moved} entitys");
                    trans.Commit();

                }
            }
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


        private double ValidAngle(double angle) {
            if (angle < 0) {
                angle += 2 * Math.PI;
            }

            if (angle > 1.57) {
                if (angle < 4.71) {
                    angle -= Math.PI;
                }
            }

            return angle;
        }
    }
}
