using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public class SampleItemViewModel
        : ViewModelBase
    {
        public static IEnumerable<SampleItemViewModel> CreateSampleItems(
            int enabledCount, string enabledTitleFormat = "Item {0}"
            , int disabledCount = 1
        )
            => CreateSampleItems(
                enabledCount, enabledTitleFormat
                , disabledCount, $"{enabledTitleFormat} but disabled"
            );


        static SampleItemViewModel CreateEnabledItem(string title)
            => new()
            {
                Title = title,
                Enabled = true,
            };

        static SampleItemViewModel CreateDisabledItem(string title)
            => new()
            {
                Title = title,
                Enabled = false,
            };

        public static IEnumerable<SampleItemViewModel> CreateSampleItems(
            int enabledCount, string enabledTitleFormat,
            int disabledCount, string disabledTitleFormat
        )
        {
            List<SampleItemViewModel> items = new();

            CreateSampleItemsInternal(ref items, 0, enabledCount, enabledTitleFormat, CreateEnabledItem);
            CreateSampleItemsInternal(ref items, enabledCount, disabledCount, disabledTitleFormat, CreateDisabledItem);

            return items;
        }


        static void CreateSampleItemsInternal<TSampleItemVM>(
            ref List<TSampleItemVM> items
            , int start, int count
            , string titleFormat
            , Func<string, TSampleItemVM> createItem
        )
            where TSampleItemVM
                : SampleItemViewModel
        {
            for (int i = 0; i < count; i++)
            {
                string title = string.Format(titleFormat, i + start);
                TSampleItemVM item = createItem(title);
                items.Add(item);
            }
        }




        string _title = null;
        public string Title
        {
            get => _title;
            init => RASIC(ref _title, value);
        }


        bool _enabled = true;
        public bool Enabled
        {
            get => _enabled;
            set => RASIC(ref _enabled, value);
        }




        public override Type GetViewType()
            => Views.ViewLocator.USE_TOSTRING;


        public override string ToString()
            => Title;
    }
}