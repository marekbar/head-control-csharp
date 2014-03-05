using System;
using System.Drawing;

namespace HeadControlLibrary
{
    public class FrameProviderArgs : EventArgs
    {
        public Bitmap Frame{get;set;}

        public FrameProviderArgs(Bitmap bmp)
        {
            Frame = bmp;
        }
    }
}
