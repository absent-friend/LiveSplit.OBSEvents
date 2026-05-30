using System;

using LiveSplit.Model;
using LiveSplit.UI.Components;

[assembly: ComponentFactory(typeof(LiveSplit.OBSEvents.OBSEventsFactory))]

namespace LiveSplit.OBSEvents;

public sealed class OBSEventsFactory : IComponentFactory
{
    // The name of the component that will be displayed in the layout editor.
    public string ComponentName => OBSEventsComponent.Name;

    // The tooltip shown when hovering over the component when adding it to the layout.
    public string Description => "Sends commands to OBS in response to specific events during a run.";

    // Specifies the category of the component.
    // Determines under what category the component is listed when adding it to the layout.
    public ComponentCategory Category => ComponentCategory.Control;

    // A URL to the component's repository.
    public string UpdateURL => "https://github.com/absent-friend/LiveSplit.OBSEvents/releases/download";

    // A URL to the component's update XML file.
    public string XMLURL => $"https://raw.githubusercontent.com/absent-friend/LiveSplit.OBSEvents/main/Components/update.LiveSplit.OBSEvents.xml";

    // The current version of the component.
    public Version Version => Version.Parse("1.1.0");

    public IComponent Create(LiveSplitState state)
        => new OBSEventsComponent(state);

    // This property is unused.
    public string UpdateName => throw null;
}
