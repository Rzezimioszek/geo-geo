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
using System.Runtime.InteropServices;
using Autodesk.AutoCAD.Internal;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Geo_geo
{
    public class acCommands
    {

        public static PaletteSet psc = null;
        public static PaletteSet psi = null;
        public static PaletteSet psiD = null;

        public double SumLenght = 0.0;

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

            string filename = "Z:\\Programy\\MAKRA\\geogeo\\instrukcja.pdf";

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filename = $"{name}\\instrukcja.pdf";

            System.Diagnostics.Process.Start(filename);

        }

        [CommandMethod("S_id")]
        public void cmdID() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect();

        }

        [CommandMethod("S_point_to_geoportal")]
        public void cmdGeoportal() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("geoportal");

        }

        [CommandMethod("S_point_to_streetview")]
        public void cmdStreetview() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("streetview");

        }

        [CommandMethod("S_point_to_google")]
        public void cmdGoogle() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("google");

        }

        [CommandMethod("S_point_to_earth")]
        public void cmdEarth() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("earth");

        }

        [CommandMethod("S_point_to_emapa")]
        public void cmdEmapa() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("emapa");

        }

        [CommandMethod("S_point_to_emapacloud")]
        public void cmdEmapacloud() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("emapacloud");

        }

        [CommandMethod("S_point_to_osm")]
        public void cmdOsm() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("osm");

        }

        [CommandMethod("S_point_to_ump")]
        public void cmdUmp() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("ump");

        }

        [CommandMethod("S_point_to_geoportal3d")]
        public void cmdGeoportal3d() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("geoportal3d");

        }

        [CommandMethod("S_point_to_geoportal2")]
        public void cmdGeoportal2() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("geoportal2");

        }

        [CommandMethod("S_point_to_geoportal360")]
        public void cmdGeoportal360() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("geoportal360");

        }

        [CommandMethod("S_point_to_yandex")]
        public void cmdYandex() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("yandex");

        }

        [CommandMethod("S_point_to_bing")]
        public void cmdBing() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("bing");

        }

        [CommandMethod("S_point_to_bingeye")]
        public void cmdBingEye() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("bingeye");

        }

        [CommandMethod("S_point_to_cgeo")]
        public void cmdCgeo() {

            cWeb cO = new Geo_geo.Class.cWeb();

            cO.GetPointBySelect("cgeo");

        }

        [CommandMethod("cid")]
        public void cmdCid() {

            CommandFlags flags = new CommandFlags();

            cSimple cO = new Geo_geo.Class.cSimple();

            cO.get_cid();

        }

        [CommandMethod("vxy")]
        public void cmdVid() {

            //CommandFlags flags = new CommandFlags();

            cSimple cO = new Geo_geo.Class.cSimple();

            cO.set_vid();

        }

        [CommandMethod("vyx")]
        public void cmdDiv() {

            //CommandFlags flags = new CommandFlags();

            cSimple cO = new Geo_geo.Class.cSimple();

            cO.set_vid(true);

        }

        [CommandMethod("hid")]
        public void cmdCidh() {

            //CommandFlags flags = new CommandFlags();

            cSimple cO = new Geo_geo.Class.cSimple();

            cO.get_cid("xyh");

        }

        [CommandMethod("dic")]
        public void cmdDic() {

            //CommandFlags flags = new CommandFlags();

            cSimple cO = new Geo_geo.Class.cSimple();

            cO.get_cid("yx");

        }

        [CommandMethod("dich")]
        public void cmdDich() {

            //CommandFlags flags = new CommandFlags();

            cSimple cO = new Geo_geo.Class.cSimple();

            cO.get_cid("yxh");

        }



        [CommandMethod("GGC")]
        public void cmdSTART() {


            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            Document doc = Application.DocumentManager.MdiActiveDocument;

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filename = $"{name}\\geogeo.cuix";

            string exe = $"_.cuiload \" {filename}";

            doc.SendStringToExecute(exe, false, false, false);

        }

        [CommandMethod("S_edycja_blokow")]
        public void cmdBlokEdit() {

            string filename = "Z:\\Programy\\MAKRA\\geogeo\\bloki.dwg";

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // String filename = "\\bloki.dwg";

            //string name = Directory.GetCurrentDirectory();
            //ed.WriteMessage($"1: {name}");

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filename = $"{name}\\bloki.dwg";

            System.Diagnostics.Process.Start(filename);

        }

        [CommandMethod("get_line_ids")]
        public void cmdGetIDs() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //ed.WriteMessage($"\nZaznacz obiekty, aby rozpocząć obrót.");

            cObrot cO = new Geo_geo.Class.cObrot();

            //cO.GetIds();



        }


        [CommandMethod("S_rotacja_etykiet", CommandFlags.UsePickSet)]
        public void cmdObrot() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage($"\nZaznacz obiekty, aby rozpocząć obrót.");

            cObrot cO = new Geo_geo.Class.cObrot();

            cO.RotateTextToLine2();



        }

        [CommandMethod("S_rotacja_etykiet_AI", CommandFlags.UsePickSet)]
        public void cmdObrotTest() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage($"\nZaznacz obiekty, aby rozpocząć obrót.");

            cObrot cO = new Geo_geo.Class.cObrot();

            cO.RotateTextToLine3();



        }


        [CommandMethod("S_rotacja_etykiet_alt", CommandFlags.UsePickSet)]
        public void cmdObrotAlt() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage($"\nZaznacz obiekty, aby rozpocząć obrót.");

            cObrot cO = new Geo_geo.Class.cObrot();

            cO.RotateTextToLine();



        }

        [CommandMethod("S_rzutowanie_etykiet", CommandFlags.UsePickSet)]
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

        [CommandMethod("S_markers")]
        public void cmdMarkers() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            try {
                if (psi == null) {
                    PaletteSet psi = new PaletteSet("Markers", "", new Guid("{51046AAA-47BD-4D03-9B82-1E245C0DD8AE}"));
                    psi.Add("Markers", new Class.FORMS.ucMarkery());


                    psi.Visible = true;
                    psi.MinimumSize = new System.Drawing.Size(423, 333);
                    psi.Size = new System.Drawing.Size(423, 333);
                }


            } catch (System.Exception ex) {

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


        [CommandMethod("S_StrikethroughText", CommandFlags.UsePickSet)]
        public void cmdStrikethroughText() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cOpisyMapy runner = new cOpisyMapy();
            runner.StrikethroughText();


        }


        [CommandMethod("S_StrikethroughTextOnPaper")]
        public void cmdStrikethroughTextOnPaper() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cOpisyMapy runner = new cOpisyMapy();
            runner.StrikethroughTextOnPaper();


        }

        [CommandMethod("S_mypoly")]
        public void cmdMyPoly() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cOther runner = new cOther();
            runner.MyPolyJig();


        }

        [CommandMethod("S_SimplifyGeometry", CommandFlags.UsePickSet)]
        public void cmdSimplifyGeometry() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cGeneralize simp = new cGeneralize();
            simp.SimplifyGeometry();


        }

        [CommandMethod("S_SimplifyGeometryPro", CommandFlags.UsePickSet)]
        public void cmdSimplifyGeometryPro() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cGeneralize simp = new cGeneralize();
            simp.SimplifyGeometry(0.001);

        }

        [CommandMethod("S_eksport_pkt", CommandFlags.UsePickSet)]
        public void cmdExportPikiet() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cExportPik exporter = new cExportPik();
            exporter.ExportPointsFromTxtForm();


        }

        [CommandMethod("S_eksport_mpkt", CommandFlags.UsePickSet)]
        public void cmdExportMPikiet() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cExportPik exporter = new cExportPik();
            exporter.ExportMText();


        }


        [CommandMethod("S_eksport_multipkt", CommandFlags.UsePickSet)]
        public void cmdExportMultiPikiet() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cExportPik exporter = new cExportPik();
            exporter.ExportMultiText();

        }

        [CommandMethod("S_eksport_pkt_kml", CommandFlags.UsePickSet)]
        public void cmdExportPikKML() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //ed.WriteMessage($"\nZaładowano pierwszą wersję oprogramowania");
            cExportPik exporter = new cExportPik();

            // string pts_type, string pts_format, double skala, bool NrH, bool h0, string separator
            exporter.ExportPointsFromTxtToKml();


        }

        [CommandMethod("S_eksport_all_kml", CommandFlags.UsePickSet)]
        public void cmdExportAllKML() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //ed.WriteMessage($"\nZaładowano pierwszą wersję oprogramowania");
            cExportPik exporter = new cExportPik();

            // string pts_type, string pts_format, double skala, bool NrH, bool h0, string separator
            exporter.ExportAllToKml();


        }

        [CommandMethod("S_eksport_blok", CommandFlags.UsePickSet)]
        public void cmdBlokExp() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBlocks runner = new cBlocks();
            runner.GetSelectedBlocks();

        }

        [CommandMethod("S_eksport_blok_extend", CommandFlags.UsePickSet)]
        public void cmdBlokExpExtend() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBlocks runner = new cBlocks();
            runner.GetSelectedBlocksExtend();

        }

        [CommandMethod("S_eksport_EWMAPA", CommandFlags.UsePickSet)]
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

        [CommandMethod("S_zapis_atr", CommandFlags.UsePickSet)]
        public void cmdBlokName() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBlocks runner = new cBlocks();
            runner.GetSelectedBlocksAtr();

        }


        [CommandMethod("S_zapis_atr_xy", CommandFlags.UsePickSet)]
        public void cmdBlokGet() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBlocks runner = new cBlocks();
            runner.GetSelectedBlocksAtrXY();

        }

        [CommandMethod("S_odczyt_atr")]
        public void cmdBlokSet() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBlocks runner = new cBlocks();
            runner.SetSelectedBlocksAtr();

        }



        [CommandMethod("S_czolowki2d", CommandFlags.UsePickSet)]
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
                // ps.Add("Bloki", new Class.FORMS.ucBlocks());
                ps.Add("Bloki", new Class.FORMS.ucBlockTable());


                ps.Visible = true;
                ps.MinimumSize = new System.Drawing.Size(243, 166);
                ps.Size = new System.Drawing.Size(243, 166);


            } catch (System.Exception ex) {

                ed.WriteMessage("\nError: " + ex.Message);

            }

        }

        [CommandMethod("S_l2t", CommandFlags.UsePickSet)]
        public void cmdLineToTxt() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\nStart: {0:HH:mm:ss.fff}", DateTime.Now);
            cRzutowanie runner = new cRzutowanie();
            runner.PlaceLineOnText();
            ed.WriteMessage("\nKoniec: {0:HH:mm:ss.fff}", DateTime.Now);

        }

        [CommandMethod("S_b2l", CommandFlags.UsePickSet)]
        public void cmdBlockToLine() {

            

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage("\nStart: {0:HH:mm:ss.fff}", DateTime.Now);
            cRzutowanie runner = new cRzutowanie();
            runner.PlaceBlockOnVertex();
            ed.WriteMessage("\nKoniec: {0:HH:mm:ss.fff}", DateTime.Now);

        }


        [CommandMethod("S_Bud")]
        public void cmdBud() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cBudynki runner = new cBudynki();
            runner.InsertBuilding();

        }

        [CommandMethod("S_Numeruj", CommandFlags.UsePickSet)]
        public void cmdNum() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cPikietowanie runner = new cPikietowanie();
            runner.putOnVertex();

        }

        [CommandMethod("S_MovePoligonIfPointInside", CommandFlags.UsePickSet)]
        public void cmdMovePoligon() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cPoligonBoundry boundry = new cPoligonBoundry();
            boundry.MovePoligonIfPointInside();

        }


        [CommandMethod("S_TEST", CommandFlags.UsePickSet)]
        public void cmdTestComand() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cTest c = new cTest();
            c.GetCoordinatesFromBlock();

        }

        [CommandMethod("S_TEST2", CommandFlags.UsePickSet)]
        public void cmdTestComand2() {


            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cTest c = new cTest();
            c.VieportFromBlock(false);

        }

        [CommandMethod("S_TEST3", CommandFlags.UsePickSet)]
        public void cmdTestComand3() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cTest c = new cTest();
            c.VieportFromBlock(true);

        }



        [CommandMethod("S_BlockToPoliline", CommandFlags.UsePickSet)]
        public void cmdBlockToPoliline() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cTest c = new cTest();
            c.VieportFromBlock(false);

        }

        [CommandMethod("S_BlockToViewport", CommandFlags.UsePickSet)]
        public void cmdBlockToViewport() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cLayout c = new cLayout();
            c.VieportFromModelspaceBlock();

        }

        [CommandMethod("S_PolyToViewport", CommandFlags.UsePickSet)]
        public void cmdPolyToViewport() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cLayout c = new cLayout();
            c.VieportFromPolyline();

        }

        [CommandMethod("S_GET_DIST_SUM")]
        public void cmdGetDistSum() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            try {
                if (psiD == null) {
                    PaletteSet psiD = new PaletteSet("Odleg", "", new Guid("{F278B70C-300E-409A-A94C-1DAD7C2F638F}"));
                    psiD.Add("Odleg", new Class.FORMS.ucDistance());


                    psiD.Visible = true;
                    psiD.MinimumSize = new System.Drawing.Size(150, 150);
                    psiD.Size = new System.Drawing.Size(230, 260);

                }

            } catch (System.Exception ex) {

                ed.WriteMessage("\nError: " + ex.Message);

            }

        }

        [CommandMethod("S_Zmierz")]
        public void cmdGetSum() {

            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;

            cSimple run = new cSimple();

            double value = run.get_distance();
            //ed.WriteMessage($"\n---\nOstatni:\n{value:0.00}");

            SumLenght = SumLenght + value;

            double result = SumLenght;
            ed.WriteMessage($"\nSuma:\n{result:0.000}");

            Clipboard.SetText($"{result:0.000}");

        }

        [CommandMethod("S_Reset")]
        public void cmdResetSumm() {

            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            SumLenght = 0.0;
            ed.WriteMessage($"\nSuma:\n0.000");


        }

        [CommandMethod("S_P3DtoPLW", CommandFlags.UsePickSet)]
        public void cmdPoliToPoliLW() {

            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;

            ed.WriteMessage($"\nKonwersja !!11!!");

            cKonwersja run = new cKonwersja();
            run.PolilineToPolilineLW();


        }



        [CommandMethod("S_GetGodlo", CommandFlags.UsePickSet)]
        public void cmdGetGodlo() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cOpisyMapy godlo = new cOpisyMapy();
            //godlo.AddLayerIfNotExists("_sekcje_500", 254);
            godlo.GridGodlo();

        }


        [CommandMethod("S_DeleteVertex", CommandFlags.UsePickSet)]
        public void cmd() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cSimple run = new cSimple();
            //godlo.AddLayerIfNotExists("_sekcje_500", 254);
            run.RemovePolilinesVertex();

        }

        [CommandMethod("S_DelByName", CommandFlags.UsePickSet)]
        public void cmdDelByName() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cSimple remover = new cSimple();

            remover.DeleteBlockByName("PIN2");
            

        }





        [CommandMethod("S_SetScale", CommandFlags.UsePickSet)]
        public void cmdSetScale() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cOpisyMapy cross = new cOpisyMapy();
            cross.SetDescScale();

        }

        [CommandMethod("S_CrossDesc", CommandFlags.UsePickSet)]
        public void cmdCrossDesc() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cOpisyMapy cross = new cOpisyMapy();
            cross.InsertCrossDescript();

        }

        [CommandMethod("S_CrossMesh", CommandFlags.UsePickSet)]
        public void cmdCrossMesh() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cOpisyMapy cross = new cOpisyMapy();
            cross.LoadCrossBySquare();

        }

        [CommandMethod("S_PointAtSurface", CommandFlags.UsePickSet)]
        public void cmdPointAtSurface() {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            cPointAtSurface runner = new cPointAtSurface();
            //ed.WriteMessage(Environment.MachineName);
            runner.getPointAtSurface();

        }

        [CommandMethod("get_entity_name", CommandFlags.UsePickSet)]
        public void cmdGetEntityName() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }


            SelectionSet selectionSet = selectionResult.Value;


            foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    using (Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity) {


                        ed.WriteMessage($"\n{entity.GetType().Name}");

                    }
                }
            }



        }



    }
}
