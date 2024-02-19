using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;

namespace Geo_geo.Class {
    internal class cPikietowanie {



        ///<summary>
        /// Umieszcza ponmerowane punkty .
        ///</summary>
        ///<param name="bufor">Wartość dodana do numru punktu 1</param>
        ///<param name="sufix">Porostek po wartości.</param>
        ///<param name="prefix">Przedrostek przed wartością.</param>



        public void putOnVertex(string prefix = "", string sufix = "", long bufor = 0) {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            //double prec = 0.05;

            long lp = 0;
            lp = lp + bufor;

            long objNum = 0;

            double textH = 0.5;
            List<string> in_dwg = new List<string>();

            string compare_value = "";

            using (DocumentLock acLckDoc = doc.LockDocument()) {

                PromptSelectionResult selectionResult = ed.GetSelection();
                if (selectionResult.Status != PromptStatus.OK) {
                    ed.WriteMessage("Nie wybrano elementów.");
                    return;
                }

                SelectionSet selectionSet = selectionResult.Value;
                int liczba_porzadkowa = 0;


                foreach (SelectedObject lineSelectedObject in selectionSet) {
                    using (Transaction trans = db.TransactionManager.StartTransaction()) {
                        using (Entity lineEntity = trans.GetObject(lineSelectedObject.ObjectId, OpenMode.ForRead) as Entity) {


                            Line line = new Line();
                            Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                            Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                            Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();
                            objNum++;
                            ed.WriteMessage($"\nObject nr {objNum}: {lineEntity.GetType().Name}");


                            if (lineEntity == null) { continue; }

                            if (lineEntity.GetType().Name == "Line") {

                                line = lineEntity as Line;

                                Point3d startPoint = line.StartPoint;
                                Point3d endPoint = line.EndPoint;

                                DBText text0 = new DBText();
                                DBText text1 = new DBText();
                                

                                using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                                    compare_value = $"{Math.Round(startPoint.X, 2)}{Math.Round(startPoint.Y, 2)}{Math.Round(startPoint.Z, 2)}";

                                    if (in_dwg.Contains(compare_value)) {
                                    } else {
                                        lp++;
                                        text0.TextString = $"{prefix}{lp.ToString()}{sufix}";
                                        text0.Position = startPoint;
                                        text0.Height = textH;
                                        text0.AdjustAlignment(db);


                                        in_dwg.Add(compare_value);

                                        btr.AppendEntity(text0);
                                        tr.AddNewlyCreatedDBObject(text0, true);
                                        ed.WriteMessage($"\nVertex {lp} on line");
                                    }

                                    compare_value = $"{Math.Round(endPoint.X, 2)}{Math.Round(endPoint.Y, 2)}{Math.Round(endPoint.Z, 2)}";

                                    if (in_dwg.Contains(compare_value)) {
                                    } else {
                                        lp++;
                                        text1.TextString = $"{prefix}{lp.ToString()}{sufix}";
                                        text1.Position = endPoint;
                                        text1.Height = textH;
                                        text1.AdjustAlignment(db);


                                        in_dwg.Add(compare_value);
                                        btr.AppendEntity(text1);
                                        tr.AddNewlyCreatedDBObject(text1, true);
                                        ed.WriteMessage($"\nVertex {lp} on line");
                                    }



                                    tr.Commit();
                                }

                            } else if (lineEntity.GetType().Name == "Polyline") {

                                pline = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline;

                                if (pline == null) { continue; }


                                for (int jk = 0; jk < pline.NumberOfVertices; jk++) {

                                    Point3d vPoint = new Point3d(pline.GetPoint2dAt(jk).X, pline.GetPoint2dAt(jk).Y, pline.Elevation);

                                    compare_value = $"{Math.Round(vPoint.X, 2)}{Math.Round(vPoint.Y, 2)}{Math.Round(vPoint.Z, 2)}";

                                    if (in_dwg.Contains(compare_value)) {
                                    } else {
                                        lp++;
                                        DBText text = new DBText();
                                        text.TextString = $"{prefix}{lp.ToString()}{sufix}";
                                        text.Position = vPoint;
                                        text.Height = textH;
                                        text.AdjustAlignment(db);
                                        in_dwg.Add(compare_value);

                                        using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                            btr.AppendEntity(text);
                                            tr.AddNewlyCreatedDBObject(text, true);
                                            tr.Commit();
                                            ed.WriteMessage($"\nVertex {lp} on polyline");
                                        }
                                    }
                                }

                            } else if (lineEntity.GetType().Name == "Polyline3d") {

                                pline3d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline3d;

                                if (pline3d != null) {

                                    foreach (ObjectId vId in pline3d) {

                                        PolylineVertex3d v3d = (PolylineVertex3d)trans.GetObject(vId, OpenMode.ForRead);
                                        Point3d vPoint = new Point3d(v3d.Position.X, v3d.Position.Y, v3d.Position.Z);

                                        compare_value = $"{Math.Round(vPoint.X, 2)}{Math.Round(vPoint.Y, 2)}{Math.Round(vPoint.Z, 2)}";

                                        if (in_dwg.Contains(compare_value)) {
                                        } else {
                                            lp++;
                                            DBText text = new DBText();
                                            text.TextString = $"{prefix}{lp.ToString()}{sufix}";
                                            text.Position = vPoint;
                                            text.Height = textH;
                                            text.AdjustAlignment(db);
                                            in_dwg.Add(compare_value);

                                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                                btr.AppendEntity(text);
                                                tr.AddNewlyCreatedDBObject(text, true);
                                                tr.Commit();
                                                ed.WriteMessage($"\nVertex {lp} on polyline3D");
                                            }
                                        }
                                    }
                                }

                            } else if (lineEntity.GetType().Name == "Polyline2d") {
                                //Polyline2d
                                pline2d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                                //ed.WriteMessage($"\nPolyline2d");
                                if (pline2d != null) {

                                    // Use foreach to get each contained vertex

                                    foreach (ObjectId vId in pline2d) {

                                        Vertex2d v2d = (Vertex2d)trans.GetObject(vId, OpenMode.ForRead);
                                        Point3d vPoint = new Point3d(v2d.Position.X, v2d.Position.Y, 0.00);

                                        compare_value = $"{Math.Round(vPoint.X, 2)}{Math.Round(vPoint.Y, 2)}{Math.Round(vPoint.Z, 2)}";
                                        //ed.WriteMessage($"\n{vPoint.X}");

                                        if (in_dwg.Contains(compare_value)) {
                                        } else {
                                            lp++;
                                            DBText text = new DBText();
                                            text.TextString = $"{prefix}{lp.ToString()}{sufix}";
                                            text.Position = vPoint;
                                            text.Height = textH;
                                            text.AdjustAlignment(db);
                                            in_dwg.Add(compare_value);

                                            using (Transaction tr = db.TransactionManager.StartTransaction()) {
                                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                                btr.AppendEntity(text);
                                                tr.AddNewlyCreatedDBObject(text, true);
                                                tr.Commit();
                                                ed.WriteMessage($"\nVertex {lp} on polyline2D");
                                            }
                                        }

                                    }
                                }
                            } else {
                                ed.WriteMessage($"\nWrong object: {lineEntity.GetType().Name}\n");

                                continue;
                            }
                            trans.Commit();
                        }
                    }
                }

            }
        }
    }
}
