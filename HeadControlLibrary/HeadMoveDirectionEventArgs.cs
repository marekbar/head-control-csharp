using System;

namespace HeadControlLibrary
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
