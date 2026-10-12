using Avalonia.Controls;
using Avalonia.Styling;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    internal sealed class CaptionButtonClusterPanelTemplate
        : ITemplate<Panel>
    {
        readonly CaptionButtonCluster _cluster;
        public CaptionButtonClusterPanelTemplate(CaptionButtonCluster cluster)
        {
            _cluster = cluster;
        }


        public Panel Build()
            => new CaptionButtonClusterPanel()
            {
                Name = "PART_ItemsPanel",
                [!CaptionButtonClusterPanel.PositionProperty] = _cluster[!CaptionButtonCluster.PositionProperty],
            };
        object ITemplate.Build()
            => Build();
    }
}