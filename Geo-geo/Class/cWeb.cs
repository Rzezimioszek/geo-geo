using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Internal;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.Utilities;
using ProjNet.CoordinateSystems;

namespace Geo_geo.Class {
    internal class cWeb {

        public void GetPointBySelect(string portal = "geoportal") {

            string url;
            string url_geoportal = "https://mapy.geoportal.gov.pl/imap/Imgp_2.html?composition=default&bbox=";
            string url_streetview = "https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=";
            // string url_google = "https://www.google.pl/maps/@";
            string url_google = "https://www.google.pl/maps/place/";
            string url_emapa = "";
            string url_emapa_1 = "https://polska.e-mapa.net?";
            string url_emapa_2 = "&zoom=11&group=2,1000255&service=123,124,129,130,133,136,137,145,148,153,154,155,169,171,175,176,177,178,179,182,187,188,193,194,195,196,197,198,201,204,205,206,223,228,229,244,249,251,252,254,260,261,271,273,278,280,282,283,284,285,295,302,318,319,321,322,323,324,330&alllayers=123,126,128,129,130,131,Plany wektorowes133,Plany wektorowes133,Plany wektorowes133,Plany wektorowes133,Plany wektorowes133,134,136,137,139,140,141,143,144,145,147,148,149,150,151,152,153,154,155,156,159,161,162,167,169,170,171,172,176,177,178,182,204,205,206,210,211,213,221,222,223,224,225,226,231,232,233,234,235,236,237,238,239,240,241,242,244,246,248,249,250,252,254,256,257,258,259,261,263,264,265,266,267,268,269,271,274,282,283,284,285,293,295,296,297,298,299,300,302,308,309,311,312,313,314,315,316,318,319,320,321,322,323,324,326,327,328,330,331&layer=124001,124002,124003,127002,127003,127004,127005,127006,127007,127008,127009,1270010,132001,132002,132003,132004,133002,133008,146002,146003,146004,146005,173001,179001,180001,184006,184007,184009,185006,185007,185009,186006,186007,186009,187007,187008,187009,1870010,187021,187022,187023,187024,188007,188008,188009,1880010,188021,188022,188023,188024,189007,189008,189009,1890010,190009,1900010,190011,190012,190013,191005,193002,193005,193007,194001,194004,194006,1940010,196001,196004,196006,1960010,197005,201001,201003,201009,202001,209001,209002,209003,209004,209005,209006,212006,212008,212009,2120010,212011,228002,229003,247004,247005,247006,247037,247038,247039,247040,247041,247042,251004,276016,277002,278002,280001,286002,286003,304001,305001,306001";
            string url_osm = "https://www.openstreetmap.org/?m";
            string url_earth = "https://earth.google.com/web/@";
            string url_ump = "https://mapa.ump.waw.pl/ump-www/?zoom=19";
            string url_emapacloud = "https://cloud.e-mapa.net/?";
            string url_geoportal3d = "https://geoportal3d.pl/?flytime=0&bbox=";
            string url_geoportal2 = "https://polska.geoportal2.pl/map/www/mapa.php?CFGF=wms&mylayers=%20OSM%20wmts1:ORTOFOTOMAPA@EPSG:2180%20g1:dzialki%20g1:numery_dzialek%20g1:budynki&bbox=";
            string url_geoportal360 = "https://geoportal360.pl/map/#l:";
            string url_cgeo = "https://www.c-geoportal.pl/map?extent=";



            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            //PromptPointResult pPtRes = ed.GetPoint("\nWskaż punkt");

            PromptPointResult pPtRes = PinFormMap();


            if (pPtRes.Status != PromptStatus.OK) {
                ed.WriteMessage("\nNie wskazano punktu.");
                return;
            }

            Point3d ptStart = pPtRes.Value;


            CoordinateSystemFactory csFact = new CoordinateSystemFactory();
            ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();

            Coordinate point = new Coordinate(ptStart.X, ptStart.Y);

            string temp_x = $"{ptStart.X}";

            string zone_num = temp_x.Substring(0, 1);



            // rozpoznawanie strefy
            string zone = "";

            ed.WriteMessage($"Rozpoznano strefę {zone_num}");

            zone = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]";
            GeoAPI.CoordinateSystems.ICoordinateSystem epsg4326 = csFact.CreateFromWkt(zone);

            switch (zone_num) {
                case ("8"):
                    zone = "PROJCS[\"ETRF2000-PL_CS2000_24_Zone_8\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",8500000.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",24.0],PARAMETER[\"Scale_Factor\",0.999923],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
                    break;
                case ("7"):
                    zone = "PROJCS[\"ETRF2000-PL_CS2000_21_Zone_7\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",7500000.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",21.0],PARAMETER[\"Scale_Factor\",0.999923],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
                    break;
                case ("6"):
                    zone = "PROJCS[\"ETRF2000-PL_CS2000_18_Zone_6\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",6500000.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",18.0],PARAMETER[\"Scale_Factor\",0.999923],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
                    break;
                case ("5"):
                    zone = "PROJCS[\"ETRF2000-PL_CS2000_15_Zone_5\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",5500000.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",15.0],PARAMETER[\"Scale_Factor\",0.999923],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
                    break;
                default:
                    zone = "PROJCS[\"ETRF2000-PL_CS92\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0],PARAMETER[\"False_Northing\",-5300000.0],PARAMETER[\"Central_Meridian\",19.0],PARAMETER[\"Scale_Factor\",0.9993],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
                    ed.WriteMessage($"Błędna strefa, translacja do 92 pominięta");
                    break;
            }




            double[] d_point = new double[2];
            d_point[0] = Convert.ToDouble(point.X);
            d_point[1] = Convert.ToDouble(point.Y);

            GeoAPI.CoordinateSystems.ICoordinateSystem epsg217X = csFact.CreateFromWkt(zone);

            GeoAPI.CoordinateSystems.ICoordinateSystem epsg2180 = csFact.CreateFromWkt("PROJCS[\"ETRF2000-PL_CS92\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0],PARAMETER[\"False_Northing\",-5300000.0],PARAMETER[\"Central_Meridian\",19.0],PARAMETER[\"Scale_Factor\",0.9993],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]");
            GeoAPI.CoordinateSystems.Transformations.ICoordinateTransformation trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg2180);


            switch (portal) {

                case ("geoportal"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg2180);
                    double[] tpoints = trans.MathTransform.Transform(d_point);


                    url_geoportal = $"{url_geoportal}{tpoints[0]},{tpoints[1]},{tpoints[0]},{tpoints[1]}";

                    ed.WriteMessage($"\n\n{url_geoportal}");
                    System.Diagnostics.Process.Start(url_geoportal);
                    break;
                case ("streetview"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);


                    PromptPointResult nPtRes = ed.GetPoint("\nWskaż drugi punkt");

                    if (nPtRes.Status != PromptStatus.OK) {
                        ed.WriteMessage("\nNie wskazano punktu. Przeniesienie bez kierunku.");

                        url_streetview = $"{url_streetview}{tpoints[1]},{tpoints[0]}";
                    } else {

                        Point3d ptEnd = nPtRes.Value;

                        Line dummy = new Line(ptStart, ptEnd);
                        double angle = dummy.Angle;

                        angle = 90 - (angle * (180 / Math.PI));

                        if (angle < 0) {
                            angle = 360 + angle;
                        }


                        url_streetview = $"{url_streetview}{tpoints[1]},{tpoints[0]}&heading={angle}&pitch=10&fov=250";

                    }

                    ed.WriteMessage($"\n\n{url_streetview}");
                    System.Diagnostics.Process.Start(url_streetview);

                    break;
                case ("google"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);

                    // url_google = $"{url_google}{tpoints[1]},{tpoints[0]},20z"; <-- old url
                    url_google = $"{url_google}{tpoints[1]},{tpoints[0]}";
                    ed.WriteMessage($"\n\n{url_google}");
                    System.Diagnostics.Process.Start(url_google);

                    break;
                case ("earth"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);

                    url_earth = $"{url_earth}{tpoints[1]},{tpoints[0]},110a,300d,90y,0h,0t,0r/data=OgMKATA";
                    ed.WriteMessage($"\n\n{url_earth}");
                    System.Diagnostics.Process.Start(url_earth);

                    break;
                case ("emapa"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg2180);
                    tpoints = trans.MathTransform.Transform(d_point);

                    int x = Convert.ToInt32(tpoints[0]);
                    int y = Convert.ToInt32(tpoints[1]);

                    url_emapa = $"{url_emapa_1}x={x}&y={y}{url_emapa_2}";
                    ed.WriteMessage($"\n\n{url_emapa}");
                    System.Diagnostics.Process.Start(url_emapa);

                    break;
                case ("emapacloud"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);

                    url_emapacloud = $"{url_emapacloud}x={tpoints[1]}&y={tpoints[0]}&srid=4326";
                    ed.WriteMessage($"\n\n{url_emapacloud}");
                    System.Diagnostics.Process.Start(url_emapacloud);

                    break;

                case ("osm"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);

                    url_osm = $"{url_osm}lat={tpoints[1]}&mLon={tpoints[0]}#map=19/{tpoints[1]}/{tpoints[0]}";
                    ed.WriteMessage($"\n\n{url_osm}");
                    System.Diagnostics.Process.Start(url_osm);

                    break;
                case ("ump"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);

                    url_ump = $"{url_ump}&lat={tpoints[1]}&lon={tpoints[0]}";
                    ed.WriteMessage($"\n\n{url_ump}");
                    System.Diagnostics.Process.Start(url_ump);

                    break;

                case ("geoportal3d"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);

                    url_geoportal3d = $"{url_geoportal3d}{tpoints[1]},{tpoints[0]},{tpoints[1]},{tpoints[0]}";
                    ed.WriteMessage($"\n\n{url_geoportal3d}");
                    System.Diagnostics.Process.Start(url_geoportal3d);

                    break;

                case ("geoportal2"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg2180);
                    tpoints = trans.MathTransform.Transform(d_point);

                    url_geoportal2 = $"{url_geoportal2}{tpoints[0]},{tpoints[1]},{tpoints[0]},{tpoints[1]}&markery=0,{tpoints[0]},{tpoints[1]}";
                    ed.WriteMessage($"\n\n{url_geoportal2}");
                    System.Diagnostics.Process.Start(url_geoportal2);

                    break;
                case ("geoportal360"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);

                    url_geoportal360 = $"{url_geoportal360}{tpoints[1]},{tpoints[0]},19";
                    ed.WriteMessage($"\n\n{url_geoportal360}");
                    System.Diagnostics.Process.Start(url_geoportal360);

                    break;
                case ("yandex"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);

                    url = "https://yandex.eu/maps/?ll=";
                    url = $"{url}{tpoints[0]}%2C{tpoints[1]}&z=19\"";
                    ed.WriteMessage($"\n\n{url}");
                    System.Diagnostics.Process.Start(url);

                    break;

                case ("bing"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);

                    url = "https://www.bing.com/maps?cp=";
                    url = $"{url}{tpoints[1]}%7E{tpoints[0]}&lvl=19";
                    ed.WriteMessage($"\n\n{url}");
                    System.Diagnostics.Process.Start(url);

                    break;
                case ("bingeye"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
                    tpoints = trans.MathTransform.Transform(d_point);

                    PromptPointResult bPtRes = ed.GetPoint("\nWskaż drugi punkt");

                    url = "https://www.bing.com/maps?cp=";

                    if (bPtRes.Status != PromptStatus.OK) {
                        ed.WriteMessage("\nNie wskazano punktu. Przeniesienie bez kierunku.");

                        url = $"{url}{tpoints[1]}%7E{tpoints[0]}&style=x&lvl=19";
                    } else {

                        Point3d ptEnd = bPtRes.Value;

                        Line dummy = new Line(ptStart, ptEnd);
                        double angle = dummy.Angle;

                        angle = 90 - (angle * (180 / Math.PI));

                        if (angle < 0) {
                            angle = 360 + angle;
                        }


                        url = $"{url}{tpoints[1]}%7E{tpoints[0]}&style=x&lvl=19&dir={angle}";

                    }


                    ed.WriteMessage($"\n\n{url}");
                    System.Diagnostics.Process.Start(url);

                    break;

                case ("cgeo"):

                    trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg2180);
                    tpoints = trans.MathTransform.Transform(d_point);

                    tpoints[0] = tpoints[0] - 100.00 ;
                    tpoints[1] = tpoints[1] - 100.00;

                    url = $"{url_cgeo}{tpoints[0]},{tpoints[1]}";

                    tpoints[0] = tpoints[0] + 200.00;
                    tpoints[1] = tpoints[1] + 200.00;

                    url = $"{url},{tpoints[0]},{tpoints[1]}";

                    ed.WriteMessage($"\n\n{url}");
                    System.Diagnostics.Process.Start(url);
                    break;

            }



        }

        public Point3d SetEPSG(Point3d oldPoint, string epsg = "4326") {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            string zone = "";

            CoordinateSystemFactory csFact = new CoordinateSystemFactory();
            ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();

            string temp_x = $"{oldPoint.X}";
            string zone_num = temp_x.Substring(0, 1);

            zone = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]";
            GeoAPI.CoordinateSystems.ICoordinateSystem epsg4326 = csFact.CreateFromWkt(zone);

            switch (zone_num) {
                case ("8"):
                    zone = "PROJCS[\"ETRF2000-PL_CS2000_24_Zone_8\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",8500000.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",24.0],PARAMETER[\"Scale_Factor\",0.999923],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
                    break;
                case ("7"):
                    zone = "PROJCS[\"ETRF2000-PL_CS2000_21_Zone_7\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",7500000.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",21.0],PARAMETER[\"Scale_Factor\",0.999923],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
                    break;
                case ("6"):
                    zone = "PROJCS[\"ETRF2000-PL_CS2000_18_Zone_6\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",6500000.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",18.0],PARAMETER[\"Scale_Factor\",0.999923],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
                    break;
                case ("5"):
                    zone = "PROJCS[\"ETRF2000-PL_CS2000_15_Zone_5\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",5500000.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",15.0],PARAMETER[\"Scale_Factor\",0.999923],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
                    break;
                default:
                    zone = "PROJCS[\"ETRF2000-PL_CS92\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0],PARAMETER[\"False_Northing\",-5300000.0],PARAMETER[\"Central_Meridian\",19.0],PARAMETER[\"Scale_Factor\",0.9993],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]";
                    ed.WriteMessage($"Błędna strefa, translacja do 92 pominięta");
                    break;
            }

            GeoAPI.CoordinateSystems.ICoordinateSystem epsg217X = csFact.CreateFromWkt(zone);

            GeoAPI.CoordinateSystems.ICoordinateSystem epsg2180 = csFact.CreateFromWkt("PROJCS[\"ETRF2000-PL_CS92\",GEOGCS[\"GCS_ETRF2000-PL\",DATUM[\"ETRF2000_Poland\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000.0],PARAMETER[\"False_Northing\",-5300000.0],PARAMETER[\"Central_Meridian\",19.0],PARAMETER[\"Scale_Factor\",0.9993],PARAMETER[\"Latitude_Of_Origin\",0.0],UNIT[\"Meter\",1.0]]");
            GeoAPI.CoordinateSystems.Transformations.ICoordinateTransformation trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg2180);

            double[] d_point = new double[2];
            d_point[0] = Convert.ToDouble(oldPoint.X);
            d_point[1] = Convert.ToDouble(oldPoint.Y);

            trans = ctFact.CreateFromCoordinateSystems(epsg217X, epsg4326);
            double[] tpoints = trans.MathTransform.Transform(d_point);


            Point3d newPoint = new Point3d(tpoints[0], tpoints[1], 0.0);

            return newPoint;

        }


        public PromptPointResult PinFormMap() {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptPointResult pPtRes;

            Point3d temp = new Point3d(0, 0, 0);

            //PromptPointResult pPtRes = ed.GetPoint("\nWskaż punkt");

            string name = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string sourceFileName = $"{name}\\pin.dwg";

            try {

                using (Transaction tr = db.TransactionManager.StartTransaction()) {

                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                    ObjectId blockId = bt.Has("PIN (2D)") ? bt["PIN (2D)"] : ImportBlock("PIN (2D)", sourceFileName);


                    if (blockId != ObjectId.Null) {

                        BlockTableRecord space = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                        BlockReference blockRef = new BlockReference(Point3d.Origin, blockId);
                        ObjectId brId = space.AppendEntity(blockRef);
                        tr.AddNewlyCreatedDBObject(blockRef, true);

                        ObjectId[] ss = new ObjectId[1] { brId };
                        ed.SetImpliedSelection(ss);
                        PromptSelectionResult psr = ed.SelectImplied();

                        Point3d basePt = blockRef.Position;
                        DragCallback callback = (Point3d pt, ref Matrix3d xform) => {
                            if (pt.IsEqualTo(basePt))
                                return SamplerStatus.NoChange;
                            xform = Matrix3d.Displacement(basePt.GetVectorTo(pt));
                            return SamplerStatus.OK;
                        };
                        PromptPointResult ppr = ed.Drag(psr.Value, "\nWskaż punkt: ", callback);


                        pPtRes = ppr;

                        temp = pPtRes.Value;


                        blockRef.Erase();

                        return pPtRes;

                    }

                    tr.Commit();


                }
            } catch {

                pPtRes = ed.GetPoint("\nWskaż punkt");
                return pPtRes;

            }



            ed.WriteMessage($"\n{temp.X}, {temp.Y}\n");

            pPtRes = ed.GetPoint("\nWskaż punkt");
            return pPtRes;
        }

        private ObjectId ImportBlock(string blockName, string filenName) {

            if (!File.Exists(filenName))
                throw new FileNotFoundException("File not found", filenName);

            Database targetDb = HostApplicationServices.WorkingDatabase;
            ObjectId owner = SymbolUtilityServices.GetBlockModelSpaceId(targetDb);

            using (Database sourceDb = new Database(false, true)) {
                sourceDb.ReadDwgFile(filenName, FileShare.ReadWrite, false, "");
                using (Transaction tr = sourceDb.TransactionManager.StartTransaction()) {
                    BlockTable bt = (BlockTable)tr.GetObject(sourceDb.BlockTableId, OpenMode.ForRead);
                    if (!bt.Has(blockName))
                        throw new ArgumentException("Block definition not found", blockName);

                    ObjectIdCollection ids = new ObjectIdCollection();
                    ObjectId blockId = bt[blockName];
                    ids.Add(blockId);
                    IdMapping idMap = new IdMapping();
                    sourceDb.WblockCloneObjects(ids, owner, idMap, DuplicateRecordCloning.Ignore, false);

                    return idMap[blockId].Value;
                }
            }


        }
    }
}
