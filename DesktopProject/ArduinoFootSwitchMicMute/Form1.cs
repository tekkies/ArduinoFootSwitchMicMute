using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using TrayAppTerminateManager;

namespace MuteFootSwitch
{
    public partial class MainForm : Form
    {
        private const int IconWidth = 64;
        private const string RestoreMenuText = "&Restore";
        private const string EXitMenuText = "E&xit";
        private const int PostMuteNoiseTime = 1500;
        private byte[] _buffer;
        private readonly List<Microphone> _microphones = new List<Microphone>();
        private TerminateManager _terminateManager;
        private int portNumber;
        private bool live;
        private static Stopwatch _muteStartedStopwatch;

        public MainForm()
        {
            _muteStartedStopwatch = new Stopwatch();
            InitializeComponent();
            _terminateManager = new TerminateManager(this, trayIcon);
            SetCheckbox(CheckState.Unchecked);
            StartLevelMeter();
            StartPortHunt();
            ShowWindow();
        }

        private void StartPortHunt()
        {
            live = false;
            portNumber = 5;
            arduinoSerialPort.Close();
            portTimer.Start();
        }

        private void StartLevelMeter()
        {
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
                foreach (var device in devices)
                {
                    var waveIn = new WasapiCapture(device);
                    var microphone = new Microphone() { Device = device, WaveIn = waveIn };
                    waveIn.StartRecording();
                    _microphones.Add(microphone);
                }
            }

            levelsTimer.Start();
        }

        private void arduinoSerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (live)
            {
                var lastChar = GetLastChar();
                var checkState = CheckState.Indeterminate;
                if (lastChar == 'M')
                {
                    checkState = CheckState.Unchecked;
                }
                else if (lastChar == 'H')
                {
                    checkState = CheckState.Checked;
                }

                Invoke((Action) (() => SetCheckbox(checkState)));
            }
        }
        private void SetCheckbox(CheckState checkState)
        {
            micLiveCheckBox.CheckState = checkState;
        }

        private int GetLastChar()
        {
            var charsRead = arduinoSerialPort.Read(_buffer, 0, _buffer.Length);
            int readChar = _buffer[charsRead - 1];
            return readChar;
        }

        private void switchedPressedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkState = micLiveCheckBox.Checked;
            SwitchMic(checkState);
        }

        private static void SwitchMic(bool micEnabled)
        {
            if (micEnabled)
            {
                _muteStartedStopwatch.Reset();
            }
            else
            {
                _muteStartedStopwatch.Restart();
            }

            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
                foreach (var device in devices)
                {
                    //Debug.WriteLine($"{device.FriendlyName}:{!device.AudioEndpointVolume.Mute}");
                    //if (!(device.AudioEndpointVolume.Mute == !enabled))
                    {
                        device.AudioEndpointVolume.Mute = !micEnabled;
                    }
                }
            }

        }

        private void levelsTimer_Tick(object sender, EventArgs e)
        {
            float masterPeakValue = 0;
            foreach (var microphone in _microphones)
            {
                var deviceAudioMeterInformation = microphone.Device.AudioMeterInformation;
                masterPeakValue += deviceAudioMeterInformation.MasterPeakValue;
            }
            masterPeakValue = masterPeakValue * 100 / _microphones.Count;
            if (masterPeakValue > 1)
            {
                masterPeakValue = (float)(Math.Log10(masterPeakValue) * (100 / 2));
            }

            DealWithHotMicAfterMuteCommand(masterPeakValue);
            micLevel.Value = (int)masterPeakValue;
            DrawIcon((int)masterPeakValue);
        }

        private void DealWithHotMicAfterMuteCommand(float masterPeakValue)
        {
            if (masterPeakValue > 0.00001 && _muteStartedStopwatch.ElapsedMilliseconds > PostMuteNoiseTime)
            {
                micLiveCheckBox.CheckState = CheckState.Checked;
                reMuteTime.Start();
                System.Diagnostics.Debug.WriteLine($"{_muteStartedStopwatch.ElapsedMilliseconds:0000} Mic is not actually muted: {masterPeakValue}");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _terminateManager.FormClosing(sender, e);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private void DrawIcon(int masterPeakValue)
        {
            if (!_terminateManager.Terminating)
            {
                using (var bitmap = new Bitmap(IconWidth, IconWidth, PixelFormat.Format32bppPArgb))
                {
                    var dateTime = DateTime.Now;
                    using (var graphics = Graphics.FromImage(bitmap))
                    {
                        var rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        if (micLiveCheckBox.CheckState == CheckState.Checked)
                        {
                            graphics.DrawIcon(ArduinoFootSwitchMicMute.Properties.Resources.micwhite, rectangle);
                            var barHeight = (bitmap.Height * masterPeakValue) / 100;
                            graphics.FillRectangle(Brushes.White, 0, bitmap.Height - barHeight, (bitmap.Width * 10) / 32, bitmap.Height);
                        }
                        else
                        {
                            graphics.DrawIcon(ArduinoFootSwitchMicMute.Properties.Resources.micdisabled, rectangle);
                        }
                    }
                    var hIcon = bitmap.GetHicon();
                    trayIcon.Icon = Icon.FromHandle(hIcon);
                    DestroyIcon(trayIcon.Icon.Handle);
                }
            }
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            micLiveCheckBox.Checked = !micLiveCheckBox.Checked;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            trayIconContextMenuStrip.Items.Clear();
            trayIconContextMenuStrip.Items.Add(RestoreMenuText);
            trayIconContextMenuStrip.Items.Add(EXitMenuText);
            WindowState = FormWindowState.Minimized;
        }

        private void trayIconContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == RestoreMenuText)
            {
                ShowWindow();
            }

            else if (e.ClickedItem.Text == EXitMenuText)
            {
                Close();
            }
        }

        private void ShowWindow()
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                ShowInTaskbar = false;
                Hide();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        private void portTimer_Tick(object sender, EventArgs e)
        {
            if (live && arduinoSerialPort.IsOpen)
            {
                return;
            }
            ShowWindow();
            live = false;
            portTimer.Stop();
            arduinoSerialPort.PortName = $"COM{portNumber}";
            labelStatus.Text = $"Probing {arduinoSerialPort.PortName}...";
            _buffer = new byte[arduinoSerialPort.ReadBufferSize];
            try
            {
                arduinoSerialPort.Open();
            }
            catch (System.IO.IOException)
            {
            }

            if (arduinoSerialPort.IsOpen)
            {
                Thread.Sleep(2000);
                arduinoSerialPort.ReadTimeout = 2000;
                arduinoSerialPort.WriteTimeout = 100;
                try
                {
                    arduinoSerialPort.Write("i");
                }
                catch (Exception)
                {
                }
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                
                try
                {
                    var helloLine = arduinoSerialPort.ReadLine();
                    stopwatch.Stop();
                    if (!String.IsNullOrEmpty(helloLine))
                    {
                        if (helloLine.StartsWith("Tekkies Foot Switch:"))
                        {
                            labelStatus.Text = $"Connected {arduinoSerialPort.PortName}";
                            WindowState = FormWindowState.Minimized;
                            live = true;
                        }
                    }
                }
                catch (System.TimeoutException)
                {
                }
            }
            if (!live)
            {
                SkipPort();
            }
            portTimer.Start();
        }

        private void SkipPort()
        {
            if (arduinoSerialPort.IsOpen)
            {
                arduinoSerialPort.Close();
            }
            portNumber++;
            if (portNumber > 30)
            {
                portNumber = 1;
            }
        }

        private void doubleCheckTimer_Tick(object sender, EventArgs e)
        {
            SwitchMic(micLiveCheckBox.Checked);
        }

        private void reMuteTime_Tick(object sender, EventArgs e)
        {
            reMuteTime.Enabled = false;
            micLiveCheckBox.CheckState = CheckState.Unchecked;
        }
    }

    internal class Microphone
    {
        public MMDevice Device { get; set; }
        public WasapiCapture WaveIn { get; set; }
    }
}
