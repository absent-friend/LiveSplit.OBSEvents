using LiveSplit.Model;
using LiveSplit.Model.Comparisons;
using LiveSplit.OBSEvents.UI;
using LiveSplit.OBSEvents.Utility;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.OBSEvents;

public sealed class OBSEventsComponent : LogicComponent
{
    private readonly LiveSplitState _state;
    private readonly OBSEventsSettings _settings;

    private event EventHandler SettingsLoaded;

    public OBSEventsComponent(LiveSplitState state)
    {
        _state = state;
        _settings = new();

        _state.OnSplit += _state_OnSplit;
        SettingsLoaded += OnComponentInitialized;
    }

    private async void OnComponentInitialized(object sender, EventArgs e)
    {
        if (_settings.ConnectAutomatically)
        {
            await _settings.AutoConnect();
        }
        SettingsLoaded -= OnComponentInitialized;
    }

    private async void _state_OnSplit(object sender, EventArgs e)
    {
        if (_settings.Client == null)
        {
            return;
        }

        // getting this before delay in case there's another split during the delay (not all that likely though)
        int index = _state.CurrentSplitIndex - 1;
        TimingMethod method = _state.CurrentTimingMethod;

        if (_settings.ReplayDelay > 0)
        {
            await Task.Delay(TimeSpan.FromSeconds(_settings.ReplayDelay));
        }

        if (_settings.SaveBestSegmentReplay && IsBestSegment(_state, index, method))
        {
            try
            {
                TimeSpan segmentTime = LiveSplitStateHelper.GetPreviousSegmentTime(_state, index, method).Value;
                await _settings.Client.SaveBestSegmentReplay(_state, index, segmentTime);
            }
            catch (Exception ex) {
                Logger.Error(ex.Message);
            }
        }
    }

    private bool IsBestSegment(LiveSplitState state, int index, TimingMethod method)
    {
        TimeSpan? segmentTime;
        if (index == 0)
        {
            segmentTime = state.Run[index].SplitTime[method];
        }
        else
        {
            TimeSpan? split = state.Run[index].SplitTime[method];
            TimeSpan? prevSplit = state.Run[index - 1].SplitTime[method];
            segmentTime = split - prevSplit;
        }

        if (!segmentTime.HasValue)
        {
            // only save replays for golds that span a single named segment.
            return false;
        }

        TimeSpan? bestSegment = state.Run[index].BestSegmentTime[method];
        TimeSpan? diffToBest = segmentTime - bestSegment;
        TimeSpan? threshold = TimeSpan.FromSeconds(_settings.ReplayThreshold);
        return bestSegment == null || diffToBest < threshold;
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
        SettingsLoaded?.Invoke(this, null);
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
