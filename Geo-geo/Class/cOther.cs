using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace Geo_geo.Class {
    internal class cOther {

        public Point3dCollection ExtractBounds(DBText txt, double fac = 1.0) {

            var pts = new Point3dCollection();



            if (txt.Bounds.HasValue && txt.Visible) {

                // Create a straight version of the text object

                // and copy across all the relevant properties

                // (stopped copying AlignmentPoint, as it would

                // sometimes cause an eNotApplicable error)



                // We'll create the text at the WCS origin

                // with no rotation, so it's easier to use its

                // extents



                var txt2 = new DBText();

                txt2.Normal = Vector3d.ZAxis;

                txt2.Position = Point3d.Origin;



                // Other properties are copied from the original



                txt2.TextString = txt.TextString;

                txt2.TextStyleId = txt.TextStyleId;

                txt2.LineWeight = txt.LineWeight;

                txt2.Thickness = txt2.Thickness;

                txt2.HorizontalMode = txt.HorizontalMode;

                txt2.VerticalMode = txt.VerticalMode;

                txt2.WidthFactor = txt.WidthFactor;

                txt2.Height = txt.Height;

                txt2.IsMirroredInX = txt2.IsMirroredInX;

                txt2.IsMirroredInY = txt2.IsMirroredInY;

                txt2.Oblique = txt.Oblique;



                // Get its bounds if it has them defined

                // (which it should, as the original did)



                if (txt2.Bounds.HasValue) {

                    var maxPt = txt2.Bounds.Value.MaxPoint;



                    // Only worry about this single case, for now



                    Matrix3d mat = Matrix3d.Identity;

                    if (txt.Justify == AttachmentPoint.MiddleCenter) {

                        mat = Matrix3d.Displacement((Point3d.Origin - maxPt) * 0.5);

                    }



                    // Place all four corners of the bounding box

                    // in an array



                    double minX, minY, maxX, maxY;

                    if (txt.Justify == AttachmentPoint.MiddleCenter) {

                        minX = -maxPt.X * 0.5 * fac;

                        maxX = maxPt.X * 0.5 * fac;

                        minY = -maxPt.Y * 0.5 * fac;

                        maxY = maxPt.Y * 0.5 * fac;

                    } else {

                        minX = 0;

                        minY = 0;

                        maxX = maxPt.X * fac;

                        maxY = maxPt.Y * fac;

                    }

                    var bounds =

                      new Point2d[] {

              new Point2d(minX, minY),

              new Point2d(minX, maxY),

              new Point2d(maxX, maxY),

              new Point2d(maxX, minY)

                      };



                    // We're going to get each point's WCS coordinates

                    // using the plane the text is on



                    var pl = new Plane(txt.Position, txt.Normal);



                    // Rotate each point and add its WCS location to the

                    // collection



                    foreach (Point2d pt in bounds) {

                        pts.Add(

                          pl.EvaluatePoint(

                            pt.RotateBy(txt.Rotation, Point2d.Origin)

                          )

                        );

                    }

                }

            }

            return pts;

        }

        public void selectAllOnPaper() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;


            TypedValue[] filterlist = new TypedValue[5];


            filterlist[0] = new TypedValue(0, "TEXT");

            //8 = DxfCode.LayerName

            filterlist[1] = new TypedValue(8, "_NR_DZIALEK_DZIELONYCH,_Numery_dzielone");

            //410 = DxfCode.LayoutName

            filterlist[2] = new TypedValue(-4, "<NOT");

            filterlist[3] = new TypedValue(410, "Model");

            filterlist[4] = new TypedValue(-4, "NOT>");

            SelectionFilter filter =

                                    new SelectionFilter(filterlist);



            PromptSelectionResult selRes = ed.SelectAll(filter);



            if (selRes.Status != PromptStatus.OK) {

                ed.WriteMessage(

                            "\nerror in getting the selectAll");

                return;

            }

            ObjectId[] ids = selRes.Value.GetObjectIds();



            ed.WriteMessage("No entity found: " + ids.Length.ToString() + "\n");

        }





        class PlineJig : EntityJig {

            // Maintain a list of vertices...

            // Not strictly necessary, as these will be stored in the

            // polyline, but will not adversely impact performance

            Point3dCollection m_pts;
            Point3dCollection l_pts;

            // Use a separate variable for the most recent point...

            // Again, not strictly necessary, but easier to reference

            Point3d m_tempPoint;

            Plane m_plane;

            // Jig fajnie zwraca 4 narożniki 
            public PlineJig(Matrix3d ucs)

              : base(new Polyline()) {

                // Create a point collection to store our vertices

                m_pts = new Point3dCollection();

                l_pts = new Point3dCollection();


                // Create a temporary plane, to help with calcs

                Point3d origin = new Point3d(0, 0, 0);

                Vector3d normal = new Vector3d(0, 0, 1);

                normal = normal.TransformBy(ucs);

                m_plane = new Plane(origin, normal);


                // Create polyline, set defaults, add dummy vertex

                Polyline pline = Entity as Polyline;

                pline.SetDatabaseDefaults();

                pline.Normal = normal;

                pline.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);

            }


            protected override SamplerStatus Sampler(JigPrompts prompts) {

                JigPromptPointOptions jigOpts =

                  new JigPromptPointOptions();

                jigOpts.UserInputControls =

                  (UserInputControls.Accept3dCoordinates |

                  UserInputControls.NullResponseAccepted |

                  UserInputControls.NoNegativeResponseAccepted

                  );


                if (m_pts.Count == 0) {

                    // For the first vertex, just ask for the point

                    jigOpts.Message = "\nStart point of polyline: ";

                } else if (m_pts.Count > 0) {

                    // For subsequent vertices, use a base point

                    jigOpts.BasePoint = m_pts[m_pts.Count - 1];
                    jigOpts.UseBasePoint = true;
                    jigOpts.Message = "\nPolyline vertex: ";

                } else // should never happen

                    return SamplerStatus.Cancel;


                // Get the point itself

                PromptPointResult res = prompts.AcquirePoint(jigOpts);


                // Check if it has changed or not
                // (reduces flicker)

                if (m_tempPoint == res.Value) {

                    return SamplerStatus.NoChange;

                } else if (res.Status == PromptStatus.OK) {

                    m_tempPoint = res.Value;

                    return SamplerStatus.OK;

                }

                return SamplerStatus.Cancel;

            }


            protected override bool Update() {

                // Update the dummy vertex to be our
                // 3D point projected onto our plane

                Polyline pline = Entity as Polyline;

                pline.SetPointAt(pline.NumberOfVertices - 1, m_tempPoint.Convert2d(m_plane));

                if (l_pts.Count == 0) {

                    l_pts.Add(m_tempPoint);
                    l_pts.Add(m_tempPoint);
                    l_pts.Add(m_tempPoint);
                    l_pts.Add(m_tempPoint);


                }




                if ((pline.NumberOfVertices < 4) && (pline.NumberOfVertices > 1)) {

                    pline.AddVertexAt(pline.NumberOfVertices, m_tempPoint.Convert2d(m_plane), 0, 0, 0);
                    pline.AddVertexAt(pline.NumberOfVertices, m_tempPoint.Convert2d(m_plane), 0, 0, 0);
                    pline.AddVertexAt(pline.NumberOfVertices, m_tempPoint.Convert2d(m_plane), 0, 0, 0);
                    //pline.AddVertexAt(pline.NumberOfVertices, new Point2d(pline.GetPoint2dAt(0).X, pline.GetPoint2dAt(0).Y), 0, 0, 0);

                } else if (pline.NumberOfVertices >= 4) {

                    pline.SetPointAt(pline.NumberOfVertices - 1, new Point2d(pline.GetPoint2dAt(0).X, pline.GetPoint2dAt(0).Y));
                    pline.SetPointAt(pline.NumberOfVertices - 2, new Point2d(m_tempPoint.X, pline.GetPoint2dAt(0).Y));
                    pline.SetPointAt(pline.NumberOfVertices - 3, m_tempPoint.Convert2d(m_plane));
                    pline.SetPointAt(pline.NumberOfVertices - 4, new Point2d(pline.GetPoint2dAt(0).X, m_tempPoint.Y));

                    l_pts[0] = (new Point3d(pline.GetPoint2dAt(0).X, pline.GetPoint2dAt(0).Y, 0.0));
                    l_pts[1] = (new Point3d(m_tempPoint.X, pline.GetPoint2dAt(0).Y, 0.0));
                    l_pts[2] = (m_tempPoint);
                    l_pts[3] = (new Point3d(pline.GetPoint2dAt(0).X, m_tempPoint.Y, 0.0));


                }



                return true;

            }

            public Point3dCollection GetPoints() {

                return l_pts;
            }



            public Entity GetEntity() {

                return Entity;

            }


            public void AddLatestVertex() {

                // Add the latest selected point to
                // our internal list...
                // This point will already be in the
                // most recently added pline vertex


                m_pts.Add(m_tempPoint);

                Polyline pline = Entity as Polyline;

                // Create a new dummy vertex...
                // can have any initial value

                pline.AddVertexAt(pline.NumberOfVertices, m_tempPoint.Convert2d(m_plane), 0, 0, 0);

            }


            public void RemoveLastVertex() {

                // Let's remove our dummy vertex
                Polyline pline = Entity as Polyline;

                /*for (int i = 0; i <= m_pts.Count; i++) {
                    pline.RemoveVertexAt(i);
                }
                */

                //pline.RemoveVertexAt(m_pts.Count);

            }

        }


        //[CommandMethod("MYPOLY")]

        public Point3dCollection MyPolyJig() {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            // Get the current UCS, to pass to the Jig
            Matrix3d ucs = ed.CurrentUserCoordinateSystem;

            // Create our Jig object
            PlineJig jig = new PlineJig(ucs);


            // Loop to set the vertices directly on the polyline
            bool bSuccess = true, bComplete = false;

            /*

            do {

                PromptResult res = ed.Drag(jig);
                bSuccess = (res.Status == PromptStatus.OK);

                // A new point was added

                if (bSuccess)

                    jig.AddLatestVertex();

                // Null input terminates the command

                bComplete = (res.Status == PromptStatus.None);

                if (bComplete)

                    // Let's clean-up the polyline before adding it

                    jig.RemoveLastVertex();

            } while (bSuccess && !bComplete);

            */



            for (int i = 0; i < 2; i++) {

                PromptResult res = ed.Drag(jig);
                bSuccess = (res.Status == PromptStatus.OK);

                if (bSuccess)

                    jig.AddLatestVertex();

            }

            jig.RemoveLastVertex();


            // If the jig completed successfully, add the polyline

            //if (bComplete) {
            if (bSuccess) {

                // Append entity
                /*
                Database db = doc.Database;
                Transaction tr = db.TransactionManager.StartTransaction();


                using (tr) {

                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                    btr.AppendEntity(jig.GetEntity());
                    tr.AddNewlyCreatedDBObject(jig.GetEntity(), true);
                    tr.Commit();

                }
                */






            }

            try {

                Point3dCollection pts = jig.GetPoints();


                foreach (Point3d pt in pts) {

                    ed.WriteMessage($"\n{pt.X}\t{pt.Y}");
                }
                ed.WriteMessage($"\n");


                return pts;

            }
             catch {

                Point3dCollection empty = new Point3dCollection();

                return empty;


            }


        }
    }
}



