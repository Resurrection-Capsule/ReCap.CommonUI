using System.Collections.Generic;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal abstract class WindowChromeFactoryImplBaseBase
        : IWindowChromeFactoryImpl
    {
        public abstract bool CanUseManagedWindowChrome
        {
            get;
        }


        public abstract bool PrefersManagedWindowChrome
        {
            get;
        }


        public virtual bool DefaultShowCaptionIcon
        {
            get => true;
        }


        public virtual bool DefaultShowCaptionText
        {
            get => true;
        }


        public abstract CaptionButtonRolesPair DefaultCaptionButtonRoles
        {
            get;
        }


        public abstract IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get;
        }




        public abstract IWindowChromeImpl CreateImpl(WindowChrome chrome);
    }




    internal abstract class WindowChromeFactoryImplBase
        : WindowChromeFactoryImplBaseBase
    {
#region Properties
        readonly CaptionButtonRolesPair _defaultButtonRoles;
        public sealed override CaptionButtonRolesPair DefaultCaptionButtonRoles
        {
            get => _defaultButtonRoles;
        }


        readonly IEnumerable<CaptionButtonRole> _validButtonRoles;
        public sealed override IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => _validButtonRoles;
        }
#endregion




        public WindowChromeFactoryImplBase()
            : base()
        {
            _defaultButtonRoles = InitDefaultButtonRoles();
            _validButtonRoles = InitValidButtonRoles();
        }




        protected virtual CaptionButtonRolesPair InitDefaultButtonRoles()
            => new(
                new[]
                {
                    CaptionButtonRole.WindowMenu,
                },
                new[]
                {
                    CaptionButtonRole.Minimize,
                    CaptionButtonRole.Maximize,
                    CaptionButtonRole.Close,
                }
            );
        protected abstract IEnumerable<CaptionButtonRole> InitValidButtonRoles();
    }
}