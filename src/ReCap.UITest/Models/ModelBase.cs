using System;

namespace ReCap.UITest.Models
{
    public abstract class ModelBase
        : RxObjectBase
    {
        public override Type GetViewType()
            => Type.GetType(GetType().FullName.Replace("Model", "View"));
    }
}
