using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public static class SampleItemFactory
    {
        public static IEnumerable<T> CreateSampleItems<T>(
            int enabledCount, string enabledTitleFormat = "Item {0}"
            , int disabledCount = 1
        )
            where T
                : SampleItemViewModel
                , new()
            => CreateSampleItems<T>(
                enabledCount, enabledTitleFormat
                , disabledCount, $"{enabledTitleFormat} but disabled"
            );

        public static IEnumerable<T> CreateSampleItems<T>(
            int enabledCount, string enabledTitleFormat,
            int disabledCount, string disabledTitleFormat
        )
            where T
                : SampleItemViewModel
                , new()
        {
            List<T> items = new();

            CreateSampleItemsInternal(ref items, 0, enabledCount, enabledTitleFormat, CreateEnabledItem<T>);
            CreateSampleItemsInternal(ref items, enabledCount, disabledCount, disabledTitleFormat, CreateDisabledItem<T>);

            return items;
        }


        static T CreateEnabledItem<T>(string title)
            where T
                : SampleItemViewModel
                , new()
            => new()
            {
                Title = title,
                IsEnabled = true,
            };

        static T CreateDisabledItem<T>(string title)
            where T
                : SampleItemViewModel
                , new()
            => new()
            {
                Title = title,
                IsEnabled = false,
            };


        static void CreateSampleItemsInternal<T>(
            ref List<T> items
            , int start, int count
            , string titleFormat
            , Func<string, T> createItem
        )
            where T
                : SampleItemViewModel
        {
            for (int i = 0; i < count; i++)
            {
                string title = string.Format(titleFormat, i + start);
                T item = createItem(title);
                items.Add(item);
            }
        }
    }
}