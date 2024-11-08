using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices.Filters;
using Autodesk.AutoCAD.EditorInput;
using Geo_geo.Class.FORMS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo_geo.Class {
    internal class cFileDlg {

        //cFileDlg.SaveDlg()

        public string SaveDlg(string filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*", string defExtend = ".txt") {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string fileName = "";

            saveFileDialog.Filter = filter;
            saveFileDialog.Title = $"Zapisz do pliku {defExtend}";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK) {

                fileName = saveFileDialog.FileName;

                if (!fileName.EndsWith(defExtend)) {
                    fileName = fileName + defExtend;
                }
            } else {
                return "return";
            }

            return fileName;
        }

        public string OpenDlg(string filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*", string defExtend = ".txt") {

            string fileName = "";


            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Title = $"Otwórz plik {defExtend}";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    fileName = openFileDialog.FileName;
                }
                else {
                    return "return";
                }
            }

            return fileName;

        }

        public string VieportDialog(string defScale = "1",string defActive = "True") {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (fLayoutSelector frm = new fLayoutSelector(Cursor.Position.X, Cursor.Position.Y, defScale, defActive)) {

                // frm.SetDesktopLocation(Cursor.Position.X, Cursor.Position.Y);

                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    //ed.WriteMessage($"\n{frm.ReturnValue}");
                    return frm.ReturnValue;
                }



            }

            string result = "err";
            return result;
        }
    }
}
