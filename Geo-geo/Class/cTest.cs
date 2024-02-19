using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace Geo_geo.Class {
    internal class cTest {
        public void Create3DMesh() {

            // Get the current document and database, and start a transaction
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction()) {
                // Open the Block table record for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                // Create a polygon mesh
                using (PolygonMesh acPolyMesh = new PolygonMesh()) {
                    acPolyMesh.MSize = 4;
                    acPolyMesh.NSize = 4;

                    // Add the new object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(acPolyMesh);
                    acTrans.AddNewlyCreatedDBObject(acPolyMesh, true);

                    // Before adding vertexes, the polyline must be in the drawing
                    Point3dCollection acPts3dPMesh = new Point3dCollection();
                    acPts3dPMesh.Add(new Point3d(0, 0, 1));
                    acPts3dPMesh.Add(new Point3d(2, 0, 2));
                    acPts3dPMesh.Add(new Point3d(4, 0, 3));
                    acPts3dPMesh.Add(new Point3d(6, 0, 4));

                    acPts3dPMesh.Add(new Point3d(-1, 2, 4));
                    acPts3dPMesh.Add(new Point3d(1, 2, 3));
                    acPts3dPMesh.Add(new Point3d(3, 2, 2));
                    acPts3dPMesh.Add(new Point3d(5, 2, 1));

                    acPts3dPMesh.Add(new Point3d(0, 4, 1));
                    acPts3dPMesh.Add(new Point3d(2, 4, 2));
                    acPts3dPMesh.Add(new Point3d(4, 4, 3));
                    acPts3dPMesh.Add(new Point3d(6, 4, 4));

                    acPts3dPMesh.Add(new Point3d(-1, 6, 3));
                    acPts3dPMesh.Add(new Point3d(1, 6, 1));
                    acPts3dPMesh.Add(new Point3d(3, 6, 2));
                    acPts3dPMesh.Add(new Point3d(5, 6, 4));

                    foreach (Point3d acPt3d in acPts3dPMesh) {
                        PolygonMeshVertex acPMeshVer = new PolygonMeshVertex(acPt3d);
                        acPolyMesh.AppendVertex(acPMeshVer);
                        acTrans.AddNewlyCreatedDBObject(acPMeshVer, true);
                    }
                }

                // Open the active viewport
                ViewportTableRecord acVportTblRec;
                acVportTblRec = acTrans.GetObject(acDoc.Editor.ActiveViewportId,
                                                    OpenMode.ForWrite) as ViewportTableRecord;

                // Rotate the view direction of the current viewport
                acVportTblRec.ViewDirection = new Vector3d(-1, -1, 1);
                acDoc.Editor.UpdateTiledViewportsFromDatabase();

                // Save the new objects to the database
                acTrans.Commit();
            }
        }
        public void CreatePolyfaceMesh() {
            // Get the current document and database, and start a transaction
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction()) {
                // Open the Block table record for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                // Create a polyface mesh
                using (PolyFaceMesh acPFaceMesh = new PolyFaceMesh()) {
                    // Add the new object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(acPFaceMesh);
                    acTrans.AddNewlyCreatedDBObject(acPFaceMesh, true);

                    // Before adding vertexes, the polyline must be in the drawing
                    Point3dCollection acPts3dPFMesh = new Point3dCollection();
                    acPts3dPFMesh.Add(new Point3d(4, 7, 0));
                    acPts3dPFMesh.Add(new Point3d(5, 7, 0));
                    acPts3dPFMesh.Add(new Point3d(6, 7, 0));

                    acPts3dPFMesh.Add(new Point3d(4, 6, 0));
                    acPts3dPFMesh.Add(new Point3d(5, 6, 0));
                    acPts3dPFMesh.Add(new Point3d(6, 6, 1));

                    foreach (Point3d acPt3d in acPts3dPFMesh) {
                        PolyFaceMeshVertex acPMeshVer = new PolyFaceMeshVertex(acPt3d);
                        acPFaceMesh.AppendVertex(acPMeshVer);
                        acTrans.AddNewlyCreatedDBObject(acPMeshVer, true);
                    }

                    using (FaceRecord acFaceRec1 = new FaceRecord(1, 2, 5, 4)) {
                        acPFaceMesh.AppendFaceRecord(acFaceRec1);
                        acTrans.AddNewlyCreatedDBObject(acFaceRec1, true);
                    }

                    using (FaceRecord acFaceRec2 = new FaceRecord(2, 3, 6, 5)) {
                        acPFaceMesh.AppendFaceRecord(acFaceRec2);
                        acTrans.AddNewlyCreatedDBObject(acFaceRec2, true);
                    }
                }

                // Open the active viewport
                ViewportTableRecord acVportTblRec;
                acVportTblRec = acTrans.GetObject(acDoc.Editor.ActiveViewportId,
                                                    OpenMode.ForWrite) as ViewportTableRecord;

                // Rotate the view direction of the current viewport
                acVportTblRec.ViewDirection = new Vector3d(-1, -1, 1);
                acDoc.Editor.UpdateTiledViewportsFromDatabase();

                // Save the new objects to the database
                acTrans.Commit();
            }
        }
        public void AddPointAndSetPointStyle() {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction()) {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForWrite) as BlockTableRecord;

                // Create a point at (4, 3, 0) in Model space
                using (DBPoint acPoint = new DBPoint(new Point3d(4, 3, 0))) {
                    // Add the new object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(acPoint);
                    acTrans.AddNewlyCreatedDBObject(acPoint, true);
                }

                // Set the style for all point objects in the drawing
                acCurDb.Pdmode = 34;
                acCurDb.Pdsize = 1;

                // Save the new object to the database
                acTrans.Commit();

            }
        }

        public void ListBlockReferences() {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            List<string> blocks = new List<string>();

            using (Transaction trans = db.TransactionManager.StartTransaction()) {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                foreach (ObjectId btrId in bt) {
                    BlockTableRecord btr = trans.GetObject(btrId, OpenMode.ForRead) as BlockTableRecord;

                    if (btr.IsLayout || btr.IsAnonymous || btr.IsFromExternalReference)
                        continue;

                    blocks.Add(btr.Name.ToString());

                }

                foreach (string bl in blocks) {
                    ed.WriteMessage($"Block: {bl}\n");
                }


            }

            
        }

        public void ImportBlocks() {

            DocumentCollection dm = Application.DocumentManager;

            Editor ed = dm.MdiActiveDocument.Editor;

            Database destDb = dm.MdiActiveDocument.Database;

            Database sourceDb = new Database(false, true);

            String sourceFileName;

            try {

                // Get name of DWG from which to copy blocks

                sourceFileName = "Z:\\Programy\\MAKRA\\geogeo\\bloki.dwg";

                // Read the DWG into a side database

                sourceDb.ReadDwgFile(sourceFileName, System.IO.FileShare.Read, true, "");


                // Create a variable to store the list of block identifiers

                ObjectIdCollection blockIds = new ObjectIdCollection();


                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm =

                  sourceDb.TransactionManager;


                using (Transaction myT = tm.StartTransaction()) {

                    // Open the block table

                    BlockTable bt =

                        (BlockTable)tm.GetObject(sourceDb.BlockTableId,

                                                OpenMode.ForRead,

                                                false);


                    // Check each block in the block table

                    foreach (ObjectId btrId in bt) {

                        BlockTableRecord btr =

                          (BlockTableRecord)tm.GetObject(btrId,

                                                        OpenMode.ForRead,

                                                        false);

                        // Only add named & non-layout blocks to the copy list

                        if (!btr.IsAnonymous && !btr.IsLayout)

                            blockIds.Add(btrId);

                        btr.Dispose();

                    }

                }

                // Copy blocks from source to destination database

                IdMapping mapping = new IdMapping();

                sourceDb.WblockCloneObjects(blockIds,

                                            destDb.BlockTableId,

                                            mapping,

                                            DuplicateRecordCloning.Replace,

                                            false);

                ed.WriteMessage("\nCopied "

                                + blockIds.Count.ToString()

                                + " block definitions from "

                                + sourceFileName

                                + " to the current drawing.");

            } catch (Autodesk.AutoCAD.Runtime.Exception ex) {

                ed.WriteMessage("\nError during copy: " + ex.Message);

            }

            sourceDb.Dispose();

        }

        public void AddBlockTest(Point3d point, string blockName = "pikieta_test") {

            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            using (Transaction myT = db.TransactionManager.StartTransaction()) {

                BlockTable bt = db.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;

                BlockTableRecord blockDef = bt[blockName].GetObject(OpenMode.ForRead) as BlockTableRecord;

                //Also open modelspace - we'll be adding our BlockReference to it

                BlockTableRecord ms = bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite) as BlockTableRecord;

                //Create new BlockReference, and link it to our block definition

                using (BlockReference blockRef = new BlockReference(point, blockDef.ObjectId)) {

                    //Add the block reference to modelspace

                    ms.AppendEntity(blockRef);

                    myT.AddNewlyCreatedDBObject(blockRef, true);

                    //Iterate block definition to find all non-constant

                    // AttributeDefinitions

                    foreach (ObjectId id in blockDef) {

                        DBObject obj = id.GetObject(OpenMode.ForRead);

                        AttributeDefinition attDef = obj as AttributeDefinition;

                        if ((attDef != null) && (!attDef.Constant)) {

                            //This is a non-constant AttributeDefinition

                            //Create a new AttributeReference

                            using (AttributeReference attRef = new AttributeReference()) {

                                attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);

                                attRef.TextString = "Hello World";

                                //Add the AttributeReference to the BlockReference

                                blockRef.AttributeCollection.AppendAttribute(attRef);

                                myT.AddNewlyCreatedDBObject(attRef, true);

                            }

                        }

                    }

                }

                myT.Commit();

            }

        }

        public void ClosePointTest() {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            double prec = 0.05;
            bool on_line = false;

            PromptSelectionResult selectionResult = ed.GetSelection();
            if (selectionResult.Status != PromptStatus.OK) {
                ed.WriteMessage("Nie wybrano elementów.");
                return;
            }

            SelectionSet selectionSet = selectionResult.Value;
            int liczba_porzadkowa = 0;

            foreach (SelectedObject selectedObject in selectionSet) {
                using (Transaction trans = db.TransactionManager.StartTransaction()) {
                    Entity entity = trans.GetObject(selectedObject.ObjectId, OpenMode.ForRead) as Entity;
                    if (entity == null) {
                        continue;
                    }

                    string objType = entity.GetType().Name;
                    double wspXP = 0.0;
                    double wspYP = 0.0;
                    double orignal_rotation = 0;


                    if (objType == "BlockReference") {
                        BlockReference blockRef = entity as BlockReference;
                        wspXP = blockRef.Position.X;
                        wspYP = blockRef.Position.Y;
                        orignal_rotation = blockRef.Rotation;
                    } else if (objType == "DBText") {
                        DBText textObj = entity as DBText;
                        wspXP = textObj.Position.X;
                        wspYP = textObj.Position.Y;
                        orignal_rotation = textObj.Rotation;
                    } else if (objType == "MText") {
                        MText textMObj = entity as MText;
                        wspXP = textMObj.Location.X;
                        wspXP = textMObj.Location.Y;
                        orignal_rotation = textMObj.Rotation;
                    } else {
                        continue;
                    }
                    ed.WriteMessage(objType + $"\n");


                    foreach (SelectedObject lineSelectedObject in selectionSet) {
                        using (Entity lineEntity = trans.GetObject(lineSelectedObject.ObjectId, OpenMode.ForRead) as Entity) {

                            ed.WriteMessage(lineEntity.GetType().Name + $"\n");

                            Line line = new Line();

                            Autodesk.AutoCAD.DatabaseServices.Polyline pline = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                            Autodesk.AutoCAD.DatabaseServices.Polyline3d pline3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                            Autodesk.AutoCAD.DatabaseServices.Polyline2d pline2d = new Autodesk.AutoCAD.DatabaseServices.Polyline2d();

                            double angle = 0.0;

                            double wspX0 = 0.0;
                            double wspY0 = 0.0;
                            double wspX1 = 0.0;
                            double wspY1 = 0.0;

                            double deltaX01 = 0.0;
                            double deltaY01 = 0.0;


                            if (lineEntity == null) { continue; }

                            if (lineEntity.GetType().Name == "Line") {

                                line = lineEntity as Line;
                                angle = line.Angle;

                                Point3d startPoint = new Point3d(line.StartPoint.X, line.StartPoint.Y, 0.0);
                                Point3d endPoint = new Point3d(line.EndPoint.X, line.EndPoint.Y, 0.0);
                                Line dummy = new Line(startPoint, endPoint);
                                Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                if (endPoint == objPts) {  continue; }

                                if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                Point3d vec = new Point3d(dummy.GetClosestPointTo(objPts, true).X, dummy.GetClosestPointTo(objPts, true).Y, 0.0);
                                on_line = vec.DistanceTo(objPts) < prec;

                                if (on_line && (orignal_rotation == 0)) {
                                    angle = ValidAngle(angle);

                                    InserRotation(db, doc, entity, objType, angle);

                                    liczba_porzadkowa++;
                                    //ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} -> {lineEntity.GetType().Name}");
                                }

                            } else if (lineEntity.GetType().Name == "Polyline") {

                                pline = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline;

                                for (int jk = 0; jk < (pline.NumberOfVertices - 1); jk++) {

                                    wspX0 = Math.Round(pline.GetPoint2dAt(jk).X, 3);
                                    wspY0 = Math.Round(pline.GetPoint2dAt(jk).Y, 3);
                                    wspX1 = Math.Round(pline.GetPoint2dAt(jk + 1).X, 3);
                                    wspY1 = Math.Round(pline.GetPoint2dAt(jk + 1).Y, 3);

                                    deltaX01 = wspX1 - wspX0;
                                    deltaY01 = wspY1 - wspY0;

                                    if (deltaX01 == 0 && deltaY01 == 0) {

                                        angle = 0;

                                    } else {
                                        angle = Math.Atan2(deltaY01, deltaX01);
                                    }

                                    Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                                    Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                                    Line dummy = new Line(startPoint, endPoint);
                                    Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                    if (endPoint == objPts) { continue; }

                                    if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                    Point3d vec = new Point3d(dummy.GetClosestPointTo(objPts, true).X, dummy.GetClosestPointTo(objPts, true).Y, 0.0);
                                    on_line = vec.DistanceTo(objPts) < prec;

                                    if (on_line && (orignal_rotation == 0)) {

                                        angle = ValidAngle(angle);

                                        InserRotation(db, doc, entity, objType, angle);

                                        liczba_porzadkowa++;
                                        //ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} -> {lineEntity.GetType().Name}");
                                    }
                                }
                            } else if (lineEntity.GetType().Name == "Polyline3d") {



                                //Polyline
                                pline3d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline3d;


                                int counter_of_vertex = 0;

                                List<double> wspX = new List<double>();
                                List<double> wspY = new List<double>();
                                List<double> wspZ = new List<double>();

                                if (pline3d != null) {

                                    foreach (ObjectId vId in pline3d) {

                                        PolylineVertex3d v3d = (PolylineVertex3d)trans.GetObject(vId, OpenMode.ForRead);
                                        counter_of_vertex++;

                                        wspX.Add(v3d.Position.X);
                                        wspY.Add(v3d.Position.Y);
                                        wspZ.Add(v3d.Position.Z);

                                    }

                                    for (int jk = 0; jk < (counter_of_vertex - 1); jk++) {

                                        wspX0 = Math.Round(wspX[jk], 3);
                                        wspY0 = Math.Round(wspY[jk], 3);
                                        //wspZ0 = Math.Round(wspZ[jk], 3);

                                        wspX1 = Math.Round(wspX[jk + 1], 3);
                                        wspY1 = Math.Round(wspY[jk + 1], 3);
                                        //wspZ1 = Math.Round(wspZ[jk + 1], 3);

                                        deltaX01 = wspX1 - wspX0;
                                        deltaY01 = wspY1 - wspY0;

                                        if (deltaX01 == 0 && deltaY01 == 0) {

                                            angle = 0;

                                        } else {
                                            angle = Math.Atan2(deltaY01, deltaX01);
                                        }

                                        Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                                        Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                                        Line dummy = new Line(startPoint, endPoint);
                                        Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                        if (endPoint == objPts) { continue; }

                                        if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                        Point3d vec = new Point3d(dummy.GetClosestPointTo(objPts, true).X, dummy.GetClosestPointTo(objPts, true).Y, 0.0);
                                        on_line = vec.DistanceTo(objPts) < prec;

                                        if (on_line && (orignal_rotation == 0)) {

                                            angle = ValidAngle(angle);

                                            InserRotation(db, doc, entity, objType, angle);

                                            liczba_porzadkowa++;
                                            //ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} -> {lineEntity.GetType().Name}");
                                        }   
                                    }
                                }
                            } else if (lineEntity.GetType().Name == "Polyline2d") {

                                pline2d = lineEntity as Autodesk.AutoCAD.DatabaseServices.Polyline2d;

                                int counter_of_vertex = 0;

                                List<double> wspX = new List<double>();
                                List<double> wspY = new List<double>();
                                List<double> wspZ = new List<double>();

                                if (pline2d != null) {

                                    foreach (ObjectId vId in pline2d) {

                                        Vertex2d v2d = (Vertex2d)trans.GetObject(vId, OpenMode.ForRead);
                                        counter_of_vertex++;

                                        wspX.Add(v2d.Position.X);
                                        wspY.Add(v2d.Position.Y);
                                        wspZ.Add(0.0);

                                    }

                                    for (int jk = 0; jk < (counter_of_vertex - 1); jk++) {

                                        wspX0 = Math.Round(wspX[jk], 3);
                                        wspY0 = Math.Round(wspY[jk], 3);
                                        //wspZ0 = Math.Round(wspZ[jk], 3);

                                        wspX1 = Math.Round(wspX[jk + 1], 3);
                                        wspY1 = Math.Round(wspY[jk + 1], 3);
                                        //wspZ1 = Math.Round(wspZ[jk + 1], 3);

                                        deltaX01 = wspX1 - wspX0;
                                        deltaY01 = wspY1 - wspY0;


                                        if (deltaX01 == 0 && deltaY01 == 0) {

                                            angle = 0;

                                        } else {
                                            angle = Math.Atan2(deltaY01, deltaX01);
                                        }

                                        Point3d startPoint = new Point3d(wspX0, wspY0, 0.0);
                                        Point3d endPoint = new Point3d(wspX1, wspY1, 0.0);
                                        Line dummy = new Line(startPoint, endPoint);
                                        Point3d objPts = new Point3d(wspXP, wspYP, 0.0);

                                        if (endPoint == objPts) { continue; }

                                        if (!InBox(startPoint, endPoint, objPts, prec)) { continue; }

                                        Point3d vec = new Point3d(dummy.GetClosestPointTo(objPts, true).X, dummy.GetClosestPointTo(objPts, true).Y, 0.0);
                                        on_line = vec.DistanceTo(objPts) < prec;

                                        if (on_line && (orignal_rotation == 0)) {

                                            angle = ValidAngle(angle);

                                            InserRotation(db, doc, entity, objType, angle);

                                            liczba_porzadkowa++;
                                            //ed.WriteMessage($"{liczba_porzadkowa} {entity.GetType().Name} -> {lineEntity.GetType().Name}");
                                        }
                                    }
                                }
                            } else {
                                continue;
                                // lineEntity.GetType().Name != "Polyline"
                            }
                        }
                    }
                    trans.Commit();
                }
            }
        }

        private void InserRotation(Database db, Document doc, Entity entity, string objType, Double angle) {

            using (Transaction transModify = db.TransactionManager.StartTransaction()) {


                if (objType == "BlockReference") {
                    BlockReference blockRef = entity as BlockReference;
                    blockRef.UpgradeOpen();

                    Matrix3d curUCSMatrix = doc.Editor.CurrentUserCoordinateSystem;
                    CoordinateSystem3d curUCS = curUCSMatrix.CoordinateSystem3d;

                    if (blockRef.Rotation == 0) {
                        blockRef.TransformBy(Matrix3d.Rotation(angle, curUCS.Zaxis, blockRef.Position));
                    }

                    transModify.Commit();
                } else if (objType == "DBText") {
                    DBText textObj = entity as DBText;
                    textObj.UpgradeOpen();
                    textObj.Rotation = angle;
                    transModify.Commit();
                    // ed.WriteMessage($"Third check\n");
                } else if (objType == "MText") {
                    MText textMObj = entity as MText;
                    textMObj.UpgradeOpen();
                    textMObj.Rotation = angle;
                    transModify.Commit();
                }
            }

        }

        private bool InBox(Point3d sP3D, Point3d eP3D, Point3d pP3D, double prec) {

            if (sP3D.X > eP3D.X) {
                if (pP3D.X > (sP3D.X + prec)) {
                    return false;
                } else if (pP3D.X < (eP3D.X - prec)) {
                    return false;
                }
            } else {
                if (pP3D.X > (eP3D.X + prec)) {
                    return false;
                } else if (pP3D.X < (sP3D.X - prec)) {
                    return false;
                }
            }

            if (sP3D.Y > eP3D.Y) {
                if (pP3D.Y > (sP3D.Y + prec)) {
                    return false;
                } else if (pP3D.Y < (eP3D.Y - prec)) {
                    return false;
                }
            } else {
                if (pP3D.Y > (eP3D.Y + prec)) {
                    return false;
                } else if (pP3D.Y < (sP3D.Y - prec)) {
                    return false;
                }
            }
            return true;
        }

        private bool ChangeRotation(double wspP, double wsp0, double wsp1, double diff) {
            bool change = true;


            if ((Math.Round(wspP, 2) < (Math.Round(wsp0, 2) - Math.Round(diff, 2))) && (Math.Round(wspP, 2) < (Math.Round(wsp1, 2) - Math.Round(diff, 2)))) {
                change = false;
            }
            if ((Math.Round(wspP, 2) > (Math.Round(wsp0, 2) + Math.Round(diff, 2))) && (Math.Round(wspP, 2) > (Math.Round(wsp1, 2) + Math.Round(diff, 2)))) {
                change = false;
            }

            return change;
        }

        private bool IsOnLine(double wspX0, double wspY0, double wspX1, double wspY1, double prec) {
            bool on_line = false;

            if (
            (Math.Round(wspX0, 2) <= (Math.Round(wspX1, 2) + prec)) &&
            (Math.Round(wspX0, 2) >= (Math.Round(wspX1, 2) - prec)) &&
            (Math.Round(wspY0, 2) <= (Math.Round(wspY1, 2) + prec)) &&
            (Math.Round(wspY0, 2) >= (Math.Round(wspY1, 2) - prec))
            ) {

                on_line = true;

            }




            return on_line;
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
                       
                            //ed.WriteMessage($"\nBLOCK TRANS={blockRef.BlockTransform}{blockRef.Position}");

                            //ed.WriteMessage($"\nAP={attRef.AlignmentPoint}");
                            //ed.WriteMessage($"\nPOS={attRef.Position}");



                            if (attRef.AlignmentPoint.X == 0) {

                                ed.WriteMessage($"\n000");

                                AttributeReference attWrt = (AttributeReference)trans.GetObject(attId, OpenMode.ForWrite);
                                attWrt.Justify = AttachmentPoint.BaseRight;

                                try {
                                    double[] mats = new double[16];

                                    

                                    mats[0] = 1;
                                    mats[1] = 0;
                                    mats[2] = 0;
                                    mats[3] = attRef.Position.X + 1;

                                    if (attRef.Justify == AttachmentPoint.BaseRight) {
                                        attWrt.Justify = AttachmentPoint.BaseRight;
                                        mats[3] = attRef.Position.X - 1;
                                    } else {
                                        attWrt.Justify = AttachmentPoint.BaseLeft;
                                        mats[3] = attRef.Position.X + 1;
                                    }

                                    mats[3] = attRef.Position.X;


                                    mats[4] = 0;
                                    mats[5] = 1;
                                    mats[6] = 0;
                                    mats[7] = attRef.Position.Y;

                                    mats[8] = 0;
                                    mats[9] = 0;
                                    mats[10] = 1;
                                    mats[11] = 0;

                                    mats[12] = 0;
                                    mats[13] = 0;
                                    mats[14] = 0;
                                    mats[15] = 1;

                                    Matrix3d mat = new Matrix3d(mats);
                                    ed.WriteMessage($"\nmat={mat}");
                                    //attWrt.AlignmentPoint = attWrt.AlignmentPoint.TransformBy(mat);
                                    attWrt.AlignmentPoint = attWrt.AlignmentPoint.TransformBy(blockRef.BlockTransform);


                                } catch (Exception ex) { ed.WriteMessage(ex.ToString()); }

                            } else if (attRef.Justify == AttachmentPoint.BaseRight) {

                                ed.WriteMessage($"\n---");

                                AttributeReference attWrt = (AttributeReference)trans.GetObject(attId, OpenMode.ForWrite);
                                attWrt.Justify = AttachmentPoint.BaseLeft;

                                /*
                                try {
                                    double[] mats = new double[16];

                                    mats[0] = 1;
                                    mats[1] = 0;
                                    mats[2] = 0;
                                    mats[3] =  -5.85;

                                    mats[4] = 0;
                                    mats[5] = 1;
                                    mats[6] = 0;
                                    mats[7] = 0;

                                    mats[8] = 0;
                                    mats[9] = 0;
                                    mats[10] = 1;
                                    mats[11] = 0;

                                    mats[12] = 0;
                                    mats[13] = 0;
                                    mats[14] = 0;
                                    mats[15] = 1;

                                    Matrix3d mat = new Matrix3d(mats);
                                    ed.WriteMessage($"\nmat={mat}");
                                    attWrt.AlignmentPoint = attWrt.AlignmentPoint.TransformBy(mat);
                                } catch (Exception ex) { ed.WriteMessage(ex.ToString()); }
                                */



                            } else if (attRef.Justify == AttachmentPoint.BaseLeft) {

                                ed.WriteMessage($"\n+++");

                                AttributeReference attWrt = (AttributeReference)trans.GetObject(attId, OpenMode.ForWrite);
                                attWrt.Justify = AttachmentPoint.BaseRight;
                                /*
                                try {
                                    double[] mats = new double[16];

                                    mats[0] = 1;
                                    mats[1] = 0;
                                    mats[2] = 0;
                                    mats[3] = 5.85;

                                    mats[4] = 0;
                                    mats[5] = 1;
                                    mats[6] = 0;
                                    mats[7] = 0;

                                    mats[8] = 0;
                                    mats[9] = 0;
                                    mats[10] = 1;
                                    mats[11] = 0;

                                    mats[12] = 0;
                                    mats[13] = 0;
                                    mats[14] = 0;
                                    mats[15] = 1;

                                    Matrix3d mat = new Matrix3d(mats);
                                    ed.WriteMessage($"\nmat={mat}");
                                    attWrt.AlignmentPoint = attWrt.AlignmentPoint.TransformBy(mat);
                                } catch (Exception ex) { ed.WriteMessage(ex.ToString()); }
                                */

                            }
                            break;
                            }

                    } else {
                        continue;
                    }

                    //ed.WriteMessage($"\n{lp}");

                    trans.Commit();
                }
            }
        }

        private double ValidAngle(double angle) {
            if (angle < 0) {
                angle += 2 * Math.PI;
            }

            if (angle > (Math.PI / 2)) {
                if (angle < (1.5 * Math.PI)) {
                    angle -= Math.PI;
                }
            }

            return angle;
        }

    }
    
}
