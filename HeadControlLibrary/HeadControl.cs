using System;
using System.Drawing;
using System.IO;
using Accord.Vision.Detection;
using AForge.Video;

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

        private bool detecting = false;
        private bool tracking = false;
        private float scaleX;
        private float scaleY;
        private static int processWidth = 160;
        private static int processHeight = 120;
        private Accord.Vision.Detection.HaarObjectDetector detector = null;
        private Accord.Vision.Tracking.Camshift tracker = null;
        private Accord.Imaging.Filters.RectanglesMarker marker = new Accord.Imaging.Filters.RectanglesMarker(Color.Fuchsia);
        private MJPEGStream video = null;
        private Bitmap bmp = null;
        private String cameraUrl;
        private String cameraLogin;
        private String cameraPassword;
        private Rectangle previous;
        private Rectangle current;

        public Rectangle Current
        {
            get { return current; }
            set 
            {
                previous = current;
                current = value;
            }
        }
        #endregion

        #region CONSTRUCTORS
        private void init()
        {
            video = new MJPEGStream(this.cameraUrl);
            video.Login = this.cameraLogin;
            video.Password = this.cameraPassword;
            video.NewFrame += new NewFrameEventHandler(processFrameQuickly);
            video.VideoSourceError += new VideoSourceErrorEventHandler(processFrameError);
            
            detector = new HaarObjectDetector(
                HaarCascade.FromXml(new StringReader(
                    HeadControlLibrary.Properties.Resources.haarcascade_frontalface_default)));
            detector.MinSize = new Size(10, 10);
            detector.ScalingFactor = 1.2f;
            detector.ScalingMode = ObjectDetectorScalingMode.SmallerToGreater;
            detector.SearchMode = ObjectDetectorSearchMode.Single;  
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
                detecting = false;
                tracking = false;
            }
        }

        #endregion

        #region PROCESSING
        private void processFrameQuickly(object sender, NewFrameEventArgs args)
        {
            if (!detecting && !tracking) { return; }

            bmp = args.Frame.Clone() as Bitmap;
            var data = bmp.GetDirectAccess();
            try
            {
                scaleX = bmp.Width / processWidth;
                scaleY = bmp.Height / processHeight;
                var im = data.GetUnmanaged();
                if (detecting)
                {
                    detecting = false;
                    tracking = false;

                    var downsample = im.ResizeTo(processWidth, processHeight);
                    var detections = detector.ProcessFrame(downsample);

                    if (detections.Length > 0)
                    {
                        tracker.Reset();
                        var face = detections.First();
                        var window = face.AvoidBackgroundTracking(scaleX, scaleY);
                        window.Inflate((int)(0.2f * face.Width * scaleX),(int)(0.4f * face.Height * scaleY));

                        tracker.SearchWindow = window;
                        tracker.ProcessFrame(im);

                        marker.Set(ref im, window);

                        tracking = true;
                        Current = window;
                    }
                    else
                    {
                        detecting = true;
                    }
                }
                else if (tracking)
                {
                    tracker.ProcessFrame(im);
                    im.DrawHorizontalAxis(ref tracker);
                    marker.Set(ref im, tracker.TrackingObject.Rectangle);
                    Current = tracker.TrackingObject.Rectangle;
                }
                else
                {
                    marker.ApplyInPlace(im);
                    previous = current;
                }
                
                UpdateHeadMove(GetDirection(), current.Current());
            }
            catch (Exception ex)
            {
                UpdateError(ex.GetError());
            }
            finally
            {
                bmp.UnlockBits(data);
                UpdateFrame(bmp);
            }
        }

        private Direction GetDirection()
        {
            Direction d = Direction.NoMove;

            ///warning - in camera is mirrored image of your face 
            if (current.X + 5 > previous.X) d = Direction.MoveLeft;
            else if (current.X + 5 < previous.X) d = Direction.MoveRight;

            else if (current.Height < previous.Height) d = Direction.MoveDown;
            else if (current.Height > previous.Height) d = Direction.MoveUp;
            //else if (current.Y < previous.Y) d = Direction.MoveDown;
            //else if (current.Y > previous.Y) d = Direction.MoveUp;
            else d = Direction.NoMove;

            return d;
        }

        private void processFrameError(object sender, VideoSourceErrorEventArgs args)
        {
            UpdateError(args.Description);
            
        }
        #endregion
    }
}
