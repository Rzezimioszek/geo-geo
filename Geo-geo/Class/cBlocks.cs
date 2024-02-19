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

namespace Geo_geo.Class {
    internal class cBlocks {

        public void GetSelectedBlocksAtr() {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;


            string fileName;

            PromptSaveFileOptions saveOpts = new PromptSaveFileOptions("Wybierz plik do zapisu");
            saveOpts.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            PromptFileNameResult fileResult = ed.GetFileNameForSave(saveOpts);
            if (fileResult.Status != PromptStatus.OK) {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }

            TypedValue[] typedValueArray = new TypedValue[1];
            typedValueArray.SetValue((object)new TypedValue(0, (object)"TEXT"), 0);

            fileName = fileResult.StringResult;


            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            SelectionSet selectionSet = selectionResult.Value;

            /*
            SelectionFilter selectionFilter = new SelectionFilter(typedValueArray);
            PromptSelectionResult selection = ed.GetSelection(selectionFilter);
            if (selection.Status != PromptStatus.OK)
                return;
            */

            int lp = 0;

            using (StreamWriter streamWriter = new StreamWriter(fileName)) {

                foreach (SelectedObject selectedObject in selectionSet) {
                    using (Transaction trans = db.TransactionManager.StartTransaction()) {
                        Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity;
                        if (entity == null) {
                            continue;
                        }

                        //
                        //
                        //


                        string objType = entity.GetType().Name;
                        double wspXP = 0.0;
                        double wspYP = 0.0;
                        string atr;

                        string id = "";


                        if (objType == "BlockReference") {
                            BlockReference blockRef = entity as BlockReference;
                            wspXP = blockRef.Position.X;
                            wspYP = blockRef.Position.Y;

                            AttributeCollection attCol = blockRef.AttributeCollection;

                            string str = "";

                            str = "";
                            foreach (ObjectId attId in attCol) {

                                AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);

                                if (attRef.Tag == "ID") { id = attRef.TextString; }


                                //str = (str +  attRef.Tag + "\t");


                                str = (str + attRef.TextString + "\t");

                                /* Przypisanie wartości pola "Pochodzenie" na podstawie id 
                                if ((attRef.Tag == "POCHODZENIE") && (id == "E-W33")){
                                    AttributeReference attWrt = (AttributeReference)trans.GetObject(attId, OpenMode.ForWrite);
                                    attWrt.TextString = $"var: {id}";
                                }
                                */

                            }
                            streamWriter.WriteLine(str);
                            lp++;

                        } else {
                            continue;
                        }

                        ed.WriteMessage($"\n{lp}");

                        trans.Commit();
                    }
                }
            }
        }

        public void SetSelectedBlocksAtr() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;


            string fileName;
            string fileContent;
            string[] lines;


            PromptOpenFileOptions fileOpts = new PromptOpenFileOptions("Wybierz plik tekstowy: ");
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


            TypedValue[] typedValueArray = new TypedValue[1];
            typedValueArray.SetValue((object)new TypedValue(0, (object)"TEXT"), 0);

            fileName = fileResult.StringResult;


            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            SelectionSet selectionSet = selectionResult.Value;

            long lp = 0;
            long ip = 0;
            string[] line;


            foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity;
                    if (entity == null) {
                        continue;
                    }

                    //
                    //
                    //


                    string objType = entity.GetType().Name;
                    double wspXP = 0.0;
                    double wspYP = 0.0;
                    string atr;

                    string id = "";


                    if (objType == "BlockReference") {
                        BlockReference blockRef = entity as BlockReference;
                        wspXP = blockRef.Position.X;
                        wspYP = blockRef.Position.Y;

                        AttributeCollection attCol = blockRef.AttributeCollection;

                        string str = "";

                        str = "";
                        bool z = false;
                        foreach (ObjectId attId in attCol) {

                            AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);

                            lp++;

                            if (attRef.Tag == "ID") {
                                lp = 0;
                                for (long i = 0; i < lines.Length; i++) {
                                    line = lines[i].Split('\t');
                                    id = line[0];
                                    ip = i;
                                    z = false;
                                    if (attRef.TextString == id) {

                                        z = true; break;

                                    }
                                }
                            } else {

                                line = lines[ip].Split('\t');

                                if (lp >= line.Length) { continue; }

                                if (z) {

                                    str = line[lp];

                                    if (str == "skip") { continue; }

                                    AttributeReference attWrt = (AttributeReference)trans.GetObject(attId, OpenMode.ForWrite);
                                    attWrt.TextString = $"{str}";
                                }
                            }
                        }

                    } else {
                        continue;
                    }

                    ed.WriteMessage($"\n{lp}");

                    trans.Commit();
                }
            }
        }

        public void GetSelectedBlocks() {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;


            string fileName;

            PromptSaveFileOptions saveOpts = new PromptSaveFileOptions("Wybierz plik do zapisu");
            saveOpts.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            PromptFileNameResult fileResult = ed.GetFileNameForSave(saveOpts);
            if (fileResult.Status != PromptStatus.OK) {
                ed.WriteMessage("\nError opening the file or no file selected!");
                return;
            }

            TypedValue[] typedValueArray = new TypedValue[1];
            typedValueArray.SetValue((object)new TypedValue(0, (object)"TEXT"), 0);

            fileName = fileResult.StringResult;


            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            SelectionSet selectionSet = selectionResult.Value;

            /*
            SelectionFilter selectionFilter = new SelectionFilter(typedValueArray);
            PromptSelectionResult selection = ed.GetSelection(selectionFilter);
            if (selection.Status != PromptStatus.OK)
                return;
            */

            int lp = 0;

            using (StreamWriter streamWriter = new StreamWriter(fileName)) {

                foreach (SelectedObject selectedObject in selectionSet) {
                    using (Transaction trans = db.TransactionManager.StartTransaction()) {
                        Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity;
                        if (entity == null) {
                            continue;
                        }

                        //
                        //
                        //


                        string objType = entity.GetType().Name;
                        double wspXP = 0.0;
                        double wspYP = 0.0;
                        string atr;

                        string id = "";


                        if (objType == "BlockReference") {
                            BlockReference blockRef = entity as BlockReference;
                            wspXP = blockRef.Position.X;
                            wspYP = blockRef.Position.Y;

                            AttributeCollection attCol = blockRef.AttributeCollection;

                            string str = "";

                           
                            foreach (ObjectId attId in attCol) {

                                AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);

                                //if (attRef.Tag == "ID") { str = (str + attRef.TextString + "\t"); }


                                //str = (str +  attRef.Tag + "\t");


                                str = (str + attRef.TextString + "\t");
                                break;

                                /* Przypisanie wartości pola "Pochodzenie" na podstawie id 
                                if ((attRef.Tag == "POCHODZENIE") && (id == "E-W33")){
                                    AttributeReference attWrt = (AttributeReference)trans.GetObject(attId, OpenMode.ForWrite);
                                    attWrt.TextString = $"var: {id}";
                                }
                                */

                            }
                            str = str + $"{blockRef.Position.X.ToString()}\t{blockRef.Position.Y.ToString()}\t{blockRef.Position.Z.ToString()}\t";

                            streamWriter.WriteLine(str);
                            lp++;

                        } else {
                            continue;
                        }

                        ed.WriteMessage($"\n{lp}");

                        trans.Commit();
                    }
                }
            }
        }
        public void SetSelectedBlocksJust() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;


            string fileName;
            string fileContent;
            string[] lines;



            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            SelectionSet selectionSet = selectionResult.Value;

            long lp = 0;
            long ip = 0;
            string[] line;


            foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity;
                    if (entity == null) {
                        continue;
                    }


                    string objType = entity.GetType().Name;
                    double wspXP = 0.0;
                    double wspYP = 0.0;
                    string atr;

                    string id = "";


                    if (objType == "BlockReference") {
                        BlockReference blockRef = entity as BlockReference;
                        wspXP = blockRef.Position.X;
                        wspYP = blockRef.Position.Y;

                        AttributeCollection attCol = blockRef.AttributeCollection;

                        string str = "";

                        str = "";
                        bool z = false;
                        foreach (ObjectId attId in attCol) {

                            AttributeReference attRef = (AttributeReference)trans.GetObject(attId, OpenMode.ForRead);

                            lp++;
                            //attRef.Justify;


                            ed.WriteMessage($"\nIs: {attRef.IsMTextAttribute}, Justy:{attRef.Justify}");

                            double X0 = attRef.Position.X;
                            double Y0 = attRef.Position.Y;

                            Point3d newpos = new Point3d(X0, Y0, 0.0);

                            

                            ed.WriteMessage($"\nOLD: X={attRef.AlignmentPoint.X}, Y={attRef.AlignmentPoint.Y}");
                            ed.WriteMessage($"\nPOSOLD: X={attRef.Position.X}, Y={attRef.Position.Y}");


                            if (attRef.Justify == AttachmentPoint.BaseLeft) {

                                

                                AttributeReference attWrt = (AttributeReference)trans.GetObject(attId, OpenMode.ForWrite);
                                attWrt.Justify = AttachmentPoint.BaseRight;
                                try { 
                                    attWrt.AlignmentPoint = attWrt.AlignmentPoint.TransformBy(blockRef.BlockTransform); 
                                }
                                catch (Exception ex) { ed.WriteMessage(ex.ToString()); }


                            } else {
                                AttributeReference attWrt = (AttributeReference)trans.GetObject(attId, OpenMode.ForWrite);
                                attWrt.Justify = AttachmentPoint.BaseLeft;
                                try {
                                    attWrt.AlignmentPoint = attWrt.AlignmentPoint.TransformBy(blockRef.BlockTransform);
                                } catch (Exception ex) { ed.WriteMessage(ex.ToString()); }

                            }
                            
                        }

                    } else {
                        continue;
                    }

                    ed.WriteMessage($"\n{lp}");

                    trans.Commit();
                }
            }
        }
    }
}
