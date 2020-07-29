using System;
using System.Diagnostics;
using System.Windows.Forms;
using NAudio.CoreAudioApi;

namespace MuteFootSwitch
{
    public partial class MainForm : Form
    {
        private byte[] _buffer;

        public MainForm()
        {
            InitializeComponent();
            if (!arduinoSerialPort.IsOpen)
            {
                arduinoSerialPort.Open();
            }
            _buffer = new byte[arduinoSerialPort.ReadBufferSize];
            SetCheckbox(CheckState.Indeterminate);
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
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
                foreach (var device in devices)
                {
                    device.AudioEndpointVolume.Mute = !micLiveCheckBox.Checked;
                }
            }
        }
    }
}
