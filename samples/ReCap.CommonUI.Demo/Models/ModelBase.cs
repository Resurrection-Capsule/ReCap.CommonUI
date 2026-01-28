using System;

namespace ReCap.CommonUI.Demo.Models
{
    public abstract class ModelBase
        : RxObjectBase
    {
        public override Type GetViewType()
            => Type.GetType(GetType().FullName.Replace("Model", "View"));
    }
}
