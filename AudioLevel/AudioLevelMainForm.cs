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

        private MMDeviceCollection mMDeviceCollection_in;
        private MMDeviceCollection mMDeviceCollection_out;
        private List<string> mMDeviceCollection_in_ids = new List<string>();
        private List<string> mMDeviceCollection_out_ids = new List<string>();
        private string mMDevice_in_id_next = string.Empty;
        private string mMDevice_out_id_next = string.Empty;


        private bool isCaptureChangeByAuto = false;
        private bool isRenderChangeByAuto = false;
        private bool isInit = true;
        private bool isMute = false;

        public AudioLevelForm()
        {
            InitializeComponent();
            mute_CheckBox.Checked = isMute;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetCaptureComboBox();
            SetRenderComboBox();

            client = new MMNotificationClient();
            client.DefaultDeviceChanged += DefaultDeviceChanged;

            enumerator.RegisterEndpointNotificationCallback(client);

            SetDefaultAudioEndpointCapture();
            SetDefaultAudioEndpointRender();

            SetAudioEndpointCapture(mMDevice_in_id_next);
            SetAudioEndpointRender(mMDevice_out_id_next);

            SetCapture();
            SetRender();

            StartCapture();
            StartRender();

            isInit = false;
        }

        private void DefaultDeviceChanged(object? sender, DefaultDeviceChangedEventArgs e)
        {
            Debug.WriteLine($"Default Device Changed: {e.flow}, {e.role}, {e.defaultDeviceId}");

            if (e.flow == DataFlow.Capture)
            {
                if (capture_capture_in != null)
                {
                    isCaptureChangeByAuto = true;
                    SetDefaultAudioEndpointCapture();
                    StopCapture();
                }
            }
            else if (e.flow == DataFlow.Render)
            {
                if (capture_render_out != null)
                {
                    isRenderChangeByAuto = true;
                    SetDefaultAudioEndpointRender();
                    StopRender();
                }
            }
        }

        private void SetDefaultAudioEndpointCapture()
        {
            try
            {
                mMDevice_in = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
             
                mMDevice_in_id_next = mMDevice_in.ID; // Store the ID for future use
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

                mMDevice_out_id_next = mMDevice_out.ID; // Store the ID for future use
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting default audio endpoint: {ex.Message}");
                return;
            }
        }

        private void SetAudioEndpointCapture(string id)
        {
            try
            {
                mMDevice_in = enumerator.GetDevice(id);
                SetCaptureLabel(mMDevice_in.FriendlyName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting default audio endpoint: {ex.Message}");
                return;
            }
        }

        private void SetAudioEndpointRender(string id)
        {
            try
            {
                mMDevice_out = enumerator.GetDevice(id);
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

                capture_capture_in.DataAvailable += CaptureDataAvailable;
                capture_capture_in.RecordingStopped += CaptureRecoringStopped;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Fixed: Use MessageBox.Show instead of MessageBox
                return;
            }
        }

        private void CaptureDataAvailable(object? sender, WaveInEventArgs e)
        {
            Debug.WriteLine($"Capture Bytes Recorded: {e.BytesRecorded}");
            if (e.BytesRecorded != 0)
            {
                var buffer = new float[e.BytesRecorded / sizeof(float)];
                Buffer.BlockCopy(e.Buffer, 0, buffer, 0, e.BytesRecorded);
                var max = buffer.Max();
                var level = (int)(max * 100); // Convert to percentage
                SetCaptureBar(level);
            }
        }

        private void CaptureRecoringStopped(object? sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageBox.Show($"Capture stopped with error: {e.Exception.Message}");
            }
            else
            {
                DisposeCapture();

                if (mMDevice_in_id_next != string.Empty)
                {
                    SetAudioEndpointCapture(mMDevice_in_id_next);
                    SetCapture();
                    StartCapture();
                    SetCaptureLabel(mMDevice_in.FriendlyName);
                    SetCaptureBar(0);
                }
            }
        }

        private void SetRender()
        {
            try
            {
                capture_render_out = new WasapiLoopbackCapture(mMDevice_out);

                capture_render_out.DataAvailable += RenderDataAvailable;
                capture_render_out.RecordingStopped += RenderRecoringStopped;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Fixed: Use MessageBox.Show instead of MessageBox
                return;
            }
        }

        private void RenderDataAvailable(object? sender, WaveInEventArgs e)
        {
            Debug.WriteLine($"Render Bytes Recorded: {e.BytesRecorded}");
            if (e.BytesRecorded != 0)
            {
                var buffer = new float[e.BytesRecorded / sizeof(float)];
                Buffer.BlockCopy(e.Buffer, 0, buffer, 0, e.BytesRecorded);
                var max = buffer.Max();
                var level = (int)(max * 100); // Convert to percentage
                SetRenderBar(level);
            }
        }

        private void RenderRecoringStopped(object? sender, StoppedEventArgs e)
        {
            if (e.Exception != null)
            {
                MessageBox.Show($"Render stopped with error: {e.Exception.Message}");
            }
            else
            {
                DisposeRender();

                if (mMDevice_out_id_next != string.Empty)
                {
                    SetAudioEndpointRender(mMDevice_out_id_next);
                    SetRender();
                    StartRender();
                    SetRenderLabel(mMDevice_out.FriendlyName);
                    SetRenderBar(0);
                }
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
                capture_capture_in.DataAvailable -= CaptureDataAvailable;
                capture_capture_in.RecordingStopped -= CaptureRecoringStopped;
                capture_capture_in.Dispose();
                capture_capture_in = null;
            }
        }

        private void DisposeRender()
        {
            if (capture_render_out != null)
            {
                capture_render_out.DataAvailable -= RenderDataAvailable;
                capture_render_out.RecordingStopped -= RenderRecoringStopped;
                capture_render_out.Dispose();
                capture_render_out = null;
            }
        }

        private void SetCaptureLabel(string str)
        {
            if (captureLabel1.InvokeRequired)
            {
                captureLabel1.Invoke(new Action(() =>
                {
                    captureLabel1.Text = str;
                }));
            }
            else
            {
                captureLabel1.Text = str;
            }

            if (captureComboBox.InvokeRequired)
            {
                captureComboBox.Invoke(new Action(() =>
                {
                    SelectCaptureDeviceByName(mMDevice_in.FriendlyName);
                }));
            }
            else
            {
                SelectCaptureDeviceByName(mMDevice_in.FriendlyName);
            }
        }

        private void SetRenderLabel(string str)
        {
            if (renderLabel2.InvokeRequired)
            {
                renderLabel2.Invoke(new Action(() =>
                {
                    renderLabel2.Text = str;
                }));
            }
            else
            {
                renderLabel2.Text = str;
            }

            if (renderComboBox.InvokeRequired)
            {
                renderComboBox.Invoke(new Action(() =>
                {
                    SelectRenderDeviceByName(mMDevice_out.FriendlyName);
                }));
            }
            else
            {
                SelectRenderDeviceByName(mMDevice_out.FriendlyName);
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
            client.DefaultDeviceChanged -= DefaultDeviceChanged;

            StopCapture();
            StopRender();
        }

        private new void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this, e.Location);
            }
        }

        private void TopView_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item != null)
            {
                if (item.Checked)
                {
                    item.Checked = false;
                    this.TopMost = false;
                }
                else
                {
                    item.Checked = true;
                    this.TopMost = true;
                }
            }
        }

        private void SetCaptureComboBox()
        {
            mMDeviceCollection_in_ids.Clear();
            mMDeviceCollection_in = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            foreach (var device in mMDeviceCollection_in)
            {
                Debug.WriteLine($"Device: {device.FriendlyName}, ID: {device.ID}");
                var id = captureComboBox.Items.Add(device.FriendlyName);
                mMDeviceCollection_in_ids.Add(device.ID);
            }
        }

        private void SetRenderComboBox()
        {
            mMDeviceCollection_out_ids.Clear();
            mMDeviceCollection_out = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            foreach (var device in mMDeviceCollection_out)
            {
                Debug.WriteLine($"Device: {device.FriendlyName}, ID: {device.ID}");
                var id = renderComboBox.Items.Add(device.FriendlyName);
                mMDeviceCollection_out_ids.Add(device.ID);
            }
        }

        private void SelectCaptureDeviceByName(string name)
        {
            var id = captureComboBox.Items.IndexOf(name);
            if (id != -1)
            {
                captureComboBox.SelectedIndex = id;
            }
        }

        private void SelectRenderDeviceByName(string name)
        {
            var id = renderComboBox.Items.IndexOf(name);
            if (id != -1)
            {
                renderComboBox.SelectedIndex = id;
            }
        }

        private void captureComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isCaptureChangeByAuto && !isInit)
            {

                if (captureComboBox.SelectedItem != null)
                {
                    string selectedDevice = captureComboBox.SelectedItem.ToString();
                    Debug.WriteLine($"Selected Capture Device: {selectedDevice}");
                    mMDevice_in_id_next = mMDeviceCollection_in_ids[captureComboBox.SelectedIndex];
                    SetAudioEndpointCapture(mMDevice_in_id_next);
                    StopCapture();
                }
            }
            isCaptureChangeByAuto = false;
        }

        private void renderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isRenderChangeByAuto && !isInit)
            {
                if (renderComboBox.SelectedItem != null)
                {
                    string selectedDevice = renderComboBox.SelectedItem.ToString();
                    Debug.WriteLine($"Selected Render Device: {selectedDevice}");
                    mMDevice_out_id_next = mMDeviceCollection_out_ids[renderComboBox.SelectedIndex];
                    SetAudioEndpointRender(mMDevice_out_id_next);
                    StopRender();
                }
            }
            isRenderChangeByAuto = false;
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

        private void mute_CheckBox_Click(object sender, EventArgs e)
        {
            isMute = !isMute;
            mute_CheckBox.Checked = isMute;

            if (mMDevice_in != null)
            {
                mMDevice_in.AudioEndpointVolume.Mute = isMute;
            }

        }
    }
}
