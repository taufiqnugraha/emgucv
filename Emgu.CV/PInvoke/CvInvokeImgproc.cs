//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
   public partial class CvInvoke
   {
      #region Sampling, Interpolation and Geometrical Transforms
      /// <summary>
      /// Implements a particular case of application of line iterators. The function reads all the image points lying on the line between pt1 and pt2, including the ending points, and stores them into the buffer
      /// </summary>
      /// <param name="image">Image to sample the line from</param>
      /// <param name="pt1">Starting the line point.</param>
      /// <param name="pt2">Ending the line point</param>
      /// <param name="buffer">Buffer to store the line points; must have enough size to store max( |pt2.x-pt1.x|+1, |pt2.y-pt1.y|+1 ) points in case of 8-connected line and |pt2.x-pt1.x|+|pt2.y-pt1.y|+1 in case of 4-connected line</param>
      /// <param name="connectivity">The line connectivity, 4 or 8</param>
      /// <returns></returns>
#if ANDROID
      public static int cvSampleLine(IntPtr image, Point pt1, Point pt2, IntPtr buffer, CvEnum.Connectivity connectivity)
      {
         return cvSampleLine(image, pt1.X, pt1.Y, pt2.X, pt2.Y, buffer, connectivity);
      }
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int cvSampleLine(IntPtr image, int pt1X, int pt1Y, int pt2X, int pt2Y, IntPtr buffer, CvEnum.Connectivity connectivity);
#else
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int cvSampleLine(IntPtr image, Point pt1, Point pt2, IntPtr buffer, CvEnum.Connectivity connectivity);
#endif

      /// <summary>
      /// Extracts pixels from src:
      /// dst(x, y) = src(x + center.x - (width(dst)-1)*0.5, y + center.y - (height(dst)-1)*0.5)
      /// where the values of pixels at non-integer coordinates are retrieved using bilinear interpolation. Every channel of multiple-channel images is processed independently. Whereas the rectangle center must be inside the image, the whole rectangle may be partially occluded. In this case, the replication border mode is used to get pixel values beyond the image boundaries.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Extracted rectangle</param>
      /// <param name="center">Floating point coordinates of the extracted rectangle center within the source image. The center must be inside the image.</param>
#if ANDROID
      public static void cvGetRectSubPix(IntPtr src, IntPtr dst, PointF center)
      {
         cvGetRectSubPix(src, dst, center.X, center.Y);
      }
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvGetRectSubPix(IntPtr src, IntPtr dst, float centerX, float centerY);
#else
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetRectSubPix(IntPtr src, IntPtr dst, PointF center);
#endif

      /// <summary>
      /// Extracts pixels from src at sub-pixel accuracy and stores them to dst as follows:
      /// dst(x, y)= src( A_11 x'+A_12 y'+ b1, A_21 x'+A_22 y'+ b2),
      /// where A and b are taken from map_matrix:
      /// map_matrix = [ [A11 A12  b1], [ A21 A22  b2 ] ]
      /// x'=x-(width(dst)-1)*0.5, y'=y-(height(dst)-1)*0.5
      /// where the values of pixels at non-integer coordinates A (x,y)^T + b are retrieved using bilinear interpolation. When the function needs pixels outside of the image, it uses replication border mode to reconstruct the values. Every channel of multiple-channel images is processed independently.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Extracted quadrangle</param>
      /// <param name="mapMatrix">The transformation 2 x 3 matrix [A|b]</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetQuadrangleSubPix(IntPtr src, IntPtr dst, IntPtr mapMatrix);

      /// <summary>
      /// Resizes the image src down to or up to the specified size
      /// </summary>
      /// <param name="src">Source image.</param>
      /// <param name="dst">Destination image</param>
      /// <param name="dsize">Output image size; if it equals zero, it is computed as: dsize=Size(round(fx*src.cols), round(fy * src.rows)). Either dsize or both fx and fy must be non-zero.</param>
      /// <param name="fx">Scale factor along the horizontal axis</param>
      /// <param name="fy">Scale factor along the vertical axis;</param>
      /// <param name="interpolation">Interpolation method</param>
      public static void Resize(IInputArray src, IOutputArray dst, Size dsize, double fx = 0, double fy = 0, CvEnum.Inter interpolation = CvEnum.Inter.Linear)
      {
         cveResize(src.InputArrayPtr, dst.OutputArrayPtr, ref dsize, fx, fy, interpolation);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveResize(IntPtr src, IntPtr dst, ref Size dsize, double fx, double fy, CvEnum.Inter interpolation);

      /// <summary>
      /// Applies an affine transformation to an image.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="mapMatrix">2x3 transformation matrix</param>
      /// <param name="dsize">Size of the output image.</param>
      /// <param name="interpMethod">Interpolation method</param>
      /// <param name="warpMethod">Warp method</param>
      /// <param name="borderMode">Pixel extrapolation method</param>
      /// <param name="borderValue">A value used to fill outliers</param>
      public static void WarpAffine(IInputArray src, IOutputArray dst, IInputArray mapMatrix, Size dsize, CvEnum.Inter interpMethod = CvEnum.Inter.Linear, CvEnum.Warp warpMethod = CvEnum.Warp.Default, CvEnum.BorderType borderMode = CvEnum.BorderType.Constant, MCvScalar borderValue = new MCvScalar())
      {
         cveWarpAffine(src.InputArrayPtr, dst.OutputArrayPtr, mapMatrix.InputArrayPtr, ref dsize, (int) interpMethod | (int) warpMethod, borderMode, ref borderValue); 
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveWarpAffine(
         IntPtr src,
         IntPtr dst,
         IntPtr mapMatrix,
         ref Size dsize,
         int flags,
         CvEnum.BorderType borderMode,
         ref MCvScalar fillval);

      /// <summary>
      /// Calculates the matrix of an affine transform such that:
      /// (x'_i,y'_i)^T=map_matrix (x_i,y_i,1)^T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..2.
      /// </summary>
      /// <param name="src">Pointer to an array of PointF, Coordinates of 3 triangle vertices in the source image.</param>
      /// <param name="dst">Pointer to an array of PointF, Coordinates of the 3 corresponding triangle vertices in the destination image</param>
      /// <param name="mapMatrix">Pointer to the destination 2x3 matrix</param>
      /// <returns>Pointer to the destination 2x3 matrix</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetAffineTransform(
         IntPtr src,
         IntPtr dst,
         IntPtr mapMatrix);

      /// <summary>
      /// Calculates the matrix of an affine transform such that:
      /// (x'_i,y'_i)^T=map_matrix (x_i,y_i,1)^T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..2.
      /// </summary>
      /// <param name="src">Coordinates of 3 triangle vertices in the source image.</param>
      /// <param name="dst">Coordinates of the 3 corresponding triangle vertices in the destination image</param>
      /// <param name="mapMatrix">Pointer to the destination 2x3 matrix</param>
      /// <returns>Pointer to the destination 2x3 matrix</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetAffineTransform(
         PointF[] src,
         PointF[] dst,
         IntPtr mapMatrix);

      /// <summary>
      /// Calculates rotation matrix
      /// </summary>
      /// <param name="center">Center of the rotation in the source image. </param>
      /// <param name="angle">The rotation angle in degrees. Positive values mean couter-clockwise rotation (the coordiate origin is assumed at top-left corner).</param>
      /// <param name="scale">Isotropic scale factor</param>
      /// <param name="mapMatrix">Pointer to the destination 2x3 matrix</param>
      /// <returns>Pointer to the destination 2x3 matrix</returns>
      public static void GetRotationMatrix2D(PointF center, double angle, double scale, IOutputArray mapMatrix)
      {
         cveGetRotationMatrix2D(ref center, angle, scale, mapMatrix.OutputArrayPtr);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveGetRotationMatrix2D(
          ref PointF center,
          double angle,
          double scale,
          IntPtr mapMatrix);


      /// <summary>
      /// Applies a perspective transformation to an image
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="mapMatrix">3x3 transformation matrix</param>
      /// <param name="dsize">Size of the output image</param>
      /// <param name="interpolationType">Interpolation method</param>
      /// <param name="warpType">Warp method</param>
      /// <param name="borderMode">Pixel extrapolation method</param>
      /// <param name="borderValue">value used in case of a constant border</param>
      public static void WarpPerspective(
         IInputArray src,
         IOutputArray dst,
         IInputArray mapMatrix,
         Size dsize,
         CvEnum.Inter interpolationType = CvEnum.Inter.Linear,
         CvEnum.Warp warpType = CvEnum.Warp.Default,
         CvEnum.BorderType borderMode = CvEnum.BorderType.Constant,
         MCvScalar borderValue = new MCvScalar())
      {
         cveWarpPerspective(src.InputArrayPtr, dst.OutputArrayPtr, mapMatrix.InputArrayPtr, ref dsize, (int)interpolationType | (int)warpType, borderMode, ref borderValue);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveWarpPerspective(
         IntPtr src,
         IntPtr dst,
         IntPtr m,
         ref Size dsize,
         int flags,
         CvEnum.BorderType borderMode,
         ref MCvScalar fillval);

      /// <summary>
      /// calculates matrix of perspective transform such that:
      /// (t_i x'_i,t_i y'_i,t_i)^T=map_matrix (x_i,y_i,1)^T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..3.
      /// </summary>
      /// <param name="src">Coordinates of 4 quadrangle vertices in the source image</param>
      /// <param name="dst">Coordinates of the 4 corresponding quadrangle vertices in the destination image</param>
      /// <param name="mapMatrix">Pointer to the destination 3x3 matrix</param>
      /// <returns>Pointer to the perspective transform matrix</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetPerspectiveTransform(
         PointF[] src,
         PointF[] dst,
         IntPtr mapMatrix);

      /// <summary>
      /// calculates matrix of perspective transform such that:
      /// (t_i x'_i,t_i y'_i,t_i)^T=map_matrix (x_i,y_i,1)T
      /// where dst(i)=(x'_i,y'_i), src(i)=(x_i,y_i), i=0..3.
      /// </summary>
      /// <param name="src">Coordinates of 4 quadrangle vertices in the source image</param>
      /// <param name="dst">Coordinates of the 4 corresponding quadrangle vertices in the destination image</param>
      /// <param name="mapMatrix">Pointer to the destination 3x3 matrix</param>
      /// <returns>Pointer to the perspective transform matrix</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvGetPerspectiveTransform(
         IntPtr src,
         IntPtr dst,
         IntPtr mapMatrix);

      /// <summary>
      /// Applies a generic geometrical transformation to an image.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="map1">The first map of either (x,y) points or just x values having the type CV_16SC2 , CV_32FC1 , or CV_32FC2 . See convertMaps() for details on converting a floating point representation to fixed-point for speed.</param>
      /// <param name="map2">The second map of y values having the type CV_16UC1 , CV_32FC1 , or none (empty map if map1 is (x,y) points), respectively.</param>
      /// <param name="interpolation">Interpolation method (see resize() ). The method 'Area' is not supported by this function. </param>
      /// <param name="borderMode">Pixel extrapolation method </param>
      /// <param name="borderValue">A value used to fill outliers</param>
      public static void Remap(
         IInputArray src, IOutputArray dst,
         IInputArray map1, IInputArray map2, 
         CvEnum.Inter interpolation, 
         CvEnum.BorderType borderMode = CvEnum.BorderType.Constant,
         MCvScalar borderValue = new MCvScalar() )
      {
         cveRemap(src.InputArrayPtr, dst.OutputArrayPtr, map1.InputArrayPtr, map2.InputArrayPtr, interpolation, borderMode, ref borderValue);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveRemap(IntPtr src, IntPtr dst, IntPtr map1, IntPtr map2, CvEnum.Inter interpolation, CvEnum.BorderType borderMode, ref MCvScalar borderValue);

      /// <summary>
      /// Inverts an affine transformation
      /// </summary>
      /// <param name="m">Original affine transformation</param>
      /// <param name="im">Output reverse affine transformation.</param>
      public static void InvertAffineTransform(IInputArray m, IOutputArray im)
      {
         cveInvertAffineTransform(m.InputArrayPtr, im.OutputArrayPtr);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveInvertAffineTransform(IntPtr m, IntPtr im);

      /// <summary>
      /// Returns the default new camera matrix.
      /// </summary>
      /// <param name="cameraMatrix">Input camera matrix.</param>
      /// <param name="imgsize">Camera view image size in pixels.</param>
      /// <param name="centerPrincipalPoint">Location of the principal point in the new camera matrix. The parameter indicates whether this location should be at the image center or not.</param>
      /// <returns>The default new camera matrix.</returns>
      public static Mat GetDefaultNewCameraMatrix(IInputArray cameraMatrix, Size imgsize = new Size(), bool centerPrincipalPoint = false)
      {
         Mat m = new Mat();
         cveGetDefaultNewCameraMatrix(cameraMatrix.InputArrayPtr, ref imgsize, centerPrincipalPoint, m.Ptr);
         return m;
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveGetDefaultNewCameraMatrix(
         IntPtr cameraMatrix, ref Size imgsize, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool centerPrincipalPoint, 
         IntPtr cm);

      /// <summary>
      /// The function emulates the human "foveal" vision and can be used for fast scale and rotation-invariant template matching, for object tracking etc.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="center">The transformation center, where the output precision is maximal</param>
      /// <param name="M">Magnitude scale parameter</param>
      /// <param name="interpolationType">Interpolation method</param>
      /// <param name="warpType">warp method</param>
      public static void LogPolar(
         IInputArray src,
         IOutputArray dst,
         PointF center,
         double M,
         CvEnum.Inter interpolationType = CvEnum.Inter.Linear, 
         CvEnum.Warp warpType = CvEnum.Warp.FillOutliers)
      {
         cveLogPolar(src.InputArrayPtr, dst.OutputArrayPtr, ref center, M, (int)interpolationType | (int)warpType);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveLogPolar(
         IntPtr src,
         IntPtr dst,
         ref PointF center,
         double M,
         int flags);

      /// <summary>
      /// The function emulates the human "foveal" vision and can be used for fast scale and rotation-invariant template matching, for object tracking etc.
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="center">The transformation center, where the output precision is maximal</param>
      /// <param name="maxRadius">Maximum radius</param>
      /// <param name="interpolationType">Interpolation method</param>
      /// <param name="warpType">Warp method</param>
      public static void LinearPolar(
         IInputArray src,
         IOutputArray dst,
         PointF center,
         double maxRadius,
         CvEnum.Inter interpolationType = CvEnum.Inter.Linear,
         CvEnum.Warp warpType = CvEnum.Warp.FillOutliers)
      {
         cveLinearPolar(src.InputArrayPtr, dst.OutputArrayPtr, ref center, maxRadius, (int)interpolationType | (int)warpType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveLinearPolar(
         IntPtr src,
         IntPtr dst,
         ref PointF center,
         double maxRadius,
         int flags);

      #endregion

      /// <summary>
      /// Performs downsampling step of Gaussian pyramid decomposition. First it convolves source image with the specified filter and then downsamples the image by rejecting even rows and columns.
      /// </summary>
      /// <param name="src">The source image.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      /// <param name="borderType">Border type</param>
      public static void PyrDown(IInputArray src, IOutputArray dst, CvEnum.BorderType borderType = CvEnum.BorderType.Default)
      {
         Size s = Size.Empty;
         cvePyrDown(src.InputArrayPtr, dst.OutputArrayPtr, ref s, borderType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvePyrDown(IntPtr src, IntPtr dst, ref Size size, CvEnum.BorderType borderType);

      /// <summary>
      /// Performs up-sampling step of Gaussian pyramid decomposition. First it upsamples the source image by injecting even zero rows and columns and then convolves result with the specified filter multiplied by 4 for interpolation. So the destination image is four times larger than the source image.
      /// </summary>
      /// <param name="src">The source image.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      /// <param name="borderType">Border type</param>
      public static void PyrUp(IInputArray src, IOutputArray dst, CvEnum.BorderType borderType = CvEnum.BorderType.Default)
      {
         Size s = Size.Empty;
         cvePyrUp(src.InputArrayPtr, dst.OutputArrayPtr, ref s, borderType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvePyrUp(IntPtr src, IntPtr dst, ref Size size, CvEnum.BorderType borderType);

      /// <summary>
      /// Implements one of the variants of watershed, non-parametric marker-based segmentation algorithm, described in [Meyer92] Before passing the image to the function, user has to outline roughly the desired regions in the image markers with positive (>0) indices, i.e. every region is represented as one or more connected components with the pixel values 1, 2, 3 etc. Those components will be "seeds" of the future image regions. All the other pixels in markers, which relation to the outlined regions is not known and should be defined by the algorithm, should be set to 0's. On the output of the function, each pixel in markers is set to one of values of the "seed" components, or to -1 at boundaries between the regions.
      /// </summary>
      /// <remarks>Note, that it is not necessary that every two neighbor connected components are separated by a watershed boundary (-1's pixels), for example, in case when such tangent components exist in the initial marker image. </remarks>
      /// <param name="image">The input 8-bit 3-channel image</param>
      /// <param name="markers">The input/output Int32 depth single-channel image (map) of markers. </param>
      public static void Watershed(IInputArray image, IInputOutputArray markers)
      {
         cveWatershed(image.InputArrayPtr, markers.InputOutputArrayPtr);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveWatershed(IntPtr image, IntPtr markers);

      #region Computational Geometry
      /// <summary>
      /// Finds minimum area rectangle that contains both input rectangles inside
      /// </summary>
      /// <param name="rect1">First rectangle </param>
      /// <param name="rect2">Second rectangle </param>
      /// <returns>The minimum area rectangle that contains both input rectangles inside</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern Rectangle cvMaxRect(ref Rectangle rect1, ref Rectangle rect2);

      /// <summary>
      /// Fits line to 2D or 3D point set 
      /// </summary>
      /// <param name="points">Input vector of 2D or 3D points, stored in std::vector or Mat.</param>
      /// <param name="distType">The distance used for fitting </param>
      /// <param name="param">Numerical parameter (C) for some types of distances, if 0 then some optimal value is chosen</param>
      /// <param name="reps">Sufficient accuracy for radius (distance between the coordinate origin and the line),  0.01 would be a good default</param>
      /// <param name="aeps">Sufficient accuracy for angle, 0.01 would be a good default</param>
      /// <param name="line">Output line parameters. In case of 2D ?tting, it should be a vector of 4 elements (like Vec4f) - (vx, vy, x0, y0), where (vx, vy) is a normalized vector collinear to the line 
      /// and (x0, y0) is a point on the line. In case of 3D ?tting, it should be a vector of 6 elements
      /// (like Vec6f) - (vx, vy, vz, x0, y0, z0), where (vx, vy, vz) is a normalized vector
      /// collinear to the line and (x0, y0, z0) is a point on the line.
      /// </param>
      public static void FitLine(
          IInputArray points,
          IOutputArray line,
          CvEnum.DistType distType,
          double param,
          double reps,
          double aeps)
      {
         cveFitLine(points.InputArrayPtr, line.OutputArrayPtr, distType, param, reps, aeps);
      }

      /// <summary>
      /// Fits line to 2D or 3D point set 
      /// </summary>
      /// <param name="points">Input vector of 2D points.</param>
      /// <param name="distType">The distance used for fitting </param>
      /// <param name="param">Numerical parameter (C) for some types of distances, if 0 then some optimal value is chosen</param>
      /// <param name="reps">Sufficient accuracy for radius (distance between the coordinate origin and the line),  0.01 would be a good default</param>
      /// <param name="aeps">Sufficient accuracy for angle, 0.01 would be a good default</param>
      /// <param name="direction">A normalized vector collinear to the line </param>
      /// <param name="pointOnLine">A point on the line.</param>
      public static void FitLine(
          PointF[] points,
          out PointF direction, 
          out PointF pointOnLine,
          CvEnum.DistType distType,
          double param,
          double reps,
          double aeps)
      {
         using(VectorOfPointF pv = new VectorOfPointF(points))
         using (VectorOfFloat line = new VectorOfFloat())
         {
            cveFitLine(pv.InputArrayPtr, line.OutputArrayPtr, distType, param, reps, aeps);
            float[] values = line.ToArray();
            direction = new PointF(values[0], values[1]);
            pointOnLine = new PointF(values[2], values[3]);

         }
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveFitLine(IntPtr points, IntPtr line, CvEnum.DistType distType, double param, double reps, double aeps);

      /// <summary>
      /// Finds out if there is any intersection between two rotated rectangles.
      /// </summary>
      /// <param name="rect1">First rectangle</param>
      /// <param name="rect2">Second rectangle</param>
      /// <param name="intersectingRegion">The output array of the verticies of the intersecting region. It returns at most 8 vertices. Stored as VectorOfPointF or Mat as Mx1 of type CV_32FC2.</param>
      /// <returns>The intersect type</returns>
      public static CvEnum.RectIntersectType RotatedRectangleIntersection(RotatedRect rect1, RotatedRect rect2, IOutputArray intersectingRegion)
      {
         return cveRotatedRectangleIntersection(ref rect1, ref rect2, intersectingRegion.OutputArrayPtr);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern CvEnum.RectIntersectType cveRotatedRectangleIntersection(ref RotatedRect rect1, ref RotatedRect rect2, IntPtr intersectingRegion);

      /// <summary>
      /// Calculates vertices of the input 2d box.
      /// </summary>
      /// <param name="box">The box</param>
      /// <returns>The four vertices of rectangles.</returns>
      public static PointF[] BoxPoints(RotatedRect box)
      {
         PointF[] pts = new PointF[4];
         GCHandle handle = GCHandle.Alloc(pts, GCHandleType.Pinned);
         using (Mat vp = new Mat(4, 2, DepthType.Cv32F, 1, handle.AddrOfPinnedObject(), 8))
         {
            cveBoxPoints(ref box, vp.OutputArrayPtr);
         }
         handle.Free();
         return pts;
      }

      /// <summary>
      /// Calculates vertices of the input 2d box.
      /// </summary>
      /// <param name="box">The box</param>
      /// <param name="points">The output array of four vertices of rectangles.</param>
      public static void BoxPoints(RotatedRect box, IOutputArray points)
      {
         cveBoxPoints(ref box, points.OutputArrayPtr);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveBoxPoints(
         ref RotatedRect box,
         IntPtr pt);

      /// <summary>
      /// Fits an ellipse around a set of 2D points.
      /// </summary>
      /// <param name="points">Input 2D point set</param>
      /// <returns>The ellipse that fits best (in least-squares sense) to a set of 2D points</returns>
      public static RotatedRect FitEllipse(IInputArray points)
      {
         RotatedRect ellipse = new RotatedRect();
         cveFitEllipse(points.InputArrayPtr, ref ellipse);
         return ellipse;
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveFitEllipse(IntPtr points, ref RotatedRect ellipse);


      /// <summary>
      /// Finds convex hull of 2D point set using Sklansky's algorithm
      /// </summary>
      /// <param name="points">The points to find convex hull from</param>
      /// <param name="clockwise">Orientation flag. If it is true, the output convex hull is oriented clockwise. Otherwise, it is oriented counter-clockwise. The assumed coordinate system has its X axis pointing to the right, and its Y axis pointing upwards.</param>
      /// <returns>The convex hull of the points</returns>
      public static PointF[] ConvexHull(PointF[] points, bool clockwise = false)
      {
         using (VectorOfPointF vpf = new VectorOfPointF(points))
         using (VectorOfPointF hull = new VectorOfPointF())
         {
            CvInvoke.ConvexHull(vpf, hull, clockwise, true);
            return hull.ToArray();
         }
      }

      /// <summary>
      /// The function cvConvexHull2 finds convex hull of 2D point set using Sklansky's algorithm. 
      /// </summary>
      /// <param name="points"> Input 2D point set</param>
      /// <param name="hull">Output convex hull. It is either an integer vector of indices or vector of points. In the first case, the hull elements are 0-based indices of the convex hull points in the original array (since the set of convex hull points is a subset of the original point set). In the second case, hull elements are the convex hull points themselves.</param>
      /// <param name="clockwise">Orientation flag. If it is true, the output convex hull is oriented clockwise. Otherwise, it is oriented counter-clockwise. The assumed coordinate system has its X axis pointing to the right, and its Y axis pointing upwards.</param>
      /// <param name="returnPoints">Operation flag. In case of a matrix, when the flag is true, the function returns convex hull points. Otherwise, it returns indices of the convex hull points. When the output array is std::vector, the flag is ignored, and the output depends on the type of the vector</param>
      public static void ConvexHull(IInputArray points, IOutputArray hull, bool clockwise = false, bool returnPoints = true)
      {
         cveConvexHull(points.InputArrayPtr, hull.OutputArrayPtr, clockwise, returnPoints);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveConvexHull(
         IntPtr points,
         IntPtr hull,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool clockwise,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool returnPoints);

      #endregion

      /// <summary>
      /// The default morphology value.
      /// </summary>
      public static MCvScalar MorphologyDefaultBorderValue = new MCvScalar(double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue);

      //public static Point MorphologyDefaultAnchor = new Point(-1, -1);
      /// <summary>
      /// Erodes the source image using the specified structuring element that determines the shape of a pixel neighborhood over which the minimum is taken:
      /// dst=erode(src,element):  dst(x,y)=min((x',y') in element)) src(x+x',y+y')
      /// The function supports the in-place mode. Erosion can be applied several (iterations) times. In case of color image each channel is processed independently.
      /// </summary>
      /// <param name="src">Source image. </param>
      /// <param name="dst">Destination image</param>
      /// <param name="element">Structuring element used for erosion. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used.</param>
      /// <param name="iterations">Number of times erosion is applied.</param>
      /// <param name="borderType">Pixel extrapolation method</param>
      /// <param name="borderValue">Border value in case of a constant border, use Constant for default</param>
      /// <param name="anchor">Position of the anchor within the element; default value (-1, -1) means that the anchor is at the element center.</param>
      public static void Erode(IInputArray src, IOutputArray dst, IInputArray element, Point anchor, int iterations, CvEnum.BorderType borderType, MCvScalar borderValue)
      {
         cveErode(src.InputArrayPtr, dst.OutputArrayPtr, element == null ? IntPtr.Zero : element.InputArrayPtr, ref anchor, iterations, borderType, ref borderValue);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveErode(IntPtr src, IntPtr dst, IntPtr kernel, ref Point anchor, int iterations, CvEnum.BorderType borderType, ref MCvScalar borderValue);

      /// <summary>
      /// Dilates the source image using the specified structuring element that determines the shape of a pixel neighborhood over which the maximum is taken
      /// The function supports the in-place mode. Dilation can be applied several (iterations) times. In case of color image each channel is processed independently
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Destination image</param>
      /// <param name="element">Structuring element used for erosion. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used</param>
      /// <param name="iterations">Number of times erosion is applied</param>
      /// <param name="borderType">Pixel extrapolation method</param>
      /// <param name="borderValue">Border value in case of a constant border </param>
      /// <param name="anchor">Position of the anchor within the element; default value (-1, -1) means that the anchor is at the element center.</param>
      public static void Dilate(IInputArray src, IOutputArray dst, IInputArray element, Point anchor, int iterations, CvEnum.BorderType borderType, MCvScalar borderValue)
      {
         cveDilate(src.InputArrayPtr, dst.OutputArrayPtr, element == null ? IntPtr.Zero : element.InputArrayPtr, ref anchor, iterations, borderType, ref borderValue);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDilate(IntPtr src, IntPtr dst, IntPtr kernel, ref Point anchor, int iterations, CvEnum.BorderType borderType, ref MCvScalar borderValue);

      public static void GaussianBlur(IInputArray src, IOutputArray dst, Size ksize, double sigmaX, double sigmaY = 0,
         CvEnum.BorderType borderType = BorderType.Default)
      {
         cveGaussianBlur(src.InputArrayPtr, dst.OutputArrayPtr, ref ksize, sigmaX, sigmaY, borderType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveGaussianBlur(IntPtr src, IntPtr dst, ref Size ksize, double sigmaX, double sigmaY, CvEnum.BorderType borderType);

      public static void Blur(IInputArray src, IOutputArray dst, Size ksize, Point anchor,
         CvEnum.BorderType borderType = BorderType.Default)
      {
         cveBlur(src.InputArrayPtr, dst.OutputArrayPtr, ref ksize, ref anchor, borderType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveBlur(IntPtr src, IntPtr dst, ref Size kSize, ref Point anchor, CvEnum.BorderType borderType);

      public static void MedianBlur(IInputArray src, IOutputArray dst, int ksize)
      {
         cveMedianBlur(src.InputArrayPtr, dst.OutputArrayPtr, ksize);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveMedianBlur(IntPtr src, IntPtr dst, int ksize);

      public static void BoxFilter(IInputArray src, IOutputArray dst, DepthType ddepth, Size ksize, Point anchor,
         bool normalize = true, CvEnum.BorderType borderType = BorderType.Default)
      {
         cveBoxFilter(src.InputArrayPtr, dst.OutputArrayPtr, ddepth, ref ksize, ref anchor, normalize, borderType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveBoxFilter(IntPtr src, IntPtr dst, DepthType ddepth, ref Size ksize, ref Point anchor, bool normailize, CvEnum.BorderType borderType);

      public static void BilateralFilter(IInputArray src, IOutputArray dst, int d, double sigmaColor, double sigmaSpace,
         CvEnum.BorderType borderType = BorderType.Default)
      {
         cveBilateralFilter(src.InputArrayPtr, dst.OutputArrayPtr, d, sigmaColor, sigmaSpace, borderType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveBilateralFilter(IntPtr src, IntPtr dst, int d, double sigmaColor, double sigmaSpace, CvEnum.BorderType borderType);

      /// <summary>
      /// The Sobel operators combine Gaussian smoothing and differentiation so the result is more or less robust to the noise. Most often, the function is called with (xorder=1, yorder=0, aperture_size=3) or (xorder=0, yorder=1, aperture_size=3) to calculate first x- or y- image derivative. The first case corresponds to
      /// <pre> 
      ///  |-1  0  1|
      ///  |-2  0  2|
      ///  |-1  0  1|</pre>
      /// kernel and the second one corresponds to
      /// <pre>
      ///  |-1 -2 -1|
      ///  | 0  0  0|
      ///  | 1  2  1|</pre>
      /// or
      /// <pre>
      ///  | 1  2  1|
      ///  | 0  0  0|
      ///  |-1 -2 -1|</pre>
      /// kernel, depending on the image origin (origin field of IplImage structure). No scaling is done, so the destination image usually has larger by absolute value numbers than the source image. To avoid overflow, the function requires 16-bit destination image if the source image is 8-bit. The result can be converted back to 8-bit using cvConvertScale or cvConvertScaleAbs functions. Besides 8-bit images the function can process 32-bit floating-point images. Both source and destination must be single-channel images of equal size or ROI size
      /// </summary>
      /// <param name="src">Source image.</param>
      /// <param name="dst">Destination image</param>
      /// <param name="ddepth">output image depth; the following combinations of src.depth() and ddepth are supported:
      /// <para> src.depth() = CV_8U, ddepth = -1/CV_16S/CV_32F/CV_64F </para>
      /// <para> src.depth() = CV_16U/CV_16S, ddepth = -1/CV_32F/CV_64F</para>
      /// <para> src.depth() = CV_32F, ddepth = -1/CV_32F/CV_64F</para>
      /// <para>src.depth() = CV_64F, ddepth = -1/CV_64F</para>
      /// when ddepth=-1, the destination image will have the same depth as the source; in the case of 8-bit input images it will result in truncated derivatives.</param>
      /// <param name="xorder">Order of the derivative x </param>
      /// <param name="yorder">Order of the derivative y</param>
      /// <param name="kSize">Size of the extended Sobel kernel, must be 1, 3, 5 or 7. </param>
      ///<param name="borderType">Pixel extrapolation method </param>
      ///<param name="scale">Optional scale factor for the computed derivative values</param>
      ///<param name="delta">Optional delta value that is added to the results prior to storing them in <paramref name="dst"/></param>
      public static void Sobel(
         IInputArray src, IOutputArray dst, 
         CvEnum.DepthType ddepth, int xorder, int yorder, 
         int kSize = 3, double scale = 1, double delta = 0, 
         CvEnum.BorderType borderType = CvEnum.BorderType.Default)
      {
         cveSobel(src.InputArrayPtr, dst.OutputArrayPtr, ddepth, xorder, yorder, kSize, scale, delta, borderType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveSobel(IntPtr src, IntPtr dst, CvEnum.DepthType ddepth, int xorder, int yorder, int apertureSize, double scale, double delta, CvEnum.BorderType borderType);

      
      /// <summary>
      /// Calculates Laplacian of the source image by summing second x- and y- derivatives calculated using Sobel operator:
      /// dst(x,y) = d2src/dx2 + d2src/dy2
      /// Specifying aperture_size=1 gives the fastest variant that is equal to convolving the image with the following kernel:
      /// |0  1  0|
      /// |1 -4  1|
      /// |0  1  0|
      /// Similar to cvSobel function, no scaling is done and the same combinations of input and output formats are supported. 
      /// </summary>
      /// <param name="src">Source image. </param>
      /// <param name="dst">Destination image. Should have type of float</param>
      /// <param name="ddepth">Desired depth of the destination image.</param>
      /// <param name="ksize">Aperture size used to compute the second-derivative filters.</param>
      /// <param name="scale">Optional scale factor for the computed Laplacian values. By default, no scaling is applied. </param>
      /// <param name="delta">Optional delta value that is added to the results prior to storing them in dst.</param>
      /// <param name="borderType"> Pixel extrapolation method.</param>
      public static void Laplacian(
         IInputArray src, IOutputArray dst, 
         CvEnum.DepthType ddepth, int ksize = 1, double scale = 1, double delta = 0, 
         CvEnum.BorderType borderType = CvEnum.BorderType.Default)
      {
         cveLaplacian(src.InputArrayPtr, dst.OutputArrayPtr, ddepth, ksize, scale, delta, borderType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveLaplacian(IntPtr src, IntPtr dst, CvEnum.DepthType ddepth, int ksize, double scale, double delta, CvEnum.BorderType borderType);

      /// <summary>
      /// Finds the edges on the input <paramref name="image"/> and marks them in the output image edges using the Canny algorithm. The smallest of threshold1 and threshold2 is used for edge linking, the largest - to find initial segments of strong edges.
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="edges">Image to store the edges found by the function</param>
      /// <param name="threshold1">The first threshold</param>
      /// <param name="threshold2">The second threshold.</param>
      /// <param name="apertureSize">Aperture parameter for Sobel operator </param>
      ///<param name="l2Gradient">a flag, indicating whether a more accurate norm should be used to calculate the image gradient magnitude ( L2gradient=true ), or whether the default norm is enough ( L2gradient=false ).</param>
      public static void Canny(
          IInputArray image,
          IOutputArray edges,
          double threshold1,
          double threshold2,
          int apertureSize = 3,
          bool l2Gradient = false)
      {
         cveCanny(image.InputArrayPtr, edges.OutputArrayPtr, threshold1, threshold2, apertureSize, l2Gradient);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCanny(IntPtr image, IntPtr edges, double threshold1, double threshold2, int apertureSize, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool l2Gradient);

      /// <summary>
      /// The function tests whether the input contour is convex or not. The contour must be simple, that is, without self-intersections. Otherwise, the function output is undefined.
      /// </summary>
      /// <param name="contour">Input vector of 2D points </param>
      /// <returns>true if input is convex</returns>
      public static bool IsContourConvex(IInputArray contour)
      {
         return cveIsContourConvex(contour.InputArrayPtr);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      private static extern bool cveIsContourConvex(IntPtr contour);

      /// <summary>
      /// Determines whether the point is inside contour, outside, or lies on an edge (or coinsides with a vertex). It returns positive, negative or zero value, correspondingly
      /// </summary>
      /// <param name="contour">Input contour</param>
      /// <param name="pt">The point tested against the contour</param>
      /// <param name="measureDist">If != 0, the function estimates distance from the point to the nearest contour edge</param>
      /// <returns>
      /// When measureDist = false, the return value is &gt;0 (inside), &lt;0 (outside) and =0 (on edge), respectively. 
      /// When measureDist != true, it is a signed distance between the point and the nearest contour edge
      /// </returns>
      public static double PointPolygonTest(IInputArray contour, PointF pt, bool measureDist)
      {
         return cvePointPolygonTest(contour.InputArrayPtr, ref pt, measureDist);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern double cvePointPolygonTest(
         IntPtr contour,
         ref PointF pt,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool measureDist);


      /// <summary>
      /// Finds all convexity defects of the input contour and returns a sequence of the CvConvexityDefect structures. 
      /// </summary>
      /// <param name="contour">Input contour</param>
      /// <param name="convexhull">Convex hull obtained using cvConvexHull2 that should contain pointers or indices to the contour points, not the hull points themselves, i.e. return_points parameter in cvConvexHull2 should be 0</param>
      /// <param name="storage">Container for output sequence of convexity defects. If it is NULL, contour or hull (in that order) storage is used</param>
      /// <returns>Pointer to the sequence of the CvConvexityDefect structures. </returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvConvexityDefects(
         IntPtr contour,
         IntPtr convexhull,
         IntPtr storage);

      /// <summary>
      /// Find the bounding rectangle for the specific array of points
      /// </summary>
      /// <param name="points">The collection of points</param>
      /// <returns>The bounding rectangle for the array of points</returns>
      public static RotatedRect MinAreaRect(PointF[] points)
      {
         using (VectorOfPointF vpf = new VectorOfPointF(points))
         {
            return MinAreaRect(vpf);
         }
      }

      /// <summary>
      /// Finds a rotated rectangle of the minimum area enclosing the input 2D point set.
      /// </summary>
      /// <param name="points">Input vector of 2D points</param>
      /// <returns>a circumscribed rectangle of the minimal area for 2D point set</returns>
      public static RotatedRect MinAreaRect(IInputArray points)
      {
         RotatedRect rect = new RotatedRect();
         cveMinAreaRect(points.InputArrayPtr, ref rect);
         return rect;
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveMinAreaRect(IntPtr points, ref RotatedRect box);


      /// <summary>
      /// Finds the minimal circumscribed circle for 2D point set using iterative algorithm. It returns nonzero if the resultant circle contains all the input points and zero otherwise (i.e. algorithm failed)
      /// </summary>
      /// <param name="points">Sequence or array of 2D points</param>
      ///<returns>The minimal circumscribed circle for 2D point set</returns>
      public static CircleF MinEnclosingCircle(PointF[] points)
      {
         using (VectorOfPointF vp = new VectorOfPointF(points))
            return MinEnclosingCircle(vp);
      }

      /// <summary>
      /// Finds the minimal circumscribed circle for 2D point set using iterative algorithm. It returns nonzero if the resultant circle contains all the input points and zero otherwise (i.e. algorithm failed)
      /// </summary>
      /// <param name="points">Sequence or array of 2D points</param>
      ///<returns>The minimal circumscribed circle for 2D point set</returns>
      public static CircleF MinEnclosingCircle(IInputArray points)
      {
         PointF center = new PointF();
         float radius = 0;
         cveMinEnclosingCircle(points.InputArrayPtr, ref center, ref radius);
         return new CircleF(center, radius);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveMinEnclosingCircle(IntPtr points, ref PointF center, ref float radius);

      /// <summary>
      /// Finds a triangle of minimum area enclosing a 2D point set and returns its area.
      /// </summary>
      /// <param name="points">Input vector of 2D points with depth CV_32S or CV_32F</param>
      /// <param name="triangles">Output vector of three 2D points defining the vertices of the triangle. The depth of the OutputArray must be CV_32F.</param>
      /// <returns>The triangle's area</returns>
      public static double MinEnclosingTriangle(IInputArray points, IOutputArray triangles)
      {
         return cveMinEnclosingTriangle(points.InputArrayPtr, triangles.OutputArrayPtr);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern double cveMinEnclosingTriangle(IntPtr points, IntPtr triangle);

      #region Contour Processing Functions

      /// <summary>
      /// Approximates a polygonal curve(s) with the specified precision.
      /// </summary>
      ///<param name="curve">Input vector of a 2D point</param>
      /// <param name="approxCurve">Result of the approximation. The type should match the type of the input curve. </param>
      /// <param name="epsilon">Parameter specifying the approximation accuracy. This is the maximum distance between the original curve and its approximation.</param>
      /// <param name="closed"> If true, the approximated curve is closed (its first and last vertices are connected). Otherwise, it is not closed.</param>
      public static void ApproxPolyDP(IInputArray curve, IOutputArray approxCurve, double epsilon, bool closed)
      {
         cveApproxPolyDP(curve.InputArrayPtr, approxCurve.OutputArrayPtr, epsilon, closed);         
      }
      
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveApproxPolyDP(
         IntPtr curve,
         IntPtr approxCurve,
         double epsilon,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool closed);

      /// <summary>
      /// Returns the up-right bounding rectangle for 2d point set
      /// </summary>
      /// <param name="points"> Input 2D point set, stored in std::vector or Mat.</param>
      /// <returns>The up-right bounding rectangle for 2d point set</returns>
      public static Rectangle BoundingRectangle(IInputArray points)
      {
         Rectangle rectangle = new Rectangle();
         cveBoundingRectangle(points.InputArrayPtr, ref rectangle);
         return rectangle;
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveBoundingRectangle(IntPtr points, ref Rectangle boundingRect);

      /// <summary>
      /// Calculates area of the whole contour or contour section. 
      /// </summary>
      /// <param name="contour">Input vector of 2D points (contour vertices), stored in std::vector or Mat. </param>
      /// <param name="oriented">Oriented area flag. If it is true, the function returns a signed area value, depending on the contour orientation (clockwise or counter-clockwise).
      /// Using this feature you can determine orientation of a contour by taking the sign of an area. 
      /// By default, the parameter is false, which means that the absolute value is returned.</param>
      /// <returns>The area of the whole contour or contour section</returns>
      public static double ContourArea(IInputArray contour, bool oriented = false)
      {
         return cveContourArea(contour.InputArrayPtr, oriented);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern double cveContourArea(
         IntPtr contour, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool oriented);

      /// <summary>
      /// Calculates a contour perimeter or a curve length
      /// </summary>
      /// <param name="curve">Sequence or array of the curve points</param>
      /// <param name="isClosed">
      /// Indicates whether the curve is closed or not.
      /// </param>
      /// <returns>Contour perimeter or a curve length</returns>
      public static double ArcLength(IInputArray curve, bool isClosed)
      {
         return cveArcLength(curve.InputArrayPtr, isClosed);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern double cveArcLength(
         IntPtr curve,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool isClosed);

      /*
      /// <summary>
      /// Find the perimeter of the contour
      /// </summary>
      /// <param name="contour">Pointer to the contour</param>
      /// <returns>the perimeter of the contour</returns>
      public static double cvContourPerimeter(IntPtr contour)
      {
         return cvArcLength(contour, MCvSlice.WholeSeq, 1);
      }

      
      /// <summary>
      /// Creates binary tree representation for the input contour and returns the pointer to its root.
      /// </summary>
      /// <param name="contour">Input contour</param>
      /// <param name="storage">Container for output tree</param>
      /// <param name="threshold">If the parameter threshold is less than or equal to 0, the function creates full binary tree representation. If the threshold is greater than 0, the function creates representation with the precision threshold: if the vertices with the interceptive area of its base line are less than threshold, the tree should not be built any further</param>
      /// <returns>The binary tree representation for the input contour</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateContourTree(
         IntPtr contour,
         IntPtr storage,
         double threshold);

      /// <summary>
      /// Return the contour from its binary tree representation
      /// </summary>
      /// <param name="tree">Contour tree</param>
      /// <param name="storage">Container for the reconstructed contour</param>
      /// <param name="criteria">Criteria, where to stop reconstruction</param>
      /// <returns>The contour represented by this contour tree</returns>
#if ANDROID
      public static IntPtr cvContourFromContourTree(
         IntPtr tree,
         IntPtr storage,
         MCvTermCriteria criteria)
      {
         return cvContourFromContourTree(tree, storage, criteria.type, criteria.max_iter, criteria.epsilon);
      }
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvContourFromContourTree(
         IntPtr tree,
         IntPtr storage,
          CvEnum.TermCritType type,
         int maxIter,
         double epsilon);
#else
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvContourFromContourTree(
         IntPtr tree,
         IntPtr storage,
         MCvTermCriteria criteria);
#endif*/

      /// <summary>
      /// Calculates the value of the matching measure for two contour trees. The similarity measure is calculated level by level from the binary tree roots. If at the certain level difference between contours becomes less than threshold, the reconstruction process is interrupted and the current difference is returned
      /// </summary>
      /// <param name="tree1">First contour tree</param>
      /// <param name="tree2">Second contour tree</param>
      /// <param name="method">Similarity measure, only CV_CONTOUR_TREES_MATCH_I1 is supported</param>
      /// <param name="threshold">Similarity threshold</param>
      /// <returns>The value of the matching measure for two contour trees</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvMatchContourTrees(
         IntPtr tree1,
         IntPtr tree2,
         CvEnum.MatchContourTreeMethod method,
         double threshold);
      #endregion

      /// <summary>
      /// Applies fixed-level thresholding to single-channel array. The function is typically used to get bi-level (binary) image out of grayscale image (cvCmpS could be also used for this purpose) or for removing a noise, i.e. filtering out pixels with too small or too large values. There are several types of thresholding the function supports that are determined by threshold_type
      /// </summary>
      /// <param name="src">Source array (single-channel, 8-bit of 32-bit floating point). </param>
      /// <param name="dst">Destination array; must be either the same type as src or 8-bit. </param>
      /// <param name="threshold">Threshold value</param>
      /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
      /// <param name="thresholdType">Thresholding type </param>
      public static double Threshold(
         IInputArray src,
         IOutputArray dst,
         double threshold,
         double maxValue,
         CvEnum.ThresholdType thresholdType)
      {
         return cveThreshold(src.InputArrayPtr, dst.OutputArrayPtr, threshold, maxValue, thresholdType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern double cveThreshold(
         IntPtr src,
         IntPtr dst,
         double threshold,
         double maxValue,
         CvEnum.ThresholdType thresholdType);

      /// <summary>
      /// Transforms grayscale image to binary image. 
      /// Threshold calculated individually for each pixel. 
      /// For the method CV_ADAPTIVE_THRESH_MEAN_C it is a mean of <paramref name="blockSize"/> x <paramref name="blockSize"/> pixel
      /// neighborhood, subtracted by param1. 
      /// For the method CV_ADAPTIVE_THRESH_GAUSSIAN_C it is a weighted sum (gaussian) of <paramref name="blockSize"/> x <paramref name="blockSize"/> pixel neighborhood, subtracted by param1.
      /// </summary>
      /// <param name="src">Source array (single-channel, 8-bit of 32-bit floating point). </param>
      /// <param name="dst">Destination array; must be either the same type as src or 8-bit. </param>
      /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
      /// <param name="adaptiveType">Adaptive_method </param>
      /// <param name="thresholdType">Thresholding type. must be one of CV_THRESH_BINARY, CV_THRESH_BINARY_INV  </param>
      /// <param name="blockSize">The size of a pixel neighborhood that is used to calculate a threshold value for the pixel: 3, 5, 7, ... </param>
      /// <param name="param1">Constant subtracted from mean or weighted mean. It may be negative. </param>
      public static void AdaptiveThreshold(
         IInputOutputArray src,
         IOutputArray dst,
         double maxValue,
         CvEnum.AdaptiveThresholdType adaptiveType,
         CvEnum.ThresholdType thresholdType,
         int blockSize,
         double param1)
      {
         cveAdaptiveThreshold(src.InputOutputArrayPtr, dst.OutputArrayPtr,
            maxValue, adaptiveType, thresholdType, blockSize, param1);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveAdaptiveThreshold(
         IntPtr src,
         IntPtr dst,
         double maxValue,
         CvEnum.AdaptiveThresholdType adaptiveType,
         CvEnum.ThresholdType thresholdType,
         int blockSize,
         double param1);

      /// <summary>
      /// Retrieves contours from the binary image and returns the number of retrieved contours. The pointer firstContour is filled by the function. It will contain pointer to the first most outer contour or IntPtr.Zero if no contours is detected (if the image is completely black). Other contours may be reached from firstContour using h_next and v_next links. The sample in cvDrawContours discussion shows how to use contours for connected component detection. Contours can be also used for shape analysis and object recognition - see squares.c in OpenCV sample directory
      /// The function modifies the source image content
      /// </summary>
      /// <param name="image">The source 8-bit single channel image. Non-zero pixels are treated as 1s, zero pixels remain 0s - that is image treated as binary. To get such a binary image from grayscale, one may use cvThreshold, cvAdaptiveThreshold or cvCanny. The function modifies the source image content</param>
      /// <param name="contours">Detected contours. Each contour is stored as a vector of points.</param>
      /// <param name="hierarchy">Optional output vector, containing information about the image topology.</param>
      /// <param name="mode">Retrieval mode</param>
      /// <param name="method">Approximation method (for all the modes, except CV_RETR_RUNS, which uses built-in approximation). </param>
      /// <param name="offset">Offset, by which every contour point is shifted. This is useful if the contours are extracted from the image ROI and then they should be analyzed in the whole image context</param>
      /// <returns>The number of countours</returns>
      public static void FindContours(
         IInputOutputArray image, IOutputArray contours, IOutputArray hierarchy,
         CvEnum.RetrType mode,
         CvEnum.ChainApproxMethod method,
         Point offset = new Point())
      {
         cveFindContours(image.InputOutputArrayPtr, contours.OutputArrayPtr, hierarchy == null ? IntPtr.Zero : hierarchy.OutputArrayPtr, mode, method, ref offset);
      }

      /// <summary>
      /// Retrieves contours from the binary image as a contour tree. The pointer firstContour is filled by the function. It is provided as a convenient way to obtain the hierarchy value as int[,].
      /// The function modifies the source image content
      /// </summary>
      /// <param name="image">The source 8-bit single channel image. Non-zero pixels are treated as 1s, zero pixels remain 0s - that is image treated as binary. To get such a binary image from grayscale, one may use cvThreshold, cvAdaptiveThreshold or cvCanny. The function modifies the source image content</param>
      /// <param name="contours">Detected contours. Each contour is stored as a vector of points.</param>
      /// <param name="method">Approximation method (for all the modes, except CV_RETR_RUNS, which uses built-in approximation). </param>
      /// <param name="offset">Offset, by which every contour point is shifted. This is useful if the contours are extracted from the image ROI and then they should be analyzed in the whole image context</param>
      /// <returns>The contour hierarchy</returns>
      public static int[,] FindContourTree(
         IInputOutputArray image, IOutputArray contours,
         CvEnum.ChainApproxMethod method,
         Point offset = new Point())
      {
         using (Mat hierachyMat = new Mat())
         {
            cveFindContours(image.InputOutputArrayPtr, contours.OutputArrayPtr, hierachyMat.OutputArrayPtr, RetrType.Tree, method, ref offset);
            int[,] hierachy = new int[hierachyMat.Cols, 4];
            GCHandle handle = GCHandle.Alloc(hierachy, GCHandleType.Pinned);
            using (Mat tmp = new Mat(hierachyMat.Rows, hierachyMat.Cols, hierachyMat.Depth, 4, handle.AddrOfPinnedObject(), hierachyMat.Step))
            {
               hierachyMat.CopyTo(tmp);
            }
            handle.Free();
            return hierachy;
         }

      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveFindContours(IntPtr image, IntPtr contours, IntPtr hierarchy, CvEnum.RetrType mode, CvEnum.ChainApproxMethod method, ref Point offset);

      /*
      /// <summary>
      /// Initializes and returns a pointer to the contour scanner. The scanner is used in
      /// cvFindNextContour to retrieve the rest of the contours.
      /// </summary>
      /// <param name="image">The 8-bit, single channel, binary source image</param>
      /// <param name="storage">Container of the retrieved contours</param>
      /// <param name="headerSize">Size of the sequence header, &gt;=sizeof(CvChain) if method=CHAIN_CODE, and &gt;=sizeof(CvContour) otherwise</param>
      /// <param name="mode">Retrieval mode</param>
      /// <param name="method">Approximation method (for all the modes, except CV_RETR_RUNS, which uses built-in approximation). </param>
      /// <param name="offset">Offset, by which every contour point is shifted. This is useful if the contours are extracted from the image ROI and then they should be analyzed in the whole image context</param>
      /// <returns>Pointer to the contour scaner</returns>
#if ANDROID
      public static IntPtr cvStartFindContours(
         IntPtr image,
         IntPtr storage,
         int headerSize,
         CvEnum.RETR_TYPE mode,
         CvEnum.CHAIN_APPROX_METHOD method,
         Point offset)
      {
         return cvStartFindContours(image, storage, headerSize, mode, method, offset.X, offset.Y);
      }
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr cvStartFindContours(
         IntPtr image,
         IntPtr storage,
         int headerSize,
         CvEnum.RETR_TYPE mode,
         CvEnum.CHAIN_APPROX_METHOD method,
         int offsetX, int offsetY);
#else
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvStartFindContours(
         IntPtr image,
         IntPtr storage,
         int headerSize,
         CvEnum.RetrType mode,
         CvEnum.ChainApproxMethod method,
         Point offset);
#endif

      /// <summary>
      /// Finds the next contour in the image
      /// </summary>
      /// <param name="scanner">Pointer to the contour scaner</param>
      /// <returns>The next contour in the image</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvFindNextContour(IntPtr scanner);

      /// <summary>
      /// The function replaces the retrieved contour, that was returned from the preceding call of
      /// cvFindNextContour and stored inside the contour scanner state, with the user-specified contour.
      /// The contour is inserted into the resulting structure, list, two-level hierarchy, or tree, depending on
      /// the retrieval mode. If the parameter new contour is IntPtr.Zero, the retrieved contour is not included
      /// in the resulting structure, nor are any of its children that might be added to this structure later.
      /// </summary>
      /// <param name="scanner">Contour scanner initialized by cvStartFindContours</param>
      /// <param name="newContour">Substituting contour</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvSubstituteContour(
         IntPtr scanner,
         IntPtr newContour);*/

#if !( IOS || ANDROID || NETFX_CORE )
      /// <summary>
      /// Convert raw data to bitmap
      /// </summary>
      /// <param name="scan0">The pointer to the raw data</param>
      /// <param name="step">The step</param>
      /// <param name="size">The size of the image</param>
      /// <param name="srcColorType">The source image color type</param>
      /// <param name="numberOfChannels">The number of channels</param>
      /// <param name="srcDepthType">The source image depth type</param>
      /// <param name="tryDataSharing">Try to create Bitmap that shares the data with the image</param>
      /// <returns>The Bitmap</returns>
      public static Bitmap RawDataToBitmap(IntPtr scan0, int step, Size size, Type srcColorType, int numberOfChannels, Type srcDepthType, bool tryDataSharing = false)
      {
         if (tryDataSharing)
         {
            if (srcColorType == typeof(Gray) && srcDepthType == typeof(Byte))
            {   //Grayscale of Bytes
               Bitmap bmpGray = new Bitmap(
                   size.Width,
                   size.Height,
                   step,
                   System.Drawing.Imaging.PixelFormat.Format8bppIndexed,
                   scan0
                   );

               bmpGray.Palette = CvToolbox.GrayscalePalette;

               return bmpGray;
            }
            // Mono in Linux doesn't support scan0 constructure with Format24bppRgb, use ToBitmap instead
            // See https://bugzilla.novell.com/show_bug.cgi?id=363431
            // TODO: check mono buzilla Bug 363431 to see when it will be fixed 
            else if (
               Emgu.Util.Platform.OperationSystem == Emgu.Util.TypeEnum.OS.Windows &&
               Emgu.Util.Platform.ClrType == Emgu.Util.TypeEnum.ClrType.DotNet &&
               srcColorType == typeof(Bgr) && srcDepthType == typeof(Byte) 
               && (step & 3) == 0)
            {   //Bgr byte    
               return new Bitmap(
                   size.Width,
                   size.Height,
                   step,
                   System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                   scan0);
            }
            else if (srcColorType == typeof(Bgra) && srcDepthType == typeof(Byte))
            {   //Bgra byte
               return new Bitmap(
                   size.Width,
                   size.Height,
                   step,
                   System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                   scan0);
            }

            //PixelFormat.Format16bppGrayScale is not supported in .NET
            //else if (typeof(TColor) == typeof(Gray) && typeof(TDepth) == typeof(UInt16))
            //{
            //   return new Bitmap(
            //      size.width,
            //      size.height,
            //      step,
            //      PixelFormat.Format16bppGrayScale;
            //      scan0);
            //}
         }

         System.Drawing.Imaging.PixelFormat format = System.Drawing.Imaging.PixelFormat.Undefined;

         if (srcColorType == typeof(Gray)) // if this is a gray scale image
         {
            format = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
         }
         else if (srcColorType == typeof(Bgra)) //if this is Bgra image
         {
            format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
         }
         else if (srcColorType == typeof(Bgr))  //if this is a Bgr Byte image
         {
            format = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
         }
         else
         {
            using (Mat m = new Mat(size.Height, size.Width, CvInvoke.GetDepthType(srcDepthType), numberOfChannels, scan0, step))
            using (Mat m2 = new Mat())
            {
               CvInvoke.CvtColor(m, m2, srcColorType, typeof(Bgr));
               return RawDataToBitmap(m2.DataPointer, m2.Step, m2.Size, typeof(Bgr), 3, srcDepthType, false);
            }
         }

         Bitmap bmp = new Bitmap(size.Width, size.Height, format);
         System.Drawing.Imaging.BitmapData data = bmp.LockBits(
             new Rectangle(Point.Empty, size),
              System.Drawing.Imaging.ImageLockMode.WriteOnly,
             format);
         using (Mat bmpMat = new Mat(size.Height, size.Width, CvEnum.DepthType.Cv8U, numberOfChannels, data.Scan0, data.Stride))
         using (Mat dataMat = new Mat(size.Height, size.Width, CvEnum.DepthType.Cv8U, numberOfChannels, scan0, step))
         {
            if (srcDepthType == typeof(Byte))
               dataMat.CopyTo(bmpMat);
            else
            {

               double scale = 1.0, shift = 0.0;
               RangeF range = dataMat.GetValueRange();
               if (range.Max > 255.0 || range.Min < 0)
               {
                  scale = (range.Max == range.Min) ? 0.0 : 255.0 / (range.Max - range.Min);
                  shift = (scale == 0) ? range.Min : -range.Min * scale;
               }
               CvInvoke.ConvertScaleAbs(dataMat, bmpMat, scale, shift);
            }
         }
         bmp.UnlockBits(data);

         if (format == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            bmp.Palette = CvToolbox.GrayscalePalette;
         return bmp;
      }
#endif

      /// <summary>
      /// Finishes the scanning process and returns a pointer to the first contour on the
      /// highest level.
      /// </summary>
      /// <param name="scanner">Reference to the contour scanner</param>
      /// <returns>pointer to the first contour on the highest level</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvEndFindContours(ref IntPtr scanner);

      /// <summary>
      /// Converts input image from one color space to another. The function ignores colorModel and channelSeq fields of IplImage header, so the source image color space should be specified correctly (including order of the channels in case of RGB space, e.g. BGR means 24-bit format with B0 G0 R0 B1 G1 R1 ... layout, whereas RGB means 24-bit format with R0 G0 B0 R1 G1 B1 ... layout). 
      /// </summary>
      /// <param name="src">The source 8-bit (8u), 16-bit (16u) or single-precision floating-point (32f) image</param>
      /// <param name="dest">The destination image of the same data type as the source one. The number of channels may be different</param>
      /// <param name="srcColor">Source color type. </param>
      /// <param name="destColor">Destination color type</param>
      public static void CvtColor(IInputArray src, IOutputArray dest, Type srcColor, Type destColor)
      {
         try
         {
            // if the direct conversion exist, apply the conversion
            CvInvoke.CvtColor(src, dest, CvToolbox.GetColorCvtCode(srcColor, destColor));
         }
         catch
         {
            try
            {
               //if a direct conversion doesn't exist, apply a two step conversion
               using (Mat tmp = new Mat())
               {
                  CvInvoke.CvtColor(src, tmp, CvToolbox.GetColorCvtCode(srcColor, typeof(Bgr)));
                  CvInvoke.CvtColor(tmp, dest, CvToolbox.GetColorCvtCode(typeof(Bgr), destColor));
               }
            }
            catch
            {
               throw new NotSupportedException(String.Format(
                  "Convertion from {0} to {1} is not supported by OpenCV",
                  srcColor.ToString(),
                  destColor.ToString()));
            }
         }
      }

      /// <summary>
      /// Converts input image from one color space to another. The function ignores colorModel and channelSeq fields of IplImage header, so the source image color space should be specified correctly (including order of the channels in case of RGB space, e.g. BGR means 24-bit format with B0 G0 R0 B1 G1 R1 ... layout, whereas RGB means 24-bit format with R0 G0 B0 R1 G1 B1 ... layout). 
      /// </summary>
      /// <param name="src">The source 8-bit (8u), 16-bit (16u) or single-precision floating-point (32f) image</param>
      /// <param name="dst">The destination image of the same data type as the source one. The number of channels may be different</param>
      /// <param name="code">Color conversion operation that can be specifed using CV_src_color_space2dst_color_space constants </param>
      /// <param name="dstCn">number of channels in the destination image; if the parameter is 0, the number of the channels is derived automatically from src and code .</param>
      public static void CvtColor(IInputArray src, IOutputArray dst, CvEnum.ColorConversion code, int dstCn = 0)
      {
         cveCvtColor(src.InputArrayPtr, dst.OutputArrayPtr, code, dstCn);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCvtColor(IntPtr src, IntPtr dst, CvEnum.ColorConversion code, int dstCn);


      /// <summary>
      /// Finds circles in grayscale image using some modification of Hough transform
      /// </summary>
      /// <param name="image">The input 8-bit single-channel grayscale image</param>
      /// <param name="circles">The storage for the circles detected. It can be a memory storage (in this case a sequence of circles is created in the storage and returned by the function) or single row/single column matrix (CvMat*) of type CV_32FC3, to which the circles' parameters are written. The matrix header is modified by the function so its cols or rows will contain a number of lines detected. If circle_storage is a matrix and the actual number of lines exceeds the matrix size, the maximum possible number of circles is returned. Every circle is encoded as 3 floating-point numbers: center coordinates (x,y) and the radius</param>
      /// <param name="method">Currently, the only implemented method is CV_HOUGH_GRADIENT</param>
      /// <param name="dp">Resolution of the accumulator used to detect centers of the circles. For example, if it is 1, the accumulator will have the same resolution as the input image, if it is 2 - accumulator will have twice smaller width and height, etc</param>
      /// <param name="minDist">Minimum distance between centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one. If it is too large, some circles may be missed</param>
      /// <param name="param1">The first method-specific parameter. In case of CV_HOUGH_GRADIENT it is the higher threshold of the two passed to Canny edge detector (the lower one will be twice smaller). </param>
      /// <param name="param2">The second method-specific parameter. In case of CV_HOUGH_GRADIENT it is accumulator threshold at the center detection stage. The smaller it is, the more false circles may be detected. Circles, corresponding to the larger accumulator values, will be returned first</param>
      /// <param name="minRadius">Minimal radius of the circles to search for</param>
      /// <param name="maxRadius">Maximal radius of the circles to search for. By default the maximal radius is set to max(image_width, image_height). </param>
      /// <returns>Pointer to the sequence of circles</returns>
      public static void HoughCircles(
         IInputArray image,
         IOutputArray circles,
         CvEnum.HoughType method,
         double dp,
         double minDist,
         double param1 = 100,
         double param2 = 100,
         int minRadius = 0,
         int maxRadius = 0)
      {
         cveHoughCircles(image.InputArrayPtr, circles.OutputArrayPtr, method, dp, minDist, param1, param2, minRadius, maxRadius);
      }

      /// <summary>
      /// Finds circles in a grayscale image using the Hough transform
      /// </summary>
      /// <param name="image">8-bit, single-channel, grayscale input image.</param>
      /// <param name="method">Detection method to use. Currently, the only implemented method is CV_HOUGH_GRADIENT , which is basically 21HT</param>
      /// <param name="dp">Inverse ratio of the accumulator resolution to the image resolution. For example, if dp=1 , the accumulator has the same resolution as the input image. If dp=2 , the accumulator has half as big width and height.</param>
      /// <param name="minDist">Minimum distance between the centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one. If it is too large, some circles may be missed.</param>
      /// <param name="param1">First method-specific parameter. In case of CV_HOUGH_GRADIENT , it is the higher threshold of the two passed to the Canny() edge detector (the lower one is twice smaller).</param>
      /// <param name="param2">Second method-specific parameter. In case of CV_HOUGH_GRADIENT , it is the accumulator threshold for the circle centers at the detection stage. The smaller it is, the more false circles may be detected. Circles, corresponding to the larger accumulator values, will be returned first.</param>
      /// <param name="minRadius"> Minimum circle radius.</param>
      /// <param name="maxRadius">Maximum circle radius.</param>
      /// <returns>The circles detected</returns>
      public static CircleF[] HoughCircles(
         IInputArray image,
         CvEnum.HoughType method,
         double dp,
         double minDist,
         double param1 = 100,
         double param2 = 100,
         int minRadius = 0,
         int maxRadius = 0)
      {
         using (Mat circles = new Mat())
         {
            HoughCircles(image, circles, method, dp, minDist, param1, param2, minRadius, maxRadius);
            Size s = circles.Size;
            CircleF[] results = new CircleF[s.Width];
            GCHandle handle = GCHandle.Alloc(results, GCHandleType.Pinned);
            using (Mat tmp = new Mat(s.Height, s.Width, CV.CvEnum.DepthType.Cv32F, 3, handle.AddrOfPinnedObject(), sizeof(float) * 3))
            {
               circles.CopyTo(tmp);
            }
            handle.Free();

            return results;
         }
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveHoughCircles(
         IntPtr image,
         IntPtr circles,
         CvEnum.HoughType method,
         double dp,
         double minDist,
         double param1,
         double param2,
         int minRadius,
         int maxRadius);

      /// <summary>
      /// Finds lines in a binary image using the standard Hough transform.
      /// </summary>
      /// <param name="image">8-bit, single-channel binary source image. The image may be modified by the function.</param>
      /// <param name="lines">Output vector of lines. Each line is represented by a two-element vector</param>
      /// <param name="rho">Distance resolution of the accumulator in pixels.</param>
      /// <param name="theta">Angle resolution of the accumulator in radians.</param>
      /// <param name="threshold">Accumulator threshold parameter. Only those lines are returned that get enough votes (&gt; threshold)</param>
      /// <param name="srn">For the multi-scale Hough transform, it is a divisor for the distance resolution rho . The coarse accumulator distance resolution is rho and the accurate accumulator resolution is rho/srn . If both srn=0 and stn=0 , the classical Hough transform is used. Otherwise, both these parameters should be positive.</param>
      /// <param name="stn"> For the multi-scale Hough transform, it is a divisor for the distance resolution theta</param>
      public static void HoughLines(IInputArray image, IOutputArray lines, double rho, double theta, int threshold, double srn = 0, double stn = 0)
      {
         cveHoughLines(image.InputArrayPtr, lines.OutputArrayPtr, rho, theta, threshold, srn, stn);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveHoughLines(IntPtr image, IntPtr lines, double rho, double theta, int threshold, double srn, double stn);

      /// <summary>
      /// Finds line segments in a binary image using the probabilistic Hough transform.
      /// </summary>
      /// <param name="image">8-bit, single-channel binary source image. The image may be modified by the function.</param>
      /// <param name="rho">Distance resolution of the accumulator in pixels</param>
      /// <param name="theta">Angle resolution of the accumulator in radians</param>
      /// <param name="threshold">Accumulator threshold parameter. Only those lines are returned that get enough votes</param>
      /// <param name="minLineLength">Minimum line length. Line segments shorter than that are rejected.</param>
      /// <param name="maxGap">Maximum allowed gap between points on the same line to link them.</param>
      /// <returns>The found line segments</returns>
      public static LineSegment2D[] HoughLinesP(IInputArray image, double rho, double theta, int threshold, double minLineLength = 0, double maxGap = 0)
      {
         using (Mat lines = new Mat())
         {
            HoughLinesP(image, lines, rho, theta, threshold, minLineLength, maxGap);
            Size s = lines.Size;

            LineSegment2D[] segments = new LineSegment2D[s.Height];
            GCHandle handle = GCHandle.Alloc(segments, GCHandleType.Pinned);
            using (Mat tmp = new Mat(s.Height, s.Width, CV.CvEnum.DepthType.Cv32S, 4, handle.AddrOfPinnedObject(), sizeof(int) * 4))
            {
               lines.CopyTo(tmp);
            }
            handle.Free();

            return segments;
         }
      }

      /// <summary>
      /// Finds line segments in a binary image using the probabilistic Hough transform.
      /// </summary>
      /// <param name="image">8-bit, single-channel binary source image. The image may be modified by the function.</param>
      /// <param name="lines"> Output vector of lines. Each line is represented by a 4-element vector (x1, y1, x2, y2)</param>
      /// <param name="rho">Distance resolution of the accumulator in pixels</param>
      /// <param name="theta">Angle resolution of the accumulator in radians</param>
      /// <param name="threshold">Accumulator threshold parameter. Only those lines are returned that get enough votes</param>
      /// <param name="minLineLength">Minimum line length. Line segments shorter than that are rejected.</param>
      /// <param name="maxGap">Maximum allowed gap between points on the same line to link them.</param>
      public static void HoughLinesP(IInputArray image, IOutputArray lines, double rho, double theta, int threshold, double minLineLength = 0, double maxGap = 0)
      {
         cveHoughLinesP(image.InputArrayPtr, lines.OutputArrayPtr, rho, theta, threshold, minLineLength, maxGap);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveHoughLinesP(IntPtr image, IntPtr lines, double rho, double theta, int threshold, double minLineLength, double maxGap);

      /// <summary>
      /// Calculates spatial and central moments up to the third order and writes them to moments. The moments may be used then to calculate gravity center of the shape, its area, main axises and various shape characeteristics including 7 Hu invariants.
      /// </summary>
      /// <param name="arr">Image (1-channel or 3-channel with COI set) or polygon (CvSeq of points or a vector of points)</param>
      /// <param name="binaryImage">(For images only) If the flag is true, all the zero pixel values are treated as zeroes, all the others are treated as 1s</param>
      /// <returns>The moment</returns>
      public static MCvMoments Moments(IInputArray arr, bool binaryImage = false)
      {
         MCvMoments m = new MCvMoments();
         cveMoments(arr.InputArrayPtr, binaryImage, ref m);
         return m;
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveMoments(
         IntPtr arr, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool binaryImage, 
         ref MCvMoments moments);

      /*
      /// <summary>
      /// Finds corners with big eigenvalues in the image. 
      /// </summary>
      /// <remarks>
      /// The function first calculates the minimal eigenvalue for every source image pixel using cvCornerMinEigenVal function and stores them in eig_image. 
      /// Then it performs non-maxima suppression (only local maxima in 3x3 neighborhood remain). 
      /// The next step is rejecting the corners with the minimal eigenvalue less than quality_level*max(eigImage(x,y)). Finally, the function ensures that all the corners found are distanced enough one from another by considering the corners (the most strongest corners are considered first) and checking that the distance between the newly considered feature and the features considered earlier is larger than min_distance. So, the function removes the features than are too close to the stronger features.
      /// </remarks>
      /// <param name="image">The source 8-bit or floating-point 32-bit, single-channel image</param>
      /// <param name="eigImage">Temporary floating-point 32-bit image of the same size as image</param>
      /// <param name="tempImage">Another temporary image of the same size and same format as eig_image</param>
      /// <param name="corners">Output parameter. Detected corners</param>
      /// <param name="cornerCount">Output parameter. Number of detected corners</param>
      /// <param name="qualityLevel">Multiplier for the maxmin eigenvalue; specifies minimal accepted quality of image corners</param>
      /// <param name="minDistance">Limit, specifying minimum possible distance between returned corners; Euclidian distance is used</param>
      /// <param name="mask">Region of interest. The function selects points either in the specified region or in the whole image if the mask is IntPtr.Zero</param>
      /// <param name="blockSize">Size of the averaging block, passed to underlying cvCornerMinEigenVal or cvCornerHarris used by the function</param>
      /// <param name="useHarris">If nonzero, Harris operator (cvCornerHarris) is used instead of default cvCornerMinEigenVal.</param>
      /// <param name="k">Free parameter of Harris detector; used only if <paramref name="useHarris"/> != 0</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGoodFeaturesToTrack(
          IntPtr image,
          IntPtr eigImage,
          IntPtr tempImage,
          IntPtr corners,
          ref int cornerCount,
          double qualityLevel,
          double minDistance,
          IntPtr mask,
          int blockSize,
          int useHarris,
          double k);
      */
      /// <summary>
      /// This function is similiar to cvCalcBackProjectPatch. It slids through image, compares overlapped patches of size wxh with templ using the specified method and stores the comparison results to result
      /// </summary>
      /// <param name="image">Image where the search is running. It should be 8-bit or 32-bit floating-point</param>
      /// <param name="templ">Searched template; must be not greater than the source image and the same data type as the image</param>
      /// <param name="result">A map of comparison results; single-channel 32-bit floating-point. If image is WxH and templ is wxh then result must be W-w+1xH-h+1.</param>
      /// <param name="method">Specifies the way the template must be compared with image regions </param>
      public static void MatchTemplate(
         IInputArray image,
         IInputArray templ,
         IOutputArray result,
         CvEnum.TemplateMatchingType method)
      {
         cveMatchTemplate(image.InputArrayPtr, templ.InputArrayPtr, result.OutputArrayPtr, method);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveMatchTemplate(
          IntPtr image,
          IntPtr templ,
          IntPtr result,
          CvEnum.TemplateMatchingType method);

      /// <summary>
      /// Compares two shapes. The 3 implemented methods all use Hu moments
      /// </summary>
      /// <param name="contour1">First contour or grayscale image</param>
      /// <param name="contour2">Second contour or grayscale image</param>
      /// <param name="method">Comparison method</param>
      /// <param name="parameter">Method-specific parameter (is not used now)</param>
      /// <returns>The result of the comparison</returns>
      public static double MatchShapes(
         IInputArray contour1, IInputArray contour2, 
         CvEnum.ContoursMatchType method,
         double parameter = 0)
      {
         return cveMatchShapes(contour1.InputArrayPtr, contour2.InputArrayPtr, method, parameter);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern double cveMatchShapes(
         IntPtr contour1,
         IntPtr contour2,
         CvEnum.ContoursMatchType method,
         double parameter);


      /// <summary>
      /// Returns a structuring element of the specified size and shape for morphological operations.
      /// </summary>
      /// <param name="shape">Element shape</param>
      /// <param name="ksize">Size of the structuring element.</param>
      /// <param name="anchor">Anchor position within the element. The value (-1, -1) means that the anchor is at the center. Note that only the shape of a cross-shaped element depends on the anchor position. In other cases the anchor just regulates how much the result of the morphological operation is shifted.</param>
      /// <returns>The structuring element</returns>
      public static Mat GetStructuringElement(CvEnum.ElementShape shape, Size ksize, Point anchor)
      {
         Mat res = new Mat();
         cveGetStructuringElement(res, shape, ref ksize, ref anchor);
         return res;
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveGetStructuringElement(IntPtr mat, CvEnum.ElementShape shape, ref Size ksize, ref Point anchor);

      /// <summary>
      /// Performs advanced morphological transformations.
      /// </summary>
      /// <param name="src">Source image.</param>
      /// <param name="dst">Destination image.</param>
      /// <param name="kernel">Structuring element.</param>
      /// <param name="operation">Type of morphological operation.</param>
      /// <param name="iterations">Number of times erosion and dilation are applied.</param>
      /// <param name="borderType">Pixel extrapolation method.</param>
      /// <param name="anchor">Anchor position with the kernel. Negative values mean that the anchor is at the kernel center.</param>
      /// <param name="borderValue">Border value in case of a constant border. </param>
      public static void MorphologyEx(
         IInputArray src,
         IOutputArray dst,
         CvEnum.MorphOp operation,
         IInputArray kernel,
         Point anchor,
         int iterations,
         CvEnum.BorderType borderType,
         MCvScalar borderValue)
      {
         cveMorphologyEx(src.InputArrayPtr, dst.OutputArrayPtr, operation, kernel.InputArrayPtr, ref anchor, iterations, borderType, ref borderValue);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveMorphologyEx(
         IntPtr src,
         IntPtr dst,
         CvEnum.MorphOp operation,
         IntPtr kernel,
         ref Point anchor,
         int iterations, 
         CvEnum.BorderType borderType,
         ref MCvScalar borderValue);

      #region Histograms
      /*
      /// <summary>
      /// Creates a histogram of the specified size and returns the pointer to the created histogram. If the array ranges is 0, the histogram bin ranges must be specified later via The function cvSetHistBinRanges, though cvCalcHist and cvCalcBackProject may process 8-bit images without setting bin ranges, they assume equally spaced in 0..255 bins
      /// </summary>
      /// <param name="dims">Number of histogram dimensions</param>
      /// <param name="sizes">Array of histogram dimension sizes</param>
      /// <param name="type">Histogram representation format: CV_HIST_ARRAY means that histogram data is represented as an multi-dimensional dense array CvMatND; CV_HIST_SPARSE means that histogram data is represented as a multi-dimensional sparse array CvSparseMat</param>
      /// <param name="ranges">Array of ranges for histogram bins. Its meaning depends on the uniform parameter value. The ranges are used for when histogram is calculated or backprojected to determine, which histogram bin corresponds to which value/tuple of values from the input image[s]. </param>
      /// <param name="uniform">
      /// Uniformity flag; 
      /// if != 0, the histogram has evenly spaced bins and for every 0&lt;=i&lt;cDims ranges[i] is array of two numbers: lower and upper boundaries for the i-th histogram dimension. 
      /// The whole range [lower,upper] is split then into dims[i] equal parts to determine i-th input tuple value ranges for every histogram bin. 
      /// And if uniform == 0, then i-th element of ranges array contains dims[i]+1 elements: lower0, upper0, lower1, upper1 == lower2, ..., upperdims[i]-1, where lowerj and upperj are lower and upper boundaries of i-th input tuple value for j-th bin, respectively. 
      /// In either case, the input values that are beyond the specified range for a histogram bin, are not counted by cvCalcHist and filled with 0 by cvCalcBackProject</param>
      /// <returns>A pointer to the histogram</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvCreateHist(
         int dims,
         [In]
         int[] sizes,
         CvEnum.HIST_TYPE type,
         [In]
         IntPtr[] ranges,
         int uniform);

      /// <summary>
      /// Finds the minimum and maximum histogram bins and their positions
      /// </summary>
      /// <remarks>
      /// Among several extremums with the same value the ones with minimum index (in lexicographical order). 
      /// In case of several maximums or minimums the earliest in lexicographical order extrema locations are returned.
      /// </remarks>
      /// <param name="hist">Histogram</param>
      /// <param name="minValue">Pointer to the minimum value of the histogram </param>
      /// <param name="maxValue">Pointer to the maximum value of the histogram </param>
      /// <param name="minIdx">Pointer to the array of coordinates for minimum </param>
      /// <param name="maxIdx">Pointer to the array of coordinates for maximum </param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetMinMaxHistValue(
         IntPtr hist,
         ref float minValue,
         ref float maxValue,
         int[] minIdx,
         int[] maxIdx);

      /// <summary>
      /// Normalizes the histogram bins by scaling them, such that the sum of the bins becomes equal to factor
      /// </summary>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="factor">Normalization factor</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvNormalizeHist(IntPtr hist, double factor);

      /// <summary>
      /// Clears histogram bins that are below the specified threshold
      /// </summary>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="threshold">Threshold level</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvThreshHist(IntPtr hist, double threshold);


      /// <summary>
      /// Sets all histogram bins to 0 in case of dense histogram and removes all histogram bins in case of sparse array
      /// </summary>
      /// <param name="hist">Histogram</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvClearHist(IntPtr hist);

      /// <summary>
      /// initializes the histogram, which header and bins are allocated by user. No cvReleaseHist need to be called afterwards. Only dense histograms can be initialized this way. 
      /// </summary>
      /// <param name="dims">Number of histogram dimensions</param>
      /// <param name="sizes">Array of histogram dimension sizes</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="data">The underline memory storage (pointer to array of float)</param>
      /// <param name="ranges">Array of ranges for histogram bins. Its meaning depends on the uniform parameter value. The ranges are used for when histogram is calculated or backprojected to determine, which histogram bin corresponds to which value/tuple of values from the input image[s]. </param>
      /// <param name="uniform">
      /// Uniformity flag; 
      /// if true, the histogram has evenly spaced bins and for every 0&lt;=i&lt;cDims ranges[i] is array of two numbers: lower and upper boundaries for the i-th histogram dimension. 
      /// The whole range [lower,upper] is split then into dims[i] equal parts to determine i-th input tuple value ranges for every histogram bin. 
      /// And if uniform=false, then i-th element of ranges array contains dims[i]+1 elements: lower0, upper0, lower1, upper1 == lower2, ..., upperdims[i]-1, where lowerj and upperj are lower and upper boundaries of i-th input tuple value for j-th bin, respectively. 
      /// In either case, the input values that are beyond the specified range for a histogram bin, are not counted by cvCalcHist and filled with 0 by cvCalcBackProject
      /// </param>
      /// <returns>Pointer to the histogram</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern IntPtr cvMakeHistHeaderForArray(
         int dims,
         [In] int[] sizes,
         IntPtr hist,
         IntPtr data,
         [In] IntPtr[] ranges,
         int uniform);

      /// <summary>
      /// Creates a histogram of the specified size and returns the pointer to the created histogram. If the array ranges is 0, the histogram bin ranges must be specified later via The function cvSetHistBinRanges, though cvCalcHist and cvCalcBackProject may process 8-bit images without setting bin ranges, they assume equally spaced in 0..255 bins
      /// </summary>
      /// <param name="dims">Number of histogram dimensions</param>
      /// <param name="sizes">Array of histogram dimension sizes</param>
      /// <param name="type">Histogram representation format: CV_HIST_ARRAY means that histogram data is represented as an multi-dimensional dense array CvMatND; CV_HIST_SPARSE means that histogram data is represented as a multi-dimensional sparse array CvSparseMat</param>
      /// <param name="ranges">Array of ranges for histogram bins. Its meaning depends on the uniform parameter value. The ranges are used for when histogram is calculated or backprojected to determine, which histogram bin corresponds to which value/tuple of values from the input image[s]. </param>
      /// <param name="uniform">
      /// Uniformity flag; 
      /// if true, the histogram has evenly spaced bins and for every 0&lt;=i&lt;cDims ranges[i] is array of two numbers: lower and upper boundaries for the i-th histogram dimension. 
      /// The whole range [lower,upper] is split then into dims[i] equal parts to determine i-th input tuple value ranges for every histogram bin. 
      /// And if uniform=false, then i-th element of ranges array contains dims[i]+1 elements: lower0, upper0, lower1, upper1 == lower2, ..., upperdims[i]-1, where lowerj and upperj are lower and upper boundaries of i-th input tuple value for j-th bin, respectively. 
      /// In either case, the input values that are beyond the specified range for a histogram bin, are not counted by cvCalcHist and filled with 0 by cvCalcBackProject
      /// </param>
      /// <returns>A pointer to the histogram</returns>
      public static IntPtr cvCreateHist(
         int dims,
         [In]
         int[] sizes,
         CvEnum.HIST_TYPE type,
         [In]
         IntPtr[] ranges,
         bool uniform)
      {
         return cvCreateHist(dims, sizes, type, ranges, uniform ? 1 : 0);
      }

      /// <summary>
      /// Calculates the histogram of one or more single-channel images. The elements of a tuple that is used to increment a histogram bin are taken at the same location from the corresponding input images.
      /// </summary>
      /// <param name="image">Source images (though, you may pass CvMat** as well), all are of the same size and type</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="accumulate">Accumulation flag. If it is set, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online</param>
      /// <param name="mask">The operation mask, determines what pixels of the source images are counted</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcArrHist(
          IntPtr[] image,
          IntPtr hist,
          int accumulate,
          IntPtr mask);

      /// <summary>
      /// Calculates the histogram of one or more single-channel images. The elements of a tuple that is used to increment a histogram bin are taken at the same location from the corresponding input images.
      /// </summary>
      /// <param name="image">Source images (though, you may pass CvMat** as well), all are of the same size and type</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="accumulate">Accumulation flag. If it is set, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online</param>
      /// <param name="mask">The operation mask, determines what pixels of the source images are counted</param>
      public static void cvCalcArrHist(IntPtr[] image, IntPtr hist, bool accumulate, IntPtr mask)
      {
         cvCalcArrHist(image, hist, accumulate ? 1 : 0, mask);
      }

      /// <summary>
      /// Makes a copy of the histogram. If the second histogram pointer *dst is NULL, a new histogram of the same size as src is created. Otherwise, both histograms must have equal types and sizes. Then the function copies the source histogram bins values to destination histogram and sets the same bin values ranges as in src.
      /// </summary>
      /// <param name="src">The source histogram</param>
      /// <param name="dst">The destination histogram</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCopyHist(IntPtr src, ref IntPtr dst);

      /// <summary>
      /// Compares two dense histograms
      /// </summary>
      /// <param name="hist1">The first dense histogram. </param>
      /// <param name="hist2">The second dense histogram.</param>
      /// <param name="method">Comparison method</param>
      /// <returns>Result of the comparison</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvCompareHist(
         IntPtr hist1,
         IntPtr hist2,
         CvEnum.HISTOGRAM_COMP_METHOD method);

      /// <summary>
      /// Calculates the histogram of one or more single-channel images. The elements of a tuple that is used to increment a histogram bin are taken at the same location from the corresponding input images.
      /// </summary>
      /// <param name="image">Source images (though, you may pass CvMat** as well), all are of the same size and type</param>
      /// <param name="hist">Pointer to the histogram</param>
      /// <param name="accumulate">Accumulation flag. If it is set, the histogram is not cleared in the beginning. This feature allows user to compute a single histogram from several images, or to update the histogram online</param>
      /// <param name="mask">The operation mask, determines what pixels of the source images are counted</param>
      public static void cvCalcHist(
          IntPtr[] image,
          IntPtr hist,
          bool accumulate,
          IntPtr mask)
      {
         cvCalcArrHist(image, hist, accumulate ? 1 : 0, mask);
      }

      /// <summary>
      /// Calculates the back project of the histogram. 
      /// For each tuple of pixels at the same position of all input single-channel images the function puts the value of the histogram bin, corresponding to the tuple, to the destination image. 
      /// In terms of statistics, the value of each output image pixel is probability of the observed tuple given the distribution (histogram). 
      /// </summary>
      /// <example>
      /// To find a red object in the picture, one may do the following: 
      /// 1. Calculate a hue histogram for the red object assuming the image contains only this object. The histogram is likely to have a strong maximum, corresponding to red color. 
      /// 2. Calculate back projection of a hue plane of input image where the object is searched, using the histogram. Threshold the image. 
      /// 3. Find connected components in the resulting picture and choose the right component using some additional criteria, for example, the largest connected component. 
      /// That is the approximate algorithm of Camshift color object tracker, except for the 3rd step, instead of which CAMSHIFT algorithm is used to locate the object on the back projection given the previous object position. 
      /// </example>
      /// <param name="image">Source images (though you may pass CvMat** as well), all are of the same size and type </param>
      /// <param name="backProject">Destination back projection image of the same type as the source images</param>
      /// <param name="hist">Histogram</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvCalcArrBackProject(IntPtr[] image, IntPtr backProject, IntPtr hist);
      */
      /// <summary>
      /// The algorithm normalizes brightness and increases contrast of the image
      /// </summary>
      /// <param name="src">The input 8-bit single-channel image</param>
      /// <param name="dst">The output image of the same size and the same data type as src</param>
      public static void EqualizeHist(IInputArray src, IOutputArray dst)
      {
         cveEqualizeHist(src.InputArrayPtr, dst.OutputArrayPtr);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveEqualizeHist(IntPtr src, IntPtr dst);

      /// <summary>
      /// Calculates a histogram of a set of arrays.
      /// </summary>
      /// <param name="images">Source arrays. They all should have the same depth, CV_8U or CV_32F , and the same size. Each of them can have an arbitrary number of channels.</param>
      /// <param name="channels">List of the channels used to compute the histogram. </param>
      /// <param name="mask">Optional mask. If the matrix is not empty, it must be an 8-bit array of the same size as images[i] . The non-zero mask elements mark the array elements counted in the histogram.</param>
      /// <param name="hist">Output histogram</param>
      /// <param name="histSize">Array of histogram sizes in each dimension.</param>
      /// <param name="ranges">Array of the dims arrays of the histogram bin boundaries in each dimension.</param>
      /// <param name="accumulate">Accumulation flag. If it is set, the histogram is not cleared in the beginning when it is allocated. This feature enables you to compute a single histogram from several sets of arrays, or to update the histogram in time.</param>
      public static void CalcHist(IInputArray images, int[] channels, IInputArray mask, IOutputArray hist, int[] histSize, float[] ranges, bool accumulate)
      {
         using (VectorOfInt channelsVec = new VectorOfInt(channels))
         using (VectorOfInt histSizeVec = new VectorOfInt(histSize))
         using (VectorOfFloat rangesVec = new VectorOfFloat(ranges))
         {
            cveCalcHist(images.InputArrayPtr, channelsVec, mask == null ? IntPtr.Zero : mask.InputArrayPtr, hist.OutputArrayPtr, histSizeVec, rangesVec, accumulate);
         }
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCalcHist(
         IntPtr images, IntPtr channels, IntPtr mask, IntPtr hist, IntPtr histSize, IntPtr ranges, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool accumulate );

      /// <summary>
      /// Calculates the back projection of a histogram.
      /// </summary>
      /// <param name="images">Source arrays. They all should have the same depth, CV_8U or CV_32F , and the same size. Each of them can have an arbitrary number of channels.</param>
      /// <param name="channels">Number of source images.</param>
      /// <param name="hist">Input histogram that can be dense or sparse.</param>
      /// <param name="backProject">Destination back projection array that is a single-channel array of the same size and depth as images[0] .</param>
      /// <param name="ranges">Array of arrays of the histogram bin boundaries in each dimension.</param>
      /// <param name="scale"> Optional scale factor for the output back projection.</param>
      public static void CalcBackProject(IInputArray images, int[] channels, IInputArray hist, IOutputArray backProject, float[] ranges, double scale = 1.0)
      {
         using (VectorOfInt channelsVec = new VectorOfInt(channels))
         using (VectorOfFloat rangeVec = new VectorOfFloat(ranges))
         {
            cveCalcBackProject(images.InputArrayPtr, channelsVec, hist.InputArrayPtr, backProject.OutputArrayPtr, rangeVec, scale);
         }
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCalcBackProject(IntPtr images, IntPtr channels, IntPtr hist, IntPtr dst, IntPtr ranges, double scale);

      /// <summary>
      /// Compares two histograms.
      /// </summary>
      /// <param name="h1">First compared histogram.</param>
      /// <param name="h2">Second compared histogram of the same size as H1 .</param>
      /// <param name="method">Comparison method</param>
      /// <returns>The distance between the histogram</returns>
      public static double CompareHist(IInputArray h1, IInputArray h2, CvEnum.HistogramCompMethod method)
      {
         return cveCompareHist(h1.InputArrayPtr, h2.InputArrayPtr, method);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern double cveCompareHist(IntPtr h1, IntPtr h2, CvEnum.HistogramCompMethod method);

      #endregion

      /// <summary>
      /// Retrieves the spatial moment, which in case of image moments is defined as:
      /// M_{x_order,y_order}=sum_{x,y}(I(x,y) * x^{x_order} * y^{y_order})
      /// where I(x,y) is the intensity of the pixel (x, y). 
      /// </summary>
      /// <param name="moments">The moment state</param>
      /// <param name="xOrder">x order of the retrieved moment, xOrder &gt;= 0. </param>
      /// <param name="yOrder">y order of the retrieved moment, yOrder &gt;= 0 and xOrder + y_order &lt;= 3</param>
      /// <returns>The spatial moment</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetSpatialMoment(
          ref MCvMoments moments,
          int xOrder,
          int yOrder);

      /// <summary>
      /// Retrieves the central moment, which in case of image moments is defined as:
      /// mu_{x_order,y_order}=sum_{x,y}(I(x,y)*(x-x_c)^{x_order} * (y-y_c)^{y_order}),
      /// where x_c=M10/M00, y_c=M01/M00 - coordinates of the gravity center
      /// </summary>
      /// <param name="moments">Reference to the moment state structure</param>
      /// <param name="xOrder">x order of the retrieved moment, xOrder &gt;= 0.</param>
      /// <param name="yOrder">y order of the retrieved moment, yOrder &gt;= 0 and xOrder + y_order &lt;= 3</param>
      /// <returns>The center moment</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetCentralMoment(
          ref MCvMoments moments,
          int xOrder,
          int yOrder);

      /// <summary>
      /// Retrieves normalized central moment, which in case of image moments is defined as:
      /// eta_{x_order,y_order}=mu_{x_order,y_order} / M00^{(y_order+x_order)/2+1},
      /// where mu_{x_order,y_order} is the central moment
      /// </summary>
      /// <param name="moments">Reference to the moment state structure</param>
      /// <param name="xOrder">x order of the retrieved moment, xOrder &gt;= 0.</param>
      /// <param name="yOrder">y order of the retrieved moment, yOrder &gt;= 0 and xOrder + y_order &lt;= 3</param>
      /// <returns>The normalized center moment</returns>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern double cvGetNormalizedCentralMoment(
          ref MCvMoments moments,
          int xOrder,
          int yOrder);

      #region Accumulation of Background Statistics
      /// <summary>
      /// Adds the whole image or its selected region to accumulator sum
      /// </summary>
      /// <param name="src">Input image, 1- or 3-channel, 8-bit or 32-bit floating point. (each channel of multi-channel image is processed independently). </param>
      /// <param name="dst">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point. </param>
      /// <param name="mask">Optional operation mask</param>
      public static void Accumulate(IInputArray src, IInputOutputArray dst, IInputArray mask = null)
      {
         cveAccumulate(src.InputArrayPtr, dst.InputOutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveAccumulate(IntPtr src, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Adds the input <paramref name="src"/> or its selected region, raised to power 2, to the accumulator sqsum
      /// </summary>
      /// <param name="src">Input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently)</param>
      /// <param name="dst">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point</param>
      /// <param name="mask">Optional operation mask</param>
      public static void AccumulateSquare(IInputArray src, IInputOutputArray dst, IInputArray mask = null)
      {
         cveAccumulateSquare(src.InputArrayPtr, dst.InputOutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveAccumulateSquare(IntPtr src, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Adds product of 2 images or thier selected regions to accumulator acc
      /// </summary>
      /// <param name="src1">First input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently)</param>
      /// <param name="src2">Second input image, the same format as the first one</param>
      /// <param name="dst">Accumulator of the same number of channels as input images, 32-bit or 64-bit floating-point</param>
      /// <param name="mask">Optional operation mask</param>
      public static void AccumulateProduct(IInputArray src1, IInputArray src2, IInputOutputArray dst, IInputOutputArray mask = null)
      {
         cveAccumulateProduct(src1.InputArrayPtr, src2.InputArrayPtr, dst.InputOutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputOutputArrayPtr);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveAccumulateProduct(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates weighted sum of input <paramref name="src"/> and the accumulator acc so that acc becomes a running average of frame sequence:
      /// acc(x,y)=(1-<paramref name="alpha"/>) * acc(x,y) + <paramref name="alpha"/> * image(x,y) if mask(x,y)!=0
      /// where <paramref name="alpha"/> regulates update speed (how fast accumulator forgets about previous frames). 
      /// </summary>
      /// <param name="src">Input image, 1- or 3-channel, 8-bit or 32-bit floating point (each channel of multi-channel image is processed independently). </param>
      /// <param name="dst">Accumulator of the same number of channels as input image, 32-bit or 64-bit floating-point. </param>
      /// <param name="alpha">Weight of input image</param>
      /// <param name="mask">Optional operation mask</param>
      public static void AccumulateWeighted(IInputArray src, IInputOutputArray dst, double alpha, IInputArray mask = null)
      {
         cveAccumulateWeighted(src.InputArrayPtr, dst.InputOutputArrayPtr, alpha, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveAccumulateWeighted(IntPtr src, IntPtr dst, double alpha, IntPtr mask);
      #endregion

      /// <summary>
      /// Calculates seven Hu invariants
      /// </summary>
      /// <param name="moments">Pointer to the moment state structure</param>
      /// <param name="huMoments">Pointer to Hu moments structure.</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvGetHuMoments(ref MCvMoments moments, ref MCvHuMoments huMoments);

      /// <summary>
      /// Runs the Harris edge detector on image. Similarly to cvCornerMinEigenVal and cvCornerEigenValsAndVecs, for each pixel it calculates 2x2 gradient covariation matrix M over block_size x block_size neighborhood. Then, it stores
      /// det(M) - k*trace(M)^2
      /// to the destination image. Corners in the image can be found as local maxima of the destination image.
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="harrisResponce">Image to store the Harris detector responces. Should have the same size as image </param>
      /// <param name="blockSize">Neighborhood size </param>
      /// <param name="apertureSize">Aperture parameter for Sobel operator (see cvSobel). format. In the case of floating-point input format this parameter is the number of the fixed float filter used for differencing. </param>
      /// <param name="k">Harris detector free parameter.</param>
      /// <param name="borderType">Pixel extrapolation method.</param>
      public static void CornerHarris(
         IInputArray image,
         IOutputArray harrisResponce,
         int blockSize,
         int apertureSize = 3,
         double k = 0.04, 
         CvEnum.BorderType borderType = CvEnum.BorderType.Default)
      {
         cveCornerHarris(image.InputArrayPtr, harrisResponce.OutputArrayPtr, blockSize, apertureSize, k, borderType);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCornerHarris(
          IntPtr image,
          IntPtr harrisResponce,
          int blockSize,
          int apertureSize,
          double k, CvEnum.BorderType borderType);

      /// <summary>
      /// Iterates to find the sub-pixel accurate location of corners, or radial saddle points
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="corners">Initial coordinates of the input corners and refined coordinates on output</param>
      /// <param name="win">Half sizes of the search window. For example, if win=(5,5) then 5*2+1 x 5*2+1 = 11 x 11 search window is used</param>
      /// <param name="zeroZone">Half size of the dead region in the middle of the search zone over which the summation in formulae below is not done. It is used sometimes to avoid possible singularities of the autocorrelation matrix. The value of (-1,-1) indicates that there is no such size</param>
      /// <param name="criteria">Criteria for termination of the iterative process of corner refinement. That is, the process of corner position refinement stops either after certain number of iteration or when a required accuracy is achieved. The criteria may specify either of or both the maximum number of iteration and the required accuracy</param>
      public static void CornerSubPix(
         IInputArray image,
         IInputOutputArray corners,
         Size win,
         Size zeroZone,
         MCvTermCriteria criteria)
      {
         cveCornerSubPix(image.InputArrayPtr, corners.InputOutputArrayPtr, ref win, ref zeroZone, ref criteria);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCornerSubPix(
         IntPtr image,
         IntPtr corners,
         ref Size win,
         ref Size zeroZone,
         ref MCvTermCriteria criteria);

      /// <summary>
      /// Calculates one or more integral images for the source image 
      /// Using these integral images, one may calculate sum, mean, standard deviation over arbitrary up-right or rotated rectangular region of the image in a constant time.
      /// It makes possible to do a fast blurring or fast block correlation with variable window size etc. In case of multi-channel images sums for each channel are accumulated independently. 
      /// </summary>
      /// <param name="image">The source image, WxH, 8-bit or floating-point (32f or 64f) image.</param>
      /// <param name="sum">The integral image, W+1xH+1, 32-bit integer or double precision floating-point (64f). </param>
      /// <param name="sqsum">The integral image for squared pixel values, W+1xH+1, double precision floating-point (64f). </param>
      /// <param name="tiltedSum">The integral for the image rotated by 45 degrees, W+1xH+1, the same data type as sum.</param>
      /// <param name="sdepth">Desired depth of the integral and the tilted integral images, CV_32S, CV_32F, or CV_64F.</param>
      /// <param name="sqdepth">Desired depth of the integral image of squared pixel values, CV_32F or CV_64F.</param>
      public static void Integral(
         IInputArray image,
         IOutputArray sum,
         IOutputArray sqsum = null,
         IOutputArray tiltedSum = null, 
         CvEnum.DepthType sdepth = CvEnum.DepthType.Default, 
         CvEnum.DepthType sqdepth = CvEnum.DepthType.Default)
      {
         cveIntegral(image.InputArrayPtr, sum.OutputArrayPtr, 
            sqsum == null ? IntPtr.Zero : sqsum.OutputArrayPtr, 
            tiltedSum == null ? IntPtr.Zero : tiltedSum.OutputArrayPtr, 
            sdepth, sqdepth);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveIntegral(
         IntPtr image,
         IntPtr sum,
         IntPtr sqsum,
         IntPtr tiltedSum, 
         CvEnum.DepthType sdepth, 
         CvEnum.DepthType sqdepth);

      /// <summary>
      /// Calculates distance to closest zero pixel for all non-zero pixels of source image
      /// </summary>
      /// <param name="src">Source 8-bit single-channel (binary) image.</param>
      /// <param name="dst">Output image with calculated distances (32-bit floating-point, single-channel). </param>
      /// <param name="distanceType">Type of distance</param>
      /// <param name="maskSize">Size of distance transform mask; can be 3 or 5.
      /// In case of CV_DIST_L1 or CV_DIST_C the parameter is forced to 3, because 3x3 mask gives the same result as 5x5 yet it is faster.</param>
      /// <param name="userMask">User-defined mask in case of user-defined distance.
      /// It consists of 2 numbers (horizontal/vertical shift cost, diagonal shift cost) in case of 3x3 mask
      /// and 3 numbers (horizontal/vertical shift cost, diagonal shift cost, knights move cost) in case of 5x5 mask.</param>
      /// <param name="labels">The optional output 2d array of labels of integer type and the same size as src and dst.</param>
      [DllImport(OpencvImgprocLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern void cvDistTransform(
         IntPtr src,
         IntPtr dst,
         CvEnum.DistType distanceType,
         int maskSize,
         float[] userMask,
         IntPtr labels);

      /// <summary>
      /// Fills a connected component with given color.
      /// </summary>
      /// <param name="src">Input 1- or 3-channel, 8-bit or floating-point image. It is modified by the function unless CV_FLOODFILL_MASK_ONLY flag is set.</param>
      /// <param name="seedPoint">The starting point.</param>
      /// <param name="newVal">New value of repainted domain pixels.</param>
      /// <param name="loDiff">Maximal lower brightness/color difference
      /// between the currently observed pixel and one of its neighbor belong to the component
      /// or seed pixel to add the pixel to component.
      /// In case of 8-bit color images it is packed value.</param>
      /// <param name="upDiff">Maximal upper brightness/color difference
      /// between the currently observed pixel and one of its neighbor belong to the component
      /// or seed pixel to add the pixel to component.
      /// In case of 8-bit color images it is packed value.</param>
      /// <param name="flags">The operation flags.
      /// Lower bits contain connectivity value, 4 (by default) or 8, used within the function.
      /// Connectivity determines which neighbors of a pixel are considered.
      /// Upper bits can be 0 or combination of the following flags:
      /// CV_FLOODFILL_FIXED_RANGE - if set the difference between the current pixel and seed pixel is considered,
      /// otherwise difference between neighbor pixels is considered (the range is floating).
      /// CV_FLOODFILL_MASK_ONLY - if set, the function does not fill the image (new_val is ignored),
      /// but the fills mask (that must be non-NULL in this case). </param>
      /// <param name="mask">Operation mask,
      /// should be singe-channel 8-bit image, 2 pixels wider and 2 pixels taller than image.
      /// If not IntPtr.Zero, the function uses and updates the mask, so user takes responsibility of initializing mask content.
      /// Floodfilling can't go across non-zero pixels in the mask, for example, an edge detector output can be used as a mask to stop filling at edges.
      /// Or it is possible to use the same mask in multiple calls to the function to make sure the filled area do not overlap.
      /// Note: because mask is larger than the filled image, pixel in mask that corresponds to (x,y) pixel in image will have coordinates (x+1,y+1).</param>
      /// <param name="rect">Output parameter set by the function to the minimum bounding rectangle of the repainted domain.</param>
      /// <param name="connectivity">Flood fill connectivity</param>
      public static int FloodFill(
         IInputOutputArray src,
         IInputOutputArray mask,
         Point seedPoint,
         MCvScalar newVal,
         out Rectangle rect,
         MCvScalar loDiff,
         MCvScalar upDiff,
         CvEnum.Connectivity connectivity = CvEnum.Connectivity.FourConnected,
         CvEnum.FloodFillType flags = CvEnum.FloodFillType.Default)
      {
         rect = new Rectangle();
         return cveFloodFill(
            src.InputOutputArrayPtr, 
            mask == null ? IntPtr.Zero : mask.InputOutputArrayPtr, 
            ref seedPoint, ref newVal, 
            ref rect,
            ref loDiff, ref upDiff, (int)connectivity | (int)flags);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int cveFloodFill(
         IntPtr src,
         IntPtr mask,
         ref Point seedPoint,
         ref MCvScalar newVal,
         ref Rectangle rect,
         ref MCvScalar loDiff,
         ref MCvScalar upDiff,
         int flags);

      /// <summary>
      /// Filters image using meanshift algorithm
      /// </summary>
      /// <param name="src">Source image</param>
      /// <param name="dst">Result image</param>
      /// <param name="sp">The spatial window radius.</param>
      /// <param name="sr">The color window radius.</param>
      /// <param name="maxLevel">Maximum level of the pyramid for the segmentation. Use 1 as default value</param>
      /// <param name="termcrit">Termination criteria: when to stop meanshift iterations. Use new MCvTermCriteria(5, 1) as default value</param>
      public static void PyrMeanShiftFiltering(
         IInputArray src, IOutputArray dst,
         double sp, double sr, int maxLevel,
         MCvTermCriteria termcrit)
      {
         cvePyrMeanShiftFiltering(src.InputArrayPtr, dst.OutputArrayPtr, sp, sr, maxLevel, ref termcrit);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvePyrMeanShiftFiltering(
         IntPtr src, IntPtr dst,
         double sp, double sr, int maxLevel,
         ref MCvTermCriteria termcrit);

      #region image undistortion
      /// <summary>
      /// Converts image transformation maps from one representation to another.
      /// </summary>
      /// <param name="map1">The first input map of type CV_16SC2 , CV_32FC1 , or CV_32FC2 .</param>
      /// <param name="map2">The second input map of type CV_16UC1 , CV_32FC1 , or none (empty matrix), respectively.</param>
      /// <param name="dstmap1">The first output map that has the type dstmap1type and the same size as src .</param>
      /// <param name="dstmap2">The second output map.</param>
      /// <param name="dstmap1Depth">Depth type of the first output map that should be CV_16SC2 , CV_32FC1 , or CV_32FC2.</param>
      /// <param name="dstmap1Channels">The number of channels in the dst map.</param>
      /// <param name="nninterpolation">Flag indicating whether the fixed-point maps are used for the nearest-neighbor or for a more complex interpolation.</param>
      public static void ConvertMaps(IInputArray map1, IInputArray map2, IOutputArray dstmap1, IOutputArray dstmap2, CvEnum.DepthType dstmap1Depth, int dstmap1Channels, bool nninterpolation = false)
      {
         cveConvertMaps(map1.InputArrayPtr, map2 == null ? IntPtr.Zero : map2.InputArrayPtr, dstmap1.OutputArrayPtr, dstmap2 == null ? IntPtr.Zero : dstmap2.OutputArrayPtr, CvInvoke.MakeType(dstmap1Depth, dstmap1Channels), nninterpolation);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveConvertMaps(
         IntPtr map1, IntPtr map2,
         IntPtr dstmap1, IntPtr dstmap2,
         int dstmap1Type,
         [MarshalAs(CvInvoke.BoolMarshalType)] 
         bool nninterpolation);

      /// <summary>
      /// Transforms the image to compensate radial and tangential lens distortion. 
      /// </summary>
      /// <param name="src">The input (distorted) image</param>
      /// <param name="dst">The output (corrected) image</param>
      /// <param name="cameraMatrix">The camera matrix (A) [fx 0 cx; 0 fy cy; 0 0 1].</param>
      /// <param name="distortionCoeffs">The vector of distortion coefficients, 4x1 or 1x4 [k1, k2, p1, p2].</param>
      /// <param name="newCameraMatrix">Camera matrix of the distorted image. By default it is the same as cameraMatrix, but you may additionally scale and shift the result by using some different matrix</param>
      public static void Undistort(
         IInputArray src,
         IOutputArray dst,
         IInputArray cameraMatrix,
         IInputArray distortionCoeffs,
         IInputArray newCameraMatrix = null)
      {
         cveUndistort(
            src.InputArrayPtr,
            dst.OutputArrayPtr,
            cameraMatrix.InputArrayPtr,
            distortionCoeffs.InputArrayPtr,
            newCameraMatrix == null ? IntPtr.Zero : newCameraMatrix.InputArrayPtr);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveUndistort(
          IntPtr src,
          IntPtr dst,
          IntPtr cameraMatrix,
          IntPtr distortionCoeffs,
          IntPtr newCameraMatrix);

      /// <summary>
      /// This function is an extended version of cvInitUndistortMap. That is, in addition to the correction of lens distortion, the function can also apply arbitrary perspective transformation R and finally it can scale and shift the image according to the new camera matrix
      /// </summary>
      /// <param name="cameraMatrix">The camera matrix A=[fx 0 cx; 0 fy cy; 0 0 1]</param>
      /// <param name="distCoeffs">The vector of distortion coefficients, 4x1, 1x4, 5x1 or 1x5</param>
      /// <param name="R">The rectification transformation in object space (3x3 matrix). R1 or R2, computed by cvStereoRectify can be passed here. If the parameter is IntPtr.Zero, the identity matrix is used</param>
      /// <param name="newCameraMatrix">The new camera matrix A'=[fx' 0 cx'; 0 fy' cy'; 0 0 1]</param>
      /// <param name="depthType">Depth type of the first output map that can be CV_32FC1 or CV_16SC2 .</param>
      /// <param name="map1">The first output map.</param>
      /// <param name="map2">The second output map.</param>
      /// <param name="size">Undistorted image size.</param>
      public static void InitUndistortRectifyMap(
         IInputArray cameraMatrix,
         IInputArray distCoeffs,
         IInputArray R,
         IInputArray newCameraMatrix,
         Size size, 
         CvEnum.DepthType depthType,
         IOutputArray map1,
         IOutputArray map2 = null)
      {
         int channels = map2 == null ? 2 : 1;
         cveInitUndistortRectifyMap(
            cameraMatrix.InputArrayPtr,
            distCoeffs.InputArrayPtr,
            R == null ? IntPtr.Zero : R.InputArrayPtr,
            newCameraMatrix.InputArrayPtr,
            ref size,
            CvInvoke.MakeType(depthType, channels),
            map1.OutputArrayPtr,
            map2 == null ? IntPtr.Zero : map2.OutputArrayPtr);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveInitUndistortRectifyMap(
         IntPtr cameraMatrix,
         IntPtr distCoeffs,
         IntPtr R,
         IntPtr newCameraMatrix,
         ref Size size,
         int m1type,
         IntPtr map1,
         IntPtr map2);

      /// <summary>
      /// Similar to cvInitUndistortRectifyMap and is opposite to it at the same time. 
      /// The functions are similar in that they both are used to correct lens distortion and to perform the optional perspective (rectification) transformation. 
      /// They are opposite because the function cvInitUndistortRectifyMap does actually perform the reverse transformation in order to initialize the maps properly, while this function does the forward transformation. 
      /// </summary>
      /// <param name="src">The observed point coordinates</param>
      /// <param name="dst">The ideal point coordinates, after undistortion and reverse perspective transformation. </param>
      /// <param name="cameraMatrix">The camera matrix A=[fx 0 cx; 0 fy cy; 0 0 1]</param>
      /// <param name="distCoeffs">The vector of distortion coefficients, 4x1, 1x4, 5x1 or 1x5. </param>
      /// <param name="R">The rectification transformation in object space (3x3 matrix). R1 or R2, computed by cvStereoRectify can be passed here. If the parameter is IntPtr.Zero, the identity matrix is used.</param>
      /// <param name="P">The new camera matrix (3x3) or the new projection matrix (3x4). P1 or P2, computed by cvStereoRectify can be passed here. If the parameter is IntPtr.Zero, the identity matrix is used.</param>
      public static void UndistortPoints(
         IInputArray src,
         IOutputArray dst,
         IInputArray cameraMatrix,
         IInputArray distCoeffs,
         IInputArray R = null,
         IInputArray P = null)
      {
         cveUndistortPoints(
            src.InputArrayPtr,
            dst.OutputArrayPtr,
            cameraMatrix.InputArrayPtr,
            distCoeffs.InputArrayPtr,
            R == null ? IntPtr.Zero : R.InputArrayPtr,
            P == null ? IntPtr.Zero : P.InputArrayPtr);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveUndistortPoints(
         IntPtr src,
         IntPtr dst,
         IntPtr cameraMatrix,
         IntPtr distCoeffs,
         IntPtr R,
         IntPtr P);
      #endregion

      #region EMD
      /// <summary>
      /// Computes the 'minimal work' distance between two weighted point configurations.
      /// </summary>
      /// <param name="signature1">First signature, a size1 x dims + 1  floating-point matrix. Each row stores the point weight followed by the point coordinates. The matrix is allowed to have a single column (weights only) if the user-defined cost matrix is used.</param>
      /// <param name="signature2">Second signature of the same format as signature1 , though the number of rows may be different. The total weights may be different. In this case an extra 'dummy' point is added to either signature1 or signature2</param>
      /// <param name="distType"> Used metric. CV_DIST_L1, CV_DIST_L2 , and CV_DIST_C stand for one of the standard metrics. CV_DIST_USER means that a pre-calculated cost matrix cost is used.</param>
      /// <param name="cost">User-defined size1 x size2 cost matrix. Also, if a cost matrix is used, lower boundary lowerBound cannot be calculated because it needs a metric function.</param>
      /// <param name="lowerBound">Optional input/output parameter: lower boundary of a distance between the two signatures that is a distance between mass centers. The lower boundary may not be calculated if the user-defined cost matrix is used, the total weights of point configurations are not equal, or if the signatures consist of weights only (the signature matrices have a single column). </param>
      /// <param name="flow"> Resultant size1 x size2 flow matrix</param>
      /// <returns>The 'minimal work' distance between two weighted point configurations.</returns>
      public static float EMD(IInputArray signature1, IInputArray signature2, CvEnum.DistType distType, IInputArray cost = null, float[] lowerBound = null, IOutputArray flow = null)
      {
         IntPtr lowerBoundPtr = IntPtr.Zero;
         GCHandle lbHandle;
         if (lowerBound == null)
         {
            lbHandle = GCHandle.Alloc(lowerBound, GCHandleType.Pinned);
            lowerBoundPtr = lbHandle.AddrOfPinnedObject();
         }
         else
            lbHandle = new GCHandle();
         try
         {
            return cveEMD(
               signature1.InputArrayPtr, signature2.InputArrayPtr, distType,
               cost == null ? IntPtr.Zero : cost.InputArrayPtr,
               lowerBoundPtr,
               flow == null ? IntPtr.Zero : flow.OutputArrayPtr);
         }
         finally
         {
            if (lowerBound == null)
            {
               lbHandle.Free();
            }
         }
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern float cveEMD(
         IntPtr signature1,
         IntPtr signature2,
         CvEnum.DistType distType,
         IntPtr cost,
         IntPtr lowerBound,
         IntPtr flow);
      #endregion

      /// <summary>
      /// The function is used to detect translational shifts that occur between two images. The operation takes advantage of the Fourier shift theorem for detecting the translational shift in the frequency domain. It can be used for fast image registration as well as motion estimation. 
      /// </summary>
      /// <param name="src1">Source floating point array (CV_32FC1 or CV_64FC1)</param>
      /// <param name="src2">Source floating point array (CV_32FC1 or CV_64FC1)</param>
      /// <param name="window">Floating point array with windowing coefficients to reduce edge effects (optional).</param>
      /// <param name="response">Signal power within the 5x5 centroid around the peak, between 0 and 1 </param>
      /// <returns>The translational shifts that occur between two images</returns>
      public static MCvPoint2D64f PhaseCorrelate(IInputArray src1, IInputArray src2, IInputArray window, out double response)
      {
         MCvPoint2D64f resultPt = new MCvPoint2D64f();
         response = 0;
         cvePhaseCorrelate(src1.InputArrayPtr, src2.InputArrayPtr, window == null ? IntPtr.Zero : window.InputArrayPtr, ref response, ref resultPt);
         return resultPt;
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cvePhaseCorrelate(IntPtr src1, IntPtr src2, IntPtr window, ref double response, ref MCvPoint2D64f result);

      /// <summary>
      /// This function computes a Hanning window coefficients in two dimensions.
      /// </summary>
      /// <param name="dst">Destination array to place Hann coefficients in</param>
      /// <param name="winSize">The window size specifications</param>
      /// <param name="type">Created array type</param>
      public static void CreateHanningWindow(IOutputArray dst, Size winSize, CvEnum.DepthType type)
      {
         cveCreateHanningWindow(dst.OutputArrayPtr, ref winSize, type);
      }
      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveCreateHanningWindow(IntPtr dst, ref Size winSize, CvEnum.DepthType type);
   }
}
