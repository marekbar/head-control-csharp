using System;

namespace HeadControlLibrary
{
    public class HeadMoveEventArgs : EventArgs
    {
        public HeadControlLibrary.Direction MoveDirection { get; set; }
        public Position CurrentPosition { get; set; }

        public HeadMoveEventArgs(HeadControlLibrary.Direction direction, Position position)
        {
            this.MoveDirection = direction;
            this.CurrentPosition = position;
        }
    }
}
