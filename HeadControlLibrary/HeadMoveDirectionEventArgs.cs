using System;

namespace HeadControl
{
    public class HeadMoveDirectionEventArgs : EventArgs
    {
        public bool HasMoved { get; set; }

        public HeadMoveDirectionEventArgs(bool hasMoved)
        {
            HasMoved = hasMoved;
        }
    }
}
