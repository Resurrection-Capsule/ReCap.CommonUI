using System;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class ChromeElementPseudoClassesPlaceholderAttribute
        : Attribute
    {
        public ChromeElementPseudoClassesPlaceholderAttribute()
            : base()
        {}
    }
}