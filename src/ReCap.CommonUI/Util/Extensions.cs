using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Platform;
using ReCap.CommonUI.Util.Win32;
using SColl = System.Collections;

namespace ReCap.CommonUI.Util
{
    public static class Extensions
    {
        public static bool TryGetPlatformHandle(this Window window, out IPlatformHandle platformHandle)
        {
            var platHandle = window.TryGetPlatformHandle();
            if (platHandle != null)
            {
                platformHandle = platHandle;
                return true;
            }
            else
            {
                platformHandle = default;
                return false;
            }
        }


        public static IntPtr GetHWnd(this Window window)
        {
            if (window.TryGetHWnd(out IntPtr hWnd))
                return hWnd;
            else
                throw new Exception($"{nameof(window)} hWndn't!??");
        }
        public static bool TryGetHWnd(this Window window, out IntPtr hWnd)
        {
            if (window == null)
                goto fail;

            if (!window.TryGetPlatformHandle(out IPlatformHandle platHandle))
                goto fail;
            
            IntPtr? hWndMaybe = platHandle.Handle;
            if (!hWndMaybe.HasValue)
                goto fail;


            hWnd = hWndMaybe.Value;
            return true;


            fail:
            hWnd = IntPtr.Zero;
            return false;
        }

        public static void MakeControlTypeNonInteractive<T>()
            where T : AvaloniaObject
        {
            InputElement.FocusableProperty.OverrideDefaultValue<T>(false);
            InputElement.IsTabStopProperty.OverrideDefaultValue<T>(false);
            ContentPresenter.RecognizesAccessKeyProperty.OverrideDefaultValue<T>(false);
        }

        public static IEnumerable<T> AppendRange<T>(this IEnumerable<T> appendTo, IEnumerable<T> appendFrom)
            => appendTo.AppendRange(appendFrom.ToArray());

        public static IEnumerable<T> AppendRange<T>(this IEnumerable<T> appendTo, params T[] appendFrom)
        {
            var ret = appendTo.ToList();
            ret.AddRange(appendFrom);
            return ret;
        }




        public static bool TryGetOldValue<T>(this AvaloniaPropertyChangedEventArgs e, out T oldValue)
        {
            if (e == null)
            {
                oldValue = default;
                return false;
            }

            oldValue = e.GetOldValue<T>();
            return true;
        }

        public static bool TryGetNewValue<T>(this AvaloniaPropertyChangedEventArgs e, out T newValue)
        {
            if (e == null)
            {
                newValue = default;
                return false;
            }

            newValue = e.GetNewValue<T>();
            return true;
        }

        public static bool TryGetOldAndNewValue<T>(this AvaloniaPropertyChangedEventArgs e, out T oldValue, out T newValue)
        {
            bool oldRet = e.TryGetOldValue(out oldValue);
            bool newRet = e.TryGetNewValue(out newValue);
            return oldRet && newRet;
        }



        internal static bool TryFind<T>(this INameScope nameScope, string name, out T result)
            where T : class
        {
            result = nameScope.Find<T>(name);
            return result != null;
        }




        internal static IDisposable BindToProperty(this AvaloniaObject o, AvaloniaProperty bindMe, object source, AvaloniaProperty bindTo, BindingMode mode)
        {
            Binding binding = new(bindTo.Name, mode)
            {
                //Mode = mode,
                Source = source
            };

            return o.Bind(bindMe, binding);
        }


        /*
        static readonly Type _SCOLL_GEN_ICOLLECTION = typeof(ICollection<>);
        static readonly Type _SCOLL_GEN_IENUMERABLE = typeof(IEnumerable<>);
        */
        // https://stevefenton.co.uk/blog/2021/06/counting-a-non-generic-ienumerable/
        // https://reddit.com/r/csharp/comments/f2t0hw/how_can_i_cast_to_generic_type_without_knowing_t/
        internal static int Count(this SColl.IEnumerable enumerable)
        {
            /*
            Type type = enumerable.GetType();
            if (type.IsAssignableTo(_SCOLL_GEN_ICOLLECTION))
            {
                // Evil and intimidating hack
                dynamic collG = enumerable;
                return collG.Count;
            }
            else if (type.IsAssignableTo(_SCOLL_GEN_IENUMERABLE))
            {
                // Evil and intimidating hack
                dynamic enumerableG = enumerable;
                return Enumerable.Count(enumerableG);
            }
            else */if (enumerable is SColl.ICollection collection)
            {
                return collection.Count;
            }
            else
            {
                int count = 0;
                var enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    count++;
                }
                return count;
            }
        }


        internal static Avalonia.Media.Imaging.Bitmap ToBitmap(this WindowIcon icon, PixelSize pxSize)
        {
            if (icon == null)
                return null;

            using System.IO.MemoryStream iconStream = new();
            icon.Save(iconStream);
            iconStream.Position = 0;
            var image = System.Drawing.Image.FromStream(iconStream);
            return new(iconStream);
        }




        public static int RoundToInt(double d)
            => (int)Math.Round(d);

        public static int RoundToInt(float f)
            => (int)Math.Round(f);

        public static int RoundToInt(decimal m)
            => (int)Math.Round(m);




        public static PixelSize ToPixelSize(this PixelPoint pxPoint)
            => new(pxPoint.X, pxPoint.Y);

        public static PixelPoint ToPixelPoint(this PixelSize pxSize)
            => new(pxSize.Width, pxSize.Height);

        public static Size ToSize(this Point point)
            => new(point.X, point.Y);

        public static Point ToPoint(this Size size)
            => new(size.Width, size.Height);




        public static IEnumerable<double> ToDoubles(this Thickness thickness)
            => new[]
            {
                thickness.Left,
                thickness.Top,
                thickness.Right,
                thickness.Bottom,
            };


        public static IEnumerable<double> ToDoubles(this Point point)
            => new[]
            {
                point.X,
                point.Y,
            };


        public static IEnumerable<int> ToInts(this PixelPoint pxPoint)
            => new[]
            {
                pxPoint.X,
                pxPoint.Y,
            };


        public static IEnumerable<double> ToDoubles(this Size size)
            => new[]
            {
                size.Width,
                size.Height,
            };


        public static IEnumerable<int> ToInts(this PixelSize pxSize)
            => new[]
            {
                pxSize.Width,
                pxSize.Height,
            };


        public static IEnumerable<double> ToDoubles(this Rect rect, RectToNumbersMode mode)
            => mode switch
            {
                RectToNumbersMode.LTRB => new[]
                {
                    rect.Left,
                    rect.Top,
                    rect.Right,
                    rect.Bottom,
                },
                _ => new[] //RectToDoublesMode.XYWH
                {
                    rect.X,
                    rect.Y,
                    rect.Width,
                    rect.Height,
                },
            };


        public static IEnumerable<int> ToInts(this PixelRect pxRect, RectToNumbersMode mode)
        {
            switch (mode)
            {
                case RectToNumbersMode.LTRB:
                {
                    // PixelRect doesn't have .Top or .Left for some reason
                    var topLeft = pxRect.TopLeft;
                    return new[]
                    {
                        topLeft.X,
                        topLeft.Y,
                        pxRect.Right,
                        pxRect.Bottom,
                    };
                }
                case RectToNumbersMode.XYWH:
                {
                    goto default;
                }


                default:
                {
                    return new[]
                    {
                        pxRect.X,
                        pxRect.Y,
                        pxRect.Width,
                        pxRect.Height,
                    };
                }
            };
        }
    }


    public enum RectToNumbersMode
    {
        XYWH,
        LTRB,
    }
}