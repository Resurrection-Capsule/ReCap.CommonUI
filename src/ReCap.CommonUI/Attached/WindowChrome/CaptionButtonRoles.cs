using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public sealed class CaptionButtonRoles
        : ObservableCollection<CaptionButtonRole>
    {
        static readonly char[] _SEPARATORS =
        {
            ','
        };
        public static bool TryParse(string text, out CaptionButtonRoles windowActions)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                windowActions = new();
                return true;
            }


            string[] parts = text.Split(_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length <= 0)
                goto fail;

            CaptionButtonRoles ret = new();
            foreach (string part in parts)
            {
                if (Enum.TryParse(part, out CaptionButtonRole windowAction))
                    ret.Add(windowAction);
                else
                    goto fail;
            }
            windowActions = ret;
            return true;


            fail:
            windowActions = default;
            return false;
        }


        public CaptionButtonRoles()
            : base()
        {}
        public CaptionButtonRoles(IEnumerable<CaptionButtonRole> collection)
            : base(collection)
        {}
        public CaptionButtonRoles(List<CaptionButtonRole> list)
            : base(list)
        {}


        public override string ToString()
            => string.Join(",", this);
    }
}