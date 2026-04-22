using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Reactive;

using ConversionFunc = System.Func<object, object>;

namespace ReCap.CommonUI.Converters
{
    public sealed class ResourcesToResourceObservableConverter
        : IValueConverter
    {
        public static readonly ResourcesToResourceObservableConverter Instance = new();
        private ResourcesToResourceObservableConverter()
        {}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not IResourceHost resHost)
                goto fail;
            /*
            else if (TryGetRetrievalInfo(parameter, out ResourceObservableRetrievalInfo info))
                return resHost.GetResourceObservable(info.ResourceKey, info.Converter);
            */
            ResourceObservableRetrievalInfo info = default;
            if (parameter is ResourceObservableRetrievalInfo retrievalInfo)
                info = retrievalInfo;
            else if (parameter != null)
                info = new(parameter);
            else
                goto fail;

            return resHost.GetResourceObservable(info.ResourceKey, info.Converter);


            fail:
            return BindingOperations.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();


        static bool TryGetRetrievalInfo(object parameter, out ResourceObservableRetrievalInfo info)
        {
            if (parameter is ResourceObservableRetrievalInfo retrievalInfo)
            {
                info = retrievalInfo;
                return true;
            }
            else if (parameter != null)
            {
                info = new(parameter);
                return true;
            }
            else
            {
                info = default;
                return false;
            }
        }
    }


    public readonly struct ResourceObservableRetrievalInfo
    {
        public object ResourceKey
        {
            get;
            init;
        }


        public ConversionFunc Converter
        {
            get;
            init;
        }


        public ResourceObservableRetrievalInfo(object key, ConversionFunc converter = null)
        {
            ResourceKey = key;
            Converter = converter;
        }
        /*
        public ResourceObservableRetrievalInfo(object key, IValueConverter converter, Type targetType, object parameter, CultureInfo culture)
        {
            ResourceKey = key;
            Converter = o => converter.Convert(o, targetType, parameter, culture);
        }
        */
        public ResourceObservableRetrievalInfo(object key, IValueConverter converter, Type targetType, object parameter, CultureInfo culture)
            : this(key, ValueConverterToConversionFunc(converter, targetType, parameter, culture))
        {}


        /*
        public ResourceObservableRetrievalInfo(object key)
            : this(key, null)
        {}
        static object NoConversion(object value)
            => value;
        */




        public static ConversionFunc ValueConverterToConversionFunc(IValueConverter converter, Type targetType, object parameter, CultureInfo culture)
            => value => converter.Convert(value, targetType, parameter, culture);
    }
}