using System;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.GoldGrabber.OBS;
using LiveSplit.UI;
using LiveSplit.Web;

namespace LiveSplit.GoldGrabber.UI;

public partial class GoldGrabberSettings : UserControl
{
    private const string OBS_PASSWORD_KEY = "LiveSplit.GoldGrabber.OBSPassword";

    public GoldGrabberSettings()
    {
        InitializeComponent();

        textHost.DataBindings.Add(nameof(TextBox.Text), this, nameof(Host), false, DataSourceUpdateMode.OnPropertyChanged);
        textPort.DataBindings.Add(nameof(TextBox.Text), this, nameof(Port), false, DataSourceUpdateMode.OnPropertyChanged);
        textPassword.DataBindings.Add(nameof(TextBox.Text), this, nameof(Password), false, DataSourceUpdateMode.OnPropertyChanged);

        Logger.AddErrorConsumer(LogError);
        Logger.AddWarningConsumer(LogWarning);
        Logger.AddInfoConsumer(LogInfo);
    }

    public string Host { get; set; } = "localhost";
    
    // default port is 4455. ref: https://github.com/obsproject/obs-websocket/blob/master/src/Config.h#L38
    public int Port { get; set; } = 4455;

    public string Password { get; set; } = "";

    internal Client Client { get; private set; } = null;

    private string FormatTimestampForLog(DateTime timestamp)
    {
        return timestamp.ToString("G");
    }

    private void LogError(string error)
    {
        string timestamp = FormatTimestampForLog(DateTime.Now);
        textStatus.Text += $"[ERROR {timestamp}] {error}\r\n";
    }

    private void LogWarning(string warning)
    {
        string timestamp = FormatTimestampForLog(DateTime.Now);
        textStatus.Text += $"[WARN  {timestamp}] {warning}\r\n";
    }

    private void LogInfo(string info)
    {
        string timestamp = FormatTimestampForLog(DateTime.Now);
        textStatus.Text += $"[INFO  {timestamp}] {info}\r\n";
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

        Host = SettingsHelper.ParseString(element[nameof(Host)], Host);
        Port = SettingsHelper.ParseInt(element[nameof(Port)], Port);
        Password = CredentialManager.ReadCredential(OBS_PASSWORD_KEY)?.Password ?? Password;
    }

    private int CreateSettingsNodes(XmlDocument document, XmlElement parent)
    {
        return SettingsHelper.CreateSetting(document, parent, nameof(Host), Host)
            ^ SettingsHelper.CreateSetting(document, parent, nameof(Port), Port)
            ^ Password.GetHashCode();
    }

    private async void buttonConnectToObs_Click(object sender, EventArgs e)
    {
        if (Client != null && Client.IsConnected)
        {
            return;
        }

        Enabled = false;
        Logger.Info("Connecting...");
        
        Client = new Client(Host, Port, Password);
        try
        {
            await Client.EstablishSession();
            Logger.Info("Connected to OBS.");
        }
        catch (Exception ex)
        {
            Client = null;
            Logger.Error(ex.Message);
        }
        
        Enabled = true;
    }

    private void buttonSavePassword_Click(object sender, EventArgs e)
    {
        CredentialManager.DeleteCredential(OBS_PASSWORD_KEY);
        CredentialManager.WriteCredential(OBS_PASSWORD_KEY, null, Password);
    }
}
