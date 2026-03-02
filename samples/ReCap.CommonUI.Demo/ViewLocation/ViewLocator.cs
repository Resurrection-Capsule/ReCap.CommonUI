using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReCap.CommonUI.Demo.ViewModels;

namespace ReCap.CommonUI.Demo.Views
{
    public class ViewLocator
        : IDataTemplate
        , ICachingDataTemplate
    {
        public static readonly Type USE_TOSTRING = typeof(ViewLocator);
        bool _useCaching = false;
        public bool UseCaching
        {
            get => _useCaching;
            set => _useCaching = value;
        }

        public virtual bool SupportsRecycling => false;

        Dictionary<object, Control> _cache = new();
        protected virtual Control BuildIfNeeded(object data)
        {
            if (data == null)
                return CreateTextForFailure($"{nameof(data)} was null");

            if (data is not RxObjectBase rxData)
                return CreateTextForFailure($"'{data.GetType().FullName}' is not assignable to '{typeof(RxObjectBase).FullName}'");

            var type = rxData.GetViewType();
            if (type == null)
                return CreateTextForFailure($"'{data}' returned null view type");
            else if (!type.IsAssignableTo(typeof(Control)))
                return CreateTextForFailure($"'{data}' returned invalid view type (not assignable to '{typeof(Control).FullName}')");


            var inst = Activator.CreateInstance(type);
            if ((inst != null) && (inst is Control ctrl))
                return ctrl;
            
            return CreateTextForFailure($"Couldn't create view of type '{type.FullName}'");
        }

        static TextBlock CreateTextForFailure(string failMsg)
        {
            Debug.WriteLine($"ViewLocator: {failMsg}");
            return new TextBlock()
            {
                Text = failMsg
            };
        }



        
        public virtual Control Build(object data)
            => this.Build_Impl(data, UseCaching, ref _cache, BuildIfNeeded);
        
        public virtual bool Match(object data)
            => (data is ViewModelBase vm)
            && (vm.GetViewType() != USE_TOSTRING)
        ;
    }
}