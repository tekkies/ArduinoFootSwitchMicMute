using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using NAudio.CoreAudioApi;

namespace MuteFootSwitch
{
    public partial class MainForm : Form
    {
        private readonly byte[] _buffer;
        private readonly List<Microphone> _microphones = new List<Microphone>();

        public MainForm()
        {
            InitializeComponent();
            if (!arduinoSerialPort.IsOpen)
            {
                arduinoSerialPort.Open();
            }
            _buffer = new byte[arduinoSerialPort.ReadBufferSize];
            SetCheckbox(CheckState.Unchecked);

            StartLevelMeter();
        }

        private void StartLevelMeter()
        {
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
                foreach (var device in devices)
                {
                    var waveIn = new WasapiCapture(device);
                    var microphone = new Microphone() {Device = device, WaveIn = waveIn};
                    waveIn.StartRecording();
                    _microphones.Add(microphone);
                }
            }

            levelsTimer.Start();
        }

        private void arduinoSerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            var lastChar = GetLastChar();
            var checkState = CheckState.Indeterminate;
            if (lastChar == 'M')
            {
                checkState = CheckState.Unchecked;
            }
            else if(lastChar == 'H')
            {
                checkState = CheckState.Checked;
            }
            Invoke((Action)(() => SetCheckbox(checkState)));
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

        private static void SwitchMic(bool enabled)
        {
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
                foreach (var device in devices)
                {
                    device.AudioEndpointVolume.Mute = !enabled;
                }
            }
        }
        
        private void levelsTimer_Tick(object sender, EventArgs e)
        {
            float masterPeakValue=0;
            foreach (var microphone in _microphones)
            {
                var deviceAudioMeterInformation = microphone.Device.AudioMeterInformation;
                masterPeakValue += deviceAudioMeterInformation.MasterPeakValue;
            }
            masterPeakValue = masterPeakValue * 100 / _microphones.Count;
            micLevel.Value = (int) masterPeakValue;
        }
    }

    internal class Microphone
    {
        public MMDevice Device { get; set; }
        public WasapiCapture WaveIn { get; set; }
    }
}
