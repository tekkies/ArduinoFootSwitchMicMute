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
        }

        private void arduinoSerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int readChar;
            readChar = GetLastChar();
            if (readChar == 50)
            {
                Invoke((Action)(() => SetCheckbox3(CheckState.Unchecked)));
            }
            else if(readChar == 49)
            {
                Invoke((Action)(() => SetCheckbox3(CheckState.Checked)));
            }
            else
            {
                Invoke((Action)(() => SetCheckbox3(CheckState.Indeterminate)));
            }
            Debug.WriteLine(readChar);
        }

        private int GetLastChar()
        {
            int readChar;
            var charsRead = arduinoSerialPort.Read(_buffer, 0, _buffer.Length);
            readChar = _buffer[charsRead - 1];
            return readChar;
        }

        private void SetCheckbox3(CheckState @checked)
        {
            micLiveCheckBox.CheckState = @checked;
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

        private void ArduinoFootSwitchMicMute_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
