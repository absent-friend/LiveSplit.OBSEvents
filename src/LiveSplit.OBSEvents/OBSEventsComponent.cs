using System;
using System.Windows.Forms;
using System.Xml;

using LiveSplit.Model;
using LiveSplit.OBSEvents.UI;
using LiveSplit.OBSEvents.Utility;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.OBSEvents;

public sealed class OBSEventsComponent : LogicComponent
{
    private readonly LiveSplitState _state;
    private readonly OBSEventsSettings _settings;

    public OBSEventsComponent(LiveSplitState state)
    {
        _state = state;
        _settings = new();

        _state.OnStart += _state_OnStart;
        _state.OnSplit += _state_OnSplit;
    }

    private async void _state_OnStart(object sender, EventArgs e)
    {
        if (_settings.ConnectAutomatically)
        {
            await _settings.InitClient();
        }
    }

    private async void _state_OnSplit(object sender, EventArgs e)
    {
        if (_settings.Client == null)
        {
            return;
        }

        int index = _state.CurrentSplitIndex - 1;
        TimingMethod method = _state.CurrentTimingMethod;
        
        bool isGold = LiveSplitStateHelper.CheckBestSegment(_state, index, method);
        if (isGold)
        {
            try
            {
                TimeSpan segmentTime = LiveSplitStateHelper.GetPreviousSegmentTime(_state, index, method).Value;
                await _settings.Client.SaveGoldSegmentReplay(_state.Run[index].Name, segmentTime);
            }
            catch (Exception ex) {
                Logger.Error(ex.Message);
            }
        }
    }

    public const string Name = "OBS Events";

    public override string ComponentName => Name;

    public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) {}

    public override XmlNode GetSettings(XmlDocument document)
    {
        return _settings.GetSettings(document);
    }

    public int GetSettingsHashCode()
    {
        return _settings.GetSettingsHashCode();
    }

    public override void SetSettings(XmlNode settings)
    {
        _settings.SetSettings(settings);
    }

    public override Control GetSettingsControl(LayoutMode mode)
    {
        return _settings;
    }

    public override void Dispose()
    {
        _state.OnSplit -= _state_OnSplit;
        _settings.Client?.Dispose();
    }
}
