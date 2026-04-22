using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;

namespace ReCap.CommonUI.Util
{
    internal static class PixelSizeRounding
    {
        static PixelSize Dummy_getSize(PixelSize pxSize)
            => pxSize;
        public static bool TryGetNearest(PixelSize to, IEnumerable<PixelSize> among, out PixelSize result)
            => TryGetNearest(to, among, Dummy_getSize, out result);




        public static bool TryGetNearest<T>(T to, IEnumerable<T> among, Func<T, PixelSize> getSize, out T result)
            => TryGetNearest(getSize(to), among, getSize, out result);
        public static bool TryGetNearest<T>(PixelSize to, IEnumerable<T> among, Func<T, PixelSize> getSize, out T result)
        {
            try
            {
                result = GetNearest(to, among, getSize);
                return result != null;
            }
            catch (ArgumentException)
            {
                result = default;
                return false;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }



        public static T GetNearest<T>(T to, IEnumerable<T> among, Func<T, PixelSize> getSize)
        {
            if (to == null)
                throw new ArgumentNullException(paramName: nameof(to));
            return GetNearest(getSize(to), among, getSize);
        }
        static T GetNearest<T>(PixelSize to, IEnumerable<T> among, Func<T, PixelSize> getSize)
        {
            if (among == null)
                throw new ArgumentNullException(paramName: nameof(among));
            if (getSize == null)
                throw new ArgumentNullException(paramName: nameof(getSize));

            int count = among.Count();
            if (count <= 0)
                throw new ArgumentException("Empty collection!??", paramName: nameof(among));

            int nearestIndex = GetNearestIndex(to, among, getSize, 0, count - 1);
            return among.ElementAt(nearestIndex);
        }


        /*
        https://stackoverflow.com/questions/3498968/find-the-nearest-dot-in-a-2d-space
        https://stackoverflow.com/a/3499125
        https://stackoverflow.com/questions/3498968/find-the-nearest-dot-in-a-2d-space/3499125#3499125
        */
        static int GetNearestIndex<T>(PixelSize d, IEnumerable<T> among, Func<T, PixelSize> getSize, int low, int high)
        {
            if (low > high)
                return -1;
            if (low == high)
                return low;

            int middle = low + (high - low) >> 1;

            int idx1 = GetNearestIndex(d, among, getSize, low, middle);
            int idx2 = GetNearestIndex(d, among, getSize, middle + 1, high);

            if (idx1 < 0)
                return idx2;
            else if (idx2 < 0)
                return idx1;
            else
                return NearerIndex(d, idx1, idx2, among, getSize);
        }




        static int NearerIndex<T>(PixelSize d, int idx1, int idx2, IEnumerable<T> among, Func<T, PixelSize> getSize)
        {
            bool has1 = idx1 >= 0;
            bool has2 = idx2 >= 0;
            if (has1 && has2)
                return NearerIndexCompare(d, idx1, idx2, among, getSize);
            else if (has1)
                return idx1;
            else if (has2)
                return idx2;
            else
                return -1;
        }
        static int NearerIndexCompare<T>(PixelSize d, int idx1, int idx2, IEnumerable<T> among, Func<T, PixelSize> getSize)
        {
            T obj1 = among.ElementAt(idx1);
            T obj2 = among.ElementAt(idx2);
            PixelSize size1 = getSize(obj1);
            PixelSize size2 = getSize(obj2);
            if (DistanceBetween(d, size1) <= DistanceBetween(d, size2))
                return idx1;
            else
                return idx2;
        }


        static double DistanceBetween(PixelSize a, PixelSize b)
            => DistanceBetween(a.Width, a.Height, b.Width, b.Height);
        static double DistanceBetween(double x1, double y1, double x2, double y2)
            => Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }
}