using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo_geo.Class {
    internal class cPointSort {

        // https://through-the-interface.typepad.com/through_the_interface/2011/01/sorting-an-autocad-point2dcollection-or-point3dcollection-using-net.html

        internal class sortBy {

            protected static bool IsZero(double a) {

                return Math.Abs(a) < Tolerance.Global.EqualPoint;

            }



            protected static bool IsEqual(double a, double b) {

                return IsZero(b - a);

            }



            protected int Compare(double aX, double bX) {

                if (IsEqual(aX, bX)) return 0; // ==

                if (aX < bX) return -1; // <

                return 1; // >

            }

        }



        internal class sort2dByX : sortBy, IComparer<Point2d> {

            public int Compare(Point2d a, Point2d b) {

                return base.Compare(a.X, b.X);

            }

        }

        internal class sort2dByY : sortBy, IComparer<Point2d> {

            public int Compare(Point2d a, Point2d b) {

                return base.Compare(a.Y, b.Y);

            }

        }



        internal class sort3dByX : sortBy, IComparer<Point3d> {

            public int Compare(Point3d a, Point3d b) {

                return base.Compare(a.X, b.X);

            }

        }
    }
}
