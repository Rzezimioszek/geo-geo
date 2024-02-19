using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static System.Net.Mime.MediaTypeNames;

namespace Geo_geo.Class {


    internal class cCzolowki {

        ///<summary>
        /// Wstawia tekst z dlugością linii w punkcie środkowym.
        ///</summary>
        ///<param name="prec">Liczba miejsc po przecinku.</param>
        ///<param name="sufix">Porostek po wartości czołówki.</param>
        ///<param name="prefix">Przedrostek przed wartością czołówki</param>
        ///<param name="height">Określa wysokosć tekstu</param>
        ///<param name="duplicate">Czy unikać duplikownia wartości</param>


        public void PlaceDistOnLine(string prec ="F2", string sufix = "-", string prefix = "-", double height = 1.0d, bool duplicate = false, double witdhfactor = 1.0d) {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            //double prec = 0.05;
            bool on_line = false;

            using (DocumentLock acLckDoc = doc.LockDocument()) {

                PromptSelectionResult selectionResult = ed.GetSelection();
                if (selectionResult.Status != PromptStatus.OK) {
                    ed.WriteMessage("Nie wybrano elementów.");
                    return;
                }

                SelectionSet selectionSet = selectionResult.Value;
                int liczba_porzadkowa = 0;

                List<string> in_dwg = new List<string>();

                foreach (SelectedObject lineSelectedObject in selectionSet) {
                    using (Transaction trans = db.TransactionManager.StartTransaction()) {
                        using (Entity lineEntity = trans.GetObject(lineSelectedObject.ObjectId, OpenMode.ForRead) as Entity) {


                            Line line = new Line();
                            Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                            Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                            Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();

                            double wspX0 = 0.0;
                            double wspY0 = 0.0;
                            double wspZ0 = 0.0;
                            double wspX1 = 0.0;
                            double wspY1 = 0.0;
                            double wspZ1 = 0.0;

                            double midX = 0.0;
                            double midY = 0.0;
                            double midZ = 0.0;

                            double deltaX01 = 0.0;
                            double deltaY01 = 0.0;

                            double d01 = 0.0;
                            double deltaX0 = 0.0;
                            double deltaY0 = 0.0;

                            double angle = 0.0;

                            double d0P = 0.0;

                            string line_text = "";
                            string compare_value = "";

                           

                            in_dwg.Add("--");


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


                                d01 = line.Length;
                                angle = ValidAngle(line.Angle);

                                midX = wspX1 + (wspX0 - wspX1) / 2;
                                midY = wspY1 + (wspY0 - wspY1) / 2;
                                midZ = wspZ1 + (wspZ0 - wspZ1) / 2;

                                DBText text = new DBText();
                                line_text = $"{prefix}{Math.Round(d01, 2).ToString(prec)}{sufix}";
                                text.TextString = line_text;
                                text.Position = new Point3d(midX, midY, midZ);
                                try {
                                    text.Justify = AttachmentPoint.BottomCenter;
                                    text.AlignmentPoint = new Point3d(midX, midY, midZ);
                                } catch { }
                                text.Height = height;
                                text.WidthFactor = witdhfactor;
                                text.Rotation = angle;
                                text.AdjustAlignment(db);

                                compare_value = $"{midX}{midY}{midZ}{line_text}";

                                if ((in_dwg.Contains(compare_value)) && duplicate) {
                                } else {

                                    using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                        btr.AppendEntity(text);
                                        tr.AddNewlyCreatedDBObject(text, true);
                                        tr.Commit();
                                    }

                                    if (duplicate) { in_dwg.Add(compare_value); }

                                }


                                liczba_porzadkowa++;

                            } else if (lineEntity.GetType().Name == "Polyline") {
                                //Polyline
                                pline = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline;

                                if (pline == null) { continue; }


                                for (int jk = 0; jk < (pline.NumberOfVertices - 1); jk++) {

                                    wspX0 = Math.Round(pline.GetPoint2dAt(jk).X, 3);
                                    wspY0 = Math.Round(pline.GetPoint2dAt(jk).Y, 3);
                                    wspZ0 = Math.Round(pline.Elevation);

                                    wspX1 = Math.Round(pline.GetPoint2dAt(jk + 1).X, 3);
                                    wspY1 = Math.Round(pline.GetPoint2dAt(jk + 1).Y, 3);
                                    wspZ1 = Math.Round(pline.Elevation);

                                    deltaX01 = wspX1 - wspX0;
                                    deltaY01 = wspY1 - wspY0;

                                    d01 = Math.Sqrt(Math.Pow(deltaX01, 2) + Math.Pow(deltaY01, 2));

                                    angle = ValidAngle(Math.Atan2(deltaY01, deltaX01));

                                    midX = wspX1 + (wspX0 - wspX1) / 2;
                                    midY = wspY1 + (wspY0 - wspY1) / 2;
                                    midZ = wspZ1 + (wspZ0 - wspZ1) / 2;

                                    DBText text = new DBText();

                                    line_text = $"{prefix}{Math.Round(d01, 2).ToString(prec)}{sufix}";
                                    text.TextString = line_text;

                                    text.Position = new Point3d(midX, midY, midZ);
                                    try {
                                        text.Justify = AttachmentPoint.BottomCenter;
                                        text.AlignmentPoint = new Point3d(midX, midY, midZ);
                                    } catch { }
                                    text.Height = height;
                                    text.WidthFactor = witdhfactor;
                                    text.Rotation = angle;
                                    text.AdjustAlignment(db);
                                    

                                    compare_value = $"{midX}{midY}{midZ}{line_text}";

                                    if ((in_dwg.Contains(compare_value)) && duplicate) {
                                    } else {
                                        
                                        using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                            btr.AppendEntity(text);
                                            tr.AddNewlyCreatedDBObject(text, true);
                                            tr.Commit();
                                        }

                                        if (duplicate) { in_dwg.Add(compare_value); }
                                        
                                    }

                                    liczba_porzadkowa++;
                                }
                            } else if (lineEntity.GetType().Name == "Polyline3d") {



                                //Polyline 3d
                                pline3d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline3d;

                                int counter_of_vertex = 0;

                                List<double> wspX = new List<double>();
                                List<double> wspY = new List<double>();
                                List<double> wspZ = new List<double>();

                                if (pline3d == null) { continue; }

                                foreach (ObjectId vId in pline3d) {

                                    PolylineVertex3d v3d = (PolylineVertex3d)trans.GetObject(vId, OpenMode.ForRead);
                                    counter_of_vertex++;

                                    wspX.Add(v3d.Position.X);
                                    wspY.Add(v3d.Position.Y);
                                    wspZ.Add(v3d.Position.Z);

                                }


                                //continue;

                                for (int jk = 0; jk < (counter_of_vertex - 1); jk++) {

                                    wspX0 = Math.Round(wspX[jk], 3);
                                    wspY0 = Math.Round(wspY[jk], 3);
                                    wspZ0 = Math.Round(wspZ[jk], 3);

                                    wspX1 = Math.Round(wspX[jk + 1], 3);
                                    wspY1 = Math.Round(wspY[jk + 1], 3);
                                    wspZ1 = Math.Round(wspZ[jk + 1], 3);

                                    deltaX01 = wspX1 - wspX0;
                                    deltaY01 = wspY1 - wspY0;

                                    d01 = Math.Sqrt(Math.Pow(deltaX01, 2) + Math.Pow(deltaY01, 2));

                                    angle = ValidAngle(Math.Atan2(deltaY01, deltaX01));

                                    midX = wspX1 + (wspX0 - wspX1) / 2;
                                    midY = wspY1 + (wspY0 - wspY1) / 2;
                                    midZ = wspZ1 + (wspZ0 - wspZ1) / 2;

                                    DBText text = new DBText();
                                    line_text = $"{prefix}{Math.Round(d01, 2).ToString(prec)}{sufix}";
                                    text.TextString = line_text;
                                    text.Position = new Point3d(midX, midY, midZ);
                                    try {
                                        text.Justify = AttachmentPoint.BottomCenter;
                                        text.AlignmentPoint = new Point3d(midX, midY, midZ);
                                    } catch { }
                                    text.Height = height;
                                    text.WidthFactor = witdhfactor;
                                    text.Rotation = angle;
                                    text.AdjustAlignment(db);

                                    compare_value = $"{midX}{midY}{midZ}{line_text}";

                                    if ((in_dwg.Contains(compare_value)) && duplicate) {
                                    } else {

                                        using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                            btr.AppendEntity(text);
                                            tr.AddNewlyCreatedDBObject(text, true);
                                            tr.Commit();
                                        }

                                        if (duplicate) { in_dwg.Add(compare_value); }

                                    }

                                }

                            } else if (lineEntity.GetType().Name == "Polyline2d") {


                                //Polyline 2d
                                pline2d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                                ed.WriteMessage("Polyline2d\n");
                                //continue;
                                int counter_of_vertex = 0;

                                List<double> wspX = new List<double>();
                                List<double> wspY = new List<double>();
                                List<double> wspZ = new List<double>();

                                if (pline2d == null) { continue; }


                                foreach (ObjectId vId in pline3d) {

                                    Vertex2d v2d = (Vertex2d)trans.GetObject(vId, OpenMode.ForRead);
                                    counter_of_vertex++;

                                    wspX.Add(v2d.Position.X);
                                    wspY.Add(v2d.Position.Y);
                                    wspZ.Add(0.0);

                                }

                                ed.WriteMessage("now has list\n");


                                //continue;

                                for (int jk = 0; jk < (counter_of_vertex - 1); jk++) {

                                    wspX0 = Math.Round(wspX[jk], 3);
                                    wspY0 = Math.Round(wspY[jk], 3);
                                    wspZ0 = 0.0;

                                    wspX1 = Math.Round(wspX[jk + 1], 3);
                                    wspY1 = Math.Round(wspY[jk + 1], 3);
                                    wspZ1 = 0.0;

                                    deltaX01 = wspX1 - wspX0;
                                    deltaY01 = wspY1 - wspY0;

                                    d01 = Math.Sqrt(Math.Pow(deltaX01, 2) + Math.Pow(deltaY01, 2));

                                    angle = ValidAngle(Math.Atan2(deltaY01, deltaX01));

                                    midX = wspX1 + (wspX0 - wspX1) / 2;
                                    midY = wspY1 + (wspY0 - wspY1) / 2;
                                    midZ = 0.0;


                                    // in_dwg

                                    DBText text = new DBText();
                                    line_text = $"{prefix}{Math.Round(d01, 2).ToString(prec)}{sufix}";
                                    text.TextString = line_text;
                                    text.Position = new Point3d(midX, midY, midZ);
                                    try {
                                        text.Justify = AttachmentPoint.BottomCenter;
                                        text.AlignmentPoint = new Point3d(midX, midY, midZ);
                                    } catch { }
                                    text.Height = height;
                                    text.WidthFactor = witdhfactor;
                                    text.Rotation = angle;
                                    text.AdjustAlignment(db);

                                    compare_value = $"{midX}{midY}{midZ}{line_text}";

                                    ed.WriteMessage("before\n");

                                    if ((in_dwg.Contains(compare_value)) && duplicate) {
                                    } else {

                                        using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                            btr.AppendEntity(text);
                                            tr.AddNewlyCreatedDBObject(text, true);
                                            tr.Commit();
                                        }
                                        ed.WriteMessage("under\n");

                                        if (duplicate) { in_dwg.Add(compare_value); }

                                    }

                                }

                            } else {
                                ed.WriteMessage($"else state{lineEntity.GetType().Name}\n");

                                continue;

                                // lineEntity.GetType().Name != "Polyline"
                            }
                           
                            
                        }
                        trans.Commit();
                    }
                }
            }
        }

        private double ValidAngle(double angle) {
            if (angle < 0) {
                angle += 2 * Math.PI;
            }

            if (angle > (Math.PI / 2)) {
                if (angle < (1.5 * Math.PI)) {
                    angle -= Math.PI;
                }
            }

            return angle;
        }
    }


}

