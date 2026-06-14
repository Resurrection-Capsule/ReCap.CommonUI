using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Util.OperatingSystem.Win32
{
    internal static partial class StructExtensions
    {
        public static IntPtr ToWMParam(this POINT point)
        {
            //https://social.msdn.microsoft.com/Forums/vstudio/en-US/d9965d14-34ac-48ee-ae4f-85cee689cc33/how-to-make-lparam?forum=vbgeneral
            
            var retList = new List<byte>();
            byte[] xByte = BitConverter.GetBytes(point.X);
            byte[] yByte = BitConverter.GetBytes(point.Y);
            retList.Add(xByte[0]);
            retList.Add(xByte[1]);
            retList.Add(yByte[0]);
            retList.Add(yByte[1]);
            return new IntPtr(BitConverter.ToInt32(retList.ToArray(), 0));
        }




        public static void Offset(this RECT rect, POINT pt)
            => rect.Offset(pt.X, pt.Y);
        public static void Offset(this RECT rect, int x, int y)
        {
            rect.left += x;
            rect.top += y;
            rect.right += x;
            rect.bottom += y;
        }
        public static void Inflate(this RECT rect, int amount)
            => rect.Inflate(amount, amount, amount, amount);
        public static void Inflate(this RECT rect, int left, int top, int right, int bottom)
        {
            rect.left -= left;
            rect.top -= top;
            rect.right += right;
            rect.bottom += bottom;
        }

        public static void Deflate(this RECT rect, int amount)
            => rect.Inflate(-amount);
        public static void Deflate(this RECT rect, int left, int top, int right, int bottom)
            => rect.Inflate(-left, -top, -right, -bottom);


        public static bool Contains(this RECT rect, POINT point)
            => rect.Contains(point.X, point.Y);
        public static bool Contains(this RECT rect, int x, int y)
            => (x >= rect.left)
            && (y >= rect.top)
            && (x < rect.right)
            && (y < rect.bottom)
        ;




        public static bool IsIcon(this ICONINFO iconInfo)
            => iconInfo.fIcon;
        public static bool IsCursor(this ICONINFO iconInfo)
            => !iconInfo.fIcon;
    }
}