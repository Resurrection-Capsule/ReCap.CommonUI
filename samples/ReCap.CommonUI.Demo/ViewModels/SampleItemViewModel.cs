using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public class SampleItemViewModel
        : ViewModelBase
    {
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


        bool _selected = false;
        public bool Selected
        {
            get => _selected;
            set => RASIC(ref _selected, value);
        }




        public override Type GetViewType()
            => Views.ViewLocator.USE_TOSTRING;


        public sealed override string ToString()
            => Title;



        List<string> _propsFormatted = null;
        public string ToString(bool extended)
        {
            if (!extended)
                return ToString();

            _propsFormatted = new();
            GetPropertiesForToString();
            

            string propsJoined = string.Join(", ", _propsFormatted);
            _propsFormatted = null;
            return $"'{CleanString(Title)}' ({propsJoined})";
        }


        protected virtual void GetPropertiesForToString()
        {
            AddPropertyIf(!Enabled, $"!{nameof(Enabled)}");
            AddPropertyIf(Selected, nameof(Selected));
        }


        protected void AddProperty(string name)
            => AddPropertyInternal(name, null, false);
        protected void AddProperty(string name, object value)
            => AddPropertyInternal(name, value, true);

        protected void AddPropertyIf(bool condition, string name)
            => AddPropertyIfInternal(condition, name, null, false);
        protected void AddPropertyIf(bool condition, string name, object value)
            => AddPropertyIfInternal(condition, name, value, true);

        protected void AddPropertyIf(Func<bool> condition, string name)
            => AddPropertyIfInternal(condition(), name, null, false);
        protected void AddPropertyIf(Func<bool> condition, string name, object value)
            => AddPropertyIfInternal(condition(), name, value, true);


        void AddPropertyIfInternal(bool condition, string name, object value, bool includeValue)
        {
            if (condition)
                AddPropertyInternal(name, value, includeValue);
        }


        void AddPropertyInternal(string name, object value, bool includeValue)
        {
            string formatted = CleanString(name);
            if (includeValue)
                formatted = $"{formatted}: {GetValueString(value)}";

            _propsFormatted.Add(formatted);
        }




        static string CleanString(string value)
        {
            if (value == null)
                return "null";

            int length = value.Length;
            for (int i = 0; i < length; i++)
            {
                if (char.IsWhiteSpace(value[i]))
                    return $"'{value}'";
            }

            return value;
        }


        static string GetValueString(object value)
        {
            string result;
            if (value == null)
                result = null;
            if (value is string valStr)
                result = valStr;
            else
                result = value.ToString();

            return CleanString(result);
        }
    }
}