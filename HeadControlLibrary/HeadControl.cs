using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using Accord.Vision.Detection;
using System.IO;
using System.Drawing.Imaging;

namespace HeadControlLibrary
{
    public class HeadControl
    {
        #region DELEATES

        public delegate void HeadControlError(object sender, HeadControlErrorArgs args);
        /// <summary>
        /// Event delegate - how event method looks like
        /// </summary>
        /// <param name="sender">object - owner</param>
        /// <param name="e">HeadMoveEventArgs</param>
        public delegate void HeadMove(object sender, HeadMoveEventArgs e);

        /// <summary>
        /// Event delegate for directional head move event
        /// </summary>
        /// <param name="sender">object - sender</param>
        /// <param name="args">HeadMoveDirectionEventArgs - args</param>
        public delegate void HeadMoveDirection(object sender, HeadMoveDirectionEventArgs args);

        /// <summary>
        /// Event delegate for event OnFrameReceived
        /// </summary>
        /// <param name="sender">object - sender</param>
        /// <param name="args">FrameProviderArgs - args</param>
        public delegate void FrameProvider(object sender, FrameProviderArgs args);

        #endregion

        #region EVENTS
        /// <summary>
        /// Event of head move
        /// </summary>
        public event HeadMove OnHeadMove;

        /// <summary>
        /// Event for head move in left
        /// </summary>
        public event HeadMoveDirection OnHeadMoveLeft;

        /// <summary>
        /// Event for head move in right
        /// </summary>
        public event HeadMoveDirection OnHeadMoveRight;

        /// <summary>
        /// Event for head move down
        /// </summary>
        public event HeadMoveDirection OnHeadMoveDown;

        /// <summary>
        /// Event for head move up
        /// </summary>
        public event HeadMoveDirection OnHeadMoveUp;

        /// <summary>
        /// Event when next frame arrived
        /// </summary>
        public event FrameProvider OnFrameReceived;

        public event HeadControlError OnErrorOccured;

        #endregion

        #region EVENTS_UPDATER
        /// <summary>
        /// Head move internal update method
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <param name="position">Position</param>
        private void UpdateHeadMove(Direction direction, Position position)
        {
            // Make sure someone is listening to event
            if (OnHeadMove != null)
            {
                HeadMoveEventArgs args = new HeadMoveEventArgs(direction, position);
                OnHeadMove(this, args);
            }

            switch (direction)
            {
                case Direction.MoveDown:
                    {
                       if (OnHeadMoveDown != null) OnHeadMoveDown(this, new HeadMoveDirectionEventArgs(true));
                    } break;
                case Direction.MoveLeft:
                    {
                        if (OnHeadMoveLeft != null) OnHeadMoveLeft(this, new HeadMoveDirectionEventArgs(true));
                    } break;
                case Direction.MoveRight:
                    {
                        if (OnHeadMoveRight != null) OnHeadMoveRight(this, new HeadMoveDirectionEventArgs(true));
                    } break;
                case Direction.MoveUp:
                    {
                        if (OnHeadMoveUp != null) OnHeadMoveUp(this, new HeadMoveDirectionEventArgs(true));
                    } break;
            }

        }

        public void UpdateError(Exception ex)
        {
            if (OnErrorOccured != null)
            {
                OnErrorOccured(this, new HeadControlErrorArgs(ex));
            }
        }

        public void UpdateError(String error)
        {
            if (OnErrorOccured != null)
            {
                OnErrorOccured(this, new HeadControlErrorArgs(error));
            }
        }

        /// <summary>
        /// Pass to event listener frame image
        /// </summary>
        /// <param name="bmp">Bitmap - image</param>
        private void UpdateFrame(Bitmap bmp)
        {
            if (OnFrameReceived != null)
            {
                OnFrameReceived(this, new FrameProviderArgs(bmp));
            }
        }
        #endregion

        #region FIELDS

        private bool detecting = true;
        private bool tracking = false;
        private int detectEveryXFrames;
        private Accord.Vision.Detection.HaarObjectDetector detector = null;
        private Accord.Vision.Tracking.Camshift tracker = null;
        private Accord.Imaging.Filters.RectanglesMarker marker = new Accord.Imaging.Filters.RectanglesMarker(Color.Fuchsia);
        private MJPEGStream video = null;
        private Rectangle[] rect;
        private Bitmap bmp = null;
        private String cameraUrl;
        private String cameraLogin;
        private String cameraPassword;
        private Position current = new Position(0, 0);
        private Position previous = new Position(0, 0);
        private AForge.Imaging.Filters.ResizeBicubic resizeBic = new AForge.Imaging.Filters.ResizeBicubic(100, 100);
        #endregion

        #region CONSTRUCTORS
        private void init()
        {
            video = new MJPEGStream(this.cameraUrl);
            video.Login = this.cameraLogin;
            video.Password = this.cameraPassword;
            video.NewFrame += new NewFrameEventHandler(processFrame);
            video.VideoSourceError += new VideoSourceErrorEventHandler(processFrameError);

            detector = new HaarObjectDetector(HaarCascade.FromXml(new StringReader(HeadControlLibrary.Properties.Resources.haarcascade_frontalface_default)));
            detector.MinSize = new Size(20, 20);
            detector.ScalingFactor = 1.2f;
            detector.ScalingMode = ObjectDetectorScalingMode.SmallerToGreater;
            detector.SearchMode = ObjectDetectorSearchMode.Default;
            detector.UseParallelProcessing = true;
        }

        public HeadControl(String url)
        {
            this.cameraUrl = url;
            this.cameraLogin = "";
            this.cameraPassword = "";
            init();
        }

        public HeadControl(String url, String login, String password)
        {
            this.cameraUrl = url;
            this.cameraLogin = login;
            this.cameraPassword = password;
            init();
        }

        public bool Start()
        {
            try
            {
                detecting = true;
                tracking = false;
                if (video == null) return false;
                video.Start();
                tracker = new Accord.Vision.Tracking.Camshift();
                return true;
            }
            catch(Exception ex)
            {
                UpdateError(ex);
                return false;
            }
        }

        public bool Stop()
        {
            try
            {
                if (video == null) return false;
                video.SignalToStop();
                return true;
            }
            catch(Exception ex)
            {
                UpdateError(ex);
                return false;
            }
            finally
            {
                detecting = true;
                tracking = false;
            }
        }

        #endregion

        #region PROCESSING
        private void processFrame(object sender, NewFrameEventArgs args)
        {
            bmp = args.Frame.Clone() as Bitmap;
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            try
            {
                UpdateFrame(bmp);
                if (detecting)
                {
                    var ui = new AForge.Imaging.UnmanagedImage(data);
                    resizeBic.NewWidth = (int)ui.Width / 2;
                    resizeBic.NewHeight = (int)ui.Height / 2;


                    try
                    {
                        detector.ProcessFrame(resizeBic.Apply(ui));
                    }
                    catch(Exception ee)
                    {
                        throw new Exception("Problem z detektorem Viola-Jones", ee);
                    }
             
                    if (detector.DetectedObjects.Length > 0)
                    {
                        rect = detector.DetectedObjects;
                        tracker.SearchWindow = rect[0];
                        tracker.AspectRatio = 1.2f;
                        tracker.Mode = Accord.Vision.Tracking.CamshiftMode.HSL;
                        
                        marker.Rectangles = rect;
                        marker.ApplyInPlace(ui);

                        detecting = false;
                        tracking = true;
                        for (int i = 0; i < rect.Length; i++)
                        {
                            if (rect[i].Width > rect[0].Width) rect[0] = rect[i];
                        }
                        previous.X = current.X;
                        previous.Y = current.Y;
                        current.X = rect[0].X;
                        current.Y = rect[0].Y;
                    }//check number of detected objects

                }

                if (tracking)
                {
                    var ui = new AForge.Imaging.UnmanagedImage(data);
                    detectEveryXFrames++;
                    if (detectEveryXFrames > 300)
                    {
                        detectEveryXFrames = 0;
                        detecting = true;
                        tracking = false;
                    }
                    rect[0] = tracker.TrackingObject.Rectangle;

                    marker.Rectangles = rect;
                    if (rect.Length > 0)
                    marker.ApplyInPlace(ui);
                    previous.X = current.X;
                    previous.Y = current.Y;
                    current.X = tracker.TrackingObject.Rectangle.X;
                    current.Y = tracker.TrackingObject.Rectangle.Y;
                }

                Direction d = Direction.MoveDown;
                ///warning - in camera is mirrored image of your face 
                if (current.X > previous.X) d = Direction.MoveLeft;
                else if (current.X < previous.X) d = Direction.MoveRight;
                else if (current.Y < previous.Y) d = Direction.MoveDown;
                else if (current.Y > previous.Y) d = Direction.MoveUp;

                UpdateHeadMove(d, new Position(1, 1));
            }
            catch (Exception ex)
            {
                UpdateError(ex.GetError());
            }
            finally
            {
                bmp.UnlockBits(data);
            }
        }

        private void processFrameError(object sender, VideoSourceErrorEventArgs args)
        {
            UpdateError(args.Description);
            
        }
        #endregion
    }
}
