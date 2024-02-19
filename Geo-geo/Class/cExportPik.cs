using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo_geo.Class {
    internal class cExportPik {

        public void ExportPointsFromTxtForm(string pts_type, string pts_format, double skala, bool NrH, bool h0, string separator) {

            Document doc = Application.DocumentManager.MdiActiveDocument;
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


            TypedValue[] typedValueArray = new TypedValue[1];
            typedValueArray.SetValue((object)new TypedValue(0, (object)"TEXT"), 0);

            SelectionFilter selectionFilter = new SelectionFilter(typedValueArray);
            PromptSelectionResult selection = ed.GetSelection(selectionFilter);
            if (selection.Status != PromptStatus.OK)
                return;


            PromptSaveFileOptions saveOpts = new PromptSaveFileOptions("Wybierz plik do zapisu");
            saveOpts.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            PromptFileNameResult fileResult = ed.GetFileNameForSave(saveOpts);
            if (fileResult.Status != PromptStatus.OK) {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }
            fileName = fileResult.StringResult;

            ed.WriteMessage($"\nExported: {fileName} points");

            using (StreamWriter streamWriter = new StreamWriter(fileName)) {

                string format = "F3";

                //CultureInfo invariantCulture = CultureInfo.InvariantCulture;
                streamWriter.WriteLine("Nr\tX\tY\tZ");


                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    //BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead);


                    foreach (SelectedObject selectedObject in selection.Value) {
                        DBText dbText = tr.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as DBText;

                        double wspX = dbText.Position.X;
                        string x = wspX.ToString(format);

                        double wspY = dbText.Position.Y;
                        string y = wspY.ToString(format);

                        double wspZ = dbText.Position.Z;
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
            Document doc = Application.DocumentManager.MdiActiveDocument;
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


            TypedValue[] typedValueArray = new TypedValue[1];
            typedValueArray.SetValue((object)new TypedValue(0, (object)"TEXT"), 0);

            SelectionFilter selectionFilter = new SelectionFilter(typedValueArray);
            PromptSelectionResult selection = ed.GetSelection(selectionFilter);
            if (selection.Status != PromptStatus.OK)
                return;


            PromptSaveFileOptions saveOpts = new PromptSaveFileOptions("Wybierz plik do zapisu");
            saveOpts.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            PromptFileNameResult fileResult = ed.GetFileNameForSave(saveOpts);
            if (fileResult.Status != PromptStatus.OK) {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }
            fileName = fileResult.StringResult;

            ed.WriteMessage($"\nExported: {fileName} points");

            using (StreamWriter streamWriter = new StreamWriter(fileName)) {

                string format = "F2";

                // CultureInfo invariantCulture = CultureInfo.InvariantCulture;
                // streamWriter.WriteLine("Nr\tX\tY\tZ");


                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    //BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead);


                    foreach (SelectedObject selectedObject in selection.Value) {
                        DBText dbText = tr.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as DBText;

                        double wspX = dbText.Position.X;
                        string x = wspX.ToString(format);

                        double wspY = dbText.Position.Y;
                        string y = wspY.ToString(format);

                        double wspZ = dbText.Position.Z;
                        string z = wspZ.ToString(format);

                        double rotate = dbText.Rotation;

                        rotate = 360 - (rotate * (180 / Math.PI));

                        if (rotate >= 360.0) {

                            rotate = rotate - 360;

                        }
                        string r = rotate.ToString(format);


                        string tag = dbText.TextString.ToString();

                        string line = $"{y.Replace(".", ",")}\t{x.Replace(".", ",")}\t0,00\t{r.Replace(".", ",")}\t\"ţRTPW02\"";
                        streamWriter.WriteLine(line);

                        line = $"{y.Replace(".", ",")}\t{x.Replace(".", ",")}\t0,00\t{r.Replace(".", ",")}\t\"{tag}\"";
                        streamWriter.WriteLine(line);
                        j++;
                    }



                    tr.Commit();
                }





                ed.WriteMessage($"\nExported: {j} points");
            }
        }
    }
}
