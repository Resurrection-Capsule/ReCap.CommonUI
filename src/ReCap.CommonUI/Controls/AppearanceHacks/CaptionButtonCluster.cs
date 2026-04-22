using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public sealed class CaptionButtonCluster
        : ItemsControl
    {
        static CaptionButtonCluster()
        {
            IsVisibleProperty.Changed.AddClassHandler<CaptionButtonCluster>(IsVisibleProperty_Changed);
        }

        static void IsVisibleProperty_Changed(CaptionButtonCluster cluster, AvaloniaPropertyChangedEventArgs args)
            => cluster.OnIsEffectivelyVisibleChanged();


        void OnIsEffectivelyVisibleChanged()
            => OnIsEffectivelyVisibleChanged(IsEffectivelyVisible);
        void OnIsEffectivelyVisibleChanged(bool isEffectivelyVisible)
        {
            double targetWidth;
            if (isEffectivelyVisible)
            {
                MaxWidth = double.PositiveInfinity;
                targetWidth = Bounds.Width;
            }
            else
            {
                MaxWidth = 0d;
                targetWidth = 0d;
            }
            IsEffectivelyVisibleChanged?.Invoke(this, new(isEffectivelyVisible, targetWidth));
        }


        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            OnIsEffectivelyVisibleChanged();
        }


        IDisposable _attachmentDisposable = null;
        Window _window = null;
        bool? _isRightSide = null;
        void Attach(CaptionButtons2 captionButtons, Window window, AvaloniaProperty<CaptionButtonRoles> captionButtonsProperty)
        {
            _window = window;

            SizeChanged += This_SizeChanged;
            IsEffectivelyVisibleChanged += This_IsEffectivelyVisibleChanged;
            _attachmentDisposable = new CompositeDisposable()
            {
                this
                    .GetObservable(IsVisibleProperty)
                    .Subscribe(isVisible => RefreshSize())
                ,
                Bind(ItemsSourceProperty, captionButtons[!captionButtonsProperty]),
            };
            RefreshSize();
        }


        internal void AttachToLeft(CaptionButtons2 captionButtons, Window window)
        {
            _isRightSide = false;
            Attach(captionButtons, window, CaptionButtons2.LeftCaptionButtonsProperty);
        }

        internal void AttachToRight(CaptionButtons2 captionButtons, Window window)
        {
            _isRightSide = true;
            Attach(captionButtons, window, CaptionButtons2.RightCaptionButtonsProperty);
        }

        internal void Detach()
        {
            _attachmentDisposable?.Dispose();
            _attachmentDisposable = null;
            _window = null;
            _isRightSide = null;

            SizeChanged -= This_SizeChanged;
            IsEffectivelyVisibleChanged -= This_IsEffectivelyVisibleChanged;
        }


        void This_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ComputeNewCaptionButtonsSize(sender, e, out double newWidth))
                SetWindowCaptionButtonsSize(newWidth);
        }
        void This_IsEffectivelyVisibleChanged(object sender, CaptionButtonClusterUpdateEventArgs e)
        {
            if (ComputeNewCaptionButtonsSize(sender, e.TargetWidth, out double newWidth))
                SetWindowCaptionButtonsSize(newWidth);
        }



        const double _DEFAULT_RefreshSize_fallback = 0d;
        internal void RefreshSize(double fallback = _DEFAULT_RefreshSize_fallback)
            => PropagateNewSize(Bounds.Width, fallback);
        void PropagateNewSize(double newSize, double fallback = _DEFAULT_RefreshSize_fallback)
        {
            if (ComputeNewCaptionButtonsSize(this, newSize, out double newWidth))
            {
                SetWindowCaptionButtonsSize(newWidth);
                return;
            }

            double fallbackSize = fallback;

            if ((fallbackSize >= 0d) && (_window != null))
                SetWindowCaptionButtonsSize(fallbackSize);
        }


        bool ComputeNewCaptionButtonsSize(object sender, SizeChangedEventArgs e, out double result)
            => ComputeNewCaptionButtonsSize(sender, e.NewSize.Width, out result);
        bool ComputeNewCaptionButtonsSize(object sender, double itemsCtlWidth, out double result)
        {
            if (sender is ItemsControl itemsCtl)
                return ComputeNewCaptionButtonsSize(itemsCtl, itemsCtlWidth, out result);

            result = default;
            return false;
        }
        bool ComputeNewCaptionButtonsSize(ItemsControl itemsCtl, double itemsCtlWidth, out double result)
        {
            if (_window == null)
                goto fail;
            else if (itemsCtl == null)
                goto fail;
            else if (!itemsCtl.IsEffectivelyVisible)
            {
                result = 0d;
                return true;
            }


            result = itemsCtlWidth;
            return true;


            fail:
            result = 0d;
            return false;
        }


        void SetWindowCaptionButtonsSize(double newSize)
        {
            if (_isRightSide == true)
                WindowChromeAddon.SetRightCaptionButtonsWidth(_window, newSize);
            else if (_isRightSide == false)
                WindowChromeAddon.SetLeftCaptionButtonsWidth(_window, newSize);
        }




        internal event EventHandler<CaptionButtonClusterUpdateEventArgs> IsEffectivelyVisibleChanged;
    }


    internal sealed class CaptionButtonClusterUpdateEventArgs
        : EventArgs
    {
        public readonly bool IsEffectivelyVisible;
        public readonly double TargetWidth;
        public CaptionButtonClusterUpdateEventArgs(bool isEffectivelyVisible, double targetWidth)
        {
            IsEffectivelyVisible = isEffectivelyVisible;
            TargetWidth = targetWidth;
        }
    }
}