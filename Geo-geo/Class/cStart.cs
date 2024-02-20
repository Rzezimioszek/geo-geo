using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;
using Geo_geo.Class;
using System.Drawing;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.IO;
using System.Reflection;
using System.Security.Policy;

namespace Geo_geo
{
    public class acCommands
    {

        public static PaletteSet psc = null;
        public static PaletteSet psi = null;

        [CommandMethod("LS_POMOCY")]
        public void cmdGeoGeo()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage($"\nDostępne komendy:");
            ed.WriteMessage($"\nS_rotacja_etykiet\t- obraca teksty do linii, nie działa poprawnie z blokami");
            ed.WriteMessage($"\nS_rzutowanie_etykiet\t- rzutuje pikiety na linie [w trakcie...]"); 
            ed.WriteMessage($"\nS_import_pkt\t- importuje obikety tekstowe do modelu");
            ed.WriteMessage($"\nS_eksport_pkt\t- eksportuje obiekty tekstowe do pliku txt");
            ed.WriteMessage($"\nS_eksport_blok\t- eksportuje bloki do pliku txt");
            ed.WriteMessage($"\nS_zapis_atr\t- zapisuje atrybuty z wybranych bloków do txt");
            ed.WriteMessage($"\nS_odczyt_atr\t- ładuje atrybuty z pliku txt do wybranych bloków");
            ed.WriteMessage($"\nS_eksport_EWMAPA\t- eksport piket z obrotem do ewmapa");
            ed.WriteMessage($"\nimport_pikiet\t- import tekstów bez formatki");


        }
        [CommandMethod("S_instrukcja_obsługi")]
        public void cmdPDF() {

            String filename = "Z:\\Programy\\MAKRA\\geogeo\\instrukcja.pdf";

            System.Diagnostics.Process.Start(filename);

        }

        [CommandMethod("S_edycja_blokow")]
        public void cmdBlokEdit() {

            String filename = "Z:\\Programy\\MAKRA\\geogeo\\bloki.dwg";

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // String filename = "\\bloki.dwg";

            //string name = Directory.GetCurrentDirectory();
            //ed.WriteMessage($"1: {name}");

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filename = $"{name}\\bloki.dwg";

            System.Diagnostics.Process.Start(filename);

        }


        [CommandMethod("S_rotacja_etykiet")]
        public void cmdObrot() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage($"\nZaznacz obiekty, aby rozpocząć obrót.");

            cObrot cO = new Geo_geo.Class.cObrot();

            cO.RotateTextToLine2();



        }


        [CommandMethod("S_rotacja_etykiet_alt")]
        public void cmdObrotAlt() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage($"\nZaznacz obiekty, aby rozpocząć obrót.");

            cObrot cO = new Geo_geo.Class.cObrot();

            cO.RotateTextToLine();



        }

        [CommandMethod("S_rzutowanie_etykiet")]
        public void cmdRzut() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage($"\nZaznacz obiekty, aby rozpocząć zrzutować etykiety.");

            cRzutowanie cO = new Geo_geo.Class.cRzutowanie();

            cO.PlaceTextOnLine();



        }


        [CommandMethod("import_pikiet")]
        public void cmdImportPikiet() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            cImportPik imp = new Geo_geo.Class.cImportPik();

            imp.LoadPointsFromTxt();


        }

        [CommandMethod("S_import_pkt")]
        public void cmdImportPikiet2() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //ed.WriteMessage($"\nZaładowano pierwszą wersję oprogramowania");

            try {
                if (psi == null) { 
                    PaletteSet psi = new PaletteSet("Import współrzędnych", "", new Guid("{554324AB-D8C4-4B16-B2EC-D3F8CE22606C}"));
                    psi.Add("Import", new Class.FORMS.ucMain());


                    psi.Visible = true;
                    psi.MinimumSize = new System.Drawing.Size(253, 270);
                    psi.Size = new System.Drawing.Size(250, 270);
                }


            }
            catch (System.Exception ex) 
            {

                ed.WriteMessage("\nError: " + ex.Message);

            }

        }

        [CommandMethod("S_czolowki_user_form")]
        public void cmdCzol_uc() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            try {

                //PaletteSet
                if (psc == null) {
                    psc = new PaletteSet("Czołówki", "", new Guid("{61D97B30-8943-4A7D-B4B8-7BA2E7D44299}"));
                    psc.Add("Czołówki", new Class.FORMS.ucCzolowki());


                    psc.Visible = true;
                    psc.MinimumSize = new System.Drawing.Size(200, 150);
                  
                    psc.Size = new System.Drawing.Size(200, 150);
                } else {
                    psc = null;

                    psc = new PaletteSet("Czołówki", "", new Guid("{61D97B30-8943-4A7D-B4B8-7BA2E7D44299}"));
                    psc.Add("Czołówki", new Class.FORMS.ucCzolowki());


                    psc.Visible = true;
                    psc.MinimumSize = new System.Drawing.Size(200, 150);
                    psc.Size = new System.Drawing.Size(200, 150);

                }


            } catch (System.Exception ex) {

                ed.WriteMessage("\nError: " + ex.Message);

            }

        }

        [CommandMethod("S_eksport_pkt")]
        public void cmdExportPikiet() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //ed.WriteMessage($"\nZaładowano pierwszą wersję oprogramowania");
            cExportPik exporter = new cExportPik();

            // string pts_type, string pts_format, double skala, bool NrH, bool h0, string separator
            exporter.ExportPointsFromTxtForm("", "", 0, true, true, "\t");


        }

        [CommandMethod("S_eksport_blok")]
        public void cmdBlokExp() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBlocks runner = new cBlocks();
            runner.GetSelectedBlocks();

        }

        [CommandMethod("S_eksport_EWMAPA")]
        public void cmdExportEWmapa() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //ed.WriteMessage($"\nZaładowano pierwszą wersję oprogramowania");
            cExportPik exporter = new cExportPik();

            // string pts_type, string pts_format, double skala, bool NrH, bool h0, string separator
            exporter.ExportPointsToEWmapa("", "", 0, true, true, "\t");


        }

        [CommandMethod("test_create_mesh")]
        public void cmdTest() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cTest runner = new cTest();

            //test.Create3DMesh();
            //test.CreatePolyfaceMesh();
            runner.AddPointAndSetPointStyle();

        }

        [CommandMethod("S_zapis_atr")]
        public void cmdBlokName() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBlocks runner = new cBlocks();
            runner.GetSelectedBlocksAtr();

        }

        [CommandMethod("S_odczyt_atr")]
        public void cmdBlokSet() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBlocks runner = new cBlocks();
            runner.SetSelectedBlocksAtr();

        }


        [CommandMethod("S_czolowki2d")]
        public void cmdCzol() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cCzolowki runner = new cCzolowki();
            runner.PlaceDistOnLine("F2", "-", "-", 1.0);

        }

        [CommandMethod("list_block")]
        public void cmdTestBlockInsert() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //ed.WriteMessage($"\nZaładowano pierwszą wersję oprogramowania");

            try {

                PaletteSet ps = new PaletteSet("Bloki", "", new Guid("{F972C2DB-67CA-4D34-95C0-F2FE64D53767}"));
                ps.Add("Bloki", new Class.FORMS.ucBlocks());


                ps.Visible = true;
                ps.MinimumSize = new System.Drawing.Size(243, 166);
                ps.Size = new System.Drawing.Size(243, 166);


            } catch (System.Exception ex) {

                ed.WriteMessage("\nError: " + ex.Message);

            }

        }

        [CommandMethod("S_l2t")]
        public void cmdLineToTxt() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cRzutowanie runner = new cRzutowanie();
            runner.PlaceLineOnText();

        }


        [CommandMethod("S_Bud")]
        public void cmdBud() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBudynki runner = new cBudynki();
            runner.InsertBuilding();

        }

        [CommandMethod("S_Numeruj")]
        public void cmdNum() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cPikietowanie runner = new cPikietowanie();
            runner.putOnVertex();

        }


        [CommandMethod("S_TEST")]
        public void cmdTestComand() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cTest runner = new cTest();
            runner.ClosePointTest();

        }

        [CommandMethod("S_TEST2")]
        public void cmdTestComand2() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBlocks runner = new cBlocks();
            runner.SetSelectedBlocksJust();

        }

        [CommandMethod("S_TEST3")]
        public void cmdTestComand3() {

            

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cTest runner = new cTest();
            ed.WriteMessage(Environment.MachineName);
            runner.ImportBlocks();

            Point3d point = new Point3d(0,0,0);
            runner.AddBlockTest(point);

        }



    }
}
