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
using static System.Net.Mime.MediaTypeNames;

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

                ed.WriteMessage($"\nLoad line: {i}");

                while (lines[i].Contains($"{sep}{sep}")) {
                    lines[i] = lines[i].Replace($"{sep}{sep}", $"{sep}");
                }

                lines[i] = lines[i].Replace(",", ".");

                //points = lines[i].Split(sep);

            }

            int lp = 0;
            int j = 1;

            for (int i = 0; i < (lines.Length - 1); i++) {

                int nr = 0;
                int x = 2;
                int y = 3;
                int h = 4;
                int h2 = 5;

                lp++;

                using (Transaction transModify = db.TransactionManager.StartTransaction()) {

                    BlockTableRecord btr = (BlockTableRecord)transModify.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                    j = i + 1;

                    points = lines[i].Split(sep);

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

            double last = 0.0;
            Point3dCollection ptr = new Point3dCollection();

            for (int i = 0; i < (lines.Length); i++) {

                int nr = 0;
                int x = 2;
                int y = 3;
                int h = 4;
                int h2 = 5;


                points = lines[i].Split(sep);

                if (last == double.Parse(points[nr])) {

                    if (double.Parse(points[h2]) > double.Parse(points[h])){

                        ptr.Add(new Point3d(double.Parse(points[x]), double.Parse(points[y]), double.Parse(points[h2])));

                    } else {

                        ptr.Add(new Point3d(double.Parse(points[x]), double.Parse(points[y]), double.Parse(points[h])));

                    }

                } else if (i == (lines.Length - 1)) {

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

                } else {

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

                last = double.Parse(points[nr]);
            }

            ed.WriteMessage($"Added {lp} buildings");
        }
    }
}
