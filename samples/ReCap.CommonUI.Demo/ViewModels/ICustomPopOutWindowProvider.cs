using System;
using System.Collections.Generic;
using Avalonia.Controls;
using ReCap.CommonUI.Demo.Reflection;
using ReCap.CommonUI.Demo.ViewModels.Pages;
using ReCap.CommonUI.Demo.Views;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public interface IPopOutWindowProvider
    {
        Window CreatePopOutWindow();
    }
}