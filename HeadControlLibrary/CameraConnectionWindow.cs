using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using System.Net;
using System.Threading.Tasks;
using AForge.Video;

namespace HeadControlLibrary
{
    public partial class CameraConnectionWindow : Form
    {
        private FilterInfoCollection usbCameras;
        public String Address { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }

        public CameraConnectionWindow()
        {
            InitializeComponent();
        }

        private void CameraConnectionWindow_Load(object sender, EventArgs e)
        {
            localCameras.Items.AddRange(getUSBCameraList());
            if (localCameras.Items.Count == 0)
            {
                localCameras.Items.Add("Nie wykryto kamer lokalnych");
                localCameras.SelectedIndex = 0;
                localCameras.Enabled = false;
                ipAddress.Text = "";
            }
            else
            {
                choices.Items.Add("Połączenie lokalne (USB lub wbudowana)");
            }
        }

        public object[] getUSBCameraList()
        {
            List<String> cams = new List<String>();
            try
            {
                usbCameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (usbCameras.Count > 0)
                    foreach (FilterInfo device in usbCameras)
                    {
                        cams.Add(device.Name);
                    }
            }
            catch {}
            return cams.ToArray();
        }

        public String getUSBAddressaAtIndex(int index)
        {
            try
            {
                return usbCameras[index].MonikerString;
            }
            catch (Exception)
            {
                return "Nie wykryto kamer lokalnych";
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            if (choices.SelectedIndex < 0)
            {
                status.Text = "Wybierz rodzaj połączenia";
                return;
            }
            bool flag = true;
            String error = "";
            try
            {
                switch (choices.SelectedIndex)
                {
                    case 0://mjpeg
                        {
                            MJPEGStream video = new MJPEGStream(Address);
                            video.Login = Login;
                            video.Password = Password;
                            video.Start();
                            if (video.IsRunning)
                            {
                                video.SignalToStop();
                            }
                            else
                            {
                                throw new Exception("Nie połączono z " + Address);
                            }
                        } break;
                    case 1://jpeg
                        {
                            JPEGStream video = new JPEGStream(Address);
                            video.Login = Login;
                            video.Password = Password;
                            video.FrameInterval = (int)6000 / 10;
                            video.Start();
                            if (video.IsRunning)
                            {
                                video.SignalToStop();
                            }
                            else
                            {
                                throw new Exception("Nie połączono z " + Address);
                            }
                           
                        } break;
                    case 2://local
                        {
                            VideoCaptureDevice video = new VideoCaptureDevice(Address);
                            video.Start();
                            if (video.IsRunning)
                            {
                                video.SignalToStop();
                            }
                            else
                            {
                                throw new Exception("Nie połączono z " + Address);
                            }

                        } break;
                }
            }
            catch(Exception ex)
            {
                error = ex.Message;
                flag = false;
            }
            finally
            {
                status.Text = flag ? "Połączono" : error;
            }
        }

        private void localCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            ipAddress.Text = getUSBAddressaAtIndex(localCameras.SelectedIndex);
        }

        private void ipAddress_TextChanged(object sender, EventArgs e)
        {
            Address = ((TextBox)sender).Text;
        }

        private void ipLogin_TextChanged(object sender, EventArgs e)
        {
            Login = ((TextBox)sender).Text;
        }

        private void ipPassword_TextChanged(object sender, EventArgs e)
        {
           Password = ((TextBox)sender).Text;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
