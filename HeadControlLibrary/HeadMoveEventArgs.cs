using System;

namespace HeadControl
{
    public class HeadMoveEventArgs : EventArgs
    {
        public Direction MoveDirection { get; set; }
        public Position CurrentPosition { get; set; }

        public HeadMoveEventArgs(Direction direction, Position position)
        {
            this.MoveDirection = direction;
            this.CurrentPosition = position;
        }
    }
}
