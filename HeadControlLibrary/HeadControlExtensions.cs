using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using AForge.Imaging;

namespace HeadControlLibrary
{
    public static class HeadControlExtensions
    {
        public static String GetError(this Exception ex)
        {
            String extra = "";

            return extra + " " + ex.Message + (ex.InnerException!= null ? ex.InnerException.Message : "");
        }
        public static Rectangle GetBiggest(this Rectangle[] rects)
        {
            return rects.Aggregate((r1, r2) => (r1.Height * r1.Width) > (r2.Height * r2.Width) ? r1 : r2);
        }

        public static Rectangle Bounds(this Bitmap bmp)
        {
            return new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        public static BitmapData GetDirectAccess(this Bitmap bmp)
        {
            return bmp.LockBits(bmp.Bounds(), ImageLockMode.ReadWrite, bmp.PixelFormat);
        }

        public static Rectangle MulBy(this Rectangle rect, int scale)
        {
            rect.X *= scale;
            rect.Y *= scale;
            rect.Width *= scale;
            rect.Height *= scale;
            return rect;
        }

        public static void Set(this Accord.Imaging.Filters.RectanglesMarker marker, ref AForge.Imaging.UnmanagedImage um, Rectangle rect)
        {
            marker.Rectangles = new Rectangle[] { rect };
            marker.ApplyInPlace(um);
        }

        public static Rectangle First(this Rectangle[] rects)
        {
            return rects[0];
        }

        public static UnmanagedImage ResizeTo(this UnmanagedImage ui, int w, int h)
        {
            AForge.Imaging.Filters.ResizeNearestNeighbor resize = new AForge.Imaging.Filters.ResizeNearestNeighbor(w, h);
            return resize.Apply(ui);
        }

        public static void DrawHorizontalAxis(this UnmanagedImage im, ref Accord.Vision.Tracking.Camshift tracker)
        {
            AForge.Math.Geometry.LineSegment axis = tracker.TrackingObject.GetAxis();
            Drawing.Line(im, axis.Start.Round(), axis.End.Round(), Color.Red);
        }

        public static UnmanagedImage GetUnmanaged(this BitmapData bd)
        {
            return new UnmanagedImage(bd);
        }

        public static Rectangle AvoidBackgroundTracking(this Rectangle rect, float scaleX, float scaleY)
        {
            return new Rectangle(
                           (int)((rect.X + rect.Width / 2) * scaleX),
                           (int)((rect.Y + rect.Height / 2) * scaleY),
                           1, 1);
        }

        public static Position Current(this Rectangle rect)
        {
            return new Position(rect.X, rect.Y);
        }
    }
}
