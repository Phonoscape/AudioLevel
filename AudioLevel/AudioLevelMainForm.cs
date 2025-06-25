using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.Wave;
using System.Data;
using System.Diagnostics;

namespace AudioLevel
{
    public partial class AudioLevelForm : Form
    {
        private WasapiCapture? capture_capture_in = null;
        private WasapiLoopbackCapture? capture_render_out = null;
        private MMNotificationClient? client = null;
        private MMDevice? mMDevice_in = null;
        private MMDevice? mMDevice_out = null;
        private MMDeviceEnumerator enumerator = new MMDeviceEnumerator();

        public AudioLevelForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MMDeviceCollection mMDeviceCollection_in = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            MMDeviceCollection mMDeviceCollection_out = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (var device in mMDeviceCollection_in)
            {
                Debug.WriteLine($"Device: {device.FriendlyName}, ID: {device.ID}");
            }

            client = new MMNotificationClient();
            client.DefaultDeviceChanged += (s, a) =>
            {
                Debug.WriteLine($"Default Device Changed: {a.flow}, {a.role}, {a.defaultDeviceId}");
                                
                if (a.flow == DataFlow.Capture)
                {
                    if (capture_capture_in != null)
                    {
                        StopCapture();

                        SetDefaultAudioEndpointCapture();
                        SetCapture();
                        StartCapture();

                        SetCaptureLabel(mMDevice_in.FriendlyName);
                        SetCaptureBar(0);
                    }
                }
                else if (a.flow == DataFlow.Render)
                {
                    if (capture_render_out != null)
                    {
                        StopRender();

                        SetDefaultAudioEndpointRender();
                        SetRender();
                        StartRender();

                        SetRenderLabel(mMDevice_out.FriendlyName);
                        SetRenderBar(0);
                    }
                }
            };

            enumerator.RegisterEndpointNotificationCallback(client);

            SetDefaultAudioEndpointCapture();
            SetDefaultAudioEndpointRender();
            SetCapture();
            SetRender();

            StartCapture();
            StartRender();
        }

        private void SetDefaultAudioEndpointCapture()
        {
            try
            {
                mMDevice_in = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
                SetCaptureLabel(mMDevice_in.FriendlyName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting default audio endpoint: {ex.Message}");
                return;
            }
        }

        private void SetDefaultAudioEndpointRender()
        { 
            try
            {
                mMDevice_out = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Communications);
                SetRenderLabel(mMDevice_out.FriendlyName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting default audio endpoint: {ex.Message}");
                return;
            }
        }

        private void SetCapture()
        {
            try
            {
                capture_capture_in = new WasapiCapture(mMDevice_in);

                capture_capture_in.DataAvailable += (s, a) =>
                {
                    Debug.WriteLine($"Capture Bytes Recorded: {a.BytesRecorded}");

                    if (a.BytesRecorded != 0)
                    {
                        var buffer = new float[a.BytesRecorded / sizeof(float)];
                        Buffer.BlockCopy(a.Buffer, 0, buffer, 0, a.BytesRecorded);
                        var max = buffer.Max();
                        var level = (int)(max * 100); // Convert to percentage
                        SetCaptureBar(level);
                    }
                };

                capture_capture_in.RecordingStopped += (s, a) =>
                {
                    if (a.Exception != null)
                    {
                        MessageBox.Show($"Capture stopped with error: {a.Exception.Message}");
                    }
                    else
                    {
                        DisposeCapture();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Fixed: Use MessageBox.Show instead of MessageBox
                return;
            }
        }

        private void SetRender()
        {
            try
            {
                capture_render_out = new WasapiLoopbackCapture(mMDevice_out);

                capture_render_out.DataAvailable += (s, a) =>
                {

                    Debug.WriteLine($"Render Bytes Recorded: {a.BytesRecorded}");

                    if (a.BytesRecorded != 0)
                    {
                        var buffer = new float[a.BytesRecorded / sizeof(float)];
                        Buffer.BlockCopy(a.Buffer, 0, buffer, 0, a.BytesRecorded);
                        var max = buffer.Max();
                        var level = (int)(max * 100); // Convert to percentage
                        SetRenderBar(level);
                    }
                };

                capture_render_out.RecordingStopped += (s, a) =>
                {
                    if (a.Exception != null)
                    {
                        MessageBox.Show($"Render stopped with error: {a.Exception.Message}");
                    }
                    else
                    {
                        DisposeRender();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Fixed: Use MessageBox.Show instead of MessageBox
                return;
            }
        }

        private void StartCapture()
        {
            if (capture_capture_in != null)
            {
                capture_capture_in.StartRecording();
            }
        }

        private void StartRender()
        {
            if (capture_render_out != null)
            {
                capture_render_out.StartRecording();
            }
        }

        private void StopCapture()
        {
            if (capture_capture_in != null)
            {
                capture_capture_in.StopRecording();
            }
        }

        private void StopRender()
        {
            if (capture_render_out != null)
            {
                capture_render_out.StopRecording();
            }
        }

        private void DisposeCapture()
        {
            if (capture_capture_in != null)
            {
                capture_capture_in.Dispose();
                capture_capture_in = null;
            }
        }

        private void DisposeRender()
        {
            if (capture_render_out != null)
            {
                capture_render_out.Dispose();
                capture_render_out = null;
            }
        }

        private void SetCaptureLabel(string str)
        {
            if (label1.InvokeRequired)
            {
                label1.Invoke(new Action(() =>
                {
                    SetCaptureLabel(str);
                }));
            }
            else
            {
                label1.Text = str;
            }
        }

        private void SetRenderLabel(string str)
        {
            if (label2.InvokeRequired)
            {
                label2.Invoke(new Action(() =>
                {
                    SetRenderLabel(str);
                }));
            }
            else
            {
                label2.Text = str;
            }
        }

        private void SetCaptureBar(int level)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action(() =>
                {
                    progressBar1.Value = Math.Min(level, 100); // Ensure it doesn't exceed 100%
                }));
            }
            else
            {
                progressBar1.Value = Math.Min(level, 100); // Ensure it doesn't exceed 100%
            }
        }

        private void SetRenderBar(int level)
        {
            if (progressBar2.InvokeRequired)
            {
                progressBar2.Invoke(new Action(() =>
                {
                    progressBar2.Value = Math.Min(level, 100); // Ensure it doesn't exceed 100%
                }));
            }
            else
            {
                progressBar2.Value = Math.Min(level, 100); // Ensure it doesn't exceed 100%
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            enumerator.UnregisterEndpointNotificationCallback(client);

            StopCapture();
            StopRender();
        }

        // https://gist.github.com/RupertAvery/b1a5297d99f498e44b534b8a5190d9d3
        public class DefaultDeviceChangedEventArgs
        {
            public DataFlow flow { get; set; }
            public Role role { get; set; }
            public string defaultDeviceId { get; set; }
        }

        private class MMNotificationClient : IMMNotificationClient
        {
            public EventHandler<DefaultDeviceChangedEventArgs> DefaultDeviceChanged { get; set; }

            public MMNotificationClient()
            {
            }

            public void OnDeviceStateChanged(string deviceId, DeviceState newState)
            {
            }

            public void OnDeviceAdded(string pwstrDeviceId)
            {
            }

            public void OnDeviceRemoved(string deviceId)
            {
            }

            public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
            {
                DefaultDeviceChanged.Invoke(this, new DefaultDeviceChangedEventArgs() { flow = flow, role = role, defaultDeviceId = defaultDeviceId });
            }

        public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
            {
            }
        }
    }
}
