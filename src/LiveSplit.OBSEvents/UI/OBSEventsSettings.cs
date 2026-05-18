using System;
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

        Logger.AddErrorConsumer(LogError);
        Logger.AddWarningConsumer(LogWarning);
        Logger.AddInfoConsumer(LogInfo);
    }

    public string Host { get; set; } = "localhost";
    
    // default port is 4455. ref: https://github.com/obsproject/obs-websocket/blob/master/src/Config.h#L38
    public int Port { get; set; } = 4455;

    public string Password { get; set; } = "";

    public bool ConnectAutomatically { get; set; } = false;

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

        Credential connectionInfo = CredentialManager.ReadCredential(OBS_CONNECTION_INFO);
        if (connectionInfo != null)
        {
            string[] authority = connectionInfo.UserName.Split(':');
            Host = authority[0];
            Port = int.Parse(authority[1]);
            Password = connectionInfo.Password;
        }
        ConnectAutomatically = SettingsHelper.ParseBool(element[nameof(ConnectAutomatically)], true);
    }

    private int CreateSettingsNodes(XmlDocument document, XmlElement parent)
    {
        return SettingsHelper.CreateSetting(document, parent, nameof(ConnectAutomatically), ConnectAutomatically);
    }

    private async void buttonConnectToObs_Click(object sender, EventArgs e)
    {
        await InitClient();
    }

    public async Task InitClient()
    {
        if (Client != null && Client.IsConnected)
        {
            return;
        }

        Enabled = false;
        labelConnectionStatus.Text = "Status: Connecting...";
        Logger.Info("Connecting...");

        Client = new Client(Host, Port, Password);
        try
        {
            await Client.EstablishSession();
            labelConnectionStatus.Text = "Status: Connected.";
            Logger.Info("Connected to OBS.");
        }
        catch (Exception ex)
        {
            Client = null;
            labelConnectionStatus.Text = "Status: Failed to connect.";
            Logger.Error(ex.Message);
        }

        Enabled = true;
    }

    private void buttonSavePassword_Click(object sender, EventArgs e)
    {
        CredentialManager.DeleteCredential(OBS_CONNECTION_INFO);

        string authority = $"{Host}:{Port}";
        CredentialManager.WriteCredential(OBS_CONNECTION_INFO, authority, Password);
    }
}
