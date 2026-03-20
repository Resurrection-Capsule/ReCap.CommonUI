using System;

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

        public CaptionButtonRolesPair(CaptionButtonRoles left, CaptionButtonRoles right)
        {
            Left = left;
            Right = right;
        }
    }
}