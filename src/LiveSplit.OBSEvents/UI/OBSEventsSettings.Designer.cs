namespace LiveSplit.OBSEvents.UI;

partial class OBSEventsSettings
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.groupOBSConnection = new System.Windows.Forms.GroupBox();
            this.tableConnectionParams = new System.Windows.Forms.TableLayoutPanel();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.checkAutoConnect = new System.Windows.Forms.CheckBox();
            this.buttonSavePassword = new System.Windows.Forms.Button();
            this.textHost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textPort = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonConnectToObs = new System.Windows.Forms.Button();
            this.labelConnectionStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textDebugLog = new System.Windows.Forms.TextBox();
            this.checkShowDebugLog = new System.Windows.Forms.CheckBox();
            this.groupSaveBest = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.checkSaveBestSegments = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textReplayFilename = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textReplayDelay = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textReplayThreshold = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupOBSConnection.SuspendLayout();
            this.tableConnectionParams.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupSaveBest.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupOBSConnection
            // 
            this.groupOBSConnection.AutoSize = true;
            this.groupOBSConnection.Controls.Add(this.tableConnectionParams);
            this.groupOBSConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupOBSConnection.Location = new System.Drawing.Point(3, 3);
            this.groupOBSConnection.Name = "groupOBSConnection";
            this.groupOBSConnection.Size = new System.Drawing.Size(465, 132);
            this.groupOBSConnection.TabIndex = 0;
            this.groupOBSConnection.TabStop = false;
            this.groupOBSConnection.Text = "OBS Connection";
            // 
            // tableConnectionParams
            // 
            this.tableConnectionParams.AutoSize = true;
            this.tableConnectionParams.ColumnCount = 2;
            this.tableConnectionParams.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableConnectionParams.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableConnectionParams.Controls.Add(this.textPassword, 1, 2);
            this.tableConnectionParams.Controls.Add(this.label1, 0, 0);
            this.tableConnectionParams.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableConnectionParams.Controls.Add(this.textHost, 1, 0);
            this.tableConnectionParams.Controls.Add(this.label2, 0, 1);
            this.tableConnectionParams.Controls.Add(this.label3, 0, 2);
            this.tableConnectionParams.Controls.Add(this.textPort, 1, 1);
            this.tableConnectionParams.Controls.Add(this.tableLayoutPanel4, 1, 3);
            this.tableConnectionParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableConnectionParams.Location = new System.Drawing.Point(3, 16);
            this.tableConnectionParams.Name = "tableConnectionParams";
            this.tableConnectionParams.RowCount = 5;
            this.tableConnectionParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableConnectionParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableConnectionParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableConnectionParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableConnectionParams.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableConnectionParams.Size = new System.Drawing.Size(459, 113);
            this.tableConnectionParams.TabIndex = 0;
            // 
            // textPassword
            // 
            this.textPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textPassword.Location = new System.Drawing.Point(62, 55);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(394, 20);
            this.textPassword.TabIndex = 5;
            this.textPassword.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableConnectionParams.SetColumnSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.checkAutoConnect, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonSavePassword, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 78);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(459, 35);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // checkAutoConnect
            // 
            this.checkAutoConnect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkAutoConnect.AutoSize = true;
            this.checkAutoConnect.Location = new System.Drawing.Point(262, 9);
            this.checkAutoConnect.Name = "checkAutoConnect";
            this.checkAutoConnect.Size = new System.Drawing.Size(164, 17);
            this.checkAutoConnect.TabIndex = 0;
            this.checkAutoConnect.Text = "Auto-Connect During Launch";
            this.checkAutoConnect.UseVisualStyleBackColor = true;
            // 
            // buttonSavePassword
            // 
            this.buttonSavePassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSavePassword.Location = new System.Drawing.Point(3, 3);
            this.buttonSavePassword.Name = "buttonSavePassword";
            this.buttonSavePassword.Size = new System.Drawing.Size(223, 29);
            this.buttonSavePassword.TabIndex = 6;
            this.buttonSavePassword.Text = "Save Connection Info";
            this.buttonSavePassword.UseVisualStyleBackColor = true;
            this.buttonSavePassword.Click += new System.EventHandler(this.buttonSavePassword_Click);
            // 
            // textHost
            // 
            this.textHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textHost.Location = new System.Drawing.Point(62, 3);
            this.textHost.Name = "textHost";
            this.textHost.Size = new System.Drawing.Size(394, 20);
            this.textHost.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Password";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textPort
            // 
            this.textPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPort.Location = new System.Drawing.Point(62, 29);
            this.textPort.MaxLength = 5;
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(394, 20);
            this.textPort.TabIndex = 4;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableConnectionParams.SetColumnSpan(this.tableLayoutPanel4, 2);
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 113);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(459, 1);
            this.tableLayoutPanel4.TabIndex = 6;
            // 
            // buttonConnectToObs
            // 
            this.buttonConnectToObs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonConnectToObs.Location = new System.Drawing.Point(3, 3);
            this.buttonConnectToObs.Name = "buttonConnectToObs";
            this.buttonConnectToObs.Size = new System.Drawing.Size(223, 29);
            this.buttonConnectToObs.TabIndex = 1;
            this.buttonConnectToObs.Text = "Connect to OBS";
            this.buttonConnectToObs.UseVisualStyleBackColor = true;
            this.buttonConnectToObs.Click += new System.EventHandler(this.buttonConnectToObs_Click);
            // 
            // labelConnectionStatus
            // 
            this.labelConnectionStatus.AutoSize = true;
            this.labelConnectionStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelConnectionStatus.Location = new System.Drawing.Point(232, 0);
            this.labelConnectionStatus.Name = "labelConnectionStatus";
            this.labelConnectionStatus.Size = new System.Drawing.Size(224, 35);
            this.labelConnectionStatus.TabIndex = 1;
            this.labelConnectionStatus.Text = "Status: Not connected.";
            this.labelConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupOBSConnection, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.textDebugLog, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.checkShowDebugLog, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.groupSaveBest, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(471, 555);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // textDebugLog
            // 
            this.textDebugLog.AcceptsReturn = true;
            this.textDebugLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textDebugLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textDebugLog.Location = new System.Drawing.Point(3, 331);
            this.textDebugLog.Multiline = true;
            this.textDebugLog.Name = "textDebugLog";
            this.textDebugLog.ReadOnly = true;
            this.textDebugLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textDebugLog.Size = new System.Drawing.Size(465, 221);
            this.textDebugLog.TabIndex = 2;
            this.textDebugLog.Visible = false;
            // 
            // checkShowDebugLog
            // 
            this.checkShowDebugLog.AutoSize = true;
            this.checkShowDebugLog.Location = new System.Drawing.Point(3, 308);
            this.checkShowDebugLog.Name = "checkShowDebugLog";
            this.checkShowDebugLog.Size = new System.Drawing.Size(109, 17);
            this.checkShowDebugLog.TabIndex = 3;
            this.checkShowDebugLog.Text = "Show Debug Log";
            this.checkShowDebugLog.UseVisualStyleBackColor = true;
            this.checkShowDebugLog.CheckedChanged += new System.EventHandler(this.checkShowDebugLog_CheckedChanged);
            // 
            // groupSaveBest
            // 
            this.groupSaveBest.AutoSize = true;
            this.groupSaveBest.Controls.Add(this.tableLayoutPanel5);
            this.groupSaveBest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupSaveBest.Location = new System.Drawing.Point(3, 182);
            this.groupSaveBest.Name = "groupSaveBest";
            this.groupSaveBest.Size = new System.Drawing.Size(465, 120);
            this.groupSaveBest.TabIndex = 4;
            this.groupSaveBest.TabStop = false;
            this.groupSaveBest.Text = "Save Best Segment Replays";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.checkSaveBestSegments, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.textReplayFilename, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.textReplayDelay, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.textReplayThreshold, 1, 3);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(459, 101);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // checkSaveBestSegments
            // 
            this.checkSaveBestSegments.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkSaveBestSegments.AutoSize = true;
            this.tableLayoutPanel5.SetColumnSpan(this.checkSaveBestSegments, 2);
            this.checkSaveBestSegments.Location = new System.Drawing.Point(3, 3);
            this.checkSaveBestSegments.Name = "checkSaveBestSegments";
            this.checkSaveBestSegments.Size = new System.Drawing.Size(65, 17);
            this.checkSaveBestSegments.TabIndex = 0;
            this.checkSaveBestSegments.Text = "Enabled";
            this.checkSaveBestSegments.UseVisualStyleBackColor = true;
            this.checkSaveBestSegments.CheckedChanged += new System.EventHandler(this.checkSaveBestSegments_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Replay Filename";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textReplayFilename
            // 
            this.textReplayFilename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textReplayFilename.Location = new System.Drawing.Point(114, 26);
            this.textReplayFilename.Name = "textReplayFilename";
            this.textReplayFilename.Size = new System.Drawing.Size(342, 20);
            this.textReplayFilename.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Delay (Seconds)";
            // 
            // textReplayDelay
            // 
            this.textReplayDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textReplayDelay.Location = new System.Drawing.Point(114, 52);
            this.textReplayDelay.Name = "textReplayDelay";
            this.textReplayDelay.Size = new System.Drawing.Size(342, 20);
            this.textReplayDelay.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Threshold (Seconds)";
            // 
            // textReplayThreshold
            // 
            this.textReplayThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textReplayThreshold.Location = new System.Drawing.Point(114, 78);
            this.textReplayThreshold.Name = "textReplayThreshold";
            this.textReplayThreshold.Size = new System.Drawing.Size(342, 20);
            this.textReplayThreshold.TabIndex = 6;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.buttonConnectToObs, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelConnectionStatus, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 141);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(459, 35);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // OBSEventsSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "OBSEventsSettings";
            this.Size = new System.Drawing.Size(471, 555);
            this.groupOBSConnection.ResumeLayout(false);
            this.groupOBSConnection.PerformLayout();
            this.tableConnectionParams.ResumeLayout(false);
            this.tableConnectionParams.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupSaveBest.ResumeLayout(false);
            this.groupSaveBest.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupOBSConnection;
    private System.Windows.Forms.TableLayoutPanel tableConnectionParams;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textHost;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox textPort;
    private System.Windows.Forms.TextBox textPassword;
    private System.Windows.Forms.Button buttonConnectToObs;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.Button buttonSavePassword;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    private System.Windows.Forms.CheckBox checkAutoConnect;
    private System.Windows.Forms.Label labelConnectionStatus;
    private System.Windows.Forms.CheckBox checkShowDebugLog;
    private System.Windows.Forms.GroupBox groupSaveBest;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
    private System.Windows.Forms.CheckBox checkSaveBestSegments;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textReplayFilename;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TextBox textDebugLog;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox textReplayDelay;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox textReplayThreshold;
}
