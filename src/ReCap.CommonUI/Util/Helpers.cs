using System;

namespace ReCap.CommonUI.Util
{
    public static class Helpers
    {
        public static int RoundToInt(double d)
            => (int)Math.Round(d);

        public static int RoundToInt(float f)
            => (int)Math.Round(f);

        public static int RoundToInt(decimal m)
            => (int)Math.Round(m);



        public static T[] LonerArray<T>(T item)
            => new[]
            {
                item
            };
    }
}