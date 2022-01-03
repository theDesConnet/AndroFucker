using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using SharpAdbClient;
using System.Diagnostics;

namespace QuickRoot
{
    public partial class Main : Form
    {
        public AdbServer server = new AdbServer();
        public static AdbClient client = new AdbClient();

        public static void RunFile(string FilePath, string Arguments, Boolean UAC, Boolean Hidden, Boolean WaitForExit)
        {
            Process RunApp = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            if (Hidden == true) startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            else startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = FilePath;
            startInfo.Arguments = Arguments;
            if (UAC == true) startInfo.Verb = "runas";

            RunApp.StartInfo = startInfo;
            RunApp.Start();
            if (WaitForExit == true) RunApp.WaitForExit();

        }

        public Main()
        {
            InitializeComponent();
            var result = server.StartServer(@"adb\adb.exe", restartServerIfNewer: true);

            if (result.ToString() == "AlreadyRunning")
            {
                toolStripStatusLabel2.Text = "ADB: is Already Running";
            }
            else
            {
                toolStripStatusLabel2.Text = "ADB: " + result.ToString();
            }
            toolStripStatusLabel1.Text = "Status: Waiting for connect...";
            var monitor = new DeviceMonitor(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)));
            monitor.DeviceConnected += this.OnDeviceConnected;
            monitor.DeviceDisconnected += this.OnDeviceDisconnected;
            monitor.Start();
        }

        private void OnDeviceDisconnected(object sender, DeviceDataEventArgs e)
        {
            toolStripStatusLabel1.Text = "Status: Waiting for connect...";
            toolStripStatusLabel3.Text = "Device: Unknown";
        }

        private void OnDeviceConnected(object sender, DeviceDataEventArgs e)
        {
            toolStripStatusLabel1.Text = "Status: Connected.";
            if (e.Device.Name == "")
            {
                toolStripStatusLabel3.Text = "Device: Name not reading";
            }
            else
            {
                toolStripStatusLabel3.Text = "Device: " + e.Device.Model;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (toolStripStatusLabel1.Text == "Status: Waiting for connect...")
            {
                MessageBox.Show("Please connect your phone and enable usb debbuging");
            }
            else
            {
                toolStripStatusLabel1.Text = "Status: Please Wait...";
                var device = client.GetDevices().First();
                client.Reboot("bootloader", device);
                progressBar1.Value = 10;
                RunFile(@"adb\fastboot.exe", "erase system", false, true, true);
                progressBar1.Value = 30;
                RunFile(@"adb\fastboot.exe", "erase boot", false, true, true);
                progressBar1.Value = 50;
                RunFile(@"adb\fastboot.exe", "erase recovery", false, true, true);
                RunFile(@"adb\fastboot.exe", "reboot", false, true, true);
                progressBar1.Value = 100;
                MessageBox.Show("Get Done!");
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(-1);
        }
    }
}
