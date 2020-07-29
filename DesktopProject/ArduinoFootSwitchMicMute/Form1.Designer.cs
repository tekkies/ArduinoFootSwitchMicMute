﻿namespace MuteFootSwitch
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
            this.SuspendLayout();
            // 
            // arduinoSerialPort
            // 
            this.arduinoSerialPort.PortName = "COM12";
            this.arduinoSerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.arduinoSerialPort_DataReceived);
            // 
            // micLiveCheckBox
            // 
            this.micLiveCheckBox.Enabled = false;
            this.micLiveCheckBox.Location = new System.Drawing.Point(12, 12);
            this.micLiveCheckBox.Name = "micLiveCheckBox";
            this.micLiveCheckBox.Size = new System.Drawing.Size(104, 24);
            this.micLiveCheckBox.TabIndex = 1;
            this.micLiveCheckBox.Text = "MICS LIVE";
            this.micLiveCheckBox.UseVisualStyleBackColor = true;
            this.micLiveCheckBox.CheckedChanged += new System.EventHandler(this.switchedPressedCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(422, 68);
            this.label1.TabIndex = 2;
            this.label1.Text = "© 2020 Andy Joiner\r\n\r\nCredits:\r\nNAudio, an audio library for .NET by Mark Heath a" +
    "nd Contributors";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 129);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.micLiveCheckBox);
            this.Name = "MainForm";
            this.Text = "ArduinoFootSwitchMicMute";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort arduinoSerialPort;
        private System.Windows.Forms.CheckBox micLiveCheckBox;
        private System.Windows.Forms.Label label1;
    }
}

