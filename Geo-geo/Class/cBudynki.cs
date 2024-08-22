using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.IO;

namespace Geo_geo.Class {
    internal class cBudynki {

        public void InsertBuilding() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
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

            cFileDlg dlg = new cFileDlg();
            fileName = dlg.OpenDlg();

            if (fileName == "return") {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }

            // Read the content of the text file
            using (StreamReader sr = new StreamReader(fileName)) {
                fileContent = sr.ReadToEnd();
            }

            // Split the file content into lines
            lines = fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            string separator = "Tabulacja";

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

                try {

                    if (lines[i] == "") {
                        continue;
                    }

                    ed.WriteMessage($"\nLoad line: {i}");

                    if (lines[i].Contains($" ")) {

                        lines[i] = lines[i].Replace($" ", $"{sep}");

                    }


                    while (lines[i].Contains($"{sep}{sep}")) {
                        lines[i] = lines[i].Replace($"{sep}{sep}", $"{sep}");
                    }

                    lines[i] = lines[i].Replace(",", ".");

                    //points = lines[i].Split(sep);
                }
                catch { }

            }

            int lp = 0;
            int j = 1;

            for (int i = 0; i < (lines.Length - 1); i++) {

                int nr = 0;
                int x = 2;
                int y = 3;
                int h = 4;
                int h2 = 5;

                if (lines[i] == "") {
                    continue;
                }

                lp++;



                using (Transaction transModify = db.TransactionManager.StartTransaction()) {

                    BlockTableRecord btr = (BlockTableRecord)transModify.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                    j = i + 1;

                    points = lines[i].Split(sep);

                    //ed.WriteMessage($"\nWALL: {points[nr]}");

                    Point3d p0 = new Point3d(double.Parse(points[x]), double.Parse(points[y]), double.Parse(points[h]));
                    Point3d p1 = new Point3d(double.Parse(points[x]), double.Parse(points[y]), double.Parse(points[h2]));

                    double number = double.Parse(points[nr]);

                    points = lines[j].Split(sep);

                    Point3d p2 = new Point3d(double.Parse(points[x]), double.Parse(points[y]), double.Parse(points[h2]));
                    Point3d p3 = new Point3d(double.Parse(points[x]), double.Parse(points[y]), double.Parse(points[h]));

                    double numberX = double.Parse(points[nr]);


                    if (numberX == number) {

                        Autodesk.AutoCAD.DatabaseServices.Face face = new Autodesk.AutoCAD.DatabaseServices.Face(p0, p1, p2, p3, true, true, true, true);

                        btr.AppendEntity(face);
                        transModify.AddNewlyCreatedDBObject(face, true);
                        transModify.Commit();

                    }
                }
            }

            double last = -1.0;
            Point3dCollection ptr = new Point3dCollection();

            List<string> errors = new List<string>();



            for (int i = 0; i < (lines.Length); i++) {

                string current = "";

                if (lines[i] == "") {
                    continue;
                }

                try {

                    int nr = 0;
                    int x = 2;
                    int y = 3;
                    int h = 4;
                    int h2 = 5;


                    points = lines[i].Split(sep);

                    if (points.Length < 2 ) {

                        using (Transaction transModify = db.TransactionManager.StartTransaction()) {

                            BlockTableRecord btr = (BlockTableRecord)transModify.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                            Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d(Poly3dType.SimplePoly, ptr, true);
                            var curves = new DBObjectCollection();
                            curves.Add(pline3d);

                            var regions = Region.CreateFromCurves(curves);
                            var region = (Region)regions[0];

                            btr.AppendEntity(region);
                            transModify.AddNewlyCreatedDBObject(region, true);
                            transModify.Commit();
                        }
                        continue; 
                    }

                    current = points[nr];

                    if (last == -1.0) { last = double.Parse(points[nr]); }

                    //ed.WriteMessage($"\nROOF: {points[nr]}");

                    if (last == double.Parse(points[nr])) {

                        if (double.Parse(points[h2]) > double.Parse(points[h])) {

                            ptr.Add(new Point3d(double.Parse(points[x]), double.Parse(points[y]), double.Parse(points[h2])));

                        } else {

                            ptr.Add(new Point3d(double.Parse(points[x]), double.Parse(points[y]), double.Parse(points[h])));

                        }

                        if (i == (lines.Length - 1)) {


                            using (Transaction transModify = db.TransactionManager.StartTransaction()) {

                                BlockTableRecord btr = (BlockTableRecord)transModify.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                                Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d(Poly3dType.SimplePoly, ptr, true);
                                var curves = new DBObjectCollection();
                                curves.Add(pline3d);

                                var regions = Region.CreateFromCurves(curves);
                                var region = (Region)regions[0];

                                btr.AppendEntity(region);
                                transModify.AddNewlyCreatedDBObject(region, true);
                                transModify.Commit();
                            }
                            ptr = new Point3dCollection();

                        }

                    }  else {

                        using (Transaction transModify = db.TransactionManager.StartTransaction()) {

                            BlockTableRecord btr = (BlockTableRecord)transModify.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                            Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d(Poly3dType.SimplePoly, ptr, true);
                            var curves = new DBObjectCollection();
                            curves.Add(pline3d);

                            var regions = Region.CreateFromCurves(curves);
                            var region = (Region)regions[0];

                            btr.AppendEntity(region);
                            transModify.AddNewlyCreatedDBObject(region, true);
                            transModify.Commit();
                        }
                        ptr = new Point3dCollection();

                        if (double.Parse(points[h2]) > double.Parse(points[h])) {

                            ptr.Add(new Point3d(double.Parse(points[x]), double.Parse(points[y]), double.Parse(points[h2])));

                        } else {

                            ptr.Add(new Point3d(double.Parse(points[x]), double.Parse(points[y]), double.Parse(points[h])));

                        }
                    }

                    last = double.Parse(points[nr]);

                } catch (Exception ex) {

                    errors.Add(current);



                    ed.WriteMessage($"\n{ex.Message} ");

                }
            }

            if (errors.Count > 0) {
                ed.WriteMessage($"\nBudynki z błędną geometrią:\n");

                foreach (string err in errors) {
                    ed.WriteMessage($"{err}; ");
                }
            }





            ed.WriteMessage($"\nAdded {lp} buildings");
        }
    }
}
