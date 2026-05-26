using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.OBSEvents.OBS;
using LiveSplit.OBSEvents.Utility;
using LiveSplit.UI;
using LiveSplit.Web;

namespace LiveSplit.OBSEvents.UI;

public partial class OBSEventsSettings : UserControl
{
    private const string OBS_CONNECTION_INFO = "LiveSplit.OBSEvents.ConnectionInfo";

    public OBSEventsSettings()
    {
        InitializeComponent();

        textHost.DataBindings.Add(nameof(TextBox.Text), this, nameof(Host), false, DataSourceUpdateMode.OnPropertyChanged);
        textPort.DataBindings.Add(nameof(TextBox.Text), this, nameof(Port), false, DataSourceUpdateMode.OnPropertyChanged);
        textPassword.DataBindings.Add(nameof(TextBox.Text), this, nameof(Password), false, DataSourceUpdateMode.OnPropertyChanged);
        checkAutoConnect.DataBindings.Add(nameof(CheckBox.Checked), this, nameof(ConnectAutomatically), false, DataSourceUpdateMode.OnPropertyChanged);
        
        checkSaveBestSegments.DataBindings.Add(nameof(CheckBox.Checked), this, nameof(SaveBestSegmentReplay), false, DataSourceUpdateMode.OnPropertyChanged);
        textReplayFilename.DataBindings.Add(nameof(TextBox.Text), this, nameof(ReplayNameFormat), false, DataSourceUpdateMode.OnPropertyChanged);
        textReplayDelay.DataBindings.Add(nameof(TextBox.Text), this, nameof(ReplayDelay), false, DataSourceUpdateMode.OnPropertyChanged);

        Logger.AddErrorConsumer(LogError);
        Logger.AddWarningConsumer(LogWarning);
        Logger.AddInfoConsumer(LogInfo);
    }

    public string Host { get; set; } = "localhost";
    
    // default port is 4455. ref: https://github.com/obsproject/obs-websocket/blob/master/src/Config.h#L38
    public int Port { get; set; } = 4455;

    public string Password { get; set; } = "";

    public bool ConnectAutomatically { get; set; } = true;

    public bool SaveBestSegmentReplay { get; set; } = true;

    public string ReplayNameFormat { get; set; } = "%game-%category-%segment-%time";

    public int ReplayDelay { get; set; } = 0;

    internal Client Client { get; private set; } = null;

    private bool HostIsNotSet => Host == null || Host.Trim().Length == 0;

    private CancellationTokenSource _autoConnectCancel = null;

    private bool IsAutoConnecting => _autoConnectCancel != null;

    private static string FormatTimestampForLog(DateTime timestamp)
    {
        return timestamp.ToString("G");
    }

    private void LogError(string error)
    {
        LogMessage("ERROR", error);
    }

    private void LogWarning(string warning)
    {
        LogMessage("WARNING", warning);
    }

    private void LogInfo(string info)
    {
        LogMessage("INFO", info);
    }

    private void LogMessage(string type, string message)
    {
        string timestamp = FormatTimestampForLog(DateTime.Now);
        textDebugLog.AppendText($"[{type} {timestamp}]\r\n{message}\r\n\r\n");
    }

    public XmlNode GetSettings(XmlDocument document)
    {
        var parent = document.CreateElement("Settings");
        CreateSettingsNodes(document, parent);
        return parent;
    }

    public int GetSettingsHashCode()
    {
        return CreateSettingsNodes(null, null);
    }

    public void SetSettings(XmlNode settings)
    {
        if (settings is not XmlElement element)
            return;

        Credential connectionInfo = CredentialManager.ReadCredential(OBS_CONNECTION_INFO);
        if (connectionInfo != null)
        {
            string[] authority = connectionInfo.UserName.Split(':');
            Host = authority[0];
            Port = int.Parse(authority[1]);
            Password = connectionInfo.Password;
        }
        ConnectAutomatically = SettingsHelper.ParseBool(element[nameof(ConnectAutomatically)], ConnectAutomatically);

        SaveBestSegmentReplay = SettingsHelper.ParseBool(element[nameof(SaveBestSegmentReplay)], SaveBestSegmentReplay);
        ReplayNameFormat = SettingsHelper.ParseString(element[nameof(ReplayNameFormat)], ReplayNameFormat);
        ReplayDelay = SettingsHelper.ParseInt(element[nameof(ReplayDelay)], ReplayDelay);
    }

    private int CreateSettingsNodes(XmlDocument document, XmlElement parent)
    {
        return SettingsHelper.CreateSetting(document, parent, nameof(ConnectAutomatically), ConnectAutomatically)
            ^ SettingsHelper.CreateSetting(document, parent, nameof(SaveBestSegmentReplay), SaveBestSegmentReplay)
            ^ SettingsHelper.CreateSetting(document, parent, nameof(ReplayNameFormat), ReplayNameFormat)
            ^ SettingsHelper.CreateSetting(document, parent, nameof(ReplayDelay), ReplayDelay);
    }

    private async void buttonConnectToObs_Click(object sender, EventArgs e)
    {
        if (IsAutoConnecting)
        {
            CancelAutoConnect();
        }
        else if (Client != null)
        {
            Disconnect();
        }
        else
        {
            await Connect();
        }
    }

    private void Disconnect()
    {
        Logger.Info("Disconnecting.");
        Client.Dispose();
        Client = null;
        buttonConnectToObs.Text = "Connect to OBS";
        labelConnectionStatus.Text = "Status: Not connected.";
    }

    public async Task AutoConnect()
    {
        if (Client != null || IsAutoConnecting)
        {
            return;
        }

        int attempts = 0;
        _autoConnectCancel = new();
        labelConnectionStatus.Text = "Status: Auto-connecting...";
        buttonConnectToObs.Text = "Cancel Auto-Connect";
        while (Client == null)
        {
            if (_autoConnectCancel.IsCancellationRequested)
            {
                break;
            }
            Logger.Info($"Auto-connecting (attempt {++attempts})");
            await InitClient();
            if (Client == null)
            {
                try
                {
                    Logger.Info("Retrying in 10 seconds...");
                    await Task.Delay(TimeSpan.FromSeconds(10), _autoConnectCancel.Token);
                }
                catch (TaskCanceledException)
                {
                    Logger.Info("Auto-connect cancelled.");
                    break;
                }
            }
        }
        if (Client == null)
        {
            labelConnectionStatus.Text = "Status: Not connected.";
            buttonConnectToObs.Text = "Connect to OBS";
        }
        _autoConnectCancel = null;
    }

    private void CancelAutoConnect()
    {
        buttonConnectToObs.Text = "Cancelling...";
        _autoConnectCancel.Cancel();
    }

    private async Task Connect()
    {
        buttonConnectToObs.Enabled = false;
        await InitClient();
        buttonConnectToObs.Enabled = true;
    }

    private async Task InitClient()
    {
        if (HostIsNotSet)
        {
            Logger.Error("Invalid connection settings. Make sure that you specified a host.");
            return;
        }

        SetEnabledForConnect(false);
        labelConnectionStatus.Text = "Status: Connecting...";
        Logger.Info("Connecting...");

        Client = new Client(Host, Port, Password, this);
        try
        {
            await Client.EstablishSession();
            labelConnectionStatus.Text = "Status: Connected.";
            buttonConnectToObs.Text = "Disconnect";
            Logger.Info("Connected to OBS.");
        }
        catch (Exception ex)
        {
            Client = null;
            labelConnectionStatus.Text = "Status: Failed to connect.";
            Logger.Error($"Failed to connect. {ex.Message}");
        }

        SetEnabledForConnect(true);
    }

    private void SetEnabledForConnect(bool value)
    {
        groupOBSConnection.Enabled = value;
        groupSaveBest.Enabled = value;
    }

    private void buttonSavePassword_Click(object sender, EventArgs e)
    {
        if (HostIsNotSet)
        {
            Logger.Error("Can't save connection info; you need to specify a host.");
            return;
        }

        CredentialManager.DeleteCredential(OBS_CONNECTION_INFO);

        string authority = $"{Host}:{Port}";
        CredentialManager.WriteCredential(OBS_CONNECTION_INFO, authority, Password);
    }

    private void checkShowDebugLog_CheckedChanged(object sender, EventArgs e)
    {
        textDebugLog.Visible = checkShowDebugLog.Checked;
    }

    private void checkSaveBestSegments_CheckedChanged(object sender, EventArgs e)
    {
        textReplayFilename.Enabled = checkSaveBestSegments.Checked;
    }
}
