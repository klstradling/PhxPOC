using Microsoft.VisualBasic.Logging;
using Squirrel;
using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PhxWinCSharp
{
    public partial class Phx_Form : Form
    {
        public Phx_Form()
        {
            InitializeComponent();
        }
        private void Phx_Form_Load(object sender, EventArgs e)
        {
            AddVersionNumber();

            CheckForUpdates();

            UUID_Txt.Text = System.Guid.NewGuid().ToString("B").ToUpper();

            ManagementObjectSearcher searcher = new
            ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            int i = 0;
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                if (i == 0)
                {
                    DiskSN_Txt0.Text = wmi_HD["SerialNumber"].ToString();
                    DiskModel_Txt0.Text = wmi_HD["Model"].ToString();
                }
                else if (i == 1)
                {
                    DiskSN_Txt1.Text = wmi_HD["SerialNumber"].ToString();
                    DiskModel_Txt1.Text = wmi_HD["Model"].ToString();
                }
                else if (i == 2)
                {
                    DiskSN_Txt2.Text = wmi_HD["SerialNumber"].ToString();
                    DiskModel_Txt2.Text = wmi_HD["Model"].ToString();
                }
                else if (i == 3)
                {
                    DiskSN_Txt3.Text = wmi_HD["SerialNumber"].ToString();
                    DiskModel_Txt2.Text = wmi_HD["Model"].ToString();
                }
                else if (i == 4)
                {
                    DiskSN_Txt4.Text = wmi_HD["SerialNumber"].ToString();
                    DiskModel_Txt4.Text = wmi_HD["Model"].ToString();
                }

                i++;

                DiskSN_Txt0.Text = wmi_HD["SerialNumber"].ToString();

                /* HardDrive hd = new HardDrive();
                 hd.Model = wmi_HD["Model"].ToString();
                 hd.Type = wmi_HD["InterfaceType"].ToString();
                 hdCollection.Add(hd);
                */
            }

            i = 0;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up && nic.GetPhysicalAddress().ToString().Length > 0)
                {
                    if (i == 0)
                    {
                        MAC_Txt0.Text = nic.GetPhysicalAddress().ToString();
                        MACDesc_0.Text = nic.Description;
                    }
                    if (i == 1)
                    {
                        MAC_Txt1.Text = nic.GetPhysicalAddress().ToString();
                        MACDesc_1.Text = nic.Description;
                    }
                    if (i == 2)
                    {
                        MAC_Txt2.Text = nic.GetPhysicalAddress().ToString();
                        MACDesc_2.Text = nic.Description;
                    }
                    if (i == 3)
                    {
                        MAC_Txt3.Text = nic.GetPhysicalAddress().ToString();
                        MACDesc_3.Text = nic.Description;
                    }
                    i++;
                }
            }


            //DiskSN_Txt.Text = DiskSN("C").ToString();
            PCName_Txt.Text = System.Environment.MachineName;
        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            this.Text += $"  Version={versionInfo.FileVersion}";
        }

        private async void CheckForUpdates()
        {
            UpdateManager mgr;

            mgr = await UpdateManager.GitHubUpdateManager(@"https://github.com/klstradling/PhxPOC", prerelease: true);

            if (mgr != null)
                lblGitVer.Text = "Found";
            else 
                lblGitVer.Text = "Not Found";
            
            await mgr.UpdateApp();
        }

        private void txtToken_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process myProcess = new Process();

            // true is the default, but it is important not to set it to false
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = "https://display-parameters.com/?token=" + txtToken.Text;
            myProcess.Start();

            System.Windows.Forms.Application.Exit();

        }

        private void txtToken_TextChanged(object sender, EventArgs e)
        {
            if (txtToken.Text.Length == 8)
            {
                BtnOK.BackColor = Color.DeepSkyBlue;
                BtnOK.Enabled = true;
            }
            else
            {
                BtnOK.BackColor = Color.Gainsboro;
                BtnOK.Enabled = false;
            }
        }
    }
}