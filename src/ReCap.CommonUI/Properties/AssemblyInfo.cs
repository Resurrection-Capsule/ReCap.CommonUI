using Avalonia.Metadata;

[assembly: XmlnsDefinition(RCNS.URL, "ReCap.CommonUI")]
[assembly: XmlnsDefinition(RCNS.URL, "ReCap.CommonUI.Controls")]
[assembly: XmlnsDefinition(RCNS.URL, "ReCap.CommonUI.Controls.AppearanceHacks")]
[assembly: XmlnsDefinition(RCNS.URL, "ReCap.CommonUI.Controls.Decorators")]
[assembly: XmlnsDefinition(RCNS.URL, "ReCap.CommonUI.Converters")]
[assembly: XmlnsDefinition(RCNS.URL, "ReCap.CommonUI.Easings")]

internal static class RCNS
{
    internal const string URL = "https://github.com/Resurrection-Capsule/ReCap.CommonUI";
}