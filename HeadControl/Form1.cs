using System;
using System.Drawing;
using System.Windows.Forms;
using HeadControlLibrary;

namespace HeadControlSampleApp
{
    public partial class Form1 : Form
    {
        private HeadControlLibrary.HeadControl hc = null;
        delegate void SetTextCallback(string text);
        delegate void SetImageCallback(Bitmap bmp);
        delegate void SetCursorPositionCallback(Direction dir);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hc = new HeadControlLibrary.HeadControl(Properties.Settings.Default.CameraAddress, 
                                Properties.Settings.Default.CameraLogin,
                                Properties.Settings.Default.CameraPassword);
            hc.OnHeadMove += new HeadControlLibrary.HeadControl.HeadMove(HeadMoveEvent);
            hc.OnHeadMoveDown += new HeadControlLibrary.HeadControl.HeadMoveDirection(HeadMoveDownEvent);
            hc.OnHeadMoveUp += new HeadControlLibrary.HeadControl.HeadMoveDirection(HeadMoveUpEvent);
            hc.OnHeadMoveLeft += new HeadControlLibrary.HeadControl.HeadMoveDirection(HeadMoveLeftEvent);
            hc.OnHeadMoveRight += new HeadControlLibrary.HeadControl.HeadMoveDirection(HeadMoveRightEvent);
            hc.OnFrameReceived += (s, args) =>
            {
                setImage((Bitmap)args.Frame.Clone());
            };

            hc.OnErrorOccured += (s, err) =>
            {
                SetText(err.ErrorMessage);
            };
            hc.Start();
        }

        private void SetText(string text)
        {
            try
            {
                if (this.status.GetCurrentParent().InvokeRequired)
                {
                    this.status.GetCurrentParent().Invoke(new MethodInvoker(delegate { status.Text = text; }));
                }
                else
                {
                    status.Text = text;
                }
            }
            catch { }
        }

        private void setImage(Bitmap bmp)
        {
            try
            {
                if (this.image.InvokeRequired)
                {
                    SetImageCallback sic = new SetImageCallback(setImage);
                    this.Invoke(sic, new object[] { bmp });
                }
                else
                {
                    this.image.Image = bmp;
                }
            }
            catch { }
        }

        private static int sensivity = 5;
        private void MoveCursor(Direction dir)
        {
            if (this.InvokeRequired)
            {
                SetCursorPositionCallback sc = new SetCursorPositionCallback(MoveCursor);
                this.Invoke(sc, new object[] { dir });
            }
            else
            {
                this.Cursor = new Cursor(Cursor.Current.Handle);
                switch (dir)
                {
                    case Direction.MoveDown:
                        {
                            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + sensivity);
                        } break;
                    case Direction.MoveLeft:
                        {
                            Cursor.Position = new Point(Cursor.Position.X - sensivity, Cursor.Position.Y);
                        } break;
                    case Direction.MoveRight:
                        {
                            Cursor.Position = new Point(Cursor.Position.X + sensivity, Cursor.Position.Y);
                        } break;
                    case Direction.MoveUp:
                        {
                            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - sensivity);
                        } break;
                }

                Cursor.Clip = new Rectangle(this.Location, this.Size);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            hc.Stop();
        }

        private void HeadMoveEvent(object sender, HeadMoveEventArgs args)
        {
            MoveCursor(args.MoveDirection);
        }

        private void HeadMoveDownEvent(object sender, HeadMoveDirectionEventArgs args)
        {
            SetText("W dół");
        }

        private void HeadMoveUpEvent(object sender, HeadMoveDirectionEventArgs args)
        {
            SetText("W górę");
        }

        private void HeadMoveLeftEvent(object sender, HeadMoveDirectionEventArgs args)
        {
            SetText("W lewo");
        }

        private void HeadMoveRightEvent(object sender, HeadMoveDirectionEventArgs args)
        {
            SetText("W prawo");
        }
    }
}