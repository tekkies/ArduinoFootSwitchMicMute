namespace MuteFootSwitch
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.arduinoSerialPort = new System.IO.Ports.SerialPort(this.components);
            this.micLiveCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.levelsTimer = new System.Windows.Forms.Timer(this.components);
            this.micLevel = new System.Windows.Forms.ProgressBar();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // arduinoSerialPort
            // 
            this.arduinoSerialPort.PortName = "COM12";
            this.arduinoSerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.arduinoSerialPort_DataReceived);
            // 
            // micLiveCheckBox
            // 
            this.micLiveCheckBox.Location = new System.Drawing.Point(12, 12);
            this.micLiveCheckBox.Name = "micLiveCheckBox";
            this.micLiveCheckBox.Size = new System.Drawing.Size(153, 24);
            this.micLiveCheckBox.TabIndex = 1;
            this.micLiveCheckBox.Text = "Mics Enabled";
            this.micLiveCheckBox.UseVisualStyleBackColor = true;
            this.micLiveCheckBox.CheckedChanged += new System.EventHandler(this.switchedPressedCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(613, 85);
            this.label1.TabIndex = 2;
            this.label1.Text = "© 2020 Andy Joiner\r\n\r\nCredits:\r\nNAudio, an audio library for .NET by Mark Heath a" +
    "nd Contributors\r\nTray icon management based on WinFormWithTrayIcon Copyright (c)" +
    " 2010-2017 John J Schultz";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // levelsTimer
            // 
            this.levelsTimer.Tick += new System.EventHandler(this.levelsTimer_Tick);
            // 
            // micLevel
            // 
            this.micLevel.Location = new System.Drawing.Point(525, 13);
            this.micLevel.Name = "micLevel";
            this.micLevel.Size = new System.Drawing.Size(100, 23);
            this.micLevel.TabIndex = 4;
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayIconContextMenuStrip;
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // trayIconContextMenuStrip
            // 
            this.trayIconContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.trayIconContextMenuStrip.Name = "contextMenuStrip1";
            this.trayIconContextMenuStrip.Size = new System.Drawing.Size(211, 32);
            this.trayIconContextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.trayIconContextMenuStrip_ItemClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 159);
            this.Controls.Add(this.micLevel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.micLiveCheckBox);
            this.Name = "MainForm";
            this.Text = "ArduinoFootSwitchMicMute";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort arduinoSerialPort;
        private System.Windows.Forms.CheckBox micLiveCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer levelsTimer;
        private System.Windows.Forms.ProgressBar micLevel;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayIconContextMenuStrip;
    }
}

