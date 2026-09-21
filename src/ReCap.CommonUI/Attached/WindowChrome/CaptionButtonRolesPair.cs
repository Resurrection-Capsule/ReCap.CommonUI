using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public readonly struct CaptionButtonRolesPair
    {
        public CaptionButtonRoles Left
        {
            get;
            init;
        }
        public CaptionButtonRoles Right
        {
            get;
            init;
        }




        public CaptionButtonRolesPair(IEnumerable<CaptionButtonRole> left, IEnumerable<CaptionButtonRole> right)
        {
            Left = AsObservableCollection(left);
            Right = AsObservableCollection(right);
        }
        static CaptionButtonRoles AsObservableCollection(IEnumerable<CaptionButtonRole> enumerable)
            => enumerable is CaptionButtonRoles roles
                ? roles
                : new(enumerable)
            ;
    }
}