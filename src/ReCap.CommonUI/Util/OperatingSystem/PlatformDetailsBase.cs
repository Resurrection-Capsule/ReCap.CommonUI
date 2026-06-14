using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReCap.CommonUI.Util.OperatingSystem
{
    public abstract class PlatformDetailsBase
        : INotifyPropertyChanged
    {
#region Properties
        internal virtual Version Version
        {
            get => Environment.OSVersion.Version;
        }


        internal virtual bool IsVersionDefinitelyAccurate
        {
            get => true;
        }
#endregion




        internal PlatformDetailsBase()
            : base()
        {}




        protected void SetAndRaise<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
        {
            backingField = newValue;
            PropertyChanged?.Invoke(this, new(propertyName));
        }


#region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
#endregion INotifyPropertyChanged
    }
}