using System.Collections.Generic;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public readonly struct CaptionButtonRolesPair
    {
        public IEnumerable<CaptionButtonRole> Left
        {
            get;
            private init;
        }


        public IEnumerable<CaptionButtonRole> Right
        {
            get;
            private init;
        }




        public CaptionButtonRolesPair(IEnumerable<CaptionButtonRole> left, IEnumerable<CaptionButtonRole> right)
        {
            Left = left;
            Right = right;
        }




        public override string ToString()
            => $"({nameof(Left)}: '{WindowChromeEnumsHelper.FormatCaptionButtonRoles(Left)}', {nameof(Right)}: '{WindowChromeEnumsHelper.FormatCaptionButtonRoles(Right)}')";
    }
}