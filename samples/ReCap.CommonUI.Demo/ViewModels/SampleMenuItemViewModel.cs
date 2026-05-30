using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public class SampleMenuItemViewModel
        : SampleItemViewModel
    {
        readonly ObservableCollection<SampleMenuItemViewModel> _childItems = new();
        public ObservableCollection<SampleMenuItemViewModel> ChildItems
        {
            get => _childItems;
            set
            {
                _childItems.Clear();
                foreach (var item in value)
                {
                    _childItems.Add(item);
                }
                IEnumerable<SampleMenuItemViewModel> _ = _childItems;
                RASIC(ref _, value);
            }
        }


        MenuItemToggleType _toggleType = MenuItemToggleType.None;
        public MenuItemToggleType ToggleType
        {
            get => _toggleType;
            set => RASIC(ref _toggleType, value);
        }


        bool _isChecked = false;
        public bool IsChecked
        {
            get => _isChecked;
            set => RASIC(ref _isChecked, value);
        }


        string _groupName = null;
        public string GroupName
        {
            get => _groupName;
            set => RASIC(ref _groupName, value);
        }




        protected override void GetPropertiesForToString()
        {
            base.GetPropertiesForToString();


            var toggleType = ToggleType;
            bool isToggle = toggleType != MenuItemToggleType.None;
            AddPropertyIf(isToggle, nameof(ToggleType), toggleType);
            AddPropertyIf(toggleType == MenuItemToggleType.Radio, nameof(GroupName), GroupName);
            AddPropertyIf(isToggle && IsChecked, nameof(IsChecked));


            var childItemCount = ChildItems.Count;
            AddPropertyIf(childItemCount > 0, $"{nameof(ChildItems)}.{nameof(ChildItems.Count)}", childItemCount);
        }
    }
}