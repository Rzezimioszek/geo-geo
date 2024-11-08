using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Geo_geo.Class.FORMS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Geo_geo.Class {
    internal class cExportPik {

        public void ExportPointsFromTxtForm() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            string fileName;
            string fileContent;
            string[] lines;
            string[] points;
            int i = 0;
            int j = 0;
            double[] origin = new double[3];


            TypedValue[] typedValueArray = new TypedValue[1];
            typedValueArray.SetValue((object)new TypedValue(0, (object)"TEXT"), 0);

            SelectionFilter selectionFilter = new SelectionFilter(typedValueArray);

            PromptSelectionResult selection = ed.GetSelection(selectionFilter);
            if (selection.Status != PromptStatus.OK)
                return;

            cFileDlg dlg = new cFileDlg();
            fileName = dlg.SaveDlg();

            if (fileName == "return") {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }

            ed.WriteMessage($"\nExported: {fileName} points");

            using (StreamWriter streamWriter = new StreamWriter(fileName)) {

                string format = "F3";
                streamWriter.WriteLine("Nr\tX\tY\tZ");

                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    foreach (SelectedObject selectedObject in selection.Value) {
                        DBText dbText = tr.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as DBText;


                        double wspX = dbText.AlignmentPoint.X;
                        double wspY = dbText.AlignmentPoint.Y;
                        double wspZ = dbText.AlignmentPoint.Z;

                        if ((Math.Round(wspX, 2) == 0.00) && (Math.Round(wspY, 2) == 0.00) && (Math.Round(wspZ, 2) == 0.00)) {
                            wspX = dbText.Position.X;
                            wspY = dbText.Position.Y;
                            wspZ = dbText.Position.Z;

                        }
                        string x = wspX.ToString(format);
                        string y = wspY.ToString(format);
                        string z = wspZ.ToString(format);

                        string line = $"{dbText.TextString}\t{x}\t{y}\t{z}";
                        streamWriter.WriteLine(line);
                        j++;
                    }

                    tr.Commit();
                }

                ed.WriteMessage($"\nExported: {j} points");

            }
        }

        public void ExportPointsToEWmapa(string pts_type, string pts_format, double skala, bool NrH, bool h0, string separator) {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;



            ed.WriteMessage($"\nTyp: {pts_type}");
            ed.WriteMessage($"\nFormat: {pts_format}");
            ed.WriteMessage($"\nSkala: {skala}");

            string fileName;
            string fileContent;
            string[] lines;
            string[] points;
            int i = 0;
            int j = 0;
            double[] origin = new double[3];


            // stara selekcja
            TypedValue[] typedValueArray = new TypedValue[1];
            typedValueArray.SetValue((object)new TypedValue(0, (object)"TEXT"), 0);

            // stara selekcja


            SelectionFilter selectionFilter = new SelectionFilter(typedValueArray);
            PromptSelectionResult selection = ed.GetSelection(selectionFilter);
            if (selection.Status != PromptStatus.OK)
                return;

            // NOWA SELEKCJA multipunktowa 

            /*

            cObrot cO = new cObrot();
            List<ObjectId> text_ids = new List<ObjectId>();
            text_ids = cO.GetIds(selection, "text");

            */

            // NOWA SELEKCJA multipunktowa 



            string code = "RTPW02";

            cFileDlg dlg = new cFileDlg();


            fileName = dlg.SaveDlg();

            code = CodeDialog();

            if (fileName == "return") {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }

            ed.WriteMessage($"\nExported: {fileName} points");

            //using (StreamWriter streamWriter = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate), Encoding.ASCII)) {
            using (StreamWriter streamWriter = new StreamWriter(fileName)) {
                string format = "F2";

                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    //BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead);


                    foreach (SelectedObject selectedObject in selection.Value) {
                        DBText dbText = tr.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as DBText;

                        double wspX = dbText.AlignmentPoint.X;
                        double wspY = dbText.AlignmentPoint.Y;
                        double wspZ = dbText.AlignmentPoint.Z;

                        if ((Math.Round(wspX, 2) == 0.00) && (Math.Round(wspY, 2) == 0.00) && (Math.Round(wspZ, 2) == 0.00)) {
                            wspX = dbText.Position.X;
                            wspY = dbText.Position.Y;
                            wspZ = dbText.Position.Z;

                        }
                        string x = wspX.ToString(format);                        
                        string y = wspY.ToString(format);
                        string z = wspZ.ToString(format);

                        double rotate = dbText.Rotation;

                        rotate = 360 - (rotate * (180 / Math.PI));

                        if (rotate >= 360.0) {

                            rotate = rotate - 360;

                        }
                        string r = rotate.ToString(format);


                        string tag = dbText.TextString.ToString();

                        //ţRTPW01
                        //ţRTPW02
                        //ţOTRN
                        //ţOTRS

                        //string line = $"{y.Replace(".", ",")}\t{x.Replace(".", ",")}\t0,00\t{r.Replace(".", ",")}\t\"ţ{code}\"";
                        string line = $"{y}\t{x}\t0.00\t{r}\t\"ţ{code}\"";
                        streamWriter.WriteLine(line);

                        // line = $"{y.Replace(".", ",")}\t{x.Replace(".", ",")}\t0,00\t{r.Replace(".", ",")}\t\"{tag}\"";
                        line = $"{y}\t{x}\t0.00\t{r}\t\"{tag}\"";
                        streamWriter.WriteLine(line);
                        j++;
                    }



                    tr.Commit();
                }





                ed.WriteMessage($"\nExported: {j} points");
            }
        }

        public void ExportMText() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            string fileName;
            string fileContent;
            string[] lines;
            string[] points;
            int i = 0;
            int j = 0;
            double[] origin = new double[3];

 
            

            TypedValue[] typedValueArray = new TypedValue[1];
            typedValueArray.SetValue((object)new TypedValue(0, (object)"MTEXT"), 0);

            SelectionFilter selectionFilter = new SelectionFilter(typedValueArray);

            PromptSelectionResult selection = ed.GetSelection(selectionFilter);
            if (selection.Status != PromptStatus.OK)
                return;

            cFileDlg dlg = new cFileDlg();
            fileName = dlg.SaveDlg();

            if (fileName == "return") {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }

     

            ed.WriteMessage($"\nExported: {fileName} points");

            using (StreamWriter streamWriter = new StreamWriter(fileName)) {

                string format = "F3";
                streamWriter.WriteLine("Nr\tX\tY\tZ");

                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    foreach (SelectedObject selectedObject in selection.Value) {

                        MText mtext = tr.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as MText;

                        double wspX = mtext.Location.X;
                        string x = wspX.ToString(format);

                        double wspY = mtext.Location.Y;
                        string y = wspY.ToString(format);

                        double wspZ = mtext.Location.Z;
                        string z = wspZ.ToString(format);

                        string line = $"{mtext.Contents}\t{x}\t{y}\t{z}";
                        streamWriter.WriteLine(line);
                        j++;
                    }

                    tr.Commit();
                }

                ed.WriteMessage($"\nExported: {j} points");
            }
        }

        public void ExportMultiText() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            string fileName;
            string fileContent;
            string[] lines;
            string[] points;
            int i = 0;
            int j = 0;
            double[] origin = new double[3];

            PromptSelectionResult selection = ed.GetSelection();
            if (selection.Status != PromptStatus.OK)
                return;

            cObrot cO = new cObrot();
            List<ObjectId> text_ids = new List<ObjectId>();
            text_ids = cO.GetIds(selection, "text");

            cFileDlg dlg = new cFileDlg();
            fileName = dlg.SaveDlg();

            if (fileName == "return") {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }

            ed.WriteMessage($"\nExported: {fileName} points");

            using (StreamWriter streamWriter = new StreamWriter(fileName)) {

                string format = "F3";
                streamWriter.WriteLine("Nr\tX\tY\tZ");

                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    foreach (ObjectId text_id in text_ids) {

                        using (Entity entity = tr.GetObject(text_id, OpenMode.ForRead) as Entity) {

                            string objType = entity.GetType().Name;

                            string name = "";

                            string x = "";
                            string y = "";
                            string z = "";

                            double wspX = 0.0;
                            double wspY = 0.0;
                            double wspZ = 0.0;

                            if (objType == "DBText") {

                                DBText dbText = entity as DBText;

                                name = dbText.TextString;

                                wspX = dbText.AlignmentPoint.X;
                                wspY = dbText.AlignmentPoint.Y;
                                wspZ = dbText.AlignmentPoint.Z;

                                if ((Math.Round(wspX, 2) == 0.00) && (Math.Round(wspY, 2) == 0.00) && (Math.Round(wspZ, 2) == 0.00)) {
                                    wspX = dbText.Position.X;
                                    wspY = dbText.Position.Y;
                                    wspZ = dbText.Position.Z;

                                }
                                x = wspX.ToString(format);
                                y = wspY.ToString(format);
                                z = wspZ.ToString(format);

                            } else if (objType == "BlockReference") {

                                BlockReference blockRef = entity as BlockReference;

                                wspX = blockRef.Position.X;
                                wspY = blockRef.Position.Y;
                                wspZ = blockRef.Position.Z;

                                x = wspX.ToString(format);
                                y = wspY.ToString(format);
                                z = wspZ.ToString(format);

                            } else if (objType == "MText") {

                                MText textMObj = entity as MText;

                                name = textMObj.Text;
                                wspX = textMObj.Location.X;
                                wspY = textMObj.Location.Y;
                                wspZ = textMObj.Location.Z;

                                x = wspX.ToString(format);
                                y = wspY.ToString(format);
                                z = wspZ.ToString(format);

                            }

                            string line = $"{name}\t{x}\t{y}\t{z}";
                            streamWriter.WriteLine(line);
                            j++;
                        }
                    }
                    tr.Commit();
                }
                ed.WriteMessage($"\nExported: {j} points");
            }
        }


        public void ExportPointsFromTxtToKml() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            string fileName;
            string fileContent;
            string[] lines;
            string[] points;
            int i = 0;
            int j = 0;
            double[] origin = new double[3];

            string heading = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<kml xmlns=\"http://www.opengis.net/kml/2.2\" xmlns:gx=\"http://www.google.com/kml/ext/2.2\" xmlns:kml=\"http://www.opengis.net/kml/2.2\" xmlns:atom=\"http://www.w3.org/2005/Atom\">";
            string h1 = "<Document id=\"root_doc\"><Schema id=\"DZIALKI\" name=\"DZIALKI\"><SimpleField name=\"Layer\" type=\"string\"/><SimpleField name=\"Name\" type=\"string\"/><SimpleField name=\"X\" type=\"float\"/><SimpleField name=\"Y\" type=\"float\"/><SimpleField name=\"H\" type=\"float\"/><SimpleField name=\"field_1\" type=\"string\"/><SimpleField name=\"field_2\" type=\"string\"/><SimpleField name=\"field_3\" type=\"string\"/><SimpleField name=\"field_4\" type=\"string\"/><SimpleField name=\"field_5\" type=\"string\"/><SimpleField name=\"field_6\" type=\"string\"/><SimpleField name=\"field_7\" type=\"string\"/><SimpleField name=\"field_8\" type=\"string\"/><SimpleField name=\"field_9\" type=\"string\"/><SimpleField name=\"field_10\" type=\"string\"/></Schema>";
            string h2 = "<Folder><name>acKML</name>";
            heading = $"{heading}{h1}{h2}";


            string footer = "</Folder></Document></kml>";

            TypedValue[] typedValueArray = new TypedValue[1];
            typedValueArray.SetValue((object)new TypedValue(0, (object)"TEXT"), 0);

            SelectionFilter selectionFilter = new SelectionFilter(typedValueArray);
            PromptSelectionResult selection = ed.GetSelection(selectionFilter);
            if (selection.Status != PromptStatus.OK)
                return;

            /*
            PromptSaveFileOptions saveOpts = new PromptSaveFileOptions("Wybierz plik do zapisu");
            saveOpts.Filter = "kml files (*.kml)|*.kml|All files (*.*)|*.*";

            PromptFileNameResult fileResult = ed.GetFileNameForSave(saveOpts);
            if (fileResult.Status != PromptStatus.OK) {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }
            fileName = fileResult.StringResult;
            */
            cFileDlg dlg = new cFileDlg();
            fileName = dlg.SaveDlg("kml files (*.kml)|*.kml|All files (*.*)|*.*", ".kml");

            if (fileName == "return") {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }

            //ed.WriteMessage($"\nExported: {fileName} points");

            cWeb xyTrans = new Geo_geo.Class.cWeb();

            using (StreamWriter streamWriter = new StreamWriter(fileName)) {

                string format = "F2";

                //CultureInfo invariantCulture = CultureInfo.InvariantCulture;
                streamWriter.WriteLine(heading);


                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    //BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead);


                    foreach (SelectedObject selectedObject in selection.Value) {
                        DBText dbText = tr.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as DBText;

                        double wspX = dbText.AlignmentPoint.X;
                        double wspY = dbText.AlignmentPoint.Y;
                        double wspZ = dbText.AlignmentPoint.Z;

                        if ((Math.Round(wspX, 2) == 0.00) && (Math.Round(wspY, 2) == 0.00) && (Math.Round(wspZ, 2) == 0.00)) {
                            wspX = dbText.Position.X;
                            wspY = dbText.Position.Y;
                            wspZ = dbText.Position.Z;

                        }

                        string x = wspX.ToString(format);
                        string y = wspY.ToString(format);

                        //double wspZ = dbText.Position.Z;
                        //string z = wspZ.ToString(format);

                        Point3d oldPoint = new Point3d(wspX, wspY, 0.0);

                        Point3d newPoint = xyTrans.SetEPSG(oldPoint);

                        string line = $"<Placemark>\n<name>{dbText.TextString}</name>\n<description>X: {y}\nY: {x}</description>\n<Point>\n<coordinates>{newPoint.X},{newPoint.Y}</coordinates>\n</Point>\n</Placemark>";
                        streamWriter.WriteLine(line);
                        j++;
                    }



                    tr.Commit();
                }

                streamWriter.WriteLine(footer);





                ed.WriteMessage($"\nExported: {j} points");


            }

        }

        public void ExportAllToKml() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            cObrot cO = new cObrot();

            string fileName;
            string fileContent;
            string[] lines;
            string[] points;
            int i = 0;
            int j = 0;
            double[] origin = new double[3];

            int curStyle = 0;

            string heading = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r" +
                "\n<kml xmlns=\"http://www.opengis.net/kml/2.2\" " +
                "xmlns:gx=\"http://www.google.com/kml/ext/2.2\" " +
                "xmlns:kml=\"http://www.opengis.net/kml/2.2\" " +
                "xmlns:atom=\"http://www.w3.org/2005/Atom\">";
            string h1 = "<Document id=\"root_doc\">" +
                "\n<Schema id=\"Point\" name=\"Point\">" +
                "\n\t<SimpleField name=\"Layer\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"Value\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"X\" type=\"float\"/>" +
                "\n\t<SimpleField name=\"Y\" type=\"float\"/>" +
                "\n\t<SimpleField name=\"H\" type=\"float\"/>" +
                "\n\t<SimpleField name=\"Rotation\" type=\"float\"/>" +
                "\n\t<SimpleField name=\"field_1\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"field_2\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"field_3\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"field_4\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"field_5\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"field_6\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"field_7\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"field_8\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"field_9\" type=\"string\"/>" +
                "\n\t<SimpleField name=\"field_10\" type=\"string\"/>" +
                "\n</Schema>";

            string h2 = "\n<Folder>\n<name>acKML</name>";
            heading = $"{heading}{h1}{h2}";
            string footer = "</Folder>\n</Document>\n</kml>";

            PromptSelectionResult selection = ed.GetSelection();
            if (selection.Status != PromptStatus.OK)
                return;

            cFileDlg dlg = new cFileDlg();
            fileName = dlg.SaveDlg("kml files (*.kml)|*.kml|All files (*.*)|*.*", ".kml");

            if (fileName == "return") {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }

            cWeb xyTrans = new Geo_geo.Class.cWeb();

            int lid = 0;

            List<ObjectId> line_ids = new List<ObjectId>();
            List<ObjectId> text_ids = new List<ObjectId>();

            line_ids = cO.GetIds(selection, "lineCircle");
            text_ids = cO.GetIds(selection, "text");

            string style = "";

            List<string> lineLayerList = new List<string>();
            lineLayerList = cO.GetLayerNameBySelected(selection, "all");

            foreach (string layerName in lineLayerList) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {

                    LayerTable acLyrTbl;
                    acLyrTbl = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                    using (LayerTableRecord acLyrTblRec = trans.GetObject(acLyrTbl[layerName], OpenMode.ForWrite) as LayerTableRecord) {

                        Autodesk.AutoCAD.Colors.Color c = Color.FromColorIndex(ColorMethod.ByLayer, acLyrTblRec.Color.ColorIndex);

                        string[] col = new string[3];
                        col[0] = ((int)c.ColorValue.B).ToString("X2");
                        col[1] = ((int)c.ColorValue.G).ToString("X2");
                        col[2] = ((int)c.ColorValue.R).ToString("X2");

                        string valid_color = $"FF{col[0]}{col[1]}{col[2]}";

                        style = style + $"<Style id=\"{layerName}\">" +
                            $"\n\t<LineStyle>" +
                            $"\n\t\t<color>{valid_color}</color>" +
                            $"\n\t\t<width>2</width>" +
                            $"\n\t</LineStyle>" +
                            $"\n\t<PolyStyle><fill>0</fill></PolyStyle>" +
                            $"\n</Style>" +
                            $"<Style id=\"pkt_{layerName}\">" +
                            $"\n\t<IconStyle>" +
                            $"\n\t\t<color>{valid_color}</color>" +
                            $"\n\t\t<scale>1</scale>" +
                            $"\n\t\t<heading>0</heading>" +
                            $"\n\t</IconStyle>" +
                            $"\n</Style>";

                    }
                }
            }

            int allObj = line_ids.Count + text_ids.Count;

            using (StreamWriter streamWriter = new StreamWriter(fileName)) {

                string format = "F2";
                streamWriter.WriteLine(heading);
                streamWriter.WriteLine(style);

                Line lineEnt = new Line();
                Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();

                if (line_ids.Count > 0) {

                    streamWriter.WriteLine("<Folder>\r\n\t<name>Linie</name>\r\n\t<open>1</open>");
                    //

                    foreach (ObjectId line_id in line_ids) {
                        using (Transaction trans = db.TransactionManager.StartTransaction()) {
                            using (Entity entity = trans.GetObject(line_id, OpenMode.ForRead) as Entity) {

                                if (entity.GetType().Name == "Line") {

                                    lineEnt = entity as Line;
                                    string layer = lineEnt.Layer;

                                    Point3d startPoint = new Point3d(lineEnt.StartPoint.X, lineEnt.StartPoint.Y, 0.0);
                                    Point3d endPoint = new Point3d(lineEnt.EndPoint.X, lineEnt.EndPoint.Y, 0.0);
                                    Point3d newSPoint = xyTrans.SetEPSG(startPoint);
                                    Point3d newEPoint = xyTrans.SetEPSG(endPoint);

                                    string coords = $"{newSPoint.X},{newSPoint.Y} {newEPoint.X},{newEPoint.Y}";

                                    string line = $"<Placemark>" +
                                        $"\n\t<styleUrl>#{layer}</styleUrl>" +
                                        $"\n\t<name>{layer}</name>" +
                                        $"\n\t<LineString>" +
                                        $"\n\t\t<coordinates>{coords}</coordinates>" +
                                        $"\n\t</LineString>" +
                                        $"\n</Placemark>";

                                    streamWriter.WriteLine(line);

                                } else if (entity.GetType().Name == "Polyline") {

                                    pline = entity as Autodesk.AutoCAD.DatabaseServices.Polyline;
                                    string coords = "";
                                    string layer = pline.Layer;

                                    for (int jk = 0; jk < (pline.NumberOfVertices); jk++) {

                                        double wspX0;
                                        double wspY0;
                                        wspX0 = Math.Round(pline.GetPoint2dAt(jk).X, 3);
                                        wspY0 = Math.Round(pline.GetPoint2dAt(jk).Y, 3);

                                        Point3d oldPoint = new Point3d(wspX0, wspY0, 0.0);
                                        Point3d newSPoint = xyTrans.SetEPSG(oldPoint);
                                        coords = $"{coords} {newSPoint.X},{newSPoint.Y}";

                                    }

                                    string line = $"<Placemark>" +
                                        $"\n\t<styleUrl>#{layer}</styleUrl>" +
                                        $"\n\t<name>{layer}</name>" +
                                        $"\n\t<LineString>" +
                                        $"\n\t\t<coordinates>{coords}</coordinates>" +
                                        $"\n\t</LineString>" +
                                        $"\n</Placemark>";
                                    streamWriter.WriteLine(line);

                                } else if (entity.GetType().Name == "Polyline3d") {

                                    //Polyline
                                    pline3d = entity as Autodesk.AutoCAD.DatabaseServices.Polyline3d;
                                    int counter_of_vertex = 0;
                                    string coords = "";
                                    string layer = pline3d.Layer;

                                    if (pline3d != null) {

                                        foreach (ObjectId vId in pline3d) {

                                            PolylineVertex3d v3d = (PolylineVertex3d)trans.GetObject(vId, OpenMode.ForRead);
                                            counter_of_vertex++;
                                            Point3d oldPoint = new Point3d(v3d.Position.X, v3d.Position.Y, 0.0);
                                            Point3d newSPoint = xyTrans.SetEPSG(oldPoint);

                                            coords = $"{coords} {newSPoint.X},{newSPoint.Y}";
                                        }

                                        string line = $"<Placemark>" +
                                            $"\n\t<styleUrl>#{layer}</styleUrl>" +
                                            $"\n\t<name>{layer}</name>" +
                                            $"\n\t<LineString>" +
                                            $"\n\t\t<coordinates>{coords}</coordinates>" +
                                            $"\n\t</LineString>" +
                                            $"\n</Placemark>";
                                        streamWriter.WriteLine(line);

                                    }


                                } else if (entity.GetType().Name == "Polyline2d") {

                                    pline2d = entity as Autodesk.AutoCAD.DatabaseServices.Polyline2d;
                                    int counter_of_vertex = 0;
                                    string coords = "";
                                    string layer = pline2d.Layer;

                                    if (pline2d != null) {

                                        foreach (ObjectId vId in pline2d) {

                                            Vertex2d v2d = (Vertex2d)trans.GetObject(vId, OpenMode.ForRead);
                                            counter_of_vertex++;
                                            Point3d oldPoint = new Point3d(v2d.Position.X, v2d.Position.Y, 0.0);
                                            Point3d newSPoint = xyTrans.SetEPSG(oldPoint);

                                            coords = $"{coords} {newSPoint.X},{newSPoint.Y}";

                                        }

                                        string line = $"<Placemark>" +
                                            $"\n\t<styleUrl>#{layer}</styleUrl>" +
                                            $"\n\t<name>{layer}</name>" +
                                            $"\n\t<LineString>" +
                                            $"\n\t\t<coordinates>{coords}</coordinates>" +
                                            $"\n\t</LineString>" +
                                            $"\n</Placemark>";
                                        streamWriter.WriteLine(line);

                                    }

                                } else if (entity.GetType().Name == "Circle") {
                                    Circle acCirc = entity as Circle;
                                    string layer = acCirc.Layer;

                                    string coords = "";

                                    Point3d oldPoint = acCirc.Center;
                                    double radius = acCirc.Radius;
                                    Point3d tempPoint = new Point3d();
                                    Point3d newPoint = new Point3d();

                                    for (double itr = 0.0; itr < 2.0; itr += 0.0625) {

                                        tempPoint = new Point3d(oldPoint.X + (radius * Math.Sin(Math.PI * itr)),
                                        oldPoint.Y + (radius * Math.Cos(Math.PI * itr)),
                                        oldPoint.Z);
                                        newPoint = xyTrans.SetEPSG(tempPoint);
                                        coords = $"{coords} {newPoint.X},{newPoint.Y}";

                                    }

                                    tempPoint = new Point3d(oldPoint.X + (radius * Math.Sin(Math.PI * 0.0)),
                                        oldPoint.Y + (radius * Math.Cos(Math.PI * 0.0)),
                                        oldPoint.Z);
                                    newPoint = xyTrans.SetEPSG(tempPoint);
                                    coords = $"{coords} {newPoint.X},{newPoint.Y}";

                                    string line = $"<Placemark>" +
                                                $"\n\t<styleUrl>#{layer}</styleUrl>" +
                                                $"\n\t<name>{layer}</name>" +
                                                $"\n\t<LineString>" +
                                                $"\n\t\t<coordinates>{coords}</coordinates>" +
                                                $"\n\t</LineString>" +
                                                $"\n</Placemark>";

                                    streamWriter.WriteLine(line);

                                } else if (entity.GetType().Name == "Arc") {

                                    Arc acArc = entity as Arc;
                                    string layer = acArc.Layer;
                                    double radius = acArc.Radius;
                                    string coords = "";

                                    Point3d oldPoint = acArc.Center;
                                    Point3d tempPoint = new Point3d();

                                    double angleMin = acArc.StartAngle;
                                    double angleMax = acArc.EndAngle;
                                    Point3d fPoint = new Point3d(acArc.StartPoint.X, acArc.StartPoint.Y, 0);
                                    Point3d lPoint = new Point3d(acArc.EndPoint.X, acArc.EndPoint.Y, 0);

                                    Line dummy = new Line(oldPoint, fPoint);
                                    double startAngle = -(dummy.Angle - (Math.PI / 2));

                                    bool reverse = false;
                                    double diff = acArc.TotalAngle;

                                    if (diff < 0) { diff = -diff; } // can't be


                                    int vNum = 0;

                                    Point3d newPoint = new Point3d();

                                    for (double itr = 0.0; itr < diff; itr += 0.1805) {

                                        tempPoint = new Point3d(oldPoint.X + (radius * Math.Sin(startAngle - itr)),
                                                                oldPoint.Y + (radius * Math.Cos(startAngle - itr)),
                                                                0.0);
                                        newPoint = xyTrans.SetEPSG(tempPoint);
                                        vNum++;
                                        coords = $"{coords} {newPoint.X},{newPoint.Y}";
                                    }

                                    tempPoint = new Point3d(oldPoint.X + (radius * Math.Sin(startAngle - diff)),
                                                            oldPoint.Y + (radius * Math.Cos(startAngle - diff)),
                                                            0.0);
                                    newPoint = xyTrans.SetEPSG(tempPoint);
                                    coords = $"{coords} {newPoint.X},{newPoint.Y}";

                                    string line = $"<Placemark>" +
                                                $"\n\t<styleUrl>#{layer}</styleUrl>" +
                                                $"\n\t<name>{layer}</name>" +
                                                $"\n\t<LineString>" +
                                                $"\n\t\t<coordinates>{coords}</coordinates>" +
                                                $"\n\t</LineString>" +
                                                $"\n</Placemark>";

                                    streamWriter.WriteLine(line);

                                } else {
                                    continue;
                                }

                                lid++;
                                ed.WriteMessage($"\n{lid}/{allObj}");

                            }
                        }
                    }
                    streamWriter.WriteLine("</Folder>");
                }

                if (text_ids.Count > 0) {

                    streamWriter.WriteLine("<Folder>\r\n\t<name>Punkty</name>\r\n\t<open>1</open>");
                    foreach (ObjectId text_id in text_ids) {
                        using (Transaction trans = db.TransactionManager.StartTransaction()) {
                            using (Entity entity = trans.GetObject(text_id, OpenMode.ForRead) as Entity) {

                                string objType = entity.GetType().Name;

                                if (objType == "DBText") {

                                    DBText dbText = entity as DBText;

                                    double wspX = dbText.AlignmentPoint.X;
                                    double wspY = dbText.AlignmentPoint.Y;
                                    double wspZ = dbText.AlignmentPoint.Z;

                                    if ((Math.Round(wspX, 2) == 0.00) || (Math.Round(wspY, 2) == 0.00) || (Math.Round(wspZ, 2) == 0.00)) {
                                        wspX = dbText.Position.X;
                                        wspY = dbText.Position.Y;
                                        wspZ = dbText.Position.Z;

                                    }
                                    string x = wspX.ToString(format);
                                    string y = wspY.ToString(format);
                                    string h = wspZ.ToString(format);

                                    Point3d oldPoint = new Point3d(wspX, wspY, 0.0);
                                    Point3d newPoint = xyTrans.SetEPSG(oldPoint);

                                    string name = dbText.TextString;
                                    string layer = dbText.Layer;
                                    double r = dbText.Rotation * (180 / Math.PI);

                                    string ldata = $"\t\t<SimpleData name=\"Layer\">{layer}</SimpleData>" +
                                        $"\n\t\t<SimpleData name=\"Value\">{name}</SimpleData>" +
                                        $"\n\t\t<SimpleData name=\"X\">{y}</SimpleData>" +
                                        $"\n\t\t<SimpleData name=\"Y\">{x}</SimpleData>" +
                                        $"\n\t\t<SimpleData name=\"H\">{h}</SimpleData>" +
                                        $"\n\t\t<SimpleData name=\"Rotation\">{r}</SimpleData>";

                                    string l1 = $"<Placemark>" +
                                        $"\n<styleUrl>#{layer}</styleUrl>" +
                                        $"\n<name>{name}</name>" +
                                        $"\n<description>X: {y}\nY: {x}</description>";
                                    string l2 = $"<ExtendedData>" +
                                        $"\n\t<SchemaData schemaUrl=\"#Point\">" +
                                        $"\n{ldata}" +
                                        $"\n\t</SchemaData>" +
                                        $"\n</ExtendedData>";
                                    string l3 = $"<Point>" +
                                        $"\n\t<coordinates>{newPoint.X},{newPoint.Y}</coordinates>" +
                                        $"\n</Point>" +
                                        $"\n</Placemark>";

                                    string line = $"{l1}\n{l2}\n{l3}";

                                    streamWriter.WriteLine(line);

                                } else if (objType == "BlockReference") {

                                    BlockReference blockRef = entity as BlockReference;
                                    double wspX = blockRef.Position.X;
                                    double wspY = blockRef.Position.Y;
                                    double wspH = blockRef.Position.Z;

                                    string x = wspX.ToString(format);
                                    string y = wspY.ToString(format);
                                    string h = wspH.ToString(format);

                                    Point3d oldPoint = new Point3d(wspX, wspY, 0.0);
                                    Point3d newPoint = xyTrans.SetEPSG(oldPoint);

                                    string name = blockRef.Name;
                                    string layer = blockRef.Layer;
                                    double r = blockRef.Rotation * (180 / Math.PI);

                                    string ldata = $"\t\t<SimpleData name=\"Layer\">{layer}</SimpleData>\n" +
                                                    $"\n\t\t<SimpleData name=\"Value\">{name}</SimpleData>" +
                                                    $"\n\t\t<SimpleData name=\"X\">{y}</SimpleData>" +
                                                    $"\n\t\t<SimpleData name=\"Y\">{x}</SimpleData>" +
                                                    $"\n\t\t<SimpleData name=\"H\">{h}</SimpleData>" +
                                                    $"\n\t\t<SimpleData name=\"Rotation\">{r}</SimpleData>";

                                    int fid = 0;
                                    AttributeCollection attCol = blockRef.AttributeCollection;
                                    foreach (ObjectId attId in attCol) {

                                        AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);
                                        fid++;
                                        if (fid > 10) { break; }
                                        if (attRef.Tag == "ID") { name = attRef.TextString; }

                                        ldata = ldata + $"\n\t\t<SimpleData name=\"field_{fid}\">{attRef.TextString}</SimpleData>";
                                    }

                                    string l1 = $"<Placemark>" +
                                        $"\n<styleUrl>#{layer}</styleUrl>" +
                                        $"\n<name>{name}</name>" +
                                        $"\n<description>X: {y}\nY: {x}</description>";
                                    string l2 = $"<ExtendedData>" +
                                        $"\n\t<SchemaData schemaUrl=\"#Point\">" +
                                        $"\n{ldata}" +
                                        $"\n\t</SchemaData>" +
                                        $"\n</ExtendedData>";
                                    string l3 = $"<Point>" +
                                        $"\n\t<coordinates>{newPoint.X},{newPoint.Y}</coordinates>" +
                                        $"\n</Point>" +
                                        $"\n</Placemark>";
                                    string line = $"{l1}\n{l2}\n{l3}";

                                    streamWriter.WriteLine(line);

                                } else if (objType == "MText") {

                                    MText textMObj = entity as MText;
                                    double wspX = textMObj.Location.X;
                                    double wspY = textMObj.Location.Y;
                                    double wspH = textMObj.Location.Z;

                                    string x = wspX.ToString(format);
                                    string y = wspY.ToString(format);
                                    string h = wspH.ToString(format);

                                    Point3d oldPoint = new Point3d(wspX, wspY, 0.0);
                                    Point3d newPoint = xyTrans.SetEPSG(oldPoint);

                                    string name = textMObj.Text;
                                    string layer = textMObj.Layer;
                                    double r = textMObj.Rotation * (180 / Math.PI);

                                    string ldata = $"\t\t<SimpleData name=\"Layer\">{layer}</SimpleData>\n" +
                                                    $"\n\t\t<SimpleData name=\"Value\">{name}</SimpleData>" +
                                                    $"\n\t\t<SimpleData name=\"X\">{y}</SimpleData>" +
                                                    $"\n\t\t<SimpleData name=\"Y\">{x}</SimpleData>" +
                                                    $"\n\t\t<SimpleData name=\"H\">{h}</SimpleData>" +
                                                    $"\n\t\t<SimpleData name=\"Rotation\">{r}</SimpleData>";

                                    string l1 = $"<Placemark>" +
                                        $"\n<styleUrl>#{layer}</styleUrl>" +
                                        $"\n<name>{name}</name>" +
                                        $"\n<description>X: {y}\nY: {x}</description>";
                                    string l2 = $"<ExtendedData>" +
                                        $"\n\t<SchemaData schemaUrl=\"#Point\">" +
                                        $"\n{ldata}" +
                                        $"\n\t</SchemaData>" +
                                        $"\n</ExtendedData>";
                                    string l3 = $"<Point>" +
                                        $"\n\t<coordinates>{newPoint.X},{newPoint.Y}</coordinates>" +
                                        $"\n</Point>" +
                                        $"\n</Placemark>";
                                    string line = $"{l1}\n{l2}\n{l3}";

                                    streamWriter.WriteLine(line);

                                } else {

                                    continue;
                                }
                            }
                        }
                        lid++;
                        ed.WriteMessage($"\n{lid}/{allObj}");
                    }
                    streamWriter.WriteLine("</Folder>");
                }
                ed.WriteMessage($"\nWyeksportowano elementów: {lid}\n");
                streamWriter.WriteLine(footer);
            }
        }

        public List<string> ExportPointsToList(bool reverse = false, bool withName = false) {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            string format = "F3";

            string fileName;
            string fileContent;
            List<string> lines = new List<string>();
            string[] points;
            int i = 0;
            int j = 0;
            double[] origin = new double[3];

            PromptSelectionResult selection = ed.GetSelection();
            if (selection.Status != PromptStatus.OK)
                return lines;

            cObrot cO = new cObrot();
            List<ObjectId> text_ids = new List<ObjectId>();
            text_ids = cO.GetIds(selection, "text");

            using (Transaction tr = db.TransactionManager.StartTransaction()) {

                foreach (ObjectId text_id in text_ids) {

                    using (Entity entity = tr.GetObject(text_id, OpenMode.ForRead) as Entity) {

                        string objType = entity.GetType().Name;

                        string name = "";

                        string x = "";
                        string y = "";
                        string z = "";

                        double wspX = 0.0;
                        double wspY = 0.0;
                        double wspZ = 0.0;

                        if (objType == "DBText") {

                            DBText dbText = entity as DBText;

                            name = dbText.TextString;
                            wspX = dbText.AlignmentPoint.X;
                            wspY = dbText.AlignmentPoint.Y;
                            wspZ = dbText.AlignmentPoint.Z;

                            if ((Math.Round(wspX, 2) == 0.00) && (Math.Round(wspY, 2) == 0.00) && (Math.Round(wspZ, 2) == 0.00)) {
                                wspX = dbText.Position.X;
                                wspY = dbText.Position.Y;
                                wspZ = dbText.Position.Z;

                            }

                            x = wspX.ToString(format);
                            y = wspY.ToString(format);
                            z = wspZ.ToString(format);

                        } else if (objType == "BlockReference") {

                            BlockReference blockRef = entity as BlockReference;

                            wspX = blockRef.Position.X;
                            wspY = blockRef.Position.Y;
                            wspZ = blockRef.Position.Z;

                            x = wspX.ToString(format);
                            y = wspY.ToString(format);
                            z = wspZ.ToString(format);

                        } else if (objType == "MText") {

                            MText textMObj = entity as MText;

                            name = textMObj.Text;
                            wspX = textMObj.Location.X;
                            wspY = textMObj.Location.Y;
                            wspZ = textMObj.Location.Z;

                            x = wspX.ToString(format);
                            y = wspY.ToString(format);
                            z = wspZ.ToString(format);

                        }

                        string line = $"{x}\t{y}\t{z}";

                        if (withName) {
                            line = $"{name}\t{x}\t{y}\t{z}";
                        }


                        if (reverse) {
                            line = $"{y}\t{x}\t{z}";

                            if (withName) {
                                line = $"{name}\t{y}\t{x}\t{z}";
                            }
                        }

                        lines.Add(line);
                        j++;
                    }
                }
                return lines;
            }
        }

        public string CodeDialog() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (fEWmapa frm = new fEWmapa(Cursor.Position.X, Cursor.Position.Y)) {

                // frm.SetDesktopLocation(Cursor.Position.X, Cursor.Position.Y);

                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    ed.WriteMessage($"\n{frm.ReturnValue}");
                    return frm.ReturnValue;
                }
            }
            return "ORTN";
        }
    }

}
